Imports System.Collections.Specialized

Namespace netcraft
    Namespace server
        Namespace api
            Public Class Logger
                Shared Sub Info(arg0 As String)
                    Form1.Log(arg0, "INFO")
                End Sub
                Shared Sub Warning(arg0 As String)
                    Form1.Log(arg0, "WARNING")
                End Sub
                Shared Sub Severe(arg0 As String)
                    Form1.Log(arg0, "ERROR")
                End Sub
            End Class
            Public Class NCSApi
                Public Shared Event BlockBreakEvent(e As events.BlockBreakEventArgs)
                Friend Shared Sub REBlockBreakEvent(e As events.BlockBreakEventArgs)
                    RaiseEvent BlockBreakEvent(e)
                End Sub
                Public Shared Event BlockPlaceEvent(e As events.BlockPlaceEventArgs)
                Friend Shared Sub REBlockPlaceEvent(e As events.BlockPlaceEventArgs)
                    RaiseEvent BlockPlaceEvent(e)
                End Sub
                Public Shared Event PlayerChatEvent(e As events.PlayerChatEventArgs)
                Friend Shared Sub REPlayerChatEvent(e As events.PlayerChatEventArgs)
                    RaiseEvent PlayerChatEvent(e)
                End Sub
                Public Shared Event PlayerMoveEvent(e As events.PlayerMoveEventArgs)
                Friend Shared Sub REPlayerMoveEvent(e As events.PlayerMoveEventArgs)
                    RaiseEvent PlayerMoveEvent(e)
                End Sub
                Public Shared Event PlayerJoinEvent(e As events.PlayerJoinEventArgs)
                Friend Shared Sub REPlayerJoinEvent(e As events.PlayerJoinEventArgs)
                    RaiseEvent PlayerJoinEvent(e)
                End Sub
                Public Shared Event PlayerLeaveEvent(e As events.PlayerLeaveEventArgs)
                Friend Shared Sub REPlayerLeaveEvent(e As events.PlayerLeaveEventArgs)
                    RaiseEvent PlayerLeaveEvent(e)
                End Sub
                Public Shared Event PlayerHealthEvent(e As events.PlayerHealthEventArgs)
                Friend Shared Sub REPlayerHealthEvent(e As events.PlayerHealthEventArgs)
                    RaiseEvent PlayerHealthEvent(e)
                End Sub
                Public Shared Event PlayerDeathEvent(e As events.PlayerDeathEventArgs)
                Friend Shared Sub REPlayerDeathEvent(e As events.PlayerDeathEventArgs)
                    RaiseEvent PlayerDeathEvent(e)
                End Sub
                Public Class Netcraft
                    Friend Shared field_a As StringCollection = New StringCollection
                    Friend Shared MainWindow As Form1
                    Private Shared field_b As List(Of Command) = New List(Of Command)
                    Friend Shared clientList As List(Of NetworkPlayer) = New List(Of NetworkPlayer)
                    Shared ReadOnly Property ConsoleCommandSender As CommandSender = New CommandSender("Server", True)
                    Shared Sub AddCommand(a As Command)
                        field_b.Add(a)
                    End Sub
                    Shared Sub RemoveCommand(a As String)
                        For Each local_a In field_b
                            If local_a.Name.ToLower = a.ToLower Then
                                field_b.Remove(local_a)
                                Exit For
                            End If
                        Next
                    End Sub
                    Shared Sub PerformCommand(arg_a As CommandSender, arg_b As Command, arg_c As String)
                        arg_b.OnCommand(arg_a, arg_b, arg_c.Split(" ").Skip(1).ToArray, arg_c)
                    End Sub
                    Shared Function GetCommands() As List(Of Command)
                        Return field_b
                    End Function
                    Shared Sub BanPlayer(a As String)
                        If field_a.Contains(a.ToLower) Then
                            Throw New Exception("Can't ban already banned player!")
                            Exit Sub
                        End If
                        For Each client In GetOnlinePlayers()
                            If client.Username.ToLower = a.ToLower Then
                                client.Kick("You have been banned from this server.")
                            End If
                        Next
                        field_a.Add(a.ToLower)
                        Form1.SaveBanlist()
                    End Sub
                    Shared Sub UnbanPlayer(a As String)
                        If Not field_a.Contains(a.ToLower) Then
                            Throw New Exception("Can't unban not banned player!")
                            Exit Sub
                        End If
                        field_a.Remove(a.ToLower)
                        Form1.SaveBanlist()
                    End Sub
                    Shared Function GetBannedPlayers() As List(Of String)
                        Dim local_a As List(Of String) = New List(Of String)
                        For Each local_b In field_a
                            local_a.Add(local_b.ToLower)
                        Next
                        Return local_a
                    End Function
                    Shared Function IsBanned(arg_a As String) As Boolean
                        Return field_a.Contains(arg_a.ToLower)
                    End Function
                    Shared Sub Broadcast(m As String)
                        'accessMainForm.Log("Broadcast: " + m)
                        'accessMainForm.Send("chat?" + m)
                        'accessMainForm(m)
                        'My.Forms.Form1.Send("chat?" + m)
                        'My.Forms.Form1.Log(m)
                        RaiseEvent dobc(m)
                    End Sub
                    Friend Shared Event dobc(m As String)
                    Shared Function GetWorld() As WorldServer
                        Return MainWindow.World
                    End Function
                    Shared Function GetPlayer(arg0 As String) As NetworkPlayer
                        For Each p In MainWindow.clientList
                            If p.Username.ToLower = arg0.ToLower Then
                                Return p
                            End If
                        Next
                        Return Nothing
                    End Function
                    Shared Function GetOnlinePlayers() As List(Of NetworkPlayer)
                        Dim s = New List(Of NetworkPlayer)
                        For Each p In MainWindow.clientList
                            s.Add(p)
                        Next
                        Return s
                    End Function
                    Shared Function GetPlayersData() As StringCollection
                        Dim s = New StringCollection
                        For Each g In IO.Directory.GetFiles(Application.StartupPath + "\playerdata\")
                            s.Add(g)
                        Next
                        Return s
                    End Function
                End Class
            End Class
        End Namespace
    End Namespace
End Namespace