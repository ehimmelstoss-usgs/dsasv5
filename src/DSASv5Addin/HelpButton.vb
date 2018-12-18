Public Class HelpButton
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Enabled = DSAS.enabled
    End Sub

    Protected Overrides Sub OnClick()
        Try
            Dim frm As New HelpForm
            frm.ShowDialog()

        Catch ex As Exception
            'Gracefully fail if we can't get help.
        End Try
    End Sub

    Protected Overrides Sub OnUpdate()
        Enabled = DSAS.enabled
    End Sub

End Class
