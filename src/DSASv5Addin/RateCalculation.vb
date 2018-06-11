Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Carto
Imports System.Math
Imports System.Data.OleDb
Imports ESRI.ArcGIS.Display
Imports Humanizer
Imports ESRI.ArcGIS.esriSystem

Public Class RateCalculation
    Shared haveShorelineTypeField As Boolean = False
    Shared haveShorelineUncertaintyTable As Boolean = False


    Class IntersectData
        Shared haveUncTable As Boolean
        Shared haveUncField As Boolean
        Shared haveUncBiasFields As Boolean
        Shared haveUncDefault As Boolean

        Shared uncTableName As String
        Shared uncyFldIdx As Integer
        Shared biasFldIdx As Integer
        Shared uncbFldIdx As Integer

        Public uncy As Double = -1   'invalid value
        Public bias As Double? = -1   'invalid value
        Public uncb As Double? = -1   'invalid value

        Public isValid As Boolean = False
        Public shoreDateStr As String
        Public shoreType As String = ""
        Public xectId As Integer
        Public blId As Integer
        Public m As Double = -1   'invalid value
        Public x As Double
        Public y As Double
        Public dFromAnchor As Double

        Public Shared cn As OleDbConnection

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks>Call Geodb.isLayerValid() on the shoreline layer before calling this.</remarks>
        Public Shared Sub determineUncertaintySources()
            DSASUtility.log("Determining sources of uncertainty info...")

            ' See if we can find LIDAR data for the shoreline layer
            Dim shoreLyrName As String = My.Settings.Shoreline_Feature_Layer
            Try
                uncTableName = My.Settings.Shoreline_Uncertainty_Table
                DSASUtility.log("Attempting to detect LIDAR data for shoreline layer: " + shoreLyrName)
                If GeoDB.checkIfTableFieldExists(uncTableName, Nothing) AndAlso
                    GeoDB.checkIfTableFieldExists(uncTableName, "ID") AndAlso
                    GeoDB.checkIfTableFieldExists(uncTableName, "UNCY") AndAlso
                    GeoDB.checkIfTableFieldExists(uncTableName, "BIAS") AndAlso
                    GeoDB.checkIfTableFieldExists(uncTableName, "UNCYB") Then

                    DSASUtility.log("LIDAR measurements table detected: " + uncTableName)
                    haveUncTable = True
                Else
                    DSASUtility.log("No LIDAR measurements table detected.")
                End If

                uncyFldIdx = -1
                biasFldIdx = -1
                uncbFldIdx = -1

                If My.Settings.Shoreline_Uncertainty_Field > "" AndAlso GeoDB.shoreFldMap("uncertainty") > -1 Then
                    haveUncField = True
                    ' Initialize field indexes
                    uncyFldIdx = GeoDB.shoreFldMap("uncertainty")
                    DSASUtility.log("Uncertainty field detected in shoreline table: " + shoreLyrName)
                Else
                    haveUncField = False
                    DSASUtility.log("Uncertainty field not detected in shoreline table: " + shoreLyrName)
                End If

                If GeoDB.shoreFldMap("bias") > -1 AndAlso GeoDB.shoreFldMap("uncertainty_bias") > -1 Then
                    haveUncBiasFields = True
                    ' Initialize field indexes
                    biasFldIdx = GeoDB.shoreFldMap("bias")
                    uncbFldIdx = GeoDB.shoreFldMap("uncertainty_bias")
                    DSASUtility.log("Bias and uncertainty bias fields detected in shoreline table: " + shoreLyrName)
                Else
                    haveUncBiasFields = False
                    DSASUtility.log("Bias and uncertainty bias fields not detected in shoreline table: " + shoreLyrName)
                End If

                If My.Settings.Uncertainty >= 0 Then
                    ' Use default uncertainty as a last resort if no better method was available.
                    haveUncDefault = True
                    DSASUtility.log("Uncertainty default value detected: " + My.Settings.Uncertainty.ToString() + " meters")
                Else
                    haveUncDefault = False
                    DSASUtility.log("Uncertainty default value not detected: " + My.Settings.Uncertainty.ToString() + " meters")
                End If
            Catch
                DSASUtility.log(TraceLevel.Error, "Encountered an error while detecting sources of uncertainty info.")
            End Try
        End Sub


        Public ReadOnly Property shoreDate() As Date
            Get
                Return CDate(shoreDateStr)
            End Get
        End Property

        Dim isectPoint As IPoint = Nothing

        Public Sub extractShoreAndIntersectInfo(ByVal shoreFeat As IFeature, ByVal xectFeat As IFeature)
            Dim hitDist As Double
            isectPoint = findDesiredShoreIntersect(shoreFeat, xectFeat.Shape, hitDist)

            'If no intersects, then hitPt will be empty.
            If isectPoint Is Nothing OrElse isectPoint.IsEmpty Then Exit Sub

            isValid = True
            dFromAnchor = hitDist
            x = isectPoint.X
            y = isectPoint.Y

            If haveUncTable Then
                If DirectCast(isectPoint, IMAware).MSimple Then
                    m = isectPoint.M
                    extractMInfo()
                Else
                    m = -1
                End If
            End If

            Dim val As Object
            If uncy = -1 AndAlso haveUncField Then
                If uncyFldIdx > -1 Then
                    val = shoreFeat.Value(uncyFldIdx)
                    uncy = IIf(val Is DBNull.Value, -1, val)
                End If
            End If

            If bias = -1 AndAlso haveUncBiasFields Then
                If biasFldIdx > -1 Then
                    val = shoreFeat.Value(biasFldIdx)
                    bias = IIf(val Is DBNull.Value, -1, val)
                End If
            End If

            If uncb = -1 AndAlso haveUncBiasFields Then
                If uncbFldIdx > -1 Then
                    val = shoreFeat.Value(uncbFldIdx)
                    uncb = IIf(val Is DBNull.Value, -1, val)
                End If
            End If

            ' Substitute default uncertainty value if we don't have an uncertainty value
            If uncy <= 0 AndAlso haveUncDefault Then
                uncy = My.Settings.Uncertainty
            End If

        End Sub


        Sub addToTransectData(transectData As IronPython.Runtime.PythonDictionary)
            Try
                transectData.Add("CI", My.Settings.Confidence_Interval)
                DirectCast(transectData.Item("dates"), IronPython.Runtime.List).append(DSASUtility.DecimalYear(shoreDate))
                DirectCast(transectData.Item("shore"), IronPython.Runtime.List).append(dFromBl)
                DirectCast(transectData.Item("uncy"), IronPython.Runtime.List).append(NothingIfMinusOne("uncy", uncy))
                If applyBias() Then
                    DirectCast(transectData.Item("type"), IronPython.Runtime.List).append(shoreType)
                    If transectData.has_key("bias") AndAlso transectData("bias") IsNot Nothing Then
                        ' We already recorded bias info from a prior shore intersect, nothing to do
                    Else
                        transectData.Add("bias", NothingIfMinusOne("bias", bias))
                        transectData.Add("biasUncy", NothingIfMinusOne("uncb", uncb))
                    End If
                    DirectCast(transectData.Item("shore_x"), IronPython.Runtime.List).append(x)
                    DirectCast(transectData.Item("shore_y"), IronPython.Runtime.List).append(y)
                    DirectCast(transectData.Item("distance_to_shore_from_origin"), IronPython.Runtime.List).append(dFromBl) ' TODO: This is suspicious. But maybe not even being used???
                End If
            Catch ex As Exception
                handleException(ex)
            End Try
        End Sub


        Sub addFeatures(rates As IronPython.Runtime.PythonDictionary)
            Dim newFeature As IFeature = GeoDB.createIntersectFeature()

            With newFeature
                .Value(newFeature.Fields.FindField("TransectId")) = xectId
                .Value(newFeature.Fields.FindField("BaselineId")) = blId
                .Value(newFeature.Fields.FindField("ShorelineId")) = shoreDateStr
                .Value(newFeature.Fields.FindField("Distance")) = dFromBl
                .Value(newFeature.Fields.FindField("IntersectX")) = x
                .Value(newFeature.Fields.FindField("IntersectY")) = y
                .Value(newFeature.Fields.FindField("Uncertainty")) = nothingToNull(DirectCast(rates("uncertainty"), IronPython.Runtime.List).pop(0))

                If applyBias() Then
                    .Value(newFeature.Fields.FindField("Bias_Distance")) = nothingToNull(DirectCast(rates("bias_distance"), IronPython.Runtime.List).pop(0))
                    .Value(newFeature.Fields.FindField("BIAS")) = nothingToNull(rates("bias"))
                    .Value(newFeature.Fields.FindField("BIAS_X")) = nothingToNull(DirectCast(rates("bias_x"), IronPython.Runtime.List).pop(0))
                    .Value(newFeature.Fields.FindField("BIAS_Y")) = nothingToNull(DirectCast(rates("bias_y"), IronPython.Runtime.List).pop(0))
                    .Value(newFeature.Fields.FindField("Bias_Uncertainty")) = nothingToNull(DirectCast(rates("bias_uncertainty"), IronPython.Runtime.List).pop(0))
                End If
            End With

            Dim point As IPoint = New PointClass()
            point.PutCoords(x, y)
            newFeature.Shape = point

            newFeature.Store()
        End Sub


        Public Sub extractMInfo()
            Dim cmd As OleDbCommand
            Dim dr As OleDbDataReader
            Dim sql As String = ""

            uncy = -1
            bias = -1
            uncb = -1

            Try
                Dim intM As String = Floor(m).ToString
                sql += "select ID, UNCY, BIAS, UNCYB from [" + uncTableName + "] "
                sql += "where ID=(select max(ID) from [" + uncTableName + "] where id<=" + intM + ") "
                sql += "or ID=(select min(ID) from [" + uncTableName + "] where id>" + intM + ") "
                sql += "order by id"
                cmd = New OleDbCommand(sql, cn)
                dr = cmd.ExecuteReader

                If dr.Read() Then
                    Dim mBefore As Integer = dr("ID")
                    Dim uncy1 As Double = dr("UNCY")
                    Dim bias1 As Double = dr("BIAS")
                    Dim uncb1 As Double = dr("UNCYB")
                    If dr.Read() Then
                        Dim mAfter As Integer = dr("ID")
                        Dim uncy2 As Double = dr("UNCY")
                        Dim bias2 As Double = dr("BIAS")
                        Dim uncb2 As Double = dr("UNCYB")
                        Dim fracM As Double = (m - mBefore) / (mAfter - mBefore)
                        Dim fracMComp As Double = (1 - fracM)   ' fracM complement (to avoid repeated subtractions)
                        uncy = uncy1 * fracM + uncy2 * fracMComp
                        bias = bias1 * fracM + bias2 * fracMComp
                        uncb = uncb1 * fracM + uncb2 * fracMComp
                        If applyBias() Then log("uncb: " & nv(uncb, ""))
                    End If
                End If
                dr.Close()
            Catch
                'All default to -1 if we get here...
            End Try
        End Sub


        Dim dFromBl As Double? = Nothing


        Sub computeDistanceFromBaseline(xectGeom As IPolyline, blGeom As IPolyline)
            Dim xectBaselinePoint As IPoint = getXectBaselinePoint(xectGeom)
            If xectBaselinePoint Is Nothing Then
                ' We should never get here unless bad transect geometry
                Return
            End If
            dFromBl = MapUtility.GetDistanceBetweenTwoPoints(isectPoint, xectBaselinePoint)

            Dim bRightSide As Boolean
            Dim ht As IHitTest = DirectCast(blGeom, IHitTest)
            ht.HitTest(isectPoint, xectGeom.Length * 1.01, esriGeometryHitPartType.esriGeometryPartBoundary, Nothing, Nothing, Nothing, Nothing, bRightSide)
            If (bRightSide AndAlso My.Settings.Land_On_Right_Side) OrElse (Not bRightSide AndAlso Not My.Settings.Land_On_Right_Side) Then
                dFromBl = -dFromBl
            End If
        End Sub

    End Class


    ''' <summary>
    ''' To apply bias to rate calculations, both shoreline type field and uncertainty table need to be present and selected by the user
    ''' </summary>
    ''' <returns>True if above condition is satisfied, false otherwise</returns>
    Shared Function applyBias() As Boolean
        Return haveShorelineTypeField AndAlso haveShorelineUncertaintyTable
    End Function

    Shared stopWatch As Stopwatch = New Stopwatch()
    Shared secondsAtLastReport As Long = 0
    Shared estimatedSecondsRemaining As Double = 0

    Shared Sub updateEstimate(xectCount As Long, xectsProcessed As Long)
        ' Report in roughly five second intervals and not before ten transects have been processed
        If stopWatch.IsRunning AndAlso stopWatch.Elapsed.TotalSeconds >= secondsAtLastReport + 5 Then
            If xectsProcessed > 0 Then estimatedSecondsRemaining = (xectCount - xectsProcessed) / xectsProcessed * stopWatch.Elapsed.TotalSeconds
            log("Elapsed: {0} secondsAtLastReport: {1} estimatedSecondsRemaining: {2}".FormatWith(stopWatch.Elapsed.TotalSeconds, secondsAtLastReport, estimatedSecondsRemaining))
            secondsAtLastReport = stopWatch.Elapsed.TotalSeconds
            DSAS.calcForm.setStatus("Calculating rates... Estimated time remaining: " + TimeSpan.FromSeconds(estimatedSecondsRemaining).Humanize(IIf(estimatedSecondsRemaining < 3600, 1, 2)))
            System.Windows.Forms.Application.DoEvents()
        End If
    End Sub



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="xectLyrName"></param>
    ''' <param name="filename"></param>
    ''' <param name="threshold"></param>
    ''' <returns>An empty string if all records are successfully written. If there were no records to write 
    ''' or if there were errors a string describing the problem is returned and must be regarded as failure.</returns>
    ''' <remarks></remarks>
    Public Shared Function walkAndCalc(ByVal xectLyrName As String, ByVal filename As String, ByVal threshold As Integer) As String
        log("START CALCS")
        DSAS.seaward = nv(getSeawardLandwardSettingFromMetadata(TransectLyrToolCtl.currentTransectLayer), My.Settings.Seaward)
        Dim blCursor As IFeatureCursor = Nothing
        Dim result As String = ""
        Dim shoreDates As New SortedDictionary(Of Date, Integer)

        stopWatch.Reset()
        Dim stepPro As ESRI.ArcGIS.esriSystem.IStepProgressor = Nothing

        Try
            DSAS.calcForm.setStatus("Recording settings in log file...")
            DSASUtility.TraceSettings()

            Dim shoreCache As New FeatureCacheClass
            Dim baseLayer As IFeatureLayer = MapUtility.findFeatureLayer(My.Settings.Baseline_Feature_Layer, "baseline")
            If Not GeoDB.layerIsValid(baseLayer, "baseline") Then
                result = "Valid baseline layer selection is required!"
                Exit Try
            End If

            Dim shoreLayer As IFeatureLayer = MapUtility.findFeatureLayer(My.Settings.Shoreline_Feature_Layer, "shoreline")
            If Not GeoDB.layerIsValid(shoreLayer, "shoreline") Then
                result = "Valid shoreline layer selection is required!"
                Exit Try
            End If

            Dim selectedShores As List(Of Integer) = MapUtility.getSelectedShoreIds()


            Dim rates2calc As New IronPython.Runtime.List
            For Each aStat As Stat In DSAS.calcForm.chkAdvancedStats.CheckedItems
                rates2calc.append(aStat.valueMember)
            Next

            Dim xectCount As Double = MapUtility.findFeatureLayer(xectLyrName, "transect").FeatureClass.FeatureCount(Nothing)
            stepPro = MapUtility.InitStepProgressor(My.ThisApplication.StatusBar, 1, xectCount, 1)

            IntersectData.cn = MapUtility.dbConnectionForLayer(My.Settings.Shoreline_Feature_Layer)
            IntersectData.determineUncertaintySources()

            Dim hasGroupFld As Boolean

            Dim shoreDateFldIdx As Integer = GeoDB.shoreFldMap("date")
            If shoreDateFldIdx = -1 Then Return "You need to select the date field for your shoreline layer." + vbCrLf + "Please do so by opening the default parameters form."

            Dim shoretypeFldIdx As Integer = GeoDB.shoreFldMap("TYPE")

            Dim blIdFldIdx As Integer = GeoDB.baseFldMap("ID")
            Dim blGrpFldIdx As Integer = -1

            If GeoDB.baseFldMap("group") > -1 Then
                blGrpFldIdx = GeoDB.baseFldMap("group")
            End If
            hasGroupFld = blGrpFldIdx > -1

            If hasGroupFld Then
                DSASUtility.log(TraceLevel.Info, "Using group field: " + DirectCast(GeoDB.tblFldInfo("group", "baseline"), Hashtable)("name").ToString)
            Else
                DSASUtility.log(TraceLevel.Info, "Group field doesn't exist, won't be used.")
            End If

            Dim shoreFeatClass As IFeatureClass = shoreLayer.FeatureClass

            'The data structure to hold info about a transect's intersections with each shoreline
            Dim shoreIntersects As New SortedDictionary(Of Date, IntersectData)

            Dim outputTableName As String = DSASUtility.makeOutputTablename(filename)
            Dim intersectLayerName As String = xectLyrName + "_intersect_" + outputTableName.Split(New String() {"_rates_"}, StringSplitOptions.None)(1)

            If GeoDB.CreateWorkspaceFeatureClassAndAddToTOC(intersectLayerName, "intersect", Nothing, xectLyrName, applyBias) Is Nothing Then
                result = "Unable to create intersect featureclass!"
                Exit Try
            End If

            Dim ratesLyrName As String = xectLyrName + "_rates_" + outputTableName.Split(New String() {"_rates_"}, StringSplitOptions.None)(1)
            Dim ratesFc As IFeatureClass = GeoDB.CreateWorkspaceFeatureClassAndAddToTOC(ratesLyrName, "rates", Nothing, xectLyrName)

            If ratesFc Is Nothing Then
                result = "Unable to create rates featureclass!"
                Exit Try
            End If

            addRateFields(ratesFc, rates2calc)

            DSAS.calcForm.setStatus("Calculating rates...")

            Dim blFeat As IFeature
            Dim blId As Long = -1
            Dim currGrp As Integer = -1
            Dim dAlongCum As Double = 0

            blCursor = GeoDB.getCursorForLayer(My.Settings.Baseline_Feature_Layer, "ID", "ID>0")
            Dim lastBlPt As IPoint = Nothing
            Dim xectsProcessed As Long = 0
            Do
                updateEstimate(xectCount, xectsProcessed)
                blFeat = blCursor.NextFeature
                If blFeat Is Nothing Then Exit Do
                Dim blGeom As IPolyline = DirectCast(blFeat.Shape, IPolyline)

                blId = blFeat.Value(blIdFldIdx)

                ' If we have a group field...
                If hasGroupFld Then
                    currGrp = blFeat.Value(blGrpFldIdx)
                End If

                ' Get a transect cursor ordered by TransOrder for transects that match the current baseline
                Dim xectCursor As IFeatureCursor = GeoDB.getCursorForLayer(xectLyrName, "TransOrder", "BaselineID=" + blId.ToString)
                Dim dAlong As Double = 0    ' Distance along baseline of transect

                If lastBlPt IsNot Nothing Then
                    ' Add straigt distance between previous and current baseline
                    dAlongCum += MapUtility.GetDistanceBetweenTwoPoints(lastBlPt, blGeom.FromPoint)
                End If

                Do
                    Dim xectFeat As IFeature = xectCursor.NextFeature
                    If xectFeat Is Nothing Then Exit Do

                    ' Maintain encountered transect count to use in estimating remaining runtime
                    xectsProcessed += 1
                    updateEstimate(xectCount, xectsProcessed)
                    If stepPro IsNot Nothing Then stepPro.Step()
                    System.Windows.Forms.Application.DoEvents()

                    If xectFeat.HasOID Then log("TransectId: {0} xectCount: {1} xectsProcessed: {2}".FormatWith(xectFeat.OID, xectCount, xectsProcessed))
                    If xectFeat.Shape Is Nothing OrElse xectFeat.Shape.IsEmpty Then Continue Do
                    Dim xectGeom As IPolyline = DirectCast(xectFeat.Shape, IPolyline)
                    Dim xectBaselinePoint As IPoint = DSASUtility.getXectBaselinePoint(xectGeom)

                    blGeom.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, xectBaselinePoint, False, Nothing, dAlong, Nothing, Nothing)

                    ' Cache all shore features around the middle of the transect and at a distance where they could touch the transect
                    Dim xectMidPt As New PointClass
                    xectGeom.QueryPoint(esriSegmentExtension.esriNoExtension, 0.5, True, xectMidPt)
                    shoreCache.Initialize(xectMidPt, xectGeom.Length * 1.01)  'Allow for 1% error
                    shoreCache.AddFeatures(shoreFeatClass)

                    For i As Integer = 0 To shoreCache.Count - 1
                        updateEstimate(xectCount, xectsProcessed)
                        If DSAS.interruptCalcs Then
                            Return "User interrupt received."
                        End If

                        Dim shoreFeat As IFeature = shoreCache.Feature(i)
                        ' If some shorelines are selected and and this feature is not in that selection then move on to next.
                        If selectedShores IsNot Nothing AndAlso Not selectedShores.Contains(shoreFeat.OID) Then Continue For
                        Dim currIntersect As New IntersectData

                        currIntersect.shoreDateStr = shoreFeat.Value(shoreDateFldIdx).ToString
                        shoreDates(currIntersect.shoreDate) = 1  'Keep track of all shore dates, not just the ones on current transect

                        If applyBias() Then
                            currIntersect.shoreType = shoreFeat.Value(shoretypeFldIdx).ToString
                        End If

                        currIntersect.extractShoreAndIntersectInfo(shoreFeat, xectFeat)

                        If currIntersect.isValid Then
                            If rates2calc.Contains("WLR") AndAlso currIntersect.uncy <= 0 Then
                                log(TraceLevel.Error, "You have attempted to compute Weighted Linear Regression (WLR) where one or more of the input shorelines has been assigned an uncertainty value of zero (or null) in both the shoreline attribute table and the DSAS default parameters window.  WLR assigns a weight component to each shoreline based on this shoreline uncertainty value.  The weight is computed by taking the inverse square of the uncertainty, or [1/(uncertainty)²].  An uncertainty value of zero has terminated the calculation of WLR and no values will be reported in the output table.")
                                Return "WLR requires non-zero uncertainty values"
                            End If

                            If Not shoreIntersects.ContainsKey(currIntersect.shoreDate) OrElse
                                currIntersect.dFromAnchor < shoreIntersects(currIntersect.shoreDate).dFromAnchor Then
                                ' Record intersect data
                                currIntersect.xectId = xectFeat.OID
                                currIntersect.blId = blId
                                ' Register intersect (possibly overwriting another intersect with the same shoreline)
                                shoreIntersects(currIntersect.shoreDate) = currIntersect
                            End If
                        End If
                    Next

                    ' If we intersected enough shorelines (greater than or equal to threshold) or if threshold is disabled (equal to -1)... 
                    If shoreIntersects.Count >= Math.Max(1, threshold) Then
                        Dim transectData As New IronPython.Runtime.PythonDictionary
                        transectData.Add("dates", New IronPython.Runtime.List)
                        transectData.Add("shore", New IronPython.Runtime.List)
                        transectData.Add("uncy", New IronPython.Runtime.List)
                        transectData.Add("rates2calc", rates2calc)
                        transectData.Add("transectId", xectFeat.OID)
                        transectData.Add("group", currGrp)
                        transectData.Add("TCD", DSASUtility.round(dAlongCum + dAlong))
                        If applyBias() Then
                            transectData.Add("type", New IronPython.Runtime.List)
                            transectData.Add("shore_x", New IronPython.Runtime.List)
                            transectData.Add("shore_y", New IronPython.Runtime.List)

                            transectData.Add("origin_x", xectBaselinePoint.X)
                            transectData.Add("origin_y", xectBaselinePoint.Y)
                            transectData.Add("distance_to_shore_from_origin", New IronPython.Runtime.List)
                        End If

                        ' Gather data for each intersect...
                        For Each currIntersect As IntersectData In shoreIntersects.Values
                            currIntersect.computeDistanceFromBaseline(xectGeom, blGeom)
                            currIntersect.addToTransectData(transectData)
                        Next

                        Dim rates As IronPython.Runtime.PythonDictionary
                        If applyBias() AndAlso shoreIntersects.Count > 0 Then log("Surviving uncb: " & nv(transectData("biasUncy"), "").ToString)
                        Try
                            rates = ipy.calc(transectData)
                        Catch ex As Exception
                            rates = New IronPython.Runtime.PythonDictionary
                        End Try

                        If Not stopWatch.IsRunning Then
                            secondsAtLastReport = 0
                            stopWatch.Start()
                        End If

                        rates.Add("TransectId", xectFeat.OID)
                        rates.Add("ShrCount", shoreIntersects.Count)
                        rates.Add("TCD", DSASUtility.round(dAlongCum + dAlong))
                        addRateFeature(ratesFc, rates, xectFeat)

                        ' Gather data for each intersect...
                        For Each currIntersect As IntersectData In shoreIntersects.Values
                            ' to table
                            Try
                                currIntersect.addFeatures(rates)
                            Catch ex As Exception
                                handleException(ex)
                            End Try
                            result = ""
                        Next
                    End If

                    ' Clear up shore intersections on this transect so we can reuse the object for next
                    shoreIntersects.Clear()
                Loop
                ' Accumulate previous baseline's length
                dAlongCum += blGeom.Length
                ' Mark this baseline's end point
                lastBlPt = blGeom.ToPoint

                DSASUtility.ReleaseComObject(xectCursor)
            Loop

            Dim summary_data As IronPython.Runtime.PythonDictionary
            If DSAS.calcForm.chkCreateSumReport.Checked OrElse rates2calc.Contains("LRR") Then
                summary_data = ipy.getSummary()
                Dim LRR_data As IronPython.Runtime.PythonDictionary = summary_data("rates")("LRR")

                If DSAS.calcForm.chkCreateSumReport.Checked Then
                    DSAS.calcForm.setStatus("Creating summary report...")
                    createSummaryReport(MapUtility.findFeatureLayer(ratesFc.AliasName, "rates").Name, summary_data, shoreDates)
                End If
            End If

            If DSAS.calcForm.ifUseColorRamp.Checked Then
                DSAS.calcForm.setStatus("Generating color ramp...")
                MapUtility.setRateColorRamp(MapUtility.findFeatureLayer(ratesFc.AliasName, "rates"), DSAS.calcForm.rateFields.SelectedItem.valueMember)
            End If

            ' This index is important for performance during forecasting
            GeoDB.AddIndexToFeatureClass("intersect", "transectid_idx", "TransectID")

        Catch ex As System.Exception
            DSASUtility.handleException(ex)
            result = "Error while calculating statistics."
        Finally
            DSAS.interruptCalcs = False
            DSAS.seaward = Nothing
            If stopWatch IsNot Nothing Then
                stopWatch.Stop()
                stopWatch.Reset()
            End If
            DSASUtility.ReleaseComObject(blCursor)
            If IntersectData.cn IsNot Nothing Then IntersectData.cn.Close()
            If stepPro IsNot Nothing Then stepPro.Hide()
        End Try

        Return result
    End Function


    Shared Sub addRateFields(ratesFc As IFeatureClass, rates2calc As IronPython.Runtime.List)
        For Each aStat As Stat In Stat.stats.Values
            If rates2calc.Contains(aStat.parent) Then
                Dim fldName = aStat.valueMember
                fldName = fldName.Replace("{CI}", CI.getCIFor(My.Settings.Confidence_Interval).fieldNameCompatible)
                If ratesFc.FindField(fldName) = -1 Then
                    GeoDB.AddFieldToFeatureClass(ratesFc, GeoDB.CreateStatField(fldName), False)
                End If
                If applyBias() Then
                    fldName = "NB_" + fldName
                    If ratesFc.FindField(fldName) = -1 Then
                        GeoDB.AddFieldToFeatureClass(ratesFc, GeoDB.CreateStatField(fldName), False)
                    End If
                End If
            End If
        Next
    End Sub


    Shared Sub addRateFeature(outputFeatClass As IFeatureClass, data As IronPython.Runtime.PythonDictionary, xect As IFeature)
        Try
            Dim rateFeature As IFeature = outputFeatClass.CreateFeature()
            With rateFeature
                ' Write out standard fields (except TransectId which is already included with rates
                .Shape = xect.Shape
                .Value(outputFeatClass.FindField("TransectId")) = xect.OID
                .Value(outputFeatClass.FindField("BaselineId")) = xect.Value(xect.Fields.FindField("BaselineId"))
                If nv(My.Settings.Baseline_Group_Field) <> "" Then .Value(outputFeatClass.FindField("GroupId")) = xect.Value(xect.Fields.FindField("GroupId"))
                .Value(outputFeatClass.FindField("TransOrder")) = xect.Value(xect.Fields.FindField("TransOrder"))
                .Value(outputFeatClass.FindField("Azimuth")) = xect.Value(xect.Fields.FindField("Azimuth"))
                .Value(outputFeatClass.FindField("ShrCount")) = data.get("ShrCount", 0)
                .Value(outputFeatClass.FindField("TCD")) = data.get("TCD", 0)

                For Each fldName As String In data.viewkeys
                    Dim NB_prefix As Boolean = fldName.StartsWith("NB_")
                    Dim non_NB_fldName As String = IIf(NB_prefix, fldName.Substring("NB_".Length), fldName)
                    If Stat.stats.ContainsKey(non_NB_fldName) Then
                        Dim fldNameInTable As String = IIf(NB_prefix, "NB_", "") + Stat.stats.Item(non_NB_fldName).asFieldName
                        Dim idx As Integer = outputFeatClass.FindField(fldNameInTable)
                        If idx > -1 Then
                            Dim val = DSASUtility.round(data(fldName))
                            If val IsNot Nothing Then
                                .Value(idx) = val
                            End If
                        End If
                    Else
                        Dim a = 1
                    End If
                Next
                ' save the row
                .Store()
            End With
        Catch ex As Exception
            handleException(ex)
            Throw ex
        End Try
    End Sub



    ''' <summary>
    ''' Performs a number of global checks on the data to make sure assumptions are satisfied and rate calculation
    ''' may be performed.
    ''' </summary>
    ''' <returns>Returns true if no issues are found.</returns>
    ''' <remarks></remarks>
    Public Shared Function checkForIntegrity() As Boolean
        Dim msg As String = ""
        Dim cn As OleDbConnection = Nothing
        Dim cmd As OleDbCommand
        Dim dr As OleDbDataReader = Nothing
        Dim sql As String
        Dim cntNulls As Integer
        Dim baselineTable As String = My.Settings.Baseline_Feature_Layer
        Dim baselineTableSource As String = GeoDB.getSourceTableName(baselineTable)
        Dim shorelineTable As String = My.Settings.Shoreline_Feature_Layer
        Dim shorelineTableSource As String = GeoDB.getSourceTableName(shorelineTable)
        Dim xectTable As String = TransectLyrToolCtl.currentTransectLayerName
        Dim xectTableSource As String = GeoDB.getSourceTableName(TransectLyrToolCtl.currentTransectLayerName)

        haveShorelineTypeField = False
        haveShorelineUncertaintyTable = False

        Try
            Dim baseLayer As IFeatureLayer = MapUtility.findFeatureLayer(baselineTable, "baseline")
            If Not GeoDB.layerIsValid(baseLayer, "baseline") Then
                msg = "Valid baseline layer selection is required!"
                log(TraceLevel.Error, msg)
                Exit Function
            End If

            Dim shoreLayer As IFeatureLayer = MapUtility.findFeatureLayer(shorelineTable, "shoreline")
            If Not GeoDB.layerIsValid(shoreLayer, "shoreline") Then
                msg = "Valid shoreline layer selection is required!"
                log(TraceLevel.Error, msg)
                Exit Function
            End If

            If Not GeoDB.checkIfTableFieldExists(baselineTableSource, Nothing) Then
                msg = "Unable to find database table for baseline layer '" + baselineTable + "'."
                log(TraceLevel.Error, msg)
                Return False
            End If

            If Not GeoDB.checkIfTableFieldExists(xectTable, Nothing) Then
                msg = "Unable to find database table for transect layer '" + xectTable + "'."
                log(TraceLevel.Error, msg)
                Return False
            End If

            cntNulls = GeoDB.countNulls(baselineTableSource, "ID")
            If cntNulls > 0 Then
                msg = "Baseline layer '" + baselineTable + "' has " + CStr(cntNulls) + " record(s) that did not have a value specified in the ID field. Please edit null values for ID in the attribute table of the specified baseline layer before proceeding."
                log(TraceLevel.Error, msg)
                Return False
            End If

            cntNulls = GeoDB.countNulls(xectTableSource, "BaselineID")
            If cntNulls > 0 Then
                msg = "Transect layer '" + xectTable + "' has " + CStr(cntNulls) + " record(s) that did not have a value specified in the BaselineID field. Please edit null values for BaselineID in the attribute table of the specified transect layer before proceeding."
                log(TraceLevel.Error, msg)
                Return False
            End If

            ' Baselines should have unique ID numbers except for those with ID=0 to indicate they are excluded.
            Try
                sql = "select ID, count(*) as cnt from [" + baselineTableSource + "] where ID>0 group by ID having count(*)>1"
                cn = MapUtility.dbConnectionForLayer(baselineTable)
                cmd = New OleDbCommand(sql, cn)
                dr = cmd.ExecuteReader

                If dr.Read() Then
                    msg = "Baseline layer '" + baselineTable + "' requires unique ID numbers (zero values are excluded). "
                    msg += "ID value " + dr("ID").ToString + " is repeated " + dr("cnt").ToString + " times."
                    log(TraceLevel.Error, msg)
                    Return False
                End If
            Finally
                If dr IsNot Nothing Then dr.Close()
                If cn IsNot Nothing Then cn.Close()
            End Try


            ' Baselines of the same group should not have another group mixed in geographically (as determined by increasing ID number)
            ' only applies if we have a group field...
            If nv(My.Settings.Baseline_Group_Field) > "" Then
                Dim grp As String = nv(My.Settings.Baseline_Group_Field)

                If Not GeoDB.checkIfTableFieldExists(baselineTable, grp) Then
                    msg = "Unable to find group field '" + grp + "' in baseline layer " + baselineTable
                    log(TraceLevel.Error, msg)
                    Return False
                End If

                Try
                    sql =
                        "SELECT a.[" + grp + "] as grpA, b.[" + grp + "] as grpB " &
                        "FROM [" + baselineTableSource + "] as a, [" + baselineTableSource + "] as b, [" + baselineTableSource + "] as c " &
                        "WHERE(a.ID < b.ID And b.ID < c.ID)" &
                        "AND a.[" + grp + "]=c.[" + grp + "] AND a.[" + grp + "]<>b.[" + grp + "]"

                    cn = MapUtility.dbConnectionForLayer(baselineTable)
                    cmd = New OleDbCommand(sql, cn)
                    dr = cmd.ExecuteReader

                    If dr.Read() Then
                        msg = "Baseline group '" + dr("grpA").ToString + "' is repeated after some baseline features belonging to group '" + dr("grpB").ToString + "'."
                        log(TraceLevel.Error, msg)
                        Return False
                    End If
                Finally
                    If dr IsNot Nothing Then dr.Close()
                    If cn IsNot Nothing Then cn.Close()
                End Try
            End If

            ' Transects should have non-null and unique TransOrder numbers.
            Try
                cntNulls = GeoDB.countNulls(xectTable, "TransOrder")

                sql = "select TransOrder, count(*) as cnt from [" + xectTableSource + "] group by TransOrder having count(*)>1"
                cn = MapUtility.dbConnectionForLayer(xectTable)
                cmd = New OleDbCommand(sql, cn)
                dr = cmd.ExecuteReader

                If cntNulls > 0 OrElse dr.Read() Then
                    msg = "Transect layer '" + xectTable + "' requires unique and non-null TransOrder values. " & vbCrLf
                    msg += "Would you like DSAS to re-assign TransOrder values preserving geospatial ordering dictated by your baseline features?"
                    'msg += "TransOrder value " + dr("TransOrder").ToString + " is repeated " + dr("cnt").ToString + " times."
                    If System.Windows.Forms.MessageBox.Show(msg, "DSAS", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.OK Then
                        If sortAlongBaseline() Then
                            MsgBox("TransOrder values have been re-assigned. Please ensure BaselineId and " & vbCrLf &
                                "(if applicable) group values are correctly assigned before calculating statistics again.")
                        Else
                            msg = "There was a problem while processing TransOrder values."
                            log(TraceLevel.Error, msg)
                        End If
                    End If
                    Return False
                End If
            Finally
                If dr IsNot Nothing Then dr.Close()
                If cn IsNot Nothing Then cn.Close()
            End Try


            ' For transects, both TransOrder and BaselineID should monotonically increase...
            Try
                sql =
                    "SELECT a.TransOrder, b.TransOrder " &
                    "FROM [" + xectTableSource + "] as a, [" + xectTableSource + "] as b " &
                    "WHERE a.BaselineID < b.BaselineID And a.TransOrder > b.TransOrder"

                cn = MapUtility.dbConnectionForLayer(xectTable)
                cmd = New OleDbCommand(sql, cn)
                dr = cmd.ExecuteReader

                If dr.Read() Then
                    msg = "Transect layer '" + xectTable + "' should have increasing TransOrder values for increasing BaselineID values. " + vbCrLf
                    msg += "TransOrder values " + dr(0).ToString + " and " + dr(1).ToString + ", for example, violate this rule."
                    log(TraceLevel.Error, msg)
                    Return False
                End If
            Finally
                If dr IsNot Nothing Then dr.Close()
                If cn IsNot Nothing Then cn.Close()
            End Try


            cntNulls = GeoDB.countNulls(shorelineTable, My.Settings.Shoreline_Date_Field)
            If cntNulls > 0 Then
                msg = "Shoreline layer '" + shorelineTable + "' has " + CStr(cntNulls) + " record(s) that did not have a value specified in date field '" + My.Settings.Shoreline_Date_Field + "'."
                log(TraceLevel.Error, msg)
                Return False
            End If

            ' Shorelines must have valid dates.
            Try
                sql = "select [" + My.Settings.Shoreline_Date_Field + "] as dt from [" + shorelineTableSource + "]"
                cn = MapUtility.dbConnectionForLayer(shorelineTable)
                cmd = New OleDbCommand(sql, cn)
                dr = cmd.ExecuteReader

                msg = ""
                While dr.Read()
                    Dim dummy As Date = CDate(dr("dt"))
                    Dim strDate As String = dr("dt").ToString.Trim
                    Dim lastSlash As Integer = strDate.LastIndexOf("/")
                    If lastSlash < 0 Then
                        msg = "Unrecognized date format encountered in shoreline date: '" + strDate + "'." + vbCrLf + "Dates must be in the form MM/DD/YYYY (preferred) or MM/DD/YY, e.g. 10/27/1986 or  10/27/86."
                        log(TraceLevel.Error, msg)
                        Return False
                    ElseIf strDate.Length - lastSlash = 3 Then
                        'msg = "2-digit year encountered in shoreline dates." + vbCrLf + "Year values less than 30 will be prefixed with 20 (e.g. 11 -> 2011)." + vbCrLf + "Year values between 30 and 99 will be prefixed with 19 (e.g. 68 -> 1968)." + vbCrLf + "Use 4-digit year values to avoid this message in the future." + vbCrLf + "Click [OK] to proceed, [Cancel] to stop."
                        msg = "Two-digit year encountered in shoreline date field. DSAS can interpret this" _
                            + vbCrLf + "to a four-digit year using the following convention:" _
                            + vbCrLf _
                            + vbCrLf + "Year values 00 through 29 will be interpreted as the years 2000 through" _
                            + vbCrLf + "2029. For example, 07/04/11 will be assumed as 2011 (07/04/2011)." _
                            + vbCrLf _
                            + vbCrLf + "Year values 30 through 99 will be interpreted as the years 1930 through" _
                            + vbCrLf + "1999. For example, 07/04/68 will be assumed as 1968 (07/04/1968)." _
                            + vbCrLf _
                            + vbCrLf + "Use 4-digit year values to avoid this message in the future." _
                            + vbCrLf + "Click [OK] to proceed, [Cancel] to stop."
                    End If
                End While
                ' If we got here without an exception, then all dates in shoreline layer are valid dates.
                ' Give a warning if 2-digit year values are used though...
                If msg IsNot Nothing AndAlso msg <> "" Then
                    If MsgBox(msg, MsgBoxStyle.OkCancel, "Proceed with 2-digit year values?") = MsgBoxResult.Cancel Then
                        Return False
                    End If
                End If
            Catch ex As Exception
                ' We got an exception. Must be a bad date.
                msg = "Shoreline layer '" + shorelineTable + "' has some record(s) that do not have a valid date specified in date field '" + My.Settings.Shoreline_Date_Field + "'."
                log(TraceLevel.Error, msg)
                Return False
            Finally
                If dr IsNot Nothing Then dr.Close()
                If cn IsNot Nothing Then cn.Close()
            End Try

            If nv(My.Settings.Baseline_Group_Field) > "" Then
                cntNulls = GeoDB.countNulls(baselineTable, My.Settings.Baseline_Group_Field)
                If cntNulls > 0 Then
                    msg = "Baseline layer '" + baselineTable + "' has " + CStr(cntNulls) + " record(s) that did not have a value specified in group field '" + My.Settings.Baseline_Group_Field + "'."
                    log(TraceLevel.Error, msg)
                    Return False
                End If
            End If

            If nv(My.Settings.Shoreline_Type_Field, "") <> "" Then
                If GeoDB.checkIfTableFieldExists(My.Settings.Shoreline_Feature_Layer, My.Settings.Shoreline_Type_Field) Then
                    Try
                        sql =
                                    "SELECT * " &
                                    "FROM [" + My.Settings.Shoreline_Feature_Layer + "] " &
                                    "WHERE [" + My.Settings.Shoreline_Type_Field + "] IS NULL OR [" + My.Settings.Shoreline_Type_Field + "] NOT IN ('MHW', 'HWL', 'WDL')"

                        cn = MapUtility.dbConnectionForLayer(My.Settings.Shoreline_Feature_Layer)
                        cmd = New OleDbCommand(sql, cn)
                        dr = cmd.ExecuteReader

                        If dr.Read() Then
                            msg = "Shoreline type field '" + My.Settings.Shoreline_Type_Field + "' should always have a value of 'MHW' or 'HWL'"
                            log(TraceLevel.Error, msg)
                            Return False
                        End If
                    Finally
                        If dr IsNot Nothing Then dr.Close()
                        If cn IsNot Nothing Then cn.Close()
                    End Try
                    haveShorelineTypeField = True
                Else
                    msg = String.Format("'{0}' has been set as shoreline type field but is not contained in the shoreline layer '{1}'. You can use the Attribute Automator to add this field.", My.Settings.Shoreline_Type_Field, My.Settings.Shoreline_Feature_Layer)
                    log(TraceLevel.Error, msg)
                    Return False
                End If
            Else
                If nv(My.Settings.Shoreline_Uncertainty_Table, "") <> "" Then
                    msg = String.Format("Shoreline type field selection is required when a shoreline uncertainty table has been selected.")
                    log(TraceLevel.Error, msg)
                    Return False
                End If
            End If

            If nv(My.Settings.Shoreline_Uncertainty_Table, "") <> "" Then
                Dim uncTableName As String = My.Settings.Shoreline_Uncertainty_Table
                If Not GeoDB.checkIfTableFieldExists(uncTableName, Nothing) Then
                    msg = String.Format("Selected shoreline  uncertainty table '{0}' is not found.", uncTableName)
                    log(TraceLevel.Error, msg)
                    Return False
                End If
                haveShorelineUncertaintyTable = True
            Else
                If nv(My.Settings.Shoreline_Type_Field, "") <> "" Then
                    msg = String.Format("Shoreline uncertainty table selection is required when a shoreline type field has been selected.")
                    log(TraceLevel.Error, msg)
                    Return False
                End If
            End If

            If getSeawardLandwardSettingFromMetadata(TransectLyrToolCtl.currentTransectLayer) Is Nothing Then
                msg = String.Format("Transect layer selected '{0}' does not have a landward/seaward setting recorded in the process steps of the metadata record (Data Quality - Process Step - Process Description). Since this setting is a new feature recorded during DSAS v5.0 transect creation, this information will be missing for legacy transects even after they are upgraded. DSAS defaults to the setting currently selected in the Default Parameters user interface which is '{1}' for this calculation.", TransectLyrToolCtl.currentTransectLayerName, IIf(My.Settings.Seaward, "seaward", "landward"))
                log(msg)
                MsgBox(msg,, DSAS.MsgBoxTitle)
            End If

        Catch ex As Exception
            handleException(ex)
            Return False
        Finally
            If msg <> "" Then
                log(msg)
            Else
                log("Integrity checks were successful.")
            End If
        End Try
        ' TODO: check nulls in date, group, others....
        Return True
    End Function


    Public Shared Function sortAlongBaseline() As Boolean
        Try
            Dim pGeoColl As IGeometryCollection = New Polyline
            Dim blFC As IFeatureCursor = GeoDB.getCursorForLayer(My.Settings.Baseline_Feature_Layer, "ID", "ID>0")
            Dim blFeat As IFeature
            Do
                blFeat = blFC.NextFeature
                If blFeat Is Nothing Then Exit Do
                Dim pPolyBase As IPolyline = CType(blFeat.Shape, IPolyline)
                Dim curGeoColl As IGeometryCollection
                curGeoColl = CType(pPolyBase, IGeometryCollection)
                pGeoColl.AddGeometryCollection(curGeoColl)

                'pGeoColl.AddGeometry(CType(CType(blFeat.Shape, IPolyline), IGeometryCollection))
            Loop

            Dim pMergeBasePoly As IPolyline 'single baseline poly
            pMergeBasePoly = CType(pGeoColl, IPolyline) 'Make the geo coll into a polyline

            Dim xectFC As IFeatureCursor = GeoDB.getCursorForLayer(TransectLyrToolCtl.currentTransectLayerName, "TransOrder", Nothing, True)
            Dim xectFeat As IFeature
            Dim offset As Double = 0 ' this is required for the sorted list index
            Dim sortedXects As New SortedList
            Do
                xectFeat = xectFC.NextFeature
                If xectFeat Is Nothing Then Exit Do

                Dim ptFrom As IPoint = DirectCast(xectFeat.Shape, IPolyline).FromPoint
                Dim distFrom As Double, distAlong As Double 'distances
                pMergeBasePoly.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, ptFrom, False, Nothing, distAlong, distFrom, False)
                sortedXects.Add(distAlong + offset, xectFeat)
                offset += 0.00000001 'required so index in sorted list not duplicated

                DSASUtility.ReleaseComObject(ptFrom)
            Loop

            Dim transOrderIdx As Integer = GeoDB.transFldMap("TransOrder")
            Dim i As Integer = 1
            For Each xectFeat In sortedXects.Values
                xectFeat.Value(transOrderIdx) = i
                xectFeat.Store()
                i += 1
            Next
            Return True
        Catch
            Return False
        End Try
    End Function

    Public Shared Sub createSummaryReport(ByVal rateLyrName As String, data As IronPython.Runtime.PythonDictionary, shoreDates As SortedDictionary(Of Date, Integer))
        Dim filePath As String = ""
        Dim rateLyrNameArray() As String = rateLyrName.Split("_"c)
        Dim rateLyrNameLength As Integer = rateLyrNameArray.Length
        Dim rateTimestamp As String = DirectCast(rateLyrNameArray.GetValue(rateLyrNameLength - 2), String) & "_" & DirectCast(rateLyrNameArray.GetValue(rateLyrNameLength - 1), String)
        If My.Settings.SummaryReportLocation = "" Or My.Settings.SummaryReportLocation = Nothing Then
            filePath = GeoDB.getFeatureClassPath() & "\" & "DSAS_Summary_" & rateTimestamp & ".txt"
        Else
            filePath = My.Settings.SummaryReportLocation & "_" + rateTimestamp & ".txt"
        End If

        Dim i As Integer = 0

        Dim rateFieldsStr As String = ""
        For i = 0 To DSAS.calcForm.chkAdvancedStats.CheckedItems.Count - 1
            rateFieldsStr += DSAS.calcForm.chkAdvancedStats.CheckedItems.Item(i).valueMember
            If i < DSAS.calcForm.chkAdvancedStats.CheckedItems.Count - 1 Then rateFieldsStr += ", "
        Next

        i = 0
        Dim shoreDateStr As String = ""
        For Each dt As Date In shoreDates.Keys
            shoreDateStr += dt.ToShortDateString
            If i < shoreDates.Count - 1 Then shoreDateStr += ", "
            i += 1
        Next

        Dim file As System.IO.StreamWriter = Nothing
        Try
            file = My.Computer.FileSystem.OpenTextFileWriter(filePath, True)
            file.WriteLine("File name: " & filePath.Substring(filePath.LastIndexOf("\"c) + 1))
            file.WriteLine("Timestamp of rate calculation: " & Format(Date.Now, "MM/dd/yyyy H:mm:ss"))
            file.WriteLine("DSAS version: " & DSAS.dsasVersion)
            file.WriteLine("ArcGIS version: " & ESRI.ArcGIS.RuntimeManager.ActiveRuntime.Version)
            file.WriteLine("Rate types run: " & rateFieldsStr)
            file.WriteLine("Shoreline dates used: " & shoreDateStr)
            file.WriteLine("Shoreline threshold: " & DSAS.calcForm.txtThreshold.Text)
            file.WriteLine("Confidence Interval (CI) selected: " & DSAS.calcForm.getCIValue.ToString())
            file.WriteLine("Default Uncertainty: " & My.Settings.Uncertainty.ToString())
            file.WriteLine("Transect spacing length: " & My.Settings.Spacing)
            file.WriteLine("Smoothing distance: " & My.Settings.Transect_Leg_Length)
            file.WriteLine("Coordinate system: " & My.Document.FocusMap.SpatialReference.Name)
            file.WriteLine("Is bias applied: " & IIf(applyBias(), "YES", "NO"))
            file.WriteLine()
            file.WriteLine("All rates reported are in meters/year, distance values are in meters.")

            Dim lookup As IronPython.Runtime.List = ipy.getSummaryCalcsLookup()

            Dim groups As New SortedList

            Dim rates As IronPython.Runtime.PythonDictionary = data("rates")
            For Each rate As IronPython.Runtime.PythonDictionary In ipy.getRateList()
                Dim rateName As String = rate("name")
                If Not rates.ContainsKey(rateName) Then Continue For
                file.WriteLine()
                file.WriteLine()
                file.WriteLine(String.Format("{3}: {0} ({1}, {2})", rateName, rate("alias"), rate("units"), rate("prefix")))

                If groups.Count = 0 Then
                    For Each groupName As String In rates(rateName).viewkeys
                        Dim groupId As Long
                        If Long.TryParse(groupName, groupId) Then
                            groups.Add(groupId, groupName)
                        ElseIf groupName = "A" Then
                            'overall average goes first
                            groups.Add(Long.MinValue, groupName)
                        End If
                    Next
                End If

                For Each groupId In groups.Keys
                    Dim groupName As String = groups(groupId)
                    Dim groupData As IronPython.Runtime.PythonDictionary = rates(rateName)(groupName)
                    If data("numGroups") = 1 AndAlso groupName <> "A" Then Continue For     ' If only one group then we skip regional averages
                    file.WriteLine(vbCrLf & rateName & " " & IIf(groupName = "A", "OVERALL AVERAGES: ", "REGIONAL AVERAGES" & IIf(data("numGroups") > 1, " [GROUP " & groupName & "]", "")))
                    For Each sumStats As IronPython.Runtime.PythonDictionary In lookup
                        Dim sumStatsName As String = sumStats("name")
                        If sumStatsName = "" Then
                            If rate("prefix") = "RATE" Then file.WriteLine("")    ' gap between sections
                        Else
                            If rate("units") = "m" AndAlso (sumStatsName = "Nstar" OrElse sumStatsName = "uncyOfAve" OrElse sumStatsName = "ave +/- uncyOfAve") Then
                                Continue For
                            End If
                            Dim sumStatsFriendlyName As String = sumStats("description")

                            If sumStatsName.Contains("+/-") Then
                                Dim parts As String() = sumStatsName.Split(New String() {" +/- "}, StringSplitOptions.None)
                                file.WriteLine("{0}: {1} +/- {2}", sumStatsFriendlyName, round(groupData(parts(0))), round(groupData(parts(1))))
                            Else
                                If groupData.ContainsKey(sumStatsName) Then
                                    Dim val As Object = groupData(sumStatsName)
                                    If sumStatsFriendlyName.StartsWith("percent") Then
                                        file.WriteLine(sumStatsFriendlyName & ": " & round(val * 100) & "%")
                                    ElseIf sumStatsFriendlyName.StartsWith("reduced n") Then
                                        file.WriteLine(sumStatsFriendlyName & ": " & Math.Round(val))
                                    Else
                                        file.WriteLine(sumStatsFriendlyName & ": " & round(val))
                                    End If
                                End If
                            End If
                        End If
                    Next
                Next
            Next
        Finally
            If file IsNot Nothing Then file.Close()
        End Try
    End Sub


End Class
