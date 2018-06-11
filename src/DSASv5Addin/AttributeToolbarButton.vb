Public Class AttributeToolbarButton
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Enabled = DSAS.enabled
    End Sub

    Protected Overrides Sub OnClick()
        Try
            Dim frm As New AttributeToolbarForm
            'set the status bar
            If DSAS.statusBar Is Nothing Then
                DSAS.statusBar = My.ThisApplication.StatusBar
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
