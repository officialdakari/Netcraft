Imports System.Reflection
Public Class PluginManager
    Shared Property Plugins As List(Of Assembly) = New List(Of Assembly)
    'Shared Property Plugin_One As Assembly = Nothing
    Shared Function Load(path As String) As Assembly

        Try
            ' If ONE_PLUGIN_LOADED Then
            Dim p = Assembly.LoadFrom(path)
            'Dim pForm As Form = New Form
            'pForm.Text = $"(TeamExplorer) Расширение {p.GetName.Name}"

            Plugins.Add(p)
            GetObject(p, "Plugin").OnLoad()
            Return p
            'GetObject(p, "Plugin").MainForm = pForm
            ' GetObject(p, "Plugin").OnMainFormSet()

            ' GetMainWindow(Plugins.Last).SetMsgBox(New MessageBox1)
            'Else
            '  Plugins(0) = Assembly.LoadFrom(path)

            '     ONE_PLUGIN_LOADED = True
            ' End If
            ' Plugin_One = Assembly.LoadFrom(path)
        Catch ex As Exception
            Form1.CrashReport(ex)
            'Main.Fatal($"Не удалось загрузить расширение. Путь к расширению: {path}{vbCrLf}{vbCrLf}{ex.ToString}")
        End Try
    End Function
    Shared Function GetObject(plugin As Assembly, obj As String)

        Try
            Dim Plugin_MainWindowType = plugin.GetType($"{plugin.GetName.Name}.{obj}", True, False)
            Dim plugin_MainWindow = Activator.CreateInstance(Plugin_MainWindowType)
            Return plugin_MainWindow

        Catch ex As Exception
            Form1.CrashReport(ex)
        End Try
    End Function
End Class