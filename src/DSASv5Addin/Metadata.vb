Imports System.Xml
Imports System.Xml.XPath
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.RuntimeManager


Module Metadata
    ''' <summary>
    ''' Check if the metadata fields have been filled out by the user
    ''' </summary>
    ''' <returns>True if all metadata fields have non-empty values, false otherwise</returns>
    Public Function checkForMetadataSetting() As Boolean
        Dim mdFile As String = getMetadataTemplateFilename()
        If mdFile Is Nothing Then Exit Function

        Dim mdDoc As XPathDocument
        Dim mdNav As XPathNavigator
        Try
            mdDoc = New XPathDocument(mdFile)
            mdNav = mdDoc.CreateNavigator()

            ' We do not check for all metadata settings, some may not apply to all users.
            If _
                hasEmptyValue("//origin", mdNav) OrElse
                hasEmptyValue("//abstract", mdNav) OrElse
                hasEmptyValue("//purpose", mdNav) OrElse
                hasEmptyValue("//current", mdNav) OrElse
                hasEmptyValue("//update", mdNav) OrElse
                hasEmptyValue("//progress", mdNav) OrElse
                hasEmptyValue("//accconst", mdNav) OrElse
                hasEmptyValue("//distrib/cntinfo/cntorgp/cntorg", mdNav) OrElse
                hasEmptyValue("//distrib/cntinfo/cntorgp/cntper", mdNav) OrElse
                hasEmptyValue("//distrib/cntinfo/cntaddr/address", mdNav) OrElse
                hasEmptyValue("//distrib/cntinfo/cntaddr/city", mdNav) OrElse
                hasEmptyValue("//distrib/cntinfo/cntaddr/state", mdNav) OrElse
                hasEmptyValue("//distrib/cntinfo/cntaddr/postal", mdNav) OrElse
                hasEmptyValue("//distrib/cntinfo/cntvoice", mdNav) OrElse
                hasEmptyValue("//distrib/cntinfo/cntemail", mdNav) _
            Then
                Return MsgBox("Warning! DSAS encourages the user to provide some basic information in the Metadata Settings tab before proceeding with casting transects and running rates of change. Please click 'Cancel' to return to the Set Default Parameters window and complete all fields in the Metadata Settings tab. Or, click 'OK' to proceed with incomplete metadata.", MsgBoxStyle.OkCancel, DSAS.MsgBoxTitle) = MsgBoxResult.Ok
            End If

            Return True

        Catch ex As Exception
            DSASUtility.handleException(ex)
        End Try
    End Function

    ''' <summary>
    ''' Used to temporarily track which metadata synchronizers are enabled
    ''' </summary>
    Dim wasSyncerEnabled As New Hashtable

    ''' <summary>
    ''' Disable all metadata synchronizers but the FGDC CSDGM metadata synchronizer.
    ''' </summary>
    Private Sub setSyncers()
        Dim msm As IMetadataSynchronizerManager = getSingleton("esriGeodatabase.MetadataSynchronizer")

        For i As Integer = 0 To msm.NumSynchronizers - 1
            Dim ms As IMetadataSynchronizer = msm.GetSynchronizer(i)
            If ms Is Nothing Then
                log(String.Format("Syncer {0} of {1} cannot be accessed. Ignoring...", i + 1, msm.NumSynchronizers))
            Else
                wasSyncerEnabled(ms.Name) = msm.GetEnabled(i)
                msm.SetEnabled(i, ms.Name = "FGDC CSDGM")
                log(ms.Name)
                log(msm.GetEnabled(i))
            End If
        Next
    End Sub

    ''' <summary>
    ''' Reset all metadata synchronizers to their prior state
    ''' </summary>
    Private Sub resetSyncers()
        Dim msm As IMetadataSynchronizerManager = getSingleton("esriGeodatabase.MetadataSynchronizer")

        For i As Integer = 0 To msm.NumSynchronizers - 1
            Dim ms As IMetadataSynchronizer = msm.GetSynchronizer(i)
            If ms IsNot Nothing Then
                msm.SetEnabled(i, wasSyncerEnabled(ms.Name))
            End If
        Next
    End Sub


    ''' <summary>
    ''' Transform given metadata (assumed to be in ESRI ISO format) into FGDC and replace the original
    ''' </summary>
    ''' <param name="mdSync"></param>
    ''' <returns>FGDC CSDGM representation of the metadata</returns>
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

    ''' <summary>
    ''' Creates metadata for the given DSAS layer
    ''' </summary>
    ''' <param name="lyrname">Name of layer to create metadata for</param>
    ''' <param name="lyrGenericName">Type of layer</param>
    ''' <param name="params">Layer type specific parameters that are needed in metadata creation</param>
    Public Sub createMetadata(ByVal lyrname As String, Optional lyrGenericName As String = "transect", Optional params As Dictionary(Of String, String) = Nothing)
        DSASUtility.log("Metadata creation starts...")

        If params Is Nothing Then params = New Dictionary(Of String, String)
        params.Add("layerName", lyrname)
        params.Add("layerType", lyrGenericName)

        Try
            Dim msm As IMetadataSynchronizerManager = getSingleton("esriGeodatabase.MetadataSynchronizer")
        Catch ex As Exception
            DoNotShowAgainForm.prompt("ArcGIS metadata synchronizer appears to be disabled in your installation." + vbCrLf + "Bounding box and spatial reference will not be synchronized.", "DoNotShowAgainCantInstantiateMetadataSynchronizer")
            Return
        End Try

        Dim mdFile As String = getMetadataTemplateFilename(lyrGenericName)
        If mdFile Is Nothing Then Exit Sub

        Dim pMD As IMetadata = MapUtility.findFeatureLayer(lyrname, lyrGenericName).FeatureClass
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

        ' Read template metadata
        Dim mdTxt = My.Computer.FileSystem.ReadAllText(mdFile)
        ' Special handling for attributes that have confidence interval built into their name
        mdTxt = mdTxt.Replace("___CI___", CI.getCIFor(My.Settings.Confidence_Interval).fieldNameCompatible)

        ' Set feature class metadata
        iXPS.SetXml(mdTxt)

        ' Set misc sections that DSAS has to populate
        setMetadataValue(iXPS, "idinfo/citation/citeinfo/pubdate", Format(Today, "yyyyMMdd"))
        setMetadataValue(iXPS, "idinfo/timeperd/timeinfo/sngdate/caldate", Format(Today, "yyyyMMdd"))
        setMetadataValue(iXPS, "metainfo/metd", Format(Today, "yyyyMMdd"))
        setMetadataValue(iXPS, "metainfo/metrd", Format(Today, "yyyyMMdd"))


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

        ' Append DSAS mandatory text to "purpose" element
        Dim purpose As String = getValueByXpath("idinfo/descript/purpose", iXPS)
        Dim purposeEpilogue = DSAS.layerTypes(lyrGenericName)("purposeEpilogue")
        If Not purpose.Contains(purposeEpilogue) Then purpose = purpose.Trim + " " + purposeEpilogue
        setMetadataValue(iXPS, "idinfo/descript/purpose", purpose)

        ' Copy procstep from source - if there is one
        Dim mdSource As String = DSAS.layerTypes(lyrGenericName)("metadataSource")
        If mdSource Is Nothing Then
            ' First wipe out any procstep's that might pre-exist (in template) and also ensure that procstep element is added if not already there
            iXPS.SetPropertyX("dataqual/lineage/procstep", "", esriXmlPropertyType.esriXPTText, esriXmlSetPropertyAction.esriXSPAAddOrReplace, False)
            iXPS.DeleteProperty("dataqual/lineage/procstep")
        Else
            Dim procstepFromSource As String = getSnippetFrom(layernameConvert(lyrname, lyrGenericName, mdSource), "dataqual/lineage")
            If procstepFromSource IsNot Nothing Then insertSnippetInto(iXPS, "dataqual/lineage", procstepFromSource)
        End If
        ' Then insert procstep 
        Dim procdesc As String = procdescStringFor(params)
        If procdesc IsNot Nothing Then iXPS.SetXml(decorateXml(procstepInsert(iXPS.GetXml("/metadata"), procdesc)))

        Dim title As String = lyrname + " - Digital Shoreline Analysis System Version " + DSAS.dsasVersion + " " + StrConv(DSAS.layerTypes(lyrGenericName)("friendlyName"), VbStrConv.ProperCase) + " Feature Class"
        setMetadataValue(iXPS, "idinfo/citation/citeinfo/title", title)

        setMetadataValue(iXPS, "idinfo/crossref/citation/citeinfo/othercit", "Please use the following citation: Himmelstoss, E.A., Farris, A.S., Henderson, R.E., Kratzmann, M.G., Ergul, A., Zhang, O., Zichichi, J.L., Thieler, E.R., 2018, Digital Shoreline Analysis System (v5.0): U.S. Geological Survey software")
        setMetadataValue(iXPS, "eainfo/detailed/enttyp/enttypl", params("layerName"))

        'Dim FGDCSync As New FGDCSynchronizer
        'FGDCSync.Update(iXPS, "SpatialReference", CType(GeoDB.GeodbFeatureClass, IGeoDataset).SpatialReference)
        'FGDCSync.Update(iXPS, "DDExtent", CType(GeoDB.GeodbFeatureClass, IGeoDataset).Extent)
        'FGDCSync.Update(iXPS, "NativeExtent", CType(GeoDB.GeodbFeatureClass, IGeoDataset).Extent)

        ' Plug property set with manipulated xml metadata back into the object
        pMD.Metadata = iXPS
        DSASUtility.log("Metadata created.")
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="params">Parameters that may be recorded in process description</param>
    ''' <returns></returns>
    Private Function procdescStringFor(params As Dictionary(Of String, String)) As String
        Dim lyrType As String = params("layerType")
        If lyrType = "transect" Then
            Return procdescStringForTransectCast(params)
        ElseIf lyrType = "rates" Then
            params("otherLayerName") = DSASUtility.layernameConvert(params("layerName"), "rates", "intersect")
            Return procdescStringForRatesAndIntersect(params)
        ElseIf lyrType = "intersect" Then
            params("otherLayerName") = DSASUtility.layernameConvert(params("layerName"), "intersect", "rates")
            Return procdescStringForRatesAndIntersect(params)
        ElseIf lyrType = "forecast" Then
            params("otherLayerName") = DSASUtility.layernameConvert(params("layerName"), "forecast", "forecast_uncy")
            Return procdescStringForForecast(params)
        ElseIf lyrType = "forecast_uncy" Then
            params("otherLayerName") = DSASUtility.layernameConvert(params("layerName"), "forecast_uncy", "forecast")
            Return procdescStringForForecast(params)
        ElseIf lyrType = "forecast_points" Then
            Return procdescStringForForecastPoints(params)
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Prepares the <procdesc/> metadata element content based on current settings for newly created transect layer
    ''' </summary>
    ''' <param name="params">Parameters that may be recorded in process description</param>
    ''' <returns>String content for the <procdesc/> metadata element</returns>
    Private Function procdescStringForTransectCast(params As Dictionary(Of String, String)) As String
        Dim baseCnt As Integer = MapUtility.GetSelectedCount(MapUtility.findFeatureLayer(My.Settings.Baseline_Feature_Layer, "baseline"))
        Dim baseMsg As String = IIf(baseCnt = 0, "", " on selected baseline features only")
        Dim procdesc As String = "Transect features generated using DSAS v" + DSAS.dsasVersion + baseMsg + ". Parameters Used: "
        procdesc += "baseline layer=" + DSASUtility.nv(My.Settings.Baseline_Feature_Layer, "NULL") + ", "
        procdesc += "baseline group field=" + DSASUtility.nv(My.Settings.Baseline_Group_Field, "NULL") + ", "
        procdesc += "transect spacing=" + DSASUtility.nv(My.Settings.Spacing.ToString, "NULL") + " meters, "
        procdesc += "search distance=" + DSASUtility.nv(My.Settings.Search_Distance.ToString, "NULL") + " meters, "
        procdesc += "land direction=" + IIf(My.Settings.Land_On_Right_Side, "right", "left") + ", "
        procdesc += "shoreline intersection=" + IIf(My.Settings.Seaward, "seaward", "landward") + ", "
        procdesc += String.Format("File produced = {0} .", params("layerName"))
        procdesc += "For additional details on these parameters, please see the DSAS help file distributed with the DSAS software, or visit the USGS website at: https://woodshole.er.usgs.gov/project-pages/DSAS/ "
        Return procdesc
    End Function


    ''' <summary>
    ''' Prepares the <procdesc/> metadata element content based on current settings and provided parameters for newly created rates and intersect layer
    ''' </summary>
    ''' <param name="params">Parameters that may be recorded in process description</param>
    ''' <returns>String content for the <procdesc/> metadata element</returns>
    Private Function procdescStringForRatesAndIntersect(params As Dictionary(Of String, String)) As String
        Dim shoreCnt As Integer = MapUtility.GetSelectedCount(MapUtility.findFeatureLayer(My.Settings.Shoreline_Feature_Layer, "shoreline"))
        Dim shoreMsg As String = IIf(shoreCnt = 0, "", " on selected shoreline features Only")
        Dim procdesc As String = "Shoreline intersects and rate calculations performed" + shoreMsg + ". Parameters Used: "
        procdesc += "shoreline layer=" + DSASUtility.nv(My.Settings.Shoreline_Feature_Layer, "NULL") + ", "
        procdesc += "shoreline date field=" + DSASUtility.nv(My.Settings.Shoreline_Date_Field, "NULL") + ", "
        procdesc += "shoreline uncertainty field name=" + DSASUtility.nv(My.Settings.Shoreline_Uncertainty_Field, "NULL") + ", "
        procdesc += "the default accuracy=" + DSASUtility.nv(My.Settings.Uncertainty.ToString(), "NULL") + " meters, "
        procdesc += "shoreline intersection=" + IIf(nv(DSAS.seaward, My.Settings.Seaward), "seaward", "landward") + ", "
        procdesc += "stats calculations=" + DSASUtility.nv(params("selectedCalcs"), "NULL") + ", "
        procdesc += "shoreline threshold=" + IIf(My.Settings.Intersect_Threshold_Enforced, My.Settings.Intersect_Threshold_Value.ToString, "none") + ", "
        procdesc += "confidence interval=" + DSASUtility.nv(My.Settings.Confidence_Interval.ToString, "NULL") + "%. "
        procdesc += String.Format("Files produced = {0} , {1} .", params("layerName"), params("otherLayerName"))
        Return procdesc
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="params">Parameters that may be recorded in process description</param>
    ''' <returns></returns>
    Private Function procdescStringForForecast(params As Dictionary(Of String, String)) As String

        Return String.Format("Shoreline forecast (polyline and point) and forecast uncertainty (polygon) calculated. A = {0} year shoreline forecast with uncertainty was performed using: input rate file (containing LRR values used in forecasting) = {1}, and intersect file = {2}. Files produced = {3} , {4} , {5} .",
                             params("forecastPeriod"),
                             layernameConvert(params("layerName"), params("layerType"), "rates"),
                             layernameConvert(params("layerName"), params("layerType"), "intersect"),
                             params("layerName"),
                             params("otherLayerName"),
                             layernameConvert(params("layerName"), params("layerType"), "forecast_points")
                             )
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="params">Parameters that may be recorded in process description</param>
    ''' <returns></returns>
    Private Function procdescStringForForecastPoints(params As Dictionary(Of String, String)) As String

        Return String.Format("Shoreline forecast (polyline and point) and forecast uncertainty (polygon) calculated. A = {0} year shoreline forecast with uncertainty was performed using: input rate file (containing LRR values used in forecasting) = {1}, and intersect file = {2}. File produced = {3} .",
                             params("forecastPeriods"),
                             layernameConvert(params("layerName"), params("layerType"), "rates"),
                             layernameConvert(params("layerName"), params("layerType"), "intersect"),
                             layernameConvert(params("layerName"), params("layerType"), "forecast_points")
                             )
    End Function


    ''' <summary>
    ''' Decorate given xml body with the customary xml headers
    ''' </summary>
    ''' <param name="xml"></param>
    ''' <returns>Decorated xml string</returns>
    Private Function decorateXml(ByVal xml As String) As String
        xml = xml.Trim
        If xml.StartsWith("<?xml ") Then Return xml
        Return "<?xml version=""1.0"" ?>" + vbCrLf + "<!-- <!DOCTYPE metadata SYSTEM ""http://www.esri.com/metadata/esriprof80.dtd"">  -->" + vbCrLf + xml
    End Function

    ''' <summary>
    ''' Inserts a new <procstep/> element into the given xml
    ''' </summary>
    ''' <param name="xml">Xml string to be modified</param>
    ''' <param name="procdesc">Contents of <procdesc/> element to be inserted as part of <procstep/> element</param>
    ''' <returns>Modified xml</returns>
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
        ' Do proctime
        eltNode = mdDoc.CreateElement("proctime")
        eltNode.AppendChild(mdDoc.CreateTextNode(Format(Now, "HH:mm:ss")))
        anchorNode.AppendChild(eltNode)

        Return mdDoc.OuterXml
    End Function

    ''' <summary>
    ''' Helper function to set the value of an xml element
    ''' </summary>
    ''' <param name="iXPS">Xml to be modified</param>
    ''' <param name="name">Xml path to be modified</param>
    ''' <param name="value">Element value to modify with</param>
    Private Sub setMetadataValue(ByVal iXPS As IXmlPropertySet2, ByVal name As String, ByVal value As String)
        If value Is Nothing OrElse value.Trim = "" Then value = "NULL"
        iXPS.SetPropertyX(name, value, esriXmlPropertyType.esriXPTText, esriXmlSetPropertyAction.esriXSPAAddOrReplace, False)
    End Sub

    ''' <summary>
    ''' Get the metedata template filename for the given layer type 
    ''' </summary>
    ''' <param name="mdTemplate"></param>
    ''' <returns>String containing the full filepath name for the metadata template</returns>
    Private Function getMetadataTemplateFilename(Optional mdTemplate As String = "Transect") As String
        Dim mdFolder As String = DSASUtility.getMetadataFolder()
        Dim mdFile As String = String.Format("{0}\DSAS_Metadata_Template_{1}.xml", mdFolder, mdTemplate)
        If Not System.IO.File.Exists(mdFile) Then
            log(TraceLevel.Error, "Unable to find metadata template file: " + mdFile)
            Return Nothing
        End If
        Return mdFile
    End Function

    ''' <summary>
    ''' Loads metadata settings tab from DSAS metadata template file.
    ''' </summary>
    ''' <param name="frm">Default Settings form to set metadata values in</param>
    Public Sub LoadMetadataTab(ByVal frm As SetDefaultsForm)
        Dim mdFile As String = getMetadataTemplateFilename()
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
    ''' <param name="frm">Default Settings form to save metadata values from</param>
    Public Sub SaveMetadataTab(ByVal frm As SetDefaultsForm)
        For Each lyrGenericName As String In DSAS.layerTypes.Keys
            SaveMetadataTabInner(frm, lyrGenericName)
        Next
    End Sub


    ''' <summary>
    ''' Saves metadata settings tab information to DSAS metadata template file.
    ''' </summary>
    ''' <param name="frm">Default Settings form to save metadata values from</param>
    ''' <param name="lyrGenericName">The type of layer template to save to</param>
    Private Sub SaveMetadataTabInner(ByVal frm As SetDefaultsForm, lyrGenericName As String)
        Dim mdFile As String = getMetadataTemplateFilename(lyrGenericName)
        If mdFile Is Nothing Then Exit Sub

        Dim mdDoc As New XmlDocument
        Dim mdNav As XPathNavigator
        Try
            mdDoc.Load(mdFile)
            mdNav = mdDoc.CreateNavigator()

            setValueByXpath("//origin", frm.origin.Text, mdNav)
            setValueByXpath("//abstract", frm.abstract.Text, mdNav)
            setValueByXpath("//purpose", frm.purpose.Text, mdNav)
            setValueByXpath("//current", frm.current.Text, mdNav)
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
    Private Function hasEmptyValue(ByVal xp As String, ByVal nav As XPathNavigator) As String
        Return getValueByXpath(xp, nav).Trim = ""
    End Function

    ''' <summary>
    ''' Given an xpath string and XPathNavigator object for an existing XML document, returns the value of 
    ''' the (first) node selected by the xpath expression. If no match, empty string is returned.
    ''' </summary>
    ''' <param name="xp"></param>
    ''' <param name="nav"></param>
    ''' <returns></returns>
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
    ''' Given an xpath string and IXmlPropertySet2 object for an existing XML document, returns the value of 
    ''' the (first) node selected by the xpath expression. If no match, empty string is returned.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="iXPS"></param>
    ''' <returns></returns>
    Private Function getValueByXpath(name As String, iXPS As IXmlPropertySet2) As String
        Dim nameParts As String() = name.Split("/")
        If Not name.EndsWith("]") Then name += "[0]"
        getValueByXpath = iXPS.GetXml(name)
        getValueByXpath = getValueByXpath.Replace(String.Format("<{0}>", nameParts(nameParts.Length - 1)), "")
        getValueByXpath = getValueByXpath.Replace(String.Format("</{0}>", nameParts(nameParts.Length - 1)), "")
    End Function

    ''' <summary>
    ''' Given an xpath string, a value string and XPathNavigator object for an existing XML document, sets the value of 
    ''' the (first) node selected by the xpath expression. If no match, no action is taken.
    ''' </summary>
    ''' <param name="xp"></param>
    ''' <param name="nav"></param>
    Private Sub setValueByXpath(ByVal xp As String, ByVal val As String, ByVal nav As XPathNavigator)
        nav = nav.SelectSingleNode(xp)
        If nav IsNot Nothing Then
            If val Is Nothing OrElse val.Trim = "" Then val = ""
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


    ''' <summary>
    ''' Retrieve desired subsection of the layer's metadata
    ''' </summary>
    ''' <param name="lyrName">Name of layer with metadata</param>
    ''' <param name="name">The xpath for the subsection of metadata</param>
    ''' <returns></returns>
    Function getSnippetFrom(lyrName As String, name As String) As String
        Dim pMD As IMetadata = MapUtility.findFeatureLayer(lyrName).FeatureClass
        Dim iXPS As IXmlPropertySet2 = pMD.Metadata
        Return iXPS.GetXml(name)
    End Function

    ''' <summary>
    ''' Insert a snippet of xml into another xml
    ''' </summary>
    ''' <param name="iXPS">Object holding target xml</param>
    ''' <param name="name">Xpath location of insertion point</param>
    ''' <param name="snippetXml">Xml to be inserted</param>
    Sub insertSnippetInto(iXPS As IXmlPropertySet2, name As String, snippetXml As String)
        Dim aGuid As String = System.Guid.NewGuid().ToString
        iXPS.SetPropertyX(name, aGuid, esriXmlPropertyType.esriXPTText, esriXmlSetPropertyAction.esriXSPAAddOrReplace, False)
        Dim Xml As String = iXPS.GetXml("")
        Dim nameParts As String() = name.Split("/")
        Dim textToReplace As String = String.Format("<{0}>{1}</{0}>", nameParts(nameParts.Length - 1), aGuid)
        Xml = Xml.Replace(textToReplace, snippetXml)
        iXPS.SetXml(Xml)
    End Sub

End Module

