Imports NetcraftServer

Public Class Commandgive
    Inherits Command
    Sub New()
        MyBase.New("give", "Выдать предмет игроку/себе", "give <игрок|@s|@a|@r> <предмет> [кол-во]")
    End Sub
    Public Overrides Function OnCommand(sender As CommandSender, cmd As Command, args() As String, label As String) As Boolean
        If Not sender.GetAdmin Then
            sender.SendMessage("У Вас недостаточно прав!")
            Return True
        End If
        If args.Length = 2 Then
            Dim t As Material = [Enum].Parse(GetType(Material), args(1).ToUpper)
            Dim p As NetworkPlayer
            If args(0) = "@s" Then
                If Not sender.IsPlayer Then
                    sender.SendMessage("Только для игрока!")
                    Return True
                End If
                p = sender
                p.Give(t)
                p.Chat($"Выдано {t.ToString.ToLower} (1 шт.) игроку " + p.Username)
                Return True
            ElseIf args(0) = "@a" Then
                For Each g In netcraft.server.api.NCSApi.Netcraft.GetOnlinePlayers
                    g.Give(t)
                Next
                sender.SendMessage($"Выдано {t.ToString.ToLower} (1 шт.) {netcraft.server.api.NCSApi.Netcraft.GetOnlinePlayers.Count} игрокам")
                Return True
            ElseIf args(0) = "@r" Then
                Dim rnd As Random = New Random
                Dim g = netcraft.server.api.NCSApi.Netcraft.GetOnlinePlayers()
                p = g(rnd.Next(0, g.Count - 1))
                p.Give(t)
                sender.SendMessage($"Выдано {t.ToString.ToLower} (1 шт.) игроку " + p.Username)
                Return True
            Else

                p = netcraft.server.api.NCSApi.Netcraft.GetPlayer(args(0))
                If IsNothing(p) Then
                    sender.SendMessage("Игрок не найден!")
                    Return True
                End If
                p.Give(t)
                sender.SendMessage($"Выдано {t.ToString.ToLower} (1 шт.) игроку " + p.Username)
                Return True
            End If

        End If


        If args.Length = 3 Then
            Dim t As Material = [Enum].Parse(GetType(Material), args(1).ToUpper)
            Dim count As Integer = CInt(args(2))
            Dim p As NetworkPlayer
            If args(0) = "@s" Then
                If Not sender.IsPlayer Then
                    sender.SendMessage("Только для игрока!")
                    Return True
                End If
                p = sender
                p.Give(t, count)
                p.Chat($"Выдано {t.ToString.ToLower} ({count} шт.) игроку " + p.Username)
                Return True
            ElseIf args(0) = "@a" Then
                For Each g In netcraft.server.api.NCSApi.Netcraft.GetOnlinePlayers
                    g.Give(t, count)
                Next
                sender.SendMessage($"Выдано {t.ToString.ToLower} ({count} шт.) {netcraft.server.api.NCSApi.Netcraft.GetOnlinePlayers.Count} игрокам")
                Return True
            ElseIf args(0) = "@r" Then
                Dim rnd As Random = New Random
                Dim g = netcraft.server.api.NCSApi.Netcraft.GetOnlinePlayers()
                p = g(rnd.Next(0, g.Count - 1))
                p.Give(t, count)
                sender.SendMessage($"Выдано {t.ToString.ToLower} ({count} шт.) игроку " + p.Username)
                Return True
            Else

                p = netcraft.server.api.NCSApi.Netcraft.GetPlayer(args(0))
                If IsNothing(p) Then
                    sender.SendMessage("Игрок не найден!")
                    Return True
                End If
                p.Give(t, count)
                sender.SendMessage($"Выдано {t.ToString.ToLower} ({count} шт.) игроку " + p.Username)
                Return True
            End If

        End If
        Return False
    End Function
End Class
