Imports System.Runtime.InteropServices
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.SystemUI


Public Class Topology
    Inherits ESRI.ArcGIS.Desktop.AddIns.Extension

    'Declare the editor and the edit event interface.
    Public Shared m_editor As IEditor
    Private Shared m_editor2 As IEditor2
    'Private e As IEditToolEvents_Event
    Private Shared WithEvents m_editEvents As IEditEvents_Event
    Private Shared WithEvents m_editEvents2 As IEditEvents2_Event
    'Private Shared pMxDoc As IMxDocument
    Private Shared cPolyline As IPolyline
    Private Shared snapOn As Boolean = True
    Private Shared vX As Double = 0
    Private Shared vY As Double = 0
    Private Shared sketchModified As Boolean = False
    'Private WithEvents zzz As EditToolEvents
    'Private deleg As IEditEvents2_OnVertexAddedEventHandler
    'Private deleg2 As IEditEvents2_OnVertexMovedEventHandler

    'Private Shared deletedObjs As New List(Of IFeature)
    Private Shared deletedObjs As New List(Of Object())
    Private Shared createdObjs As New List(Of IFeature)
    Private Shared changedObjs As New List(Of IFeature)
    Private Shared xectsToDelete As New List(Of IFeature)
    Private Shared copier As New ObjectCopy
    'Private Shared voice As New SpeechLib.SpVoice
    Private Shared inv As IInvalidArea3





    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()
    End Sub


    Private Sub WireEditEvents()
        'You can get app from ICommand :: OnCreate() hook parameter
        'Dim editorUid As UID = New UIDClass()
        'editorUid.Value = "esriEditor.Editor"
        'Dim editor As IEditor = TryCast(DSASUtility.getArcMapApp.FindExtensionByCLSID(editorUid), IEditor)
        Dim editor As IEditor = My.ArcMap.Editor
        'Wire editor events.
        AddHandler (CType(editor, IEditEvents2_Event).OnVertexAdded), AddressOf OnVertexAdded
    End Sub

    Private Sub WireEditEvents2()
        AddHandler CType(m_editor, IEditEvents2_Event).OnVertexAdded, New IEditEvents2_OnVertexAddedEventHandler(AddressOf OnVertexAdded)
    End Sub



    Private h As IEditEvents2_OnVertexAddedEventHandler

    Private Sub WireEditEvents3()
        h = New IEditEvents2_OnVertexAddedEventHandler(AddressOf OnVertexAdded)
        AddHandler CType(m_editor2, IEditEvents2_Event).OnVertexAdded, h
        'RemoveHandler CType(m_editor2, IEditEvents2_OnVertexAddedEventHandler).AfterDraw, h
    End Sub



    Private Sub OnVertexAdded(ByVal point As IPoint)
        System.Windows.Forms.MessageBox.Show("Vertex Added.")
    End Sub

    Protected Overrides Sub OnStartup()
        m_editor = My.ArcMap.Editor
        m_editEvents = CType(m_editor, IEditEvents_Event)

        ' Disabled dynamic topology 20180102
        ''Wire editor events.
        'AddHandler m_editEvents.OnStartEditing, AddressOf m_editEvents_OnStartEditing
        'AddHandler m_editEvents.OnStopEditing, AddressOf m_editEvents_OnStopEditing
    End Sub

    Protected Overrides Sub OnShutdown()
        ' Disabled dynamic topology 20180102
        'RemoveHandler m_editEvents.OnStartEditing, AddressOf m_editEvents_OnStartEditing
        'RemoveHandler m_editEvents.OnStopEditing, AddressOf m_editEvents_OnStopEditing
        m_editor2 = Nothing
        m_editEvents = Nothing
        m_editEvents2 = Nothing
    End Sub



