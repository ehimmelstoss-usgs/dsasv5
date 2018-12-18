Public NotInheritable Class SplashScreen

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build, My.Application.Info.Version.Revision)
        Version.Text = DSASAddin.My.ThisAddIn.Version

        tbCitation.Select(9999, 0)
        llCitation.Focus()
    End Sub

    Private Sub llCitation_LinkClicked(sender As Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llCitation.LinkClicked
        System.Diagnostics.Process.Start(CType(sender, System.Windows.Forms.LinkLabel).Text)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class
