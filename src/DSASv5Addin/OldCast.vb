Option Explicit On
Option Strict On

Imports System.Runtime.InteropServices
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem


Module OldCast

    Sub cast(ByVal frm As CastTransectsForm)
        'declare variables
        Dim baselineFeatCursor As IFeatureCursor = Nothing
        Dim baselineFeat As IFeature = Nothing
        'Dim featLyr As IFeatureLayer
        Dim xectGeom As IPolyline = Nothing
        Dim baselineCurve As ICurve = Nothing
        Dim cumulativeBaselineCurve As ICurve = Nothing
        Dim dAlong As Double = 0
        Dim baseFeatLayer As IFeatureLayer = Nothing
        Dim shoreFeatLayer As IFeatureLayer = Nothing


        'declare variable to hold no of feature selected in base layer
        Dim selBaseCount As Integer
        '************************************
        'display progress bar
        Dim stepPro As IStepProgressor = Nothing
        Dim xectLayer As IFeatureLayer = Nothing

        Try
            DSAS.autogen = 1
            ' If we are unable to validate cast settings
            If Not SetDefaultsForm.validateSettings("cast", True) Then
                Exit Try
            End If

            DSAS.seaward = My.Settings.Seaward

            GeoDB.init()

            DSASUtility.TraceSettings()

            baseFeatLayer = MapUtility.findFeatureLayer(My.Settings.Baseline_Feature_Layer, "baseline")
            If Not GeoDB.layerIsValid(baseFeatLayer, "baseline") Then Exit Try

            shoreFeatLayer = MapUtility.findFeatureLayer(My.Settings.Shoreline_Feature_Layer, "shoreline")
            If Not GeoDB.layerIsValid(shoreFeatLayer, "shoreline") Then Exit Try

            'check if linear unit of baseline feature is meter
            If Not (MapUtility.isLinearUnitMeter(baseFeatLayer.FeatureClass)) Then
                log(TraceLevel.Error, "Baseline layer is either not projected or the projection does not use " _
                + "Meter as linear unit! " _
                + "Edit projection for " + baseFeatLayer.Name)
                Exit Try
            End If

            If Not DSASUtility.getSpaRef(baseFeatLayer.FeatureClass) = DSASUtility.getSpaRef(shoreFeatLayer.FeatureClass) Then
                log(TraceLevel.Error, "Baseline and shoreline layers must be in the same coordinate system.")
                Exit Try
            End If


            'get the selected baseline features
            DSASUtility.log(TraceLevel.Verbose, "Retrieving selected features from baseline...")
            selBaseCount = MapUtility.GetSelectedCount(baseFeatLayer)
            'select all features if no selected feature
            'found in the base layer
            If (selBaseCount > 0) Then
                baselineFeatCursor = MapUtility.GetSelectedFeatureCursor(baseFeatLayer)
            Else
                baselineFeatCursor = baseFeatLayer.Search(Nothing, False)
                selBaseCount = baseFeatLayer.FeatureClass.FeatureCount(Nothing)
            End If
            'iterate through selected baseline features

            'sort input baselines
            Dim newBaseFeat As New List(Of IFeature)
            newBaseFeat = DSASUtility.SortBaselineFeatures(baselineFeatCursor)
            If newBaseFeat Is Nothing Then Exit Try
            If newBaseFeat.Count < 1 Then
                log(TraceLevel.Error, "No Baselines with ID > 0 Detected")
                Exit Try
            End If

            ' Set up featureclass and tables, exit if unsuccesful
            Dim xectFc As IFeatureClass = GeoDB.CreateWorkspaceFeatureClassAndAddToTOC(frm.m_transectName, "transect", frm.txtPersonalDb.Text, Nothing)
            If xectFc Is Nothing Then Exit Try
            xectLayer = MapUtility.findFeatureLayer(frm.m_transectName, "transect")

            If DSAS.createRecordOfCastSmoothing Then DSAS.record = GeoDB.CreateWorkspaceFeatureClassAndAddToTOC("record_" + frm.m_transectName, "record", frm.txtPersonalDb.Text, Nothing)

            'add feature to the transect feature layer
            DSASUtility.log(TraceLevel.Verbose, "Add features to transect feature class...")
            'display progress bar
            stepPro = MapUtility.InitStepProgressor(DSAS.statusBar, 1, newBaseFeat.Count, 1)
            'start a transaction
            GeoDB.StartTransaction(xectLayer)
            'loop through the baseline feature cursor
            Dim i As Integer

            Dim transectID As Integer = 1

            Dim totalLength As Double = 0
            For i = 0 To newBaseFeat.Count - 1
                baselineFeat = CType(newBaseFeat(i), IFeature)
                baselineCurve = CType(baselineFeat.Shape, ICurve)
                totalLength += baselineCurve.Length
            Next
            If stepPro IsNot Nothing Then stepPro.MaxRange = Math.Max(CInt(totalLength / My.Settings.Spacing) + 1, stepPro.MinRange + 1)

            Dim ptLast As IPoint = Nothing

            Dim baselineDataWithConnectedGeoms As IronPython.Runtime.List = getBaselineDataWithConnectedGeoms(newBaseFeat)
            Dim prevConnectedGeomIndex As Integer = -1
            Dim dLeftOver As Double = 0

            For Each baselineData As IronPython.Runtime.PythonDictionary In baselineDataWithConnectedGeoms
                baselineFeat = CType(baselineData("feature"), IFeature)
                baselineCurve = CType(baselineFeat.Shape, ICurve)
                cumulativeBaselineCurve = CType(baselineData("connectedGeom"), ICurve)

                Dim cumPriorDistance As Double = CType(baselineData("cumPriorDistance"), Double)
                dAlong = cumPriorDistance + dLeftOver

                While dAlong < cumPriorDistance + baselineCurve.Length
                    xectGeom = DSASUtility.CreateNormalLine(CType(cumulativeBaselineCurve, IPolyline), dAlong, getSearchDistance(baselineFeat))

                    If xectGeom IsNot Nothing Then
                        GeoDB.doInsert(xectGeom, CType(baselineData("groupID"), Integer), 0, CType(baselineData("baselineID"), Integer), transectID, xectFc)

                        transectID += 1
                    End If
                    dAlong += My.Settings.Spacing

                    Try
                        If stepPro IsNot Nothing Then
                            stepPro.Step()
                            System.Windows.Forms.Application.DoEvents()
                        End If
                    Catch ex As Exception
                    End Try
                End While
                dLeftOver = dAlong - (cumPriorDistance + baselineCurve.Length)
            Next

            'stop transaction
            GeoDB.StopTransaction(xectLayer)

            ' Create metadata for the transect
            Metadata.createMetadata(frm.m_transectName)

            TransectLyrToolCtl.addSelectLayer(xectLayer)

            'refresh the display
            My.Document.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)

            Dim msg As String = String.Format("Transect Layer '{0}' has been created and is now selected in the DSAS transect layer drop down. If you manually edit transects, please remember to select the layer before starting the edit session so that DSAS can check your edits.", frm.m_transectName)
            log(msg)
            MsgBox(msg,, DSAS.MsgBoxTitle)

        Catch argEx As ArgumentException
            DSASUtility.handleArgException(argEx)
            GeoDB.AbortTransaction(xectLayer)
        Catch comex As COMException
            DSASUtility.handleComException(comex)
            GeoDB.AbortTransaction(xectLayer)
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
            GeoDB.AbortTransaction(xectLayer)
        Finally
            DSAS.autogen = 0
            DSAS.seaward = Nothing
            'hide the progressor
            If stepPro IsNot Nothing Then stepPro.Hide()
            'refresh the display
            My.Document.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
            'release objects
            DSASUtility.ReleaseComObject(baselineFeatCursor)
            DSASUtility.ReleaseComObject(baselineFeat)
            DSASUtility.ReleaseComObject(xectGeom)
            DSASUtility.ReleaseComObject(baselineCurve)
            DSASUtility.ReleaseComObject(cumulativeBaselineCurve)
            DSASUtility.ReleaseComObject(baseFeatLayer)
            GeoDB.uninit()
        End Try
    End Sub


    Function getBaselineDataWithConnectedGeoms(newBaseFeat As List(Of IFeature)) As IronPython.Runtime.List
        Dim connectedBaselineGeoms As New List(Of ICurve)
        Dim connectedBaselineGeom As ICurve = Nothing
        Dim connectedBaselineCounter As Integer = -1

        Dim baselineData As New IronPython.Runtime.List

        Dim baselineFeatCursor As IFeatureCursor = Nothing
        Dim baselineFeat As IFeature = Nothing
        Dim xectGeom As IPolyline = Nothing
        Dim baselineCurve As ICurve = Nothing
        Dim baseFeatLayer As IFeatureLayer = Nothing

        Dim baselineID As Integer '7/28/2005: JZ added to check for the existence of the baseline ID field
        Dim groupID As Long = 0
        Dim transectID As Integer = 1

        Dim baseGroupIdx As Integer = GeoDB.baseFldMap("group")

        Dim ptLast As IPoint = Nothing
        Dim currentBaselineData As IronPython.Runtime.PythonDictionary
        Dim cumPriorDistance As Double = 0

        For i = 0 To newBaseFeat.Count - 1
            baselineFeat = CType(newBaseFeat(i), IFeature)
            baselineCurve = CType(baselineFeat.ShapeCopy, ICurve)

            If baselineCurve Is Nothing OrElse baselineCurve.IsEmpty Then Continue For

            If ptLast Is Nothing Then
                ' We have encountered the first baseline feature
                ptLast = baselineCurve.ToPoint
            Else
                ' Check if the beginning or ending point of the current baseline feature is coincident with the last one's ending point
                If MapUtility.GetDistanceBetweenTwoPoints(baselineCurve.FromPoint, ptLast) > DSAS.REP_ERR Then
                    connectedBaselineGeom = Nothing
                    cumPriorDistance = 0
                End If
                ' Make current baseline's ending point the new reference for next round
                ptLast = baselineCurve.ToPoint
            End If

            If connectedBaselineGeom Is Nothing Then
                'Use current baseline part to seed the cumulative baseline
                connectedBaselineGeom = baselineCurve
                connectedBaselineGeoms.Add(connectedBaselineGeom)
                connectedBaselineCounter += 1
            Else
                'Append current baseline part to the cumulative baseline
                Dim part1 As ISegmentCollection = CType(connectedBaselineGeom, ISegmentCollection)
                Dim part2 As ISegmentCollection = CType(baselineCurve, ISegmentCollection)
                part1.AddSegmentCollection(part2)
            End If


            baselineID = CInt(baselineFeat.Value(GeoDB.baseFldMap("ID")))
            If baseGroupIdx > -1 Then
                groupID = CType(IIf(TypeOf baselineFeat.Value(baseGroupIdx) Is DBNull, 0, baselineFeat.Value(baseGroupIdx)), Long)
            End If

            currentBaselineData = New IronPython.Runtime.PythonDictionary
            currentBaselineData.Add("baselineID", baselineID)
            currentBaselineData.Add("groupID", groupID)
            currentBaselineData.Add("feature", baselineFeat)
            currentBaselineData.Add("connectedGeomIndex", connectedBaselineCounter)
            currentBaselineData.Add("cumPriorDistance", cumPriorDistance)
            baselineData.Add(currentBaselineData)

            cumPriorDistance += baselineCurve.Length
        Next i

        For Each currentBaselineData In baselineData
            currentBaselineData.Add("connectedGeom", connectedBaselineGeoms(CType(currentBaselineData("connectedGeomIndex"), Integer)))
        Next

        Return baselineData
    End Function

End Module
