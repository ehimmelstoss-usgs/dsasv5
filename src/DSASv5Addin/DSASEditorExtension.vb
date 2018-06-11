Imports ESRI.ArcGIS.Editor

'''<summary>
'''DSASEditorExtension class implementing custom ESRI Editor Extension functionalities.
'''</summary>
Public Class DSASEditorExtension
    Inherits ESRI.ArcGIS.Desktop.AddIns.Extension

    Public Sub New()

    End Sub

    Protected Overrides Sub OnStartup()
        Dim theEditor As IEditor = My.ArcMap.Editor
    End Sub

    Protected Overrides Sub OnShutdown()

    End Sub

#Region "Editor Events"
    Private m_editorEvent As IEditEvents_Event = TryCast(My.ArcMap.Editor, IEditEvents_Event)
#Region "Shortcut properties to the various editor event interfaces"
    Private ReadOnly Property Events() As IEditEvents_Event
        Get
            Return TryCast(m_editorEvent, IEditEvents_Event)
        End Get
    End Property

    Private ReadOnly Property Events2() As IEditEvents2_Event
        Get
            Return TryCast(m_editorEvent, IEditEvents2_Event)
        End Get
    End Property

    Private ReadOnly Property Events3() As IEditEvents3_Event
        Get
            Return TryCast(m_editorEvent, IEditEvents3_Event)
        End Get
    End Property

    Private ReadOnly Property Events4() As IEditEvents4_Event
        Get
            Return TryCast(m_editorEvent, IEditEvents4_Event)
        End Get
    End Property
#End Region

    Sub WireEditorEvents()
        '
        ' TODO: Sample code demonstrating editor event wiring
        '
        AddHandler Events.OnCurrentTaskChanged, AddressOf OnCurrentTaskChangedEvent
    End Sub

    Sub OnCurrentTaskChangedEvent()
        If My.ArcMap.Editor.CurrentTask IsNot Nothing Then
            Debug.WriteLine(My.ArcMap.Editor.CurrentTask.Name)
        End If
    End Sub
#End Region
End Class
