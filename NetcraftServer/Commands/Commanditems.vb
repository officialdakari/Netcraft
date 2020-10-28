Imports NetcraftServer

Public Class Commanditems
    Inherits Command
    Sub New()
        MyBase.New("items", "Показывает список вещей в игре", "items")
    End Sub
    Public Overrides Function OnCommand(sender As CommandSender, cmd As Command, args() As String, label As String) As Boolean
        If args.Length = 0 Then
            Dim a = [Enum].GetNames(GetType(Material))
            sender.SendMessage(String.Join(", ", a).ToLower)
            Return True
        End If
        Return False
    End Function
End Class
