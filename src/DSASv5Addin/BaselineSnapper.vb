Imports System.Runtime.InteropServices
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Carto

<ComClass(BaselineSnapper.ClassId, BaselineSnapper.InterfaceId, BaselineSnapper.EventsId), _
 ProgId("Shoreline.BaselineSnapper")> _
Public Class BaselineSnapper
    Implements ISnapAgent
    Implements IPersistVariant
    Implements IExtension

    Private m_pEditor As IEditor
    Private Shared featCache As IFeatureCache = New FeatureCache


#Region "COM Registration Function(s)"
    <ComRegisterFunction(), ComVisibleAttribute(False)> _
    Public Shared Sub RegisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryRegistration(registerType)

        'Add any COM registration code after the ArcGISCategoryRegistration() call

    End Sub

    <ComUnregisterFunction(), ComVisibleAttribute(False)> _
    Public Shared Sub UnregisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryUnregistration(registerType)

        'Add any COM unregistration code after the ArcGISCategoryUnregistration() call

    End Sub

#Region "ArcGIS Component Category Registrar generated code"
    ''' <summary>
    ''' Required method for ArcGIS Component Category registration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryRegistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)

    End Sub
    ''' <summary>
    ''' Required method for ArcGIS Component Category unregistration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)

    End Sub

#End Region
#End Region


#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "6c943ff4-641d-4b10-81b7-cd116ae75c8e"
    Public Const InterfaceId As String = "5e1a7792-2720-492d-895a-e86d178ded93"
    Public Const EventsId As String = "98f95b32-7df6-4e98-9b53-afc331a9c119"
#End Region

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()
    End Sub


#Region "SnapAgentSpecificCode"
    Public ReadOnly Property Name() As String Implements ISnapAgent.Name
        Get
            Return "DSAS Baseline Snap Agent"
        End Get
    End Property


    Public Shared Function FindBaselineToSnapTo(ByVal feat As IFeature) As IFeature
        Dim bl As IFeature = Nothing
        Dim tolerance As Double = Topology.SnapTolerance
        Try
            Dim xect As IPolyline = feat.ShapeCopy
            Dim tt As IPointCollection = xect
            Dim point As IPoint = getXectBaselinePoint(xect)

            'GlobalVars.fMap = m_pEditor.Map

            ' We will snap only one feature
            'If GlobalVars.fMap.SelectionCount <> 1 Then Return False

            If Not featCache.Contains(point) Then
                Dim pFeatureClass As IFeatureClass = MapUtility.findFeatureLayer(My.Settings.Baseline_Feature_Layer, "baseline").FeatureClass
                ' Fill cache
                featCache.Initialize(point, tolerance * 10)
                featCache.AddFeatures(pFeatureClass)
            End If

            Dim dX As Double, dY As Double
            bl = New Feature

            If Not canSnap(featCache, point, dX, dY, bl) Then bl = Nothing
        Finally
        End Try

        Return bl
    End Function


    Public Function Snap(ByVal geom As IGeometry, ByVal point As IPoint, ByVal tolerance As Double) As Boolean Implements ISnapAgent.Snap
        Dim pFeatureClass As IFeatureClass
        Dim pSelection As ISelection

        'DSAS.Instance.fMap = m_pEditor.Map

        pSelection = My.Document.FocusMap.FeatureSelection

        ' We will snap only one feature
        If My.Document.FocusMap.SelectionCount <> 1 Then Return False

        pFeatureClass = MapUtility.findFeatureLayer(My.Settings.Baseline_Feature_Layer, "baseline").FeatureClass

        If Not featCache.Contains(point) Then
            ' Fill cache
            featCache.Initialize(point, tolerance * 10)
            featCache.AddFeatures(pFeatureClass)
        End If

        Dim dX As Double, dY As Double

        If canSnap(featCache, point, dX, dY) Then
            point.PutCoords(dX, dY)
            Return True
        End If

    End Function

    Shared Function canSnap(ByVal featCache As IFeatureCache, ByVal pt As IPoint, ByRef dX As Double, ByRef dY As Double, Optional ByRef closestFeature As IFeature = Nothing) As Boolean
        Dim pFeature As IFeature
        ' pHitPoint will be used in the For loop below.
        Dim pHitPoint As IPoint = New Point
        Dim pHitTest As IHitTest
        Dim lPartIndex As Long, lSegmentIndex As Long, bRightSide As Boolean
        Dim tolerance As Double = Topology.SnapTolerance
        Dim dMinDist As Double = tolerance * 10
        Dim dDist As Double
        Dim count As Integer
        ' Loop thru all of the features in the cache
        For count = 0 To featCache.Count - 1
            pFeature = featCache.Feature(count)

            pHitTest = pFeature.Shape
            If pHitTest.HitTest(pt, tolerance, esriGeometryHitPartType.esriGeometryPartBoundary, pHitPoint, dDist, lPartIndex, lSegmentIndex, bRightSide) Then
                If dDist <= tolerance Then
                    pHitPoint.QueryCoords(dX, dY)
                    dMinDist = dDist
                    If closestFeature IsNot Nothing Then closestFeature = pFeature
                End If
            End If
        Next count
        Return dMinDist <= tolerance
    End Function



#End Region

#Region "IPersistVariantSpecificCode"

    Public ReadOnly Property ID() As UID Implements IPersistVariant.ID
        Get
            log("Setting proerty for IPersistVariant")
            Dim pID As New UIDClass
            'pID.Value = "DSAS.BaselineSnapper"
            pID.Value = Me.GetType().GUID.ToString("B")
            Return pID
        End Get
    End Property

    Public Sub Load(ByVal Stream As IVariantStream) Implements IPersistVariant.Load
        log("Loading from baseline snapper")
    End Sub

    Public Sub Save(ByVal Stream As IVariantStream) Implements IPersistVariant.Save
        log("Saving from baseline snapper")
    End Sub
#End Region

#Region "IExtensionSpecificCode"

    Public ReadOnly Property Name1() As String Implements IExtension.Name
        Get
            Return "DSAS PersistVariant"
        End Get
    End Property

    Public Sub Shutdown() Implements IExtension.Shutdown
        m_pEditor = Nothing
    End Sub

    Public Sub Startup(ByRef initializationData As Object) Implements IExtension.Startup
        m_pEditor = DirectCast(initializationData, IEditor)
    End Sub
#End Region

End Class


