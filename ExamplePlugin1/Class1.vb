Imports NCore
Imports NCore.netcraft.server.api
Public Class Plugin
    Inherits NCore.Plugin

    Public Overrides Sub OnUnload()
        GetLogger.Warning("Example plugin отключается")
    End Sub

    Public Overrides Function Create() As NCore.Plugin
        SetOptions("ExamplePlugin1", "Example plugin", "0.1", {"DarkCoder15"})
        Return Me
    End Function

    Public Overrides Function OnLoad() As String
        GetLogger.Info("ЗАГРУЖЕНО")
        GetLogger.Warning("Играемся с логгером")
        GetLogger.Severe("=)")

        'Добавим обработчик события, например, когда появляется игрок ему в чат пишет 'Привет, {имя игрока}'
        AddHandler NCSApi.PlayerJoinEvent, AddressOf JoinEvent

        'Добавим обработчик события, например, когда игрок пишет в чат "1" это заменялось на "0"
        AddHandler NCSApi.PlayerChatEvent, AddressOf ChatEvent
        Return Nothing
    End Function

    Protected Async Function JoinEvent(e As events.PlayerJoinEventArgs) As Task
        Dim p = e.GetPlayer
        Await p.Chat($"Привет, {p.Username}")
    End Function

    Protected Async Function ChatEvent(e As events.PlayerChatEventArgs) As Task
        Dim p = e.GetPlayer
        If e.GetMessage = "1" Then
            e.SetMessage("0")
        End If
    End Function
End Class
