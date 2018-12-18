Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Carto


Public Class ShorelineForecastForm

    Enum ForecastTimePeriod
        TenYears = 10
        TwentyYears = 20
        TenAndTwentyYears = 30
    End Enum

    Private Sub ShorelineForcasForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initRateLyrCombobox()
        initRadioButtons()
    End Sub


    Private Sub initRateLyrCombobox()
        For Each lyr As ESRI.ArcGIS.Carto.IFeatureLayer In MapUtility.featureLayers("forecastablerates")
            If MapUtility.findFeatureLayer(DSASUtility.layernameConvert(lyr.Name, "rates", "intersect"), "intersect") IsNot Nothing Then
                listboxRateLayer.Items.Add(lyr.Name)
            End If
        Next
    End Sub

    Private Sub initRadioButtons()

        If My.Settings.Forecast_Time_Period = ForecastTimePeriod.TenYears Then
            checkboxTenYears.Checked = True
        ElseIf My.Settings.Forecast_Time_Period = ForecastTimePeriod.TwentyYears Then
            checkboxTwentyYears.Checked = True
        Else
            checkboxTenYears.Checked = True
            checkboxTwentyYears.Checked = True
        End If

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        If checkboxTenYears.Checked And Not checkboxTwentyYears.Checked Then
            My.Settings.Forecast_Time_Period = ForecastTimePeriod.TenYears
        ElseIf Not checkboxTenYears.Checked And checkboxTwentyYears.Checked Then
            My.Settings.Forecast_Time_Period = ForecastTimePeriod.TwentyYears
        Else
            My.Settings.Forecast_Time_Period = ForecastTimePeriod.TenAndTwentyYears
        End If

        Me.Close()

    End Sub

    Private Sub btnForcasHelp_Click(sender As Object, e As EventArgs) Handles btnForecastTimeHelp.Click
        MsgBox("Kalman filter results will be provided as a polyline representing the forecasted shoreline position at either 10 or 20 years (from run date), or two lines if both are selected. The uncertainty for the selected forecast position(s) will be provided as a transparent polygon. For more information on the Kalman filter approach please refer to the user-guide.",, "Select Forecast Time Period(s)")
    End Sub

    Private Sub btnForcasLayerHelp_Click(sender As Object, e As EventArgs) Handles btnForecastLayerHelp.Click
        MsgBox("Forecasting with Kalman Filter" & vbCrLf & vbCrLf &
            "To run the model, select a set of rates. In order to run shoreline forecasting" & vbCrLf & vbCrLf &
             vbTab & "• Rate file must include LRR, LCI, and LSE." & vbCrLf &
             vbTab & "• Intersects file must be added to ArcMap project." & vbCrLf & vbCrLf &
            "The statistically-based Kalman filter requires a set of linear regression rates (LRR) to initialize values in the model to more accurately forecast a future shoreline position and uncertainty. The Kalman filter combines observed and model-derived shoreline positions to predict a future shoreline position (10 years or 20 years from run date)." & vbCrLf & vbCrLf &
            "This method also calculates an estimated uncertainty of the forecast which takes into account measurement noise, process noise, and the magnitude of mismatch between the model and data at each historical shoreline position. Please refer to the user guide for more information.",,
            "Select Rate Data Layer")
    End Sub


    Private Function rateLyrNameToForecastLyrName(rateLyrName As String, period As Integer, Optional uncy As Boolean = False) As String
        Return rateLyrName.Replace("_rates_", "_") & "_forecast" & IIf(uncy, "_uncy", "") & "_" & period
    End Function


    Private xectCursor As IFeatureCursor
    Dim xectFeat As IFeature

    Private Sub btnRunForecast_Click(sender As Object, e As EventArgs) Handles btnRunForecast.Click
        btnRunForecast.Enabled = False

        DoNotShowAgainForm.prompt("PLEASE NOTE: This BETA forecasting tool will not be ideal for all locations, data types and patterns of shoreline change. It is up to the user to consider the specifications and limitations of their data when using this tool to determine a forecasted shoreline position.", "DoNotShowAgainForecastCaveat")

        Dim stepPro As ESRI.ArcGIS.esriSystem.IStepProgressor = Nothing
        Dim cn As OleDb.OleDbConnection = Nothing

        Try
            Dim forecastLyrName As String = listboxRateLayer.SelectedItem
            If forecastLyrName.Length > 52 - "_forecast_uncy_XX".Length + "_rates".Length Then
                log(TraceLevel.Error, "The layer you selected will result in forecast layer names longer than the limit. Transect layer name portion of your selected layer cannot be longer than 19 characters.")
                Return
            End If
            Dim data_forecast_periods As New IronPython.Runtime.List
            If checkboxTenYears.Checked Then data_forecast_periods.Add(10)
            If checkboxTwentyYears.Checked Then data_forecast_periods.Add(20)

            Dim forecastLyrSource As String = GeoDB.getSourceTableName(forecastLyrName)
            Dim isectLyrName As String = DSASUtility.layernameConvert(forecastLyrName, "rates", "intersect")
            Dim isectLyrSource As String = GeoDB.getSourceTableName(isectLyrName)

            ' Find the "distance" field
            Dim distanceFldName As String = "distance"
            Dim uncyFldName As String = "uncertainty"
            Dim isectLyr As IFeatureClass = MapUtility.findFeatureLayer(isectLyrName).FeatureClass
            Dim applyingBias As Boolean = False
            If isectLyr.FindField("bias_distance") > -1 AndAlso isectLyr.FindField("bias_uncertainty") > -1 Then
                applyingBias = True
                distanceFldName = "bias_distance"
                uncyFldName = "bias_uncertainty"
            End If

            ' Find the LCI field
            Dim lciFldName As String = Nothing
            Dim forecastableLyr As IFeatureLayer = MapUtility.findFeatureLayer(forecastLyrName)
            Dim flds = forecastableLyr.FeatureClass.Fields
            For i As Integer = 0 To flds.FieldCount - 1
                If flds.Field(i).Name.ToUpper.StartsWith("LCI") Then
                    lciFldName = flds.Field(i).Name
                    Exit For
                End If
            Next

            stepPro = MapUtility.InitStepProgressor(DSAS.statusBar, 1, GeoDB.getFldExtreme(forecastableLyr.FeatureClass, "TransOrder", True), 1)

            Dim sql As String = "SELECT f.TransectID, f.TransOrder, f.GroupID, f.TCD, i.ShorelineID, " &
                " i.distance, " & IIf(applyingBias, "i.bias_distance, ", "") &
                " i.uncertainty, " & IIf(applyingBias, "i.bias_uncertainty, ", "") &
                " f.LRR, f.LSE, f.[" & lciFldName & "] AS LCI " &
                " FROM [" & forecastLyrSource & "] AS f, [" & isectLyrSource & "] AS i " &
                " WHERE f.TransectID = i.TransectID " &
                " And f.LRR Is Not NULL And f.LSE <> 0 And f.[" & lciFldName & "] <> 0 " &
                " And f.TransectID IN (SELECT TransectID FROM (SELECT TransectID, COUNT(*) FROM [" & isectLyrSource & "] GROUP BY TransectID HAVING COUNT(*) >= 4))" &
                " ORDER BY f.TransOrder, CDate(i.ShorelineID)"

            cn = MapUtility.dbConnectionForLayer(forecastLyrName)

            Dim noBiasAction As String = ""
            Dim avgBias As Double = Nothing

            If applyingBias Then
                Dim nullCountSql As String = "SELECT COUNT(*) AS cnt FROM (" & sql & ") AS t WHERE bias_distance IS NULL OR bias_uncertainty IS NULL"
                Dim cmdNullCount As New OleDb.OleDbCommand(nullCountSql, cn)
                Dim drNullCount As OleDb.OleDbDataReader = cmdNullCount.ExecuteReader
                drNullCount.Read()

                If drNullCount("cnt") > 0 Then
                    drNullCount.Close()
                    Dim frm As New ForecastingBiasOptionsForm
                    frm.ShowDialog()
                    noBiasAction = frm.result
                    If noBiasAction = "stop" Then
                        Exit Sub
                    ElseIf noBiasAction = "continue" Then
                        Try
                            Dim avgBiasSql As String = "SELECT AVG(bias) AS avg_bias FROM " & My.Settings.Shoreline_Uncertainty_Table & " WHERE bias IS NOT NULL"
                            Dim cmdAvgBias As New OleDb.OleDbCommand(avgBiasSql, cn)
                            Dim drAvgBias As OleDb.OleDbDataReader = cmdAvgBias.ExecuteReader
                            drAvgBias.Read()
                            avgBias = drAvgBias("avg_bias")
                            drAvgBias.Close()
                        Catch ex As Exception
                            handleException(ex)
                            Exit Sub
                        End Try
                    End If
                End If
            End If

            Dim cmd As New OleDb.OleDbCommand(sql, cn)
            Dim dr As OleDb.OleDbDataReader = cmd.ExecuteReader

            Dim data As New IronPython.Runtime.PythonDictionary
            Dim data_dates As New IronPython.Runtime.List
            Dim data_shore As New IronPython.Runtime.List
            Dim data_uncy As New IronPython.Runtime.List

            ' We will need to retrieve transect geometry for each transect we process 
            ' in order to determine forecast points from forecast distances
            xectCursor = GeoDB.getCursorForLayer(forecastLyrName, "TransOrder")
            xectFeat = xectCursor.NextFeature

            Dim forecast_geoms As New IronPython.Runtime.PythonDictionary
            Dim forecast_fcs As New IronPython.Runtime.PythonDictionary

            For Each period As Integer In data_forecast_periods
                forecast_geoms.Add(period, New IronPython.Runtime.PythonDictionary)

                forecast_geoms(period).add("best", New PolylineClass)
                forecast_geoms(period).add("mid", New PolylineClass)
                forecast_geoms(period).add("worst", New PolylineClass)

                'Add fcs
                Dim forecast_period_fcs As New IronPython.Runtime.PythonDictionary

                Dim forecastUncyFc As IFeatureClass = setupForecastLayer(forecastLyrName, period, True)
                If forecastUncyFc Is Nothing Then Exit Sub
                forecast_period_fcs.Add("forecast_uncy", forecastUncyFc)

                Dim forecastFc As IFeatureClass = setupForecastLayer(forecastLyrName, period, False)
                If forecastFc Is Nothing Then Exit Sub
                forecast_period_fcs.Add("forecast", forecastFc)

                forecast_fcs.Add(period, forecast_period_fcs)
            Next

            Dim forecastOutput As IFeatureClass = GeoDB.CreateWorkspaceFeatureClassAndAddToTOC(DSASUtility.layernameConvert(forecastLyrName, "rates", "forecast_points"), "forecast_points", Nothing, forecastLyrName)
            If forecastOutput Is Nothing Then Exit Sub

            Dim done As Boolean = False
            Dim xectOrder As Integer = -1
            Dim xectId As Integer = -1
            Dim groupId As Integer = -1
            Dim tcd As Double = 0
            Dim skipThisTransect As Boolean = False

            Do
                done = Not dr.Read()
                If done OrElse dr("TransOrder") <> xectOrder Then
                    Try
                        If stepPro IsNot Nothing Then
                            stepPro.Position = Math.Max(0, xectOrder)
                            System.Windows.Forms.Application.DoEvents()
                        End If
                    Catch ex As Exception
                    End Try

                    If data_dates.__len__ >= 4 Then
                        Try
                            log("TransOrder: " & xectOrder)
                            If skipThisTransect Then
                                log("Skipping transect...")
                                skipThisTransect = False
                                Exit Try
                            End If

                            log("attempting FORECAST...")

                            data("dates") = data_dates
                            data("shore") = data_shore
                            data("uncy") = data_uncy
                            data("forecast_length") = data_forecast_periods

                            Dim forecasted_data = forecast(data)
                            For Each period_forecast As IronPython.Runtime.List In forecasted_data
                                Dim period = period_forecast(0)
                                Dim cal_year = period_forecast(1)
                                Dim worstcase = period_forecast(2)
                                Dim midcase = period_forecast(3)
                                Dim bestcase = period_forecast(4)

                                Dim bestcasePt As IPoint = getPointAlongTransectUsingDistance(xectOrder, bestcase)
                                Dim midcasePt As IPoint = getPointAlongTransectUsingDistance(xectOrder, midcase)
                                Dim worstcasePt As IPoint = getPointAlongTransectUsingDistance(xectOrder, worstcase)

                                forecast_geoms(period).add("yr", cal_year)

                                Dim b As IPointCollection = forecast_geoms(period)("best")
                                b.AddPoint(bestcasePt)

                                Dim m As IPointCollection = forecast_geoms(period)("mid")
                                m.AddPoint(midcasePt)

                                ' Add record to forecast table - TODO: double check values passed in
                                addForecastTableRecord(forecastOutput, period, cal_year, Math.Abs(midcase - bestcase), midcase, midcasePt, xectId, xectOrder, tcd)

                                Dim w As IPointCollection = forecast_geoms(period)("worst")
                                If w.PointCount = 0 Then
                                    w.AddPoint(worstcasePt)
                                Else
                                    w.AddPoint(worstcasePt, 0)
                                End If

                            Next
                        Catch ex As Exception
                            ' For debugging during dev...
                            Dim c = Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented)
                            log(TraceLevel.Error, c)
                        Finally
                            ' Clear data from previous transect
                            data.__init__()
                            data_dates.Clear()
                            data_shore.Clear()
                            data_uncy.Clear()
                        End Try
                    End If
                End If

                If done OrElse nv(dr("GroupID"), -99) <> groupId Then
                    addForecastedGeoms(forecast_fcs, forecast_geoms)
                End If

                If done Then Exit Do

                xectOrder = dr("TransOrder")
                xectId = dr("TransectID")
                groupId = nv(dr("GroupID"), -99)

                If dr("TCD") - tcd > My.Settings.Spacing * 2.5 Then
                    addForecastedGeoms(forecast_fcs, forecast_geoms)
                End If

                tcd = dr("TCD")

                data_dates.Add(DSASUtility.DecimalYear(CDate(dr("ShorelineID"))))
                Dim distance = dr(distanceFldName)
                Dim uncertainty = dr(uncyFldName)
                If applyingBias Then
                    If nv(distance, Nothing) Is Nothing OrElse nv(uncertainty, Nothing) Is Nothing Then
                        If noBiasAction = "continue" Then
                            distance = dr("distance") + avgBias
                            uncertainty = dr("uncertainty") + avgBias
                        ElseIf noBiasAction = "skip" Then
                            skipThisTransect = True
                        Else
                        End If
                    End If
                End If
                data_shore.Add(distance)
                data_uncy.Add(uncertainty)
                data("LRR") = dr("LRR")
                data("LCI") = dr("LCI")
                data("LSE") = dr("LSE")
            Loop

            For Each period As Integer In data_forecast_periods
                For Each typ In DirectCast(forecast_fcs(period), IronPython.Runtime.PythonDictionary).viewkeys
                    Dim ifc As IFeatureClass = forecast_fcs(period)(typ)
                    Metadata.createMetadata(ifc.AliasName, typ, New Dictionary(Of String, String) From {{"forecastPeriod", period}})
                    ReleaseComObject(forecast_fcs(period)(typ))
                Next
            Next

            Metadata.createMetadata(forecastOutput.AliasName, "forecast_points", New Dictionary(Of String, String) From {{"forecastPeriods", String.Join(", ", data_forecast_periods)}})

        Finally
            btnRunForecast.Enabled = True
            If stepPro IsNot Nothing Then stepPro.Hide()
            My.Document.CurrentContentsView.Refresh(Nothing)
            My.Document.ActiveView.Refresh()
            If cn IsNot Nothing Then cn.Close()
            ReleaseComObject(xectCursor)
        End Try
    End Sub


    Function getPointAlongTransectUsingDistance(xectOrder As Integer, dist As Double) As IPoint
        Dim done As Boolean = False

        ' So ugly but in the name of efficiency...
        If xectFeat Is Nothing Then Return Nothing
        While xectFeat.Value(xectCursor.FindField("TransOrder")) < xectOrder
            xectFeat = xectCursor.NextFeature
        End While
        If xectFeat Is Nothing Then Return Nothing

        If xectFeat.Value(xectCursor.FindField("TransOrder")) = xectOrder Then
            Dim tmpGeom As IPointCollection = xectFeat.ShapeCopy
            Dim xectGeom As ICurve = tmpGeom
            ' We remove either most seaward or landward point because we need the distance measurement 
            ' to run from the "mid" point which rests on the baseline.
            ' We prefer to keep the point that is farther away from the mid point as the other point may be coincident
            ' and may result in "line" collapsed to a point which would make it impossible to walk a distance along.
            If MapUtility.GetDistanceBetweenTwoPoints(tmpGeom.Point(0), tmpGeom.Point(1)) < MapUtility.GetDistanceBetweenTwoPoints(tmpGeom.Point(1), tmpGeom.Point(2)) Then
                ' Remove most seaward point of the transect
                tmpGeom.RemovePoints(0, 1)
                ' Flip distance as transect direction implies opposite of erosion/accretion
                dist = -dist
            Else
                ' Remove most landward point of the transect    
                tmpGeom.RemovePoints(2, 1)
                ' Flipping the geometry around so that the baseline point is the beginning of the line
                ' Since we are flipping the line we do not need to flip distance
                xectGeom.ReverseOrientation()
            End If
            Dim pt As IPoint = New PointClass
            xectGeom.QueryPoint(esriSegmentExtension.esriExtendTangents, dist, False, pt)
            Return pt
        End If

        Return Nothing  ' We should never get here as it means we couldn't find the transect
    End Function

    ''' <summary>
    ''' Add the polyline and polygon features to forecast layers
    ''' </summary>
    ''' <param name="forecast_fcs"></param>
    ''' <param name="forecast_geoms"></param>
    Sub addForecastedGeoms(forecast_fcs As IronPython.Runtime.PythonDictionary, forecast_geoms As IronPython.Runtime.PythonDictionary)
        For Each period As Integer In forecast_geoms.viewkeys
            Dim forecastedFc As IFeatureClass = forecast_fcs(period)("forecast")
            Dim forecastedRangeFc As IFeatureClass = forecast_fcs(period)("forecast_uncy")
            Dim m As IPolyline = forecast_geoms(period)("mid")
            ' Loop if nothing to do
            If m.IsEmpty Then Continue For

            ' Write the polyline forecast
            writeForecastedGeomsToFc(forecastedFc, period, forecast_geoms(period)("yr"), m)
            ' Replace with new geometry
            forecast_geoms(period).add("mid", New PolylineClass)

            ' Combine best and worst case forecast polylines into a polygon and write
            Dim b As IPointCollection = forecast_geoms(period)("best")
            Dim w As IPointCollection = forecast_geoms(period)("worst")
            b.AddPointCollection(w)
            writeForecastedGeomsToFc(forecastedRangeFc, period, forecast_geoms(period)("yr"), PolylineToPolygon(b))
            ' Replace with new geometry
            forecast_geoms(period).add("best", New PolylineClass)
            forecast_geoms(period).add("worst", New PolylineClass)
        Next
    End Sub

    ''' <summary>
    ''' Store the forecast geometry to appropriate layer
    ''' </summary>
    ''' <param name="outputFc"></param>
    ''' <param name="period"></param>
    ''' <param name="yr"></param>
    ''' <param name="ln"></param>
    Sub writeForecastedGeomsToFc(outputFc As IFeatureClass, period As Integer, yr As Double, ln As IGeometry)
        Dim feat As IFeature = outputFc.CreateFeature()
        With feat
            .Shape = IIf(ln.GeometryType = esriGeometryType.esriGeometryPolyline, TryCast(ln, IPolyline), TryCast(ln, IPolygon))
            .Value(outputFc.FindField("ForecastPeriod")) = period
            .Value(outputFc.FindField("ForecastYear")) = yr
            ' save the row
            .Store()
        End With
    End Sub

    ''' <summary>
    ''' Create a polygon out of a polyline
    ''' </summary>
    ''' <param name="pPolyline"></param>
    ''' <returns></returns>
    Private Function PolylineToPolygon(ByRef pPolyline As IPolyline) As IGeometryCollection
        ' Create a new Polygon geometry.
        PolylineToPolygon = New Polygon

        Dim pGeoms_Polyline As IGeometryCollection
        pGeoms_Polyline = pPolyline

        ' Transfer the segments from the Polyline Paths to new Rings, and add the Rings
        ' to the new Polygon.
        Dim i As Long, pSegs_Ring As ISegmentCollection
        For i = 0 To pGeoms_Polyline.GeometryCount - 1
            pSegs_Ring = New RingClass
            pSegs_Ring.AddSegmentCollection(pGeoms_Polyline.Geometry(i))
            PolylineToPolygon.AddGeometry(pSegs_Ring)
        Next i

        ' The Polygon may have it's rings oriented incorrectly. We call simplify here to
        ' ensure they are in the correct rotation.
        Dim pPolygon As IPolygon
        pPolygon = PolylineToPolygon
        pPolygon.SimplifyPreserveFromTo()
    End Function


    Private Sub listboxRateLayer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles listboxRateLayer.SelectedIndexChanged, checkboxTenYears.CheckedChanged, checkboxTwentyYears.CheckedChanged
        enableDisableRunButton()
    End Sub

    Sub enableDisableRunButton()
        btnRunForecast.Enabled = (nv(listboxRateLayer.SelectedItem) <> "" AndAlso (checkboxTenYears.Checked OrElse checkboxTwentyYears.Checked))
    End Sub



    Function setupForecastLayer(forecastableLyrName As String, period As Integer, uncy As Boolean) As IFeatureClass
        Dim simpleRenderer As New SimpleRendererClass
        Dim lyrName = rateLyrNameToForecastLyrName(forecastableLyrName, period, uncy)
        Dim lyrTypeName As String
        If uncy Then
            lyrTypeName = "forecast_uncy"
            If period = 10 Then
                simpleRenderer.Symbol = MapUtility.fillSymbol(128, 205, 193)
            ElseIf period = 20 Then
                simpleRenderer.Symbol = MapUtility.fillSymbol(223, 194, 125)
            End If
        Else
            lyrTypeName = "forecast"
            If period = 10 Then
                simpleRenderer.Symbol = MapUtility.lineSymbol(1, 133, 113)
            ElseIf period = 20 Then
                simpleRenderer.Symbol = MapUtility.lineSymbol(166, 97, 26)
            End If
        End If

        Dim fc As IFeatureClass = GeoDB.CreateWorkspaceFeatureClassAndAddToTOC(lyrName, lyrTypeName, Nothing, forecastableLyrName)
        If fc Is Nothing Then Return Nothing

        Dim lyr As IGeoFeatureLayer = MapUtility.findFeatureLayer(lyrName)
        If simpleRenderer.Symbol IsNot Nothing Then lyr.Renderer = simpleRenderer
        If uncy Then DirectCast(lyr, ILayerEffects).Transparency = 50

        Return fc
    End Function


    Shared Sub addForecastTableRecord(forecastTable As IFeatureClass, forecastPeriod As Integer, forecastDate As Integer, uncy As Double, distanceToBaseline As Double, midCasePt As IPoint, xectId As Integer, xectOrder As Integer, tcd As Double)
        Try
            Dim forecastRecord As IFeature = forecastTable.CreateFeature
            With forecastRecord
                .Shape = clonePoint(midCasePt)
                .Value(forecastTable.FindField("ForecastPeriod")) = forecastPeriod
                .Value(forecastTable.FindField("ForecastYear")) = forecastDate
                .Value(forecastTable.FindField("Uncertainty")) = uncy
                .Value(forecastTable.FindField("Distance")) = distanceToBaseline
                .Value(forecastTable.FindField("IntersectX")) = midCasePt.X
                .Value(forecastTable.FindField("IntersectY")) = midCasePt.Y
                .Value(forecastTable.FindField("TransectID")) = xectId
                .Value(forecastTable.FindField("TransOrder")) = xectOrder
                .Value(forecastTable.FindField("TCD")) = tcd

                ' save the row
                .Store()
            End With
        Catch ex As Exception
            handleException(ex)
            Throw ex
        End Try
    End Sub


End Class


