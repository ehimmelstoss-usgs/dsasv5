Option Explicit On

Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.esriSystem
Imports System.Data.OleDb
Imports ESRI.ArcGIS.Display

Public Class MapUtility
    '
    'find a table in the map based on supplied map name
    '
    Public Shared Function FindTableInMap(ByVal tableName As String) As ITable
        ' TODO: check name in map vs source?
        Dim stTabColl As IStandaloneTableCollection = DirectCast(My.Document.FocusMap, IStandaloneTableCollection)

        For i As Integer = 0 To stTabColl.StandaloneTableCount - 1
            Dim ist As IStandaloneTable = stTabColl.StandaloneTable(i)
            If tableName.ToLower = ist.Name.ToLower Then
                If ist.Valid Then
                    If GeoDB.isInPersonalGeodatabaseWorkspace(ist) Then
                        Return ist.Table
                    End If
                End If
                Exit For
            End If
        Next

        Return Nothing
    End Function


    ''' <summary>
    ''' Return the feature layer with the given name
    ''' </summary>
    ''' <param name="lyrName"></param>
    ''' <returns></returns>
    Shared Function findFeatureLayer(ByVal lyrName As String, Optional ByVal layerGenericName As String = Nothing) As IFeatureLayer
        If lyrName Is Nothing Then Return Nothing

        For Each lyr As IFeatureLayer In featureLayers()
            If lyr.Name.ToUpper = lyrName.ToUpper Then
                If layerGenericName Is Nothing OrElse GeoDB.layerIsValid(lyr, layerGenericName, True) Then
                    Return lyr
                End If
            End If
        Next

        Return Nothing
    End Function


    Shared Iterator Function tablesInMap() As IEnumerable(Of IStandaloneTable)
        For m As Integer = 0 To My.Document.Maps.Count - 1
            Dim map As IMap = My.Document.Maps.Item(m)
            Dim stTabColl As IStandaloneTableCollection = DirectCast(map, IStandaloneTableCollection)
            For i As Integer = 0 To stTabColl.StandaloneTableCount - 1
                Dim ist As IStandaloneTable = stTabColl.StandaloneTable(i)
                If ist.Valid Then
                    If GeoDB.isInPersonalGeodatabaseWorkspace(ist) Then
                        Yield ist
                    End If
                End If
            Next
        Next
    End Function

    ''' <summary>
    ''' Iterator for uncertainty tables found in the map
    ''' </summary>
    ''' <returns></returns>
    Shared Iterator Function uncertaintyTables() As IEnumerable(Of IStandaloneTable)
        For Each tbl As IStandaloneTable In tablesInMap()
            If tbl.Table.FindField("ID") > -1 AndAlso tbl.Table.FindField("BIAS") > -1 AndAlso tbl.Table.FindField("UNCY") > -1 AndAlso tbl.Table.FindField("UNCYB") > -1 Then
                Yield tbl
            End If
        Next
    End Function


    ''' <summary>
    ''' Compute the number of uncertainty tables in the map
    ''' </summary>
    ''' <returns></returns>
    Shared Function uncertaintyTableCount() As Integer
        uncertaintyTableCount = 0
        For Each tbl As IStandaloneTable In MapUtility.uncertaintyTables()
            uncertaintyTableCount += 1
        Next
    End Function


    Shared Iterator Function featureLayers(Optional ByVal layerGenericName As String = Nothing, Optional ByVal ifEventLayer As Boolean = True) As IEnumerable(Of IFeatureLayer)
        For m As Integer = 0 To My.Document.Maps.Count - 1
            Dim map As IMap = My.Document.Maps.Item(m)
            For i As Integer = 0 To map.LayerCount - 1
                If TypeOf map.Layer(i) Is IFeatureLayer Then
                    Dim featLyr As IFeatureLayer = DirectCast(map.Layer(i), IFeatureLayer)
                    If Not ifEventLayer Then
                        If featLyr.DataSourceType = "XY Event Source" Then
                            Continue For
                        End If
                    End If
                    If featLyr.Valid Then
                        If GeoDB.isInPersonalGeodatabaseWorkspace(featLyr) Then
                            If layerGenericName Is Nothing OrElse GeoDB.layerIsValid(featLyr, layerGenericName, True) Then
                                Yield featLyr
                            End If
                        End If
                    End If
                End If
            Next
        Next
    End Function

    'Pick the layers that matches the names
    Shared Iterator Function getFeatureLayersByNames(Optional ByVal layers As IEnumerable(Of IFeatureLayer) = Nothing, Optional ByVal layerNames As List(Of String) = Nothing) As IEnumerable(Of IFeatureLayer)
        If layers Is Nothing Or layerNames Is Nothing Then
            Return
        End If

        For Each layer As IFeatureLayer In layers
            If layerNames.Exists(Function(ByVal layerName As String) layerName = layer.Name) Then
                Yield layer
            End If
        Next
    End Function

    'calculate distance between two points along a straight line
    Public Shared Function GetDistanceBetweenTwoPoints(ByVal startPoint As IPoint, ByVal endPoint As IPoint) As Double
        Dim ipo As IProximityOperator = DirectCast(startPoint, IProximityOperator)
        Return ipo.ReturnDistance(endPoint)
    End Function

    'Features are selected in map.  Need to find out the selected
    'features for a particular layer in the map.
    Public Shared Function GetSelectedFeatureCursor(ByVal featLayer As IFeatureLayer) As IFeatureCursor
        Dim featSelection As IFeatureSelection
        Dim cur As ICursor = Nothing
        Dim selSet As ISelectionSet

        featSelection = CType(featLayer, IFeatureSelection)     'QI
        selSet = featSelection.SelectionSet
        'Can use Nothing keyword if you don't want to draw them,
        'otherwise, the spatial reference might not match the Map's
        selSet.Search(Nothing, False, cur)
        Return CType(cur, IFeatureCursor)
    End Function

    '
    'Returns total number of selected features
    '
    Public Shared Function GetSelectedCount(ByVal featLayer As IFeatureLayer) As Integer
        Dim featSel As IFeatureSelection = CType(featLayer, IFeatureSelection)
        Dim selSet As ISelectionSet = featSel.SelectionSet
        Return selSet.Count
    End Function

    ''' <summary>
    ''' Returns a list containing OIDs of all selected shoreline features. Returns nothing if no shorelines selected.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getSelectedShoreIds() As List(Of Integer)
        Dim fc As IFeatureCursor = GetSelectedFeatureCursor(findFeatureLayer(My.Settings.Shoreline_Feature_Layer, "shoreline"))
        getSelectedShoreIds = New List(Of Integer)

        Dim done As Boolean = False
        While Not done
            Dim feat As IFeature = fc.NextFeature
            If feat Is Nothing Then Exit While
            getSelectedShoreIds.Add(feat.OID)
        End While
        If getSelectedShoreIds.Count = 0 Then getSelectedShoreIds = Nothing
    End Function


    'add feature class to the current map.  adds feature class to the current
    'map and returns the feature layer.  If the feature class is already in the 
    'map adds nothing and return "Nothing"
    '
    Public Shared Function AddFeatureClassToMap(ByRef featCls As IFeatureClass) As IFeatureLayer
        Dim featLyr As IFeatureLayer = Nothing

        'check if input feature class is valid
        If featCls IsNot Nothing Then
            'check if input feature class is already in map
            'if not then add the input feature class to current map
            If findFeatureLayer(featCls.AliasName) Is Nothing Then
                'add layer to the map
                featLyr = New FeatureLayer
                featLyr.FeatureClass = featCls
                featLyr.Name = featCls.AliasName
                My.Document.FocusMap.AddLayer(featLyr)
            End If
        End If
        Return featLyr
    End Function

    '
    'add table to the map
    '
    Public Shared Sub AddTableToToc(ByVal dbTable As ITable)
        Dim stTab As IStandaloneTable
        Dim stTabColl As IStandaloneTableCollection
        If dbTable Is Nothing Then Exit Sub
        'check if table is already in the map
        stTab = New StandaloneTable
        stTab.Table = dbTable
        If (FindTableInMap(stTab.Name) Is Nothing) Then
            stTabColl = CType(My.Document.FocusMap, IStandaloneTableCollection)
            stTabColl.AddStandaloneTable(stTab)
        End If

        My.Document.UpdateContents()
    End Sub
    '
    'check linear unit for input layer.  If linear unit is meter
    'then return true, else false
    '
    Public Shared Function isLinearUnitMeter(ByRef featCls As IFeatureClass) As Boolean
        If featCls Is Nothing Then Return False

        'only projected coordinate system has linear unit, geographic coordinate system has
        'angular unit and unknown coordinate system unit is unknown.
        'If spatial reference is not projected coordinate system then return false.
        'Otherwise check for the linear unit and return true if the linear
        'unit is METER
        Dim spatRef As ISpatialReference = featCls.Fields.Field(featCls.FindField(featCls.ShapeFieldName)).GeometryDef.SpatialReference
        If TypeOf spatRef Is IProjectedCoordinateSystem Then
            Dim projCoordSys As IProjectedCoordinateSystem = CType(spatRef, IProjectedCoordinateSystem)
            If (projCoordSys.CoordinateUnit.Name.ToUpper = "METER") Then
                Return True
            End If
        End If
        Return False
    End Function


    'show progress
    '
    Public Shared Function InitStepProgressor(ByRef statusBar As IStatusBar,
     ByVal rangeMin As Integer,
     ByVal rangeMax As Integer,
     ByVal stepVal As Integer) As IStepProgressor
        Dim stepPro As IStepProgressor
        'if min and max range are same, OutOfScope exception occurs
        If rangeMax = rangeMin Then
            rangeMax += 1
        End If

        stepPro = statusBar.ProgressBar
        If stepPro IsNot Nothing Then
            stepPro.MaxRange = rangeMax
            stepPro.MinRange = rangeMin
            stepPro.StepValue = stepVal
            stepPro.Position = 1
            stepPro.Show()
        End If
        Return stepPro
    End Function


    Public Shared Function workspaceForLayer(ByVal layerName As String) As IWorkspace
        Dim ilyr As ILayer = findFeatureLayer(layerName)
        If ilyr IsNot Nothing Then Return DirectCast(ilyr, IDataset).Workspace
        Dim itbl As ITable = FindTableInMap(layerName)
        If itbl IsNot Nothing Then Return DirectCast(itbl, IDataset).Workspace

        Return Nothing
    End Function


    Public Shared Function dbConnectionForLayer(ByVal layerName As String) As OleDbConnection
        Try
            Dim dbPath As String = workspaceForLayer(layerName).PathName
            If dbPath Is Nothing OrElse jetProvider Is Nothing Then Return Nothing
            Dim cn As OleDbConnection = New OleDbConnection(String.Format("Provider={0};Data Source={1};", jetProvider, dbPath))
            cn.Open()
            Return cn
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Shared jetProvider_ As String = Nothing

    Private Shared ReadOnly Property jetProvider As String
        Get
            If jetProvider_ Is Nothing Then
                Dim enumerator As New OleDbEnumerator
                Dim table As System.Data.DataTable = enumerator.GetElements()
                For Each row As DataRow In table.Rows
                    jetProvider = row("SOURCES_NAME").ToString()
                    If jetProvider.StartsWith("Microsoft.Jet.OLEDB.") Then    'Microsoft.Jet.OLEDB.4.0
                        jetProvider_ = jetProvider
                        Exit For
                    End If
                Next
            End If

            Return jetProvider_
        End Get
    End Property

    Public Shared Function findGeometryFieldIndex(ByRef featClass As IFeatureClass) As Integer
        For i As Integer = 0 To featClass.Fields().FieldCount
            If featClass.Fields().Field(i).Type = esriFieldType.esriFieldTypeGeometry Then
                Return i
            End If
        Next
    End Function


    Public Shared Function getRateColorList() As List(Of IRgbColor)
        Dim colorList As List(Of IRgbColor) = New List(Of IRgbColor)
        Dim c As IRgbColor

        c = New RgbColorClass
        With c
            .Red = 178
            .Green = 24
            .Blue = 43
        End With
        colorList.Add(c)

        c = New RgbColorClass
        With c
            .Red = 214
            .Green = 96
            .Blue = 77
        End With
        colorList.Add(c)

        c = New RgbColorClass
        With c
            .Red = 244
            .Green = 165
            .Blue = 130
        End With
        colorList.Add(c)

        c = New RgbColorClass
        With c
            .Red = 253
            .Green = 219
            .Blue = 199
        End With
        colorList.Add(c)

        c = New RgbColorClass
        With c
            .Red = 231
            .Green = 224
            .Blue = 220
        End With
        colorList.Add(c)

        c = New RgbColorClass
        With c
            .Red = 209
            .Green = 229
            .Blue = 240
        End With
        colorList.Add(c)

        c = New RgbColorClass
        With c
            .Red = 146
            .Green = 197
            .Blue = 222
        End With
        colorList.Add(c)

        c = New RgbColorClass
        With c
            .Red = 67
            .Green = 147
            .Blue = 195
        End With
        colorList.Add(c)

        c = New RgbColorClass
        With c
            .Red = 33
            .Green = 102
            .Blue = 172
        End With
        colorList.Add(c)

        Return colorList

    End Function


    Public Shared Function getRateClassBreakRenderer(ByRef rateFeatClass As IFeatureClass, ByVal rate As String, scaled As Boolean) As IClassBreaksRenderer
        Dim classBreaksRenderer As IClassBreaksRenderer = New ClassBreaksRendererClass
        Dim effectiveClassBreakCount As Integer = 0
        classBreaksRenderer.Field = rate
        classBreaksRenderer.BreakCount = 0  'classBreakCount
        classBreaksRenderer.SortClassesAscending = True


        'Calculate the class interval by a simple mean value
        Dim colorList As List(Of IRgbColor) = getRateColorList()
        Dim classBreaksUIProperties As IClassBreaksUIProperties = classBreaksRenderer
        Dim breaks As List(Of Double) = getClassBreaks(rate, scaled, rateFeatClass)

        Dim j As Integer = 1
        'Loop through each class break
        For i As Integer = 0 To classBreakCount
            If i = 0 Then
                classBreaksRenderer.MinimumBreak = breaks(i)
            Else
                If breaks(i) <> breaks(i - 1) Then
                    classBreaksRenderer.BreakCount += 1
                    classBreaksRenderer.Break(j - 1) = breaks(i)
                    classBreaksRenderer.Label(j - 1) = String.Format("{1:N1} < {0} <= {2:N1}", rate, breaks(i - 1), breaks(i))
                    classBreaksUIProperties.LowBreak(j - 1) = breaks(i - 1)
                    'Create simple fill symbol and set color
                    Dim simpleLineSymbol = New SimpleLineSymbolClass
                    simpleLineSymbol.Color = colorList.Item(i - 1)
                    simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid
                    simpleLineSymbol.Width = 1.5
                    classBreaksRenderer.Symbol(j - 1) = simpleLineSymbol
                    j += 1
                End If
            End If
        Next i

        addNullDataExclusion(classBreaksRenderer, rate)
        Return classBreaksRenderer
    End Function


    Const classBreakCount = 9


    Shared Function adjustForLabel(val As Double) As Double
        Return Math.Sign(val) * Math.Truncate(Math.Ceiling(Math.Abs(val) * 10)) / 10
    End Function


    Shared Function getClassBreaks(rate As String, scaled As Boolean, rateFeatClass As IFeatureClass) As List(Of Double)
        Dim breaks As List(Of Double) = New List(Of Double)

        Dim positiveExtreme As Double = GeoDB.getFldExtreme(rateFeatClass, rate, True, String.Format("[{0}] >= 0", rate))
        Dim negativeExtreme As Double = GeoDB.getFldExtreme(rateFeatClass, rate, False, String.Format("[{0}] <= 0", rate))
        If scaled Then
            Dim positiveCount As Long = GeoDB.countMatching(rateFeatClass.AliasName, String.Format("[{0}] >= 0", rate))
            Dim negativeCount As Long = GeoDB.countMatching(rateFeatClass.AliasName, String.Format("[{0}] <= 0", rate))
            Dim positive85Value As Double = GeoDB.valueAtPercent(rateFeatClass.AliasName, rate, positiveCount * 0.85, True)
            Dim negative85Value As Double = GeoDB.valueAtPercent(rateFeatClass.AliasName, rate, negativeCount * 0.85, False)

            If checkForStat(rate, "SCE") Then
                breaks.AddRange({
                            adjustForLabel(negativeExtreme),
                            adjustForLabel(negative85Value),
                            adjustForLabel(negative85Value * 3.0 / 4.0),
                            adjustForLabel(negative85Value * 2.0 / 4.0),
                            adjustForLabel(negative85Value * 1.0 / 4.0),
                            adjustForLabel(positive85Value * 1.0 / 4.0),
                            adjustForLabel(positive85Value * 2.0 / 4.0),
                            adjustForLabel(positive85Value * 3.0 / 4.0),
                            adjustForLabel(positive85Value),
                            adjustForLabel(positiveExtreme)})
            Else
                breaks.AddRange({
                            adjustForLabel(negativeExtreme),
                            adjustForLabel(negative85Value),
                            adjustForLabel(negative85Value * 2.0 / 3.0),
                            adjustForLabel(negative85Value * 1.0 / 3.0),
                            IIf(checkForStat(rate, "NSM"), -10, -0.5),
                            IIf(checkForStat(rate, "NSM"), +10, +0.5),
                            adjustForLabel(positive85Value * 1.0 / 3.0),
                            adjustForLabel(positive85Value * 2.0 / 3.0),
                            adjustForLabel(positive85Value),
                            adjustForLabel(positiveExtreme)})
                ' If the fixed middle bin encroaches on its neighboring bin(s) then bring it in line
                If breaks(3) > breaks(4) Then breaks(4) = breaks(3)
                If breaks(5) > breaks(6) Then breaks(5) = breaks(6)
            End If
        Else
            breaks.AddRange({adjustForLabel(Math.Min(negativeExtreme, -4)), -3, -2, -1, -0.5, 0.5, 1, 2, 3, adjustForLabel(Math.Max(4, positiveExtreme))})
        End If

        Return breaks
    End Function



    Shared Sub addNullDataExclusion(classBreaksRenderer As IClassBreaksRenderer, rate As String)
        ' The following block of code allows us to render NULL valued rates as gray colored
        Dim nullColor As ESRI.ArcGIS.Display.IGrayColor = New ESRI.ArcGIS.Display.GrayColorClass()
        nullColor.Level = 192
        Dim nullSymbol As New SimpleLineSymbolClass()
        nullSymbol.Color = nullColor
        nullSymbol.Width = 1
        nullSymbol.Style = esriSimpleLineStyle.esriSLSDot   '.esriSLSDash
        Dim nullData As IDataExclusion = classBreaksRenderer
        nullData.ExclusionClause = String.Format("[{0}] IS NULL", rate)
        nullData.ExclusionDescription = "Rate not calculated at this transect"
        nullData.ExclusionLabel = "NULL"
        nullData.ExclusionSymbol = nullSymbol
        nullData.ShowExclusionClass = True
    End Sub


    Shared Sub setRateColorRamp(lyr As IGeoFeatureLayer, field As String, Optional scaled? As Boolean = Nothing)
        If scaled Is Nothing Then
            scaled = (checkForStat(field, "SCE") OrElse checkForStat(field, "NSM"))
        End If
        Dim classBreaksRenderer As IClassBreaksRenderer = getRateClassBreakRenderer(lyr.FeatureClass, field, scaled)
        If classBreaksRenderer.BreakCount < 1 Then
            log(TraceLevel.Error, "Rate chosen for color ramp display contains invalid values for scaling data. Please check the field selected to ensure that it contains numeric, non-null content.")
        Else
            lyr.Renderer = classBreaksRenderer

            My.Document.CurrentContentsView.Refresh(Nothing)
            My.Document.ActiveView.Refresh()
        End If
    End Sub

    Shared Function lineSymbol(r As Short, g As Short, b As Short) As ICartographicLineSymbol
        Dim clr As IRgbColor = New RgbColorClass
        With clr
            .Red = r
            .Green = g
            .Blue = b
        End With
        Dim cartoLine As ICartographicLineSymbol = New CartographicLineSymbolClass
        cartoLine.Width = 2
        cartoLine.Color = clr
        Return cartoLine
    End Function


    Shared Function fillSymbol(r As Short, g As Short, b As Short) As ISimpleFillSymbol
        Dim clr As IRgbColor = New RgbColorClass
        With clr
            .Red = r
            .Green = g
            .Blue = b
        End With
        'Create a Simple Fill    
        Dim smplFill As ISimpleFillSymbol = New SimpleFillSymbolClass
        'Create the Fill Symbol and set the properties  
        smplFill.Color = clr
        Return smplFill
    End Function

End Class
