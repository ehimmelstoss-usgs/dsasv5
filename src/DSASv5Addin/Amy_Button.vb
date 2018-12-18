Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry

Public Class Amy_Button
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        DSASUtility.layernameConvert("trans_meta_20180828_125321_forecast_20", "forecast", "forecast_uncy")
        DSASUtility.layernameConvert("trans_meta_20180828_125321_forecast_points", "forecast_points", "forecast_uncy")
        DSASUtility.layernameConvert("trans_meta_20180828_125321_forecast_points", "forecast_points", "transect")
        DSASUtility.layernameConvert("trans_meta_20180828_125321_forecast_20", "forecast", "rates")

        'GeoDB.layerIsValid(MapUtility.findFeatureLayer("FL_NE_baseline"), "transect")

        'discrepancy()

        'transectConvertFromV4ToV5("AE03", "offshore")

        'Dim a = getSummary()
        'Dim b = Newtonsoft.Json.JsonConvert.SerializeObject(a, Newtonsoft.Json.Formatting.Indented)
        'If b.Length > 999 Then
        '    b = Left(b, 999) + "..." + vbCrLf + " [truncated]"
        'End If
        'MsgBox(b)

        'MsgBox(runCalcs().ToString)
    End Sub

    Sub discrepancy()
        For Each bl As IFeature In GeoDB.features("BB_baseline")
            Dim bRightSide As Boolean
            Dim ht As IHitTest = DirectCast(bl.Shape, IHitTest)
            For Each pt As IFeature In GeoDB.features("BB_transects_REH_V5_intersect_20180818_012412")
                ht.HitTest(pt.Shape, 60, esriGeometryHitPartType.esriGeometryPartBoundary, Nothing, Nothing, Nothing, Nothing, bRightSide)
                log(String.Format("OID: {0}, Distance: {1:+0.##;-#.##}, rightside: {2}", pt.OID, pt.Value(pt.Fields.FindField("Distance")), bRightSide))
            Next
        Next
    End Sub

    Protected Overrides Sub OnUpdate()
        Enabled = My.ArcMap.Application IsNot Nothing
    End Sub
End Class
