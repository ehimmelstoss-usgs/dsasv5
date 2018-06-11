<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SetDefaultsForm
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SetDefaultsForm))
        Me.tabDefaults = New System.Windows.Forms.TabControl()
        Me.tabTransects = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.helpButtonVisualizeBaselineOrientation = New System.Windows.Forms.PictureBox()
        Me.chkVisualizeBaselineOrientation = New System.Windows.Forms.CheckBox()
        Me.helpButtonLandDirection = New System.Windows.Forms.PictureBox()
        Me.lblLandRelToBlOrientation = New System.Windows.Forms.Label()
        Me.rad_right = New System.Windows.Forms.RadioButton()
        Me.rad_left = New System.Windows.Forms.RadioButton()
        Me.pbRight1 = New System.Windows.Forms.PictureBox()
        Me.pbLeft1 = New System.Windows.Forms.PictureBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblBaselineSearchDistanceField = New System.Windows.Forms.Label()
        Me.cmbBaselineSearchDistance = New System.Windows.Forms.ComboBox()
        Me.baselineSearchDistanceHelpBtn = New System.Windows.Forms.PictureBox()
        Me.helpButtonBaselineGroup = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbBaseGroup = New System.Windows.Forms.ComboBox()
        Me.cmbBaselineFC = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tabShorelineCalc = New System.Windows.Forms.TabPage()
        Me.grpBias = New System.Windows.Forms.GroupBox()
        Me.helpButtonShorelineType = New System.Windows.Forms.PictureBox()
        Me.helpButtonLidarTable = New System.Windows.Forms.PictureBox()
        Me.cmbShorelineTypeField = New System.Windows.Forms.ComboBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.cmbLidar = New System.Windows.Forms.ComboBox()
        Me.lblUncertaintyTable = New System.Windows.Forms.Label()
        Me.grpIntersection = New System.Windows.Forms.GroupBox()
        Me.pbs3 = New System.Windows.Forms.PictureBox()
        Me.pbl3 = New System.Windows.Forms.PictureBox()
        Me.pbs2 = New System.Windows.Forms.PictureBox()
        Me.pbl2 = New System.Windows.Forms.PictureBox()
        Me.radLandward = New System.Windows.Forms.RadioButton()
        Me.pbClosest = New System.Windows.Forms.PictureBox()
        Me.radSeaward = New System.Windows.Forms.RadioButton()
        Me.pbFarthest = New System.Windows.Forms.PictureBox()
        Me.grpShoreline = New System.Windows.Forms.GroupBox()
        Me.btnHelpDefaultUncerty = New System.Windows.Forms.PictureBox()
        Me.txtDefaultUncertainty = New AMS.TextBox.NumericTextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbUncertaintyField = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbDateField = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbShorelineFC = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.current = New System.Windows.Forms.ComboBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.cntemail = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.cntvoice = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.postal = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.state = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.city = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.address = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.cntper = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.cntorg = New System.Windows.Forms.TextBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.progress = New System.Windows.Forms.ComboBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.status_update = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.accconst = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.purpose = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.abstract = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.origin = New System.Windows.Forms.TextBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.btnShowLogLocation = New System.Windows.Forms.Button()
        Me.radLogNone = New System.Windows.Forms.RadioButton()
        Me.radLogExtended = New System.Windows.Forms.RadioButton()
        Me.radLogRegular = New System.Windows.Forms.RadioButton()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabDefaults.SuspendLayout()
        Me.tabTransects.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.helpButtonVisualizeBaselineOrientation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.helpButtonLandDirection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbRight1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbLeft1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.baselineSearchDistanceHelpBtn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.helpButtonBaselineGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabShorelineCalc.SuspendLayout()
        Me.grpBias.SuspendLayout()
        CType(Me.helpButtonShorelineType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.helpButtonLidarTable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpIntersection.SuspendLayout()
        CType(Me.pbs3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbl3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbs2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbl2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbClosest, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbFarthest, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpShoreline.SuspendLayout()
        CType(Me.btnHelpDefaultUncerty, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabDefaults
        '
        Me.tabDefaults.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabDefaults.Controls.Add(Me.tabTransects)
        Me.tabDefaults.Controls.Add(Me.tabShorelineCalc)
        Me.tabDefaults.Controls.Add(Me.TabPage1)
        Me.tabDefaults.Location = New System.Drawing.Point(1, 1)
        Me.tabDefaults.Name = "tabDefaults"
        Me.tabDefaults.SelectedIndex = 0
        Me.tabDefaults.Size = New System.Drawing.Size(428, 533)
        Me.tabDefaults.TabIndex = 0
        '
        'tabTransects
        '
        Me.tabTransects.Controls.Add(Me.GroupBox2)
        Me.tabTransects.Controls.Add(Me.GroupBox1)
        Me.tabTransects.Location = New System.Drawing.Point(4, 22)
        Me.tabTransects.Name = "tabTransects"
        Me.tabTransects.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTransects.Size = New System.Drawing.Size(420, 507)
        Me.tabTransects.TabIndex = 0
        Me.tabTransects.Text = "Baseline Settings"
        Me.tabTransects.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.helpButtonVisualizeBaselineOrientation)
        Me.GroupBox2.Controls.Add(Me.chkVisualizeBaselineOrientation)
        Me.GroupBox2.Controls.Add(Me.helpButtonLandDirection)
        Me.GroupBox2.Controls.Add(Me.lblLandRelToBlOrientation)
        Me.GroupBox2.Controls.Add(Me.rad_right)
        Me.GroupBox2.Controls.Add(Me.rad_left)
        Me.GroupBox2.Controls.Add(Me.pbRight1)
        Me.GroupBox2.Controls.Add(Me.pbLeft1)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 131)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(407, 369)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        '
        'helpButtonVisualizeBaselineOrientation
        '
        Me.helpButtonVisualizeBaselineOrientation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.helpButtonVisualizeBaselineOrientation.Cursor = System.Windows.Forms.Cursors.Hand
        Me.helpButtonVisualizeBaselineOrientation.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.helpButtonVisualizeBaselineOrientation.Location = New System.Drawing.Point(165, 34)
        Me.helpButtonVisualizeBaselineOrientation.Name = "helpButtonVisualizeBaselineOrientation"
        Me.helpButtonVisualizeBaselineOrientation.Size = New System.Drawing.Size(25, 21)
        Me.helpButtonVisualizeBaselineOrientation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.helpButtonVisualizeBaselineOrientation.TabIndex = 9
        Me.helpButtonVisualizeBaselineOrientation.TabStop = False
        '
        'chkVisualizeBaselineOrientation
        '
        Me.chkVisualizeBaselineOrientation.AutoSize = True
        Me.chkVisualizeBaselineOrientation.Location = New System.Drawing.Point(9, 37)
        Me.chkVisualizeBaselineOrientation.Name = "chkVisualizeBaselineOrientation"
        Me.chkVisualizeBaselineOrientation.Size = New System.Drawing.Size(150, 17)
        Me.chkVisualizeBaselineOrientation.TabIndex = 10
        Me.chkVisualizeBaselineOrientation.Text = "Show Baseline Orientation"
        Me.chkVisualizeBaselineOrientation.UseVisualStyleBackColor = True
        '
        'helpButtonLandDirection
        '
        Me.helpButtonLandDirection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.helpButtonLandDirection.Cursor = System.Windows.Forms.Cursors.Hand
        Me.helpButtonLandDirection.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.helpButtonLandDirection.Location = New System.Drawing.Point(258, 12)
        Me.helpButtonLandDirection.Name = "helpButtonLandDirection"
        Me.helpButtonLandDirection.Size = New System.Drawing.Size(25, 21)
        Me.helpButtonLandDirection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.helpButtonLandDirection.TabIndex = 5
        Me.helpButtonLandDirection.TabStop = False
        '
        'lblLandRelToBlOrientation
        '
        Me.lblLandRelToBlOrientation.AutoSize = True
        Me.lblLandRelToBlOrientation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLandRelToBlOrientation.Location = New System.Drawing.Point(6, 15)
        Me.lblLandRelToBlOrientation.Name = "lblLandRelToBlOrientation"
        Me.lblLandRelToBlOrientation.Size = New System.Drawing.Size(238, 13)
        Me.lblLandRelToBlOrientation.TabIndex = 4
        Me.lblLandRelToBlOrientation.Text = "Location of Land Relative to Baseline Orientation"
        '
        'rad_right
        '
        Me.rad_right.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rad_right.AutoSize = True
        Me.rad_right.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rad_right.Location = New System.Drawing.Point(209, 62)
        Me.rad_right.Name = "rad_right"
        Me.rad_right.Size = New System.Drawing.Size(50, 17)
        Me.rad_right.TabIndex = 7
        Me.rad_right.TabStop = True
        Me.rad_right.Text = "Right"
        Me.rad_right.UseVisualStyleBackColor = True
        '
        'rad_left
        '
        Me.rad_left.AutoSize = True
        Me.rad_left.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rad_left.Location = New System.Drawing.Point(10, 62)
        Me.rad_left.Name = "rad_left"
        Me.rad_left.Size = New System.Drawing.Size(43, 17)
        Me.rad_left.TabIndex = 6
        Me.rad_left.TabStop = True
        Me.rad_left.Text = "Left"
        Me.rad_left.UseVisualStyleBackColor = True
        '
        'pbRight1
        '
        Me.pbRight1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbRight1.Image = CType(resources.GetObject("pbRight1.Image"), System.Drawing.Image)
        Me.pbRight1.Location = New System.Drawing.Point(209, 84)
        Me.pbRight1.Name = "pbRight1"
        Me.pbRight1.Size = New System.Drawing.Size(189, 282)
        Me.pbRight1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbRight1.TabIndex = 3
        Me.pbRight1.TabStop = False
        '
        'pbLeft1
        '
        Me.pbLeft1.Image = CType(resources.GetObject("pbLeft1.Image"), System.Drawing.Image)
        Me.pbLeft1.Location = New System.Drawing.Point(10, 84)
        Me.pbLeft1.Margin = New System.Windows.Forms.Padding(0)
        Me.pbLeft1.Name = "pbLeft1"
        Me.pbLeft1.Size = New System.Drawing.Size(189, 282)
        Me.pbLeft1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbLeft1.TabIndex = 1
        Me.pbLeft1.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblBaselineSearchDistanceField)
        Me.GroupBox1.Controls.Add(Me.cmbBaselineSearchDistance)
        Me.GroupBox1.Controls.Add(Me.baselineSearchDistanceHelpBtn)
        Me.GroupBox1.Controls.Add(Me.helpButtonBaselineGroup)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.cmbBaseGroup)
        Me.GroupBox1.Controls.Add(Me.cmbBaselineFC)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(406, 119)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Baseline Parameters"
        '
        'lblBaselineSearchDistanceField
        '
        Me.lblBaselineSearchDistanceField.AutoSize = True
        Me.lblBaselineSearchDistanceField.Location = New System.Drawing.Point(6, 85)
        Me.lblBaselineSearchDistanceField.Name = "lblBaselineSearchDistanceField"
        Me.lblBaselineSearchDistanceField.Size = New System.Drawing.Size(154, 13)
        Me.lblBaselineSearchDistanceField.TabIndex = 14
        Me.lblBaselineSearchDistanceField.Text = "Baseline Search Distance Field"
        '
        'cmbBaselineSearchDistance
        '
        Me.cmbBaselineSearchDistance.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbBaselineSearchDistance.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbBaselineSearchDistance.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbBaselineSearchDistance.FormattingEnabled = True
        Me.cmbBaselineSearchDistance.Location = New System.Drawing.Point(171, 82)
        Me.cmbBaselineSearchDistance.Name = "cmbBaselineSearchDistance"
        Me.cmbBaselineSearchDistance.Size = New System.Drawing.Size(193, 21)
        Me.cmbBaselineSearchDistance.TabIndex = 13
        '
        'baselineSearchDistanceHelpBtn
        '
        Me.baselineSearchDistanceHelpBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.baselineSearchDistanceHelpBtn.Cursor = System.Windows.Forms.Cursors.Hand
        Me.baselineSearchDistanceHelpBtn.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.baselineSearchDistanceHelpBtn.Location = New System.Drawing.Point(373, 82)
        Me.baselineSearchDistanceHelpBtn.Name = "baselineSearchDistanceHelpBtn"
        Me.baselineSearchDistanceHelpBtn.Size = New System.Drawing.Size(25, 21)
        Me.baselineSearchDistanceHelpBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.baselineSearchDistanceHelpBtn.TabIndex = 12
        Me.baselineSearchDistanceHelpBtn.TabStop = False
        '
        'helpButtonBaselineGroup
        '
        Me.helpButtonBaselineGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.helpButtonBaselineGroup.Cursor = System.Windows.Forms.Cursors.Hand
        Me.helpButtonBaselineGroup.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.helpButtonBaselineGroup.Location = New System.Drawing.Point(373, 50)
        Me.helpButtonBaselineGroup.Name = "helpButtonBaselineGroup"
        Me.helpButtonBaselineGroup.Size = New System.Drawing.Size(25, 21)
        Me.helpButtonBaselineGroup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.helpButtonBaselineGroup.TabIndex = 8
        Me.helpButtonBaselineGroup.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 53)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(104, 13)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "Baseline Group Field"
        '
        'cmbBaseGroup
        '
        Me.cmbBaseGroup.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbBaseGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbBaseGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbBaseGroup.FormattingEnabled = True
        Me.cmbBaseGroup.Location = New System.Drawing.Point(121, 50)
        Me.cmbBaseGroup.Name = "cmbBaseGroup"
        Me.cmbBaseGroup.Size = New System.Drawing.Size(243, 21)
        Me.cmbBaseGroup.TabIndex = 8
        '
        'cmbBaselineFC
        '
        Me.cmbBaselineFC.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbBaselineFC.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbBaselineFC.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbBaselineFC.FormattingEnabled = True
        Me.cmbBaselineFC.Location = New System.Drawing.Point(121, 19)
        Me.cmbBaselineFC.Name = "cmbBaselineFC"
        Me.cmbBaselineFC.Size = New System.Drawing.Size(274, 21)
        Me.cmbBaselineFC.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Baseline Layer"
        '
        'tabShorelineCalc
        '
        Me.tabShorelineCalc.Controls.Add(Me.grpBias)
        Me.tabShorelineCalc.Controls.Add(Me.grpIntersection)
        Me.tabShorelineCalc.Controls.Add(Me.grpShoreline)
        Me.tabShorelineCalc.Location = New System.Drawing.Point(4, 22)
        Me.tabShorelineCalc.Name = "tabShorelineCalc"
        Me.tabShorelineCalc.Padding = New System.Windows.Forms.Padding(3)
        Me.tabShorelineCalc.Size = New System.Drawing.Size(420, 507)
        Me.tabShorelineCalc.TabIndex = 1
        Me.tabShorelineCalc.Text = "Shoreline Calculation Settings"
        Me.tabShorelineCalc.UseVisualStyleBackColor = True
        '
        'grpBias
        '
        Me.grpBias.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBias.Controls.Add(Me.helpButtonShorelineType)
        Me.grpBias.Controls.Add(Me.helpButtonLidarTable)
        Me.grpBias.Controls.Add(Me.cmbShorelineTypeField)
        Me.grpBias.Controls.Add(Me.Label27)
        Me.grpBias.Controls.Add(Me.cmbLidar)
        Me.grpBias.Controls.Add(Me.lblUncertaintyTable)
        Me.grpBias.Location = New System.Drawing.Point(2, 425)
        Me.grpBias.Name = "grpBias"
        Me.grpBias.Size = New System.Drawing.Size(412, 84)
        Me.grpBias.TabIndex = 11
        Me.grpBias.TabStop = False
        Me.grpBias.Text = "Bias Parameters"
        Me.grpBias.Visible = False
        '
        'helpButtonShorelineType
        '
        Me.helpButtonShorelineType.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.helpButtonShorelineType.Cursor = System.Windows.Forms.Cursors.Hand
        Me.helpButtonShorelineType.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.helpButtonShorelineType.Location = New System.Drawing.Point(382, 24)
        Me.helpButtonShorelineType.Name = "helpButtonShorelineType"
        Me.helpButtonShorelineType.Size = New System.Drawing.Size(25, 21)
        Me.helpButtonShorelineType.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.helpButtonShorelineType.TabIndex = 14
        Me.helpButtonShorelineType.TabStop = False
        '
        'helpButtonLidarTable
        '
        Me.helpButtonLidarTable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.helpButtonLidarTable.Cursor = System.Windows.Forms.Cursors.Hand
        Me.helpButtonLidarTable.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.helpButtonLidarTable.Location = New System.Drawing.Point(382, 51)
        Me.helpButtonLidarTable.Name = "helpButtonLidarTable"
        Me.helpButtonLidarTable.Size = New System.Drawing.Size(25, 21)
        Me.helpButtonLidarTable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.helpButtonLidarTable.TabIndex = 14
        Me.helpButtonLidarTable.TabStop = False
        '
        'cmbShorelineTypeField
        '
        Me.cmbShorelineTypeField.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbShorelineTypeField.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbShorelineTypeField.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbShorelineTypeField.FormattingEnabled = True
        Me.cmbShorelineTypeField.Location = New System.Drawing.Point(148, 24)
        Me.cmbShorelineTypeField.Name = "cmbShorelineTypeField"
        Me.cmbShorelineTypeField.Size = New System.Drawing.Size(216, 21)
        Me.cmbShorelineTypeField.TabIndex = 12
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(6, 27)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(103, 13)
        Me.Label27.TabIndex = 13
        Me.Label27.Text = "Shoreline Type Field"
        '
        'cmbLidar
        '
        Me.cmbLidar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbLidar.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbLidar.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbLidar.FormattingEnabled = True
        Me.cmbLidar.Location = New System.Drawing.Point(148, 51)
        Me.cmbLidar.Name = "cmbLidar"
        Me.cmbLidar.Size = New System.Drawing.Size(216, 21)
        Me.cmbLidar.TabIndex = 12
        '
        'lblUncertaintyTable
        '
        Me.lblUncertaintyTable.AutoSize = True
        Me.lblUncertaintyTable.Location = New System.Drawing.Point(6, 54)
        Me.lblUncertaintyTable.Name = "lblUncertaintyTable"
        Me.lblUncertaintyTable.Size = New System.Drawing.Size(91, 13)
        Me.lblUncertaintyTable.TabIndex = 13
        Me.lblUncertaintyTable.Text = "Uncertainty Table"
        '
        'grpIntersection
        '
        Me.grpIntersection.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpIntersection.Controls.Add(Me.pbs3)
        Me.grpIntersection.Controls.Add(Me.pbl3)
        Me.grpIntersection.Controls.Add(Me.pbs2)
        Me.grpIntersection.Controls.Add(Me.pbl2)
        Me.grpIntersection.Controls.Add(Me.radLandward)
        Me.grpIntersection.Controls.Add(Me.pbClosest)
        Me.grpIntersection.Controls.Add(Me.radSeaward)
        Me.grpIntersection.Controls.Add(Me.pbFarthest)
        Me.grpIntersection.Location = New System.Drawing.Point(6, 151)
        Me.grpIntersection.Name = "grpIntersection"
        Me.grpIntersection.Size = New System.Drawing.Size(412, 268)
        Me.grpIntersection.TabIndex = 10
        Me.grpIntersection.TabStop = False
        Me.grpIntersection.Text = "Intersection Parameters"
        '
        'pbs3
        '
        Me.pbs3.Image = Global.DSASAddin.My.Resources.Resources.seaward3
        Me.pbs3.Location = New System.Drawing.Point(283, 46)
        Me.pbs3.Name = "pbs3"
        Me.pbs3.Size = New System.Drawing.Size(122, 90)
        Me.pbs3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbs3.TabIndex = 7
        Me.pbs3.TabStop = False
        '
        'pbl3
        '
        Me.pbl3.Image = Global.DSASAddin.My.Resources.Resources.landward3
        Me.pbl3.Location = New System.Drawing.Point(283, 172)
        Me.pbl3.Name = "pbl3"
        Me.pbl3.Size = New System.Drawing.Size(122, 90)
        Me.pbl3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbl3.TabIndex = 5
        Me.pbl3.TabStop = False
        '
        'pbs2
        '
        Me.pbs2.Image = Global.DSASAddin.My.Resources.Resources.seaward2
        Me.pbs2.Location = New System.Drawing.Point(146, 46)
        Me.pbs2.Name = "pbs2"
        Me.pbs2.Size = New System.Drawing.Size(122, 90)
        Me.pbs2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbs2.TabIndex = 6
        Me.pbs2.TabStop = False
        '
        'pbl2
        '
        Me.pbl2.Image = Global.DSASAddin.My.Resources.Resources.landward2
        Me.pbl2.Location = New System.Drawing.Point(146, 172)
        Me.pbl2.Name = "pbl2"
        Me.pbl2.Size = New System.Drawing.Size(122, 90)
        Me.pbl2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbl2.TabIndex = 4
        Me.pbl2.TabStop = False
        '
        'radLandward
        '
        Me.radLandward.AutoSize = True
        Me.radLandward.Location = New System.Drawing.Point(9, 149)
        Me.radLandward.Name = "radLandward"
        Me.radLandward.Size = New System.Drawing.Size(130, 17)
        Me.radLandward.TabIndex = 2
        Me.radLandward.TabStop = True
        Me.radLandward.Text = "Landward Intersection"
        Me.radLandward.UseVisualStyleBackColor = True
        '
        'pbClosest
        '
        Me.pbClosest.Image = Global.DSASAddin.My.Resources.Resources.landward1
        Me.pbClosest.Location = New System.Drawing.Point(9, 172)
        Me.pbClosest.Name = "pbClosest"
        Me.pbClosest.Size = New System.Drawing.Size(122, 90)
        Me.pbClosest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbClosest.TabIndex = 0
        Me.pbClosest.TabStop = False
        '
        'radSeaward
        '
        Me.radSeaward.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.radSeaward.AutoSize = True
        Me.radSeaward.Location = New System.Drawing.Point(9, 23)
        Me.radSeaward.Name = "radSeaward"
        Me.radSeaward.Size = New System.Drawing.Size(125, 17)
        Me.radSeaward.TabIndex = 3
        Me.radSeaward.TabStop = True
        Me.radSeaward.Text = "Seaward Intersection"
        Me.radSeaward.UseVisualStyleBackColor = True
        '
        'pbFarthest
        '
        Me.pbFarthest.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbFarthest.Image = Global.DSASAddin.My.Resources.Resources.seaward1
        Me.pbFarthest.Location = New System.Drawing.Point(9, 46)
        Me.pbFarthest.Name = "pbFarthest"
        Me.pbFarthest.Size = New System.Drawing.Size(122, 90)
        Me.pbFarthest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbFarthest.TabIndex = 1
        Me.pbFarthest.TabStop = False
        '
        'grpShoreline
        '
        Me.grpShoreline.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpShoreline.Controls.Add(Me.btnHelpDefaultUncerty)
        Me.grpShoreline.Controls.Add(Me.txtDefaultUncertainty)
        Me.grpShoreline.Controls.Add(Me.Label11)
        Me.grpShoreline.Controls.Add(Me.Label10)
        Me.grpShoreline.Controls.Add(Me.cmbUncertaintyField)
        Me.grpShoreline.Controls.Add(Me.Label9)
        Me.grpShoreline.Controls.Add(Me.cmbDateField)
        Me.grpShoreline.Controls.Add(Me.Label8)
        Me.grpShoreline.Controls.Add(Me.cmbShorelineFC)
        Me.grpShoreline.Controls.Add(Me.Label7)
        Me.grpShoreline.Location = New System.Drawing.Point(6, 6)
        Me.grpShoreline.Name = "grpShoreline"
        Me.grpShoreline.Size = New System.Drawing.Size(412, 139)
        Me.grpShoreline.TabIndex = 0
        Me.grpShoreline.TabStop = False
        Me.grpShoreline.Text = "Shoreline Parameters"
        '
        'btnHelpDefaultUncerty
        '
        Me.btnHelpDefaultUncerty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnHelpDefaultUncerty.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnHelpDefaultUncerty.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.btnHelpDefaultUncerty.Location = New System.Drawing.Point(374, 102)
        Me.btnHelpDefaultUncerty.Name = "btnHelpDefaultUncerty"
        Me.btnHelpDefaultUncerty.Size = New System.Drawing.Size(25, 21)
        Me.btnHelpDefaultUncerty.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.btnHelpDefaultUncerty.TabIndex = 15
        Me.btnHelpDefaultUncerty.TabStop = False
        '
        'txtDefaultUncertainty
        '
        Me.txtDefaultUncertainty.AllowNegative = True
        Me.txtDefaultUncertainty.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDefaultUncertainty.DigitsInGroup = 0
        Me.txtDefaultUncertainty.Flags = 0
        Me.txtDefaultUncertainty.Location = New System.Drawing.Point(136, 103)
        Me.txtDefaultUncertainty.MaxDecimalPlaces = 3
        Me.txtDefaultUncertainty.MaxWholeDigits = 9
        Me.txtDefaultUncertainty.Name = "txtDefaultUncertainty"
        Me.txtDefaultUncertainty.Prefix = ""
        Me.txtDefaultUncertainty.RangeMax = 1.7976931348623157E+308R
        Me.txtDefaultUncertainty.RangeMin = -1.7976931348623157E+308R
        Me.txtDefaultUncertainty.Size = New System.Drawing.Size(148, 20)
        Me.txtDefaultUncertainty.TabIndex = 11
        '
        'Label11
        '
        Me.Label11.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(290, 106)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(55, 13)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "+/- meters"
        '
        'Label10
        '
        Me.Label10.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 106)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(124, 13)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Default Data Uncertainty"
        '
        'cmbUncertaintyField
        '
        Me.cmbUncertaintyField.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbUncertaintyField.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbUncertaintyField.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbUncertaintyField.FormattingEnabled = True
        Me.cmbUncertaintyField.Location = New System.Drawing.Point(178, 73)
        Me.cmbUncertaintyField.Name = "cmbUncertaintyField"
        Me.cmbUncertaintyField.Size = New System.Drawing.Size(218, 21)
        Me.cmbUncertaintyField.TabIndex = 6
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 76)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(133, 13)
        Me.Label9.TabIndex = 7
        Me.Label9.Text = "Shoreline Uncertainty Field"
        '
        'cmbDateField
        '
        Me.cmbDateField.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbDateField.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbDateField.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbDateField.FormattingEnabled = True
        Me.cmbDateField.Location = New System.Drawing.Point(147, 46)
        Me.cmbDateField.Name = "cmbDateField"
        Me.cmbDateField.Size = New System.Drawing.Size(249, 21)
        Me.cmbDateField.TabIndex = 4
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 49)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(102, 13)
        Me.Label8.TabIndex = 5
        Me.Label8.Text = "Shoreline Date Field"
        '
        'cmbShorelineFC
        '
        Me.cmbShorelineFC.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbShorelineFC.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbShorelineFC.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbShorelineFC.FormattingEnabled = True
        Me.cmbShorelineFC.Location = New System.Drawing.Point(121, 19)
        Me.cmbShorelineFC.Name = "cmbShorelineFC"
        Me.cmbShorelineFC.Size = New System.Drawing.Size(275, 21)
        Me.cmbShorelineFC.TabIndex = 2
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 22)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(80, 13)
        Me.Label7.TabIndex = 3
        Me.Label7.Text = "Shoreline Layer"
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.current)
        Me.TabPage1.Controls.Add(Me.Label16)
        Me.TabPage1.Controls.Add(Me.GroupBox7)
        Me.TabPage1.Controls.Add(Me.GroupBox4)
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(420, 507)
        Me.TabPage1.TabIndex = 2
        Me.TabPage1.Text = "Metadata Settings"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'current
        '
        Me.current.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.current.FormattingEnabled = True
        Me.current.Items.AddRange(New Object() {"ground condition", "publication date"})
        Me.current.Location = New System.Drawing.Point(149, 364)
        Me.current.Name = "current"
        Me.current.Size = New System.Drawing.Size(168, 21)
        Me.current.TabIndex = 8
        Me.current.Text = "publication date"
        Me.current.Visible = False
        '
        'Label16
        '
        Me.Label16.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(13, 367)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(132, 13)
        Me.Label16.TabIndex = 7
        Me.Label16.Text = "What are Data Current to?"
        Me.Label16.Visible = False
        '
        'GroupBox7
        '
        Me.GroupBox7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox7.Controls.Add(Me.Label25)
        Me.GroupBox7.Controls.Add(Me.cntemail)
        Me.GroupBox7.Controls.Add(Me.Label24)
        Me.GroupBox7.Controls.Add(Me.cntvoice)
        Me.GroupBox7.Controls.Add(Me.Label23)
        Me.GroupBox7.Controls.Add(Me.postal)
        Me.GroupBox7.Controls.Add(Me.Label22)
        Me.GroupBox7.Controls.Add(Me.state)
        Me.GroupBox7.Controls.Add(Me.Label21)
        Me.GroupBox7.Controls.Add(Me.city)
        Me.GroupBox7.Controls.Add(Me.Label18)
        Me.GroupBox7.Controls.Add(Me.address)
        Me.GroupBox7.Controls.Add(Me.Label19)
        Me.GroupBox7.Controls.Add(Me.cntper)
        Me.GroupBox7.Controls.Add(Me.Label20)
        Me.GroupBox7.Controls.Add(Me.cntorg)
        Me.GroupBox7.Location = New System.Drawing.Point(7, 199)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(402, 153)
        Me.GroupBox7.TabIndex = 6
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Contact Information"
        '
        'Label25
        '
        Me.Label25.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(156, 130)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(32, 13)
        Me.Label25.TabIndex = 15
        Me.Label25.Text = "Email"
        '
        'cntemail
        '
        Me.cntemail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cntemail.Location = New System.Drawing.Point(194, 127)
        Me.cntemail.Name = "cntemail"
        Me.cntemail.Size = New System.Drawing.Size(202, 20)
        Me.cntemail.TabIndex = 14
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(6, 130)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(38, 13)
        Me.Label24.TabIndex = 13
        Me.Label24.Text = "Phone"
        '
        'cntvoice
        '
        Me.cntvoice.Location = New System.Drawing.Point(50, 127)
        Me.cntvoice.Name = "cntvoice"
        Me.cntvoice.Size = New System.Drawing.Size(86, 20)
        Me.cntvoice.TabIndex = 12
        '
        'Label23
        '
        Me.Label23.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(322, 100)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(22, 13)
        Me.Label23.TabIndex = 11
        Me.Label23.Text = "Zip"
        '
        'postal
        '
        Me.postal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.postal.Location = New System.Drawing.Point(350, 97)
        Me.postal.Name = "postal"
        Me.postal.Size = New System.Drawing.Size(46, 20)
        Me.postal.TabIndex = 10
        '
        'Label22
        '
        Me.Label22.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(237, 100)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(32, 13)
        Me.Label22.TabIndex = 9
        Me.Label22.Text = "State"
        '
        'state
        '
        Me.state.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.state.Location = New System.Drawing.Point(275, 97)
        Me.state.Name = "state"
        Me.state.Size = New System.Drawing.Size(35, 20)
        Me.state.TabIndex = 8
        '
        'Label21
        '
        Me.Label21.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(6, 100)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(24, 13)
        Me.Label21.TabIndex = 7
        Me.Label21.Text = "City"
        '
        'city
        '
        Me.city.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.city.Location = New System.Drawing.Point(36, 97)
        Me.city.Name = "city"
        Me.city.Size = New System.Drawing.Size(188, 20)
        Me.city.TabIndex = 6
        '
        'Label18
        '
        Me.Label18.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(6, 74)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(45, 13)
        Me.Label18.TabIndex = 5
        Me.Label18.Text = "Address"
        '
        'address
        '
        Me.address.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.address.Location = New System.Drawing.Point(78, 71)
        Me.address.Name = "address"
        Me.address.Size = New System.Drawing.Size(318, 20)
        Me.address.TabIndex = 4
        '
        'Label19
        '
        Me.Label19.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(6, 48)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(40, 13)
        Me.Label19.TabIndex = 3
        Me.Label19.Text = "Person"
        '
        'cntper
        '
        Me.cntper.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cntper.Location = New System.Drawing.Point(78, 45)
        Me.cntper.Name = "cntper"
        Me.cntper.Size = New System.Drawing.Size(318, 20)
        Me.cntper.TabIndex = 2
        '
        'Label20
        '
        Me.Label20.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(6, 22)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(66, 13)
        Me.Label20.TabIndex = 1
        Me.Label20.Text = "Organization"
        '
        'cntorg
        '
        Me.cntorg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cntorg.Location = New System.Drawing.Point(78, 19)
        Me.cntorg.Name = "cntorg"
        Me.cntorg.Size = New System.Drawing.Size(318, 20)
        Me.cntorg.TabIndex = 0
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.Controls.Add(Me.progress)
        Me.GroupBox4.Controls.Add(Me.Label17)
        Me.GroupBox4.Controls.Add(Me.status_update)
        Me.GroupBox4.Controls.Add(Me.Label14)
        Me.GroupBox4.Controls.Add(Me.accconst)
        Me.GroupBox4.Controls.Add(Me.Label15)
        Me.GroupBox4.Location = New System.Drawing.Point(7, 119)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(402, 74)
        Me.GroupBox4.TabIndex = 6
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Data Update and Access Information"
        '
        'progress
        '
        Me.progress.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.progress.FormattingEnabled = True
        Me.progress.Items.AddRange(New Object() {"Complete", "In work", "Planned"})
        Me.progress.Location = New System.Drawing.Point(303, 20)
        Me.progress.Name = "progress"
        Me.progress.Size = New System.Drawing.Size(93, 21)
        Me.progress.TabIndex = 9
        '
        'Label17
        '
        Me.Label17.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(249, 23)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(48, 13)
        Me.Label17.TabIndex = 8
        Me.Label17.Text = "Progress"
        '
        'status_update
        '
        Me.status_update.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.status_update.FormattingEnabled = True
        Me.status_update.Items.AddRange(New Object() {"Continually", "Daily", "Weekly", "Monthly", "Annually", "Unknown", "As needed", "Irregular", "None planned"})
        Me.status_update.Location = New System.Drawing.Point(107, 20)
        Me.status_update.Name = "status_update"
        Me.status_update.Size = New System.Drawing.Size(117, 21)
        Me.status_update.TabIndex = 7
        '
        'Label14
        '
        Me.Label14.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(6, 49)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(112, 13)
        Me.Label14.TabIndex = 5
        Me.Label14.Text = "Constraints on Access"
        '
        'accconst
        '
        Me.accconst.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.accconst.Location = New System.Drawing.Point(124, 46)
        Me.accconst.Name = "accconst"
        Me.accconst.Size = New System.Drawing.Size(272, 20)
        Me.accconst.TabIndex = 4
        '
        'Label15
        '
        Me.Label15.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(6, 23)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(95, 13)
        Me.Label15.TabIndex = 3
        Me.Label15.Text = "Update Frequency"
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.Label13)
        Me.GroupBox3.Controls.Add(Me.purpose)
        Me.GroupBox3.Controls.Add(Me.Label12)
        Me.GroupBox3.Controls.Add(Me.abstract)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.origin)
        Me.GroupBox3.Location = New System.Drawing.Point(7, 7)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(402, 97)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "General Information"
        '
        'Label13
        '
        Me.Label13.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 73)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(46, 13)
        Me.Label13.TabIndex = 5
        Me.Label13.Text = "Purpose"
        '
        'purpose
        '
        Me.purpose.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.purpose.Location = New System.Drawing.Point(64, 70)
        Me.purpose.Multiline = True
        Me.purpose.Name = "purpose"
        Me.purpose.Size = New System.Drawing.Size(332, 20)
        Me.purpose.TabIndex = 4
        '
        'Label12
        '
        Me.Label12.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(6, 47)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(46, 13)
        Me.Label12.TabIndex = 3
        Me.Label12.Text = "Abstract"
        '
        'abstract
        '
        Me.abstract.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.abstract.Location = New System.Drawing.Point(64, 44)
        Me.abstract.Multiline = True
        Me.abstract.Name = "abstract"
        Me.abstract.Size = New System.Drawing.Size(332, 20)
        Me.abstract.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 21)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 13)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Originator"
        '
        'origin
        '
        Me.origin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.origin.Location = New System.Drawing.Point(64, 18)
        Me.origin.Multiline = True
        Me.origin.Name = "origin"
        Me.origin.Size = New System.Drawing.Size(332, 20)
        Me.origin.TabIndex = 0
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(333, 587)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(92, 23)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(235, 587)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(92, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox6.Controls.Add(Me.btnShowLogLocation)
        Me.GroupBox6.Controls.Add(Me.radLogNone)
        Me.GroupBox6.Controls.Add(Me.radLogExtended)
        Me.GroupBox6.Controls.Add(Me.radLogRegular)
        Me.GroupBox6.Location = New System.Drawing.Point(11, 536)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(412, 39)
        Me.GroupBox6.TabIndex = 7
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Log File Output"
        '
        'btnShowLogLocation
        '
        Me.btnShowLogLocation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnShowLogLocation.Location = New System.Drawing.Point(293, 11)
        Me.btnShowLogLocation.Name = "btnShowLogLocation"
        Me.btnShowLogLocation.Size = New System.Drawing.Size(116, 23)
        Me.btnShowLogLocation.TabIndex = 3
        Me.btnShowLogLocation.Text = "Show Log Location"
        Me.btnShowLogLocation.UseVisualStyleBackColor = True
        '
        'radLogNone
        '
        Me.radLogNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.radLogNone.AutoSize = True
        Me.radLogNone.Location = New System.Drawing.Point(208, 16)
        Me.radLogNone.Name = "radLogNone"
        Me.radLogNone.Size = New System.Drawing.Size(51, 17)
        Me.radLogNone.TabIndex = 2
        Me.radLogNone.Text = "None"
        Me.radLogNone.UseVisualStyleBackColor = True
        '
        'radLogExtended
        '
        Me.radLogExtended.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.radLogExtended.AutoSize = True
        Me.radLogExtended.Location = New System.Drawing.Point(105, 16)
        Me.radLogExtended.Name = "radLogExtended"
        Me.radLogExtended.Size = New System.Drawing.Size(70, 17)
        Me.radLogExtended.TabIndex = 1
        Me.radLogExtended.Text = "Extended"
        Me.radLogExtended.UseVisualStyleBackColor = True
        '
        'radLogRegular
        '
        Me.radLogRegular.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.radLogRegular.AutoSize = True
        Me.radLogRegular.Checked = True
        Me.radLogRegular.Location = New System.Drawing.Point(10, 16)
        Me.radLogRegular.Name = "radLogRegular"
        Me.radLogRegular.Size = New System.Drawing.Size(62, 17)
        Me.radLogRegular.TabIndex = 0
        Me.radLogRegular.TabStop = True
        Me.radLogRegular.Text = "Regular"
        Me.radLogRegular.UseVisualStyleBackColor = True
        '
        'SetDefaultsForm
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(430, 616)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.tabDefaults)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(9999, 9999)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(428, 550)
        Me.Name = "SetDefaultsForm"
        Me.Text = "Set Default Parameters"
        Me.tabDefaults.ResumeLayout(False)
        Me.tabTransects.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.helpButtonVisualizeBaselineOrientation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.helpButtonLandDirection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbRight1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbLeft1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.baselineSearchDistanceHelpBtn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.helpButtonBaselineGroup, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabShorelineCalc.ResumeLayout(False)
        Me.grpBias.ResumeLayout(False)
        Me.grpBias.PerformLayout()
        CType(Me.helpButtonShorelineType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.helpButtonLidarTable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpIntersection.ResumeLayout(False)
        Me.grpIntersection.PerformLayout()
        CType(Me.pbs3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbl3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbs2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbl2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbClosest, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbFarthest, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpShoreline.ResumeLayout(False)
        Me.grpShoreline.PerformLayout()
        CType(Me.btnHelpDefaultUncerty, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabDefaults As System.Windows.Forms.TabControl
    Friend WithEvents tabTransects As System.Windows.Forms.TabPage
    Friend WithEvents tabShorelineCalc As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbBaselineFC As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbBaseGroup As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents grpShoreline As System.Windows.Forms.GroupBox
    Friend WithEvents cmbUncertaintyField As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cmbDateField As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbShorelineFC As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents grpIntersection As System.Windows.Forms.GroupBox
    Friend WithEvents radSeaward As System.Windows.Forms.RadioButton
    Friend WithEvents radLandward As System.Windows.Forms.RadioButton
    Friend WithEvents pbFarthest As System.Windows.Forms.PictureBox
    Friend WithEvents pbClosest As System.Windows.Forms.PictureBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents radLogNone As System.Windows.Forms.RadioButton
    Friend WithEvents radLogExtended As System.Windows.Forms.RadioButton
    Friend WithEvents radLogRegular As System.Windows.Forms.RadioButton
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtDefaultUncertainty As AMS.TextBox.NumericTextBox
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents origin As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents purpose As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents abstract As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents accconst As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents progress As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents status_update As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents city As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents address As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents cntper As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents cntorg As System.Windows.Forms.TextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents cntemail As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents cntvoice As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents postal As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents state As System.Windows.Forms.TextBox
    Friend WithEvents current As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btnShowLogLocation As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents pbLeft1 As System.Windows.Forms.PictureBox
    Friend WithEvents pbRight1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblLandRelToBlOrientation As System.Windows.Forms.Label
    Friend WithEvents helpButtonLandDirection As System.Windows.Forms.PictureBox
    Friend WithEvents rad_right As System.Windows.Forms.RadioButton
    Friend WithEvents rad_left As System.Windows.Forms.RadioButton
    Friend WithEvents helpButtonBaselineGroup As System.Windows.Forms.PictureBox
    Friend WithEvents helpButtonVisualizeBaselineOrientation As System.Windows.Forms.PictureBox
    Friend WithEvents helpButtonShorelineType As System.Windows.Forms.PictureBox
    Friend WithEvents cmbShorelineTypeField As System.Windows.Forms.ComboBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents chkVisualizeBaselineOrientation As System.Windows.Forms.CheckBox
    Friend WithEvents grpBias As System.Windows.Forms.GroupBox
    Friend WithEvents helpButtonLidarTable As System.Windows.Forms.PictureBox
    Friend WithEvents cmbLidar As System.Windows.Forms.ComboBox
    Friend WithEvents lblUncertaintyTable As System.Windows.Forms.Label
    Friend WithEvents baselineSearchDistanceHelpBtn As System.Windows.Forms.PictureBox
    Friend WithEvents cmbBaselineSearchDistance As System.Windows.Forms.ComboBox
    Friend WithEvents lblBaselineSearchDistanceField As System.Windows.Forms.Label
    Friend WithEvents pbs3 As System.Windows.Forms.PictureBox
    Friend WithEvents pbs2 As System.Windows.Forms.PictureBox
    Friend WithEvents pbl3 As System.Windows.Forms.PictureBox
    Friend WithEvents pbl2 As System.Windows.Forms.PictureBox
    Friend WithEvents btnHelpDefaultUncerty As System.Windows.Forms.PictureBox
End Class
