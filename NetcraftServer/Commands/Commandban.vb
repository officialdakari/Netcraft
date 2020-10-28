Imports NetcraftServer

Public Class Commandban
    Inherits Command
    Sub New()
        MyBase.New("ban", "Банит игрока на сервере", "ban <игрок>")
    End Sub
    Public Overrides Function OnCommand(sender As CommandSender, cmd As Command, args() As String, label As String) As Boolean
        If Not sender.GetAdmin Then
            sender.SendMessage("У Вас недостаточно прав!")
            Return True
        End If
        If args.Length = 1 Then
            Dim a = args(0)
            netcraft.server.api.NCSApi.Netcraft.Broadcast($"{sender.GetName} забанил игрока {a}.")
            netcraft.server.api.NCSApi.Netcraft.BanPlayer(a)
        End If
        Return False
    End Function
End Class
