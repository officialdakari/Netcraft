Imports NetcraftServer

Public Class Commandcraft
    Inherits Command
    Sub New()
        MyBase.New("craft", "Скрафтить вещь [ИГРОК]", "craft <material>")
    End Sub
    Public Overrides Function OnCommand(sender As CommandSender, cmd As Command, args() As String, label As String) As Boolean
        If Not sender.IsPlayer Then
            Return False
        End If
        If args.Length = 1 Then
            Try
                Dim m = [Enum].Parse(GetType(Material), args(0).ToUpper)
                Dim p As NetworkPlayer = sender
                p.Craft(m)
                p.Chat("Crafted!")
            Catch ex As Exception
                sender.SendMessage("An internal error occured.")
            End Try
            Return True
        End If
        Return False
    End Function
End Class
