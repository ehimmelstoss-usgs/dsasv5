Imports ESRI.ArcGIS.Carto


Public Class TransectLyrToolCtl
    Inherits ESRI.ArcGIS.Desktop.AddIns.ComboBox


    Private Sub m_pApplication_Initialized()
        MyBase.Enabled = DSAS.enabled
    End Sub


    Public Sub New()
        Enabled = My.ArcMap.Application IsNot Nothing
        If Enabled Then DSAS.TransectLyrToolInstance = Me
        log("Wiring 'application initialized' event handler")
        Dim m_pApplication As ESRI.ArcGIS.Framework.IApplicationStatusEvents_Event = My.ThisApplication
        AddHandler m_pApplication.Initialized, AddressOf m_pApplication_Initialized
    End Sub


    Public Shared ReadOnly Property currentTransectLayer As IFeatureLayer
        Get
            If DSAS.TransectLyrToolInstance.Selected > -1 Then
                Return DSAS.TransectLyrToolInstance.GetItem(DSAS.TransectLyrToolInstance.Selected).Tag
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public Shared ReadOnly Property currentTransectLayerName As String
        Get
            If currentTransectLayer Is Nothing Then
                Return Nothing
            Else
                Return currentTransectLayer.Name
            End If
        End Get
    End Property


    Private Function AddLayer(ByVal lyr As ILayer) As Integer
        Try
            RemoveLayer(lyr.Name)
            If TypeOf lyr Is IFeatureLayer Then
                If GeoDB.layerIsValid(lyr, "transect", True) Then
                    If GeoDB.geodbIsCurrent(lyr) Then
                        Return MyBase.Add(lyr.Name, lyr)
                    Else
                        log(TraceLevel.Error, "Transect layer " + lyr.Name + " resides in a geodatabase with a different version." + vbCrLf + "Please upgrade the geodatabase or copy the layer to a current geodatabase to use with DSAS.")
                    End If
                End If
            End If
        Finally
        End Try
        Return -1
    End Function

    Private Sub RemoveLayer(lyrName As String)
        For Each o As Item In MyBase.items
            If o.Caption = lyrName Then
                MyBase.Remove(o.Cookie)
            End If
        Next
    End Sub

    Public Sub InitializeLayers()
        SyncLock Me
            Dim selectedLayer As IFeatureLayer = Nothing
            If Me.Selected > -1 Then
                selectedLayer = Me.GetItem(Me.Selected).Tag
            End If

            MyBase.Clear()
            MyBase.Add("")
            Try
                For Each lyr As IFeatureLayer In MapUtility.featureLayers("transect")
                    Dim cookie As Integer = Me.AddLayer(lyr)
                    If lyr Is selectedLayer Then
                        Me.Select(cookie)
                    End If
                Next
            Finally
            End Try
        End SyncLock
    End Sub


    Protected Overrides Sub OnSelChange(ByVal cookie As Integer)
        Try
            If currentTransectLayer IsNot Nothing Then
                'The following ensures that the transect table mapping is populated with field indexes of selected transect layer
                GeoDB.layerIsValid(currentTransectLayer, "transect")
            End If
        Catch ex As Exception
        End Try
    End Sub


    Public Shared Sub addSelectLayer(newLyr As IFeatureLayer)
        Dim cookie = DSAS.TransectLyrToolInstance.AddLayer(newLyr)
        If cookie > -1 Then
            DSAS.TransectLyrToolInstance.Select(cookie)
        End If
    End Sub


End Class
