Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports System.Runtime.InteropServices
Imports System.Windows
Imports ESRI.ArcGIS.Display


Public Class SetDefaultsForm
    Private _cancelled As Boolean = False

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        btnHelpXectRelativeToBl.Location = New Drawing.Point(btnHelpXectRelativeToBl.Location.X, gbXectRelativeToBl.Location.Y - 4)
        GroupBox2.Size = New Drawing.Size(GroupBox2.Size.Width, GroupBox2.Size.Height + 6)
    End Sub

    Public ReadOnly Property cancelled() As Boolean
        Get
            Return _cancelled
        End Get
    End Property

    Public Shared Function validateSettings(ByVal op As String, Optional ByVal displayHowTo As Boolean = False) As Boolean
        Dim msg As String = ""
        Dim howTo As String = IIf(displayHowTo, "Please complete selections in Default Parameters window.", "")

        If op = "cast" Then
            msg = validateCastSettings()
        ElseIf op = "stats" Then
            ' The check here was deprecated and replaced with extensive integrity checks in RateCalculation.vb
        End If

        If msg.Trim <> "" Then
            log(TraceLevel.Error, msg & howTo)
        End If

        Return msg = ""
    End Function

    Public Shared Function validateCastSettings() As String
        Dim msg As String = ""

        If DSASUtility.nv(My.Settings.Baseline_Feature_Layer) = "" Then msg += "Cannot proceed without a baseline layer selection!" & vbCrLf
        If DSASUtility.nv(My.Settings.Shoreline_Feature_Layer) = "" Then msg += "Cannot proceed without a shoreline layer selection!" & vbCrLf
        If My.Settings.Spacing <= 0 Then msg += "Cannot proceed without a valid transect spacing value!" & vbCrLf
        If My.Settings.Search_Distance <= 0 Then msg += "Cannot proceed without a search distance value!" & vbCrLf
        Return msg
    End Function


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        _cancelled = False

        If radLogExtended.Checked Then
            My.Settings.Trace_Level = TraceLevel.Verbose
        ElseIf radLogRegular.Checked Then
            My.Settings.Trace_Level = TraceLevel.Info
        Else
            My.Settings.Trace_Level = TraceLevel.Off
        End If

        If Not Double.TryParse(txtDefaultUncertainty.Text, My.Settings.Uncertainty) Then My.Settings.Uncertainty = My.Settings.Properties("Uncertainty").DefaultValue
        My.Settings.Baseline_Feature_Layer = cmbBaselineFC.SelectedItem
        My.Settings.Shoreline_Feature_Layer = cmbShorelineFC.SelectedItem
        My.Settings.Shoreline_Date_Field = cmbDateField.SelectedItem
        My.Settings.Shoreline_Uncertainty_Field = cmbUncertaintyField.SelectedItem
        My.Settings.Land_On_Right_Side = rad_right.Checked ' change settings
        My.Settings.Seaward = radSeaward.Checked
        My.Settings.Baseline_ID_Field = cmbBaselineID.SelectedItem
        My.Settings.Baseline_Group_Field = cmbBaseGroup.SelectedItem
        My.Settings.search_distance_field = cmbBaselineSearchDistance.SelectedItem
        My.Settings.Shoreline_Type_Field = cmbShorelineTypeField.SelectedItem
        My.Settings.Shoreline_Uncertainty_Table = cmbLidar.SelectedItem

        If rbCastBoth.Checked Then My.Settings.CastDirection = "BOTH"
        If rbCastOnShore.Checked Then My.Settings.CastDirection = "ONSHORE"
        If rbCastOffShore.Checked Then My.Settings.CastDirection = "OFFSHORE"

        ' Save any changes on metadata settings tab
        Metadata.SaveMetadataTab(Me)

        Me.Close()
    End Sub

    Private Sub cmbShorelineFC_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbShorelineFC.SelectedIndexChanged
        Dim featLyr As IFeatureLayer
        Dim flds As IFields
        Dim i As Int32
        Dim fld As IField

        'clear dependent combo boxes
        clearShorelineDependents()

        'find the selected feature layer
        featLyr = MapUtility.findFeatureLayer(cmbShorelineFC.SelectedItem.ToString(), "shoreline")

        'populate field combo boxes
        flds = featLyr.FeatureClass.Fields
        For i = 0 To flds.FieldCount - 1
            fld = flds.Field(i)
            'AE: It would be better if we pulled in types from DSAS_tables rather than hardwiring here.
            'Minor inconvenience as these won't change once agreed on.
            If fld.Type = esriFieldType.esriFieldTypeString Then
                Me.cmbDateField.Items.Add(fld.AliasName)
                Me.cmbShorelineTypeField.Items.Add(fld.AliasName)
            ElseIf fld.Type = esriFieldType.esriFieldTypeDouble OrElse fld.Type = esriFieldType.esriFieldTypeSingle OrElse fld.Type = esriFieldType.esriFieldTypeInteger OrElse fld.Type = esriFieldType.esriFieldTypeSmallInteger Then
                Me.cmbUncertaintyField.Items.Add(fld.AliasName)
            End If
        Next

        'Try to use the previous value...
        cmbUncertaintyField.SelectedItem = cmbUncertaintyField.Text
        If cmbUncertaintyField.SelectedItem Is Nothing Then cmbUncertaintyField.Text = ""

        'Try to use the previous value...
        cmbDateField.SelectedItem = cmbDateField.Text
        If cmbDateField.SelectedItem Is Nothing Then cmbDateField.Text = ""

        'Try to use the previous value...
        cmbShorelineTypeField.SelectedItem = cmbShorelineTypeField.Text
        If cmbShorelineTypeField.SelectedItem Is Nothing Then cmbShorelineTypeField.Text = ""

    End Sub

    Private Sub SetDefaults_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Initialize the transect layer tool in case it wasn't
        If DSAS.TransectLyrToolInstance IsNot Nothing Then
            DSAS.TransectLyrToolInstance.InitializeLayers()
        End If

        radSeaward.Checked = My.Settings.Seaward
        radLandward.Checked = Not radSeaward.Checked

        rad_right.Checked = My.Settings.Land_On_Right_Side
        rad_left.Checked = Not (rad_right.Checked) 'OrElse radOnOff.Checked)

        If My.Settings.Trace_Level = TraceLevel.Verbose Then
            radLogExtended.Checked = True
        ElseIf My.Settings.Trace_Level >= TraceLevel.Info Then
            radLogRegular.Checked = True
        Else
            Me.radLogNone.Checked = True
        End If

        Try
            'populate baseline and shoreline feature class combo boxes with 
            'feature class names
            For Each lyr As IFeatureLayer In MapUtility.featureLayers("baseline")
                cmbBaselineFC.Items.Add(lyr.Name)
            Next

            For Each lyr As IFeatureLayer In MapUtility.featureLayers("shoreline")
                cmbShorelineFC.Items.Add(lyr.Name)
            Next

            For Each tbl As IStandaloneTable In MapUtility.uncertaintyTables()
                log(tbl.Name)
                cmbLidar.Items.Add(tbl.Name)
            Next

            cmbBaselineFC.SelectedItem = My.Settings.Baseline_Feature_Layer
            cmbShorelineFC.SelectedItem = My.Settings.Shoreline_Feature_Layer

            cmbBaselineID.SelectedItem = My.Settings.Baseline_ID_Field
            cmbBaseGroup.SelectedItem = My.Settings.Baseline_Group_Field
            cmbBaselineSearchDistance.SelectedItem = My.Settings.search_distance_field
            cmbDateField.SelectedItem = My.Settings.Shoreline_Date_Field
            cmbUncertaintyField.SelectedItem = My.Settings.Shoreline_Uncertainty_Field
            cmbShorelineTypeField.SelectedItem = My.Settings.Shoreline_Type_Field
            cmbLidar.SelectedItem = My.Settings.Shoreline_Uncertainty_Table

            If My.Settings.Uncertainty > 0.0 Then txtDefaultUncertainty.Double = My.Settings.Uncertainty

            rbCastBoth.Checked = (My.Settings.CastDirection = "BOTH")
            rbCastOnShore.Checked = (My.Settings.CastDirection = "ONSHORE")
            rbCastOffShore.Checked = (My.Settings.CastDirection = "OFFSHORE")

            helpButtonLandDirection.Location = New Drawing.Point(lblLandRelToBlOrientation.Location.X + lblLandRelToBlOrientation.Width + 5, helpButtonLandDirection.Location.Y)
            helpButtonVisualizeBaselineOrientation.Location = New Drawing.Point(chkVisualizeBaselineOrientation.Location.X + chkVisualizeBaselineOrientation.Width + 5, helpButtonVisualizeBaselineOrientation.Location.Y)

            Metadata.LoadMetadataTab(Me)

            Me.ToolTip1.SetToolTip(btnShowLogLocation, DSAS.logPath)

        Catch comex As COMException
            DSASUtility.handleComException(comex)
            Exit Try
        Catch ex As Exception
            DSASUtility.handleException(ex)
            Exit Try
        End Try


    End Sub


    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        _cancelled = True
        Me.Hide()
    End Sub

    Private Sub radshore_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.pbLeft1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        'Me.pbOnOff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbRight1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle

        'If radOnOff.Checked Then
        'Me.pbOnOff.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        If rad_right.Checked Then
            Me.pbRight1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Else
            Me.pbLeft1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        End If
    End Sub

    'Private Sub radClosest_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radLandward.CheckedChanged, radSeaward.CheckedChanged
    '    Me.pbClosest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    '    Me.pbFarthest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle

    '    If Me.radSeaward.Checked Then
    '        Me.pbFarthest.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
    '    Else
    '        Me.pbClosest.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
    '    End If
    'End Sub

    Private Sub cmbBaselineFC_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBaselineFC.SelectedIndexChanged
        Dim featLyr As IFeatureLayer
        Dim flds As IFields
        Dim i As Int32
        Dim fld As IField

        'clear the combo boxes
        Me.cmbBaselineID.Items.Clear()
        Me.cmbBaseGroup.Items.Clear()
        Me.cmbBaselineSearchDistance.Items.Clear()

        'find the selected feature layer
        featLyr = MapUtility.findFeatureLayer(Me.cmbBaselineFC.SelectedItem.ToString(), "baseline")

        'populate field combo boxes
        flds = featLyr.FeatureClass.Fields
        For i = 0 To flds.FieldCount - 1
            fld = flds.Field(i)
            'AE: It would be better if we pulled in types from DSAS_tables rather than hardwiring here.
            'Minor inconvenience as these won't change once agreed on.
            If fld.Type = esriFieldType.esriFieldTypeInteger OrElse fld.Type = esriFieldType.esriFieldTypeSmallInteger Then
                Me.cmbBaselineID.Items.Add(fld.AliasName)
                Me.cmbBaseGroup.Items.Add(fld.AliasName)
            End If

            If fld.Type = esriFieldType.esriFieldTypeInteger OrElse
               fld.Type = esriFieldType.esriFieldTypeSmallInteger OrElse
               fld.Type = esriFieldType.esriFieldTypeDouble OrElse
               fld.Type = esriFieldType.esriFieldTypeSingle Then
                Me.cmbBaselineSearchDistance.Items.Add(fld.AliasName)
            End If
        Next

        'Try to use the previous value...
        cmbBaselineID.SelectedItem = cmbBaselineID.Text
        If cmbBaselineID.SelectedItem Is Nothing Then cmbBaselineID.Text = ""

        cmbBaseGroup.SelectedItem = cmbBaseGroup.Text
        If cmbBaseGroup.SelectedItem Is Nothing Then cmbBaseGroup.Text = ""

        chkVisualizeBaselineOrientation.Checked = isBaselineOrientationVisualized()

        cmbBaselineSearchDistance.SelectedItem = cmbBaselineSearchDistance.Text
        If cmbBaselineSearchDistance.SelectedItem Is Nothing Then cmbBaselineSearchDistance.Text = ""
    End Sub

    Private Sub btnOpenLogFolder_Click(sender As System.Object, e As System.EventArgs) Handles btnShowLogLocation.Click
        showLogLocation()
    End Sub

    Private Sub helpButtonBaselineGroup_Click(sender As Object, e As EventArgs) Handles helpButtonBaselineGroup.Click
        MsgBox("This optional field can be used to organize rate calculation results into groups. For example, you may want to group a series of baselines into a sub region of interest. This field allows you to assign a group value to each baseline segment, and multiple baseline segments can have the same value. Each group will have average statistics reported in the summary text file you can choose to have DSAS output from the options in the calculate statistics window of the DSAS toolbar.", MsgBoxStyle.ApplicationModal, "Baseline Group Field")
    End Sub

    Private Sub helpButtonLandDirection_Click(sender As Object, e As EventArgs) Handles helpButtonLandDirection.Click
        Dim helpFrm As LandRelatiToBaseliHelpForm = New LandRelatiToBaseliHelpForm()
        helpFrm.Show()
    End Sub

    Private Sub helpButtonVisualizeBaselineOrientation_Click(sender As Object, e As EventArgs) Handles helpButtonVisualizeBaselineOrientation.Click
        MsgBox("Clicking this button will automatically add arrows to the baseline symbology indicating the direction of each baseline segment. Baseline segments that flow in the same direction will allow DSAS to establish the right or left-hand rule orienting the land and water with respect to the baseline. This will allow DSAS to properly measure erosion and accretion. Any baselines flowing in the opposite direction may be flipped in a standard Arc editing session. Select the baseline segment to flip by double-clicking it, then right-click. The option to flip will appear in the pop-up window. Please refer to the Baseline Settings Tab section of the DSAS user manual for a more detailed explanation.", MsgBoxStyle.ApplicationModal, "Baseline Orientation")
    End Sub

    Private Sub chkVisualizeBaselineOrientation_CheckedChanged(sender As Object, e As EventArgs) Handles chkVisualizeBaselineOrientation.CheckedChanged
        applyVisualizeBaselineOrientationSetting()
    End Sub


    Private Sub applyVisualizeBaselineOrientationSetting()
        Dim lyr As IGeoFeatureLayer = MapUtility.findFeatureLayer(cmbBaselineFC.SelectedItem)
        If lyr Is Nothing Then
            log(TraceLevel.Error, "Can't find the baseline layer, please check your baseline setting!")
            Exit Sub
        End If

        Dim simpleRenderer As ISimpleRenderer
        Dim oldColor As IColor = Nothing
        Dim oldWidth As Double = 1
        Try
            simpleRenderer = DirectCast(lyr.Renderer, ISimpleRenderer)
            Dim oldSymbol As ILineSymbol = simpleRenderer.Symbol
            oldColor = oldSymbol.Color
            oldWidth = oldSymbol.Width
        Catch ex As Exception
            simpleRenderer = New SimpleRendererClass
            lyr.Renderer = simpleRenderer
        End Try

        If chkVisualizeBaselineOrientation.Checked Then
            'Define an arrow marker  
            Dim arrowMarkerSymbol As IArrowMarkerSymbol = New ArrowMarkerSymbolClass()
            If oldColor IsNot Nothing Then arrowMarkerSymbol.Color = oldColor
            arrowMarkerSymbol.Size = Math.Max(2, oldWidth) * 3
            arrowMarkerSymbol.Length = Math.Max(2, oldWidth) * 4
            arrowMarkerSymbol.Width = Math.Max(2, oldWidth) * 3


            'Create cartographic line symbol  
            Dim cartographicLineSymbol As ICartographicLineSymbol = New CartographicLineSymbolClass()
            If oldColor IsNot Nothing Then cartographicLineSymbol.Color = oldColor
            cartographicLineSymbol.Width = oldWidth

            'Define simple line decoration  
            Dim simpleLineDecorationElement As ISimpleLineDecorationElement = New SimpleLineDecorationElementClass()

            'Place the arrow at the end of the line (the "To" point in the geometry below)  
            simpleLineDecorationElement.PositionAsRatio = False
            For i As Long = My.Settings.Spacing / 2 To GeoDB.getLayerTotalLength(lyr) Step My.Settings.Spacing * 10
                simpleLineDecorationElement.AddPosition(i)
            Next

            simpleLineDecorationElement.MarkerSymbol = arrowMarkerSymbol

            'Define line decoration  
            Dim lineDecoration As ILineDecoration = New LineDecorationClass()
            lineDecoration.AddElement(simpleLineDecorationElement)

            'Set line properties  
            Dim lineProperties As ILineProperties = DirectCast(cartographicLineSymbol, ILineProperties)
            lineProperties.LineDecoration = lineDecoration

            simpleRenderer.Symbol = cartographicLineSymbol  ' CreateSimpleLineSymbol(RgbColor, 5, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSSolid)
        Else
            'Revert to basic line symbol
            Dim simpleLineSymbol As ISimpleLineSymbol = New SimpleLineSymbolClass()
            If oldColor IsNot Nothing Then simpleLineSymbol.Color = oldColor
            simpleLineSymbol.Width = Math.Max(1, oldWidth) / 3

            simpleRenderer.Symbol = simpleLineSymbol
        End If

        My.Document.CurrentContentsView.Refresh(Nothing)
        My.Document.ActiveView.Refresh()
    End Sub

    ''' <summary>
    ''' Determine if baseline layer has been "visualized".
    ''' </summary>
    ''' <returns>True if baseline layer is symbolized with an arrow marker. False otherwise</returns>
    Function isBaselineOrientationVisualized() As Boolean
        Try
            Dim lyr As IGeoFeatureLayer = MapUtility.findFeatureLayer(cmbBaselineFC.SelectedItem)

            Dim simpleRenderer As ISimpleRenderer = TryCast(lyr.Renderer, ISimpleRenderer)
            Dim cartographicLineSymbol As ICartographicLineSymbol = TryCast(simpleRenderer.Symbol, ICartographicLineSymbol)
            Dim lineProperties As ILineProperties = TryCast(cartographicLineSymbol, ILineProperties)
            For i As Integer = 0 To lineProperties.LineDecoration.ElementCount - 1
                Try
                    Dim ld As ILineDecorationElement = lineProperties.LineDecoration.Element(i)
                    Dim arrowMarkerSymbol As IArrowMarkerSymbol = TryCast(TryCast(ld, ISimpleLineDecorationElement), IArrowMarkerSymbol)
                    ' If we got here successfully then the line is symbolized with an arrow marker
                    Return True
                Catch ex As Exception
                End Try
            Next
        Catch ex As Exception
        End Try

        Return False
    End Function

    Private Sub SetDefaultsForm_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        grpBias.Visible = cmbLidar.Items.Count > 0
    End Sub

    Private Sub helpButtonShorelineType_MouseClick(sender As Object, e As Forms.MouseEventArgs) Handles helpButtonShorelineType.MouseClick
        Dim form As ShorelineHelpForm = New ShorelineHelpForm()
        form.Show()
    End Sub

    Private Sub helpButtonBaselineSearchDistance_Click(sender As Object, e As EventArgs) Handles helpButtonBaselineSearchDistance.Click
        Dim popup As New PopupWithImage
        popup.Text = "Baseline Search Distance Field"
        'popup.textArea.Font.Size =
        popup.textArea.Text = "Baseline Search Distance is an optional field that can be used to specify a maximum search distance in meters for each baseline segment. When utilized, this value takes precedence over the default maximum search distance specified within the default parameters. This may be useful when a large distance is needed for one section of the study area, but may result in overshoots in another. The field may be added using the Attribute Automator in the DSAS toolbar."
        popup.pictureBox.Image = My.Resources.baseline_search_distance_help
        popup.pictureBox.SizeMode = Forms.PictureBoxSizeMode.Zoom
        popup.textArea.Padding = New Forms.Padding(20)
        popup.textArea.Margin = New Forms.Padding(10)

        popup.ShowDialog()
    End Sub


    Private Sub cmbShorelineFC_TextChanged(sender As Object, e As EventArgs) Handles cmbShorelineFC.TextChanged
        clearShorelineDependents()
    End Sub


    Private Sub clearShorelineDependents()
        cmbDateField.Items.Clear()
        cmbUncertaintyField.Items.Clear()
        cmbShorelineTypeField.Items.Clear()

        cmbDateField.Text = ""
        cmbUncertaintyField.Text = ""
        cmbShorelineTypeField.Text = ""
        cmbLidar.Text = ""
        cmbLidar.SelectedItem = Nothing
    End Sub

    Private Sub btnHelpDefaultUncerty_Click(sender As Object, e As EventArgs) Handles btnHelpDefaultUncerty.Click
        MsgBox("In order to compute shoreline change statistics, each shoreline must have a positional uncertainty associated with it. Shorelines come from different data sources, therefore the process steps performed from the source data to the line represented on the map all contribute to the overall uncertainty of that shoreline position. These measurements, in meters, are represented in the uncertainty field of the shoreline feature class attribute table which is selected in the Default Parameters window." + vbCrLf + vbCrLf + "If the uncertainty attribute field is not populated, DSAS will use the default value stored here instead. We provide a suggested default value of 10 meters, which is the approximate average of uncertainty of various shoreline data types used in recent regional reports that the U.S. Geological Survey has published under the National Assessment of Shoreline Change project. Where possible, users are encouraged to perform a quantitative assessment of the positional uncertainty associated with each shoreline they are using, as this default value may over or underestimate the uncertainty of any given dataset.", MsgBoxStyle.ApplicationModal, "Default Data Uncertainty")
    End Sub

    Private Sub helpButtonLidarTable_Click(sender As Object, e As EventArgs) Handles helpButtonLidarTable.Click
        MsgBox("The uncertainty table contains data associated with performing the proxy-datum bias correction between the datum-based Operational Mean High Water shorelines typically derived from lidar data and the High Water Line shorelines from other sources such as T-sheets. The table must contain the profile identification field used to linear reference the table to the shoreline data, the proxy datum bias used to correct the HWL shoreline distances on each transect to the MHW datum, the uncertainty associated with the bias, and the positional uncertainty associated with the lidar shoreline from which the bias data were derived.", MsgBoxStyle.ApplicationModal, "Uncertainty Table")
    End Sub

    Private Sub helpButtonBaselineID_Click(sender As Object, e As EventArgs) Handles helpButtonBaselineID.Click
        MsgBox("The baseline ID field is required by DSAS. DSAS uses this value to determine the ordering sequence of transects when the baseline feature class contains multiple segments. If this attribute field is created prior to drawing baseline segments, the ID value defaults to zero. The attribute table must be edited and a unique ID value designated for each segment of the baseline. It is recommended to organize baseline segment IDs in order alongshore. DSAS will not cast transects along baseline segments where the ID value is zero.", MsgBoxStyle.ApplicationModal, "Baseline ID Field")
    End Sub

    Private Sub btnHelpXectRelativeToBl_Click(sender As Object, e As EventArgs) Handles btnHelpXectRelativeToBl.Click
        MsgBox("DSAS v5.0 supports a baseline located anywhere (offshore, onshore, or in the middle of the shoreline data). As a result of this DSAS will search by default from either side of the baseline for shoreline data and extend transects to a length that intersects all shoreline data within range. If, however, the baseline is known to be entirely on one side of the data, (onshore or offshore), the user may select from the options below and DSAS will only search on one side of the baseline.", MsgBoxStyle.ApplicationModal, "Transect Casting")
    End Sub
End Class
