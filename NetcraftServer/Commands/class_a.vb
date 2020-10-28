Public MustInherit Class Command
    Property Name As String
    Property Description As String
    Property Usage As String
    Sub New(a As String, b As String, c As String)
        Name = a
        Description = b
        Usage = c
    End Sub
    MustOverride Function OnCommand(sender As CommandSender, cmd As Command, args As String(), label As String) As Boolean
End Class
Public Class CommandSender
    Friend senderName As String
    Friend senderAdmin As Boolean
    Sub New(a, b)
        Me.senderName = a
        Me.senderAdmin = b
    End Sub
    Function GetName() As String
        Return senderName
    End Function
    Function GetAdmin() As Boolean
        Return senderAdmin
    End Function
    ReadOnly Property IsPlayer As Boolean
        Get
            If TypeOf Me Is NetworkPlayer Then
                Return True
            End If
            Return False
        End Get
    End Property
    Sub SendMessage(m As String)
        If IsPlayer Then
            Dim p As NetworkPlayer = Me
            p.Chat(m)
            Exit Sub
        End If
        Form1.Log(m)
    End Sub
End Class
