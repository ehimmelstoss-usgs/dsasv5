Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports System.IO

Public Class DSASExtension
    Inherits ESRI.ArcGIS.Desktop.AddIns.Extension

    ' Following variable is not used anywhere but document events are lost if deleted
    Private mxDocument As IMxDocument = My.Document

    Protected Overrides Sub OnStartup()
        'Initialize the DSAS singleton upon startup
        Dim initDSAS As DSAS = DSAS.Instance

        ' Log DSAS version info
        logIntro()

        Dim m_documentEvents As IDocumentEvents_Event = DirectCast(My.Document, IDocumentEvents_Event)
        AddHandler m_documentEvents.NewDocument, AddressOf wireDocumentEvents
        AddHandler m_documentEvents.OpenDocument, AddressOf wireDocumentEvents
        AddHandler m_documentEvents.MapsChanged, AddressOf wireDocumentEvents

        wireDocumentEvents()

        AddHandler(CType(My.Settings, System.Configuration.ApplicationSettingsBase).SettingChanging), AddressOf settingChanging

    End Sub


    Private Sub InitializeLayers(item As Object)
        If DSAS.TransectLyrToolInstance IsNot Nothing Then
            DSAS.TransectLyrToolInstance.InitializeLayers()
        End If
    End Sub

    ''' <summary>
    ''' Watches for deletion from the map and clears out any setting that point to the deleted object
    ''' </summary>
    ''' <param name="item"></param>
    Private Sub settingsSentinel(item As Object)
        Try
            Dim itemName As String = ""

            If TryCast(item, IDataset) IsNot Nothing Then
                itemName = TryCast(item, IDataset).Name.ToLower
            ElseIf TryCast(item, IStandaloneTable) IsNot Nothing Then
                itemName = TryCast(item, IStandaloneTable).Name.ToLower
            End If

            If itemName = "" Then Return

            For Each prop As Configuration.SettingsProperty In My.Settings.Properties
                If prop.Name.ToLower.Contains("table") OrElse prop.Name.ToLower.Contains("layer") Then
                    If nv(My.Settings(prop.Name)).ToLower = itemName Then
                        ' Clear out the setting if it points to the layer or table deleted from the map
                        My.Settings(prop.Name) = ""
                        ' Reset shoreline type field selection if selected shoreline uncertainty table was deleted from map
                        If prop.Name = "Shoreline_Uncertainty_Table" Then My.Settings.Shoreline_Type_Field = ""
                    End If
                End If
            Next
        Catch ex As Exception
            handleException(ex)
        End Try
    End Sub


    Private Sub wireMapEvents(unwireFirst As Boolean, thenWire As Boolean)
        For m As Integer = 0 To My.Document.Maps.Count - 1
            Dim map As IActiveViewEvents_Event = My.Document.Maps.Item(m)
            Dim m_activeViewEvents As IActiveViewEvents_Event = DirectCast(map, IActiveViewEvents_Event)

            If unwireFirst Then
                RemoveHandler m_activeViewEvents.ItemAdded, AddressOf InitializeLayers
                RemoveHandler m_activeViewEvents.ItemDeleted, AddressOf InitializeLayers
                RemoveHandler m_activeViewEvents.ItemDeleted, AddressOf settingsSentinel
                log("Unwired 'item added/deleted' event handlers for data frame: " + My.Document.Maps.Item(m).Name)
            End If

            If thenWire Then
                AddHandler m_activeViewEvents.ItemAdded, AddressOf InitializeLayers
                AddHandler m_activeViewEvents.ItemDeleted, AddressOf InitializeLayers
                AddHandler m_activeViewEvents.ItemDeleted, AddressOf settingsSentinel
                log("Wired 'item added/deleted' event handlers for data frame: " + My.Document.Maps.Item(m).Name)
            End If
        Next
    End Sub


    Private Sub wireDocumentEvents()
        InitializeLayers(Nothing)

        wireMapEvents(True, True)

        ' If somehow we picked up a bad combo of no uncertainty tables but a non-empty shoreline type field selection then repair settings
        If MapUtility.uncertaintyTableCount = 0 AndAlso nv(My.Settings.Shoreline_Type_Field) <> "" Then
            My.Settings.Shoreline_Type_Field = ""
        End If

    End Sub

    Protected Overloads Overrides Sub OnLoad(ByVal inStrm As Stream)
        log("Loading data from ArcMap project using PersistenceHelper")
        Dim userSettings As UserSettings = New UserSettings
        PersistenceHelper.Load(inStrm, userSettings)
    End Sub

    Protected Overloads Overrides Sub OnSave(ByVal outStrm As Stream)
        log("Saving data from ArcMap project using PersistenceHelper")
        Dim userSettings As UserSettings = New UserSettings
        PersistenceHelper.Save(outStrm, userSettings)

        ' As a DLL, setings need to be saved explicitly
        My.Settings.Save()
    End Sub


    Protected Overrides Sub OnShutdown()
        wireMapEvents(True, False)

        Dim m_documentEvents As IDocumentEvents_Event = DirectCast(My.Document, IDocumentEvents_Event)
        RemoveHandler m_documentEvents.NewDocument, AddressOf wireDocumentEvents
        RemoveHandler m_documentEvents.OpenDocument, AddressOf wireDocumentEvents
        RemoveHandler m_documentEvents.MapsChanged, AddressOf wireDocumentEvents

        DSAS.TransectLyrToolInstance = Nothing

        RemoveHandler(CType(My.Settings, System.Configuration.ApplicationSettingsBase).SettingChanging), AddressOf settingChanging
    End Sub

    Private Shared Sub settingChanging(sender As Object, e As System.Configuration.SettingChangingEventArgs)
        If e.NewValue <> My.Settings(e.SettingName) Then
            ' Tell ArcMap that the document has changed so the project settings are written to disk (unless the user cancels)
            Dim doc As IDocumentDirty2 = My.Document
            doc.SetDirty()
        End If
        If e.SettingName = "Shoreline_Feature_Layer" Then
            Try
                Dim lyr As IFeatureLayer = MapUtility.findFeatureLayer(e.NewValue, "shoreline")
                If lyr Is Nothing Then
                    DSAS.shoreFc = Nothing
                Else
                    DSAS.shoreFc = lyr.FeatureClass
                End If
            Catch ex As Exception
                log("Error while setting shoreline featureclass")
            End Try
        End If
    End Sub

End Class
