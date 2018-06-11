Public NotInheritable Class SplashScreen

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build, My.Application.Info.Version.Revision)
        Version.Text = DSASAddin.My.ThisAddIn.Version
    End Sub

    Private Sub SplashScreen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Click, Version.Click, Me.Click
        Me.Close()
    End Sub

End Class
