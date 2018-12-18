Public Class HelpForm
    Private Sub llProjectPage_LinkClicked(sender As Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llProjectPage.LinkClicked
        Process.Start("https://woodshole.er.usgs.gov/project-pages/DSAS/")
    End Sub

    Private Sub llUserGuide_LinkClicked(sender As Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llUserGuide.LinkClicked
        Dim helpStr As String = """" & DSASUtility.getAppFolder() & "\DSAS v5.0 User Guide.pdf"""
        log(helpStr)
        Process.Start(helpStr)
    End Sub

    Private Sub llReset_LinkClicked(sender As Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llReset.LinkClicked
        My.Settings.DoNotShowAgainCantInstantiateMetadataSynchronizer = False
        My.Settings.DoNotShowAgainForecastCaveat = False
        MsgBox("Reset completed",, DSAS.MsgBoxTitle)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub HelpForm_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        lblTop.Focus()
    End Sub
End Class