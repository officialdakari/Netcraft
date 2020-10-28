Public Class EntityPlayerNPC

    REM - Сделать это сегодня!

    Property Position As Point
    Dim direction As Integer = 2
    Sub NPCTick()

        If Position.X = 0 Then
            direction = 2
        ElseIf Position.X = 100 Then
            direction = -2
        End If
        Position = New Point(Position.X + direction, 0)

        UpdateNPCPosition()

    End Sub
    Sub UpdateNPCPosition()
        Form1.Send("updateplayerposition?NPC1?" + Position.X.ToString + "?" + Position.Y.ToString)
    End Sub
    Sub New()
        Position = New Point(0, 0)
    End Sub

End Class
