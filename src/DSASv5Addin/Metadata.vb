Imports System.Xml
Imports System.Xml.XPath
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.RuntimeManager


Module Metadata

    Public Function checkForMetadataSetting() As Boolean
        Dim mdFile As String = checkForMetadataTemplate()
        If mdFile Is Nothing Then Exit Function

        Dim mdDoc As XPathDocument
        Dim mdNav As XPathNavigator
        Try
            mdDoc = New XPathDocument(mdFile)
            mdNav = mdDoc.CreateNavigator()

            ' If the user opted to turn off incomplete metadata check, do not bother.
            If My.Settings.DoNotShowAgainCompleteMetadataSettings Then Return True

            ' We do not check for all metadata settings, some may not apply to all users.
            If _
                hasEmptyValue("//origin", mdNav) OrElse
                hasEmptyValue("//abstract", mdNav) OrElse
                hasEmptyValue("//purpose", mdNav) OrElse
                hasEmptyValue("//current", mdNav) OrElse
                hasEmptyValue("//update", mdNav) OrElse
                hasEmptyValue("//progress", mdNav) OrElse
                hasEmptyValue("//accconst", mdNav) OrElse
                (hasEmptyValue("//distrib/cntinfo/cntorgp/cntorg", mdNav) AndAlso hasEmptyValue("//distrib/cntinfo/cntorgp/cntper", mdNav)) OrElse
                hasEmptyValue("//distrib/cntinfo/cntaddr/address", mdNav) OrElse
                hasEmptyValue("//distrib/cntinfo/cntaddr/city", mdNav) OrElse
                hasEmptyValue("//distrib/cntinfo/cntaddr/state", mdNav) _
            Then
                DoNotShowAgainForm.prompt("Warning! DSAS encourages the user to provide some basic information about your data to be entered into the Metadata Settings tab before proceeding with casting transects and running rates of change. Please return to the Set Default Parameters button and make sure to complete ALL fields in the Metadata Settings tab. You may choose to proceed and suppress this message, but you will not be reminded of incomplete metadata again.", "DoNotShowAgainCompleteMetadataSettings")
                Return False
            End If

            Return True

        Catch ex As Exception
            DSASUtility.handleException(ex)
        End Try
    End Function


    Dim wasSyncerEnabled As New Hashtable

    Private Sub setSyncers()
        Dim msm As IMetadataSynchronizerManager = getSingleton("esriGeodatabase.MetadataSynchronizer")

        For i As Integer = 0 To msm.NumSynchronizers - 1
            Dim ms As IMetadataSynchronizer = msm.GetSynchronizer(i)
            wasSyncerEnabled(ms.Name) = msm.GetEnabled(i)
            msm.SetEnabled(i, ms.Name = "FGDC CSDGM")
            log(ms.Name)
            log(msm.GetEnabled(i))
        Next
    End Sub

    Private Sub resetSyncers()
        Dim msm As IMetadataSynchronizerManager = getSingleton("esriGeodatabase.MetadataSynchronizer")

        For i As Integer = 0 To msm.NumSynchronizers - 1
            Dim ms As IMetadataSynchronizer = msm.GetSynchronizer(i)
            msm.SetEnabled(i, wasSyncerEnabled(ms.Name))
        Next
    End Sub


    ' Transform given metadata (assumed to be in ESRI ISO format) into FGDC and replace the original
    Private Function slurp(ByVal mdSync As IXmlPropertySet2) As String
        Dim xslt As New Xml.Xsl.XslCompiledTransform
        Dim xSettings As New Xml.Xsl.XsltSettings
        xSettings.EnableDocumentFunction = True
        slurp = Nothing

        Try
            Dim installDir As String = ActiveRuntime.Path

            xslt.Load(installDir + "Metadata\Translator\Transforms\ArcGIS2FGDC.xsl", xSettings, Nothing)

            'http://support.microsoft.com/kb/323370

            ' Instantiate an XsltArgumentList object.
            ' An XsltArgumentList object is used to supply extension object instances
            ' and values for XSLT paarmeters required for an XSLT transformation	    
            Dim xsltArgList As New System.Xml.Xsl.XsltArgumentList()

            ' Instantiate and add an instance of the extension object to the XsltArgumentList.
            ' The AddExtensionObject method is used to add the Extension object instance to the
            ' XsltArgumentList object. The namespace URI specified as the first parameter 
            ' should match the namespace URI used to reference the extension object in the
            ' XSLT style sheet.
            Dim esri As New EsriXslExtSim()
            xsltArgList.AddExtensionObject("http://www.esri.com/metadata/", esri)


            Dim insr As New System.IO.StringReader(mdSync.GetXml(""))
            Dim inr As New System.Xml.XmlTextReader(insr)
            Dim outsb As New System.Text.StringBuilder()
            Dim outw As System.Xml.XmlWriter = System.Xml.XmlTextWriter.Create(outsb, xslt.OutputSettings)

            xslt.Transform(inr, xsltArgList, outw)
            Return outsb.ToString

            ''Dim mdMain As New XmlMetadata
            'Dim mdSlurp As New XmlMetadata
            ''mdMain.SetXml(Xml)
            'mdSlurp.SetXml(outsb.ToString)

            '' Bring in elements of interest from slurped md into current md

            'mdSync.copyFrom(mdSlurp, "idinfo/spdom/bounding")

            ''AE syncEntInfo(mdSync, mdSlurp)

            'mdSync.copyFrom(mdSync, "Esri/DataProperties/itemProps[1]/itemLocation[1]/linkage[1]", "idinfo/citation/citeinfo/onlink[1]")

            'mdSync.copyFrom(mdSlurp, "spref")
            'If mdSync.CountX("spref/horizsys/geograph/geogunit[.=""Decimal Degree""]") > 0 Then
            '    mdSync.SetPropertyX("spref/horizsys/geograph/geogunit[.=""Decimal Degree""]", "Decimal Degrees", esriXmlPropertyType.esriXPTText, esriXmlSetPropertyAction.esriXSPAAddOrReplace, False)
            'End If

        Catch ex As Exception
            DSASUtility.handleException(ex)
        End Try
    End Function


    Public Sub createMetadata(ByVal m_transectName As String)
        DSASUtility.log("Metadata creation starts...")

        Try
            Dim msm As IMetadataSynchronizerManager = getSingleton("esriGeodatabase.MetadataSynchronizer")
        Catch ex As Exception
            DoNotShowAgainForm.prompt("ArcGIS metadata synchronizer appears to be disabled in your installation." + vbCrLf + "Bounding box and spatial reference will not be synchronized.", "DoNotShowAgainCantInstantiateMetadataSynchronizer")
            Return
        End Try

        Dim mdFile As String = checkForMetadataTemplate()
        If mdFile Is Nothing Then Exit Sub

        Dim pMD As IMetadata = MapUtility.findFeatureLayer(m_transectName, "transect").FeatureClass
        ' We need to make sure FGDC syncer is enabled so we can have it do some work for us.
        setSyncers()
        ' We synchronize to ensure that the feature class has metadata created and populated.
        pMD.Synchronize(esriMetadataSyncAction.esriMSAAlways, 0)
        ' Leave FGDC syncer off if we turned it on.
        resetSyncers()



        ' Load up metadata associated with the feature class
        Dim iXPS As IXmlPropertySet2 = pMD.Metadata
        iXPS.SetXml(slurp(pMD.Metadata))
        'Save bounding and spref sections for later use
        Dim boundingXml As String = iXPS.GetXml("idinfo/spdom/bounding")
        Dim sprefXml As String = iXPS.GetXml("spref")
        ' Read template metadata into feature class metadata
        iXPS.SetXml(My.Computer.FileSystem.ReadAllText(mdFile))

        ' Set misc sections that DSAS has to populate during transect creation
        setMetadataValue(iXPS, "idinfo/citation/citeinfo/title", m_transectName + " - Digital Shoreline Analysis System Version " + DSAS.dsasVersion + " Transect Feature Class")
        setMetadataValue(iXPS, "idinfo/citation/citeinfo/pubdate", Format(Today, "yyyyMMdd"))
        setMetadataValue(iXPS, "idinfo/timeperd/timeinfo/sngdate/caldate", Format(Today, "yyyyMMdd"))
        setMetadataValue(iXPS, "metainfo/metd", Format(Today, "yyyyMMdd"))
        setMetadataValue(iXPS, "metainfo/metrd", Format(Today, "yyyyMMdd"))


        ' Transect Features Generated. Parameters Used: baseline layer=MA_Baseline, baseline group field=group_1, transect spacing=100 meters, transect length=100 meters, cast-direction=right, shoreline uncertainty field name=accuracy, baseline location=onshore, cast method=simple, smoothing distance=10, flip baselines=selected. For additional details on these parameters, please see the DSAS help file distributed with the DSAS software, or visit the USGS website at: http://woodshole.er.usgs.gov/project-pages/dsas/.

        ' Empty out bounding and spref sections that were prefilled by synchronization
        setMetadataValue(iXPS, "idinfo/spdom/bounding", "DSAS")
        setMetadataValue(iXPS, "spref", "DSAS")

        ' Refill bounding and spref sections using synchronization generated content
        Dim xml As String = iXPS.GetXml("/metadata")
        xml = xml.Replace("<bounding>DSAS</bounding>", boundingXml)
        xml = xml.Replace("<spref>DSAS</spref>", sprefXml)
        ' Clear out all sync hints inserted during synchronization
        xml = xml.Replace(" Sync=""TRUE"">", ">")
        ' Plug manipulated xml metadata back into the property set
        iXPS.SetXml(xml)
        ' Other fixes to synchronization generated info to keep MP happy
        iXPS.DeleteProperty("spref/vertdef")
        setMetadataValue(iXPS, "spref/horizsys/planar/planci/coordrep/absres", "1.000000")
        setMetadataValue(iXPS, "spref/horizsys/planar/planci/coordrep/ordres", "1.000000")

        ' Do procstep
        ' First wipe out any procstep's that might pre-exist (in template) and also ensure that procstep element is added if not already there
        iXPS.SetPropertyX("dataqual/lineage/procstep", "", esriXmlPropertyType.esriXPTText, esriXmlSetPropertyAction.esriXSPAAddOrReplace, False)
        iXPS.DeleteProperty("dataqual/lineage/procstep")
        ' Then insert procstep 
        iXPS.SetXml(decorateXml(procstepInsert(iXPS.GetXml("/metadata"), procdescStringForTransectCast())))

        'Dim FGDCSync As New FGDCSynchronizer
        'FGDCSync.Update(iXPS, "SpatialReference", CType(GeoDB.GeodbFeatureClass, IGeoDataset).SpatialReference)
        'FGDCSync.Update(iXPS, "DDExtent", CType(GeoDB.GeodbFeatureClass, IGeoDataset).Extent)
        'FGDCSync.Update(iXPS, "NativeExtent", CType(GeoDB.GeodbFeatureClass, IGeoDataset).Extent)

        ' iXPS.GetXml("/metadata")

        ' Plug property set with manipulated xml metadata back into the object
        pMD.Metadata = iXPS
        DSASUtility.log("Metadata created.")
    End Sub

    Private Function procdescStringForTransectCast() As String
        Dim baseCnt As Integer = MapUtility.GetSelectedCount(MapUtility.findFeatureLayer(My.Settings.Baseline_Feature_Layer, "baseline"))
        Dim baseMsg As String = IIf(baseCnt = 0, "", " on Selected Baseline Features Only")
        Dim procdesc As String = "Transect Features Generated using DSAS v" + DSAS.dsasVersion + baseMsg + ". Parameters Used: "
        procdesc += "baseline layer=" + DSASUtility.nv(My.Settings.Baseline_Feature_Layer, "NULL") + ", "
        procdesc += "baseline group field=" + DSASUtility.nv(My.Settings.Baseline_Group_Field, "NULL") + ", "
        procdesc += "transect spacing=" + DSASUtility.nv(My.Settings.Spacing.ToString, "NULL") + " meters, "
        procdesc += "search distance=" + DSASUtility.nv(My.Settings.Search_Distance.ToString, "NULL") + " meters, "
        procdesc += "land direction=" + IIf(My.Settings.Land_On_Right_Side, "right", "left") + ", "
        procdesc += "shoreline intersection=" + IIf(My.Settings.Seaward, "seaward", "landward") + ", "
        'procdesc += "cast method=" + IIf(My.Settings.Transect_Leg_Length = 0, "simple", "smoothed") + ", "
        procdesc += "smoothing distance=" + DSASUtility.nv(My.Settings.Transect_Leg_Length.ToString, "NULL") + " meters, "
        procdesc += "For additional details on these parameters, please see the DSAS help file distributed with the DSAS software, or visit the USGS website at: http://pubs.usgs.gov/of/2008/1278/ ."
        Return procdesc
    End Function


    ' Rate Calculations Performed.  Parameters Used: 
    ' shoreline layer=MA_Shore, 
    ' shoreline date field=date_, 
    ' shoreline uncertainty field name=accuracy, 
    ' the default accuracy=6 meters, 
    ' shoreline intersection parameters =nearest, 
    ' stats calculations=[Linear Regression Rate (LRR), Weighted Linear Regression (WLR), Least Median of Squares (LMS)], 
    ' shoreline threshold=4, 
    ' confidence interval=99.9 . 
    ' Output rate table name=XYZ_rates.

    Private Function procdescStringForCalculateStats(ByVal selectedStats As String, ByVal threshold As String, ByVal outputTablename As String) As String
        Dim shoreCnt As Integer = MapUtility.GetSelectedCount(MapUtility.findFeatureLayer(My.Settings.Shoreline_Feature_Layer, "shoreline"))
        Dim shoreMsg As String = IIf(shoreCnt = 0, "", " on Selected Shoreline Features Only")
        Dim procdesc As String = "Rate Calculations Performed" + shoreMsg + ".  Parameters Used: "
        procdesc += "shoreline layer=" + DSASUtility.nv(My.Settings.Shoreline_Feature_Layer, "NULL") + ", "
        procdesc += "shoreline date field=" + DSASUtility.nv(My.Settings.Shoreline_Date_Field, "NULL") + ", "
        procdesc += "shoreline uncertainty field name=" + DSASUtility.nv(My.Settings.Shoreline_Uncertainty_Field, "NULL") + ", "
        procdesc += "the default accuracy=" + DSASUtility.nv(My.Settings.Uncertainty.ToString(), "NULL") + " meters, "
        procdesc += "shoreline intersection=" + IIf(nv(DSAS.seaward, My.Settings.Seaward), "seaward", "landward") + ", "
        procdesc += "stats calculations=" + DSASUtility.nv(selectedStats, "NULL") + ", "
        procdesc += "shoreline threshold=" + DSASUtility.nv(threshold, "NULL") + ", "
        procdesc += "confidence interval=" + DSASUtility.nv(My.Settings.Confidence_Interval.ToString, "NULL") + "%, "
        procdesc += "Output rate table name=" + DSASUtility.nv(outputTablename, "NULL") + ". "
        Return procdesc
    End Function

    Public Sub updateMetadataForCalculateStats(ByVal selectedStats As String, ByVal threshold As String, ByVal outputTablename As String)
        DSASUtility.log("Metadata update starts...")
        Dim mdFile As String = checkForMetadataTemplate()
        If mdFile Is Nothing Then Exit Sub

        ' First we synchronize to ensure that the feature class has metadata created.
        Dim pMD As IMetadata = TransectLyrToolCtl.currentTransectLayer.FeatureClass
        ' Load up metadata associated with the feature class
        Dim iXPS As IXmlPropertySet2 = pMD.Metadata

        ' This makes sure dataqual/lineage is there (if DSAS did not create the metadata before)
        iXPS.SetPropertyX("dataqual/lineage", "", esriXmlPropertyType.esriXPTText, esriXmlSetPropertyAction.esriXSPAAddIfNotExists, False)

        ' Then insert procstep 
        iXPS.SetXml(decorateXml(procstepInsert(iXPS.GetXml("/metadata"), procdescStringForCalculateStats(selectedStats, threshold, outputTablename))))

        ' Plug property set with manipulated xml metadata back into the object
        pMD.Metadata = iXPS
        DSASUtility.log("Metadata updated.")
    End Sub

    Private Function decorateXml(ByVal xml As String) As String
        xml = xml.Trim
        If xml.StartsWith("<?xml ") Then Return xml
        Return "<?xml version=""1.0"" ?>" + vbCrLf + "<!-- <!DOCTYPE metadata SYSTEM ""http://www.esri.com/metadata/esriprof80.dtd"">  -->" + vbCrLf + xml
    End Function

    Public Function procstepInsert(ByVal xml As String, ByVal procdesc As String) As String
        Dim mdDoc As New XmlDocument
        mdDoc.LoadXml(xml)
        Dim anchorNode As XmlNode = mdDoc.SelectSingleNode("//dataqual/lineage")

        Dim eltNode As XmlNode
        ' Do procdesc
        eltNode = mdDoc.CreateElement("procstep")
        anchorNode.AppendChild(eltNode)
        anchorNode = eltNode
        ' Do procdesc
        eltNode = mdDoc.CreateElement("procdesc")
        eltNode.AppendChild(mdDoc.CreateTextNode(procdesc))
        anchorNode.AppendChild(eltNode)
        ' Do procdate
        eltNode = mdDoc.CreateElement("procdate")
        eltNode.AppendChild(mdDoc.CreateTextNode(Format(Today, "yyyyMMdd")))
        anchorNode.AppendChild(eltNode)

        Return mdDoc.OuterXml
    End Function

    Private Sub setMetadataValue(ByVal iXPS As IXmlPropertySet2, ByVal name As String, ByVal value As String, Optional ByVal action As esriXmlSetPropertyAction = esriXmlSetPropertyAction.esriXSPAAddOrReplace)
        If value Is Nothing OrElse value.Trim = "" Then value = "NULL"
        iXPS.SetPropertyX(name, value, esriXmlPropertyType.esriXPTText, action, False)
    End Sub

    Private Function checkForMetadataTemplate() As String
        Dim mdFile As String = DSASUtility.getMetadataFolder() + "\DSAS_Metadata_Template.xml"
        If Not System.IO.File.Exists(mdFile) Then
            log(TraceLevel.Error, "Unable to find metadata template file: " + mdFile)
            Return Nothing
        End If
        Return mdFile
    End Function

    ''' <summary>
    ''' Loads metadata settings tab from DSAS metadata template file.
    ''' </summary>
    ''' <param name="frm"></param>
    ''' <remarks></remarks>
    Public Sub LoadMetadataTab(ByVal frm As SetDefaultsForm)
        Dim mdFile As String = checkForMetadataTemplate()
        If mdFile Is Nothing Then Exit Sub

        Dim mdDoc As XPathDocument
        Dim mdNav As XPathNavigator
        Try
            mdDoc = New XPathDocument(mdFile)
            mdNav = mdDoc.CreateNavigator()

            frm.origin.Text = getValueByXpath("//origin", mdNav)
            frm.abstract.Text = getValueByXpath("//abstract", mdNav)
            frm.purpose.Text = getValueByXpath("//purpose", mdNav)
            frm.current.Text = getValueByXpath("//current", mdNav)
            frm.status_update.Text = getValueByXpath("//update", mdNav)
            frm.progress.Text = getValueByXpath("//progress", mdNav)
            frm.accconst.Text = getValueByXpath("//accconst", mdNav)
            frm.cntorg.Text = getValueByXpath("//distrib/cntinfo/cntorgp/cntorg", mdNav)
            frm.cntper.Text = getValueByXpath("//distrib/cntinfo/cntorgp/cntper", mdNav)
            frm.address.Text = getValueByXpath("//distrib/cntinfo/cntaddr/address", mdNav)
            frm.city.Text = getValueByXpath("//distrib/cntinfo/cntaddr/city", mdNav)
            frm.state.Text = getValueByXpath("//distrib/cntinfo/cntaddr/state", mdNav)
            frm.postal.Text = getValueByXpath("//distrib/cntinfo/cntaddr/postal", mdNav)
            frm.cntvoice.Text = getValueByXpath("//distrib/cntinfo/cntvoice", mdNav)
            frm.cntemail.Text = getValueByXpath("//distrib/cntinfo/cntemail", mdNav)

        Catch ex As Exception
            DSASUtility.handleException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Saves metadata settings tab information to DSAS metadata template file.
    ''' </summary>
    ''' <param name="frm"></param>
    ''' <remarks></remarks>
    Public Sub SaveMetadataTab(ByVal frm As SetDefaultsForm)
        Dim mdFile As String = checkForMetadataTemplate()
        If mdFile Is Nothing Then Exit Sub

        Dim mdDoc As New XmlDocument
        Dim mdNav As XPathNavigator
        Try
            mdDoc.Load(mdFile)
            mdNav = mdDoc.CreateNavigator()

            setValueByXpath("//origin", frm.origin.Text, mdNav)
            setValueByXpath("//abstract", frm.abstract.Text, mdNav)
            setValueByXpath("//purpose", frm.purpose.Text, mdNav)
            'setValueByXpath("//current", frm.current.Text, mdNav)
            setValueByXpath("//update", frm.status_update.Text, mdNav)
            setValueByXpath("//progress", frm.progress.Text, mdNav)
            setValueByXpath("//accconst", frm.accconst.Text, mdNav)
            setValueByXpath("//distrib/cntinfo/cntorgp/cntorg", frm.cntorg.Text, mdNav)
            setValueByXpath("//distrib/cntinfo/cntorgp/cntper", frm.cntper.Text, mdNav)
            setValueByXpath("//distrib/cntinfo/cntaddr/address", frm.address.Text, mdNav)
            setValueByXpath("//distrib/cntinfo/cntaddr/city", frm.city.Text, mdNav)
            setValueByXpath("//distrib/cntinfo/cntaddr/state", frm.state.Text, mdNav)
            setValueByXpath("//distrib/cntinfo/cntaddr/postal", frm.postal.Text, mdNav)
            setValueByXpath("//distrib/cntinfo/cntvoice", frm.cntvoice.Text, mdNav)
            setValueByXpath("//distrib/cntinfo/cntemail", frm.cntemail.Text, mdNav)
            setValueByXpath("//metc/cntinfo/cntorgp/cntorg", frm.cntorg.Text, mdNav)
            setValueByXpath("//metc/cntinfo/cntorgp/cntper", frm.cntper.Text, mdNav)
            setValueByXpath("//metc/cntinfo/cntaddr/address", frm.address.Text, mdNav)
            setValueByXpath("//metc/cntinfo/cntaddr/city", frm.city.Text, mdNav)
            setValueByXpath("//metc/cntinfo/cntaddr/state", frm.state.Text, mdNav)
            setValueByXpath("//metc/cntinfo/cntaddr/postal", frm.postal.Text, mdNav)
            setValueByXpath("//metc/cntinfo/cntvoice", frm.cntvoice.Text, mdNav)
            setValueByXpath("//metc/cntinfo/cntemail", frm.cntemail.Text, mdNav)

            mdDoc.Save(mdFile)

        Catch ex As XPathException
            DSASUtility.handleException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Given an xpath string and XPathNavigator object for an existing XML document, returns true 
    ''' if the (first) node selected by the xpath expression has a value considered empty by DSAS.
    ''' </summary>
    ''' <param name="xp"></param>
    ''' <param name="nav"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function hasEmptyValue(ByVal xp As String, ByVal nav As XPathNavigator) As String
        Dim val As String = getValueByXpath(xp, nav).Trim
        Return val = "" OrElse val = "From User Interface" OrElse val = "NULL"
    End Function

    ''' <summary>
    ''' Given an xpath string and XPathNavigator object for an existing XML document, returns the value of 
    ''' the (first) node selected by the xpath expression. If no match, empty string is returned.
    ''' </summary>
    ''' <param name="xp"></param>
    ''' <param name="nav"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getValueByXpath(ByVal xp As String, ByVal nav As XPathNavigator) As String
        nav = nav.SelectSingleNode(xp)
        If nav Is Nothing Then
            Return ""
        ElseIf nav.Value = "" OrElse nav.Value = "From User Interface" OrElse nav.Value = "NULL" Then
            Return ""
        Else
            Return nav.Value
        End If
    End Function

    ''' <summary>
    ''' Given an xpath string, a value string and XPathNavigator object for an existing XML document, sets the value of 
    ''' the (first) node selected by the xpath expression. If no match, no action is taken.
    ''' </summary>
    ''' <param name="xp"></param>
    ''' <param name="nav"></param>
    ''' <remarks></remarks>
    Private Sub setValueByXpath(ByVal xp As String, ByVal val As String, ByVal nav As XPathNavigator)
        nav = nav.SelectSingleNode(xp)
        If nav IsNot Nothing Then
            If val Is Nothing OrElse val.Trim = "" Then val = "NULL"
            nav.SetValue(val)
        End If
    End Sub


    ''' <summary>
    ''' Current seaward/landward setting may be different than what it was when transects were cast.
    ''' This function attempts to determine that from metadata.
    ''' </summary>
    ''' <returns></returns>
    Function getSeawardLandwardSettingFromMetadata(xectLayer As ESRI.ArcGIS.Carto.IFeatureLayer) As Boolean?
        If xectLayer Is Nothing Then Return Nothing

        Dim pMD As IMetadata = xectLayer.FeatureClass
        Dim iXPS As IXmlPropertySet2 = pMD.Metadata
        Dim procstep As String = iXPS.GetXml("dataqual/lineage")
        If procstep Is Nothing Then Return Nothing
        Dim landSeaIdx As Integer = procstep.LastIndexOf("shoreline intersection")
        If landSeaIdx > -1 Then
            landSeaIdx += "shoreline intersection".Length
            Dim rest As String = procstep.Substring(landSeaIdx)
            If rest.StartsWith(" parameters") Then
                rest = rest.Substring(" parameters".Length + 1)
            Else
                rest = rest.Substring(1)
            End If
            If rest.StartsWith("seaward") OrElse rest.StartsWith("farthest") Then
                Return True
            ElseIf rest.StartsWith("landward") OrElse rest.StartsWith("nearest") Then
                Return False
            End If
        Else
            Return Nothing
        End If
    End Function

