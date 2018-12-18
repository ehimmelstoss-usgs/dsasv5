Imports System.Linq
Imports System.Text.RegularExpressions
Imports System.Windows
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase

Public Class AttributeToolbarForm

    Private Enum LayerType
        Shoreline
        Baseline
    End Enum

    Protected Property shorelineLayerList As List(Of IFeatureLayer)
    Protected Property baselineLayerList As List(Of IFeatureLayer)

    Private Sub AttributeToolbarForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        shorelineLayerList = New List(Of IFeatureLayer)
        baselineLayerList = New List(Of IFeatureLayer)

        Me.initializeShorelineLayerList()
        Me.initializeBaselineLayerList()
    End Sub

    Private Sub initializeBaselineLayerList()
        For Each lyr As IFeatureLayer In MapUtility.featureLayers("baseline", False)
            Me.baselineLayerList.Add(lyr)
            baselineListCtrl.Items.Add(lyr.Name)
        Next
    End Sub

    Private Sub initializeShorelineLayerList()
        For Each lyr As IFeatureLayer In MapUtility.featureLayers("shoreline", False)
            Me.shorelineLayerList.Add(lyr)
            layerListControl.Items.Add(lyr.Name)
        Next
    End Sub

    Private Sub AddFieldsButton_Click(sender As Object, e As EventArgs) Handles AddFieldsButton.Click
        Me.askForAddFields(LayerType.Shoreline)
    End Sub

    Private Sub askForAddFields(ByVal layerType As LayerType)
        Dim result As Integer = MessageBox.Show("Are you sure you want to add fields to the selected layers?", "Confirm", MessageBoxButton.YesNo)

        If result = MessageBoxResult.Yes Then
            Select Case layerType
                Case LayerType.Shoreline
                    addShorelineFields()
                Case LayerType.Baseline
                    addBaselineFields()
            End Select
        End If
    End Sub

    Private Sub addBaselineFields()
        Dim selectedLayerNamesCollection As Forms.ListBox.SelectedObjectCollection = baselineListCtrl.SelectedItems()
        Dim selectedLayerNames As List(Of String) = New List(Of String)
        For Each layerName As String In selectedLayerNamesCollection
            selectedLayerNames.Add(layerName)
        Next

        If selectedLayerNames.Count = 0 Then
            MessageBox.Show("Please select one or more layer(s) to proceed.")
            Return
        End If

        Dim selectedLayers As IEnumerable(Of IFeatureLayer) = MapUtility.getFeatureLayersByNames(baselineLayerList, selectedLayerNames)

        Dim idFieldName As String = idFieldNameControl.Text
        Dim groupFieldName As String = groupFiledNameControl.Text
        Dim searchDistanceFieldName As String = searchDistanceFieldNameControl.Text

        Dim regexItem = New Regex("^[a-zA-Z0-9_ ]*$")

        Dim ifAddIdField As Boolean = ifAddIdFieldNameControl.Checked
        Dim ifAddGroupField As Boolean = ifAddGroupFieldNameControl.Checked
        Dim ifAddSearchDistanceName As Boolean = ifAddSearchDistanceNameControl.Checked

        If ((Not regexItem.IsMatch(idFieldName) And ifAddIdField) Xor (Not regexItem.IsMatch(groupFieldName) And ifAddGroupField) Xor (Not regexItem.IsMatch(searchDistanceFieldName) And ifAddSearchDistanceName)) Then
            MessageBox.Show("Only alphanumeric and underscores are allowed for field names.")
            Return
        End If

        If Not ifAddIdField And Not ifAddGroupField And Not ifAddSearchDistanceName Then
            MessageBox.Show("Please check one or more field(s) to proceed.")
            Return
        End If

        Dim editedLayerNames As List(Of String) = New List(Of String)
        Dim conflictDateLayerNames As List(Of String) = New List(Of String)
        Dim conflictUncyLayerNames As List(Of String) = New List(Of String)
        Dim conflictShorelineTypeLayerNames As List(Of String) = New List(Of String)
        For Each layer As IFeatureLayer In selectedLayers

            Try
                Dim ifAddField As Boolean = True

                If GeoDB.hasField(layer.FeatureClass, idFieldName) And ifAddIdField Then
                    ifAddField = False
                    conflictDateLayerNames.Add(layer.Name)
                End If

                If GeoDB.hasField(layer.FeatureClass, groupFieldName) And ifAddGroupField Then
                    ifAddField = False
                    conflictUncyLayerNames.Add(layer.Name)
                End If

                If GeoDB.hasField(layer.FeatureClass, searchDistanceFieldName) And ifAddSearchDistanceName Then
                    ifAddField = False
                    conflictShorelineTypeLayerNames.Add(layer.Name)
                End If

            Catch exc As Exception
                ' Handle this in a way appropriate to your application.
                Console.WriteLine(exc.Message)
            End Try

        Next

        Dim conflictLayerNamesStr As String = ""

        If conflictDateLayerNames.Count <> 0 Then
            Dim conflictDateLayerNamesStr = ""
            For i As Integer = 0 To conflictDateLayerNames.Count - 1
                If i = 0 Then
                    conflictDateLayerNamesStr += conflictDateLayerNames.Item(i)
                Else
                    conflictDateLayerNamesStr += ", " + conflictDateLayerNames.Item(i)
                End If
            Next
            conflictLayerNamesStr += "ID field name conflict detected for the following layer(s): " + conflictDateLayerNamesStr + "." + Environment.NewLine
        End If

        If conflictUncyLayerNames.Count <> 0 Then
            Dim conflictUncyLayerNamesStr = ""
            For i As Integer = 0 To conflictUncyLayerNames.Count - 1
                If i = 0 Then
                    conflictUncyLayerNamesStr += conflictUncyLayerNames.Item(i)
                Else
                    conflictUncyLayerNamesStr += ", " + conflictUncyLayerNames.Item(i)
                End If
            Next
            conflictLayerNamesStr += "Group field name conflict detected for the following layer(s): " + conflictUncyLayerNamesStr + "." + Environment.NewLine
        End If

        If conflictShorelineTypeLayerNames.Count <> 0 Then
            Dim conflictShorelineTypeLayerNamesStr = ""
            For i As Integer = 0 To conflictShorelineTypeLayerNames.Count - 1
                If i = 0 Then
                    conflictShorelineTypeLayerNamesStr += conflictShorelineTypeLayerNames.Item(i)
                Else
                    conflictShorelineTypeLayerNamesStr += ", " + conflictShorelineTypeLayerNames.Item(i)
                End If
            Next
            conflictLayerNamesStr += "Search Distance type field name conflict detected for the following layer(s): " + conflictShorelineTypeLayerNamesStr + "." + Environment.NewLine
        End If


        If conflictDateLayerNames.Count <> 0 OrElse conflictUncyLayerNames.Count <> 0 OrElse conflictShorelineTypeLayerNames.Count <> 0 Then
            log(TraceLevel.Error, "Conflict detected - action canceled." + vbCrLf +
                   "The attribute automator is unable to add the specified field(s) due to a conflict in the listed file(s) because the field name(s) already exists." + vbCrLf +
                   "To proceed you may specify a different field name or unselect all files that have a conflict." + vbCrLf +
                   vbCrLf +
                   conflictLayerNamesStr
            )

            Exit Sub
        End If

        For Each layer As IFeatureLayer In selectedLayers
            If (ifAddIdField) Then
                Dim idField As IField = GeoDB.CreateField(
                    idFieldName,
                    idFieldName,
                    esriFieldType.esriFieldTypeInteger,
                    Nothing,
                    8,
                    8,
                    0
                )

                GeoDB.AddFieldToFeatureClass(layer.FeatureClass, idField, False)
            End If

            If (ifAddGroupField) Then
                Dim groupField As IField = GeoDB.CreateField(
                    groupFieldName,
                    groupFieldName,
                    esriFieldType.esriFieldTypeInteger,
                    Nothing,
                    8,
                    8,
                    0
                )

                GeoDB.AddFieldToFeatureClass(layer.FeatureClass, groupField, False)
            End If

            If (ifAddSearchDistanceName) Then
                Dim searchDistanceField As IField = GeoDB.CreateField(
                    searchDistanceFieldName,
                    searchDistanceFieldName,
                    esriFieldType.esriFieldTypeDouble,
                    Nothing,
                    10,
                    9,
                    2
                )

                GeoDB.AddFieldToFeatureClass(layer.FeatureClass, searchDistanceField, False)
            End If

            editedLayerNames.Add(layer.Name)
        Next

        ' Automatically update project setting for baseline ID field if we just added one
        If ifAddIdField Then My.Settings.Baseline_ID_Field = idFieldName
        ' Automatically update project setting for shoreline ID field if we just added one
        If IfAddDateFieldControl.Checked Then My.Settings.Shoreline_Date_Field = dateFieldNameControl.Text
        Me.unselectLayerNames(editedLayerNames)
        MessageBox.Show("Fields are successfully added.")
    End Sub

    Private Sub addShorelineFields()
        Dim selectedLayerNamesCollection As Forms.ListBox.SelectedObjectCollection = layerListControl.SelectedItems()
        Dim selectedLayerNames As List(Of String) = New List(Of String)
        For Each layerName As String In selectedLayerNamesCollection
            selectedLayerNames.Add(layerName)
        Next

        If selectedLayerNames.Count = 0 Then
            MessageBox.Show("Please select one or more layer(s) to proceed.")
            Return
        End If

        Dim selectedLayers As IEnumerable(Of IFeatureLayer) = MapUtility.getFeatureLayersByNames(shorelineLayerList, selectedLayerNames)

        Dim dateFieldName As String = dateFieldNameControl.Text
        Dim uncertaintyFieldName As String = UncyFieldNameControl.Text
        Dim shorelineTypeFieldName As String = shorelineTypeFieldContrl.Text

        Dim regexItem = New Regex("^[a-zA-Z0-9_ ]*$")

        Dim ifAddDateField As Boolean = IfAddDateFieldControl.Checked
        Dim ifAddUncertaintyField As Boolean = IfAddUncyFieldControl.Checked
        Dim ifAddShorelineTypeField As Boolean = IfAddShorelineTypeFieldControl.Checked

        If ((Not regexItem.IsMatch(dateFieldName) And ifAddDateField) Xor (Not regexItem.IsMatch(uncertaintyFieldName) And ifAddUncertaintyField) Xor (Not regexItem.IsMatch(shorelineTypeFieldName) And ifAddShorelineTypeField)) Then
            MessageBox.Show("Only alphanumeric and underscores are allowed for field names.")
            Return
        End If

        If Not ifAddDateField And Not ifAddUncertaintyField And Not ifAddShorelineTypeField Then
            MessageBox.Show("Please check one or more field(s) to proceed.")
            Return
        End If

        Dim editedLayerNames As List(Of String) = New List(Of String)
        Dim conflictDateLayerNames As List(Of String) = New List(Of String)
        Dim conflictUncyLayerNames As List(Of String) = New List(Of String)
        Dim conflictShorelineTypeLayerNames As List(Of String) = New List(Of String)
        For Each layer As IFeatureLayer In selectedLayers

            Try
                Dim ifAddField As Boolean = True

                If GeoDB.hasField(layer.FeatureClass, dateFieldName) And ifAddDateField Then
                    ifAddField = False
                    conflictDateLayerNames.Add(layer.Name)
                End If

                If GeoDB.hasField(layer.FeatureClass, uncertaintyFieldName) And ifAddUncertaintyField Then
                    ifAddField = False
                    conflictUncyLayerNames.Add(layer.Name)
                End If

                If GeoDB.hasField(layer.FeatureClass, shorelineTypeFieldName) And ifAddShorelineTypeField Then
                    ifAddField = False
                    conflictShorelineTypeLayerNames.Add(layer.Name)
                End If

            Catch exc As Exception
                ' Handle this in a way appropriate to your application.
                Console.WriteLine(exc.Message)
            End Try

        Next

        Dim conflictLayerNamesStr As String = ""

        If conflictDateLayerNames.Count <> 0 Then
            Dim conflictDateLayerNamesStr = ""
            For i As Integer = 0 To conflictDateLayerNames.Count - 1
                If i = 0 Then
                    conflictDateLayerNamesStr += conflictDateLayerNames.Item(i)
                Else
                    conflictDateLayerNamesStr += ", " + conflictDateLayerNames.Item(i)
                End If
            Next
            conflictLayerNamesStr += "Date field name conflict detected for the following layer(s): " + conflictDateLayerNamesStr + "." + Environment.NewLine
        End If

        If conflictUncyLayerNames.Count <> 0 Then
            Dim conflictUncyLayerNamesStr = ""
            For i As Integer = 0 To conflictUncyLayerNames.Count - 1
                If i = 0 Then
                    conflictUncyLayerNamesStr += conflictUncyLayerNames.Item(i)
                Else
                    conflictUncyLayerNamesStr += ", " + conflictUncyLayerNames.Item(i)
                End If
            Next
            conflictLayerNamesStr += "Uncertainty field name conflict detected for the following layer(s): " + conflictUncyLayerNamesStr + "." + Environment.NewLine
        End If

        If conflictShorelineTypeLayerNames.Count <> 0 Then
            Dim conflictShorelineTypeLayerNamesStr = ""
            For i As Integer = 0 To conflictShorelineTypeLayerNames.Count - 1
                If i = 0 Then
                    conflictShorelineTypeLayerNamesStr += conflictShorelineTypeLayerNames.Item(i)
                Else
                    conflictShorelineTypeLayerNamesStr += ", " + conflictShorelineTypeLayerNames.Item(i)
                End If
            Next
            conflictLayerNamesStr += "Shoreline type field name conflict detected for the following layer(s): " + conflictShorelineTypeLayerNamesStr + "." + Environment.NewLine
        End If


        If conflictDateLayerNames.Count <> 0 OrElse conflictUncyLayerNames.Count <> 0 OrElse conflictShorelineTypeLayerNames.Count <> 0 Then
            log(TraceLevel.Error, "Conflict detected - action canceled." + vbCrLf +
                   "The attribute automator is unable to add the specified field(s) due to a conflict in the listed file(s) because the field name(s) already exists." + vbCrLf +
                   "To proceed you may specify a different field name or unselect all files that have a conflict." + vbCrLf +
                   vbCrLf +
                   conflictLayerNamesStr
            )

            Exit Sub
        End If

        For Each layer As IFeatureLayer In selectedLayers
            If (ifAddDateField) Then
                Dim dateField As IField = GeoDB.CreateField(
                    dateFieldName,
                    dateFieldName,
                    esriFieldType.esriFieldTypeString,
                    Nothing,
                    10,
                    Nothing,
                    Nothing
                )

                GeoDB.AddFieldToFeatureClass(layer.FeatureClass, dateField, False)
            End If

            If (ifAddUncertaintyField) Then
                Dim uncertaintyField As IField = GeoDB.CreateField(
                    uncertaintyFieldName,
                    uncertaintyFieldName,
                    esriFieldType.esriFieldTypeDouble,
                    Nothing,
                    0,
                    20,
                    6
                )


                GeoDB.AddFieldToFeatureClass(layer.FeatureClass, uncertaintyField, False)
            End If

            If (ifAddShorelineTypeField) Then
                Dim shorelineTypeField As IField = GeoDB.CreateField(
                    shorelineTypeFieldName,
                    shorelineTypeFieldName,
                    esriFieldType.esriFieldTypeString,
                    Nothing,
                    10,
                    Nothing,
                    Nothing
                )


                GeoDB.AddFieldToFeatureClass(layer.FeatureClass, shorelineTypeField, False)
            End If

            editedLayerNames.Add(layer.Name)
        Next

        Me.unselectLayerNames(editedLayerNames)
        MessageBox.Show("Fields are successfully added.")

    End Sub

    Protected Sub unselectLayerNames(ByVal layerNames As List(Of String))
        For Each layerName As String In layerNames
            layerListControl.SelectedItems.Remove(layerName)
        Next
    End Sub

    Private Sub closeForm()
        Me.Close()
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        closeForm()
    End Sub

    Private Sub btnBaselineAddFields_Click(sender As Object, e As EventArgs) Handles btnBaselineAddFields.Click
        Me.askForAddFields(LayerType.Baseline)
    End Sub

    Private Sub btnBaselineClose_Click(sender As Object, e As EventArgs) Handles btnBaselineClose.Click
        closeForm()
    End Sub
End Class