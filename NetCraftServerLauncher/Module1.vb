Imports System.IO

Module Module1
    WithEvents proc As Process
    WithEvents watcher As FileSystemWatcher = New FileSystemWatcher
    Sub Main()
        proc = New Process
        proc.StartInfo.FileName = "C:\Users\gkiki\Desktop\netcraft\NetCraftServer\bin\Debug\NetCraftServer.exe"
        watcher.Path = "C:\Users\gkiki\Desktop\netcraft\NetCraftServer\bin\Debug\crash-reports"
        watcher.EnableRaisingEvents = True
        watcher.Filter = "*.txt"
        proc.StartInfo.Arguments = ""
        proc.StartInfo.RedirectStandardOutput = True
        proc.StartInfo.RedirectStandardError = True
        proc.StartInfo.UseShellExecute = False
        proc.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
        proc.StartInfo.StandardOutputEncoding = Text.Encoding.Default
        proc.StartInfo.StandardErrorEncoding = Text.Encoding.Default
        proc.Start()
        proc.BeginErrorReadLine()
        proc.BeginOutputReadLine()
        While True

        End While
    End Sub
    Private Sub onData(sender As Object, e As DataReceivedEventArgs) Handles proc.OutputDataReceived
        Console.WriteLine(e.Data)
    End Sub
    Private Sub onCrashData(sender As Object, e As DataReceivedEventArgs) Handles proc.ErrorDataReceived
        Console.ForegroundColor = ConsoleColor.Yellow
        Console.WriteLine(e.Data)
        Console.ResetColor()
    End Sub
    Private Sub onCrash(sender As Object, e As FileSystemEventArgs) Handles watcher.Created
        Console.WriteLine("Found crash report at " + e.FullPath)
        If Not proc.HasExited Then
            proc.Kill()
        End If
    End Sub

End Module
