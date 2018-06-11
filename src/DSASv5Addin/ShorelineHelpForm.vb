Public Class ShorelineHelpForm
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Close()
    End Sub

    Private Sub LinkLabelHazardsPortal_LinkClicked(sender As Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabelHazardsPortal.LinkClicked
        Process.Start("IEXPLORE.exe", "https://marine.usgs.gov/coastalchangehazardsportal/")
    End Sub
End Class