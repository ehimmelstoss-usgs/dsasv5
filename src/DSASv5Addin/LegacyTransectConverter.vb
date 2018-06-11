Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry


Module LegacyTransectConverter

    Sub transectConvertFromV4ToV5(v4LyrName As String, baselineType As String)
        baselineType = baselineType.ToLower
        Debug.Assert(baselineType = "onshore" OrElse baselineType = "offshore")

        Dim stepPro As ESRI.ArcGIS.esriSystem.IStepProgressor = Nothing

        Dim v4Lyr As IFeatureLayer = MapUtility.findFeatureLayer(v4LyrName, "transectv4")
        If v4Lyr Is Nothing Then
            Return
        End If
        Dim v4fc As IFeatureCursor = GeoDB.getCursorForLayer(v4LyrName, "TransOrder")
        Dim v5fc As IFeatureClass = Nothing
        Dim v5LyrName As String = v4LyrName & "_V5"
        Dim v4GroupIdFldIdx As Integer = v4fc.FindField("Group")

        Try
            stepPro = MapUtility.InitStepProgressor(DSAS.statusBar, 1, GeoDB.getFldExtreme(v4Lyr.FeatureClass, "TransOrder", True), 1)

            Do
                Dim v4Feat As IFeature = v4fc.NextFeature()
                If v4Feat Is Nothing Then Return

                If v4Feat.Shape Is Nothing OrElse v4Feat.Shape.IsEmpty Then Continue Do
                Dim geom As IPointCollection = v4Feat.ShapeCopy
                If geom.PointCount > 0 AndAlso geom.PointCount <> 2 Then
                    log(TraceLevel.Error, String.Format("{0} does not look like a legacy transect layer! It will not be modified.", v4LyrName))
                    Return
                End If

                If v5fc Is Nothing Then
                    v5fc = GeoDB.CreateWorkspaceFeatureClassAndAddToTOC(v5LyrName, "transect", Nothing, v4LyrName)
                    If v5fc Is Nothing Then
                        log(TraceLevel.Error, String.Format("Unable to create target layer {0} for input layer {1}", v5LyrName, v4LyrName))
                        Return
                    End If
                    log(String.Format("Converting legacy transect layer {0}", v4LyrName))
                End If

                Dim transOrder As Integer = v4Feat.Value(v4Feat.Fields.FindField("TransOrder"))
                log(String.Format("Converting TransOrder = {0}", transOrder))
                Dim pt As IPoint
                If baselineType = "onshore" Then
                    Dim reversed As ICurve = geom
                    reversed.ReverseOrientation()
                    pt = clonePoint(geom.Point(1))
                    geom.AddPoint(pt)   ' Added at the end - at index 2
                ElseIf baselineType = "offshore" Then
                    pt = clonePoint(geom.Point(0))
                    geom.AddPoint(pt, 0) ' Addes at the beginning - at index 0
                End If

                clipToShorelineEnvelope(geom, getXectBaselinePoint(geom))

                Dim v5feat As IFeature = v5fc.CreateFeature
                With v5feat
                    .Shape = geom
                    .Value(v5fc.FindField("BaselineID")) = v4Feat.Value(v4Feat.Fields.FindField("BaselineID"))
                    If v4GroupIdFldIdx > -1 Then
                        .Value(v5fc.FindField("GroupID")) = v4Feat.Value(v4GroupIdFldIdx)
                    End If
                    .Value(v5fc.FindField("TransOrder")) = transOrder
                    Dim autogen As Object = v4Feat.Value(AutogenToTransEdit(v4Feat.Fields.FindField("Autogen")))
                    If autogen IsNot DBNull.Value Then
                        .Value(v5fc.FindField("TransEdit")) = AutogenToTransEdit(autogen)
                    End If
                    Dim ln As IPolyline = geom
                    .Value(v5fc.FindField("Azimuth")) = DSASUtility.round(DSASUtility.CalculateAzimuth(ln))
                    Try
                        .Store()
                    Catch ex As Exception
                    End Try
                End With

                Try
                    If stepPro IsNot Nothing Then
                        stepPro.Position = Math.Max(0, transOrder)
                        System.Windows.Forms.Application.DoEvents()
                    End If
                Catch ex As Exception
                End Try
            Loop

        Catch ex As Exception
            handleException(ex)
            Dim a = 1
        Finally
            ReleaseComObject(v4fc)
            ReleaseComObject(v5fc)
            If stepPro IsNot Nothing Then stepPro.Hide()
        End Try

        log(String.Format("Generated DSASv5 transect layer {0}", v5LyrName))
    End Sub


    Function AutogenToTransEdit(autogen As String) As String
        Return IIf(autogen = "0", "1", IIf(autogen = "1", "0", autogen))
    End Function

End Module
