Imports ESRI.ArcGIS.esriSystem


Public Class DSAS
    Private Sub New()
        Dim msg As String = ""
        Try
            If Not checkAndCopyTemplate() Then
                msg = "Problem while initializing work area."
                Exit Sub
            End If

            Try
                logPath_ = DSASUtility.getAppDataFolder + IO.Path.DirectorySeparatorChar + logFilename
                'Get rid of old logfile here if it exists: Emily & Rob's specifications
                System.IO.File.Delete(logPath_)
                Dim DSASListener As New System.Diagnostics.TextWriterTraceListener(logPath_)
                Trace.Listeners.Add(DSASListener)
                Trace.AutoFlush = True
            Catch ex As Exception
                msg = "Problem while initializing log file."
                Exit Sub
            End Try

            Try
                dsasTables = DirectCast(XML2HT.fromString(My.Resources.DSAS_tables)("tables"), Hashtable)
            Catch ex As Exception
                msg = "Problem while initializing data structures."
                Exit Sub
            End Try
        Finally
            If msg = "" Then
                enabled_ = True
            Else
                msg += vbCrLf + "DSAS will be disabled..."
                log(TraceLevel.Error, msg)
            End If
        End Try
    End Sub

    Public Shared ReadOnly Property Instance As DSAS
        Get
            Static INST As DSAS = New DSAS
            Return INST
        End Get
    End Property


    Public Shared ReadOnly Property logFilename As String
        Get
            Return "DSAS_log.txt"
        End Get
    End Property


    Private Shared enabled_ As Boolean = False

    Public Shared ReadOnly Property enabled As Boolean
        Get
            Return enabled_
        End Get
    End Property


    Private Shared logPath_ As String = Nothing

    Public Shared ReadOnly Property logPath As String
        Get
            Return logPath_
        End Get
    End Property


    Public dsasTables As Hashtable

    'When true, automatic reporting of error logs will be suppressed
    Public Shared suppressErrorDialogs As Boolean = False

    'Handle to the calculate stats form
    Public Shared calcForm As CalcStatsForm

    'status bar of arcmap
    Public Shared statusBar As IStatusBar


    'Representation error. Distances smaller than this are considered insignificant, i.e. equal to zero
    Public Const REP_ERR As Double = 0.001


    ' Number of digits to be used while rounding calculation results. Used for:
    ' XML output
    Public Const roundDigits As Integer = 2

    Public Shared dsasVersion As String = My.Version


    ''' <summary>
    ''' Set to 1 only when DSAS is autogenerating transects. Elsewhere set to 0.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared autogen As Integer = 0

    Public Shared TransectLyrToolInstance As TransectLyrToolCtl = Nothing

    ''' <summary>
    ''' Set to true if user attempts to interrupt rate calculations
    ''' </summary>
    Public Shared interruptCalcs As Boolean = False

    ''' <summary>
    ''' Holds the featureclass for the currently selected shoreline layer
    ''' </summary>
    Public Shared shoreFc As ESRI.ArcGIS.Geodatabase.IFeatureClass = Nothing


    Public Shared createRecordOfCastSmoothing As Boolean = False
    Public Shared record As ESRI.ArcGIS.Geodatabase.IFeatureClass = Nothing

    ''' <summary>
    ''' Holds the seaward setting in effect. Use this over My.Settings.Seaward
    ''' </summary>
    Public Shared seaward As Boolean? = Nothing

    Public Const MsgBoxTitle = "DSAS Alert"
    Public Const MsgBoxTitleError = "DSAS Error"


    Public Shared layerTypes As New Dictionary(Of String, Dictionary(Of String, String)) From
        {
            {
                "transect", New Dictionary(Of String, String) From
                {
                    {"friendlyName", "transect"},
                    {"metadataSource", Nothing},
                    {"purposeEpilogue", "This dataset consists of measurement transects cast by the Digital Shoreline Analysis System version 5.0 (DSAS v5.0) from a user defined baseline to intersect shoreline vectors. The points of intersection will provide location and time information used to calculate rates of change."}
                }
            },
            {
                "rates", New Dictionary(Of String, String) From
                {
                    {"friendlyName", "transect rate"},
                    {"metadataSource", "transect"},
                    {"purposeEpilogue", "This dataset consists of shoreline change rates calculated with the Digital Shoreline Analysis System version 5.0 (DSAS v5.0) and stored as a new transect layer. Original measurement transects are cast by DSAS from the baseline to intersect shoreline vectors, and the intersect data provide location and time information used to calculate rates of change. These statistics are appended to the rate transect layer."}
                }
            },
            {
                "intersect", New Dictionary(Of String, String) From
                {
                    {"friendlyName", "intersect"},
                    {"metadataSource", "transect"},
                    {"purposeEpilogue", "This dataset consists of point data which are created by the Digital Shoreline Analysis System version 5.0 (DSAS v5.0) at the intersection of shoreline and transect locations. The point data contain identification information related to the DSAS transect (TransectID, TransOrder) as well as the shoreline (ShorelineID, Uncertainty). Original measurement transects are cast by DSAS from the baseline to intersect shoreline vectors, and the intersect data provide location and time information used to calculate rates of change."}
                }
            },
            {
                "forecast", New Dictionary(Of String, String) From
                {
                    {"friendlyName", "forecast"},
                    {"metadataSource", "rates"},
                    {"purposeEpilogue", "This dataset consists of a polyline file representing the Digital Shoreline Analysis System version 5.0 (DSAS v5.0) beta shoreline forecast at a 10- or 20-year time horizon. New to DSAS v5.0 is an option to calculate a shoreline position either 10 or 20 years into the future based on historical shoreline positions.  The method assimilates shoreline observations (shoreline position change, shoreline uncertainty) then estimates process noise to forecast the shoreline position and provide a quantitative estimate of uncertainty for the forecast."}
                }
            },
            {
                "forecast_uncy", New Dictionary(Of String, String) From
                {
                    {"friendlyName", "forecast uncertainty"},
                    {"metadataSource", "rates"},
                    {"purposeEpilogue", "This dataset consists of a polygon file representing the uncertainty of the Digital Shoreline Analysis System version 5.0 (DSAS v5.0) beta shoreline forecast at a 10- or 20-year time horizon. New to DSAS v5.0 is an option to calculate a shoreline position either 10 or 20 years into the future based on historical shoreline positions. The method assimilates shoreline observations (shoreline position change, shoreline uncertainty) then estimates process noise to forecast the shoreline position and provide a quantitative estimate of uncertainty for the forecast."}
                }
            },
            {
                "forecast_points", New Dictionary(Of String, String) From
                {
                    {"friendlyName", "forecast points"},
                    {"metadataSource", "rates"},
                    {"purposeEpilogue", "This dataset consists of a point file representing the Digital Shoreline Analysis System version 5.0 (DSAS v5.0) beta shoreline forecast at a 10- or 20-year time horizon. New to DSAS v5.0 is an option to calculate a shoreline position either 10 or 20 years into the future based on historical shoreline positions. The method assimilates shoreline observations (shoreline position change, shoreline uncertainty) then estimates process noise to forecast the shoreline position and provide a quantitative estimate of uncertainty for the forecast."}
                }
            }
        }


    Public Enum layerCheckingMode
        validation  'Strict check. OPTION fields are also checked
        detection   'Relaxed check. OPTION fields are not checked
    End Enum


End Class