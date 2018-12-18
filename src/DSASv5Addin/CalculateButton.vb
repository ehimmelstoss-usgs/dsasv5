Public Class CalculateButton
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Enabled = DSAS.enabled
    End Sub

    Protected Overrides Sub OnClick()
        Try
            'if there is less than 3 layers in the map exit sub
            If My.Document.FocusMap.LayerCount < 3 Then
                DSASUtility.log(TraceLevel.Error, "Less than 3 layers in map, cannot proceed!")
                Exit Sub
            End If
            If TransectLyrToolCtl.currentTransectLayerName Is Nothing Then
                DSASUtility.log(TraceLevel.Error, "You must select a transect layer first to calculate rates!")
                Exit Sub
            End If

            If Not isValidTransectName(TransectLyrToolCtl.currentTransectLayerName, True) Then
                Return
            End If

            If Topology.isInEditSession Then
                DSASUtility.log(TraceLevel.Error, "You must close your edit session first to calculate rates!")
                Exit Sub
            End If
            DSAS.calcForm = New CalcStatsForm
            DSAS.calcForm.ShowDialog()
            DSAS.calcForm = Nothing
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
            Exit Try
        End Try
    End Sub

    Protected Overrides Sub OnUpdate()
        Enabled = DSAS.enabled
    End Sub
End Class
