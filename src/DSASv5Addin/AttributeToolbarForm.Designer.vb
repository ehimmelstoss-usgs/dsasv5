<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AttributeToolbarForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AttributeToolbarForm))
        Me.layerListControl = New System.Windows.Forms.ListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dateFieldNameControl = New System.Windows.Forms.TextBox()
        Me.UncyFieldNameControl = New System.Windows.Forms.TextBox()
        Me.IfAddDateFieldControl = New System.Windows.Forms.CheckBox()
        Me.IfAddUncyFieldControl = New System.Windows.Forms.CheckBox()
        Me.AddFieldsButton = New System.Windows.Forms.Button()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.IfAddShorelineTypeFieldControl = New System.Windows.Forms.CheckBox()
        Me.shorelineTypeFieldContrl = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.textArea = New System.Windows.Forms.TextBox()
        Me.GroupBoxShoreline = New System.Windows.Forms.GroupBox()
        Me.GroupBoxBaseline = New System.Windows.Forms.GroupBox()
        Me.baselineListCtrl = New System.Windows.Forms.ListBox()
        Me.ifAddSearchDistanceNameControl = New System.Windows.Forms.CheckBox()
        Me.labelIdFieldName = New System.Windows.Forms.Label()
        Me.searchDistanceFieldNameControl = New System.Windows.Forms.TextBox()
        Me.labelGroupFieldName = New System.Windows.Forms.Label()
        Me.labelSearchDistanceFieldName = New System.Windows.Forms.Label()
        Me.idFieldNameControl = New System.Windows.Forms.TextBox()
        Me.btnBaselineClose = New System.Windows.Forms.Button()
        Me.groupFiledNameControl = New System.Windows.Forms.TextBox()
        Me.btnBaselineAddFields = New System.Windows.Forms.Button()
        Me.ifAddIdFieldNameControl = New System.Windows.Forms.CheckBox()
        Me.ifAddGroupFieldNameControl = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBoxShoreline.SuspendLayout()
        Me.GroupBoxBaseline.SuspendLayout()
        Me.SuspendLayout()
        '
        'layerListControl
        '
        Me.layerListControl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.layerListControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.layerListControl.FormattingEnabled = True
        Me.layerListControl.ItemHeight = 15
        Me.layerListControl.Location = New System.Drawing.Point(9, 21)
        Me.layerListControl.Name = "layerListControl"
        Me.layerListControl.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.layerListControl.Size = New System.Drawing.Size(379, 64)
        Me.layerListControl.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 102)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(117, 17)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Date Field Name:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 141)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(159, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Uncertainty Field Name:"
        '
        'dateFieldNameControl
        '
        Me.dateFieldNameControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dateFieldNameControl.Location = New System.Drawing.Point(203, 103)
        Me.dateFieldNameControl.Name = "dateFieldNameControl"
        Me.dateFieldNameControl.Size = New System.Drawing.Size(117, 20)
        Me.dateFieldNameControl.TabIndex = 3
        Me.dateFieldNameControl.Text = "DSAS_date"
        '
        'UncyFieldNameControl
        '
        Me.UncyFieldNameControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UncyFieldNameControl.Location = New System.Drawing.Point(203, 143)
        Me.UncyFieldNameControl.Name = "UncyFieldNameControl"
        Me.UncyFieldNameControl.Size = New System.Drawing.Size(117, 20)
        Me.UncyFieldNameControl.TabIndex = 3
        Me.UncyFieldNameControl.Text = "DSAS_uncy"
        '
        'IfAddDateFieldControl
        '
        Me.IfAddDateFieldControl.AutoSize = True
        Me.IfAddDateFieldControl.Location = New System.Drawing.Point(337, 107)
        Me.IfAddDateFieldControl.Name = "IfAddDateFieldControl"
        Me.IfAddDateFieldControl.Size = New System.Drawing.Size(15, 14)
        Me.IfAddDateFieldControl.TabIndex = 4
        Me.IfAddDateFieldControl.UseVisualStyleBackColor = True
        '
        'IfAddUncyFieldControl
        '
        Me.IfAddUncyFieldControl.AutoSize = True
        Me.IfAddUncyFieldControl.Location = New System.Drawing.Point(337, 146)
        Me.IfAddUncyFieldControl.Name = "IfAddUncyFieldControl"
        Me.IfAddUncyFieldControl.Size = New System.Drawing.Size(15, 14)
        Me.IfAddUncyFieldControl.TabIndex = 4
        Me.IfAddUncyFieldControl.UseVisualStyleBackColor = True
        '
        'AddFieldsButton
        '
        Me.AddFieldsButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AddFieldsButton.Location = New System.Drawing.Point(201, 219)
        Me.AddFieldsButton.Name = "AddFieldsButton"
        Me.AddFieldsButton.Size = New System.Drawing.Size(92, 23)
        Me.AddFieldsButton.TabIndex = 5
        Me.AddFieldsButton.Text = "Add Fields"
        Me.AddFieldsButton.UseVisualStyleBackColor = True
        '
        'CloseButton
        '
        Me.CloseButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CloseButton.Location = New System.Drawing.Point(299, 219)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(92, 23)
        Me.CloseButton.TabIndex = 5
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'IfAddShorelineTypeFieldControl
        '
        Me.IfAddShorelineTypeFieldControl.AutoSize = True
        Me.IfAddShorelineTypeFieldControl.Location = New System.Drawing.Point(337, 188)
        Me.IfAddShorelineTypeFieldControl.Name = "IfAddShorelineTypeFieldControl"
        Me.IfAddShorelineTypeFieldControl.Size = New System.Drawing.Size(15, 14)
        Me.IfAddShorelineTypeFieldControl.TabIndex = 8
        Me.IfAddShorelineTypeFieldControl.UseVisualStyleBackColor = True
        '
        'shorelineTypeFieldContrl
        '
        Me.shorelineTypeFieldContrl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.shorelineTypeFieldContrl.Location = New System.Drawing.Point(203, 182)
        Me.shorelineTypeFieldContrl.Name = "shorelineTypeFieldContrl"
        Me.shorelineTypeFieldContrl.Size = New System.Drawing.Size(117, 20)
        Me.shorelineTypeFieldContrl.TabIndex = 7
        Me.shorelineTypeFieldContrl.Text = "DSAS_type"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 183)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(181, 16)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "*Shoreline Type Field Name:"
        '
        'textArea
        '
        Me.textArea.BackColor = System.Drawing.SystemColors.Control
        Me.textArea.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.textArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.textArea.Location = New System.Drawing.Point(12, 12)
        Me.textArea.Multiline = True
        Me.textArea.Name = "textArea"
        Me.textArea.ReadOnly = True
        Me.textArea.Size = New System.Drawing.Size(414, 91)
        Me.textArea.TabIndex = 9
        Me.textArea.TabStop = False
        Me.textArea.Text = resources.GetString("textArea.Text")
        '
        'GroupBoxShoreline
        '
        Me.GroupBoxShoreline.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxShoreline.Controls.Add(Me.layerListControl)
        Me.GroupBoxShoreline.Controls.Add(Me.IfAddShorelineTypeFieldControl)
        Me.GroupBoxShoreline.Controls.Add(Me.Label2)
        Me.GroupBoxShoreline.Controls.Add(Me.shorelineTypeFieldContrl)
        Me.GroupBoxShoreline.Controls.Add(Me.Label3)
        Me.GroupBoxShoreline.Controls.Add(Me.Label4)
        Me.GroupBoxShoreline.Controls.Add(Me.dateFieldNameControl)
        Me.GroupBoxShoreline.Controls.Add(Me.CloseButton)
        Me.GroupBoxShoreline.Controls.Add(Me.UncyFieldNameControl)
        Me.GroupBoxShoreline.Controls.Add(Me.AddFieldsButton)
        Me.GroupBoxShoreline.Controls.Add(Me.IfAddDateFieldControl)
        Me.GroupBoxShoreline.Controls.Add(Me.IfAddUncyFieldControl)
        Me.GroupBoxShoreline.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxShoreline.Location = New System.Drawing.Point(12, 109)
        Me.GroupBoxShoreline.Name = "GroupBoxShoreline"
        Me.GroupBoxShoreline.Size = New System.Drawing.Size(414, 254)
        Me.GroupBoxShoreline.TabIndex = 10
        Me.GroupBoxShoreline.TabStop = False
        Me.GroupBoxShoreline.Text = "Select one or more shoreline layer(s):"
        '
        'GroupBoxBaseline
        '
        Me.GroupBoxBaseline.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxBaseline.Controls.Add(Me.baselineListCtrl)
        Me.GroupBoxBaseline.Controls.Add(Me.ifAddSearchDistanceNameControl)
        Me.GroupBoxBaseline.Controls.Add(Me.labelIdFieldName)
        Me.GroupBoxBaseline.Controls.Add(Me.searchDistanceFieldNameControl)
        Me.GroupBoxBaseline.Controls.Add(Me.labelGroupFieldName)
        Me.GroupBoxBaseline.Controls.Add(Me.labelSearchDistanceFieldName)
        Me.GroupBoxBaseline.Controls.Add(Me.idFieldNameControl)
        Me.GroupBoxBaseline.Controls.Add(Me.btnBaselineClose)
        Me.GroupBoxBaseline.Controls.Add(Me.groupFiledNameControl)
        Me.GroupBoxBaseline.Controls.Add(Me.btnBaselineAddFields)
        Me.GroupBoxBaseline.Controls.Add(Me.ifAddIdFieldNameControl)
        Me.GroupBoxBaseline.Controls.Add(Me.ifAddGroupFieldNameControl)
        Me.GroupBoxBaseline.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxBaseline.Location = New System.Drawing.Point(12, 369)
        Me.GroupBoxBaseline.Name = "GroupBoxBaseline"
        Me.GroupBoxBaseline.Size = New System.Drawing.Size(414, 254)
        Me.GroupBoxBaseline.TabIndex = 11
        Me.GroupBoxBaseline.TabStop = False
        Me.GroupBoxBaseline.Text = "Select baseline layer:"
        '
        'baselineListCtrl
        '
        Me.baselineListCtrl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.baselineListCtrl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.baselineListCtrl.FormattingEnabled = True
        Me.baselineListCtrl.ItemHeight = 15
        Me.baselineListCtrl.Location = New System.Drawing.Point(6, 21)
        Me.baselineListCtrl.Name = "baselineListCtrl"
        Me.baselineListCtrl.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.baselineListCtrl.Size = New System.Drawing.Size(379, 64)
        Me.baselineListCtrl.TabIndex = 1
        '
        'ifAddSearchDistanceNameControl
        '
        Me.ifAddSearchDistanceNameControl.AutoSize = True
        Me.ifAddSearchDistanceNameControl.Location = New System.Drawing.Point(334, 187)
        Me.ifAddSearchDistanceNameControl.Name = "ifAddSearchDistanceNameControl"
        Me.ifAddSearchDistanceNameControl.Size = New System.Drawing.Size(15, 14)
        Me.ifAddSearchDistanceNameControl.TabIndex = 8
        Me.ifAddSearchDistanceNameControl.UseVisualStyleBackColor = True
        '
        'labelIdFieldName
        '
        Me.labelIdFieldName.AutoSize = True
        Me.labelIdFieldName.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelIdFieldName.Location = New System.Drawing.Point(6, 100)
        Me.labelIdFieldName.Name = "labelIdFieldName"
        Me.labelIdFieldName.Size = New System.Drawing.Size(100, 17)
        Me.labelIdFieldName.TabIndex = 2
        Me.labelIdFieldName.Text = "ID Field Name:"
        '
        'searchDistanceFieldNameControl
        '
        Me.searchDistanceFieldNameControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.searchDistanceFieldNameControl.Location = New System.Drawing.Point(200, 181)
        Me.searchDistanceFieldNameControl.Name = "searchDistanceFieldNameControl"
        Me.searchDistanceFieldNameControl.Size = New System.Drawing.Size(117, 20)
        Me.searchDistanceFieldNameControl.TabIndex = 7
        Me.searchDistanceFieldNameControl.Text = "DSAS_search"
        '
        'labelGroupFieldName
        '
        Me.labelGroupFieldName.AutoSize = True
        Me.labelGroupFieldName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelGroupFieldName.Location = New System.Drawing.Point(6, 142)
        Me.labelGroupFieldName.Name = "labelGroupFieldName"
        Me.labelGroupFieldName.Size = New System.Drawing.Size(126, 16)
        Me.labelGroupFieldName.TabIndex = 2
        Me.labelGroupFieldName.Text = "*Group Field Name:"
        '
        'labelSearchDistanceFieldName
        '
        Me.labelSearchDistanceFieldName.AutoSize = True
        Me.labelSearchDistanceFieldName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelSearchDistanceFieldName.Location = New System.Drawing.Point(6, 184)
        Me.labelSearchDistanceFieldName.Name = "labelSearchDistanceFieldName"
        Me.labelSearchDistanceFieldName.Size = New System.Drawing.Size(188, 16)
        Me.labelSearchDistanceFieldName.TabIndex = 6
        Me.labelSearchDistanceFieldName.Text = "*Search Distance Field Name:"
        '
        'idFieldNameControl
        '
        Me.idFieldNameControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.idFieldNameControl.Location = New System.Drawing.Point(200, 100)
        Me.idFieldNameControl.Name = "idFieldNameControl"
        Me.idFieldNameControl.Size = New System.Drawing.Size(117, 20)
        Me.idFieldNameControl.TabIndex = 3
        Me.idFieldNameControl.Text = "DSAS_ID"
        '
        'btnBaselineClose
        '
        Me.btnBaselineClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBaselineClose.Location = New System.Drawing.Point(298, 220)
        Me.btnBaselineClose.Name = "btnBaselineClose"
        Me.btnBaselineClose.Size = New System.Drawing.Size(92, 23)
        Me.btnBaselineClose.TabIndex = 5
        Me.btnBaselineClose.Text = "Close"
        Me.btnBaselineClose.UseVisualStyleBackColor = True
        '
        'groupFiledNameControl
        '
        Me.groupFiledNameControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.groupFiledNameControl.Location = New System.Drawing.Point(200, 142)
        Me.groupFiledNameControl.Name = "groupFiledNameControl"
        Me.groupFiledNameControl.Size = New System.Drawing.Size(117, 20)
        Me.groupFiledNameControl.TabIndex = 3
        Me.groupFiledNameControl.Text = "DSAS_group"
        '
        'btnBaselineAddFields
        '
        Me.btnBaselineAddFields.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBaselineAddFields.Location = New System.Drawing.Point(200, 220)
        Me.btnBaselineAddFields.Name = "btnBaselineAddFields"
        Me.btnBaselineAddFields.Size = New System.Drawing.Size(92, 23)
        Me.btnBaselineAddFields.TabIndex = 5
        Me.btnBaselineAddFields.Text = "Add Fields"
        Me.btnBaselineAddFields.UseVisualStyleBackColor = True
        '
        'ifAddIdFieldNameControl
        '
        Me.ifAddIdFieldNameControl.AutoSize = True
        Me.ifAddIdFieldNameControl.Location = New System.Drawing.Point(334, 104)
        Me.ifAddIdFieldNameControl.Name = "ifAddIdFieldNameControl"
        Me.ifAddIdFieldNameControl.Size = New System.Drawing.Size(15, 14)
        Me.ifAddIdFieldNameControl.TabIndex = 4
        Me.ifAddIdFieldNameControl.UseVisualStyleBackColor = True
        '
        'ifAddGroupFieldNameControl
        '
        Me.ifAddGroupFieldNameControl.AutoSize = True
        Me.ifAddGroupFieldNameControl.Location = New System.Drawing.Point(334, 145)
        Me.ifAddGroupFieldNameControl.Name = "ifAddGroupFieldNameControl"
        Me.ifAddGroupFieldNameControl.Size = New System.Drawing.Size(15, 14)
        Me.ifAddGroupFieldNameControl.TabIndex = 4
        Me.ifAddGroupFieldNameControl.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(18, 626)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(103, 16)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "*Optional Fields"
        '
        'AttributeToolbarForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(438, 649)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBoxBaseline)
        Me.Controls.Add(Me.GroupBoxShoreline)
        Me.Controls.Add(Me.textArea)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AttributeToolbarForm"
        Me.Text = "DSAS v5 Shoreline Attribute Automator"
        Me.GroupBoxShoreline.ResumeLayout(False)
        Me.GroupBoxShoreline.PerformLayout()
        Me.GroupBoxBaseline.ResumeLayout(False)
        Me.GroupBoxBaseline.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents layerListControl As System.Windows.Forms.ListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents UncyFieldNameControl As System.Windows.Forms.TextBox
    Friend WithEvents IfAddDateFieldControl As System.Windows.Forms.CheckBox
    Friend WithEvents IfAddUncyFieldControl As System.Windows.Forms.CheckBox
    Friend WithEvents AddFieldsButton As System.Windows.Forms.Button
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents dateFieldNameControl As System.Windows.Forms.TextBox
    Friend WithEvents IfAddShorelineTypeFieldControl As System.Windows.Forms.CheckBox
    Friend WithEvents shorelineTypeFieldContrl As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents textArea As System.Windows.Forms.TextBox
    Friend WithEvents GroupBoxShoreline As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBoxBaseline As System.Windows.Forms.GroupBox
    Friend WithEvents baselineListCtrl As System.Windows.Forms.ListBox
    Friend WithEvents ifAddSearchDistanceNameControl As System.Windows.Forms.CheckBox
    Friend WithEvents labelIdFieldName As System.Windows.Forms.Label
    Friend WithEvents searchDistanceFieldNameControl As System.Windows.Forms.TextBox
    Friend WithEvents labelGroupFieldName As System.Windows.Forms.Label
    Friend WithEvents labelSearchDistanceFieldName As System.Windows.Forms.Label
    Friend WithEvents idFieldNameControl As System.Windows.Forms.TextBox
    Friend WithEvents btnBaselineClose As System.Windows.Forms.Button
    Friend WithEvents groupFiledNameControl As System.Windows.Forms.TextBox
    Friend WithEvents btnBaselineAddFields As System.Windows.Forms.Button
    Friend WithEvents ifAddIdFieldNameControl As System.Windows.Forms.CheckBox
    Friend WithEvents ifAddGroupFieldNameControl As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
