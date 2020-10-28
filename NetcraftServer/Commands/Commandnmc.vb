Imports NetcraftServer

Public Class Commandnmc
    Inherits Command
    Sub New()
        MyBase.New("nmc", "Выключает проверку движения [ТОЛЬКО ИГРОК]", "nmc")
    End Sub
    Public Overrides Function OnCommand(sender As CommandSender, cmd As Command, args() As String, label As String) As Boolean
        If Not sender.IsPlayer Then
            sender.SendMessage("Только для игрока.")
            Return True
        End If
        If sender.GetAdmin Then
            Dim p As NetworkPlayer = sender
            p.DisableMovementCheck = Not p.DisableMovementCheck
            If p.DisableMovementCheck Then
                sender.SendMessage("Отключена проверка передвижения.")
            End If
            Return True
        End If
        Return False
    End Function
End Class
