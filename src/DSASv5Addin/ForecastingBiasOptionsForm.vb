Imports System.Windows.Forms

Public Class ForecastingBiasOptionsForm
    Public result As String = ""

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click, btnContinueWithAvgBias.Click, btnSkipNoBiasTransects.Click
        result = sender.Tag
        Me.Close()
    End Sub

    Private Sub ForecastingBiasOptionsForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If result = "" Then result = Me.btnStop.Tag
    End Sub

End Class