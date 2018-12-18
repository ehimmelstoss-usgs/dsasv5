Imports System.Xml

''' <summary>
''' This class simulates the ESRI XSLT extension functions used in ESRI2FGDC.xsl
''' </summary>
''' <remarks></remarks>
Public Class EsriXslExtSim

    ''' <summary>
    ''' Convert to uppercase
    ''' </summary>
    ''' <param name="s">String to operate on</param>
    ''' <returns>String value containing the uppercase form of the given input string.</returns>
    ''' <remarks></remarks>
    Public Function strtoupper(ByVal s As String) As String
        Return s.ToUpper
    End Function

    ''' <summary>
    ''' Convert to lowercase
    ''' </summary>
    ''' <param name="s">String to operate on</param>
    ''' <returns>String value containing the lowercase form of the given input string.</returns>
    ''' <remarks></remarks>
    Public Function strtolower(ByVal s As String) As String
        Return s.ToLower
    End Function

    ''' <summary>
    ''' Deserialize nodeset
    ''' </summary>
    ''' <param name="s">String containing the serialized form of a nodeset</param>
    ''' <returns>XPathNavigator objects for navigating the nodeset</returns>
    ''' <remarks></remarks>
    Public Function decodenodeset(ByVal s As String) As XPath.XPathNavigator
        Dim xd As New XPath.XPathDocument(New System.IO.StringReader(s))
        Return xd.CreateNavigator()
    End Function

    ''' <summary>
    ''' Used only for testing
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function v(ByVal s As String) As String
        log(s)
        Return s
    End Function



End Class
