'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports System
Imports System.Collections.Generic

Namespace My
    
    '''<summary>
    '''A class for looking up declarative information in the associated configuration xml file (.esriaddinx).
    '''</summary>
    Friend Module ThisAddIn
        
        Friend ReadOnly Property Name() As String
            Get
                Return "DSAS v5.0 add-in"
            End Get
        End Property
        
        Friend ReadOnly Property AddInID() As String
            Get
                Return "{a2b17571-2e57-4082-aba8-3f1ea3129e38}"
            End Get
        End Property
        
        Friend ReadOnly Property Company() As String
            Get
                Return "Corona Environmental Consulting"
            End Get
        End Property
        
        Friend ReadOnly Property Version() As String
            Get
                Return "5.0.20180213.1225"
            End Get
        End Property
        
        Friend ReadOnly Property Description() As String
            Get
                Return "Digital Shoreline Analysis System"
            End Get
        End Property
        
        Friend ReadOnly Property Author() As String
            Get
                Return "Ayhan Ergul"
            End Get
        End Property
        
        Friend ReadOnly Property [Date]() As String
            Get
                Return "2/13/2018"
            End Get
        End Property
        
        <System.Runtime.CompilerServices.ExtensionAttribute()>  _
        Friend Function ToUID(ByVal id As String) As ESRI.ArcGIS.esriSystem.UID
            Dim uid As ESRI.ArcGIS.esriSystem.UID = New ESRI.ArcGIS.esriSystem.UIDClass()
            uid.Value = id
            Return uid
        End Function
        
        '''<summary>
        '''A class for looking up Add-in id strings declared in the associated configuration xml file (.esriaddinx).
        '''</summary>
        Friend Class IDs
            
            '''<summary>
            '''Returns 'Amy_Button', the id declared for Add-in Button class 'Amy_Button'
            '''</summary>
            Friend Shared ReadOnly Property Amy_Button() As String
                Get
                    Return "Amy_Button"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'DSASAddin_AttributeToolbarButton', the id declared for Add-in Button class 'AttributeToolbarButton'
            '''</summary>
            Friend Shared ReadOnly Property AttributeToolbarButton() As String
                Get
                    Return "DSASAddin_AttributeToolbarButton"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'DSASAddin_CastButton', the id declared for Add-in Button class 'CastButton'
            '''</summary>
            Friend Shared ReadOnly Property CastButton() As String
                Get
                    Return "DSASAddin_CastButton"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'DSASAddin_DefaultsButton', the id declared for Add-in Button class 'DefaultsButton'
            '''</summary>
            Friend Shared ReadOnly Property DefaultsButton() As String
                Get
                    Return "DSASAddin_DefaultsButton"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'DSASAddin_CalculateButton', the id declared for Add-in Button class 'CalculateButton'
            '''</summary>
            Friend Shared ReadOnly Property CalculateButton() As String
                Get
                    Return "DSASAddin_CalculateButton"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'DSASAddin_AboutButton', the id declared for Add-in Button class 'AboutButton'
            '''</summary>
            Friend Shared ReadOnly Property AboutButton() As String
                Get
                    Return "DSASAddin_AboutButton"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'DSASAddin_HelpButton', the id declared for Add-in Button class 'HelpButton'
            '''</summary>
            Friend Shared ReadOnly Property HelpButton() As String
                Get
                    Return "DSASAddin_HelpButton"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'DSASAddin_ClipButton', the id declared for Add-in Button class 'ClipButton'
            '''</summary>
            Friend Shared ReadOnly Property ClipButton() As String
                Get
                    Return "DSASAddin_ClipButton"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'DSASAddin_ShorelineForecastButton', the id declared for Add-in Button class 'ShorelineForecastButton'
            '''</summary>
            Friend Shared ReadOnly Property ShorelineForecastButton() As String
                Get
                    Return "DSASAddin_ShorelineForecastButton"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'DSASAddin_TransectLyrToolCtl', the id declared for Add-in ComboBox class 'TransectLyrToolCtl'
            '''</summary>
            Friend Shared ReadOnly Property TransectLyrToolCtl() As String
                Get
                    Return "DSASAddin_TransectLyrToolCtl"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'DSASAddin_DSASExtension', the id declared for Add-in Extension class 'DSASExtension'
            '''</summary>
            Friend Shared ReadOnly Property DSASExtension() As String
                Get
                    Return "DSASAddin_DSASExtension"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'DSASAddin_DSASEditorExtension', the id declared for Add-in Extension class 'TransectAutoFix'
            '''</summary>
            Friend Shared ReadOnly Property TransectAutoFix() As String
                Get
                    Return "DSASAddin_DSASEditorExtension"
                End Get
            End Property
        End Class
    End Module
    
