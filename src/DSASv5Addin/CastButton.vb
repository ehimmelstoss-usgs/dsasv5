Public Class CastButton
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Enabled = DSAS.enabled
    End Sub

    Protected Overrides Sub OnClick()
        Try
            Dim frm As New CastTransectsForm
            'set the status bar
            If DSAS.statusBar Is Nothing Then
                DSAS.statusBar = My.ThisApplication.StatusBar
            End If

            'if there are less than two layers in the map then exit
            If My.Document.FocusMap.LayerCount < 2 Then
                DSASUtility.log(TraceLevel.Error, "Less than two layers in map, cannot procced!")
                Exit Sub
            End If

            ' check for metadata settings
            If Not checkForMetadataSetting() Then
                Exit Sub
            End If

            frm.ShowDialog()
        Catch ex As System.Exception
            DSASUtility.handleException(ex)
            Exit Try
        End Try
    End Sub

    Protected Overrides Sub OnUpdate()
        Enabled = DSAS.enabled
    End Sub
End Class
