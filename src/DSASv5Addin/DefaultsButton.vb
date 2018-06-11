Public Class DefaultsButton
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button


    Public Sub New()
        Enabled = DSAS.enabled
    End Sub

    Protected Overrides Sub OnClick()
        Dim frm As New SetDefaultsForm
        'if there is less than two layers in the map then exit
        If My.Document.FocusMap.LayerCount < 2 Then
            DSASUtility.log(TraceLevel.Error, "Less than two layers in map, cannot procced!")
            Exit Sub
        End If

        frm.ShowDialog()
    End Sub

    Protected Overrides Sub OnUpdate()
        Enabled = DSAS.enabled
    End Sub

End Class
