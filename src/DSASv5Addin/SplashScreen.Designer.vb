<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SplashScreen
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SplashScreen))
        Me.Version = New System.Windows.Forms.Label()
        Me.tbCitation = New System.Windows.Forms.TextBox()
        Me.llCitation = New System.Windows.Forms.LinkLabel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Version
        '
        Me.Version.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Version.BackColor = System.Drawing.Color.Transparent
        Me.Version.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Version.ForeColor = System.Drawing.Color.Black
        Me.Version.Location = New System.Drawing.Point(497, 343)
        Me.Version.Name = "Version"
        Me.Version.Size = New System.Drawing.Size(157, 28)
        Me.Version.TabIndex = 3
        Me.Version.Text = "Version {0}.{1}.{2}"
        Me.Version.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbCitation
        '
        Me.tbCitation.BackColor = System.Drawing.Color.White
        Me.tbCitation.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tbCitation.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbCitation.ForeColor = System.Drawing.Color.Black
        Me.tbCitation.Location = New System.Drawing.Point(25, 311)
        Me.tbCitation.Multiline = True
        Me.tbCitation.Name = "tbCitation"
        Me.tbCitation.Size = New System.Drawing.Size(637, 35)
        Me.tbCitation.TabIndex = 8
        Me.tbCitation.Text = resources.GetString("tbCitation.Text")
        '
        'llCitation
        '
        Me.llCitation.ActiveLinkColor = System.Drawing.Color.Black
        Me.llCitation.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llCitation.AutoSize = True
        Me.llCitation.BackColor = System.Drawing.Color.White
        Me.llCitation.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llCitation.ForeColor = System.Drawing.Color.Black
        Me.llCitation.LinkColor = System.Drawing.Color.Black
        Me.llCitation.Location = New System.Drawing.Point(23, 349)
        Me.llCitation.Name = "llCitation"
        Me.llCitation.Size = New System.Drawing.Size(199, 17)
        Me.llCitation.TabIndex = 9
        Me.llCitation.TabStop = True
        Me.llCitation.Text = "https://code.usgs.gov/usgs/dsas"
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.Transparent
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Arial Rounded MT Bold", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(2, 2)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(30, 31)
        Me.btnClose.TabIndex = 10
        Me.btnClose.Text = "X"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'SplashScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.DSASAddin.My.Resources.Resources.AboutDSAS
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(680, 380)
        Me.ControlBox = False
        Me.Controls.Add(Me.tbCitation)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Version)
        Me.Controls.Add(Me.llCitation)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SplashScreen"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Version As System.Windows.Forms.Label
    Friend WithEvents tbCitation As System.Windows.Forms.TextBox
    Friend WithEvents llCitation As System.Windows.Forms.LinkLabel
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
