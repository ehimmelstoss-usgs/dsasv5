Option Explicit On
Option Strict On

Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.esriSystem
Imports System.Data.OleDb


Public Class GeoDB
    Private Shared tables As New Generic.Dictionary(Of String, ITable)()
    Private Shared featureClassMap As New Generic.Dictionary(Of String, IFeatureClass)()

    Public Shared Sub init()
        tables("Intersect") = Nothing
        tables("Distance") = Nothing
        tables("Bias") = Nothing
        tables("BiasInters") = Nothing

        featureClassMap("Intersect") = Nothing
    End Sub

    Public Shared Sub uninit()
        For Each tbl As ITable In tables.Values
            DSASUtility.ReleaseComObject(tbl)
        Next
    End Sub


    '
    'start a transaction
    '
    Public Shared Sub StartTransaction(ByVal lyr As IFeatureLayer)
        Dim wsEdit As IWorkspaceEdit
        'start an edit operation or transaction
        wsEdit = CType(DirectCast(lyr, IDataset).Workspace, IWorkspaceEdit)
        If Not (wsEdit.IsBeingEdited) Then
            wsEdit.StartEditing(True)
            wsEdit.StartEditOperation()
        End If
    End Sub
    '
    'stops a transaction
    '
    Public Shared Sub StopTransaction(ByVal lyr As IFeatureLayer)
        Dim wsEdit As IWorkspaceEdit
        'end the transection
        wsEdit = CType(DirectCast(lyr, IDataset).Workspace, IWorkspaceEdit)
        If wsEdit.IsBeingEdited Then
            wsEdit.StopEditOperation()
            wsEdit.StopEditing(True)
        End If
    End Sub
    '
    'abort a transaction
    '
    Public Shared Sub AbortTransaction(ByVal lyr As IFeatureLayer)
        Dim wsEdit As IWorkspaceEdit
        'end the transection
        wsEdit = CType(DirectCast(lyr, IDataset).Workspace, IWorkspaceEdit)
        If wsEdit.IsBeingEdited Then wsEdit.AbortEditOperation()
        wsEdit = Nothing
    End Sub


    ' create a feature class inside a geodatabase workspace
    '
    Public Shared Function CreateWorkspaceFeatureClassAndAddToTOC(featureClassName As String, genericName As String, Optional personalGeodbFileName As String = Nothing, Optional workspaceLayerName As String = Nothing, Optional applyBias As Boolean = False) As IFeatureClass
        Dim featWs As IFeatureWorkspace
        Dim ws As IWorkspace
        Dim ws2 As IWorkspace2
        Dim pCLSID As New ESRI.ArcGIS.esriSystem.UID
        Dim m_featCls As IFeatureClass = Nothing

        If workspaceLayerName IsNot Nothing Then
            ws = MapUtility.workspaceForLayer(workspaceLayerName)
        ElseIf personalGeodbFileName Is Nothing Then
            ws = MapUtility.workspaceForLayer(featureClassName)
        Else
            Dim awf As IWorkspaceFactory = DirectCast(getSingleton("esriDataSourcesGDB.AccessWorkspaceFactory"), IWorkspaceFactory)
            ws = awf.OpenFromFile(personalGeodbFileName, 0)
        End If
        ws2 = CType(ws, IWorkspace2)
        featWs = CType(ws2, IFeatureWorkspace)

        'check if feature class already exists
        If (ws2.NameExists(esriDatasetType.esriDTFeatureClass, featureClassName)) Then
            Dim vbResp As MsgBoxResult
            vbResp = MsgBox(featureClassName & " already exists, overwrite?", MsgBoxStyle.YesNo, "Existing Feature Class")
            If vbResp = MsgBoxResult.Yes Then
                Dim pDS As IDataset
                pDS = CType(featWs.OpenFeatureClass(featureClassName), IDataset)
                If pDS.CanDelete() Then
                    Dim pLayer As ILayer = MapUtility.findFeatureLayer(featureClassName)
                    If pLayer IsNot Nothing Then
                        My.Document.FocusMap.DeleteLayer(pLayer)
                    End If
                    pDS.Delete()
                Else
                    log(TraceLevel.Error, "Unable to delete " + featureClassName + "." + vbCrLf + "Please make sure the geodatabase is not exclusively opened by another process such as a catalog window.")
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        End If

        pCLSID.Value = "esriGeoDatabase.Feature"

        'set the fields
        Dim m_flds As IFields = CreateFields(genericName, applyBias)

        'create the feature class and set the member feature class variable
        m_featCls = featWs.CreateFeatureClass(featureClassName, m_flds,
        pCLSID, Nothing, esriFeatureType.esriFTSimple, "SHAPE", Nothing)

        If m_featCls Is Nothing Then
            log(TraceLevel.Error, featureClassName + " feature class could not be created!")
            Return Nothing
        End If

        featureClassMap(genericName) = m_featCls
        MapUtility.AddFeatureClassToMap(m_featCls)

        Return m_featCls

    End Function

    ''' <summary>
    ''' create a object class, table inside a workspace
    ''' </summary>
    ''' <param name="tableName"></param>
    ''' <param name="tblGenericName"></param>
    ''' <param name="tablePrefix"></param>
    ''' <returns></returns>
    ''' <remarks>This does not currently handle the case where the table already exists like its sister CreateWorkspaceFeatureClassAndAddToTOC does.</remarks>
    Public Shared Function CreateWorkspaceTableAndAddToTOC(tableName As String, tblGenericName As String, tablePrefix As String) As ITable
        Dim featWs As IFeatureWorkspace = Nothing
        Dim ws2 As IWorkspace2 = Nothing
        Dim tbl As ITable = Nothing

        'create the behavior classid for the table
        Dim pCLSID As New ESRI.ArcGIS.esriSystem.UID
        Try

            ws2 = CType(MapUtility.workspaceForLayer(tablePrefix), IWorkspace2)
            featWs = CType(ws2, IFeatureWorkspace)

            'check if  workspace is empty
            If featWs Is Nothing Then
                DSASUtility.log(TraceLevel.Error, "Feature Workspace is Nothing")
                Return Nothing
            End If
            'check if object class already exists
            If ws2.NameExists(esriDatasetType.esriDTTable, tableName) Then
                DSASUtility.log(TraceLevel.Info, tableName + " table Exists")
                Dim pDS As IDataset
                pDS = CType(featWs.OpenTable(tableName), IDataset)
                If pDS.CanDelete() Then
                    pDS.Delete()
                    DSASUtility.log(TraceLevel.Info, "Deleted " + tableName + " table.")
                Else
                    DSASUtility.log(TraceLevel.Error, "Could not delete " + tableName + " table.")
                    Return Nothing
                End If
            End If

            pCLSID.Value = "esriGeoDatabase.Object"

            'set the fields
            Dim m_flds As IFields = CreateFields(tblGenericName)

            'create the table
            tbl = featWs.CreateTable(tableName, m_flds, pCLSID, Nothing, "")
            If tbl Is Nothing Then
                log(TraceLevel.Error, tableName + " table could not be created!")
                Return Nothing
            End If
            MapUtility.AddTableToToc(tbl)

            tables(tblGenericName) = tbl
            Return tbl
        Finally
            ws2 = Nothing
            featWs = Nothing
        End Try
    End Function

    'AE: Inline class for sorting fields represented as Hashtables.
    Class OrderedFieldSort
        Implements IComparer
        Private tblKey As String

        Sub New(ByVal tblGenericName As String)
            tblKey = tblGenericName
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Return Integer.Parse(DirectCast(tblFldInfo(x.ToString, tblKey), Hashtable)("order").ToString) - Integer.Parse(DirectCast(tblFldInfo(y.ToString, tblKey), Hashtable)("order").ToString)
        End Function
    End Class

    Shared Function finalizeFieldName(ByVal genericName As String) As String
        Dim selectedCI As String
        selectedCI = My.Settings.Confidence_Interval.ToString
        selectedCI = selectedCI.Replace(".", "_")
        If genericName.Contains("%") Then genericName = genericName.Replace("%", selectedCI)
        Return genericName
    End Function


    ''' <summary>
    ''' Create geodatabase fields for feature class and tables
    ''' </summary>
    ''' <param name="tblGenericName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CreateFields(ByVal tblGenericName As String, Optional applyBias As Boolean = False) As IFields
        tblGenericName = tblGenericName.ToLower
        Dim flds As Hashtable = DirectCast(DirectCast(DSAS.Instance.dsasTables(tblGenericName), Hashtable)("fields"), Hashtable)
        Dim orderedFlds As New SortedList(flds, New OrderedFieldSort(tblGenericName))

        Dim m_flds As New Fields
        Dim fldsEdt As IFieldsEdit = CType(m_flds, IFieldsEdit)

        For Each entry As DictionaryEntry In orderedFlds
            Dim fld As Hashtable = DirectCast(entry.Value, Hashtable)
            log(fld("name").ToString)
            Dim fldOption As String = fld("option").ToString
            If fldOption = "" OrElse fldOption = "USER" OrElse fldOption = "GROUP" OrElse (fldOption = "BIAS" And applyBias) OrElse fldOption = "OUTPUT" Then
                Dim fldName As String = finalizeFieldName(fld("name").ToString)
                Dim fldTyp As esriFieldType = fieldName2Type(fld("type").ToString.Split(","c)(0))
                Dim geomDef As IGeometryDef
                'If this is a geometry field...
                If fldTyp = esriFieldType.esriFieldTypeGeometry Then
                    'then create geometry definition
                    Dim geomTyp As esriGeometryType = geometryName2Type(fld("geometry").ToString.Split(","c)(0))
                    geomDef = CreateGeometryDef(geomTyp, My.Document.FocusMap.SpatialReference)
                Else
                    geomDef = Nothing
                End If
                fldsEdt.AddField(CreateField(
                    fldName,
                    fld("alias").ToString,
                    fldTyp,
                    geomDef,
                    CType(fld("length").ToString.Split(","c)(0), Int32),
                    CType(fld("precision"), Int32),
                    CType(fld("scale"), Int32)))
            End If
        Next
        Return m_flds
    End Function

    Public Shared Function hasField(ByRef featureClass As IFeatureClass, ByVal fieldName As String) As Boolean
        Return featureClass.FindField(fieldName) <> -1
    End Function

    '
    'create a geodatabase field
    '
    Public Shared Function CreateField(
        ByVal fldName As String,
        ByVal fldAliasName As String,
        ByVal fldType As esriFieldType,
        ByVal fldGeomDef As IGeometryDef,
        ByVal fldLength As Int32,
        ByVal fldPrecision As Int32,
        ByVal fldScale As Int32
    ) As IField

        Dim fldEdt As IFieldEdit
        Dim fld As IField
        fld = New Field
        fldEdt = CType(fld, IFieldEdit)
        fldEdt.Name_2 = fldName
        fldEdt.AliasName_2 = fldAliasName
        fldEdt.Type_2 = fldType
        If fldGeomDef IsNot Nothing Then
            fldEdt.GeometryDef_2 = fldGeomDef
        End If
        If fldLength > 0 Then
            fldEdt.Length_2 = fldLength
        End If
        If fldPrecision > 0 Then
            fldEdt.Precision_2 = fldPrecision
        End If
        If fldScale > 0 Then
            fldEdt.Scale_2 = fldScale
        End If
        Return fld
    End Function

    ''' <summary>
    ''' Helper function to create a field for holding statistics values.
    ''' </summary>
    ''' <param name="fldName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CreateStatField(ByVal fldName As String) As IField
        Return CreateField(fldName, "", esriFieldType.esriFieldTypeDouble, Nothing, 0, 20, 10)
    End Function


    '
    'create feature class geometry definition
    '
    Public Shared Function CreateGeometryDef(ByVal inEsriGeometryType As esriGeometryType, ByVal inSpatialReference As ISpatialReference) As IGeometryDef
        'check if there is a valid spatial reference
        If inSpatialReference Is Nothing Then
            log(TraceLevel.Error, "Spatial reference for geometry not found! Can not proceed.")
            Return Nothing
        End If
        CreateGeometryDef = New GeometryDef
        Dim geomDefEdt As IGeometryDefEdit = CType(CreateGeometryDef, IGeometryDefEdit)
        geomDefEdt.GeometryType_2 = inEsriGeometryType
        geomDefEdt.GridCount_2 = 1
        'geomDefEdt.GridSize_2(0) = 10 'TODO: disabled during switch to .NET 4.5
        CSharpHelper.IGeometryDefEdit_set_GridSize_2(geomDefEdt, 0, 10)
        geomDefEdt.AvgNumPoints_2 = 2
        geomDefEdt.HasM_2 = False
        geomDefEdt.HasZ_2 = False
        geomDefEdt.SpatialReference_2 = inSpatialReference
    End Function


    ''
    ''insert intersect table values
    ''
    'Private Shared Sub insertIntersectRows(ByVal transId As Integer, ByRef valueList As ArrayList)
    '    Dim i As Integer
    '    Dim baseOid As Integer
    '    Dim shoreIds As ArrayList
    '    Dim distance As ArrayList
    '    Dim xcoord As ArrayList
    '    Dim ycoord As ArrayList
    '    Dim fldNames As ArrayList = GeoDB.getFieldNames("intersect")

    '    Dim fldIndex(fldNames.Count - 1) As Int32
    '    Dim tabRow As IRow
    '    'check if the table is properly set
    '    If tables("Intersect") Is Nothing Then Exit Sub
    '    i = valueList.Count
    '    baseOid = CType(valueList(i - 5), Integer)
    '    shoreIds = CType(valueList(i - 4), ArrayList)
    '    distance = CType(valueList(i - 3), ArrayList)
    '    xcoord = CType(valueList(i - 2), ArrayList)
    '    ycoord = CType(valueList(i - 1), ArrayList)
    '    'create and store records
    '    'set the field indexes
    '    For i = 0 To fldNames.Count - 1
    '        fldIndex(i) = tables("Intersect").Fields.FindField(CType(fldNames(i), String))
    '    Next
    '    For i = 0 To shoreIds.Count - 1
    '        tabRow = tables("Intersect").CreateRow
    '        'add the attributes
    '        With tabRow
    '            .Value(fldIndex(1)) = transId
    '            .Value(fldIndex(2)) = baseOid
    '            .Value(fldIndex(3)) = shoreIds(i)
    '            .Value(fldIndex(4)) = distance(i)
    '            .Value(fldIndex(5)) = xcoord(i)
    '            .Value(fldIndex(6)) = ycoord(i)
    '        End With
    '        'save the values
    '        tabRow.Store()
    '        tabRow = Nothing
    '    Next
    'End Sub

    'Private Shared Sub insertBiasDistRows(ByVal transOid As Integer, ByRef valueList As ArrayList)
    '    Dim shoreIds As ArrayList
    '    Dim distance As ArrayList
    '    Dim distance1 As ArrayList
    '    Dim distance2 As ArrayList
    '    Dim tabRow As IRow
    '    'check if the table is properly set
    '    If tables("BiasInters") Is Nothing Then Exit Sub
    '    shoreIds = CType(valueList(0), ArrayList)
    '    distance = CType(valueList(1), ArrayList)
    '    If valueList.Count > 2 Then
    '        distance1 = CType(valueList(2), ArrayList)
    '        distance2 = CType(valueList(3), ArrayList)
    '    End If
    '    'create and store records
    '    For i As Integer = 0 To shoreIds.Count - 1
    '        tabRow = tables("BiasInters").CreateRow
    '        'set the field indexes
    '        'add the attributes
    '        With tabRow
    '            .Value(1) = transOid
    '            .Value(2) = shoreIds(i)
    '            .Value(3) = distance(i)
    '            If valueList.Count > 2 Then
    '                .Value(4) = distance1(i)
    '                .Value(5) = distance2(i)
    '            End If
    '        End With
    '        'save the values
    '        tabRow.Store()
    '        DSASUtility.ReleaseComObject(tabRow)
    '    Next
    'End Sub
    ''Insert rows into the bias table
    'Private Shared Sub insertBiasRows(ByVal transOid As Integer, _
    'ByRef valueList As ArrayList)
    '    Dim tabRow As IRow
    '    If tables("Bias") Is Nothing Then Exit Sub
    '    If valueList Is Nothing Then Exit Sub
    '    'create new row
    '    tabRow = tables("Bias").CreateRow
    '    'add all values
    '    With tabRow
    '        .Value(1) = transOid
    '        For i As Integer = 0 To valueList.Count - 1
    '            .Value(i + 2) = valueList(i)
    '        Next
    '    End With
    '    'save the values
    '    tabRow.Store()
    '    DSASUtility.ReleaseComObject(tabRow)
    'End Sub

    ''insert rows into the distance table
    'Private Shared Sub insertDistanceRows(ByVal transOid As Integer, ByRef valueList As ArrayList)
    '    Dim tabRow As IRow
    '    Dim intBaseID As Integer = CType(valueList(0), Integer)
    '    Dim shoreIDs As ArrayList = CType(valueList(1), ArrayList)
    '    If tables("Distance") Is Nothing Then Exit Sub
    '    If valueList Is Nothing Then Exit Sub
    '    'create new row
    '    tabRow = tables("Distance").CreateRow

    '    With tabRow
    '        .Value(1) = transOid
    '        .Value(2) = intBaseID
    '        .Value(3) = String.Join(",", DirectCast(shoreIDs.ToArray(GetType(String)), String()))
    '        For i As Integer = 2 To valueList.Count - 1
    '            .Value(i + 2) = valueList(i)
    '        Next
    '    End With
    '    'save the values
    '    tabRow.Store()
    '    DSASUtility.ReleaseComObject(tabRow)
    'End Sub


    Public Shared Sub doInsert(
        ByVal inPolyline As IPolyline,
        ByVal group As Long,
        ByVal transEdit As Int32,
        ByVal baseVal As Int32,
        ByVal transOrder As Integer,
        ByVal xectFc As IFeatureClass
    )

        Dim feat As IFeature = xectFc.CreateFeature()
        feat.Shape = inPolyline

        Dim fldIdx As Integer

        fldIdx = feat.Fields.FindField("BaselineId")
        If fldIdx > -1 Then feat.Value(fldIdx) = baseVal

        If My.Settings.Baseline_Group_Field > "" Then
            fldIdx = feat.Fields.FindField("GroupId")
            If fldIdx > -1 Then feat.Value(fldIdx) = group
        End If

        fldIdx = feat.Fields.FindField("TransOrder")
        If fldIdx > -1 Then feat.Value(fldIdx) = transOrder

        fldIdx = feat.Fields.FindField("TransEdit")
        If fldIdx > -1 Then feat.Value(fldIdx) = transEdit

        fldIdx = feat.Fields.FindField("Azimuth")
        If fldIdx > -1 AndAlso inPolyline IsNot Nothing Then feat.Value(fldIdx) = DSASUtility.round(DSASUtility.CalculateAzimuth(inPolyline))

        feat.Store()
    End Sub


    'Public Shared Function SetWorkspaceTables(ByVal xFeatLyrNm As String, ByVal doBias As Boolean) As Boolean
    '    Return _
    '        SetWorkspaceTable(xFeatLyrNm & "_Distance", tables("Distance")) AndAlso _
    '        SetWorkspaceTable(xFeatLyrNm & "_Intersect", tables("Intersect")) AndAlso _
    '        (Not doBias OrElse _
    '            (SetWorkspaceTable(xFeatLyrNm & "_BiasInters", tables("BiasInters")) AndAlso _
    '            SetWorkspaceTable(xFeatLyrNm & "_Bias", tables("Bias"))))
    'End Function

    'Shared Function SetWorkspaceTable(ByVal tableName As String, ByRef tbl As ITable) As Boolean
    '    Dim msg As String
    '    If (DirectCast(m_ws, IWorkspace2).NameExists(esriDatasetType.esriDTTable, tableName)) Then
    '        tbl = DirectCast(m_ws, IFeatureWorkspace).OpenTable(tableName)
    '        If tbl IsNot Nothing Then
    '            Return True
    '        Else
    '            msg = "Table " & tableName & " not created properly."
    '        End If
    '    Else
    '        msg = "Table " & tableName & " not found in the geodatabase."
    '    End If

    '    DSASUtility.log(TraceLevel.Info, msg)
    '    MsgBox(msg, MsgBoxStyle.Information, "Set Workspace Table")
    '    Return False
    'End Function

    'Public Shared Function PropagateDeletions(ByVal inTransLyr As IFeatureLayer) As Integer
    '    Dim qFltr As IQueryFilter
    '    Dim cntDeleted As Integer = 0

    '    If tables("Intersect") Is Nothing Then Exit Function
    '    'define query to select rows where
    '    'transect id equals the value provided (transoid)
    '    qFltr = New QueryFilterClass
    '    Dim pTransCursor As IFeatureCursor
    '    Dim pRow As IRow
    '    Dim pCursor As ICursor
    '    Dim intTransID As Integer, intVal As Integer, prevVal As Integer = -1
    '    pCursor = tables("Intersect").Search(Nothing, False)
    '    pRow = pCursor.NextRow
    '    intTransID = pCursor.Fields.FindField("TransectID")
    '    While pRow IsNot Nothing
    '        intVal = CType(pRow.Value(intTransID), Integer)
    '        'Delete from tables only if we haven't seen this transect id before.
    '        If intVal <> prevVal Then
    '            prevVal = intVal
    '            qFltr.WhereClause = "[OBJECTID]=" & intVal
    '            DSASUtility.log(TraceLevel.Error, qFltr.WhereClause)
    '            pTransCursor = inTransLyr.FeatureClass.Search(qFltr, False)
    '            If pTransCursor.NextFeature Is Nothing Then
    '                cntDeleted += 1
    '                qFltr.WhereClause = "[TransectID] = " & intVal
    '                DSASUtility.log(TraceLevel.Error, "Deleting " & intVal & " from intersect table.")
    '                tables("Intersect").DeleteSearchedRows(qFltr)
    '                If tables("Distance") IsNot Nothing Then
    '                    DSASUtility.log(TraceLevel.Error, "Deleting " & intVal & " from distance table.")
    '                    tables("Distance").DeleteSearchedRows(qFltr)
    '                End If
    '                If tables("Bias") IsNot Nothing Then
    '                    DSASUtility.log(TraceLevel.Error, "Deleting " & intVal & " from bias table.")
    '                    tables("Bias").DeleteSearchedRows(qFltr)
    '                End If
    '                If tables("BiasInters") IsNot Nothing Then
    '                    DSASUtility.log(TraceLevel.Error, "Deleting " & intVal & " from bias distance table.")
    '                    tables("BiasInters").DeleteSearchedRows(qFltr)
    '                End If
    '            End If
    '            DSASUtility.ReleaseComObject(pTransCursor)
    '        End If
    '        pRow = pCursor.NextRow
    '    End While
    '    Return cntDeleted
    'End Function
    '
    'find and return the index of a field in a feature layer
    'Returns -1 if the field is not found
    'or the feature layer is nothing
    '
    Public Shared Function GetFieldIndex(ByVal featLayer As IFeatureLayer, ByVal inFldName As String) As Integer
        Dim idx As Integer = GetTableFieldIndex(DirectCast(featLayer, ITable), inFldName)
        If idx = -1 Then
            ' It might be a joined table
            inFldName = String.Format("{0}.{1}", featLayer.Name, inFldName)
            idx = GetTableFieldIndex(DirectCast(featLayer, ITable), inFldName)
        End If
        Return idx
    End Function

    Public Shared Function GetTableFieldIndex(ByVal inTable As ITable, ByVal inFldName As String) As Integer
        inFldName = inFldName.ToUpper
        Dim flds As IFields = inTable.Fields

        For i As Integer = 0 To flds.FieldCount - 1
            ' Handle differently if name has wildcard %
            If inFldName.Length > 1 AndAlso inFldName.EndsWith("%") Then
                Dim tblFldName As String = flds.Field(i).Name.ToUpper
                If tblFldName.StartsWith(inFldName.Substring(0, inFldName.Length - 1)) Then Return i
            Else
                If flds.Field(i).Name.ToUpper = inFldName Then Return i
            End If
        Next

        Return -1
    End Function

    'Public Shared Function GetLCIName(ByVal featLayer As IFeatureLayer) As String
    '    Dim fldName As String, fldIndex As Integer
    '    Dim lyrFields As ILayerFields = CType(featLayer, ILayerFields)

    '    For Each fldName In New String() {"LCI", "LCI90", "LCI95", "LCI98", "LCI99", "LCI99_8", "LCI99_9"}
    '        fldIndex = lyrFields.FindField(fldName)
    '        If fldIndex > -1 Then
    '            Return fldName.Substring(3).Replace("_"c, "."c)
    '        End If
    '    Next
    '    Return ""
    'End Function


    '
    'find and return the index of a field in a feature layer
    'Returns -1 if the field is not found
    'or the feature layer is nothing
    '
    Public Shared Function GetFieldType(ByVal featLayer As IFeatureLayer, ByVal inFldIndex As Int32) As Int32
        Dim fldtype As esriFieldType
        Dim lyrFields As ILayerFields

        If (featLayer Is Nothing) Then
            DSASUtility.log(TraceLevel.Error, "Feature layer " + featLayer.Name + " not found.")
            Return -1
        Else
            'get the fields collection
            lyrFields = CType(featLayer, ILayerFields)
            fldtype = lyrFields.Field(inFldIndex).Type()
        End If
        Return fldtype
    End Function

    Public Shared Function fieldType2Name(ByVal typ As Integer) As String
        Select Case typ
            Case esriFieldType.esriFieldTypeDouble
                Return "Double"
            Case esriFieldType.esriFieldTypeGeometry
                Return "Geometry"
            Case esriFieldType.esriFieldTypeInteger
                Return "Integer"
            Case esriFieldType.esriFieldTypeOID
                Return "OID"
            Case esriFieldType.esriFieldTypeSingle
                Return "Single"
            Case esriFieldType.esriFieldTypeSmallInteger
                Return "SmallInteger"
            Case esriFieldType.esriFieldTypeString
                Return "String"
            Case Else
                Return "Unknown"
        End Select
    End Function

    Public Shared Function fieldName2Type(ByVal name As String) As esriFieldType

        Select Case name
            Case "Double"
                Return esriFieldType.esriFieldTypeDouble
            Case "Geometry"
                Return esriFieldType.esriFieldTypeGeometry
            Case "Integer"
                Return esriFieldType.esriFieldTypeInteger
            Case "OID"
                Return esriFieldType.esriFieldTypeOID
            Case "Single"
                Return esriFieldType.esriFieldTypeSingle
            Case "SmallInteger"
                Return esriFieldType.esriFieldTypeSmallInteger
            Case "String"
                Return esriFieldType.esriFieldTypeString
            Case Else
                Return Nothing
        End Select
    End Function


    ''' <summary>
    ''' Delete a field from a feature layer
    ''' </summary>
    ''' <param name="lyr">Layer to delete field from</param>
    ''' <param name="fldIdx">The index of the field to delete</param>
    ''' <remarks></remarks>
    Private Shared Sub deleteField(ByVal lyr As IFeatureLayer, ByVal fldIdx As Integer)
        Dim fc As IFeatureClass = lyr.FeatureClass
        Dim fld As IField = fc.Fields.Field(fldIdx)
        fc.DeleteField(fld)
    End Sub

    Public Shared Function ifLayerContainsFields(ByRef featLyr As IFeatureLayer, ByVal fields As Hashtable) As Boolean

        For Each field As DictionaryEntry In fields
            If GetFieldIndex(featLyr, field.Key.ToString()) <> -1 Then
                Continue For
            End If

            Return False
        Next

        Return True

    End Function


    Shared Function listFromCsv(csv As Object) As List(Of String)
        listFromCsv = New List(Of String)
        listFromCsv.AddRange(csv.ToString.Trim.Split(","c))
        If listFromCsv.Item(0) = "" Then listFromCsv.RemoveAt(0)
    End Function


    Shared Function layerIsValid(ByVal lyr As IFeatureLayer, ByVal lyrGenericName As String, Optional ByVal mode As DSAS.layerCheckingMode = DSAS.layerCheckingMode.validation) As Boolean
        Try
            Dim lyrName As String = lyr.Name
            lyrGenericName = lyrGenericName.ToLower
            If lyr Is Nothing OrElse Not lyr.Valid Then Return False

            DSASUtility.log(TraceLevel.Info, "".PadLeft(40, "-"c))
            If mode = DSAS.layerCheckingMode.detection Then
                DSASUtility.log(TraceLevel.Verbose, "Detecting " + lyrGenericName + " layer: " + lyr.Name)
            Else
                DSASUtility.log(TraceLevel.Info, "Validating " + lyrGenericName + " layer: " + lyr.Name)
            End If
            Dim fields As Hashtable = DirectCast(DirectCast(DSAS.Instance.dsasTables(lyrGenericName), Hashtable)("fields"), Hashtable)

            For Each fld As Hashtable In fields.Values

                Dim fldName As String = fld("name").ToString
                If mode = DSAS.layerCheckingMode.validation Then DSASUtility.log(TraceLevel.Verbose, "Finding field: " + fldName)
                Dim fldOptionList = listFromCsv(fld("option"))
                ' If the field name is user specified, get the name from settings
                If fldOptionList.Contains("USER") Then
                    'Check to see if we can find the field name from user settings
                    fldName = DSASUtility.nv(My.Settings(String.Join("_", New String() {lyrGenericName, fldName, "field"})), "").ToString
                End If
                Dim fldTypes = listFromCsv(fld("type"))
                Dim fldIdx As Integer = GetFieldIndex(lyr, fldName)
                fld("idx") = fldIdx
                If fldIdx = -1 Then
                    'If this was a user specified field that wasn't selected, use the generic field name
                    fldName = IIf(fldName.Trim = "", fld("name"), fldName).ToString
                    ' If a field is not found, fail if the field is not optional
                    If mode = DSAS.layerCheckingMode.detection Then
                        If fldOptionList.Count = 0 Then
                            Return False
                        End If
                    Else    ' validation
                        If fldOptionList.Contains("USER") AndAlso (
                            (lyrGenericName = "baseline" AndAlso My.Settings.Baseline_ID_Field IsNot Nothing AndAlso fldName.ToLower = My.Settings.Baseline_ID_Field.ToLower) OrElse
                            (lyrGenericName = "baseline" AndAlso My.Settings.Baseline_Group_Field IsNot Nothing AndAlso fldName.ToLower = My.Settings.Baseline_Group_Field.ToLower) OrElse
                            (lyrGenericName = "shoreline" AndAlso My.Settings.Shoreline_Date_Field IsNot Nothing AndAlso fldName.ToLower = My.Settings.Shoreline_Date_Field.ToLower) OrElse
                            (lyrGenericName = "shoreline" AndAlso My.Settings.Shoreline_Uncertainty_Field IsNot Nothing AndAlso fldName.ToLower = My.Settings.Shoreline_Uncertainty_Field.ToLower) OrElse
                            (lyrGenericName = "shoreline" AndAlso My.Settings.Shoreline_Type_Field IsNot Nothing AndAlso fldName.ToLower = My.Settings.Shoreline_Type_Field.ToLower)
                            ) Then
                            DSASUtility.log(TraceLevel.Error, "Optional field not found in " + lyrGenericName + " layer: " + lyr.Name + "." + fldName)
                            Return False
                        ElseIf fldOptionList.Count = 0 Then
                            DSASUtility.log(TraceLevel.Error, "Required field not found in " + lyrGenericName + " layer: " + lyr.Name + "." + fldName)
                            Return False
                        End If
                    End If
                Else
                    If mode = DSAS.layerCheckingMode.validation Then DSASUtility.log(TraceLevel.Verbose, "Field found: " + fldName)

                    If mode = DSAS.layerCheckingMode.validation Then DSASUtility.log(TraceLevel.Verbose, "Confirming field type: " + fldName)
                    'AE: Handle multiple types
                    If Not fldTypes.Contains(fieldType2Name(GetFieldType(lyr, fldIdx))) Then
                        Dim msg = fldName + " field in " + lyr.Name + " layer should be of type " + String.Join(", ", fldTypes)
                        If mode = DSAS.layerCheckingMode.validation Then
                            log(TraceLevel.Error, msg)
                        Else
                            log(TraceLevel.Verbose, msg)
                        End If
                        Return False
                    ElseIf ("esriFieldType" + fldTypes(0)).ToUpper = esriFieldType.esriFieldTypeGeometry.ToString.ToUpper Then
                        Dim geomTypes = listFromCsv(fld("geometry"))
                        If Not geomTypes.Contains(geometryType2Name(lyr.FeatureClass.ShapeType)) Then
                            Dim msg = lyrGenericName + " layer " + lyr.Name + " layer should have " + String.Join(" or ", geomTypes) + " geometry"
                            If mode = DSAS.layerCheckingMode.validation Then
                                log(TraceLevel.Error, msg)
                            Else
                                log(TraceLevel.Verbose, msg)
                            End If
                            Return False
                        Else
                            If mode = DSAS.layerCheckingMode.validation Then DSASUtility.log(TraceLevel.Verbose, "Confirmed Layer Geometry.......................")
                        End If
                    End If
                End If
            Next
            Return True
        Catch ex As Exception
            If mode = DSAS.layerCheckingMode.validation Then DSASUtility.log(TraceLevel.Error, "Unexpected error while validating " + lyrGenericName + " layer: " + lyr.Name)
            Return False
        Finally
            DSASUtility.log(TraceLevel.Info, "".PadLeft(40, "-"c))
        End Try
    End Function

    Private Shared Function geometryName2Type(ByVal name As String) As esriGeometryType
        name = name.ToLower
        If name = "line" Then
            Return esriGeometryType.esriGeometryLine
        ElseIf name = "polyline" Then
            Return esriGeometryType.esriGeometryPolyline
        ElseIf name = "point" Then
            Return esriGeometryType.esriGeometryPoint
        ElseIf name = "polygon" Then
            Return esriGeometryType.esriGeometryPolygon
        Else
            Return Nothing
        End If
    End Function

    Private Shared Function geometryType2Name(ByVal typ As esriGeometryType) As String
        If typ = esriGeometryType.esriGeometryLine Then
            Return "Line"
        ElseIf typ = esriGeometryType.esriGeometryPolyline Then
            Return "Polyline"
        ElseIf typ = esriGeometryType.esriGeometryPoint Then
            Return "Point"
        ElseIf typ = esriGeometryType.esriGeometryPolygon Then
            Return "Polygon"
        Else
            Return "Unknown"
        End If
    End Function

    Shared Function baseFldMap(ByVal fldName As String) As Integer
        Return tblFldMap(fldName, "baseline")
    End Function

    Shared Function shoreFldMap(ByVal fldName As String) As Integer
        Return tblFldMap(fldName, "shoreline")
    End Function

    Shared Function transFldMap(ByVal fldName As String) As Integer
        Return tblFldMap(fldName, "transect")
    End Function

    Shared Function intersFldMap(ByVal fldName As String) As Integer
        Return tblFldMap(fldName, "intersect")
    End Function

    Shared Function tblFldInfo(ByVal fldName As String, ByVal tblGenericName As String) As Hashtable
        Try
            Return DirectCast(DirectCast(DirectCast(DSAS.Instance.dsasTables(tblGenericName.ToLower), Hashtable)("fields"), Hashtable)(fldName.ToLower), Hashtable)
        Catch
            DSASUtility.log(TraceLevel.Error, "Can't find: " + tblGenericName + "." + fldName)
        End Try
        Return Nothing
    End Function

    Shared Function tblFldMap(ByVal fldName As String, ByVal tblGenericName As String) As Integer
        Try
            Return DirectCast(tblFldInfo(fldName, tblGenericName)("idx"), Integer)
        Catch
            DSASUtility.log(TraceLevel.Info, "Can't find idx for: " + tblGenericName + "." + fldName)
        End Try
        Return -1
    End Function



    ''' <summary>
    ''' Check if a table and a field in it exists using an OleDB connection.
    ''' </summary>
    ''' <param name="tblName"></param>
    ''' <param name="fldName"></param>
    ''' <returns></returns>
    Public Shared Function checkIfTableFieldExists(ByVal tblName As String, ByVal fldName As String) As Boolean
        Dim cn As OleDbConnection = Nothing
        Dim cmd As OleDbCommand = Nothing

        Try
            cn = MapUtility.dbConnectionForLayer(tblName)
            If cn Is Nothing Then Return False
            tblName = getSourceTableName(tblName)
            Dim sql As String = String.Format("select {0} from [{1}] where 1=0", IIf(nv(fldName) = "", "*", "[" + fldName + "]"), tblName)
            cmd = New OleDbCommand(sql, cn)
            cmd.ExecuteReader().Close()
            ' If we got here without error, table is there.
            Return True
        Catch ex As Exception
            safeCloseConnection(cn)
        End Try
    End Function

    ''' <summary>
    ''' Returns the number of records with null value in the specified table/field.
    ''' </summary>
    ''' <param name="tblName"></param>
    ''' <param name="fldName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function countNulls(ByVal tblName As String, ByVal fldName As String) As Integer
        Dim cn As OleDbConnection = Nothing
        Dim cmd As OleDbCommand
        Dim dr As OleDbDataReader
        Dim sql As String
        Dim cnt As Integer
        Try
            cn = MapUtility.dbConnectionForLayer(tblName)
            If cn Is Nothing Then Return 0
            tblName = getSourceTableName(tblName)
            sql =
                    "SELECT count(*) as cnt " &
                    "FROM [" + tblName + "] " &
                    "WHERE [" + fldName + "] is null"
            cmd = New OleDbCommand(sql, cn)
            dr = cmd.ExecuteReader
            dr.Read()
            cnt = CInt(dr("cnt"))
            dr.Close()
            Return cnt
        Finally
            If cn IsNot Nothing Then cn.Close()
        End Try
    End Function

    ''' <summary>
    ''' Returns the number of records that match the give SQL where clause.
    ''' </summary>
    ''' <param name="tblName"></param>
    ''' <param name="whereClause"></param>
    ''' <returns></returns>
    Public Shared Function countMatching(tblName As String, whereClause As String) As Integer
        Dim cn As OleDbConnection = Nothing
        Dim cmd As OleDbCommand
        Dim dr As OleDbDataReader
        Dim sql As String
        Dim cnt As Integer
        Try
            cn = MapUtility.dbConnectionForLayer(tblName)
            If cn Is Nothing Then Return 0
            tblName = getSourceTableName(tblName)
            sql =
                    "SELECT count(*) as cnt " &
                    "FROM [" + tblName + "] " &
                    "WHERE " & whereClause
            cmd = New OleDbCommand(sql, cn)
            dr = cmd.ExecuteReader
            dr.Read()
            cnt = CInt(dr("cnt"))
            dr.Close()
            Return cnt
        Finally
            If cn IsNot Nothing Then cn.Close()
        End Try
    End Function


    ''' <summary>
    ''' Checks for manually edited transects that either don't have a TransOrder
    ''' or have one that is a duplicate of another transect in the layer
    ''' </summary>
    ''' <param name="tblName"></param>
    ''' <returns></returns>
    Public Shared Function checkForTransOrderAnomaly(tblName As String) As Integer
        Dim cn As OleDbConnection = Nothing
        Dim cmd As OleDbCommand
        Dim dr As OleDbDataReader
        Dim sql As String
        Dim cnt As Integer
        Try
            cn = MapUtility.dbConnectionForLayer(tblName)
            If cn Is Nothing Then Return 0
            tblName = getSourceTableName(tblName)
            sql =
                    "SELECT count(*) as cnt " &
                    "FROM [" + tblName + "] " &
                    "WHERE (TransEdit IS NULL OR TransEdit <> '0') " &
                    "AND (TransOrder IS NULL OR TransOrder IN (SELECT TransOrder  FROM [" + tblName + "] WHERE TransOrder IS NOT NULL GROUP BY TransOrder HAVING COUNT(*)>1))"
            cmd = New OleDbCommand(sql, cn)
            dr = cmd.ExecuteReader
            dr.Read()
            cnt = CInt(dr("cnt"))
            dr.Close()
            Return cnt
        Catch ex As Exception
        Finally
            If cn IsNot Nothing Then cn.Close()
        End Try
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="tblName"></param>
    ''' <param name="fldName"></param>
    ''' <param name="valueAtCount"></param>
    ''' <param name="positive"></param>
    ''' <returns></returns>
    Public Shared Function valueAtPercent(tblName As String, fldName As String, valueAtCount As Long, positive As Boolean) As Double
        Dim whereClause As String = ""
        Dim orderDirection As String = ""
        If positive Then
            whereClause = String.Format("[{0}] >= 0", fldName)
            orderDirection = "ASC"
        Else
            whereClause = String.Format("[{0}] <= 0", fldName)
            orderDirection = "DESC"
        End If

        Dim cn As OleDbConnection = Nothing
        Dim cmd As OleDbCommand
        Dim dr As OleDbDataReader
        Dim sql As String
        Try
            cn = MapUtility.dbConnectionForLayer(tblName)
            If cn Is Nothing Then Return 0
            tblName = getSourceTableName(tblName)
            sql = String.Format("SELECT [{0}] as v FROM [{1}] WHERE {2} ORDER BY [{0}] {3}", fldName, tblName, whereClause, orderDirection)
            cmd = New OleDbCommand(sql, cn)
            dr = cmd.ExecuteReader
            Dim ctr As Long = 0
            Do While dr.Read() AndAlso ctr <= valueAtCount
                ctr += 1
                valueAtPercent = CType(dr("v"), Double)
            Loop
            dr.Close()
        Finally
            If cn IsNot Nothing Then cn.Close()
        End Try
    End Function

    Public Shared Function createIntersectFeature() As IFeature
        Return featureClassMap("intersect").CreateFeature()
    End Function

    Public Shared Function createIntersectRow() As IRow
        Return tables("intersect").CreateRow()
    End Function

    Public Shared Function geodbIsCurrent(ByVal iws As IWorkspace) As Boolean
        Dim iGdbRel As IGeodatabaseRelease = DirectCast(iws, IGeodatabaseRelease)
        Return iGdbRel IsNot Nothing AndAlso iGdbRel.CurrentRelease
    End Function

    Public Shared Function geodbIsCurrent(ByVal lyr As ILayer) As Boolean
        Return geodbIsCurrent(DirectCast(lyr, IDataset).Workspace)
    End Function

    Public Shared Function geodbIsCurrent(ByVal filePathname As String) As Boolean
        Dim iws As IWorkspace
        Try
            Dim awf As IWorkspaceFactory = DirectCast(getSingleton("esriDataSourcesGDB.AccessWorkspaceFactory"), IWorkspaceFactory)
            iws = awf.OpenFromFile(filePathname, 0)
            Return geodbIsCurrent(iws)
        Catch ex As Exception
        End Try
    End Function


    ''' <summary>
    ''' Get a feature cursor for a layer that is optionally sorted on specified fields 
    ''' and optionally filtered if a WHERE clause is provided.
    ''' </summary>
    ''' <param name="lyrId"></param>
    ''' <param name="sortFields"></param>
    ''' <param name="filter"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getCursorForLayer(ByVal lyrId As Object, Optional ByVal sortFields As String = Nothing, Optional ByVal filter As String = Nothing, Optional ByVal forUpdate As Boolean = False) As IFeatureCursor
        Dim fc As IFeatureClass = Nothing
        If lyrId.GetType Is GetType(String) Then
            fc = MapUtility.findFeatureLayer(DirectCast(lyrId, String)).FeatureClass
        ElseIf TryCast(lyrId, IFeatureClass) IsNot Nothing Then
            fc = DirectCast(lyrId, IFeatureClass)
        Else
            Return Nothing
        End If

        Dim iqf As QueryFilterClass = Nothing

        If filter IsNot Nothing AndAlso filter.Trim > "" Then
            iqf = New QueryFilterClass
            iqf.WhereClause = filter
        End If

        If sortFields IsNot Nothing AndAlso sortFields.Trim > "" Then
            Dim iTs As New TableSortClass
            iTs.Fields = sortFields
            iTs.Table = DirectCast(fc, ITable)
            If filter IsNot Nothing Then
                iTs.QueryFilter = iqf
            End If
            iTs.Sort(Nothing)
            Return DirectCast(iTs.Rows, IFeatureCursor)
        Else
            If forUpdate Then
                Return fc.Update(iqf, False)
            Else
                Return fc.Search(iqf, False)
            End If
        End If
    End Function


    Public Shared Iterator Function features(ByVal lyrId As Object, Optional sortFields As String = Nothing, Optional filter As String = Nothing, Optional forUpdate As Boolean = False) As IEnumerable(Of IFeature)
        Dim fc As IFeatureCursor = GeoDB.getCursorForLayer(lyrId, sortFields, filter, forUpdate)
        Dim feat As IFeature
        Do
            feat = fc.NextFeature
            If feat Is Nothing Then Exit Do
            Yield feat
        Loop
    End Function


    Public Shared Function copyXectFc(ByVal workspacePath As String, ByVal fcName As String) As String
        copyXectFc = Nothing
        ' Create new workspace name objects.
        Dim sourceWorkspaceName As IWorkspaceName = DirectCast(New WorkspaceName(), IWorkspaceName)
        Dim targetWorkspaceName As IWorkspaceName = DirectCast(New WorkspaceName(), IWorkspaceName)

        Dim targetName As IName = CType(targetWorkspaceName, IName)

        ' Set the workspace name properties.
        sourceWorkspaceName.PathName = workspacePath
        sourceWorkspaceName.WorkspaceFactoryProgID = "esriDataSourcesGDB.AccessWorkspaceFactory"
        targetWorkspaceName.PathName = workspacePath
        targetWorkspaceName.WorkspaceFactoryProgID = "esriDataSourcesGDB.AccessWorkspaceFactory"

        ' Create a name object for the source feature class.
        Dim featureClassName As IFeatureClassName = DirectCast(New FeatureClassName(), IFeatureClassName)

        ' Set the featureClassName properties.
        Dim sourceDatasetName As IDatasetName = CType(featureClassName, IDatasetName)
        sourceDatasetName.WorkspaceName = sourceWorkspaceName
        sourceDatasetName.Name = fcName

        Dim sourceName As IName = CType(sourceDatasetName, IName)

        ' Create an enumerator for source datasets.
        Dim sourceEnumName As IEnumName = New NamesEnumerator()

        Dim sourceEnumNameEdit As IEnumNameEdit = CType(sourceEnumName, IEnumNameEdit)

        ' Add the name object for the source class to the enumerator.
        sourceEnumNameEdit.Add(sourceName)

        ' Create a GeoDBDataTransfer object and a null name mapping enumerator.
        Dim geoDBDataTransfer As IGeoDBDataTransfer = New GeoDBDataTransfer()
        Dim enumNameMapping As IEnumNameMapping = Nothing

        ' Use the data transfer object to create a name mapping enumerator.
        Dim conflictsFound As Boolean = geoDBDataTransfer.GenerateNameMapping(sourceEnumName, targetName, enumNameMapping)
        enumNameMapping.Reset()

        If conflictsFound Then
            ' Iterate through each name mapping.
            Dim nameMapping As INameMapping = enumNameMapping.Next()
            Do While Not nameMapping Is Nothing
                ' Resolve the mapping's conflict (if there is one).
                If nameMapping.NameConflicts Then
                    nameMapping.TargetName = nameMapping.GetSuggestedName(targetName)
                    copyXectFc = nameMapping.TargetName
                End If

                ' See if the mapping's children have conflicts.
                Dim childEnumNameMapping As IEnumNameMapping = nameMapping.Children
                If Not childEnumNameMapping Is Nothing Then
                    childEnumNameMapping.Reset()

                    ' Iterate through each child mapping.
                    Dim childNameMapping As INameMapping = childEnumNameMapping.Next()
                    Do While Not childNameMapping Is Nothing
                        If childNameMapping.NameConflicts Then
                            childNameMapping.TargetName = childNameMapping.GetSuggestedName(targetName)
                        End If

                        childNameMapping = childEnumNameMapping.Next()
                    Loop
                End If

                nameMapping = enumNameMapping.Next()
            Loop
        End If

        ' Start the transfer.
        geoDBDataTransfer.Transfer(enumNameMapping, targetName)

    End Function


    Public Shared Function findDatasetInWorkspace(ByVal pName As String, ByVal pWorkspace As IWorkspace, Optional ByVal pDatatype As esriDatasetType = esriDatasetType.esriDTFeatureClass) As IDataset

        Dim pDataset As IDataset
        Dim pEnumDataset As IEnumDataset

        pEnumDataset = pWorkspace.Datasets(pDatatype)
        Do
            pDataset = pEnumDataset.Next
            If pDataset Is Nothing Then Exit Do
            If pDataset.Name = pName Then Return pDataset
        Loop
        Return Nothing
    End Function

    Public Shared Function isInPersonalGeodatabaseWorkspace(o As Object) As Boolean
        Dim iws As IWorkspace = Nothing
        Try
            iws = DirectCast(o, IDataset).Workspace
            Return iws.Type = esriWorkspaceType.esriLocalDatabaseWorkspace AndAlso Not iws.IsDirectory AndAlso iws.PathName.ToLower.EndsWith(".mdb")
        Catch ex As Exception
            Dim a = 1
        Finally
            iws = Nothing
        End Try
    End Function

    Public Shared Sub AddFieldToFeatureClass(ByVal featureClass As IFeatureClass, ByVal field As IField, Optional lock As Boolean = True)

        Dim schemaLock As ISchemaLock = CType(featureClass, ISchemaLock)

        Try
            ' A try block is necessary, as an exclusive lock may not be available.
            If lock Then schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock)

            ' Add the field.
            featureClass.AddField(field)
        Catch exc As Exception
            ' Handle this in a way appropriate to your application.
            Console.WriteLine(exc.Message)
        Finally
            ' Set the lock to shared, whether or not an error occurred.
            If lock Then schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock)
        End Try

    End Sub

    Public Shared Sub AddFieldToTable(ByVal tbl As ITable, ByVal field As IField)
        Dim schemaLock As ISchemaLock = CType(tbl, ISchemaLock)

        ' A try block is necessary, as an exclusive lock may not be available.
        schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock)

        ' Add the field.
        tbl.AddField(field)
        ' Set the lock to shared, whether or not an error occurred.
        schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock)
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="layer_name"></param>
    ''' <returns>The name of the source table for given layer/table name in the map</returns>
    Public Shared Function getSourceTableName(ByVal layer_name As String) As String
        Dim lyr As IFeatureLayer = MapUtility.findFeatureLayer(layer_name)
        If lyr Is Nothing Then
            Return layer_name
        Else
            Return DirectCast(lyr, IDataset).BrowseName
        End If
    End Function


    Public Shared Function getFldExtreme(ByVal featClass As IFeatureClass, ByVal fldName As String, ByVal isMax As Boolean, Optional whereClause As String = "") As Double
        Dim ifc As IFeatureCursor = Nothing
        Try
            Dim pQueryFilter As IQueryFilter = New QueryFilter
            pQueryFilter.SubFields = fldName
            pQueryFilter.WhereClause = fldName + " IS NOT NULL"
            If whereClause > "" Then pQueryFilter.WhereClause = String.Format("({0}) AND ({1})", pQueryFilter.WhereClause, whereClause)

            Dim pQueryFilterDefinition As IQueryFilterDefinition
            pQueryFilterDefinition = DirectCast(pQueryFilter, IQueryFilterDefinition)

            If isMax Then
                pQueryFilterDefinition.PostfixClause = "ORDER BY " + fldName + " DESC"
            Else
                pQueryFilterDefinition.PostfixClause = "ORDER BY " + fldName + " ASC"
            End If

            ifc = featClass.Search(pQueryFilter, True)
            Dim feat As IFeature = ifc.NextFeature()
            While feat IsNot Nothing
                Dim val As Double = CDbl(feat.Value(ifc.FindField(fldName)))
                If Not Double.IsNaN(val) Then
                    Return val
                Else
                    feat = ifc.NextFeature()
                End If
            End While

            Return 0
        Catch ex As Exception
        Finally
            If ifc IsNot Nothing Then DSASUtility.ReleaseComObject(ifc)
        End Try

    End Function


    Public Shared Function getLayerTotalLength(lyr As IFeatureLayer) As Double
        Dim pFC As IFeatureClass = lyr.FeatureClass
        Dim dLength As Double = 0
        If TypeOf pFC Is IFeatureClass Then
            Dim pFeatureCursor As IFeatureCursor
            pFeatureCursor = pFC.Search(Nothing, False)
            If TypeOf pFeatureCursor Is IFeatureCursor Then
                Dim pFeature As IFeature = pFeatureCursor.NextFeature()
                Try
                    While pFeature IsNot Nothing
                        Dim pLine As IGeometry
                        pLine = pFeature.Shape
                        If TypeOf pLine Is IPolyline Then
                            dLength += DirectCast(pLine, IPolyline).Length
                            pFeature = pFeatureCursor.NextFeature
                        End If
                    End While
                Catch ex As Exception
                End Try
            End If
            pFeatureCursor.Flush()
        End If
        Return dLength
    End Function


    Public Shared Function ifLayerContainsFields(ByRef layer As IFeatureLayer, ByRef fields As List(Of String)) As Boolean

        Dim featClass As IFeatureClass = layer.FeatureClass
        For Each fieldName As String In fields
            If featClass.FindField(fieldName) = -1 Then
                Return False
            End If
        Next

        Return True

    End Function


    Public Shared Function isEsriNumericField(ByRef featClass As IFeatureClass, ByVal fieldName As String) As Boolean

        Dim fieldType As esriFieldType = featClass.Fields.Field(featClass.FindField(fieldName)).Type

        If (
            fieldType = esriFieldType.esriFieldTypeDouble Xor
            fieldType = esriFieldType.esriFieldTypeInteger Xor
            fieldType = esriFieldType.esriFieldTypeSingle Xor
            fieldType = esriFieldType.esriFieldTypeSmallInteger
        ) Then

            Return True

        End If

        Return False

    End Function

    Public Shared Function isEsriDoubleField(ByRef featClass As IFeatureClass, ByVal fieldName As String) As Boolean

        Dim fieldType As esriFieldType = featClass.Fields.Field(featClass.FindField(fieldName)).Type

        If fieldType = esriFieldType.esriFieldTypeDouble Then

            Return True

        End If

        Return False

    End Function

    Public Shared Function getFeatureClassPath(Optional ByRef featClass As IFeatureClass = Nothing) As String
        If featClass Is Nothing Then
            If TransectLyrToolCtl.currentTransectLayer Is Nothing Then
                Return ""
            End If
            featClass = TransectLyrToolCtl.currentTransectLayer.FeatureClass
        End If

        Dim iws As IWorkspace = Nothing
        Try
            iws = DirectCast(featClass, IDataset).Workspace
            Return System.IO.Path.GetDirectoryName(iws.PathName)
        Catch ex As Exception
            Dim a = 1
        Finally
            iws = Nothing
        End Try

        Return ""

    End Function



    Public Shared Sub AddIndexToFeatureClass(lyrGenericName As String, indexName As String, nameOfField As String)
        Dim featureClass As IFeatureClass = featureClassMap(lyrGenericName)

        ' Ensure the feature class contains the specified field.
        Dim fieldIndex As Integer = featureClass.FindField(nameOfField)
        If fieldIndex = -1 Then
            Throw New ArgumentException("The specified field does not exist in the feature class.")
        End If

        ' Get the specified field from the feature class.
        Dim featureClassFields As IFields = featureClass.Fields
        Dim field As IField = featureClassFields.Field(fieldIndex)

        ' Create a fields collection and add the specified field to it.
        Dim fields As IFields = New FieldsClass()
        Dim fieldsEdit As IFieldsEdit = CType(fields, IFieldsEdit)
        fieldsEdit.FieldCount_2 = 1
        CSharpHelper.IFieldsEdit_set_Field_2(fieldsEdit, 0, field)
        'fieldsEdit.Field_2(0) = field

        'Create an index and cast to the IIndexEdit interface.
        Dim index As IIndex = New IndexClass()
        Dim indexEdit As IIndexEdit = CType(index, IIndexEdit)

        ' Set the index's properties, including the associated fields.
        indexEdit.Fields_2 = fields
        indexEdit.IsAscending_2 = False
        indexEdit.IsUnique_2 = False
        indexEdit.Name_2 = indexName

        ' Attempt to acquire an exclusive schema lock on the feature class.
        Dim schemaLock As ISchemaLock = CType(featureClass, ISchemaLock)
        Try
            schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock)
            featureClass.AddIndex(index)
        Catch comExc As System.Runtime.InteropServices.COMException
            ' Handle this in a way appropriate to your application.
            Console.WriteLine("A COM Exception was thrown: {0}", comExc.Message)
        Finally
            schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock)
        End Try

    End Sub

End Class