Friend Module ArcMap
  Private s_app As ESRI.ArcGIS.Framework.IApplication
  Private s_docEvent As ESRI.ArcGIS.ArcMapUI.IDocumentEvents_Event

  Public ReadOnly Property Application() As ESRI.ArcGIS.Framework.IApplication
    Get
      If s_app Is Nothing Then
        s_app = TryCast(Internal.AddInStartupObject.GetHook(Of ESRI.ArcGIS.ArcMapUI.IMxApplication)(), ESRI.ArcGIS.Framework.IApplication)
        If s_app Is Nothing Then
          Dim editorHost As ESRI.ArcGIS.Editor.IEditor = Internal.AddInStartupObject.GetHook(Of ESRI.ArcGIS.Editor.IEditor)()
          If editorHost IsNot Nothing Then s_app = editorHost.Parent
        End If
      End If

      Return s_app
    End Get
  End Property

  Public ReadOnly Property Document() As ESRI.ArcGIS.ArcMapUI.IMxDocument
    Get
      If Application IsNot Nothing Then
        Return TryCast(Application.Document, ESRI.ArcGIS.ArcMapUI.IMxDocument)
      End If

      Return Nothing
    End Get
  End Property

  Public ReadOnly Property ThisApplication() As ESRI.ArcGIS.ArcMapUI.IMxApplication
    Get
      Return TryCast(Application, ESRI.ArcGIS.ArcMapUI.IMxApplication)
    End Get
  End Property
  Public ReadOnly Property DockableWindowManager() As ESRI.ArcGIS.Framework.IDockableWindowManager
    Get
      Return TryCast(Application, ESRI.ArcGIS.Framework.IDockableWindowManager)
    End Get
  End Property

  Public ReadOnly Property Events() As ESRI.ArcGIS.ArcMapUI.IDocumentEvents_Event
    Get
      s_docEvent = TryCast(Document, ESRI.ArcGIS.ArcMapUI.IDocumentEvents_Event)
      Return s_docEvent
    End Get
  End Property

  Public ReadOnly Property Editor() As ESRI.ArcGIS.Editor.IEditor
    Get
      Dim editorUID As New ESRI.ArcGIS.esriSystem.UID
      editorUID.Value = "esriEditor.Editor"
      Return TryCast(Application.FindExtensionByCLSID(editorUID), ESRI.ArcGIS.Editor.IEditor)
    End Get
  End Property
End Module

Namespace Internal
  <ESRI.ArcGIS.Desktop.AddIns.StartupObjectAttribute(), _
   Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), _
   Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()> _
  Partial Public Class AddInStartupObject
    Inherits ESRI.ArcGIS.Desktop.AddIns.AddInEntryPoint

    Private m_addinHooks As List(Of Object)
    Private Shared _sAddInHostManager As AddInStartupObject

    Public Sub New()

    End Sub

    <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Protected Overrides Function Initialize(ByVal hook As Object) As Boolean
      Dim createSingleton As Boolean = _sAddInHostManager Is Nothing
      If createSingleton Then
        _sAddInHostManager = Me
        m_addinHooks = New List(Of Object)
        m_addinHooks.Add(hook)
      ElseIf Not _sAddInHostManager.m_addinHooks.Contains(hook) Then
        _sAddInHostManager.m_addinHooks.Add(hook)
      End If

      Return createSingleton
    End Function

    <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Protected Overrides Sub Shutdown()
      _sAddInHostManager = Nothing
      m_addinHooks = Nothing
    End Sub

    <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Friend Shared Function GetHook(Of T As Class)() As T
      If _sAddInHostManager IsNot Nothing Then
        For Each o As Object In _sAddInHostManager.m_addinHooks
          If TypeOf o Is T Then
            Return DirectCast(o, T)
          End If
        Next
      End If

      Return Nothing
    End Function

    ''' <summary>
    ''' Expose this instance of Add-in class externally
    ''' </summary>
    Public Shared Function GetThis() As AddInStartupObject
      Return _sAddInHostManager
    End Function

  End Class
End Namespace

End Namespace
