<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SCEClipForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SCEClipForm))
        Me.btnDone = New System.Windows.Forms.Button()
        Me.btnClip = New System.Windows.Forms.Button()
        Me.cbRates = New System.Windows.Forms.ComboBox()
        Me.lblDummy = New System.Windows.Forms.Label()
        Me.btnScaleToMyData = New System.Windows.Forms.Button()
        Me.btnApplyColorRamp = New System.Windows.Forms.Button()
        Me.rateField = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cbRatesColorRamp = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabControl1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnDone
        '
        Me.btnDone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDone.Location = New System.Drawing.Point(251, 213)
        Me.btnDone.Name = "btnDone"
        Me.btnDone.Size = New System.Drawing.Size(75, 23)
        Me.btnDone.TabIndex = 7
        Me.btnDone.Text = "Cancel/Exit"
        Me.btnDone.UseVisualStyleBackColor = True
        '
        'btnClip
        '
        Me.btnClip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClip.Location = New System.Drawing.Point(210, 124)
        Me.btnClip.Name = "btnClip"
        Me.btnClip.Size = New System.Drawing.Size(75, 23)
        Me.btnClip.TabIndex = 6
        Me.btnClip.Text = "Clip"
        Me.btnClip.UseVisualStyleBackColor = True
        '
        'cbRates
        '
        Me.cbRates.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbRates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbRates.FormattingEnabled = True
        Me.cbRates.Location = New System.Drawing.Point(9, 33)
        Me.cbRates.Name = "cbRates"
        Me.cbRates.Size = New System.Drawing.Size(276, 21)
        Me.cbRates.TabIndex = 8
        '
        'lblDummy
        '
        Me.lblDummy.AutoSize = True
        Me.lblDummy.Location = New System.Drawing.Point(9, 15)
        Me.lblDummy.Name = "lblDummy"
        Me.lblDummy.Size = New System.Drawing.Size(220, 13)
        Me.lblDummy.TabIndex = 9
        Me.lblDummy.Text = "Select transect layer to create a clipped copy"
        '
        'btnScaleToMyData
        '
        Me.btnScaleToMyData.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnScaleToMyData.Location = New System.Drawing.Point(184, 124)
        Me.btnScaleToMyData.Name = "btnScaleToMyData"
        Me.btnScaleToMyData.Size = New System.Drawing.Size(98, 23)
        Me.btnScaleToMyData.TabIndex = 16
        Me.btnScaleToMyData.Text = "Scale to my data"
        Me.btnScaleToMyData.UseVisualStyleBackColor = True
        '
        'btnApplyColorRamp
        '
        Me.btnApplyColorRamp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApplyColorRamp.Location = New System.Drawing.Point(80, 124)
        Me.btnApplyColorRamp.Name = "btnApplyColorRamp"
        Me.btnApplyColorRamp.Size = New System.Drawing.Size(98, 23)
        Me.btnApplyColorRamp.TabIndex = 12
        Me.btnApplyColorRamp.Text = "Apply color ramp"
        Me.btnApplyColorRamp.UseVisualStyleBackColor = True
        '
        'rateField
        '
        Me.rateField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.rateField.FormattingEnabled = True
        Me.rateField.Location = New System.Drawing.Point(9, 81)
        Me.rateField.Name = "rateField"
        Me.rateField.Size = New System.Drawing.Size(273, 21)
        Me.rateField.TabIndex = 15
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(160, 13)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Select rate for color ramp display"
        '
        'cbRatesColorRamp
        '
        Me.cbRatesColorRamp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbRatesColorRamp.FormattingEnabled = True
        Me.cbRatesColorRamp.Location = New System.Drawing.Point(9, 31)
        Me.cbRatesColorRamp.Name = "cbRatesColorRamp"
        Me.cbRatesColorRamp.Size = New System.Drawing.Size(273, 21)
        Me.cbRatesColorRamp.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(244, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Select rate layer to visualize rates using color ramp"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Location = New System.Drawing.Point(16, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(310, 187)
        Me.TabControl1.TabIndex = 14
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.btnScaleToMyData)
        Me.TabPage2.Controls.Add(Me.Label2)
        Me.TabPage2.Controls.Add(Me.btnApplyColorRamp)
        Me.TabPage2.Controls.Add(Me.cbRatesColorRamp)
        Me.TabPage2.Controls.Add(Me.rateField)
        Me.TabPage2.Controls.Add(Me.Label3)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(302, 161)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Rates display"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.lblDummy)
        Me.TabPage1.Controls.Add(Me.cbRates)
        Me.TabPage1.Controls.Add(Me.btnClip)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(302, 161)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Clip rates to SCE"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'SCEClipForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(343, 248)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.btnDone)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SCEClipForm"
        Me.Text = "DSAS Data Visualization"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnDone As System.Windows.Forms.Button
    Friend WithEvents btnClip As System.Windows.Forms.Button
    Friend WithEvents cbRates As System.Windows.Forms.ComboBox
    Friend WithEvents lblDummy As System.Windows.Forms.Label
    Friend WithEvents btnApplyColorRamp As System.Windows.Forms.Button
    Friend WithEvents rateField As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cbRatesColorRamp As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnScaleToMyData As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
End Class
