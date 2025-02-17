Public Class DoNotShowAgainForm
    Dim mySettingName As String

    Shared Sub prompt(ByVal message As String, ByVal settingName As String)
        ' Do nothing if disabled
        If My.Settings(settingName) Then Exit Sub
        ' Otherwise, show the dialog with given message
        Dim frm As New DoNotShowAgainForm
        frm.txtMessage.Text = message
        frm.txtMessage.Select(0, 0)
        frm.mySettingName = settingName
        frm.txtMessage.Size = New Drawing.Size(frm.btnOK.Location.X + frm.btnOK.Size.Width - frm.txtMessage.Location.X, frm.chkDoNotShowAgain.Location.Y - frm.txtMessage.Location.Y - frm.chkDoNotShowAgain.Height)
        frm.ShowDialog()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Me.chkDoNotShowAgain.Checked Then
            My.Settings(Me.mySettingName) = True
        End If

        Me.Close()
    End Sub
End Class