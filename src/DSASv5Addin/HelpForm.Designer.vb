<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HelpForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(HelpForm))
        Me.lblTop = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.llProjectPage = New System.Windows.Forms.LinkLabel()
        Me.llUserGuide = New System.Windows.Forms.LinkLabel()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.llReset = New System.Windows.Forms.LinkLabel()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblTop
        '
        Me.lblTop.AutoSize = True
        Me.lblTop.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTop.Location = New System.Drawing.Point(2, 9)
        Me.lblTop.Name = "lblTop"
        Me.lblTop.Size = New System.Drawing.Size(345, 16)
        Me.lblTop.TabIndex = 0
        Me.lblTop.Text = "Use the following links to explore DSAS help options."
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.Control
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(6, 49)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(342, 61)
        Me.TextBox1.TabIndex = 1
        Me.TextBox1.Text = "                                -- the USGS DSAS project page hosts the DSAS v5.0" &
    " addin, sample data and tutorials for using the sample data."
        '
        'llProjectPage
        '
        Me.llProjectPage.AutoSize = True
        Me.llProjectPage.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llProjectPage.Location = New System.Drawing.Point(6, 49)
        Me.llProjectPage.Name = "llProjectPage"
        Me.llProjectPage.Size = New System.Drawing.Size(128, 17)
        Me.llProjectPage.TabIndex = 2
        Me.llProjectPage.TabStop = True
        Me.llProjectPage.Text = "DSAS project page"
        '
        'llUserGuide
        '
        Me.llUserGuide.AutoSize = True
        Me.llUserGuide.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llUserGuide.Location = New System.Drawing.Point(6, 125)
        Me.llUserGuide.Name = "llUserGuide"
        Me.llUserGuide.Size = New System.Drawing.Size(109, 17)
        Me.llUserGuide.TabIndex = 4
        Me.llUserGuide.TabStop = True
        Me.llUserGuide.Text = "DSAS user guide"
        '
        'TextBox2
        '
        Me.TextBox2.BackColor = System.Drawing.SystemColors.Control
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(6, 125)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(342, 44)
        Me.TextBox2.TabIndex = 3
        Me.TextBox2.Text = "                           -- the complete user guide with detailed instructions " &
    "for use."
        '
        'llReset
        '
        Me.llReset.AutoSize = True
        Me.llReset.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llReset.Location = New System.Drawing.Point(6, 185)
        Me.llReset.Name = "llReset"
        Me.llReset.Size = New System.Drawing.Size(166, 17)
        Me.llReset.TabIndex = 6
        Me.llReset.TabStop = True
        Me.llReset.Text = "Restore hidden warnings"
        '
        'TextBox3
        '
        Me.TextBox3.BackColor = System.Drawing.SystemColors.Control
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox3.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(6, 185)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(342, 45)
        Me.TextBox3.TabIndex = 5
        Me.TextBox3.Text = "                                        -- restore warnings that you previously c" &
    "hose to hide."
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(261, 258)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'HelpForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(348, 293)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.llReset)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.llUserGuide)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.llProjectPage)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.lblTop)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "HelpForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "DSAS Help"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTop As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents llProjectPage As System.Windows.Forms.LinkLabel
    Friend WithEvents llUserGuide As System.Windows.Forms.LinkLabel
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents llReset As System.Windows.Forms.LinkLabel
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
