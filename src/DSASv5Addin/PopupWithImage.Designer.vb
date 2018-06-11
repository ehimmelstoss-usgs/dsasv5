<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PopupWithImage
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.pictureBox = New System.Windows.Forms.PictureBox()
        Me.textArea = New System.Windows.Forms.TextBox()
        CType(Me.pictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(268, 492)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Close"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'pictureBox
        '
        Me.pictureBox.Location = New System.Drawing.Point(14, 171)
        Me.pictureBox.Name = "pictureBox"
        Me.pictureBox.Size = New System.Drawing.Size(330, 315)
        Me.pictureBox.TabIndex = 2
        Me.pictureBox.TabStop = False
        '
        'textArea
        '
        Me.textArea.BackColor = System.Drawing.SystemColors.Control
        Me.textArea.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.textArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.textArea.Location = New System.Drawing.Point(14, 13)
        Me.textArea.Multiline = True
        Me.textArea.Name = "textArea"
        Me.textArea.Size = New System.Drawing.Size(329, 152)
        Me.textArea.TabIndex = 3
        '
        'PopupWithImage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(357, 522)
        Me.Controls.Add(Me.textArea)
        Me.Controls.Add(Me.pictureBox)
        Me.Controls.Add(Me.Button1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "PopupWithImage"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.ShowIcon = False
        CType(Me.pictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents pictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents textArea As System.Windows.Forms.TextBox
End Class
