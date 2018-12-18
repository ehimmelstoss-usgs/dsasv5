Imports System.Xml
Imports System.Collections

Module XML2HT


    Public Function fromString(ByVal xmlStr As String) As Hashtable
        Dim xmldoc As New XmlDocument
        Try
            xmldoc.LoadXml(xmlStr)
            Return node2HT(elementNodesOnly(xmldoc.ChildNodes)(0))
        Catch ex As Exception
            log(TraceLevel.Error, "Provided resource needs to be a well-formed and valid XML file!")
            Return Nothing
        End Try
    End Function

    Public Function fromFile(ByVal filePathName As String) As Hashtable
        Dim xmldoc As New XmlDocument
        Try
            xmldoc.Load(filePathName)
            Return node2HT(elementNodesOnly(xmldoc.ChildNodes)(0))
        Catch ex As Exception
            log(TraceLevel.Error, filePathName + " needs to be a well-formed and valid XML file!")
            Return Nothing
        End Try
    End Function

    Private Function node2HT(ByVal node As XmlNode) As Hashtable
        Dim ht As New Hashtable
        Dim order As Integer = 1

        For Each attr As XmlAttribute In node.Attributes
            ht(attr.Name.ToLower) = attr.Value
        Next
        ht("name") = IIf(ht.ContainsKey("name"), ht("name"), node.Name)

        Dim childrenHT As New Hashtable
        Dim childHT As Hashtable
        Dim childNodes As Generic.List(Of XmlNode) = elementNodesOnly(node.ChildNodes)
        For Each child As XmlNode In childNodes
            childHT = node2HT(child)
            childHT("order") = order
            childrenHT(childHT("name").ToString.ToLower.Replace("%", "")) = childHT
            order += 1
        Next
        If childNodes.Count > 0 Then
            ht(childNodes(0).Name.ToLower + "s") = childrenHT
        End If
        Return ht
    End Function

    Public Function elementNodesOnly(ByVal nodes As XmlNodeList) As Generic.List(Of XmlNode)
        elementNodesOnly = New Generic.List(Of XmlNode)
        For Each node As XmlNode In nodes
            If node.NodeType = XmlNodeType.Element Then elementNodesOnly.Add(node)
        Next
        Return elementNodesOnly
    End Function

End Module
