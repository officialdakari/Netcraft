Imports System.Collections.Specialized

Public Class SendQueue

    Dim field1 As NetworkPlayer
    Dim field2 As StringCollection = New StringCollection
    Sub SendQueue()
        Dim a As String = ""
        For Each f In field2
            a += f + vbLf
        Next
        'a = a.TrimEnd(vbCrLf)
        'Console.WriteLine("[DEBUG] Send packet " + a)
        field1.Send(a)
        field2.Clear()
    End Sub
    Sub AddQueue(arg0 As String)
        field2.Add(arg0)
    End Sub
    Sub RemoveQueue(arg0 As String)
        If Not field2.Contains(arg0) Then
            Throw New KeyNotFoundException("Cannot delete unexisting packet in queue")
        End If
        field2.Remove(arg0)
    End Sub
    Sub New(ByRef arg0 As NetworkPlayer)
        field1 = arg0
    End Sub

End Class
