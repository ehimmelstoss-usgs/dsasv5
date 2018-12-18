'Imports System
Imports IronPython.Hosting
'Imports IronPython.Runtime
Imports Microsoft.Scripting.Hosting
Imports System.Reflection
Imports System.IO


Module ipy

    Public ReadOnly Property AssemblyDirectory() As String
        Get
            Dim codeBase As String = Assembly.GetExecutingAssembly().CodeBase
            Dim uri__1 As New UriBuilder(codeBase)
            Dim path__2 As String = Uri.UnescapeDataString(uri__1.Path)
            Return Path.GetDirectoryName(path__2)
        End Get
    End Property

    Function makeAbsoluteScriptLocation(scriptLocation As String) As String
        ' Make sure script location is absolute
        If System.IO.Path.IsPathRooted(scriptLocation) Then
            Return scriptLocation
        Else
            Return AssemblyDirectory() + "\\" + scriptLocation
        End If
    End Function

    Dim ipyEnvs As New Dictionary(Of String, ScriptEngine)

    Function setupEnvForPyScript(scriptLocation As String) As ScriptEngine
        ' If we already set up this script then just return the script engine that was created
        If ipyEnvs.ContainsKey(scriptLocation) Then Return ipyEnvs(scriptLocation)

        Dim options As New Dictionary(Of String, Object)()
        options("Debug") = True
        ' Create an engine to access IronPython.
        Dim engine As ScriptEngine = Python.CreateEngine(options)
        ' Describe where to load the script.
        Dim source As ScriptSource = engine.CreateScriptSourceFromFile(makeAbsoluteScriptLocation(scriptLocation))
        ' Obtain the default scope for executing the script.
        Dim scope As ScriptScope = engine.CreateScope()

        Dim paths As ICollection(Of String) = engine.GetSearchPaths()
        'MsgBox(AssemblyDirectory())
        paths.Add(AssemblyDirectory() + "\Lib")
        paths.Add(AssemblyDirectory() + "\Lib\site-packages")
        paths.Add(AssemblyDirectory() + "\usgs_scripts")
        'paths.Add(@"C:\Python27\Lib")  ' Or you can add the CPython libs instead
        engine.SetSearchPaths(paths)

        ' Create an object for performing tasks with the script.
        Dim Ops As ObjectOperations = engine.CreateOperations()

        ' Create the class object.
        source.Execute(scope)
        ipyEnvs.Add(scriptLocation, engine)
        Return engine
    End Function

    Function callPyFunction(scriptRelativePath As String, functionName As String, Optional params As IronPython.Runtime.PythonDictionary = Nothing) As Object
        Dim scriptLocation As String = makeAbsoluteScriptLocation(scriptRelativePath)
        Dim engine As ScriptEngine = setupEnvForPyScript(scriptLocation)
        ' Obtain the function object.
        Dim var As Object = engine.GetScope(scriptLocation).GetVariable(functionName)
        ' Invoke the function.
        Dim val As Object
        Try
            If params Is Nothing Then
                val = engine.Operations.Invoke(var)
            Else
                val = engine.Operations.Invoke(var, params)
            End If
        Catch ex As Exception
            handleException(ex)
            Throw ex
        End Try
        ' Return function output value.
        Return val
    End Function


    Function getSummary() As Object
        Return callPyFunction("usgs_scripts\main.py", "getSummary")
    End Function


    Function runCalcs() As Object
        Return callPyFunction("usgs_scripts\main.py", "runCalcs")
    End Function


    Function calc(params As IronPython.Runtime.PythonDictionary) As Object
        Return callPyFunction("usgs_scripts\main.py", "calc", params)
    End Function


    Function getRateList() As Object
        Return callPyFunction("usgs_scripts\main.py", "rateList")
    End Function


    Function getSummaryCalcsLookup() As Object
        Return callPyFunction("usgs_scripts\summaryCalcsLookup.py", "getSummaryCalcsLookup")
    End Function


    Function forecast(params As IronPython.Runtime.PythonDictionary) As Object
        Return callPyFunction("usgs_scripts\main_forecasting.py", "calc_forecast", params)
    End Function


    Function testIPY() As String
        Dim scriptLocation As String = makeAbsoluteScriptLocation("TestIPY.py")
        Dim engine As ScriptEngine = setupEnvForPyScript(scriptLocation)
        ' Obtain the class object.
        Dim calcClass As Object = engine.GetScope(scriptLocation).GetVariable("DoCalculations")
        ' Create an instance of the class.
        Dim calcObj As Object = engine.Operations.Invoke(calcClass)
        ' Execute a member from the class instance.
        Return engine.Operations.InvokeMember(calcObj, "doit", My.Document)
        'Return Ops.InvokeMember(CalcObj, "doStudentTDistribution", 5, 2.1)
    End Function



End Module
