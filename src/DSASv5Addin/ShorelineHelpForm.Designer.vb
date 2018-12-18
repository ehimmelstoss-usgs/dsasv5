<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ShorelineHelpForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ShorelineHelpForm))
        Me.textAreaShorelineTypeField = New System.Windows.Forms.TextBox()
        Me.LinkLabelHazardsPortal = New System.Windows.Forms.LinkLabel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'textAreaShorelineTypeField
        '
        Me.textAreaShorelineTypeField.BackColor = System.Drawing.SystemColors.Control
        Me.textAreaShorelineTypeField.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.textAreaShorelineTypeField.Location = New System.Drawing.Point(12, 12)
        Me.textAreaShorelineTypeField.Multiline = True
        Me.textAreaShorelineTypeField.Name = "textAreaShorelineTypeField"
        Me.textAreaShorelineTypeField.ReadOnly = True
        Me.textAreaShorelineTypeField.Size = New System.Drawing.Size(526, 126)
        Me.textAreaShorelineTypeField.TabIndex = 4
        Me.textAreaShorelineTypeField.TabStop = False
        Me.textAreaShorelineTypeField.Text = resources.GetString("textAreaShorelineTypeField.Text")
        '
        'LinkLabelHazardsPortal
        '
        Me.LinkLabelHazardsPortal.AutoSize = True
        Me.LinkLabelHazardsPortal.Location = New System.Drawing.Point(9, 141)
        Me.LinkLabelHazardsPortal.Name = "LinkLabelHazardsPortal"
        Me.LinkLabelHazardsPortal.Size = New System.Drawing.Size(263, 13)
        Me.LinkLabelHazardsPortal.TabIndex = 5
        Me.LinkLabelHazardsPortal.TabStop = True
        Me.LinkLabelHazardsPortal.Text = "https://marine.usgs.gov/coastalchangehazardsportal/"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(439, 161)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Close"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ShorelineHelpForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(550, 196)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.LinkLabelHazardsPortal)
        Me.Controls.Add(Me.textAreaShorelineTypeField)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ShorelineHelpForm"
        Me.Text = "Shoreline Type Field"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents textAreaShorelineTypeField As System.Windows.Forms.TextBox
    Friend WithEvents LinkLabelHazardsPortal As System.Windows.Forms.LinkLabel
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
