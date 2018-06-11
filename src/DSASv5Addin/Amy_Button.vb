Public Class Amy_Button
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        'transectConvertFromV4ToV5("AE03", "offshore")

        'Dim a = getSummary()
        'Dim b = Newtonsoft.Json.JsonConvert.SerializeObject(a, Newtonsoft.Json.Formatting.Indented)
        'If b.Length > 999 Then
        '    b = Left(b, 999) + "..." + vbCrLf + " [truncated]"
        'End If
        'MsgBox(b)

        'MsgBox(runCalcs().ToString)
    End Sub

    Protected Overrides Sub OnUpdate()
        Enabled = My.ArcMap.Application IsNot Nothing
    End Sub
End Class
