Imports NetcraftServer
Imports NetcraftServer.netcraft.server.api

Public Class Commandtoggleadmin
    Inherits Command
    Sub New()
        MyBase.New("toggleadmin", "Переключает статус администратора игроку", "toggleadmin <arg>")
    End Sub
    Public Overrides Function OnCommand(sender As CommandSender, cmd As Command, args() As String, label As String) As Boolean
        If Not sender.GetAdmin Then
            sender.SendMessage("У Вас недостаточно прав!")
            Return True
        End If
        If args.Length = 1 Then
            Dim local_a = NCSApi.Netcraft.GetPlayer(args(0))
            If IsNothing(local_a) Then
                sender.SendMessage("Игрок не найден")
            End If
            local_a.IsAdmin = Not local_a.IsAdmin
            If local_a.IsAdmin Then
                local_a.Chat("Вы теперь администратор")
            Else
                local_a.Chat("Вы больше не администратор")
            End If
            Return True
        End If
        Return False
    End Function

End Class
