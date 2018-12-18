<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CastTransectsForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CastTransectsForm))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbExistingLayer = New System.Windows.Forms.ComboBox()
        Me.txtPersonalDb = New System.Windows.Forms.TextBox()
        Me.btnOpenGeodatabase = New System.Windows.Forms.Button()
        Me.gbCasting = New System.Windows.Forms.GroupBox()
        Me.chkClipTransectsToShorelineExtent = New System.Windows.Forms.CheckBox()
        Me.helpButtonClipTransectsToShorelineExtent = New System.Windows.Forms.PictureBox()
        Me.pbSmoothDistance0 = New System.Windows.Forms.PictureBox()
        Me.pbSmoothDistance500 = New System.Windows.Forms.PictureBox()
        Me.pbSmoothDistance2500 = New System.Windows.Forms.PictureBox()
        Me.pbSmoothDistanceRight = New System.Windows.Forms.PictureBox()
        Me.pbSmoothDistanceLeft = New System.Windows.Forms.PictureBox()
        Me.pbSmoothDistance1000 = New System.Windows.Forms.PictureBox()
        Me.pbSmoothDistance100 = New System.Windows.Forms.PictureBox()
        Me.btnHelpSmoothDistance = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.helpButtonSearchDistance = New System.Windows.Forms.PictureBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.pbTransectSpacing = New System.Windows.Forms.PictureBox()
        Me.txtSearchDistance = New AMS.TextBox.NumericTextBox()
        Me.txtTransectSpacing = New AMS.TextBox.NumericTextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.pbSearchDistance = New System.Windows.Forms.PictureBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtSmoothDist = New AMS.TextBox.NumericTextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.ofdGeodatabase = New System.Windows.Forms.OpenFileDialog()
        Me.tcTransect = New System.Windows.Forms.TabControl()
        Me.tpCast = New System.Windows.Forms.TabPage()
        Me.tcUpgrade = New System.Windows.Forms.TabPage()
        Me.chkClipLegacyTransectsToShorelineExtent = New System.Windows.Forms.CheckBox()
        Me.helpButtonClipLegacyTransectsToShorelineExtent = New System.Windows.Forms.PictureBox()
        Me.btnUpgradeTransectLayerHelp = New System.Windows.Forms.PictureBox()
        Me.gbBaselineType = New System.Windows.Forms.GroupBox()
        Me.rbOffshore = New System.Windows.Forms.RadioButton()
        Me.rbOnshore = New System.Windows.Forms.RadioButton()
        Me.btnUpgrade = New System.Windows.Forms.Button()
        Me.btnCancel2 = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cbUpgradeCandidates = New System.Windows.Forms.ComboBox()
        Me.txtTransectSpacingLegacy = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.gbCasting.SuspendLayout()
        CType(Me.helpButtonClipTransectsToShorelineExtent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbSmoothDistance0, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbSmoothDistance500, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbSmoothDistance2500, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbSmoothDistanceRight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbSmoothDistanceLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbSmoothDistance1000, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbSmoothDistance100, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btnHelpSmoothDistance, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.helpButtonSearchDistance, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbTransectSpacing, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbSearchDistance, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tcTransect.SuspendLayout()
        Me.tpCast.SuspendLayout()
        Me.tcUpgrade.SuspendLayout()
        CType(Me.helpButtonClipLegacyTransectsToShorelineExtent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btnUpgradeTransectLayerHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbBaselineType.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.cmbExistingLayer)
        Me.GroupBox1.Controls.Add(Me.txtPersonalDb)
        Me.GroupBox1.Controls.Add(Me.btnOpenGeodatabase)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(409, 108)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Transect Storage Parameters"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 50)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(217, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Select Existing or Enter New Transect Name"
        '
        'cmbExistingLayer
        '
        Me.cmbExistingLayer.FormattingEnabled = True
        Me.cmbExistingLayer.Location = New System.Drawing.Point(19, 71)
        Me.cmbExistingLayer.Name = "cmbExistingLayer"
        Me.cmbExistingLayer.Size = New System.Drawing.Size(371, 21)
        Me.cmbExistingLayer.TabIndex = 2
        '
        'txtPersonalDb
        '
        Me.txtPersonalDb.Location = New System.Drawing.Point(19, 20)
        Me.txtPersonalDb.Name = "txtPersonalDb"
        Me.txtPersonalDb.Size = New System.Drawing.Size(338, 20)
        Me.txtPersonalDb.TabIndex = 1
        '
        'btnOpenGeodatabase
        '
        Me.btnOpenGeodatabase.Location = New System.Drawing.Point(367, 18)
        Me.btnOpenGeodatabase.Name = "btnOpenGeodatabase"
        Me.btnOpenGeodatabase.Size = New System.Drawing.Size(26, 23)
        Me.btnOpenGeodatabase.TabIndex = 0
        Me.btnOpenGeodatabase.Text = "...."
        Me.btnOpenGeodatabase.UseVisualStyleBackColor = True
        '
        'gbCasting
        '
        Me.gbCasting.Controls.Add(Me.chkClipTransectsToShorelineExtent)
        Me.gbCasting.Controls.Add(Me.helpButtonClipTransectsToShorelineExtent)
        Me.gbCasting.Controls.Add(Me.pbSmoothDistance0)
        Me.gbCasting.Controls.Add(Me.pbSmoothDistance500)
        Me.gbCasting.Controls.Add(Me.pbSmoothDistance2500)
        Me.gbCasting.Controls.Add(Me.pbSmoothDistanceRight)
        Me.gbCasting.Controls.Add(Me.pbSmoothDistanceLeft)
        Me.gbCasting.Controls.Add(Me.pbSmoothDistance1000)
        Me.gbCasting.Controls.Add(Me.pbSmoothDistance100)
        Me.gbCasting.Controls.Add(Me.btnHelpSmoothDistance)
        Me.gbCasting.Controls.Add(Me.Label6)
        Me.gbCasting.Controls.Add(Me.helpButtonSearchDistance)
        Me.gbCasting.Controls.Add(Me.Label26)
        Me.gbCasting.Controls.Add(Me.pbTransectSpacing)
        Me.gbCasting.Controls.Add(Me.txtSearchDistance)
        Me.gbCasting.Controls.Add(Me.txtTransectSpacing)
        Me.gbCasting.Controls.Add(Me.Label5)
        Me.gbCasting.Controls.Add(Me.pbSearchDistance)
        Me.gbCasting.Controls.Add(Me.Label3)
        Me.gbCasting.Controls.Add(Me.txtSmoothDist)
        Me.gbCasting.Controls.Add(Me.Label4)
        Me.gbCasting.Location = New System.Drawing.Point(6, 120)
        Me.gbCasting.Name = "gbCasting"
        Me.gbCasting.Size = New System.Drawing.Size(409, 422)
        Me.gbCasting.TabIndex = 1
        Me.gbCasting.TabStop = False
        Me.gbCasting.Text = "Casting"
        '
        'chkClipTransectsToShorelineExtent
        '
        Me.chkClipTransectsToShorelineExtent.AutoSize = True
        Me.chkClipTransectsToShorelineExtent.Checked = True
        Me.chkClipTransectsToShorelineExtent.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkClipTransectsToShorelineExtent.Location = New System.Drawing.Point(17, 399)
        Me.chkClipTransectsToShorelineExtent.Name = "chkClipTransectsToShorelineExtent"
        Me.chkClipTransectsToShorelineExtent.Size = New System.Drawing.Size(178, 17)
        Me.chkClipTransectsToShorelineExtent.TabIndex = 33
        Me.chkClipTransectsToShorelineExtent.Text = "Clip transects to shoreline extent"
        Me.chkClipTransectsToShorelineExtent.UseVisualStyleBackColor = True
        '
        'helpButtonClipTransectsToShorelineExtent
        '
        Me.helpButtonClipTransectsToShorelineExtent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.helpButtonClipTransectsToShorelineExtent.Cursor = System.Windows.Forms.Cursors.Hand
        Me.helpButtonClipTransectsToShorelineExtent.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.helpButtonClipTransectsToShorelineExtent.Location = New System.Drawing.Point(201, 395)
        Me.helpButtonClipTransectsToShorelineExtent.Name = "helpButtonClipTransectsToShorelineExtent"
        Me.helpButtonClipTransectsToShorelineExtent.Size = New System.Drawing.Size(25, 21)
        Me.helpButtonClipTransectsToShorelineExtent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.helpButtonClipTransectsToShorelineExtent.TabIndex = 34
        Me.helpButtonClipTransectsToShorelineExtent.TabStop = False
        '
        'pbSmoothDistance0
        '
        Me.pbSmoothDistance0.Image = CType(resources.GetObject("pbSmoothDistance0.Image"), System.Drawing.Image)
        Me.pbSmoothDistance0.Location = New System.Drawing.Point(18, 224)
        Me.pbSmoothDistance0.Name = "pbSmoothDistance0"
        Me.pbSmoothDistance0.Size = New System.Drawing.Size(372, 139)
        Me.pbSmoothDistance0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbSmoothDistance0.TabIndex = 24
        Me.pbSmoothDistance0.TabStop = False
        '
        'pbSmoothDistance500
        '
        Me.pbSmoothDistance500.Image = CType(resources.GetObject("pbSmoothDistance500.Image"), System.Drawing.Image)
        Me.pbSmoothDistance500.Location = New System.Drawing.Point(18, 224)
        Me.pbSmoothDistance500.Name = "pbSmoothDistance500"
        Me.pbSmoothDistance500.Size = New System.Drawing.Size(372, 139)
        Me.pbSmoothDistance500.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbSmoothDistance500.TabIndex = 29
        Me.pbSmoothDistance500.TabStop = False
        Me.pbSmoothDistance500.Visible = False
        '
        'pbSmoothDistance2500
        '
        Me.pbSmoothDistance2500.Image = CType(resources.GetObject("pbSmoothDistance2500.Image"), System.Drawing.Image)
        Me.pbSmoothDistance2500.Location = New System.Drawing.Point(18, 224)
        Me.pbSmoothDistance2500.Name = "pbSmoothDistance2500"
        Me.pbSmoothDistance2500.Size = New System.Drawing.Size(372, 139)
        Me.pbSmoothDistance2500.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbSmoothDistance2500.TabIndex = 31
        Me.pbSmoothDistance2500.TabStop = False
        Me.pbSmoothDistance2500.Visible = False
        '
        'pbSmoothDistanceRight
        '
        Me.pbSmoothDistanceRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.pbSmoothDistanceRight.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbSmoothDistanceRight.Image = CType(resources.GetObject("pbSmoothDistanceRight.Image"), System.Drawing.Image)
        Me.pbSmoothDistanceRight.Location = New System.Drawing.Point(365, 197)
        Me.pbSmoothDistanceRight.Name = "pbSmoothDistanceRight"
        Me.pbSmoothDistanceRight.Size = New System.Drawing.Size(25, 21)
        Me.pbSmoothDistanceRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbSmoothDistanceRight.TabIndex = 32
        Me.pbSmoothDistanceRight.TabStop = False
        '
        'pbSmoothDistanceLeft
        '
        Me.pbSmoothDistanceLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.pbSmoothDistanceLeft.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbSmoothDistanceLeft.Image = CType(resources.GetObject("pbSmoothDistanceLeft.Image"), System.Drawing.Image)
        Me.pbSmoothDistanceLeft.Location = New System.Drawing.Point(334, 197)
        Me.pbSmoothDistanceLeft.Name = "pbSmoothDistanceLeft"
        Me.pbSmoothDistanceLeft.Size = New System.Drawing.Size(25, 21)
        Me.pbSmoothDistanceLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbSmoothDistanceLeft.TabIndex = 31
        Me.pbSmoothDistanceLeft.TabStop = False
        '
        'pbSmoothDistance1000
        '
        Me.pbSmoothDistance1000.Image = CType(resources.GetObject("pbSmoothDistance1000.Image"), System.Drawing.Image)
        Me.pbSmoothDistance1000.Location = New System.Drawing.Point(18, 224)
        Me.pbSmoothDistance1000.Name = "pbSmoothDistance1000"
        Me.pbSmoothDistance1000.Size = New System.Drawing.Size(372, 139)
        Me.pbSmoothDistance1000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbSmoothDistance1000.TabIndex = 30
        Me.pbSmoothDistance1000.TabStop = False
        Me.pbSmoothDistance1000.Visible = False
        '
        'pbSmoothDistance100
        '
        Me.pbSmoothDistance100.Image = CType(resources.GetObject("pbSmoothDistance100.Image"), System.Drawing.Image)
        Me.pbSmoothDistance100.Location = New System.Drawing.Point(18, 224)
        Me.pbSmoothDistance100.Name = "pbSmoothDistance100"
        Me.pbSmoothDistance100.Size = New System.Drawing.Size(372, 139)
        Me.pbSmoothDistance100.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbSmoothDistance100.TabIndex = 28
        Me.pbSmoothDistance100.TabStop = False
        Me.pbSmoothDistance100.Visible = False
        '
        'btnHelpSmoothDistance
        '
        Me.btnHelpSmoothDistance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnHelpSmoothDistance.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnHelpSmoothDistance.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.btnHelpSmoothDistance.Location = New System.Drawing.Point(169, 197)
        Me.btnHelpSmoothDistance.Name = "btnHelpSmoothDistance"
        Me.btnHelpSmoothDistance.Size = New System.Drawing.Size(25, 21)
        Me.btnHelpSmoothDistance.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.btnHelpSmoothDistance.TabIndex = 27
        Me.btnHelpSmoothDistance.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(14, 366)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(199, 13)
        Me.Label6.TabIndex = 26
        Me.Label6.Text = "All Distance Measurements are in Meters"
        '
        'helpButtonSearchDistance
        '
        Me.helpButtonSearchDistance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.helpButtonSearchDistance.Cursor = System.Windows.Forms.Cursors.Hand
        Me.helpButtonSearchDistance.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.helpButtonSearchDistance.Location = New System.Drawing.Point(169, 38)
        Me.helpButtonSearchDistance.Name = "helpButtonSearchDistance"
        Me.helpButtonSearchDistance.Size = New System.Drawing.Size(25, 21)
        Me.helpButtonSearchDistance.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.helpButtonSearchDistance.TabIndex = 19
        Me.helpButtonSearchDistance.TabStop = False
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(14, 38)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(73, 13)
        Me.Label26.TabIndex = 16
        Me.Label26.Text = "From Baseline"
        '
        'pbTransectSpacing
        '
        Me.pbTransectSpacing.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbTransectSpacing.Image = CType(resources.GetObject("pbTransectSpacing.Image"), System.Drawing.Image)
        Me.pbTransectSpacing.Location = New System.Drawing.Point(214, 62)
        Me.pbTransectSpacing.Name = "pbTransectSpacing"
        Me.pbTransectSpacing.Size = New System.Drawing.Size(176, 127)
        Me.pbTransectSpacing.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbTransectSpacing.TabIndex = 23
        Me.pbTransectSpacing.TabStop = False
        '
        'txtSearchDistance
        '
        Me.txtSearchDistance.AllowNegative = True
        Me.txtSearchDistance.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearchDistance.DigitsInGroup = 0
        Me.txtSearchDistance.Flags = 0
        Me.txtSearchDistance.Location = New System.Drawing.Point(96, 38)
        Me.txtSearchDistance.MaxDecimalPlaces = 3
        Me.txtSearchDistance.MaxWholeDigits = 9
        Me.txtSearchDistance.Name = "txtSearchDistance"
        Me.txtSearchDistance.Prefix = ""
        Me.txtSearchDistance.RangeMax = 1.7976931348623157E+308R
        Me.txtSearchDistance.RangeMin = -1.7976931348623157E+308R
        Me.txtSearchDistance.Size = New System.Drawing.Size(67, 20)
        Me.txtSearchDistance.TabIndex = 22
        Me.txtSearchDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTransectSpacing
        '
        Me.txtTransectSpacing.AllowNegative = True
        Me.txtTransectSpacing.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTransectSpacing.DigitsInGroup = 0
        Me.txtTransectSpacing.Flags = 0
        Me.txtTransectSpacing.Location = New System.Drawing.Point(316, 37)
        Me.txtTransectSpacing.MaxDecimalPlaces = 3
        Me.txtTransectSpacing.MaxWholeDigits = 9
        Me.txtTransectSpacing.Name = "txtTransectSpacing"
        Me.txtTransectSpacing.Prefix = ""
        Me.txtTransectSpacing.RangeMax = 1.7976931348623157E+308R
        Me.txtTransectSpacing.RangeMin = -1.7976931348623157E+308R
        Me.txtTransectSpacing.Size = New System.Drawing.Size(74, 20)
        Me.txtTransectSpacing.TabIndex = 21
        Me.txtTransectSpacing.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.Label5.Location = New System.Drawing.Point(219, 38)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(91, 13)
        Me.Label5.TabIndex = 20
        Me.Label5.Text = "Transect Spacing"
        '
        'pbSearchDistance
        '
        Me.pbSearchDistance.Image = CType(resources.GetObject("pbSearchDistance.Image"), System.Drawing.Image)
        Me.pbSearchDistance.Location = New System.Drawing.Point(18, 62)
        Me.pbSearchDistance.Name = "pbSearchDistance"
        Me.pbSearchDistance.Size = New System.Drawing.Size(176, 127)
        Me.pbSearchDistance.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbSearchDistance.TabIndex = 18
        Me.pbSearchDistance.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(14, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(133, 13)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "Maximum Search Distance"
        '
        'txtSmoothDist
        '
        Me.txtSmoothDist.AllowNegative = False
        Me.txtSmoothDist.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSmoothDist.DigitsInGroup = 0
        Me.txtSmoothDist.Flags = 65536
        Me.txtSmoothDist.Location = New System.Drawing.Point(122, 197)
        Me.txtSmoothDist.MaxDecimalPlaces = 3
        Me.txtSmoothDist.MaxWholeDigits = 9
        Me.txtSmoothDist.Name = "txtSmoothDist"
        Me.txtSmoothDist.Prefix = ""
        Me.txtSmoothDist.RangeMax = 1000000.0R
        Me.txtSmoothDist.RangeMin = 0R
        Me.txtSmoothDist.Size = New System.Drawing.Size(41, 20)
        Me.txtSmoothDist.TabIndex = 14
        Me.txtSmoothDist.Text = "2500"
        Me.txtSmoothDist.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(14, 202)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(102, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Smoothing Distance"
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(340, 548)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(259, 548)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'ofdGeodatabase
        '
        Me.ofdGeodatabase.Filter = "Personal Geodatabase (*.mdb) |*.mdb"
        '
        'tcTransect
        '
        Me.tcTransect.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcTransect.Controls.Add(Me.tpCast)
        Me.tcTransect.Controls.Add(Me.tcUpgrade)
        Me.tcTransect.Location = New System.Drawing.Point(2, 3)
        Me.tcTransect.Name = "tcTransect"
        Me.tcTransect.SelectedIndex = 0
        Me.tcTransect.Size = New System.Drawing.Size(429, 600)
        Me.tcTransect.TabIndex = 4
        '
        'tpCast
        '
        Me.tpCast.Controls.Add(Me.GroupBox1)
        Me.tpCast.Controls.Add(Me.btnCancel)
        Me.tpCast.Controls.Add(Me.gbCasting)
        Me.tpCast.Controls.Add(Me.btnOK)
        Me.tpCast.Location = New System.Drawing.Point(4, 22)
        Me.tpCast.Name = "tpCast"
        Me.tpCast.Padding = New System.Windows.Forms.Padding(3)
        Me.tpCast.Size = New System.Drawing.Size(421, 574)
        Me.tpCast.TabIndex = 0
        Me.tpCast.Text = "Cast Transects"
        Me.tpCast.UseVisualStyleBackColor = True
        '
        'tcUpgrade
        '
        Me.tcUpgrade.Controls.Add(Me.txtTransectSpacingLegacy)
        Me.tcUpgrade.Controls.Add(Me.chkClipLegacyTransectsToShorelineExtent)
        Me.tcUpgrade.Controls.Add(Me.helpButtonClipLegacyTransectsToShorelineExtent)
        Me.tcUpgrade.Controls.Add(Me.btnUpgradeTransectLayerHelp)
        Me.tcUpgrade.Controls.Add(Me.gbBaselineType)
        Me.tcUpgrade.Controls.Add(Me.btnUpgrade)
        Me.tcUpgrade.Controls.Add(Me.btnCancel2)
        Me.tcUpgrade.Controls.Add(Me.Label7)
        Me.tcUpgrade.Controls.Add(Me.cbUpgradeCandidates)
        Me.tcUpgrade.Location = New System.Drawing.Point(4, 22)
        Me.tcUpgrade.Name = "tcUpgrade"
        Me.tcUpgrade.Padding = New System.Windows.Forms.Padding(3)
        Me.tcUpgrade.Size = New System.Drawing.Size(421, 574)
        Me.tcUpgrade.TabIndex = 1
        Me.tcUpgrade.Text = "Upgrade Transect Layer"
        Me.tcUpgrade.UseVisualStyleBackColor = True
        '
        'chkClipLegacyTransectsToShorelineExtent
        '
        Me.chkClipLegacyTransectsToShorelineExtent.AutoSize = True
        Me.chkClipLegacyTransectsToShorelineExtent.Checked = True
        Me.chkClipLegacyTransectsToShorelineExtent.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkClipLegacyTransectsToShorelineExtent.Location = New System.Drawing.Point(15, 142)
        Me.chkClipLegacyTransectsToShorelineExtent.Name = "chkClipLegacyTransectsToShorelineExtent"
        Me.chkClipLegacyTransectsToShorelineExtent.Size = New System.Drawing.Size(178, 17)
        Me.chkClipLegacyTransectsToShorelineExtent.TabIndex = 35
        Me.chkClipLegacyTransectsToShorelineExtent.Text = "Clip transects to shoreline extent"
        Me.chkClipLegacyTransectsToShorelineExtent.UseVisualStyleBackColor = True
        '
        'helpButtonClipLegacyTransectsToShorelineExtent
        '
        Me.helpButtonClipLegacyTransectsToShorelineExtent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.helpButtonClipLegacyTransectsToShorelineExtent.Cursor = System.Windows.Forms.Cursors.Hand
        Me.helpButtonClipLegacyTransectsToShorelineExtent.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.helpButtonClipLegacyTransectsToShorelineExtent.Location = New System.Drawing.Point(199, 138)
        Me.helpButtonClipLegacyTransectsToShorelineExtent.Name = "helpButtonClipLegacyTransectsToShorelineExtent"
        Me.helpButtonClipLegacyTransectsToShorelineExtent.Size = New System.Drawing.Size(25, 21)
        Me.helpButtonClipLegacyTransectsToShorelineExtent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.helpButtonClipLegacyTransectsToShorelineExtent.TabIndex = 36
        Me.helpButtonClipLegacyTransectsToShorelineExtent.TabStop = False
        '
        'btnUpgradeTransectLayerHelp
        '
        Me.btnUpgradeTransectLayerHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnUpgradeTransectLayerHelp.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnUpgradeTransectLayerHelp.Image = Global.DSASAddin.My.Resources.Resources.question_mark_gray
        Me.btnUpgradeTransectLayerHelp.Location = New System.Drawing.Point(203, 11)
        Me.btnUpgradeTransectLayerHelp.Name = "btnUpgradeTransectLayerHelp"
        Me.btnUpgradeTransectLayerHelp.Size = New System.Drawing.Size(25, 21)
        Me.btnUpgradeTransectLayerHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.btnUpgradeTransectLayerHelp.TabIndex = 20
        Me.btnUpgradeTransectLayerHelp.TabStop = False
        '
        'gbBaselineType
        '
        Me.gbBaselineType.Controls.Add(Me.rbOffshore)
        Me.gbBaselineType.Controls.Add(Me.rbOnshore)
        Me.gbBaselineType.Location = New System.Drawing.Point(9, 108)
        Me.gbBaselineType.Name = "gbBaselineType"
        Me.gbBaselineType.Size = New System.Drawing.Size(406, 48)
        Me.gbBaselineType.TabIndex = 7
        Me.gbBaselineType.TabStop = False
        Me.gbBaselineType.Text = "Legacy baseline type"
        '
        'rbOffshore
        '
        Me.rbOffshore.AutoSize = True
        Me.rbOffshore.Location = New System.Drawing.Point(114, 19)
        Me.rbOffshore.Name = "rbOffshore"
        Me.rbOffshore.Size = New System.Drawing.Size(65, 17)
        Me.rbOffshore.TabIndex = 8
        Me.rbOffshore.TabStop = True
        Me.rbOffshore.Text = "Offshore"
        Me.rbOffshore.UseVisualStyleBackColor = True
        '
        'rbOnshore
        '
        Me.rbOnshore.AutoSize = True
        Me.rbOnshore.Location = New System.Drawing.Point(6, 19)
        Me.rbOnshore.Name = "rbOnshore"
        Me.rbOnshore.Size = New System.Drawing.Size(65, 17)
        Me.rbOnshore.TabIndex = 6
        Me.rbOnshore.TabStop = True
        Me.rbOnshore.Text = "Onshore"
        Me.rbOnshore.UseVisualStyleBackColor = True
        '
        'btnUpgrade
        '
        Me.btnUpgrade.Enabled = False
        Me.btnUpgrade.Location = New System.Drawing.Point(340, 178)
        Me.btnUpgrade.Name = "btnUpgrade"
        Me.btnUpgrade.Size = New System.Drawing.Size(75, 23)
        Me.btnUpgrade.TabIndex = 5
        Me.btnUpgrade.Text = "Upgrade"
        Me.btnUpgrade.UseVisualStyleBackColor = True
        '
        'btnCancel2
        '
        Me.btnCancel2.Location = New System.Drawing.Point(259, 178)
        Me.btnCancel2.Name = "btnCancel2"
        Me.btnCancel2.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel2.TabIndex = 4
        Me.btnCancel2.Text = "Cancel"
        Me.btnCancel2.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 13)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(191, 13)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Select a legacy transect file to upgrade"
        '
        'cbUpgradeCandidates
        '
        Me.cbUpgradeCandidates.FormattingEnabled = True
        Me.cbUpgradeCandidates.Location = New System.Drawing.Point(9, 38)
        Me.cbUpgradeCandidates.Name = "cbUpgradeCandidates"
        Me.cbUpgradeCandidates.Size = New System.Drawing.Size(406, 21)
        Me.cbUpgradeCandidates.TabIndex = 0
        '
        'txtTransectSpacingLegacy
        '
        Me.txtTransectSpacingLegacy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTransectSpacingLegacy.AutoSize = True
        Me.txtTransectSpacingLegacy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.txtTransectSpacingLegacy.Location = New System.Drawing.Point(12, 77)
        Me.txtTransectSpacingLegacy.Name = "txtTransectSpacingLegacy"
        Me.txtTransectSpacingLegacy.Size = New System.Drawing.Size(0, 13)
        Me.txtTransectSpacingLegacy.TabIndex = 37
        '
        'CastTransectsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(433, 604)
        Me.Controls.Add(Me.tcTransect)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "CastTransectsForm"
        Me.Text = "Cast Transects"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.gbCasting.ResumeLayout(False)
        Me.gbCasting.PerformLayout()
        CType(Me.helpButtonClipTransectsToShorelineExtent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbSmoothDistance0, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbSmoothDistance500, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbSmoothDistance2500, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbSmoothDistanceRight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbSmoothDistanceLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbSmoothDistance1000, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbSmoothDistance100, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btnHelpSmoothDistance, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.helpButtonSearchDistance, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbTransectSpacing, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbSearchDistance, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tcTransect.ResumeLayout(False)
        Me.tpCast.ResumeLayout(False)
        Me.tcUpgrade.ResumeLayout(False)
        Me.tcUpgrade.PerformLayout()
        CType(Me.helpButtonClipLegacyTransectsToShorelineExtent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btnUpgradeTransectLayerHelp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbBaselineType.ResumeLayout(False)
        Me.gbBaselineType.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnOpenGeodatabase As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbExistingLayer As System.Windows.Forms.ComboBox
    Friend WithEvents txtPersonalDb As System.Windows.Forms.TextBox
    Friend WithEvents gbCasting As System.Windows.Forms.GroupBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ofdGeodatabase As System.Windows.Forms.OpenFileDialog
    Friend WithEvents txtSmoothDist As AMS.TextBox.NumericTextBox
    Friend WithEvents helpButtonSearchDistance As System.Windows.Forms.PictureBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents pbTransectSpacing As System.Windows.Forms.PictureBox
    Friend WithEvents txtSearchDistance As AMS.TextBox.NumericTextBox
    Friend WithEvents txtTransectSpacing As AMS.TextBox.NumericTextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents pbSearchDistance As System.Windows.Forms.PictureBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents pbSmoothDistance0 As System.Windows.Forms.PictureBox
    Friend WithEvents btnHelpSmoothDistance As System.Windows.Forms.PictureBox
    Friend WithEvents tcTransect As System.Windows.Forms.TabControl
    Friend WithEvents tpCast As System.Windows.Forms.TabPage
    Friend WithEvents tcUpgrade As System.Windows.Forms.TabPage
    Friend WithEvents btnUpgrade As System.Windows.Forms.Button
    Friend WithEvents btnCancel2 As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cbUpgradeCandidates As System.Windows.Forms.ComboBox
    Friend WithEvents gbBaselineType As System.Windows.Forms.GroupBox
    Friend WithEvents rbOffshore As System.Windows.Forms.RadioButton
    Friend WithEvents rbOnshore As System.Windows.Forms.RadioButton
    Friend WithEvents btnUpgradeTransectLayerHelp As System.Windows.Forms.PictureBox
    Friend WithEvents pbSmoothDistanceRight As System.Windows.Forms.PictureBox
    Friend WithEvents pbSmoothDistanceLeft As System.Windows.Forms.PictureBox
    Friend WithEvents pbSmoothDistance1000 As System.Windows.Forms.PictureBox
    Friend WithEvents pbSmoothDistance100 As System.Windows.Forms.PictureBox
    Friend WithEvents pbSmoothDistance2500 As System.Windows.Forms.PictureBox
    Friend WithEvents pbSmoothDistance500 As System.Windows.Forms.PictureBox
    Friend WithEvents chkClipTransectsToShorelineExtent As System.Windows.Forms.CheckBox
    Friend WithEvents helpButtonClipTransectsToShorelineExtent As System.Windows.Forms.PictureBox
    Friend WithEvents chkClipLegacyTransectsToShorelineExtent As System.Windows.Forms.CheckBox
    Friend WithEvents helpButtonClipLegacyTransectsToShorelineExtent As System.Windows.Forms.PictureBox
    Friend WithEvents txtTransectSpacingLegacy As System.Windows.Forms.Label
End Class
