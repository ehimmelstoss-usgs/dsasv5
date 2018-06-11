Imports System.IO
Imports ESRI.ArcGIS.ADF.Serialization

Public Class DSASMapDataExtension
    Inherits ESRI.ArcGIS.Desktop.AddIns.Extension
    Private _data As MyPersistentData

    <Serializable()>
    Private Structure MyPersistentData
        Public SuppressNonPgdbDataWarning As Boolean
    End Structure


    Protected Overloads Overrides Sub OnSave(ByVal outStrm As Stream)
        'Get called when saving document. 
        _data = New MyPersistentData()
        _data.SuppressNonPgdbDataWarning = False

        PersistenceHelper.Save(Of MyPersistentData)(outStrm, _data)
    End Sub

    Protected Overloads Overrides Sub OnLoad(ByVal inStrm As Stream)
        'Get called when opening a document with persisted stream. 
        PersistenceHelper.Load(Of MyPersistentData)(inStrm, _data)
    End Sub
End Class
