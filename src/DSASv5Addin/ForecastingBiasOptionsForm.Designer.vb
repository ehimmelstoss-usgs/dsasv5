<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ForecastingBiasOptionsForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ForecastingBiasOptionsForm))
        Me.btnStop = New System.Windows.Forms.Button()
        Me.textArea = New System.Windows.Forms.TextBox()
        Me.btnContinueWithAvgBias = New System.Windows.Forms.Button()
        Me.btnSkipNoBiasTransects = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnStop
        '
        Me.btnStop.Location = New System.Drawing.Point(23, 231)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(212, 23)
        Me.btnStop.TabIndex = 0
        Me.btnStop.Tag = "stop"
        Me.btnStop.Text = "Stop forecasting"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'textArea
        '
        Me.textArea.BackColor = System.Drawing.SystemColors.Control
        Me.textArea.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.textArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.textArea.Location = New System.Drawing.Point(12, 12)
        Me.textArea.Multiline = True
        Me.textArea.Name = "textArea"
        Me.textArea.Size = New System.Drawing.Size(237, 138)
        Me.textArea.TabIndex = 4
        Me.textArea.Text = resources.GetString("textArea.Text")
        '
        'btnContinueWithAvgBias
        '
        Me.btnContinueWithAvgBias.Location = New System.Drawing.Point(23, 199)
        Me.btnContinueWithAvgBias.Name = "btnContinueWithAvgBias"
        Me.btnContinueWithAvgBias.Size = New System.Drawing.Size(212, 23)
        Me.btnContinueWithAvgBias.TabIndex = 5
        Me.btnContinueWithAvgBias.Tag = "continue"
        Me.btnContinueWithAvgBias.Text = "Continue with regionally averaged bias"
        Me.btnContinueWithAvgBias.UseVisualStyleBackColor = True
        '
        'btnSkipNoBiasTransects
        '
        Me.btnSkipNoBiasTransects.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.btnSkipNoBiasTransects.Location = New System.Drawing.Point(23, 167)
        Me.btnSkipNoBiasTransects.Name = "btnSkipNoBiasTransects"
        Me.btnSkipNoBiasTransects.Size = New System.Drawing.Size(212, 23)
        Me.btnSkipNoBiasTransects.TabIndex = 6
        Me.btnSkipNoBiasTransects.Tag = "skip"
        Me.btnSkipNoBiasTransects.Text = "Skip transects with no bias data"
        Me.btnSkipNoBiasTransects.UseVisualStyleBackColor = True
        '
        'ForecastingBiasOptionsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(257, 276)
        Me.Controls.Add(Me.btnSkipNoBiasTransects)
        Me.Controls.Add(Me.btnContinueWithAvgBias)
        Me.Controls.Add(Me.textArea)
        Me.Controls.Add(Me.btnStop)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ForecastingBiasOptionsForm"
        Me.Text = "DSAS Alert"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnStop As System.Windows.Forms.Button
    Friend WithEvents textArea As System.Windows.Forms.TextBox
    Friend WithEvents btnContinueWithAvgBias As System.Windows.Forms.Button
    Friend WithEvents btnSkipNoBiasTransects As System.Windows.Forms.Button
End Class
