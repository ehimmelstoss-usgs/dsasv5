Imports System.Windows.Forms
Imports ESRI.ArcGIS.Carto

Public Class CalcStatsForm
    Private dataFilename As String
    Private rateFeatLyr As IFeatureLayer

    Dim chkThresholdCheckedSave As Boolean
    Dim txtThresholdCheckedSave As String = ""
    Dim cboCI As String = ""

    Delegate Sub setStatusCallback(ByVal msg As String)

    Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Me.chkAdvancedStats.DisplayMember = "displayMember"
        Me.chkAdvancedStats.ValueMember = "valueMember"

        Me.rateFields.DisplayMember = "valueMember"
        Me.rateFields.ValueMember = "valueMember"

        Me.cboConfidence.DisplayMember = "displayName"
        Me.cboConfidence.ValueMember = "itemValue"
        Me.cboConfidence.Items.Add(CI.getCIFor(90D + 0.0D))
        Me.cboConfidence.Items.Add(CI.getCIFor(95D + 0.0D))
        Me.cboConfidence.Items.Add(CI.getCIFor(68.3D + 0.0D))
        Me.cboConfidence.Items.Add(CI.getCIFor(95.5D + 0.0D))
        Me.cboConfidence.Items.Add(CI.getCIFor(99.7D + 0.0D))
        Me.cboConfidence.Sorted = True
    End Sub


    Public Sub setStatus(ByVal msg As String)
        log(msg)
        Me.lblStatus.Text = withDate(msg)
        Me.forceRefresh()
    End Sub


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
        Try
            btnRun.Enabled = False
            DSAS.interruptCalcs = False

            If String.IsNullOrEmpty(cboConfidence.Text) And String.IsNullOrEmpty(tbConfidence.Text) Then
                log(TraceLevel.Error, "Please select or type a confidence interval for rate calculation.")
                Exit Sub
            End If

            If Me.chkAdvancedStats.CheckedItems.Count = 0 Then
                log(TraceLevel.Error, "No calculations to run!" + vbCrLf + "Please select one or more statistics to calculate.")
                Exit Sub
            End If

            If Me.chkThreshold.Checked AndAlso Me.txtThreshold.Int < 0 Then
                log(TraceLevel.Error, "Please enter a non-negative threshold value!")
                Exit Sub
            End If

            If ifUseColorRamp.Checked AndAlso (rateFields.SelectedItem Is Nothing) Then
                log(TraceLevel.Error, "You checked the box to display calculation results using color ramp, but you didn't select a rate for color ramp display.")
                Exit Sub
            End If

            Me.Cursor = Cursors.WaitCursor
            'Me.btnDone.Enabled = False

            'If Me.chkThreshold.Checked <> Me.chkThresholdCheckedSave OrElse Me.txtThreshold.Text <> Me.txtThresholdCheckedSave OrElse Me.cboConfidence.SelectedValue <> Me.cboCI Then
            setStatus("Performing data integrity checks...")

            If RateCalculation.checkForIntegrity() Then
                setStatus("Integrity checks completed. No issues detected.")
            Else
                setStatus("Data integrity issue(s) encountered.")
                Me.Cursor = Cursors.Default
                Me.btnDone.Enabled = True
                Exit Sub
            End If


            setStatus("Initializing...")
            ' Generate data file
            dataFilename = DSASUtility.makeDataFilename(TransectLyrToolCtl.currentTransectLayerName)
            Dim result As String = RateCalculation.walkAndCalc(TransectLyrToolCtl.currentTransectLayerName, dataFilename, IIf(Me.chkThreshold.Checked, Me.txtThreshold.Int, -1))
            If result <> "" Then
                setStatus(result)
                Me.Cursor = Cursors.Default
                log(TraceLevel.Error, result)
                Exit Sub
            End If

            Dim selectedCalcs As New ArrayList()

            For Each item As Stat In Me.chkAdvancedStats.CheckedItems
                selectedCalcs.Add(item.valueMember + " (" + item.valueMember + ")")
            Next

            setStatus("Updating metadata...")
            Metadata.updateMetadataForCalculateStats(
            "[" + String.Join(", ", selectedCalcs.ToArray("".GetType)) + "]",
            IIf(Me.chkThreshold.Checked, Me.txtThreshold.Text, "none"),
            DSASUtility.makeOutputTablename(dataFilename)
        )

            setStatus("Run Complete.")

        Catch ex As System.Exception
            setStatus("Error during execution. Please review log file.")
            DSASUtility.handleException(ex)
        Finally
            Me.Cursor = Cursors.Default
            btnRun.Enabled = True
        End Try
    End Sub


    Public Sub setRateFeatLyr(ByVal newRateFeatLyr As IFeatureLayer)
        If newRateFeatLyr Is Nothing Then
            Return
        End If

        rateFeatLyr = newRateFeatLyr
    End Sub

    Delegate Sub setCursorDefaultCallback()

    Public Sub setCursorDefault()
        ' InvokeRequired compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If Me.InvokeRequired Then
            Dim d As New setCursorDefaultCallback(AddressOf setCursorDefault)
            Me.Invoke(d, New Object() {})
        Else
            Me.Cursor = Cursors.Default
            Me.btnDone.Enabled = True
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
        DSAS.interruptCalcs = True
        updateCI()
        Me.Close()
    End Sub

    Private Sub CalcStatsForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        My.Settings.Intersect_Threshold_Enforced = Me.chkThreshold.Checked
        My.Settings.Intersect_Threshold_Value = Me.txtThreshold.Int
        My.Settings.CreateSummaryReport = Me.chkCreateSumReport.Checked
    End Sub

    Private Sub CalcStatsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.cboConfidence.SelectedItem = CI.getCIFor(My.Settings.Confidence_Interval)

        If Me.cboConfidence.SelectedIndex < 0 Then
            Me.tbConfidence.Text = My.Settings.Confidence_Interval
        End If

        Me.chkThreshold.Checked = My.Settings.Intersect_Threshold_Enforced
        Me.txtThreshold.Int = My.Settings.Intersect_Threshold_Value
        Me.txtThreshold.Enabled = Me.chkThreshold.Checked


        Stat.init()
        For Each aStat As Stat In Stat.parentStats
            Me.chkAdvancedStats.Items.Add(aStat)
        Next

        ifUseColorRamp.Checked = My.Settings.ifApplyRateColorRamp

        Me.chkCreateSumReport.Checked = My.Settings.CreateSummaryReport
        sumReportLocation.Text = GeoDB.getFeatureClassPath() & "\" & getSumReportName()
        My.Settings.SummaryReportLocation = sumReportLocation.Text
    End Sub



    Private Sub chkThreshold_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkThreshold.CheckedChanged
        Me.txtThreshold.Enabled = DirectCast(sender, CheckBox).Checked
        My.Settings.Intersect_Threshold_Enforced = Me.chkThreshold.Checked
    End Sub

    Private Function decorate(ByVal fld As String) As String
        Return fld.Replace("{CI}", Me.getCIValue())
    End Function

    Delegate Sub refreshCallback()

    Public Sub forceRefresh()
        ' InvokeRequired compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If Me.chkAdvancedStats.InvokeRequired Then
            Dim d As New refreshCallback(AddressOf forceRefresh)
            Me.Invoke(d, New Object() {})
        Else
            Me.chkAdvancedStats.Refresh()
            Me.lblStatus.Refresh()
            Application.DoEvents()
        End If
    End Sub

    Private Sub cboConfidence_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboConfidence.SelectedIndexChanged
        updateCI()
    End Sub

    Private Sub updateCI()
        My.Settings.Confidence_Interval = getCIValue()
    End Sub

    Public Function getCIValue() As Double
        If Me.cboConfidence.Enabled AndAlso Me.cboConfidence.SelectedItem IsNot Nothing Then
            Return Me.cboConfidence.SelectedItem.itemvalue
        ElseIf Me.tbConfidence.IsValid Then
            Return Me.tbConfidence.NumericText()
        Else
            Return My.Settings.Properties("Confidence_Interval").DefaultValue
        End If
    End Function

    Private Sub btnIntegrityChecks_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIntegrityChecks.Click
        If RateCalculation.checkForIntegrity() Then
            MsgBox("Integrity checks completed. No issues detected.",, DSAS.MsgBoxTitle)
            Me.btnRun.Enabled = True
        Else
            Me.btnRun.Enabled = False
        End If
    End Sub


    Private Sub btnThreshold_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnThreshold.Click
        MsgBox(
            "This option provides users with the ability to establish a minimum" + vbCrLf +
            "for the number of shorelines a transect must intersect in order for" + vbCrLf +
            "it to be included in analysis. For example, if your data consists" + vbCrLf +
            "of four historic shoreline positions, but there are gaps in coverage" + vbCrLf +
            "alongshore you may elect to set the intersection threshold to 4 " + vbCrLf +
            "so that any transect that does not intersect all four shorelines is" + vbCrLf +
            "not included in the statistical analysis. The output attribute table" + vbCrLf +
            "will simply skip over that transect and move on to the next one." + vbCrLf +
            "This provides a quality check so that all results are based on the" + vbCrLf +
            "same input data.", MsgBoxStyle.ApplicationModal, "Shoreline Intersection Threshold")
    End Sub

    Private Sub btnConfidenceClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfidenceClear.Click
        Me.tbConfidence.Text = ""
        updateCI()
    End Sub

    Private Sub tbConfidence_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbConfidence.TextChanged
        Me.cboConfidence.Enabled = sender.text Is Nothing OrElse sender.text.trim() = ""
    End Sub

    Private Sub tbConfidence_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbConfidence.Leave
        If sender.text Is Nothing OrElse sender.text.trim() = "" Then Return
        If Me.tbConfidence.IsValid Then
            updateCI()
        Else
            log(TraceLevel.Error, "A numeric value between 0.01 and 99.99 is required.")
        End If
    End Sub

    Private Sub rateColorRampBtn_Click(sender As Object, e As EventArgs) Handles rateColorRampBtn.Click
        MsgBox("DSAS will return all rate calculations as a new feature class within your geodatabase. Choosing this option will have the results automatically symbolized by the rate attributes you select from the drop-down menu. If you choose not to symbolize the rates now, or want to switch the display to a different attribute you may also access this visualization within the DSAS toolbar after rate calculations are complete." +
            vbCrLf + vbCrLf + "When EPR, LRR, or WLR rate attributes are selected for display, the default color ramp has been optimized to bin rate data based on average rates calculated for open-ocean coasts of the United States. An option to scale the color ramp to data extent is available from the visualization button on the DSAS toolbar and can be accessed after rate calculations are complete." +
            vbCrLf + vbCrLf + "When NSM or SCE distance attributes are selected for display, the default color ramp will automatically scale to the data extent by default." _
        , MsgBoxStyle.ApplicationModal, "Select Rate for Color Ramp")
    End Sub

    Private Sub chkAdvancedStats_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles chkAdvancedStats.ItemCheck

        Dim rate As Stat = chkAdvancedStats.Items.Item(e.Index)

        If e.NewValue = CheckState.Checked Then
            rateFields.Items.Add(rate)
        Else
            If rateFields.SelectedItem IsNot Nothing AndAlso DirectCast(rateFields.SelectedItem, Stat).asFieldName = rate.asFieldName Then
                'rateFields.Text = ""
                rateFields.SelectedItem = Nothing
            End If
            rateFields.Items.Remove(rate)
        End If

        If rateFields.Items.Count = 1 And ifUseColorRamp.Checked Then
            rateFields.SelectedItem = rateFields.Items.Item(0)
        End If

    End Sub

    Private Sub ifUseColorRamp_CheckedChanged(sender As Object, e As EventArgs) Handles ifUseColorRamp.CheckedChanged
        My.Settings.ifApplyRateColorRamp = ifUseColorRamp.Checked
    End Sub

    Public Shared Function getSumReportName() As String
        Return "DSAS_Summary_" & nv(TransectLyrToolCtl.currentTransectLayerName, "")
    End Function

    Private Sub btnSelectSumReportPath_Click(sender As Object, e As EventArgs) Handles btnSelectSumReportPath.Click

        Dim apppath As String = ""
        Try
            If apppath = "" Then
                Dim dialog As New FolderBrowserDialog()
                dialog.RootFolder = Environment.SpecialFolder.Desktop
                dialog.SelectedPath = System.IO.Path.GetDirectoryName(sumReportLocation.Text)
                dialog.Description = "Select summary report location"
                If dialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    apppath = dialog.SelectedPath
                End If
                sumReportLocation.Text = apppath & "\" & getSumReportName()
                My.Settings.SummaryReportLocation = sumReportLocation.Text
            End If
        Catch ex As System.Exception
            log(TraceLevel.Error, ex.Message)
        End Try

    End Sub

    Private Sub sumReportHelpBtn_Click(sender As Object, e As EventArgs) Handles sumReportHelpBtn.Click

        MsgBox("A text file will be generated with results of rate calculations summarized and saved to a specified location. The summary file will include descriptive information on the selected rate calculations including input transect file name, unique shoreline dates used, regional averages, and descriptive (min, max) values for erosion and accretion. See the DSAS user guide for a complete list of statistics compiled in the summary report." _
         , MsgBoxStyle.ApplicationModal, "DSAS Summary Report")

    End Sub

    Private Sub btnRun_DoubleClick(sender As Object, e As EventArgs) Handles btnRun.DoubleClick
        btnOK_Click(sender, e)
    End Sub

    Private Sub chkSelectAllStats_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAllStats.CheckedChanged
        For i As Integer = 0 To Me.chkAdvancedStats.Items.Count - 1
            chkAdvancedStats.SetItemChecked(i, chkSelectAllStats.Checked)
        Next
    End Sub

End Class
