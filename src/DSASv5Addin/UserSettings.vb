Imports System.Runtime.Serialization

<Serializable()> Public Class UserSettings
    Implements ISerializable

    Public Sub New()
    End Sub


    Protected Sub New(info As SerializationInfo, context As StreamingContext)

        For Each prop As Configuration.SettingsProperty In My.Settings.Properties
            Dim val As Object = Nothing
            'Overwrite table or field name from user settings with empty value so they are only retrieved from project settings
            If prop.Name.ToLower.Contains("table") OrElse prop.Name.ToLower.Contains("layer") OrElse prop.Name.ToLower.Contains("field") Then val = ""
            Try
                Select Case prop.PropertyType.Name
                    Case "String"
                        val = info.GetString(prop.Name)
                    Case "Boolean"
                        val = info.GetBoolean(prop.Name)
                    Case "Single"
                        val = info.GetSingle(prop.Name)
                    Case "Double"
                        val = info.GetDouble(prop.Name)
                    Case "Int16"
                        val = info.GetInt16(prop.Name)
                    Case "Int32"
                        val = info.GetInt32(prop.Name)
                    Case "Int64"
                        val = info.GetInt64(prop.Name)
                End Select
            Catch ex As Exception
                log(prop.Name)
                log(prop.PropertyType.Name)
                log(ex.Message)
                val = prop.DefaultValue
            End Try

            Try
                My.Settings(prop.Name) = val
            Catch ex As Exception
                log(String.Format("Error setting {0} to {1}", prop.Name, val))
            End Try
        Next
    End Sub


    Public Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData
        For Each prop As Configuration.SettingsProperty In My.Settings.Properties
            Try
                info.AddValue(prop.Name, My.Settings(prop.Name))
            Catch ex As Exception
                log(ex.Message)
            End Try
        Next
    End Sub


End Class