#Region "EditorExtensionSpecificCode"

    Private Sub m_editEvents_OnStartEditing()
        If TransectLyrToolCtl.currentTransectLayer IsNot Nothing Then
            Try
                DSAS.seaward = getSeawardLandwardSettingFromMetadata(TransectLyrToolCtl.currentTransectLayer)
                If DSAS.seaward Is Nothing Then
                    Dim msg As String = String.Format("Transect layer selected '{0}' does not have a landward/seaward setting recorded in the process steps of the metadata record (Data Quality - Process Step - Process Description). Since this setting is a new feature recorded during DSAS v5.0 transect creation, this information will be missing for legacy transects even after they are upgraded. DSAS defaults to the setting currently selected in the Default Parameters user interface which is '{1}' for this calculation.", TransectLyrToolCtl.currentTransectLayerName, IIf(My.Settings.Seaward, "seaward", "landward"))
                    log(msg)
                    MsgBox(msg,, DSAS.MsgBoxTitle)
                    DSAS.seaward = My.Settings.Seaward
                End If
            Catch ex As Exception
            End Try

            wireEvents()
        End If
    End Sub

    Public Shared Sub wireEvents()
        Try

            ' Nothing to do if not in an edit session
            'If Not isInEditSession() Then Return

            'WireEditEvents3()
            'DSAS.Instance.fMap = m_editor2.Map

            ' Nothing to do if no transect layer is selected
            'If TransectLyrToolCtl.currentTransectLayerName Is Nothing Then Return

            Dim baselineLayer As IFeatureLayer = MapUtility.findFeatureLayer(My.Settings.Baseline_Feature_Layer, "baseline")
            If baselineLayer Is Nothing Then
                DSASUtility.log(TraceLevel.Error,
                "Please make sure that your default settings for baseline layer are correct." + vbCrLf +
                "DSAS topology enforcement will be offline during this edit session.")
                Return
            End If
            'If Not GeoDB.layerIsValid(baselineLayer, "baseline") Then
            '    DSASUtility.log(TraceLevel.Error,
            '    "Please make sure that your baseline layer has the correct structure." + vbCrLf +
            '    "DSAS topology enforcement will be offline during this edit session.")
            '    Return
            'End If

            m_editor2 = My.ArcMap.Editor
            m_editEvents2 = CType(m_editor2, IEditEvents2_Event)

            inv = New InvalidArea
            inv.Display = m_editor2.Display

            AddHandler m_editEvents.OnChangeFeature, AddressOf OnChangeFeatureHndlr
            AddHandler m_editEvents.OnDeleteFeature, AddressOf OnDeleteFeatureHndlr
            AddHandler m_editEvents.OnCreateFeature, AddressOf OnCreateFeatureHndlr
            AddHandler m_editEvents.OnUndo, AddressOf OnUndoHndlr
            'deleg = New IEditEvents2_OnVertexAddedEventHandler(AddressOf OnVertexAddedHndlr)
            'AddHandler m_editEvents2.OnVertexAdded, deleg
            'AddHandler (CType(m_editor2, IEditEvents2_Event)).OnVertexAdded, AddressOf OnVertexAddedHndlr
            AddHandler m_editEvents.OnSketchModified, AddressOf OnSketchModifiedHndlr
            'AddHandler m_editEvents2.OnVertexAdded, AddressOf OnVertexAddedHndlr    'XXXXXXX
            'AddHandler m_editEvents2.OnVertexDeleted, AddressOf OnVertexDeletedHndlr
            AddHandler m_editEvents2.OnVertexMoved, AddressOf OnVertexMovedHndlr
            AddHandler m_editEvents2.OnStopOperation, AddressOf OnStopOperationHndlr
            'AddHandler m_editEvents.OnCurrentTaskChanged, AddressOf m_editEvents_OnCurrentTaskChanged

            AddHandler m_editEvents.OnSelectionChanged, AddressOf OnSelectionChangedHndlr
            'AddHandler m_editEvents.OnCreateFeature, AddressOf m_editEvents_OnCreateFeature
            'AddHandler m_editEvents.OnCurrentLayerChanged, AddressOf m_editEvents_OnCurrentLayerChanged

            'The following code would be needed for hooking into Edit Tool Events
            'though they don't serve a purpose for DSAS.
            'Dim t As Type = Type.GetTypeFromProgID("esriFramework.AppRef")
            'Dim obj As System.Object = Activator.CreateInstance(t)
            'Dim app As ESRI.ArcGIS.Framework.IApplication = obj

            'Dim pUID As New UID
            'pUID.Value = "esriCore.EditTool"
            'Dim pCommandItem As ESRI.ArcGIS.Framework.ICommandItem
            'pCommandItem = app.Document.CommandBars.Find(pUID)
            'Dim pEditTool As IEditTool
            'pEditTool = pCommandItem.Command
            'e = CType(pEditTool.EventSource, IEditToolEvents_Event)
            'AddHandler e.OnFinishMove, AddressOf OnFinishMove
        Catch ex As Exception
            DSASUtility.handleException(ex)
        End Try
    End Sub

    Private Sub m_editEvents_OnStopEditing(ByVal save As Boolean)
        DSAS.seaward = Nothing
        unwireEvents()
    End Sub

    Public Shared Sub unwireEvents()
        If m_editEvents IsNot Nothing Then
            RemoveHandler m_editEvents.OnChangeFeature, AddressOf OnChangeFeatureHndlr
            RemoveHandler m_editEvents.OnDeleteFeature, AddressOf OnDeleteFeatureHndlr
            RemoveHandler m_editEvents.OnCreateFeature, AddressOf OnCreateFeatureHndlr
            RemoveHandler m_editEvents.OnUndo, AddressOf OnUndoHndlr
            RemoveHandler m_editEvents2.OnStopOperation, AddressOf OnStopOperationHndlr
            'RemoveHandler m_editEvents2.OnVertexAdded, deleg
            'RemoveHandler m_editEvents2.OnVertexAdded, AddressOf OnVertexAddedHndlr
            'RemoveHandler m_editEvents2.OnVertexDeleted, AddressOf OnVertexDeletedHndlr
            RemoveHandler m_editEvents2.OnVertexMoved, AddressOf OnVertexMovedHndlr
            'RemoveHandler m_editEvents.OnCurrentTaskChanged, AddressOf m_editEvents_OnCurrentTaskChanged
            RemoveHandler m_editEvents.OnSketchModified, AddressOf OnSketchModifiedHndlr

            RemoveHandler m_editEvents.OnSelectionChanged, AddressOf OnSelectionChangedHndlr
            'RemoveHandler m_editEvents.OnCreateFeature, AddressOf m_editEvents_OnChangeFeature
        End If

        DSASUtility.ReleaseComObject(inv)
        refreshAll()
    End Sub

    Private Shared Sub OnStopOperationHndlr()
        Try
            debugEvent("Stop Operation")
            If deletedObjs.Count + createdObjs.Count + changedObjs.Count = 0 Then Return

            Dim iops As IOperationStack = CType(m_editor.Parent.Document, IMxDocument).OperationStack
            If iops.Count = 0 Then Return
            Dim opStr As String = iops.Item(iops.Count - 1).MenuString

            log("Deletes: " & deletedObjs.Count)
            log("Creates: " & createdObjs.Count)
            log("Changes: " & changedObjs.Count)
            If opStr = "Split" Then
                'We handle split during change feature
            ElseIf opStr = "Merge" Then
                'We handle merge during change feature
            ElseIf opStr = "Delete Feature" Then
                deleteFeature()
            Else
                log("Don't handle: " & opStr)
            End If
            deletedObjs.Clear()
            createdObjs.Clear()
            changedObjs.Clear()

            'We delete transects on a deleted baseline feature post-humously  
            'so that we can wrap it up in its own operation which provides undo capability.
            If xectsToDelete.Count > 0 Then
                m_editor.StartOperation()
                For Each f As IFeature In xectsToDelete
                    f.Delete()
                Next
                xectsToDelete.Clear()
                m_editor.StopOperation("Delete Transects")
            End If

            refreshAll()
        Catch argEx As ArgumentException
            DSASUtility.handleArgException(argEx)
            Exit Try
        Catch comex As COMException
            DSASUtility.handleComException(comex)
            Exit Try
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
            Exit Try
        Finally
        End Try
    End Sub

    Private Shared Sub OnDeleteFeatureHndlr(ByVal o As IObject)
        'If getSelectedBaselineFeature() Is Nothing Then Return
        debugEvent("Delete Feature")
        If o.Class.AliasName.ToLower = My.Settings.Baseline_Feature_Layer.ToLower Then
            'deletedObjs.Add(o)
            'deletedObjs.Add(New Object() {o, o.Value(GeoDB.baseFldMap("ID"))})
            deletedObjs.Add(New Object() {copier.Copy(CType(o, IFeature).ShapeCopy), o.Value(GeoDB.baseFldMap("ID"))})
        Else
            inv.Add(o)
            'AE: is this really useful?
            'inv.Invalidate(-2)  'esriAllScreenCaches = -2

            'm_editor2.Display.Invalidate(Nothing, True, -2)
            'm_editor2.Display.Invalidate(Nothing, True, Nothing)
        End If
    End Sub

    Private Shared Sub OnCreateFeatureHndlr(ByVal o As IObject)
        'If getSelectedBaselineFeature() Is Nothing Then Return
        debugEvent("Create Feature")
        If o.Class.AliasName.ToLower = My.Settings.Baseline_Feature_Layer.ToLower Then
            createdObjs.Add(o)
        Else
            'AE
            Dim xectLyr As String = TransectLyrToolCtl.currentTransectLayerName
            If xectLyr Is Nothing Then Return
            If o.Class.AliasName.ToLower <> xectLyr.ToLower Then Return
            setXectAttrs(o)
        End If
    End Sub



    ''' <summary>
    ''' Delete the feature whose info is provided in deletedObjs array
    ''' </summary>
    ''' <remarks>We can't lookup feature's ID from the object itself as the feature has already been deleted and its fields detached</remarks>
    Private Shared Sub deleteFeature()
        Dim featCache As New FeatureCacheClass
        Dim pt As IPoint = New Point

        Try
            Dim bl As IPolyline = CType(deletedObjs(0)(0), IPolyline)

            Dim xectLyr As IFeatureLayer = TransectLyrToolCtl.currentTransectLayer
            If xectLyr Is Nothing Then Return

            Dim blIdFldIdx As Integer = GeoDB.baseFldMap("id")
            Dim xectBlIdFldIdx As Integer = GeoDB.transFldMap("BaselineId")

            'Dim featChanges As IFeatureChanges = DirectCast(feat, IFeatureChanges)

            Dim xectFeatClass As IFeatureClass = xectLyr.FeatureClass

            'Find the mid-point of original segment
            bl.QueryPoint(esriSegmentExtension.esriNoExtension, 0.5, True, pt)
            'Fill the cache with transects around that midpoint
            featCache.Initialize(pt, bl.Length * 1.01)  'Allow for 1% error
            featCache.AddFeatures(xectFeatClass)

            For i As Integer = 0 To featCache.Count - 1
                Dim pFeature As IFeature = featCache.Feature(i)
                'Check if the cached transect is actually connected to the baseline feature
                ' First, check for baseline id match...
                If pFeature.Value(xectBlIdFldIdx) <> deletedObjs(0)(1) Then Continue For
                ' Then, check if transect's from point is on the baseline feature...
                Dim xect As IPolyline = DirectCast(pFeature.Shape, IPolyline)
                Dim proxOp As IProximityOperator = anchorPoint(xect)
                If proxOp.ReturnDistance(bl) > DSAS.REP_ERR Then Continue For
                xectsToDelete.Add(pFeature)
                'pFeature.Delete()
            Next

            refreshAll()
        Catch argEx As ArgumentException
            DSASUtility.handleArgException(argEx)
            Exit Try
        Catch comex As COMException
            DSASUtility.handleComException(comex)
            Exit Try
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
            Exit Try
        Finally
            DSASUtility.ReleaseComObject(featCache)
        End Try
    End Sub

    Private Shared Sub copyBaselineAttrs2Xects3(ByVal blFeat As IFeature, ByVal xectLyrName As String, ByVal prevBlId As Long)
        Dim featCache As New FeatureCacheClass
        Dim pt As IPoint = New Point

        Try
            Dim blGroupFldIdx As Integer = GeoDB.baseFldMap("group")
            Dim blIdFldIdx As Integer = GeoDB.baseFldMap("id")

            Dim xectGroupFldIdx As Integer = GeoDB.transFldMap("GroupId")
            Dim xectBlIdFldIdx As Integer = GeoDB.transFldMap("BaselineId")

            Dim xectFeatClass As IFeatureClass = MapUtility.findFeatureLayer(xectLyrName).FeatureClass
            'Find the mid-point of original segment
            Dim bl As IPolyline = blFeat.ShapeCopy
            bl.QueryPoint(esriSegmentExtension.esriNoExtension, 0.5, True, pt)
            'Fill the cache with transects around that midpoint
            featCache.Initialize(pt, bl.Length * 1.01)  'Allow for 1% error
            featCache.AddFeatures(xectFeatClass)

            For i As Integer = 0 To featCache.Count - 1
                Dim xect As IFeature = featCache.Feature(i)
                'Check if the cached transect is actually connected to the baseline feature
                ' First, check for baseline id match...
                If xect.Value(xectBlIdFldIdx) <> prevBlId Then Continue For
                ' Then, check if transect's from point is on the baseline feature...
                Dim xectGeom As IPolyline = DirectCast(xect.Shape, IPolyline)

                Dim proxOp As IProximityOperator = blFeat.ShapeCopy
                If proxOp.ReturnDistance(anchorPoint(xectGeom)) > DSAS.REP_ERR Then Continue For

                'Nothing to do if transect layer has no group field
                If xectGroupFldIdx > -1 Then
                    If blGroupFldIdx > -1 Then
                        xect.Value(xectGroupFldIdx) = blFeat.Value(blGroupFldIdx)
                    Else
                        xect.Value(xectGroupFldIdx) = 0 'DBNull.Value
                    End If
                End If

                If xectBlIdFldIdx > -1 Then
                    If blIdFldIdx > -1 Then
                        xect.Value(xectBlIdFldIdx) = blFeat.Value(blIdFldIdx)
                    Else
                        xect.Value(xectBlIdFldIdx) = DBNull.Value
                    End If
                End If

                xect.Store()
            Next
        Catch ex As Exception
        Finally
            DSASUtility.ReleaseComObject(featCache)
        End Try
    End Sub

    Private Shared Sub copyBaselineAttrs2Xect(ByVal bl As IFeature, ByVal xect As IFeature)
        Dim blGroupFldIdx As Integer = GeoDB.baseFldMap("group")
        Dim blIdFldIdx As Integer = GeoDB.baseFldMap("id")

        Dim xectGroupFldIdx As Integer = GeoDB.transFldMap("GroupId")
        Dim xectBlIdFldIdx As Integer = GeoDB.transFldMap("BaselineId")

        If xectGroupFldIdx > -1 Then
            If blGroupFldIdx > -1 AndAlso bl IsNot Nothing Then
                xect.Value(xectGroupFldIdx) = bl.Value(blGroupFldIdx)
            Else
                xect.Value(xectGroupFldIdx) = 0 'DBNull.Value
            End If
        End If

        If blIdFldIdx > -1 AndAlso bl IsNot Nothing Then
            xect.Value(xectBlIdFldIdx) = bl.Value(blIdFldIdx)
        Else
            xect.Value(xectBlIdFldIdx) = DBNull.Value
        End If
    End Sub


    Private Shared Function getFldExtreme(ByVal featClass As IFeatureClass, ByVal fldName As String) As Double
        Dim pQueryFilter As IQueryFilter = New QueryFilter
        pQueryFilter.SubFields = fldName

        Dim pQueryFilterDefinition As IQueryFilterDefinition
        pQueryFilterDefinition = pQueryFilter
        pQueryFilterDefinition.PostfixClause = "ORDER BY " + fldName + " DESC"

        Dim ifc As IFeatureCursor = featClass.Search(pQueryFilter, True)
        Try
            getFldExtreme = ifc.NextFeature.Value(ifc.FindField(fldName))
        Finally
            DSASUtility.ReleaseComObject(ifc)
        End Try
    End Function


    Private Shared Sub OnChangeFeatureHndlr(ByVal o As IObject)
        Try
            debugEvent("Change Feature")
            Dim feat As IFeature = DirectCast(o, IFeature)
            If feat Is Nothing OrElse feat.Shape Is Nothing Then Return
            If feat.Shape.GeometryType <> esriGeometryType.esriGeometryPolyline Then Return

            Dim xectLyr As String = TransectLyrToolCtl.currentTransectLayerName
            If xectLyr = "" Then Return
            'Nothing to do if changed feature is not a baseline or transect feature
            If feat.Class.AliasName.ToLower <> My.Settings.Baseline_Feature_Layer.ToLower AndAlso feat.Class.AliasName.ToLower <> xectLyr.ToLower Then Return

            If feat.Class.AliasName.ToLower = My.Settings.Baseline_Feature_Layer.ToLower AndAlso (deletedObjs.Count > 0 OrElse createdObjs.Count > 0) Then
                'We got called as part of sth else (merge, split..)
                If deletedObjs.Count = 1 AndAlso createdObjs.Count = 0 Then
                    'We got called for merge
                    copyBaselineAttrs2Xects3(feat, xectLyr, deletedObjs(0)(1))
                ElseIf deletedObjs.Count = 0 AndAlso createdObjs.Count = 1 Then
                    'We got called for split
                    Dim createdFeat As IFeature = createdObjs(0)
                    'First find the max id for bl
                    createdFeat.Value(GeoDB.baseFldMap("id")) = getFldExtreme(feat.Class, "id") + 1
                    'Clear the created objects list before we call IFeature.Store() as it will invoke OnChangeFeature on it.
                    createdObjs.Clear()
                    createdFeat.Store()
                    'We don't copy attrs here but IFeature.Store() will cause this sub to be called again which is when attrs are copied to transects.
                    'copyBaselineAttrs2Xects3(createdFeat, xectLyr, False)
                Else
                End If
            Else
                Dim featChanges As IFeatureChanges = DirectCast(feat, IFeatureChanges)
                If featChanges.ShapeChanged Then
                    Dim orig As IPolyline = DirectCast(featChanges.OriginalShape, IPolyline)
                    Dim curr As IPolyline = DirectCast(feat.Shape, IPolyline)
                    If feat.Class.AliasName.ToUpper = My.Settings.Baseline_Feature_Layer.ToUpper Then
                        snapOn = False
                        If Math.Abs(CType(featChanges.OriginalShape, ICurve).Length - CType(feat.Shape, ICurve).Length) < DSAS.REP_ERR _
                            AndAlso CType(featChanges.OriginalShape, IPointCollection).PointCount = CType(feat.Shape, IPointCollection).PointCount Then
                            handleBaselineXlateRotate(featChanges.OriginalShape, feat.Shape, feat.Value(GeoDB.baseFldMap("ID")))
                        Else
                            'handleBaselineChange(featChanges.OriginalShape, feat.Shape, xectFeatClass)
                        End If
                        snapOn = True
                    ElseIf feat.Class.AliasName.ToUpper = xectLyr.ToUpper Then
                        handleXectChange(orig, curr, feat)
                    End If
                Else
                    ' Row changes...
                    Dim rwChanges As IRowChanges = feat
                    If feat.Class.AliasName.ToUpper = My.Settings.Baseline_Feature_Layer.ToUpper Then
                        copyBaselineAttrs2Xects3(feat, xectLyr, rwChanges.OriginalValue(GeoDB.baseFldMap("ID")))
                    End If
                End If
            End If

            If feat.Class.AliasName.ToUpper = My.Settings.Baseline_Feature_Layer.ToUpper Then
                refreshAll()
            End If

        Catch argEx As ArgumentException
            DSASUtility.handleArgException(argEx)
            Exit Try
        Catch comex As COMException
            DSASUtility.handleComException(comex)
            Exit Try
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
            Exit Try
        Finally
        End Try
    End Sub


    Private Shared Sub handleXectChange(ByVal xectOrig As IPolyline, ByVal xectCurr As IPolyline, ByVal xect As IFeature)
        setXectAttrs(xect)
    End Sub


    Private Shared Sub setXectAttrs(xect As IFeature)
        Dim pNormal As IPolyline = xect.Shape
        If pNormal IsNot Nothing Then xect.Value(xect.Fields.FindField("Azimuth")) = Math.Round(DSASUtility.CalculateAzimuth(pNormal), 2)

        xect.Value(xect.Fields.FindField("TransEdit")) = IIf(DSAS.autogen = 0, 1, 0)

        Dim bl As IFeature = getSelectedBaselineFeature()
        If bl IsNot Nothing Then
            copyBaselineAttrs2Xect(bl, xect)
        Else
            bl = BaselineSnapper.FindBaselineToSnapTo(xect)
            copyBaselineAttrs2Xect(bl, xect)
        End If


    End Sub

    ''' <summary>
    ''' Given the original and new baseline geometries, move and rotate all transects in the given
    ''' feature class touching the original geometry to the new geometry.
    ''' </summary>
    ''' <param name="blOrig"></param>
    ''' <param name="blNew"></param>
    ''' <param name="blId"></param>
    ''' <remarks>
    ''' For each segment in the original geometry, we determine the touching transects, their relative location
    ''' and relative angle. We then move them to the same relative location on the new geometry and rotate to retain
    ''' the same relative angle.
    ''' </remarks>
    Private Shared Sub handleBaselineXlateRotate(ByVal blOrig As IPolyline, ByVal blNew As IPolyline, ByVal blId? As Long)
        'm_editor.StartOperation()
        Dim enumOrigSeg As IEnumSegment = DirectCast(blOrig, ISegmentCollection).EnumSegments
        Dim enumNewSeg As IEnumSegment = DirectCast(blNew, ISegmentCollection).EnumSegments
        Dim origSeg As ISegment = Nothing
        Dim newSeg As ISegment = Nothing
        Do
            enumOrigSeg.Next(origSeg, Nothing, Nothing)
            If origSeg Is Nothing Then Exit Do
            enumNewSeg.Next(newSeg, Nothing, Nothing)
            handleBaselineSegXlateRotate(origSeg, newSeg, blId)
        Loop
        'm_editor.StopOperation("Baseline Move/Rotate")
        refreshAll()
    End Sub


    Shared Function anchorPoint(xect As IPointCollection) As IPoint
        Debug.Assert(xect.PointCount = 3)
        anchorPoint = xect.Point(1)
    End Function


    Private Shared Sub handleBaselineSegXlateRotate(ByVal origSeg As ISegment, ByVal newSeg As ISegment, ByVal blId? As Long)
        Dim featCache As New FeatureCacheClass
        Dim pt As IPoint = New Point

        Try
            'Find the mid-point of original segment
            origSeg.QueryPoint(esriSegmentExtension.esriNoExtension, 0.5, True, pt)
            'Fill the cache with transects around that midpoint
            featCache.Initialize(pt, origSeg.Length / 2 * 1.01)  'Allow for 1% error
            featCache.AddFeatures(TransectLyrToolCtl.currentTransectLayer.FeatureClass)

            For i As Integer = 0 To featCache.Count - 1
                'Check if the cached transect is actually connected to current segment
                Dim pFeature As IFeature = featCache.Feature(i)
                If blId IsNot Nothing AndAlso pFeature.Value(GeoDB.transFldMap("BaselineId")) <> blId Then Continue For
                Dim xect As IPolyline = DirectCast(pFeature.Shape, IPolyline)
                Dim proxOp As IProximityOperator = anchorPoint(xect)
                If proxOp.ReturnDistance(origSeg) > DSAS.REP_ERR Then Continue For

                'Move transect's FromPoint based on distance from segment beginning
                Dim fromDist As Double = proxOp.ReturnDistance(origSeg.FromPoint)
                'newSeg.QueryPoint(esriSegmentExtension.esriNoExtension, fromDist, False, pt)
                'xect.FromPoint = pt

                Dim newPoly As IPolyline = New PolylineClass
                Dim segcol As ISegmentCollection = newPoly
                segcol.AddSegment(newSeg)
                Dim normal As IPolyline = New PolylineClass

                normal = DSASUtility.CreateNormalLine(newPoly, fromDist, getSearchDistance(getSelectedBaselineFeature()))
                If normal Is Nothing Then
                    pFeature.Delete()
                Else
                    pFeature.Shape = normal
                    pFeature.Store()
                End If
            Next
        Catch ex As COMException
            Dim ccc As Int16 = 1
        Catch ex As Exception
            Dim ccc As Int16 = 1
        Finally
            DSASUtility.ReleaseComObject(featCache)
        End Try
    End Sub



    ' Not fired
    Private Shared Sub OnVertexAddedHndlr(ByVal point As IPoint)
        snapOn = False

        Try
            'If not working on a baseline feature then nothing to do.
            If getSelectedBaselineFeature() Is Nothing Then Return
            debugEvent("Vertex Added")

            Dim es As IEditSketch2 = m_editor

            Dim hitDist As Double
            Dim vertIdx As Integer
            Dim ht As IHitTest = es.Geometry    'AE
            'Find the vertex on sketch that was added   'AE
            If ht.HitTest(point, 0.1, esriGeometryHitPartType.esriGeometryPartVertex, Nothing, hitDist, Nothing, vertIdx, Nothing) Then
                'If the vertex lies on the previous sketch then it's simple add (i.e. not undo of delete) and there is nothing to do...
                ht = cPolyline
                If ht.HitTest(point, 0.1, esriGeometryHitPartType.esriGeometryPartBoundary, Nothing, Nothing, Nothing, Nothing, Nothing) Then Return

                'Construct two features representing the original and current versions of the affected baseline section
                Dim pl As New PolylineClass
                Dim plNew As New PolylineClass
                pl.SpatialReference = cPolyline.SpatialReference
                plNew.SpatialReference = cPolyline.SpatialReference

                Dim blSegs As ISegmentCollection = es.Geometry  'AE
                Dim blNewSegs As ISegmentCollection = cPolyline 'AE

                'If from point, then try to borrow transects from next segment
                If vertIdx = 0 Then
                    If blSegs.SegmentCount > 0 Then
                        vertIdx = 1
                    Else
                        Return
                    End If
                End If

                'If to point, then try to borrow transects from previous segment
                If vertIdx = blSegs.SegmentCount Then
                    If vertIdx > 0 Then
                        vertIdx = vertIdx - 1
                    Else
                        Return
                    End If
                End If

                If vertIdx > 0 Then
                    'The segment right before deleted vertex
                    pl.AddSegment(blSegs.Segment(vertIdx - 1))
                End If
                If vertIdx < blSegs.SegmentCount Then
                    'The segment right after deleted vertex
                    pl.AddSegment(blSegs.Segment(vertIdx))
                End If

                If vertIdx > 0 AndAlso vertIdx < blSegs.SegmentCount Then
                    'The new segment created between the vertices on each side of the deleted vertex
                    plNew.AddSegment(blNewSegs.Segment(vertIdx - 1))
                End If

                redecorateXects(plNew, pl)
            End If

        Catch argEx As ArgumentException
            DSASUtility.handleArgException(argEx)
        Catch comex As COMException
            DSASUtility.handleComException(comex)
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
        Finally
            snapOn = True
        End Try
    End Sub

    Private Shared Sub debugEvent(ByVal hndlr As String, Optional ByVal speak As Boolean = True)
        log("------------------------------------------------------")
        log(hndlr)
        'If speak Then voice.Speak(hndlr)
        log("------------------------------------------------------")

        printOperationStack()
    End Sub

    Private Shared Sub printOperationStack()
        Dim iops As IOperationStack = CType(m_editor.Parent.Document, IMxDocument).OperationStack

        Dim s As String = ""
        For i As Integer = 0 To iops.Count - 1
            Dim opStr As String = iops.Item(i).MenuString
            'Dim iop As IOperation = iops.Item(i)
            'Dim opStr As String = GetType(iop)
            s = s & opStr & " / "
        Next
        log("OperationStack: " & s)
    End Sub

    Private Shared Sub OnSketchModifiedHndlr()
        Try
            'We don't care about the sketch except that of the baseline layer
            If getSelectedBaselineFeature() Is Nothing Then Return
            debugEvent("Sketch Modified")

            Dim es As IEditSketch2 = m_editor
            If es.Geometry Is Nothing Then Return
            Dim prevSketch As IPointCollection = cPolyline
            Dim currSketch As IPointCollection = CType(es.Geometry, IPolyline)
            Dim copySketch As Boolean = False

            If currSketch.PointCount = prevSketch.PointCount - 1 Then
                'Vertex deleted
                copySketch = True
                Dim deletedVertex As Point = findDeletedVertex(prevSketch, currSketch)
                If deletedVertex IsNot Nothing Then
                    log("vertex deleted")
                    OnVertexDeletedHndlr(deletedVertex)
                End If
            ElseIf currSketch.PointCount = prevSketch.PointCount + 1 Then
                'Vertex added
                copySketch = True
                Dim addedVertex As Point = findDeletedVertex(currSketch, prevSketch)
                If addedVertex IsNot Nothing Then
                    log("vertex added")
                    OnVertexAddedHndlr(addedVertex)
                End If
            End If

            If copySketch Then
                log("*** Sketch copied ***")
                cPolyline = copier.Copy(es.Geometry)
            End If
        Catch argEx As ArgumentException
            DSASUtility.handleArgException(argEx)
            Exit Try
        Catch comex As COMException
            DSASUtility.handleComException(comex)
            Exit Try
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
            Exit Try
        Finally
        End Try
    End Sub

    Private Shared Function findDeletedVertex(ByVal orig As IPointCollection, ByVal curr As IPointCollection) As Point
        For i As Integer = 0 To curr.PointCount - 1
            If curr.Point(i).Compare(orig.Point(i)) <> 0 Then
                If curr.Point(i).Compare(orig.Point(i + 1)) = 0 Then
                    Return orig.Point(i)
                End If
            End If
        Next
        Return orig.Point(orig.PointCount - 1)
    End Function

    Private Shared Sub OnVertexDeletedHndlr(ByVal point As IPoint)
        snapOn = False

        Try
            'If not working on a baseline feature then nothing to do.
            If getSelectedBaselineFeature() Is Nothing Then Return
            debugEvent("Vertex Deleted")

            Dim es As IEditSketch2 = m_editor

            Dim hitDist As Double
            Dim vertIdx As Integer
            Dim ht As IHitTest = cPolyline
            'Find the vertex on sketch that was deleted
            If ht.HitTest(point, 0.1, esriGeometryHitPartType.esriGeometryPartVertex, Nothing, hitDist, Nothing, vertIdx, Nothing) Then

                'Construct two features representing the original and current versions of the affected baseline section
                Dim pl As New PolylineClass
                Dim plNew As New PolylineClass
                pl.SpatialReference = cPolyline.SpatialReference
                plNew.SpatialReference = cPolyline.SpatialReference

                Dim blSegs As ISegmentCollection = cPolyline
                Dim blNewSegs As ISegmentCollection = es.Geometry

                If vertIdx > 0 Then
                    'The segment right before deleted vertex
                    pl.AddSegment(blSegs.Segment(vertIdx - 1))
                End If
                If vertIdx < blSegs.SegmentCount Then
                    'The segment right after deleted vertex
                    pl.AddSegment(blSegs.Segment(vertIdx))
                End If

                If vertIdx > 0 AndAlso vertIdx < blSegs.SegmentCount Then
                    'The new segment created between the vertices on each side of the deleted vertex
                    plNew.AddSegment(blNewSegs.Segment(vertIdx - 1))
                End If

                redecorateXects(pl, plNew)

            End If
        Catch argEx As ArgumentException
            DSASUtility.handleArgException(argEx)
        Catch comex As COMException
            DSASUtility.handleComException(comex)
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
        Finally
            snapOn = True
        End Try
    End Sub


    Private Shared Sub OnVertexMovedHndlr(ByVal point As IPoint)
        snapOn = False

        Try
            'If not working on a baseline feature then nothing to do.
            If getSelectedBaselineFeature() Is Nothing Then Exit Sub

            debugEvent("Vertex Moved")
            log("X: " & point.X)
            log("Y: " & point.Y)

            Dim es As IEditSketch2 = m_editor

            If point.X = vX AndAlso point.Y = vY Then
                log("Same vertex location!!!!!")
                Return  'AE: will this stop null blid?
            Else
                vX = point.X
                vY = point.Y
            End If

            Dim hitDist As Double
            Dim vertIdx As Integer

            'Construct two features representing the original and current versions of the affected baseline section
            Dim pl As New PolylineClass
            Dim plNew As New PolylineClass

            Dim blSegs As ISegmentCollection
            Dim blNewSegs As ISegmentCollection

            Dim ht As IHitTest = es.Geometry
            blSegs = cPolyline
            blNewSegs = es.Geometry

            'Find the vertex on sketch that was moved
            If ht.HitTest(point, 0.1, esriGeometryHitPartType.esriGeometryPartVertex, Nothing, hitDist, Nothing, vertIdx, Nothing) Then
            Else
                'Undo in progress???
                ht = cPolyline
                If ht.HitTest(point, 0.1, esriGeometryHitPartType.esriGeometryPartVertex, Nothing, hitDist, Nothing, vertIdx, Nothing) Then
                    'undoLastDSASOp()
                Else
                    Return
                End If
            End If

            If vertIdx > 0 Then
                pl.AddSegment(blSegs.Segment(vertIdx - 1))
                plNew.AddSegment(blNewSegs.Segment(vertIdx - 1))
            End If

            If vertIdx < blSegs.SegmentCount Then
                pl.AddSegment(blSegs.Segment(vertIdx))
                plNew.AddSegment(blNewSegs.Segment(vertIdx))
            End If

            redecorateXects(pl, plNew)

            cPolyline = copier.Copy(es.Geometry)
        Catch argEx As ArgumentException
            DSASUtility.handleArgException(argEx)
            Exit Try
        Catch comex As COMException
            DSASUtility.handleComException(comex)
            Exit Try
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
            Exit Try
        Finally
            snapOn = True
        End Try
    End Sub

    Private Shared Sub redecorateXects(ByRef pl As PolylineClass, ByRef plNew As PolylineClass)
        Dim fc As IFeatureCursor = Nothing
        Dim featCache As New FeatureCacheClass
        Try
            DSAS.autogen = 1
            Dim xectFeatClass As IFeatureClass = TransectLyrToolCtl.currentTransectLayer.FeatureClass

            pl.SpatialReference = cPolyline.SpatialReference
            plNew.SpatialReference = cPolyline.SpatialReference
            'Fill feature cache with transects around the affected baseline section
            featCache.Initialize(pl.Point(1), pl.Length * 2)
            featCache.AddFeatures(xectFeatClass)

            Dim progress As IStepProgressor = m_editor.Parent.StatusBar.ProgressBar
            progress.MinRange = 0
            progress.Position = progress.MinRange
            progress.MaxRange = Math.Max(featCache.Count, progress.Position + 1)
            progress.Show()

            Dim transOrderIdx As Integer = GeoDB.transFldMap("TransOrder")
            Dim sortedXects As New SortedDictionary(Of Double, IFeature)
            Dim xect As IFeature
            'Dim ht As IHitTest = pl
            Dim dAlong, dAlongFirst, dFrom As Double
            dAlongFirst = -1
            log("#features: " & featCache.Count)
            For count As Integer = 0 To featCache.Count - 1
                xect = featCache.Feature(count)
                If xect.Shape Is Nothing Then Continue For

                'Test whether the transect is anchored on the changed section of the baseline
                Dim aa As IPoint = anchorPoint(DirectCast(xect.ShapeCopy, IPolyline))
                Try
                    pl.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, aa, False, Nothing, dAlong, dFrom, Nothing)
                Catch ex As Exception
                    Dim a = 1
                End Try
                If dFrom < DSAS.REP_ERR Then
                    sortedXects(dAlong) = xect
                    If dAlongFirst < 0 Then
                        dAlongFirst = dAlong
                    End If
                End If
                log(String.Format("{0} : {1} : {2}", aa.X, aa.Y, dFrom))
                Try
                    If progress.Position < progress.MaxRange - 1 Then progress.Step()
                Catch ex As Exception
                End Try
            Next count

            'Calculate the no of transects needed to cover the new baseline segment.
            Dim nXects As Integer = Math.Ceiling(Math.Max(0, plNew.Length - dAlongFirst) / My.Settings.Spacing)
            'Don't cast beginning transect if the new baseline segment is of zero length.
            If plNew.Length = 0 Then nXects = 0

            Dim sortedXectsNew As New List(Of IFeature)
            sortedXectsNew.AddRange(sortedXects.Values)
            sortedXects = Nothing

            GC.Collect()

            ' Delete any extra transects
            For i As Integer = nXects + 1 To sortedXectsNew.Count
                sortedXectsNew(i - 1).Delete()
                DSASUtility.ReleaseComObject(sortedXectsNew(i - 1))
            Next
            If sortedXectsNew.Count > nXects Then sortedXectsNew.RemoveRange(nXects, sortedXectsNew.Count - nXects)

            GC.Collect()

            If sortedXectsNew.Count > 0 Then
                'We may need to bump up TransOrder values of some transects to make space for new ones.
                Dim transOrderFirst As Integer = sortedXectsNew(0).Value(transOrderIdx)
                Dim transOrderLast As Integer = sortedXectsNew(sortedXectsNew.Count - 1).Value(transOrderIdx)
                Dim bump As Integer = nXects - (transOrderLast - transOrderFirst + 1)
                If bump > 0 Then
                    Dim iqf As New QueryFilterClass
                    iqf.WhereClause = " TransOrder > " & transOrderLast
                    fc = xectFeatClass.Update(iqf, False)
                    Do
                        xect = fc.NextFeature
                        If xect Is Nothing Then Exit Do
                        xect.Value(transOrderIdx) = xect.Value(transOrderIdx) + bump
                    Loop
                End If

                GC.Collect()

                If progress.Position >= nXects Then progress.Position = nXects - 1 'AE
                progress.MaxRange = nXects

                dAlong = Math.Max(0, dAlongFirst)
                xect = sortedXectsNew(0)
                Dim xectBlId As Double = xect.Value(GeoDB.transFldMap("BaselineId"))
                Dim xectBlGrp As Object

                If GeoDB.transFldMap("GroupID") > -1 Then
                    xectBlGrp = xect.Value(GeoDB.transFldMap("GroupID"))
                Else
                    xectBlGrp = 0
                End If

                'Dim transId As Integer = getFldExtreme(xectFeatClass, "TransectID") + 1    ' TransectID deprecated
                Dim transOrder As Integer = transOrderFirst
                ' Add any extra transects
                For i As Integer = 1 To nXects
                    log(i)
                    If i <= sortedXectsNew.Count Then
                        'Reuse an existing transect
                        xect = sortedXectsNew(i - 1)
                    Else
                        xect = xectFeatClass.CreateFeature()
                        'We update some attributes only if this is a new transect
                        xect.Value(GeoDB.transFldMap("BaselineId")) = xectBlId
                        If GeoDB.transFldMap("GroupId") > -1 Then xect.Value(GeoDB.transFldMap("GroupId")) = xectBlGrp
                    End If
                    Dim pNormal As IPolyline = New PolylineClass

                    pNormal = DSASUtility.CreateNormalLine(plNew, dAlong, getSearchDistance(getSelectedBaselineFeature()))
                    xect.Shape = pNormal
                    xect.Value(transOrderIdx) = transOrder
                    setXectAttrs(xect)
                    xect.Store()
                    DSASUtility.ReleaseComObject(xect)
                    transOrder = transOrder + 1
                    dAlong = dAlong + My.Settings.Spacing

                    Try
                        If progress.Position < progress.MaxRange - 1 Then progress.Step()
                    Catch ex As Exception
                    End Try
                Next
            End If

            GC.Collect()

            progress.Hide()

        Catch argEx As ArgumentException
            DSASUtility.handleArgException(argEx)
            Exit Try
        Catch comex As COMException
            DSASUtility.handleComException(comex)
            Exit Try
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
            Exit Try
        Finally
            DSAS.autogen = 0
            snapOn = True
            DSASUtility.ReleaseComObject(fc)
            DSASUtility.ReleaseComObject(featCache)
        End Try

        refreshAll()
    End Sub

    Public Shared Sub beeper(Optional ByVal times As Integer = 1)
        System.Threading.Thread.Sleep(50)
        For i As Integer = 1 To times
            System.Threading.Thread.Sleep(50)
            Beep()
        Next
    End Sub


    Private Shared Function getSelectedBaselineFeature() As IFeature
        'If m_editor2.SelectionCount <> 1 Then Return Nothing
        getSelectedBaselineFeature = Nothing
        Dim enumFeature As IEnumFeature = m_editor2.EditSelection
        Do
            Dim cFeature As IFeature = enumFeature.Next
            If cFeature Is Nothing Then Exit Do
            If cFeature.Class.AliasName.ToLower = My.Settings.Baseline_Feature_Layer.ToLower Then
                If getSelectedBaselineFeature Is Nothing Then
                    getSelectedBaselineFeature = cFeature
                Else
                    'More than one baseline feature selected
                End If
            End If
        Loop
    End Function

    Private Shared Sub OnUndoHndlr()
        Try
            debugEvent("Undo")
            refreshAll()
            ''AE: is this really useful?
            'inv.Invalidate(-2)  'esriAllScreenCaches = -2

            'm_editor2.Display.Invalidate(Nothing, True, -2)
            ''m_editor2.Display.Invalidate(Nothing, True, Nothing)
            'CType(m_editor2.Map, IActiveView).Refresh()
        Catch argEx As ArgumentException
            DSASUtility.handleArgException(argEx)
            Exit Try
        Catch comex As COMException
            DSASUtility.handleComException(comex)
            Exit Try
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
            Exit Try
        End Try
    End Sub

    Private Shared Sub OnSelectionChangedHndlr()
        Try
            'debugEvent("Selection Changed")
            Dim bl As IFeature = getSelectedBaselineFeature()
            If bl Is Nothing Then
                cPolyline = Nothing
                'Dim es As IEditSketch2 = m_editor2
                'If Not es.Geometry.IsEmpty Then es.FinishSketch()
            Else
                cPolyline = bl.ShapeCopy
                'Dim es As IEditSketch2 = m_editor2
                'If es.Geometry.IsEmpty Then
                '    cPolyline = bl.ShapeCopy
                'Else
                '    cPolyline = es.Geometry
                'End If
            End If
        Catch argEx As ArgumentException
            DSASUtility.handleArgException(argEx)
            Exit Try
        Catch comex As COMException
            DSASUtility.handleComException(comex)
            Exit Try
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
            Exit Try
        End Try
    End Sub

    Private Sub OnCurrentTaskChangedHndlr()
        Try
            debugEvent("Current Task Changed")
            'beeper(4)
            Dim enable As Boolean = True
            If m_editor.CurrentTask.Name = "Modify Feature" Then
                Dim feat As IFeature = getSelectedBaselineFeature()
                If feat IsNot Nothing AndAlso feat.Class.AliasName.ToLower = My.Settings.Baseline_Feature_Layer.ToLower Then
                    enable = False
                End If
            End If
            m_editor.EnableUndoRedo(enable)
        Catch argEx As ArgumentException
            DSASUtility.handleArgException(argEx)
            Exit Try
        Catch comex As COMException
            DSASUtility.handleComException(comex)
            Exit Try
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
            Exit Try
        End Try
    End Sub

