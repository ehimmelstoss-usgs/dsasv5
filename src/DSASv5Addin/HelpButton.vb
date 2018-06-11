Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Display

Public Class HelpButton
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Enabled = DSAS.enabled
    End Sub

    Protected Overrides Sub OnClick()
        HelpSeeker("/Help_Main.html")
    End Sub

    Protected Overrides Sub OnUpdate()
        Enabled = DSAS.enabled
    End Sub


    ''' <summary>
    ''' Keeps track of the HTML Help process (hh.exe) to avoid having more than one help window open.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared proc As New Process()

    Public Shared Sub HelpSeeker(ByVal name As String, Optional ByVal defaultHelp As String = "/Help_Main.html")
        Try
            proc.CloseMainWindow()
            proc.Refresh()
        Catch ex As Exception
        End Try
        Try
            'Read path to hh.exe from application settings
            'Dim HHPath As String = My.Settings.HHPath.Trim()
            Dim HHPath As String = ""
            'If nothing in application settings or the specified file does not exist...
            If HHPath = "" OrElse Not System.IO.File.Exists(HHPath) Then
                'then look for it in windows directory
                HHPath = Environ$("windir") & "\hh.exe"
                If Not System.IO.File.Exists(HHPath) Then
                    log(TraceLevel.Error, "Unable to find hh.exe required for displaying help. Please enter full pathname for it in '" & DSASUtility.getAppFolder() & "\app.config', e.g. 'C:\Windows\hh.exe'")
                    Exit Sub
                End If
            End If
            proc.StartInfo.FileName = HHPath

            Dim helpStr As String = """" & DSASUtility.getAppFolder() & "\DSAShelp.chm::"
            helpStr &= name
            helpStr &= """"
            log(helpStr)
            proc.StartInfo.Arguments = helpStr
            proc.Start()
        Catch ex As Exception
            'Gracefully fail if we can't get help.
            log(TraceLevel.Error, "Unable to show help for this element!")
        End Try

    End Sub

End Class
