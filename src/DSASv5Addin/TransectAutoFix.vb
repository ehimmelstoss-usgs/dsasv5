Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.ArcMapUI


Public Class TransectAutoFix
    Inherits ESRI.ArcGIS.Desktop.AddIns.Extension

    'Declare the editor and the edit event interface.
    Public Shared m_editor As IEditor
    Private Shared WithEvents m_editEvents As IEditEvents_Event
    Private Shared WithEvents m_editEvents2 As IEditEvents2_Event
    Private Shared userInitiatedSaveInProgress As Boolean = False


    Protected Overrides Sub OnStartup()
        m_editor = My.ArcMap.Editor
        m_editEvents = CType(m_editor, IEditEvents_Event)
        m_editEvents2 = CType(m_editor, IEditEvents2_Event)

        ''Wire editor events.
        AddHandler m_editEvents.OnStartEditing, AddressOf m_editEvents_OnStartEditing
        AddHandler m_editEvents.OnStopEditing, AddressOf m_editEvents_OnStopEditing
        AddHandler m_editEvents2.OnSaveEdits, AddressOf m_editEvents2_OnSaveEdits
    End Sub

    Protected Overrides Sub OnShutdown()
        RemoveHandler m_editEvents.OnStartEditing, AddressOf m_editEvents_OnStartEditing
        RemoveHandler m_editEvents.OnStopEditing, AddressOf m_editEvents_OnStopEditing
        RemoveHandler m_editEvents2.OnSaveEdits, AddressOf m_editEvents2_OnSaveEdits
        m_editEvents = Nothing
        m_editEvents2 = Nothing
    End Sub


    Private Sub m_editEvents_OnStartEditing()
        log("Starting edit session")
        log("Selected transect layer is: " & nv(TransectLyrToolCtl.currentTransectLayerName, "NONE SELECTED"))
        If TransectLyrToolCtl.currentTransectLayer IsNot Nothing Then
            'Try
            '    DSAS.seaward = getSeawardLandwardSettingFromMetadata(TransectLyrToolCtl.currentTransectLayer)
            '    If DSAS.seaward Is Nothing Then
            '        Dim msg As String = String.Format("Transect layer selected '{0}' does not have a landward/seaward setting recorded in the process steps of the metadata record (Data Quality - Process Step - Process Description). Since this setting is a new feature recorded during DSAS v5.0 transect creation, this information will be missing for legacy transects even after they are upgraded. DSAS defaults to the setting currently selected in the Default Parameters user interface which is '{1}' for this calculation.", TransectLyrToolCtl.currentTransectLayerName, IIf(My.Settings.Seaward, "seaward", "landward"))
            '        log(msg)
            '        MsgBox(msg,, "DSAS Alert")
            '        DSAS.seaward = My.Settings.Seaward
            '    End If
            'Catch ex As Exception
            'End Try

            wireEvents()
        End If
    End Sub


    Public Shared Sub wireEvents()
        Try
            Dim baselineLayer As IFeatureLayer = MapUtility.findFeatureLayer(My.Settings.Baseline_Feature_Layer, "baseline")
            If baselineLayer Is Nothing Then
                DSASUtility.log(TraceLevel.Error,
                "Please make sure that your default settings for baseline layer are correct." + vbCrLf +
                "DSAS topology support will be offline during this edit session.")
                Return
            End If

            AddHandler m_editEvents.OnSketchModified, AddressOf markAsEdited
            AddHandler m_editEvents.OnCreateFeature, AddressOf markAsEdited
        Catch ex As Exception
            DSASUtility.handleException(ex)
        End Try
    End Sub


    Private Sub m_editEvents_OnStopEditing(ByVal save As Boolean)
        If userInitiatedSaveInProgress Then
            userInitiatedSaveInProgress = False
            Return
        End If
        unwireEvents()
        log("Stopped edit session")
        fixNewOrEditedTransects()
    End Sub


    Private Sub m_editEvents2_OnSaveEdits()
        userInitiatedSaveInProgress = True
    End Sub


    Public Shared Sub unwireEvents()
        If m_editEvents IsNot Nothing Then
            RemoveHandler m_editEvents.OnSketchModified, AddressOf markAsEdited
            RemoveHandler m_editEvents.OnCreateFeature, AddressOf markAsEdited
        End If
    End Sub


    Sub fixNewOrEditedTransects()
        If GeoDB.checkForTransOrderAnomaly(TransectLyrToolCtl.currentTransectLayerName) > 0 Then
            RateCalculation.sortAlongBaseline()
        End If
        Dim issues As New IronPython.Runtime.PythonDictionary()
        issues.Add("Transects with no baseline intersection", New IronPython.Runtime.List())
        issues.Add("Transects with a missing vertex", New IronPython.Runtime.List())

        Dim blFc As IFeatureClass = MapUtility.findFeatureLayer(My.Settings.Baseline_Feature_Layer, "baseline").FeatureClass
        For Each xectFeat As IFeature In GeoDB.features(TransectLyrToolCtl.currentTransectLayer.FeatureClass, "TransOrder")
            Dim transEdit = xectFeat.Value(xectFeat.Fields.FindField("TransEdit"))
            Dim transId = xectFeat.OID
            If transEdit Is DBNull.Value OrElse transEdit <> 0 Then
                fixNewOrEditedTransect(xectFeat, blFc, issues)
            End If
        Next
        blFc = Nothing

        refreshAll()

        Dim issueStr As String = "You  have some manually edited transects with potential problems:" + vbCrLf + vbCrLf
        Dim issuesEncountered As Boolean = False
        For Each issue As String In issues.viewkeys
            Dim issueList As IronPython.Runtime.List = issues(issue)
            If issueList.__len__ > 0 Then
                If Not issuesEncountered Then
                    issuesEncountered = True
                    log("You  have some manually edited transects with potential problems (listed numbers are transect OIDs):")
                End If
                log(issue & ":")
                log(String.Join(", ", issueList))
                issueStr = String.Format("{0}{1}: {2} transects{3}{3}", issueStr, issue, issueList.__len__, vbCrLf)
            End If
        Next

        If issuesEncountered Then
            issueStr = String.Format("{0}{1}", issueStr, "Please refer to the log file for OIDs of transects with issues.")
            MsgBox(issueStr,, DSAS.MsgBoxTitle)
        End If
    End Sub



    Private Shared Sub markAsEdited(Optional o As IObject = Nothing)
        Dim xectFeat As IFeature = TryCast(o, IFeature)
        If xectFeat Is Nothing Then xectFeat = getSelectedTransectFeature()
        If xectFeat Is Nothing Then Return
        xectFeat.Value(xectFeat.Fields.FindField("TransEdit")) = 1
        refreshAttrWindow(TransectLyrToolCtl.currentTransectLayer)
    End Sub


    Private Shared Sub recordIssue(issues As IronPython.Runtime.PythonDictionary, issueType As String, data As Object)
        Dim listOfObjectsWithIssue As IronPython.Runtime.List = issues(issueType)
        listOfObjectsWithIssue.append(data)
    End Sub


    Private Shared Sub fixNewOrEditedTransect(xectFeat As IFeature, blFc As IFeatureClass, issues As IronPython.Runtime.PythonDictionary)
        Try
            If xectFeat Is Nothing Then Return
            Dim vertices As IPointCollection = xectFeat.Shape
            Dim transid = xectFeat.OID
            ' Sanity check, can this even happen?
            If vertices.PointCount < 2 Then
                recordIssue(issues, "Transects with a missing vertex", xectFeat.OID)
                Return
            End If

            ' Too many vertices, fix it up
            If vertices.PointCount > 2 Then
                ' Remove all but from/to points
                vertices.RemovePoints(1, vertices.PointCount - 2)
            End If
            ' Create baseline/mid point
            Dim midPt As IPoint
            midPt = New PointClass
            Dim ln As IPolyline = New PolylineClass
            ln.FromPoint = clonePoint(vertices.Point(0))
            ln.ToPoint = clonePoint(vertices.Point(1))
            'For now, make midPt the middle of the transect which we will change later
            ln.QueryPoint(esriSegmentExtension.esriNoExtension, 0.5, True, midPt)

            Dim blAnchor As IPointCollection = Nothing
            Dim ito As ITopologicalOperator = ln
            For Each blFeat As IFeature In GeoDB.features(blFc)
                blAnchor = ito.Intersect(blFeat.ShapeCopy, esriGeometryDimension.esriGeometry0Dimension)
                If blAnchor IsNot Nothing AndAlso blAnchor.PointCount > 0 Then
                    vertices.AddPoint(blAnchor.Point(0), 1)
                    flipTransectIfNeeded(vertices, blFeat.Shape)
                    Dim azimuth As Double = round(DSASUtility.CalculateAzimuth(vertices))
                    xectFeat.Value(xectFeat.Fields.FindField("Azimuth")) = azimuth
                    copyBaselineAttrs2Xect(blFeat, xectFeat)

                    Exit For
                End If
            Next
            If vertices.PointCount <> 3 Then
                recordIssue(issues, "Transects with no baseline intersection", xectFeat.OID)
            End If

            xectFeat.Store()
        Catch ex As Exception
            Dim x = 1
        End Try
    End Sub

    ''' <summary>
    ''' Determine if two points can be considered coincident based on distance between them
    ''' </summary>
    ''' <param name="p1"></param>
    ''' <param name="p2"></param>
    ''' <returns></returns>
    Public Shared Function coincident(p1 As IPoint, p2 As IPoint) As Boolean
        Return MapUtility.GetDistanceBetweenTwoPoints(p1, p2) < DSAS.REP_ERR
    End Function

    ''' <summary>
    ''' Check where a transects from/to points lie wrt to the baseline and compare to the land orientation to determine if a transect should be flipped
    ''' </summary>
    ''' <param name="xectGeom"></param>
    ''' <param name="blGeom"></param>
    Private Shared Sub flipTransectIfNeeded(xectGeom As IPolycurve, blGeom As IPolyline)
        Dim xectPoints As IPointCollection = xectGeom
        Dim ht As IHitTest = DirectCast(blGeom, IHitTest)
        Dim transectPointsRight As Boolean

        Dim bRightSideTo As Boolean
        ht.HitTest(xectGeom.ToPoint, xectGeom.Length * 1.01, esriGeometryHitPartType.esriGeometryPartBoundary, Nothing, Nothing, Nothing, Nothing, bRightSideTo)
        If bRightSideTo AndAlso Not coincident(xectGeom.ToPoint, xectPoints.Point(1)) Then
            transectPointsRight = True
        Else
            Dim bRightSideFrom As Boolean
            ht.HitTest(xectGeom.FromPoint, xectGeom.Length * 1.01, esriGeometryHitPartType.esriGeometryPartBoundary, Nothing, Nothing, Nothing, Nothing, bRightSideFrom)
            If Not bRightSideFrom AndAlso Not coincident(xectGeom.FromPoint, xectPoints.Point(1)) Then transectPointsRight = True
        End If


        If (My.Settings.Land_On_Right_Side AndAlso transectPointsRight) OrElse (Not My.Settings.Land_On_Right_Side AndAlso Not transectPointsRight) Then
            ' Orientation correct: Sea-to-land
        Else
            ' Flip the transect
            xectGeom.ReverseOrientation()
        End If
    End Sub


    Private Shared Sub copyBaselineAttrs2Xect(ByVal bl As IFeature, ByVal xect As IFeature)
        Dim blGroupFldIdx As Integer = GeoDB.baseFldMap("group")
        Dim blIdFldIdx As Integer = GeoDB.baseFldMap("ID")

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


    Private Shared Function getSelectedTransectFeature() As IFeature
        getSelectedTransectFeature = Nothing
        If TransectLyrToolCtl.currentTransectLayer Is Nothing Then Exit Function
        If My.Document.FocusMap.SelectionCount <> 1 Then Exit Function
        Dim enumFeature As IEnumFeature = My.Document.FocusMap.FeatureSelection
        Dim cFeature As IFeature = enumFeature.Next
        If cFeature Is Nothing OrElse cFeature.Class.AliasName <> TransectLyrToolCtl.currentTransectLayerName Then Exit Function
        getSelectedTransectFeature = cFeature
    End Function


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
        End Try
    End Sub



    ''' <summary>
    ''' Refresh the attribute windows, if open, as well as ArcMap display
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub refreshAll()
        Try
            'If m_editor2 IsNot Nothing Then
            '    m_editor2.Display.Invalidate(Nothing, True, esriViewDrawPhase.esriViewAll)
            'End If
            My.Document.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, Nothing, Nothing)
            refreshAttrWindow(TransectLyrToolCtl.currentTransectLayer)
        Catch
        End Try
    End Sub


End Class
