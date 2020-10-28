Imports NetcraftServer

Public Class Commandkill
    Inherits Command
    Sub New()
        MyBase.New("kill", "Убить указанного игрока", "kill <игрок>")
    End Sub
    Public Overrides Function OnCommand(sender As CommandSender, cmd As Command, args() As String, label As String) As Boolean
        If Not sender.GetAdmin Then
            sender.SendMessage("У Вас недостаточно прав!")
            Return True
        End If
        If args.Length = 1 Then
            Dim p As NetworkPlayer
            p = netcraft.server.api.NCSApi.Netcraft.GetPlayer(args(0))
            If IsNothing(p) Then
                sender.SendMessage("Игрок не найден!")
                Return True
            End If
            p.Kill("was slain!")
            sender.SendMessage("Slain: " + args(0))
            Return True
        End If
        Return False
    End Function
End Class
