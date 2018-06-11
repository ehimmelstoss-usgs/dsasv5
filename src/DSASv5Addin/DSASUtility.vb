Option Explicit On
Option Strict On

Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.esriSystem
Imports System.Math
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.IO
'Imports Microsoft.Win32

Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Display

Module DSASUtility

    Private logLock As New Semaphore(1, 1)

    'set an array list of shorelines
    'JZ Amended this to use only selected shorelines if they are selected
    Public Function SortShorelinesByDate(ByVal featCursor As IFeatureCursor, ByVal dateIndex As Int32) As List(Of IFeature)
        SortShorelinesByDate = Nothing
        Dim shoreAList As New List(Of IFeature)
        Dim feat As IFeature
        Dim pTopoOp As ITopologicalOperator
        'select all features in the feature layer
        If featCursor Is Nothing Then Exit Function
        feat = featCursor.NextFeature
        If TypeOf feat.Shape Is IPolyline Then
            pTopoOp = CType(feat.Shape, ITopologicalOperator)
            If Not pTopoOp.IsSimple Then
                pTopoOp.Simplify()
            End If
        End If
        While feat IsNot Nothing
            If Not TypeOf feat.Value(dateIndex) Is DBNull Then
                If Trim(CStr(feat.Value(dateIndex))).Length > 0 Then
                    shoreAList.Add(feat)
                End If
            End If
            feat = featCursor.NextFeature
        End While
        'sort the features
        BubbleSort(shoreAList, dateIndex)
        Return shoreAList
    End Function
    'bubble sort and input array list
    '
    Public Sub BubbleSort(ByRef sortedList As List(Of IFeature), ByVal fldIndex As Int32)
        Dim i As Int32, j As Int32
        Dim tempFeat As IFeature

        For i = sortedList.Count - 1 To 0 Step -1
            For j = 1 To i
                If CDate((sortedList(j - 1).Value(fldIndex))) < CDate((sortedList(j).Value(fldIndex))) Then
                    tempFeat = sortedList(j - 1)
                    sortedList(j - 1) = sortedList(j)
                    sortedList(j) = tempFeat
                End If
            Next
        Next
    End Sub

    '
    'return the current date and time
    '
    Public Function CreateTimestamp() As String
        Return Date.Now.ToString("yyyy/MM/dd HH:mm:ss")
    End Function

    '
    'calculate azimuth
    '
    Public Function CalculateAzimuth(ByVal inPolyline As IPolyline) As Double
        Try
            Dim azimVector As IVector3D = DirectCast(New Vector3D, IVector3D)
            azimVector.ConstructDifference(inPolyline.ToPoint, inPolyline.FromPoint)
            Return azimVector.Azimuth / Atan(1) * 45
        Catch ex As Exception
            Dim a = 1
        End Try
    End Function


    Public Function clonePoint(original As IPoint) As IPoint
        clonePoint = CType(CType(original, IClone).Clone(), IPoint)
    End Function

    ''' <summary>
    ''' Find the "desired" intersection point of shore and transect
    ''' as determined by user setting of landward/seaward.
    ''' Distance from landward point of the transect is also computed into "hitDist"
    ''' </summary>
    ''' <param name="shoreFeat"></param>
    ''' <param name="xectGeom"></param>
    ''' <param name="hitDist"></param>
    ''' <returns></returns>
    Public Function findDesiredShoreIntersect(shoreFeat As IFeature, xectGeom As IPolyline, Optional ByRef hitDist As Double = Nothing) As IPoint
        Dim topoOp As ITopologicalOperator = DirectCast(shoreFeat.Shape, ITopologicalOperator)
        'We need to do the intersect operation on shore geometry so that its M-awareness (if so) is preserved in the resulting 
        'intersection points.
        Dim intersections As IPointCollection = DirectCast(topoOp.Intersect(xectGeom, esriGeometryDimension.esriGeometry0Dimension), IPointCollection)

        If intersections.PointCount = 0 Then
            findDesiredShoreIntersect = Nothing
        Else
            Dim ht As IHitTest = DirectCast(intersections, IHitTest)
            findDesiredShoreIntersect = New PointClass
            If DSAS.seaward Then
                ht.HitTest(xectGeom.FromPoint, xectGeom.Length * 1.01, esriGeometryHitPartType.esriGeometryPartVertex, findDesiredShoreIntersect, hitDist, Nothing, Nothing, Nothing)
            Else
                ht.HitTest(xectGeom.ToPoint, xectGeom.Length * 1.01, esriGeometryHitPartType.esriGeometryPartVertex, findDesiredShoreIntersect, hitDist, Nothing, Nothing, Nothing)
            End If
        End If
    End Function


    Sub clipToShorelineEnvelope(ByRef xect As IPolyline, xectMidPt As IPoint)
        If DSAS.shoreFc Is Nothing Then
            log("Error: Casting without shoreline featureclass")
            Exit Sub
        End If
        Dim shoreCache As New ESRI.ArcGIS.Carto.FeatureCacheClass
        shoreCache.Initialize(xectMidPt, xect.Length * 1.01)  'Allow for 1% error
        shoreCache.AddFeatures(DSAS.shoreFc)

        ' Add each shore intersection
        Dim distToDesiredDirection As Double
        Dim desiredIntersects As New Dictionary(Of Date, IPoint)
        Dim desiredIntersectDistances As New Dictionary(Of Date, Double)
        For i As Integer = 0 To shoreCache.Count - 1
            Dim shoreFeat As IFeature = shoreCache.Feature(i)
            Dim isect As IPoint = findDesiredShoreIntersect(shoreFeat, xect, distToDesiredDirection)
            If isect IsNot Nothing Then
                Dim shoreDate As Date = CDate(shoreFeat.Value(GeoDB.shoreFldMap("DATE")))
                If Not desiredIntersectDistances.ContainsKey(shoreDate) OrElse distToDesiredDirection < desiredIntersectDistances(shoreDate) Then
                    desiredIntersectDistances(shoreDate) = distToDesiredDirection
                    desiredIntersects(shoreDate) = isect
                End If
            End If
        Next

        Dim isects As IPointCollection = New PolylineClass
        For Each isect In desiredIntersects.Values()
            isects.AddPoint(isect)
        Next
        ' Add transect mid point which rests on the baseline 
        isects.AddPoint(xectMidPt)

        If isects.PointCount < 2 Then
            xect = Nothing
        Else
            Dim iht As IHitTest
            iht = DirectCast(isects, IHitTest)
            Dim fromPoint As IPoint = New PointClass
            iht.HitTest(xect.FromPoint, xect.Length, esriGeometryHitPartType.esriGeometryPartVertex, fromPoint, Nothing, Nothing, Nothing, Nothing)
            xect.FromPoint = fromPoint
            Dim toPoint As IPoint = New PointClass
            iht.HitTest(xect.ToPoint, xect.Length, esriGeometryHitPartType.esriGeometryPartVertex, toPoint, Nothing, Nothing, Nothing, Nothing)
            xect.ToPoint = toPoint
        End If
    End Sub

    ''
    'get a normal line
    '
    Public Function CreateNormalLine(
       bl As IPolyline,
       dAlong As Double,
       searchDistance As Double
      ) As IPolyline
        If bl.Length < dAlong Then Return Nothing

        Dim pNormal As IPolyline

        If My.Settings.Transect_Leg_Length = 0 Then
            pNormal = CreateNormalLineSimple(bl, dAlong, searchDistance)
        Else
            pNormal = CreateNormalLineSmooth(bl, dAlong, searchDistance, My.Settings.Transect_Leg_Length)
        End If

        Dim midPt As IPoint = clonePoint(pNormal.FromPoint)

        mirrorTransect(pNormal)

        ''AE: Do we really need this?
        'Dim topoOperator As ITopologicalOperator
        'Dim polyLn As IPolyline
        'Dim pSegColl As ISegmentCollection
        'pSegColl = DirectCast(New Polyline, ISegmentCollection)
        'pSegColl.AddSegment(CType(pNormal, ISegment))
        ''make sure the line generated is topologically correct
        'polyLn = CType(pSegColl, IPolyline)
        'topoOperator = CType(polyLn, ITopologicalOperator)
        'topoOperator.Simplify()

        'clipToShorelineEnvelope(polyLn, bl, midPt)

        'Return polyLn

        clipToShorelineEnvelope(pNormal, midPt)
        Return pNormal

    End Function

    ' get a perpendicular line to the input polyline
    Public Function CreateNormalLineSimple(
     ByVal pPolyline As IPolyline,
     ByVal dAlong As Double,
     ByVal pLength As Double
     ) As IPolyline
        Dim pNormal As ILine = New LineClass
        pPolyline.QueryNormal(esriSegmentExtension.esriExtendAtFrom, dAlong, False, pLength, pNormal)
        CreateNormalLineSimple = New PolylineClass
        CreateNormalLineSimple.FromPoint = pNormal.FromPoint
        CreateNormalLineSimple.ToPoint = pNormal.ToPoint
    End Function

    '
    'get a two-legged normal line
    '
    Public Function CreateNormalLineSmooth(
      ByVal pPolyline As IPolyline,
      ByVal dAlong As Double,
      ByVal pLength As Double,
      ByVal dTwoLeg As Double
      ) As IPolyline

        Dim pNormal As IPolyline
        Dim pt As IPoint
        Dim ptLeft As IPoint
        Dim ptRight As IPoint
        Dim ptConstruct As IConstructPoint
        Dim ptNormal As IPoint
        Dim dLegAlong As Double
        Dim baseSegment As ISegment
        Dim pSegColl As ISegmentCollection

        pt = New Point
        ptLeft = New Point
        ptRight = New Point
        ptConstruct = DirectCast(New Point, IConstructPoint)
        ptNormal = New Point
        baseSegment = DirectCast(New Line, ISegment)
        pNormal = New PolylineClass
        pSegColl = DirectCast(New Polyline, ISegmentCollection)
        pPolyline.QueryPoint(esriSegmentExtension.esriNoExtension, dAlong, False, pt)
        dLegAlong = dAlong - (dTwoLeg / 2)
        pPolyline.QueryPoint(esriSegmentExtension.esriNoExtension, dLegAlong, False, ptLeft)
        dLegAlong = dAlong + (dTwoLeg / 2)
        pPolyline.QueryPoint(esriSegmentExtension.esriNoExtension, dLegAlong, False, ptRight)
        log(String.Format("dAlong: {0}   dLegAlong: {1}   dLegAlong: {2}   ", dAlong, dLegAlong - dTwoLeg / 2, dLegAlong + dTwoLeg / 2))
        baseSegment.FromPoint = ptLeft
        baseSegment.ToPoint = ptRight
        'baseSegment.SpatialReference = pPolyline.SpatialReference
        'addGraphic(cloneGeom(baseSegment))
        ptConstruct.ConstructPerpendicular(baseSegment, esriSegmentExtension.esriNoExtension, pt, pLength, True)
        ptNormal = CType(ptConstruct, IPoint)
        pNormal.FromPoint = pt
        pNormal.ToPoint = ptNormal
        If DSAS.createRecordOfCastSmoothing Then record(ptLeft, pt, ptRight)
        Return pNormal
    End Function


    Sub record(ptLeft As IPoint, ptBase As IPoint, ptRight As IPoint)
        If DSAS.record Is Nothing Then Return

        Dim ipc As IPointCollection = New PolylineClass
        ipc.AddPoint(CType(cloneGeom(ptLeft), IPoint))
        ipc.AddPoint(CType(cloneGeom(ptBase), IPoint))
        ipc.AddPoint(CType(cloneGeom(ptRight), IPoint))
        ipc.AddPoint(CType(cloneGeom(ptLeft), IPoint))
        Dim feat As IFeature = DSAS.record.CreateFeature()
        feat.Shape = CType(ipc, IGeometry)
        feat.Store()
    End Sub


    Public Function cloneGeom(original As IGeometry) As IGeometry
        cloneGeom = CType(CType(original, IClone).Clone(), IGeometry)
    End Function


    Private Sub addGraphic(outgeom As IGeometry, Optional buffer_size As Double = 100, Optional r As Integer = 255, Optional g As Integer = 0, Optional b As Integer = 0)
        Dim bcp As IBufferConstructionProperties = New BufferConstructionClass()
        Dim bc As IBufferConstruction = TryCast(bcp, BufferConstruction)
        Dim resultGeo As IGeometry = outgeom    ' bc.Buffer(outgeom, 2)

        Dim element As IElement = Nothing

        Dim rgbColor As IRgbColor = New RgbColorClass()
        rgbColor.RGB = RGB(222, 0, 0)

        'Create cartographic line symbol  
        Dim cartographicLineSymbol As ICartographicLineSymbol = New CartographicLineSymbolClass()
        cartographicLineSymbol.Color = rgbColor
        cartographicLineSymbol.Width = 2

        'Define line element  
        Dim lineElement As ILineElement = New LineElementClass()
        lineElement.Symbol = DirectCast(CartographicLineSymbol, ILineSymbol)

        If resultGeo IsNot Nothing Then
            element = DirectCast(lineElement, ESRI.ArcGIS.Carto.IElement)
            ' Explicit Cast
            element.Geometry = DirectCast(resultGeo, ILine)
            Dim graphicsContainer As IGraphicsContainer = DirectCast(My.Document.FocusMap, ESRI.ArcGIS.Carto.IGraphicsContainer)
            'graphicsContainer.DeleteAllElements()
            graphicsContainer.AddElement(element, 0)
            My.Document.ActiveView.Refresh()
        End If

        'Add a dot to the tail of the geometry
        If outgeom.GeometryType <> esriGeometryType.esriGeometryPoint AndAlso Not outgeom.IsEmpty Then
            addGraphic(DirectCast(outgeom, IPolyline).ToPoint, buffer_size * 2 / 3, 255, 0, 100)
        End If
    End Sub


    Public Sub mirrorTransect(xect As IPolyline)
        Dim xectPoints As IPointCollection = DirectCast(xect, IPointCollection)
        Dim ptFrom As IPoint = xectPoints.Point(0)
        Dim ptTo As IPoint = xectPoints.Point(1)
        Dim ptFromNew As IPoint = clonePoint(ptFrom)
        ptFromNew.X -= ptTo.X - ptFrom.X
        ptFromNew.Y -= ptTo.Y - ptFrom.Y

        Dim xectPointsNew As IPointCollection = New PolylineClass
        xectPointsNew.AddPoint(ptFromNew)
        xectPointsNew.AddPoint(ptFrom)
        xectPointsNew.AddPoint(ptTo)
        xectPoints.SetPointCollection(xectPointsNew)
        ' Before the call to mirror, transect is one side only and always on the right side
        ' The other side has now been added but we need to reverse the transect if land on left side
        ' to maintain sea-to-shore transect orientation.
        If Not My.Settings.Land_On_Right_Side Then
            Dim ic As ICurve = DirectCast(xectPoints, ICurve)
            ic.ReverseOrientation()
        End If
    End Sub


    'Function to sort the baselines.
    Public Function SortBaselineFeatures(ByVal inBaseFeature As IFeatureCursor) As List(Of IFeature)
        'First load all baselines with id>0 into a sorted data structure
        Dim sorted As New SortedDictionary(Of Integer, IFeature)
        Dim idFldNumber As Integer = GeoDB.baseFldMap("ID")
        Do
            Dim feat As IFeature = inBaseFeature.NextFeature
            If feat Is Nothing Then Exit Do
            If feat.Value(idFldNumber) IsNot DBNull.Value AndAlso CInt(feat.Value(idFldNumber)) > 0 Then
                Dim key As Integer = CInt(feat.Value(idFldNumber))
                If sorted.ContainsKey(key) Then
                    log(TraceLevel.Error, "Baseline ID: " & key.ToString & " is repeated. Cannot proceed!")
                    Return Nothing
                Else
                    sorted.Add(key, feat)
                End If
            End If
        Loop
        'Then return ArrayList containing baseline features sorted by id.
        Dim shrBaseArray As New List(Of IFeature)
        For Each feat As IFeature In sorted.Values
            shrBaseArray.Add(feat)
        Next
        Return shrBaseArray
    End Function

    Public Function getSpaRef(ByRef featCls As IFeatureClass) As String
        If featCls Is Nothing Then Return "Unknown"
        Dim spatialRef As ISpatialReference
        spatialRef = featCls.Fields.Field(featCls.FindField(featCls.ShapeFieldName)).GeometryDef.SpatialReference
        Return spatialRef.Name
    End Function

    'Public Shared Function CheckTempDirectory() As Boolean
    '    Dim blnExists As Boolean
    '    blnExists = System.IO.Directory.Exists(DSAS.Instance.logDir)
    '    Return blnExists
    'End Function

    '
    'add timestamp
    '
    Public Function withDate(ByVal inStr As String) As String
        Return String.Format("[{0}] : {1}", Date.Now, inStr)
    End Function

    Public Sub log(ByVal level As Integer, ByVal msg As String, Optional ByVal internalError As Boolean = False)
        logLock.WaitOne()
        Try
            Trace.WriteLineIf(My.Settings.Trace_Level >= level, withDate(msg))
            If level = TraceLevel.Error AndAlso Not DSAS.suppressErrorDialogs Then
                If internalError Then
                    msg = "An unexpected error occurred. Please refer to the" + vbCrLf + "log file (" + DSAS.logPath + ") for additional details after closing ArcMap."
                End If
                msg += vbCrLf + vbCrLf + "[Press Ctrl+C to copy this message to the clipboard]"
                MsgBox(msg,, DSAS.MsgBoxTitleError)
                Trace.Flush()
            End If
        Finally
            logLock.Release()
        End Try
    End Sub

    Public Sub log(ByVal msg As String)
        log(TraceLevel.Verbose, msg)
    End Sub

    Public Sub showLogLocation()
        Try
            log("Show log file")
            Trace.Flush()
            Dim showPath As String = DSAS.logPath
            Dim cmd As String = ""
            If IO.File.Exists(DSAS.logPath) Then
                cmd = "/select, "
            Else
                ' Strip filename and just show the folder
                showPath = showPath.Substring(0, showPath.Length - DSAS.logFilename.Length)
            End If
            Call Shell("explorer.exe " & cmd & """" & showPath & """", AppWinStyle.NormalFocus)
        Catch ex As Exception
        End Try
    End Sub

    '
    'handle argument exception
    '
    Public Sub handleArgException(ByVal ex As ArgumentException)
        handleExceptionInner("Argument Exception", ex)
    End Sub

    '
    'handle com exception
    '
    Public Sub handleComException(ByVal ex As COMException)
        handleExceptionInner("COM Exception", ex)
    End Sub

    '
    'handle exception
    '
    Public Sub handleException(ByVal ex As Exception)
        handleExceptionInner("System Exception", ex)
    End Sub

    Private Sub handleExceptionInner(ByVal typ As String, ByVal ex As Exception)
        Dim msgStr As String = typ & vbCrLf & ex.ToString
        logIntro()
        log(TraceLevel.Info, "********EXCEPTION INFO BEGIN*********")
        log(TraceLevel.Info, typ)
        log(TraceLevel.Info, "Exception Source:")
        log(TraceLevel.Info, ex.Source)
        log(TraceLevel.Info, "Exception Target Site:")
        log(TraceLevel.Info, ex.TargetSite.ToString)
        log(TraceLevel.Info, "Exception Message:")
        log(TraceLevel.Info, ex.Message)
        Dim exclusiveLockIssue As Boolean = (ex.HResult = -2147220970)
        If exclusiveLockIssue Then
            log(TraceLevel.Info, msgStr)
            log(TraceLevel.Error, "Unable to lock database. Please make sure that only one instance of all " + vbCrLf + "ArcGIS tools is open (e.g., ArcMap, ArcCatalog, ArcToolbox)" + vbCrLf + "This message may repeat.")
        Else
            log(TraceLevel.Error, msgStr, True)
        End If
        log(TraceLevel.Info, "********EXCEPTION INFO END*********")
    End Sub


    Public Sub logIntro()

        log(TraceLevel.Info, "DSAS version: " + DSASAddin.My.ThisAddIn.Version)
        log(TraceLevel.Info, "DSAS build timestamp: " + getDsasBuildTimestamp() + " UTC")
        log(TraceLevel.Info, "ArcGIS version: " + ESRI.ArcGIS.RuntimeManager.ActiveRuntime.Version)
        log(TraceLevel.Info, "ArcGIS installed in: " + ESRI.ArcGIS.RuntimeManager.ActiveRuntime.Path)
        log(TraceLevel.Info, "")
    End Sub


    Public Function ReleaseComObject(ByVal o As Object) As Integer
        If o IsNot Nothing Then
            Return Marshal.ReleaseComObject(o)
        Else
            Return 0
        End If
    End Function

    '
    'trace values
    '
    Public Sub TraceSettings()
        Dim lookupTable As New Dictionary(Of String, String) From {
        {"Baseline_Feature_Layer", "Baseline Feature Layer"},
        {"Baseline_Group_Field", "Baseline Group Field"},
        {"Confidence_Interval", "Confidence Interval"},
        {"ifApplyColorRamp", "Color Ramp Applied?"},
        {"Intersect_Threshold_Enforced", "Intersect Threshold Enforced?"},
        {"Intersect_Threshold_Value", "Intersect Threshold Value"},
        {"Land_On_Right_Side", "Land is on Right Side?"},
        {"Search_Distance", "Maximum Search Distance From Baseline"},
        {"search_distance_field", "Baseline Search Distance Field"},
        {"Seaward", "Use Seaward Intersection?"},
        {"Shoreline_Date_Field", "Shoreline Date Field"},
        {"Shoreline_Feature_Layer", "Shoreline Feature Layer"},
        {"Shoreline_Type_Field", "Shoreline Type Field"},
        {"Shoreline_Uncertainty_Field", "Shoreline Uncertainty Field"},
        {"Shoreline_Uncertainty_Table", "Shoreline Uncertainty Table"},
        {"Spacing", "Transect Spacing"},
        {"Transect_Leg_Length", "Smoothing Distance"},
        {"TransectLayerSelected", "Transect Layer Selected"},
        {"Uncertainty", "Default Data Uncertainty"}
        }

        DSASUtility.log(TraceLevel.Info, "Settings are:")
        For Each prop As Configuration.SettingsProperty In My.Settings.Properties
            Try
                If lookupTable.ContainsKey(prop.Name) Then
                    DSASUtility.log(TraceLevel.Info, String.Format("{0}:{1}{2}", lookupTable(prop.Name).PadRight(40, "."c), " ", My.Settings(prop.Name)))
                Else
                    ' Useful for debugging
                    ' DSASUtility.log(TraceLevel.Info, String.Format("CAN'T FIND:{0}{1}", vbTab, prop.Name))
                End If
            Catch ex As Exception
                log(ex.Message)
            End Try
        Next

        DSASUtility.log(TraceLevel.Info, String.Format("{0}:{1}{2}", "Transect Layer Selected".PadRight(40, "."c), " ", nv(TransectLyrToolCtl.currentTransectLayerName)))
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="val"></param>
    ''' <param name="defaultVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function nv(val As Object, defaultVal As Object) As Object
        If val Is Nothing OrElse TypeOf val Is DBNull OrElse val.ToString.Trim = "" Then
            Return defaultVal
        Else
            Return val
        End If
    End Function

    Public Function nv(val As String, Optional defaultVal As String = "") As String
        If val Is Nothing OrElse val.ToString.Trim = "" Then
            Return defaultVal
        Else
            Return val
        End If
    End Function

    Public Function DecimalYear(ByVal inDate As Date) As Double
        Return (inDate.Year + (inDate.DayOfYear() + (inDate.Hour + (inDate.Minute + (inDate.Second / 60.0)) / 60.0) / 24.0) / 365.242199)
    End Function


    Public Function IsAlphaNumeric(ByVal str As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(str, "^[\w]+$")
    End Function




    ''' <summary>
    ''' Returns the folder path where application is installed in.
    ''' </summary>
    ''' <returns></returns> 
    ''' <remarks></remarks>
    Public Function getAppFolder() As String
        'Beep()
        Return System.IO.Path.GetDirectoryName([Assembly].GetCallingAssembly().Location)
    End Function

    ''' <summary>
    ''' Returns the folder path where the template files are stored in.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Template files are copied to application data folder at start up if necessary.</remarks>
    Public Function getTemplateFolder() As String
        Return getAppFolder() + "\template"
    End Function

    ''' <summary>
    ''' Returns the folder path where application data is stored.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getAppDataFolder() As String
        Return System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\" + My.Company + "\" + My.Name
    End Function

    ''' <summary>
    ''' Returns the folder path where files supporting metadata operations are stored.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getMetadataFolder() As String
        Return getAppDataFolder() + "\metadata\"
    End Function


    ''' <summary>
    ''' Copy source directory with given path to given target directory path.
    ''' </summary>
    ''' <param name="sourceDir">Full path to source directory</param>
    ''' <param name="targetDir">Full path to target directory</param>
    ''' <param name="searchOption">Whether only top level files are included from source directory or all files and directories under it. Defaults to all.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function copyDir(ByVal sourceDir As String, ByVal targetDir As String, Optional ByVal searchOption As IO.SearchOption = SearchOption.AllDirectories) As Boolean
        Try
            For Each f As String In System.IO.Directory.GetFiles(sourceDir, "*", searchOption)
                Dim target As String = targetDir + f.Replace(sourceDir, "")
                If Not System.IO.File.Exists(target) Then My.Computer.FileSystem.CopyFile(f, target)
            Next
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' Copy the contents of the template folder to the user's AppData system/special folder.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>We need this because each user has to work in folders reserved for them due to increasing security/privacy restrictions in Windows operating systems.</remarks>
    Public Function checkAndCopyTemplate() As Boolean
        Try
            'MsgBox("Template folder is: " + getTemplateFolder())
            'MsgBox("AppData folder is: " + getAppDataFolder())
            Return copyDir(getTemplateFolder(), getAppDataFolder())
        Catch ex As Exception
            Return False
        End Try
    End Function


    Sub ErrorHandler(ex As Exception)
        handleException(ex)
    End Sub

    ''' <summary>
    ''' Used for instantiating ArcGIS singleton objects 
    ''' per https://desktop.arcgis.com/en/arcobjects/latest/net/webframe.htm#d91e445e-47c5-41ea-94ca-45f945b73c0f.htm
    ''' and https://blogs.esri.com/esri/arcgis/2008/08/18/what-every-arcobjects-programmer-should-know-about-singletons/
    ''' </summary>
    ''' <param name="progID"></param>
    ''' <returns></returns>
    Public Function getSingleton(progID As String) As Object
        Dim t As Type = Type.GetTypeFromProgID(progID)
        Return Activator.CreateInstance(t)
    End Function



    Public Function makeDataFilename(ByVal xectLayerName As String) As String
        Return getAppDataFolder() & "\data\" & Format(Date.Now, "yyyyMMdd_HHmmss") & "." & xectLayerName
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dataFilename"></param>
    ''' <returns></returns>
    ''' <remarks>C:\x\y\z\20080924_152523.AE01.in.xml -> AE01_20080924_152523</remarks>
    Public Function makeOutputTablename(ByVal dataFilename As String) As String
        Dim parts As String()
        parts = dataFilename.Split("\"c)
        parts = parts(parts.Length - 1).Split("."c)
        Return parts(1) & "_rates_" & parts(0)
    End Function


    ''' <summary>
    ''' Returns the input value rounded if the value is numeric
    ''' </summary>
    ''' <param name="o"></param>
    ''' <returns></returns>
    Public Function round(o As Object) As Object
        If o Is Nothing OrElse o Is DBNull.Value Then
            Return Nothing
        Else
            If o.ToString().Contains("."c) Then
                Return Math.Round(Convert.ToDouble(o), DSAS.roundDigits)
            Else
                Return o
            End If
        End If
    End Function


    Public Function NothingIfMinusOne(fld As String, val As Double) As Double?
        If val = -1 Then
            log("NothingIfMinusOne: " + fld)
            Return Nothing
        Else
            Return val
        End If
    End Function


    Public Function checkForStat(fldname As String, desiredStatVariant As String) As Boolean
        Return fldname = desiredStatVariant OrElse fldname = "NB_" & desiredStatVariant
    End Function


    Public Sub safeCloseConnection(cn As OleDb.OleDbConnection)
        Try
            If cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed Then cn.Close()
            cn.Dispose()
        Catch ex As Exception
        End Try
    End Sub


    ''' <summary>
    ''' Determine given DLL's build time.
    ''' Attributed to Dustin Aleksiuk as cited at: https://blog.codinghorror.com/determining-build-date-the-hard-way/
    ''' </summary>
    ''' <param name="filePath"></param>
    ''' <returns></returns>
    Function RetrieveLinkerTimestamp(ByVal filePath As String) As DateTime
        Const PeHeaderOffset As Integer = 60
        Const LinkerTimestampOffset As Integer = 8
        Dim b(2047) As Byte
        Dim s As Stream = Nothing
        Try
            s = New IO.FileStream(filePath, FileMode.Open, FileAccess.Read)
            s.Read(b, 0, 2048)
        Finally
            If Not s Is Nothing Then s.Close()
        End Try
        Dim i As Integer = BitConverter.ToInt32(b, PeHeaderOffset)
        Dim SecondsSince1970 As Integer = BitConverter.ToInt32(b, i + LinkerTimestampOffset)
        Dim dt As New DateTime(1970, 1, 1, 0, 0, 0)
        dt = dt.AddSeconds(SecondsSince1970)
        'dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours)
        Return dt
    End Function


    Function getDsasBuildTimestamp() As String
        Return RetrieveLinkerTimestamp([Assembly].GetCallingAssembly().Location).ToString("o")
    End Function


    ''' <summary>
    ''' Given a transect geometry, returns the point on the transect that sits on the baseline on which it was cast
    ''' </summary>
    ''' <param name="xectPoints"></param>
    ''' <returns></returns>
    Function getXectBaselinePoint(xectPoints As IPointCollection) As IPoint
        Dim xectBaselinePoint As IPoint
        If xectPoints.PointCount = 3 Then
            ' v5 transect's midpoint is anchored on the baseline
            xectBaselinePoint = xectPoints.Point(1)
        ElseIf xectPoints.PointCount = 2 Then
            ' v4 or earlier legacy transect's starting point is anchored on the baseline
            xectBaselinePoint = xectPoints.Point(0)
        Else
            ' We should never get here unless bad transect geometry
            xectBaselinePoint = Nothing
        End If
        Return xectBaselinePoint
    End Function


    Function isValidTransectName(name As String, Optional popWarning As Boolean = False) As Boolean
        isValidTransectName = DSASUtility.IsAlphaNumeric(name) AndAlso name.Length > 0 AndAlso name.Length <= 19
        If Not isValidTransectName AndAlso popWarning Then log(TraceLevel.Error, "Transect name must be an alphanumeric string upto 19 characters!")
    End Function


    ''' <summary>
    ''' Get baseline search distance from a baseline layer attribute
    ''' in a rock solid fashion or else default to the setting for this.
    ''' </summary>
    ''' <param name="baseline"></param>
    ''' <returns></returns>
    Public Function getSearchDistance(baseline As IFeature) As Double
        Try
            If Not String.IsNullOrEmpty(My.Settings.search_distance_field) Then
                Dim search_distance_field_index As Integer = baseline.Fields.FindField(My.Settings.search_distance_field)
                If search_distance_field_index > -1 Then
                    Dim search_distance_from_baseline = baseline.Value(search_distance_field_index)
                    If search_distance_from_baseline IsNot DBNull.Value Then
                        Return CDbl(search_distance_from_baseline)
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
        Return My.Settings.Search_Distance
    End Function

    ''' <summary>
    ''' Convert Nothing values to database nulls
    ''' </summary>
    ''' <param name="val"></param>
    ''' <returns></returns>
    Public Function nothingToNull(val As Object) As Object
        If val Is Nothing Then
            Return DBNull.Value
        Else
            Return val
        End If
    End Function

End Module