End Module


' procstep template
'
'<dataqual>
'	<logic>These data were generated using DSAS, an automated software program which does perform checks for fidelity of the input features. Any testing on these data should be carried out by end-users to ensure fidelity of relationships.</logic>
'	<complete>This dataset contains all the transects automatically generated by the DSAS software application.</complete>
'	<lineage>
'		<procstep>
'			<procdesc>Transect Features Generated.  Parameters Used: baseline layer=MA_Baseline, baseline group field=group_1, transect spacing=100 meters, transect length=100 meters, cast-direction=right, shoreline uncertainty field name=accuracy, baseline location=onshore, cast method=simple, smoothing distance=10, flip baselines=selected. For additional details on these parameters, please see the DSAS help file distributed with the DSAS software, or visit the USGS website at: http://woodshole.er.usgs.gov/project-pages/dsas/.</procdesc>
'			<procdate>20080513</procdate>
'		</procstep>
'		<procstep>
'			<procdesc>Rate Calculations Performed.  Parameters Used: shoreline layer=MA_Shore, shoreline date field=date_, shoreline uncertainty field name=accuracy, the default accuracy=4.4 meters, shoreline intersection parameters =nearest, stats calculations=[End Point Rate (EPR), Linear Regression Rate (LRR), Weighted Linear Regression (WLR)], shoreline threshold=4, confidence interval=99.9 . Output rate table name=XYZ_rates.</procdesc>
'			<procdate>20080513</procdate>
'		</procstep>
'		<procstep>
'			<procdesc>Rate Calculations Performed.  Parameters Used: shoreline layer=MA_Shore, shoreline date field=date_, shoreline uncertainty field name=accuracy, the default accuracy=6 meters, shoreline intersection parameters =nearest, stats calculations=[Linear Regression Rate (LRR), Weighted Linear Regression (WLR), Least Median of Squares (LMS)], shoreline threshold=4, confidence interval=99.9 . Output rate table name=XYZ_rates.</procdesc>
'			<procdate>20080514</procdate>
'		</procstep>
'	</lineage>
'</dataqual>