#End Region

    ''' <summary>
    ''' Refresh the attribute window of a layer, if open
    ''' </summary>
    ''' <param name="lyr">layer</param>
    ''' <remarks></remarks>
    Private Shared Sub refreshAttrWindow(ByVal lyr As IFeatureLayer)
        Try
            If lyr Is Nothing Then Return
            Dim tblWin As ITableWindow2
            Dim tblWin2 As ITableWindow2 = New TableWindow

            tblWin = tblWin2.FindViaLayer(CType(lyr, ILayer))
            If tblWin IsNot Nothing AndAlso tblWin.IsVisible Then
                tblWin.Refresh()
            End If
        Catch
            beeper()
        End Try
    End Sub

    ''' <summary>
    ''' Refresh the attribute windows of the current baseline and transect windows, if open
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub refreshAttrWindows()
        Try
            refreshAttrWindow(MapUtility.findFeatureLayer(My.Settings.Baseline_Feature_Layer, "baseline"))
            refreshAttrWindow(TransectLyrToolCtl.currentTransectLayer)
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Refresh the attribute windows of the current baseline and transect windows, if open, as well as ArcMap display
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub refreshAll()
        Try
            If m_editor2 IsNot Nothing Then
                m_editor2.Display.Invalidate(Nothing, True, Nothing)
            End If
            refreshAttrWindows()
        Catch
        End Try
    End Sub


    Public Shared Function isInEditSession() As Boolean
        Try
            Return My.ArcMap.Editor.EditState <> esriEditState.esriStateNotEditing
        Catch ex As Exception
            DSASUtility.handleException(ex)
        End Try
    End Function


    Public Shared ReadOnly Property SnapTolerance() As Double
        Get
            Try
                'DirectCast(m_editor, ISnapEnvironment).SnapToleranceUnits = esriSnapToleranceUnits.esriSnapToleranceMapUnits
                Return DirectCast(m_editor2, ISnapEnvironment).SnapTolerance
            Finally
            End Try

            Return 7
        End Get
    End Property

End Class


