<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShorelineForecastForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ShorelineForecastForm))
        Me.lblRateLayer = New System.Windows.Forms.Label()
        Me.listboxRateLayer = New System.Windows.Forms.ListBox()
        Me.lblForecastPeriod = New System.Windows.Forms.Label()
        Me.btnForecastTimeHelp = New System.Windows.Forms.PictureBox()
        Me.btnRunForecast = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.checkboxTenYears = New System.Windows.Forms.CheckBox()
        Me.checkboxTwentyYears = New System.Windows.Forms.CheckBox()
        Me.btnForecastLayerHelp = New System.Windows.Forms.PictureBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        CType(Me.btnForecastTimeHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btnForecastLayerHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblRateLayer
        '
        Me.lblRateLayer.AutoSize = True
        Me.lblRateLayer.Location = New System.Drawing.Point(8, 74)
        Me.lblRateLayer.Name = "lblRateLayer"
        Me.lblRateLayer.Size = New System.Drawing.Size(118, 13)
        Me.lblRateLayer.TabIndex = 0
        Me.lblRateLayer.Text = "Select Rate Data Layer"
        '
        'listboxRateLayer
        '
        Me.listboxRateLayer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.listboxRateLayer.FormattingEnabled = True
        Me.listboxRateLayer.Location = New System.Drawing.Point(11, 100)
        Me.listboxRateLayer.Name = "listboxRateLayer"
        Me.listboxRateLayer.Size = New System.Drawing.Size(263, 95)
        Me.listboxRateLayer.TabIndex = 1
        '
        'lblForecastPeriod
        '
        Me.lblForecastPeriod.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblForecastPeriod.AutoSize = True
        Me.lblForecastPeriod.Location = New System.Drawing.Point(9, 206)
        Me.lblForecastPeriod.Name = "lblForecastPeriod"
        Me.lblForecastPeriod.Size = New System.Drawing.Size(151, 13)
        Me.lblForecastPeriod.TabIndex = 2
        Me.lblForecastPeriod.Text = "Select Forecast Time Period(s)"
        '
        'btnForecastTimeHelp
        '
        Me.btnForecastTimeHelp.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnForecastTimeHelp.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnForecastTimeHelp.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.btnForecastTimeHelp.InitialImage = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.btnForecastTimeHelp.Location = New System.Drawing.Point(166, 197)
        Me.btnForecastTimeHelp.Name = "btnForecastTimeHelp"
        Me.btnForecastTimeHelp.Size = New System.Drawing.Size(28, 31)
        Me.btnForecastTimeHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.btnForecastTimeHelp.TabIndex = 5
        Me.btnForecastTimeHelp.TabStop = False
        '
        'btnRunForecast
        '
        Me.btnRunForecast.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRunForecast.Enabled = False
        Me.btnRunForecast.Location = New System.Drawing.Point(199, 285)
        Me.btnRunForecast.Name = "btnRunForecast"
        Me.btnRunForecast.Size = New System.Drawing.Size(75, 23)
        Me.btnRunForecast.TabIndex = 6
        Me.btnRunForecast.Text = "Run"
        Me.btnRunForecast.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(105, 285)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'checkboxTenYears
        '
        Me.checkboxTenYears.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.checkboxTenYears.AutoSize = True
        Me.checkboxTenYears.Location = New System.Drawing.Point(12, 232)
        Me.checkboxTenYears.Name = "checkboxTenYears"
        Me.checkboxTenYears.Size = New System.Drawing.Size(66, 17)
        Me.checkboxTenYears.TabIndex = 8
        Me.checkboxTenYears.Text = "10 years"
        Me.checkboxTenYears.UseVisualStyleBackColor = True
        '
        'checkboxTwentyYears
        '
        Me.checkboxTwentyYears.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.checkboxTwentyYears.AutoSize = True
        Me.checkboxTwentyYears.Location = New System.Drawing.Point(102, 232)
        Me.checkboxTwentyYears.Name = "checkboxTwentyYears"
        Me.checkboxTwentyYears.Size = New System.Drawing.Size(66, 17)
        Me.checkboxTwentyYears.TabIndex = 9
        Me.checkboxTwentyYears.Text = "20 years"
        Me.checkboxTwentyYears.UseVisualStyleBackColor = True
        '
        'btnForecastLayerHelp
        '
        Me.btnForecastLayerHelp.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnForecastLayerHelp.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.btnForecastLayerHelp.InitialImage = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.btnForecastLayerHelp.Location = New System.Drawing.Point(132, 65)
        Me.btnForecastLayerHelp.Name = "btnForecastLayerHelp"
        Me.btnForecastLayerHelp.Size = New System.Drawing.Size(28, 31)
        Me.btnForecastLayerHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.btnForecastLayerHelp.TabIndex = 10
        Me.btnForecastLayerHelp.TabStop = False
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.Control
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Location = New System.Drawing.Point(11, 12)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(263, 47)
        Me.TextBox1.TabIndex = 11
        Me.TextBox1.Text = "DSAS requires a minimum of four shoreline positions to forecast and will skip ove" &
    "r any transect with three or fewer shorelines."
        '
        'TextBox2
        '
        Me.TextBox2.BackColor = System.Drawing.SystemColors.Control
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Location = New System.Drawing.Point(11, 258)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(263, 13)
        Me.TextBox2.TabIndex = 12
        Me.TextBox2.Text = "Forecast time period starts at run date."
        '
        'ShorelineForecastForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(286, 320)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.btnForecastLayerHelp)
        Me.Controls.Add(Me.checkboxTwentyYears)
        Me.Controls.Add(Me.checkboxTenYears)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnRunForecast)
        Me.Controls.Add(Me.btnForecastTimeHelp)
        Me.Controls.Add(Me.lblForecastPeriod)
        Me.Controls.Add(Me.listboxRateLayer)
        Me.Controls.Add(Me.lblRateLayer)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ShorelineForecastForm"
        Me.Text = "Shoreline Forecasting"
        CType(Me.btnForecastTimeHelp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btnForecastLayerHelp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblRateLayer As System.Windows.Forms.Label
    Friend WithEvents listboxRateLayer As System.Windows.Forms.ListBox
    Friend WithEvents lblForecastPeriod As System.Windows.Forms.Label
    Friend WithEvents btnForecastTimeHelp As System.Windows.Forms.PictureBox
    Friend WithEvents btnRunForecast As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents checkboxTenYears As System.Windows.Forms.CheckBox
    Friend WithEvents checkboxTwentyYears As System.Windows.Forms.CheckBox
    Friend WithEvents btnForecastLayerHelp As System.Windows.Forms.PictureBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
End Class
