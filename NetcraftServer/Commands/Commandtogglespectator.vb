Imports NetcraftServer

Public Class Commandtogglespectator
    Inherits Command
    Sub New()
        MyBase.New("togglespectator", "Переключает режим наблюдателя.", "togglespectator")
    End Sub
    Public Overrides Function OnCommand(sender As CommandSender, cmd As Command, args() As String, label As String) As Boolean
        If Not sender.GetAdmin Then
            sender.SendMessage("У Вас недостаточно прав!")
            Return True
        End If
        If Not sender.IsPlayer Then
            Return False
        End If
        If args.Length = 0 Then
            Dim p As NetworkPlayer = sender
            If p.IsSpectator Then
                p.Survival()
            Else
                p.Spectator()
            End If
            Return True
        End If
        Return False
    End Function
End Class
