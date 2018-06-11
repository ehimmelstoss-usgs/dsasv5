<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CalcStatsForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CalcStatsForm))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkSelectAllStats = New System.Windows.Forms.CheckBox()
        Me.chkAdvancedStats = New System.Windows.Forms.CheckedListBox()
        Me.btnDone = New System.Windows.Forms.Button()
        Me.btnRun = New System.Windows.Forms.Button()
        Me.chkThreshold = New System.Windows.Forms.CheckBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnThreshold = New System.Windows.Forms.PictureBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.btnConfidenceClear = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbConfidence = New AMS.TextBox.NumericTextBox()
        Me.lblConfidence = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cboConfidence = New System.Windows.Forms.ComboBox()
        Me.txtThreshold = New AMS.TextBox.NumericTextBox()
        Me.btnIntegrityChecks = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.rateColorRampBtn = New System.Windows.Forms.PictureBox()
        Me.rateFields = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ifUseColorRamp = New System.Windows.Forms.CheckBox()
        Me.summarReportGroupBox = New System.Windows.Forms.GroupBox()
        Me.btnSelectSumReportPath = New System.Windows.Forms.PictureBox()
        Me.sumReportLocation = New System.Windows.Forms.TextBox()
        Me.sumReportHelpBtn = New System.Windows.Forms.PictureBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkCreateSumReport = New System.Windows.Forms.CheckBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.btnThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.rateColorRampBtn, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.summarReportGroupBox.SuspendLayout()
        CType(Me.btnSelectSumReportPath, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sumReportHelpBtn, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox5.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.chkSelectAllStats)
        Me.GroupBox1.Controls.Add(Me.chkAdvancedStats)
        Me.GroupBox1.Controls.Add(Me.btnIntegrityChecks)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 7)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(598, 133)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Select Statistics to Calculate"
        '
        'chkSelectAllStats
        '
        Me.chkSelectAllStats.AutoSize = True
        Me.chkSelectAllStats.Location = New System.Drawing.Point(9, 19)
        Me.chkSelectAllStats.Name = "chkSelectAllStats"
        Me.chkSelectAllStats.Size = New System.Drawing.Size(69, 17)
        Me.chkSelectAllStats.TabIndex = 10
        Me.chkSelectAllStats.Text = "Select all"
        Me.chkSelectAllStats.UseVisualStyleBackColor = True
        '
        'chkAdvancedStats
        '
        Me.chkAdvancedStats.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkAdvancedStats.CheckOnClick = True
        Me.chkAdvancedStats.FormattingEnabled = True
        Me.chkAdvancedStats.Location = New System.Drawing.Point(6, 39)
        Me.chkAdvancedStats.Name = "chkAdvancedStats"
        Me.chkAdvancedStats.Size = New System.Drawing.Size(586, 79)
        Me.chkAdvancedStats.TabIndex = 9
        '
        'btnDone
        '
        Me.btnDone.Location = New System.Drawing.Point(461, 315)
        Me.btnDone.Name = "btnDone"
        Me.btnDone.Size = New System.Drawing.Size(75, 23)
        Me.btnDone.TabIndex = 5
        Me.btnDone.Text = "Cancel/Exit"
        Me.btnDone.UseVisualStyleBackColor = True
        '
        'btnRun
        '
        Me.btnRun.Location = New System.Drawing.Point(538, 315)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(62, 23)
        Me.btnRun.TabIndex = 4
        Me.btnRun.Text = "Calculate"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'chkThreshold
        '
        Me.chkThreshold.AutoSize = True
        Me.chkThreshold.Location = New System.Drawing.Point(11, 19)
        Me.chkThreshold.Name = "chkThreshold"
        Me.chkThreshold.Size = New System.Drawing.Size(203, 17)
        Me.chkThreshold.TabIndex = 7
        Me.chkThreshold.Text = "Apply shoreline intersection threshold:"
        Me.chkThreshold.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.summarReportGroupBox)
        Me.GroupBox2.Controls.Add(Me.GroupBox4)
        Me.GroupBox2.Controls.Add(Me.GroupBox5)
        Me.GroupBox2.Controls.Add(Me.GroupBox3)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 146)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(598, 153)
        Me.GroupBox2.TabIndex = 8
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Additional Parameters"
        '
        'btnThreshold
        '
        Me.btnThreshold.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.btnThreshold.Location = New System.Drawing.Point(257, 19)
        Me.btnThreshold.Name = "btnThreshold"
        Me.btnThreshold.Size = New System.Drawing.Size(28, 20)
        Me.btnThreshold.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.btnThreshold.TabIndex = 18
        Me.btnThreshold.TabStop = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnConfidenceClear)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.tbConfidence)
        Me.GroupBox3.Controls.Add(Me.lblConfidence)
        Me.GroupBox3.Controls.Add(Me.Label13)
        Me.GroupBox3.Controls.Add(Me.cboConfidence)
        Me.GroupBox3.Location = New System.Drawing.Point(11, 82)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(294, 51)
        Me.GroupBox3.TabIndex = 17
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Confidence Interval"
        '
        'btnConfidenceClear
        '
        Me.btnConfidenceClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnConfidenceClear.Location = New System.Drawing.Point(246, 18)
        Me.btnConfidenceClear.Name = "btnConfidenceClear"
        Me.btnConfidenceClear.Size = New System.Drawing.Size(42, 25)
        Me.btnConfidenceClear.TabIndex = 16
        Me.btnConfidenceClear.Text = "Clear"
        Me.btnConfidenceClear.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(128, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "or, type:"
        '
        'tbConfidence
        '
        Me.tbConfidence.AllowNegative = False
        Me.tbConfidence.DigitsInGroup = 0
        Me.tbConfidence.Flags = 65536
        Me.tbConfidence.Location = New System.Drawing.Point(179, 21)
        Me.tbConfidence.MaxDecimalPlaces = 2
        Me.tbConfidence.MaxLength = 5
        Me.tbConfidence.MaxWholeDigits = 2
        Me.tbConfidence.Name = "tbConfidence"
        Me.tbConfidence.Prefix = ""
        Me.tbConfidence.RangeMax = 99.99R
        Me.tbConfidence.RangeMin = 0.01R
        Me.tbConfidence.Size = New System.Drawing.Size(46, 20)
        Me.tbConfidence.TabIndex = 17
        '
        'lblConfidence
        '
        Me.lblConfidence.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblConfidence.AutoSize = True
        Me.lblConfidence.Location = New System.Drawing.Point(6, 25)
        Me.lblConfidence.Name = "lblConfidence"
        Me.lblConfidence.Size = New System.Drawing.Size(31, 13)
        Me.lblConfidence.TabIndex = 11
        Me.lblConfidence.Text = "Pick:"
        '
        'Label13
        '
        Me.Label13.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(228, 24)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(15, 13)
        Me.Label13.TabIndex = 12
        Me.Label13.Text = "%"
        '
        'cboConfidence
        '
        Me.cboConfidence.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cboConfidence.FormattingEnabled = True
        Me.cboConfidence.Location = New System.Drawing.Point(43, 21)
        Me.cboConfidence.Name = "cboConfidence"
        Me.cboConfidence.Size = New System.Drawing.Size(79, 21)
        Me.cboConfidence.TabIndex = 10
        '
        'txtThreshold
        '
        Me.txtThreshold.AllowNegative = False
        Me.txtThreshold.DigitsInGroup = 0
        Me.txtThreshold.Flags = 65536
        Me.txtThreshold.Location = New System.Drawing.Point(220, 19)
        Me.txtThreshold.MaxDecimalPlaces = 0
        Me.txtThreshold.MaxLength = 3
        Me.txtThreshold.MaxWholeDigits = 3
        Me.txtThreshold.Name = "txtThreshold"
        Me.txtThreshold.Prefix = ""
        Me.txtThreshold.RangeMax = 999.0R
        Me.txtThreshold.RangeMin = 1.0R
        Me.txtThreshold.Size = New System.Drawing.Size(31, 20)
        Me.txtThreshold.TabIndex = 13
        Me.txtThreshold.Text = "1"
        '
        'btnIntegrityChecks
        '
        Me.btnIntegrityChecks.Location = New System.Drawing.Point(466, 13)
        Me.btnIntegrityChecks.Name = "btnIntegrityChecks"
        Me.btnIntegrityChecks.Size = New System.Drawing.Size(126, 23)
        Me.btnIntegrityChecks.TabIndex = 14
        Me.btnIntegrityChecks.Text = "Run Integrity Checks"
        Me.btnIntegrityChecks.UseVisualStyleBackColor = True
        Me.btnIntegrityChecks.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoEllipsis = True
        Me.lblStatus.Location = New System.Drawing.Point(5, 315)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(450, 23)
        Me.lblStatus.TabIndex = 9
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.rateColorRampBtn)
        Me.GroupBox4.Controls.Add(Me.rateFields)
        Me.GroupBox4.Controls.Add(Me.Label2)
        Me.GroupBox4.Controls.Add(Me.ifUseColorRamp)
        Me.GroupBox4.Location = New System.Drawing.Point(311, 7)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(281, 71)
        Me.GroupBox4.TabIndex = 16
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Outputs"
        '
        'rateColorRampBtn
        '
        Me.rateColorRampBtn.Cursor = System.Windows.Forms.Cursors.Hand
        Me.rateColorRampBtn.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.rateColorRampBtn.Location = New System.Drawing.Point(252, 43)
        Me.rateColorRampBtn.Name = "rateColorRampBtn"
        Me.rateColorRampBtn.Size = New System.Drawing.Size(23, 21)
        Me.rateColorRampBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.rateColorRampBtn.TabIndex = 3
        Me.rateColorRampBtn.TabStop = False
        '
        'rateFields
        '
        Me.rateFields.FormattingEnabled = True
        Me.rateFields.Location = New System.Drawing.Point(189, 43)
        Me.rateFields.Name = "rateFields"
        Me.rateFields.Size = New System.Drawing.Size(60, 21)
        Me.rateFields.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(179, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Select Rate For Color Ramp Display:"
        '
        'ifUseColorRamp
        '
        Me.ifUseColorRamp.AutoSize = True
        Me.ifUseColorRamp.Location = New System.Drawing.Point(9, 22)
        Me.ifUseColorRamp.Name = "ifUseColorRamp"
        Me.ifUseColorRamp.Size = New System.Drawing.Size(227, 17)
        Me.ifUseColorRamp.TabIndex = 0
        Me.ifUseColorRamp.Text = "Display calculation results using color ramp"
        Me.ifUseColorRamp.UseVisualStyleBackColor = True
        '
        'summarReportGroupBox
        '
        Me.summarReportGroupBox.Controls.Add(Me.btnSelectSumReportPath)
        Me.summarReportGroupBox.Controls.Add(Me.sumReportLocation)
        Me.summarReportGroupBox.Controls.Add(Me.sumReportHelpBtn)
        Me.summarReportGroupBox.Controls.Add(Me.Label3)
        Me.summarReportGroupBox.Controls.Add(Me.chkCreateSumReport)
        Me.summarReportGroupBox.Location = New System.Drawing.Point(311, 77)
        Me.summarReportGroupBox.Name = "summarReportGroupBox"
        Me.summarReportGroupBox.Size = New System.Drawing.Size(281, 69)
        Me.summarReportGroupBox.TabIndex = 17
        Me.summarReportGroupBox.TabStop = False
        '
        'btnSelectSumReportPath
        '
        Me.btnSelectSumReportPath.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnSelectSumReportPath.Image = Global.DSASAddin.My.Resources.Resources.folder
        Me.btnSelectSumReportPath.Location = New System.Drawing.Point(224, 40)
        Me.btnSelectSumReportPath.Name = "btnSelectSumReportPath"
        Me.btnSelectSumReportPath.Size = New System.Drawing.Size(23, 21)
        Me.btnSelectSumReportPath.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.btnSelectSumReportPath.TabIndex = 5
        Me.btnSelectSumReportPath.TabStop = False
        '
        'sumReportLocation
        '
        Me.sumReportLocation.Location = New System.Drawing.Point(54, 40)
        Me.sumReportLocation.Name = "sumReportLocation"
        Me.sumReportLocation.Size = New System.Drawing.Size(164, 20)
        Me.sumReportLocation.TabIndex = 4
        '
        'sumReportHelpBtn
        '
        Me.sumReportHelpBtn.Cursor = System.Windows.Forms.Cursors.Hand
        Me.sumReportHelpBtn.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.sumReportHelpBtn.Location = New System.Drawing.Point(252, 40)
        Me.sumReportHelpBtn.Name = "sumReportHelpBtn"
        Me.sumReportHelpBtn.Size = New System.Drawing.Size(23, 21)
        Me.sumReportHelpBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.sumReportHelpBtn.TabIndex = 3
        Me.sumReportHelpBtn.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 43)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(48, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Location"
        '
        'chkCreateSumReport
        '
        Me.chkCreateSumReport.AutoSize = True
        Me.chkCreateSumReport.Location = New System.Drawing.Point(7, 19)
        Me.chkCreateSumReport.Name = "chkCreateSumReport"
        Me.chkCreateSumReport.Size = New System.Drawing.Size(170, 17)
        Me.chkCreateSumReport.TabIndex = 0
        Me.chkCreateSumReport.Text = "Create DSAS Summary Report"
        Me.chkCreateSumReport.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.txtThreshold)
        Me.GroupBox5.Controls.Add(Me.chkThreshold)
        Me.GroupBox5.Controls.Add(Me.btnThreshold)
        Me.GroupBox5.Location = New System.Drawing.Point(11, 27)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(294, 51)
        Me.GroupBox5.TabIndex = 19
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Intersection Threshold"
        '
        'CalcStatsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(610, 347)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.btnDone)
        Me.Controls.Add(Me.btnRun)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(550, 300)
        Me.Name = "CalcStatsForm"
        Me.Text = "Calculate Rates"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.btnThreshold, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.rateColorRampBtn, System.ComponentModel.ISupportInitialize).EndInit()
        Me.summarReportGroupBox.ResumeLayout(False)
        Me.summarReportGroupBox.PerformLayout()
        CType(Me.btnSelectSumReportPath, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sumReportHelpBtn, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnDone As System.Windows.Forms.Button
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents chkThreshold As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkAdvancedStats As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents lblConfidence As System.Windows.Forms.Label
    Friend WithEvents cboConfidence As System.Windows.Forms.ComboBox
    Friend WithEvents txtThreshold As AMS.TextBox.NumericTextBox
    Friend WithEvents btnIntegrityChecks As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents tbConfidence As AMS.TextBox.NumericTextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnConfidenceClear As System.Windows.Forms.Button
    Friend WithEvents btnThreshold As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents ifUseColorRamp As System.Windows.Forms.CheckBox
    Friend WithEvents rateColorRampBtn As System.Windows.Forms.PictureBox
    Friend WithEvents rateFields As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents summarReportGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents btnSelectSumReportPath As System.Windows.Forms.PictureBox
    Friend WithEvents sumReportLocation As System.Windows.Forms.TextBox
    Friend WithEvents sumReportHelpBtn As System.Windows.Forms.PictureBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkCreateSumReport As System.Windows.Forms.CheckBox
    Friend WithEvents chkSelectAllStats As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
End Class
