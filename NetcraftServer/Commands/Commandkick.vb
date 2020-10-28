Imports NetcraftServer

Public Class Commandkick
    Inherits Command
    Sub New()
        MyBase.New("kick", "Выгоняет игрока с сервера", "kick <игрок> [причина]")
    End Sub
    Public Overrides Function OnCommand(sender As CommandSender, cmd As Command, args() As String, label As String) As Boolean
        If Not sender.GetAdmin Then
            sender.SendMessage("У Вас недостаточно прав!")
            Return True
        End If
        If args.Length = 1 Then
            Dim p = netcraft.server.api.NCSApi.Netcraft.GetPlayer(args(0))
            If IsNothing(p) Then
                sender.SendMessage("Игрок не найден!")
                Return True
            End If
            p.Kick("Вы были выгнаны администратором.")
            netcraft.server.api.NCSApi.Netcraft.Broadcast($"{sender.GetName} выгнал игрока {p.GetName}. Причина не указана.")
        ElseIf args.Length > 1 Then
            Dim p = netcraft.server.api.NCSApi.Netcraft.GetPlayer(args(0))
            If IsNothing(p) Then
                sender.SendMessage("Игрок не найден!")
                Return True
            End If
            p.Kick("Вы были выгнаны администратором. Причина: " + String.Join(" ", args.Skip(1)))
            netcraft.server.api.NCSApi.Netcraft.Broadcast($"{sender.GetName} выгнал игрока {p.GetName}. Причина: {String.Join(" ", args.Skip(1))}")
        End If
        Return False
    End Function
End Class
