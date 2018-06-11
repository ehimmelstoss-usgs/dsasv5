Imports System.Data.OleDb
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Carto

Public Class SCEClipForm

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        initComboboxes()
        setColorRampButtonsDisability()

    End Sub

    Private Sub initComboboxes()
        initCombobox(Me.cbRates, "rates", True)
        initCombobox(Me.cbRatesColorRamp, "rates")
    End Sub

    Private Sub initCombobox(ByVal cb As ComboBox, ByVal layerType As String, Optional checkForIntersectLayer As Boolean = False)
        cb.Items.Add("")
        For Each lyr As IFeatureLayer In MapUtility.featureLayers()
            If GeoDB.layerIsValid(lyr, layerType, True) Then
                If GeoDB.geodbIsCurrent(lyr) Then
                    If Not checkForIntersectLayer OrElse GeoDB.checkIfTableFieldExists(lyr.Name.Replace("_rates_", "_intersect_"), Nothing) Then
                        cb.Items.Add(lyr.Name)
                    End If
                End If
            End If
        Next
        cb.Refresh()

        cb.SelectedItem = ""
    End Sub

    Private Sub btnClip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClip.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim rateName As String = Me.cbRates.Text
            Dim intersectName As String = rateName.Replace("_rates_", "_intersect_")   'TODO: Check that the fc exists
            Dim xectLyr As IFeatureLayer = MapUtility.findFeatureLayer(rateName, "rates")
            Dim xectDataset As IDataset = xectLyr
            Dim mdb As String = xectDataset.Workspace.PathName
            'Dim mdb As String = "C:\WORK\DSAS\DSAS_work\20091002_near_far_issue\sample_near_far.mdb"
            Dim clippedFcName As String = GeoDB.copyXectFc(mdb, rateName)
            If clippedFcName IsNot Nothing AndAlso clippedFcName.Trim() <> "" Then
                Dim pWorkspaceFactory As IWorkspaceFactory = DirectCast(getSingleton("esriDataSourcesGDB.AccessWorkspaceFactory"), IWorkspaceFactory)
                Dim pWorkspace As IWorkspace = pWorkspaceFactory.OpenFromFile(mdb, 0)

                Dim fcClipped As IFeatureClass = GeoDB.findDatasetInWorkspace(clippedFcName, pWorkspace)
                clipAll(fcClipped, intersectName)
                ' Is there a field we can delete to differentiate a clipped rate layer from a rate layer?
                'fcClipped.DeleteField(fcClipped.Fields().Field(GeoDB.transFldMap("TransEdit")))
                ' This will trigger transect layer detection...
                MapUtility.AddFeatureClassToMap(fcClipped)
                ' Make the new layer available for color ramp
                cbRatesColorRamp.Items.Insert(1, clippedFcName)
                MsgBox("SCE clipped rates layer '" + clippedFcName + "' has been created and added to your map.")
            Else
                log(TraceLevel.Error, "Unable to create SCE clipped rates layer.")
            End If
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub



    Private Sub clipAll(ByVal clippedFc As IFeatureClass, ByVal intersectFcName As String)
        Dim xectFC As IFeatureCursor = GeoDB.getCursorForLayer(clippedFc, clippedFc.OIDFieldName, , True)
        Dim xect As IFeature
        Do
            xect = xectFC.NextFeature
            If xect Is Nothing Then Exit Do
            clipXect(xect, intersectFcName)
        Loop
        xectFC.Flush()
        ReleaseComObject(xect)
        ReleaseComObject(xectFC)
    End Sub


    Private Sub clipXect(ByVal xect As IFeature, ByVal intersectFcName As String)
        Dim cn As OleDbConnection = Nothing
        Dim sql As String = ""
        Dim cmd As OleDbCommand
        Dim dr As OleDbDataReader = Nothing

        Try
            sql = "select TransectId, IntersectX, IntersectY from [" + intersectFcName + "] WHERE TransectId = " + Str(xect.OID) + " ORDER BY ABS(Distance)"
            cn = MapUtility.dbConnectionForLayer(intersectFcName)
            cmd = New OleDbCommand(sql, cn)
            dr = cmd.ExecuteReader

            Dim startX As Double = -1
            Dim startY As Double = -1
            Dim endX As Double = -1
            Dim endY As Double = -1

            Dim first As Boolean = True
            While dr.Read()
                endX = dr("IntersectX")
                endY = dr("IntersectY")
                If first Then
                    startX = endX
                    startY = endY
                End If
                first = False
            End While

            If startX = -1 Then
                xect.Shape = New Polyline()
            Else
                Dim p As New Point
                p.X = startX
                p.Y = startY
                Dim q As New Point
                q.X = endX
                q.Y = endY

                Dim polyline As IPolyline = New PolylineClass()
                Dim pointCollection As IPointCollection = TryCast(polyline, IPointCollection)
                pointCollection.AddPoint(p)
                pointCollection.AddPoint(q)
                xect.Shape = TryCast(pointCollection, IPolyline)
            End If

            xect.Store()
        Catch
            Dim zz As Integer = 1
        Finally
            If dr IsNot Nothing Then dr.Close()
            If cn IsNot Nothing Then cn.Close()
        End Try
    End Sub

    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
        Me.Close()
    End Sub

    Private Sub updateClipButton()
        btnClip.Enabled = cbRates.Text.Trim <> ""
    End Sub

    Private Sub cbXect_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbRates.SelectedIndexChanged
        updateClipButton()
    End Sub


    Private Sub btnApplyColorRamp_Click(sender As Object, e As EventArgs) Handles btnApplyColorRamp.MouseClick
        MapUtility.setRateColorRamp(MapUtility.findFeatureLayer(cbRatesColorRamp.SelectedItem), rateField.SelectedItem.ToString, False)
    End Sub


    Private Function isRateFieldsValid() As Boolean

        If cbRatesColorRamp.SelectedItem IsNot Nothing And rateField.SelectedItem IsNot Nothing Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub setColorRampButtonsDisability()
        btnApplyColorRamp.Enabled = isRateFieldsValid() AndAlso Not checkForStat(rateField.SelectedItem, "SCE") AndAlso Not checkForStat(rateField.SelectedItem, "NSM")
        btnScaleToMyData.Enabled = isRateFieldsValid()
    End Sub

    Private Sub cbRatesColorRamp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbRatesColorRamp.SelectedIndexChanged

        Dim featLyr = MapUtility.findFeatureLayer(cbRatesColorRamp.SelectedItem)

        If featLyr Is Nothing Then
            Exit Sub
        End If

        rateField.Items.Clear()
        For i As Integer = 0 To featLyr.FeatureClass.Fields.FieldCount - 1
            Dim fldName As String = featLyr.FeatureClass.Fields.Field(i).Name.ToUpper
            If GeoDB.isEsriDoubleField(featLyr.FeatureClass, fldName) Then
                If checkForStat(fldName, "SCE") OrElse
                   checkForStat(fldName, "NSM") OrElse
                    checkForStat(fldName, "EPR") OrElse
                    checkForStat(fldName, "LRR") OrElse
                    checkForStat(fldName, "WLR") Then
                    rateField.Items.Add(fldName)
                End If
            End If
        Next

        setColorRampButtonsDisability()

    End Sub


    Private Sub btnScaleToMyData_Click(sender As Object, e As EventArgs) Handles btnScaleToMyData.Click
        MapUtility.setRateColorRamp(MapUtility.findFeatureLayer(cbRatesColorRamp.SelectedItem), rateField.SelectedItem.ToString, True)
    End Sub


    Private Sub rateField_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rateField.SelectedIndexChanged
        setColorRampButtonsDisability()
    End Sub

    Private Sub SCEClipForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class