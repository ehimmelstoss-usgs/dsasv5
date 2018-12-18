Option Explicit On

Public Class Stat
    Public Shared stats As New Dictionary(Of String, Stat)

    Private name As String
    Public parent As String
    Private displayName As String
    Private type As String
    Private category As String


    Public Sub New(fromDict As IronPython.Runtime.PythonDictionary)
        name = fromDict.get("name")
        parent = fromDict.get("parent")
        type = fromDict.get("type")
        category = fromDict.get("category", Nothing)
        displayName = String.Format("[{0}] {1}: {2}", category, name, fromDict.get("alias"))
    End Sub


    Public ReadOnly Property valueMember As String
        Get
            Return name
        End Get
    End Property


    Public ReadOnly Property displayMember As String
        Get
            Return displayName
        End Get
    End Property


    Public ReadOnly Property asFieldName As String
        Get
            Return name.Replace("{CI}", CI.getCIFor(My.Settings.Confidence_Interval).fieldNameCompatible)
        End Get
    End Property


    Public Shared Sub init()
        If stats.Count = 0 Then
            For Each dcf As IronPython.Runtime.PythonDictionary In ipy.getRateList()
                Dim aStat As New Stat(dcf)
                stats.Add(aStat.name.Replace("{CI}", ""), aStat)
            Next
        End If
    End Sub

    Public Shared Function parentStats() As List(Of Stat)
        parentStats = New List(Of Stat)
        For Each aStat As Stat In stats.Values
            If aStat.category IsNot Nothing Then
                parentStats.Add(aStat)
            End If
        Next
    End Function

End Class
