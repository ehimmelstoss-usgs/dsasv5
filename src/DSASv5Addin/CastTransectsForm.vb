Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports System.Runtime.InteropServices


Public Class CastTransectsForm
    Public m_transectName As String
    Friend radLogNone As System.Windows.Forms.RadioButton

    Private Sub buttonOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        'validate form inputs
        If validateForm() Then
            'hide the form
            Me.Hide()
            DSASUtility.log(TraceLevel.Info, "Casting starts...")

            Try
                My.Settings.Transect_Leg_Length = Me.txtSmoothDist.Text
                If Not Integer.TryParse(txtTransectSpacing.Text, My.Settings.Spacing) Then My.Settings.Spacing = 0
                If Not Integer.TryParse(txtSearchDistance.Text, My.Settings.Search_Distance) Then My.Settings.Search_Distance = 20000

                'change the mouse pointer
                Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                OldCast.cast(Me)
            Catch
            Finally
                'change the mouse pointer to original
                Me.Cursor = System.Windows.Forms.Cursors.Default
            End Try

            DSASUtility.log(TraceLevel.Info, "Casting Ends...")
            DSASUtility.log(TraceLevel.Info, "**************************************** ")
        End If
    End Sub

    '
    'handle cancel request
    '
    Private Sub buttonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click, btnCancel2.Click
        Me.Dispose()
    End Sub

    '
    'allow user to browse to personal geodatabase
    '
    Private Sub btnOpenGeodatabase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenGeodatabase.Click
        If Me.ofdGeodatabase().ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            If GeoDB.geodbIsCurrent(Me.ofdGeodatabase.FileName) Then
                Me.txtPersonalDb.Text = Me.ofdGeodatabase.FileName
            Else
                log(TraceLevel.Error, "The geodatabase you selected is not current version." + vbCrLf + "Either select a current geodatabase or upgrade this geodatabase.")
            End If
        End If
    End Sub

    '
    'validate form inputs
    '
    Private Function validateForm() As Boolean
        'check if personal geodatabase name is provided
        If (txtPersonalDb.Text = "") Then
            log(TraceLevel.Error, "Output Geodatabase: Please browse to a personal geodatabase!")
            Return False
        End If

        'check if new transect feature class name is typed in and valid
        m_transectName = cmbExistingLayer.Text.Trim

        If Not isValidTransectName(m_transectName, True) Then
            Return False
        End If

        Return True
    End Function
    '
    'populate personal geodatabase name when layer selection changes
    '
    Private Sub cmbExistingLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbExistingLayer.SelectedIndexChanged
        Dim selLyr As String
        Dim featLyr As IFeatureLayer
        Dim ds As IDataset

        If cmbExistingLayer.SelectedItem IsNot Nothing Then
            selLyr = CType(cmbExistingLayer.SelectedItem, String)
            featLyr = MapUtility.findFeatureLayer(selLyr, "transect")
            If featLyr IsNot Nothing Then
                ds = CType(featLyr, IDataset)
                txtPersonalDb.Text = ds.Workspace.PathName
            End If
        End If
    End Sub


    Private Sub CastTransects_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            DSASUtility.log("CastTransects_Load: Enter")

            DSASUtility.log("CastTransects_Load: Finding layers...")
            For Each lyr As IFeatureLayer In MapUtility.featureLayers("transect")
                If GeoDB.geodbIsCurrent(lyr) Then
                    Me.cmbExistingLayer.Items.Add(lyr.Name)
                End If
            Next

            Try
                DSASUtility.log("CastTransects_Load: Set workspace...")
                If My.Settings.Baseline_Feature_Layer > "" Then
                    Me.txtPersonalDb.Text = MapUtility.workspaceForLayer(My.Settings.Baseline_Feature_Layer).PathName
                End If

                If My.Settings.Spacing > 0.0 Then txtTransectSpacing.Text = My.Settings.Spacing
                If My.Settings.Search_Distance > 0.0 Then txtSearchDistance.Text = My.Settings.Search_Distance
            Catch ex As Exception
                My.Settings.Baseline_Feature_Layer = ""
            End Try

            DSASUtility.log("CastTransects_Load: Set txtSmoothDist")
            Me.txtSmoothDist.Text = My.Settings.Transect_Leg_Length

            smoothDistanceShiftImage()

            initUpgradeCandidates()

        Catch argEx As ArgumentException
            DSASUtility.handleArgException(argEx)
            Exit Try
        Catch comex As COMException
            DSASUtility.handleComException(comex)
            Exit Try
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
            Exit Try
        End Try
    End Sub


    Private Sub cmbExistingLayer_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbExistingLayer.TextChanged
        isValidTransectName(sender.text)
    End Sub


    Private Sub btnHelpSmoothDistance_Click(sender As Object, e As EventArgs) Handles btnHelpSmoothDistance.Click
        MsgBox("The smoothing value creates a long and straight orthogonal reference for DSAS to use when casting transects.  A longer smoothing distance will result in adjacent transects that are oriented more parallel to each other.  Smoothing distance has more influence along curved baseline segments than straight segments. The default value of 2500 meters is considered the upper limit of effectiveness.", MsgBoxStyle.ApplicationModal, "Smoothing Distance")
    End Sub

    Private Sub helpButtonSearchDistance_Click(sender As Object, e As EventArgs) Handles helpButtonSearchDistance.Click
        MsgBox("This optional value provides a maximum distance in meters that DSAS will search for shorelines, extending out from either side of the baseline. If you entered 50, DSAS would search no more than 50 meters on either side of the baseline for shoreline data and any data beyond that would be ignored. This value can be useful when shoreline data that you do not want to incorporate into rate analysis is in close proximity to shoreline data that you do want to use. For example, if you are calculating shoreline change rates on an island, you can make sure shorelines on the opposite side of the island aren’t included by setting a maximum search distance.", MsgBoxStyle.ApplicationModal, "Maximum Search Distance From Baseline")
    End Sub



    Private upgradeCandidatesInitialized As Boolean = False

    Private Sub initUpgradeCandidates()
        log("Detecting legacy transects")
        If Not upgradeCandidatesInitialized Then
            For Each lyr As IFeatureLayer In MapUtility.featureLayers("transectv4")
                Me.cbUpgradeCandidates.Items.Add(lyr.Name)
            Next
            If cbUpgradeCandidates.Items.Count = 0 Then
                cbUpgradeCandidates.Text = "No legacy transect layers found in the map"
            End If
            updateUpgradeEnabled()
        End If
    End Sub


    Private Sub cbUpgradeCandidates_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbUpgradeCandidates.SelectedIndexChanged
        updateUpgradeEnabled()
    End Sub


    Private Sub rb_CheckedChanged(sender As Object, e As EventArgs) Handles rbOnshore.CheckedChanged, rbOffshore.CheckedChanged
        updateUpgradeEnabled()
    End Sub


    Private Sub btnUpgrade_Click(sender As Object, e As EventArgs) Handles btnUpgrade.Click
        Dim baselineType As String = IIf(rbOnshore.Checked, "onshore", IIf(rbOffshore.Checked, "offshore", ""))
        If baselineType <> "" AndAlso cbUpgradeCandidates.SelectedItem <> "" Then
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Try
                LegacyTransectConverter.transectConvertFromV4ToV5(cbUpgradeCandidates.SelectedItem, baselineType)
            Catch ex As Exception
                log("Error while detecting legacy transects!")
            Finally
                Me.Cursor = System.Windows.Forms.Cursors.Default
                My.Document.CurrentContentsView.Refresh(Nothing)
                My.Document.ActiveView.Refresh()
            End Try

        End If
    End Sub


    Private Sub updateUpgradeEnabled()
        cbUpgradeCandidates.Enabled = cbUpgradeCandidates.Items.Count > 0
        gbBaselineType.Enabled = cbUpgradeCandidates.SelectedItem IsNot Nothing
        btnUpgrade.Enabled = cbUpgradeCandidates.SelectedItem IsNot Nothing AndAlso (rbOnshore.Checked OrElse rbOffshore.Checked)
    End Sub

    Private Sub btnUpgradeTransectLayerHelp_Click(sender As Object, e As EventArgs) Handles btnUpgradeTransectLayerHelp.Click
        MsgBox("Legacy transects (previously created in DSAS 4.x) may be used in DSAS version 5 but must be upgraded to be compatible. Prior to upgrading transects, please make sure the Default Parameters settings are populated and accurately represent the data. Baseline type is maintained, so if an offshore baseline was used to cast the legacy transects, select Offshore for Legacy baseline type.",, "Upgrade Transect File")
    End Sub


    Private pbSmoothDistanceImageIndex As Integer = 0

    Private Sub pbSmoothDistanceRight_Click(sender As Object, e As EventArgs) Handles pbSmoothDistanceRight.Click
        pbSmoothDistanceImageIndex = Math.Min(pbSmoothDistanceImageIndex + 1, 4)
        smoothDistanceShiftImage()
    End Sub

    Private Sub pbSmoothDistanceLeft_Click(sender As Object, e As EventArgs) Handles pbSmoothDistanceLeft.Click
        pbSmoothDistanceImageIndex = Math.Max(pbSmoothDistanceImageIndex - 1, 0)
        smoothDistanceShiftImage()
    End Sub

    Private Sub smoothDistanceShiftImage()
        Dim suffixes() = {"0", "100", "500", "1000", "2500"}
        For i As Integer = 0 To suffixes.Length - 1
            Dim suffix As String = suffixes(i)
            Me.gbCasting.Controls("pbSmoothDistance" + suffix).Location = Me.pbSmoothDistance0.Location
            Me.gbCasting.Controls("pbSmoothDistance" + suffix).Visible = (i = pbSmoothDistanceImageIndex)
        Next
        pbSmoothDistanceLeft.Enabled = (pbSmoothDistanceImageIndex > 0)
        pbSmoothDistanceRight.Enabled = (pbSmoothDistanceImageIndex < 4)
    End Sub

End Class