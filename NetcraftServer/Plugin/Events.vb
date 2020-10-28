Namespace netcraft.server.api
    Namespace events
        Public Class BlockBreakEventArgs
            Dim a As Block
            Dim b As NetworkPlayer
            Dim c As Boolean
            Function GetBlock() As Block
                Return a
            End Function
            Function GetPlayer() As NetworkPlayer
                Return b
            End Function
            Sub SetCancelled(arg0 As Boolean)
                c = arg0
            End Sub
            Function GetCancelled() As Boolean
                Return c
            End Function

            Friend Sub New(p As NetworkPlayer, b As Block)
                Me.b = p
                a = b
                c = False
            End Sub

        End Class
        Public Class BlockPlaceEventArgs
            Dim a As Block
            Dim b As NetworkPlayer
            Dim c As Boolean
            Function GetBlock() As Block
                Return a
            End Function
            Function GetPlayer() As NetworkPlayer
                Return b
            End Function
            Sub SetCancelled(arg0 As Boolean)
                c = arg0
            End Sub
            Function GetCancelled() As Boolean
                Return c
            End Function

            Friend Sub New(p As NetworkPlayer, b As Block)
                Me.b = p
                a = b
                c = False
            End Sub

        End Class
        Public Class PlayerChatEventArgs
            Dim a As String
            Dim b As NetworkPlayer
            Dim c As Boolean
            Function GetMessage() As String
                Return a
            End Function
            Function GetPlayer() As NetworkPlayer
                Return b
            End Function
            Sub SetCancelled(arg0 As Boolean)
                c = arg0
            End Sub
            Function GetCancelled() As Boolean
                Return c
            End Function

            Friend Sub New(p As NetworkPlayer, m As String)
                b = p
                a = m
                c = False
            End Sub
        End Class
        Public Class PlayerMoveEventArgs
            Dim a As Point
            Dim b As Point
            Dim c As NetworkPlayer
            Dim d As Boolean
            Function GetFrom() As Point
                Return a
            End Function
            Function GetTo() As Point
                Return b
            End Function
            Function GetPlayer() As NetworkPlayer
                Return c
            End Function
            Function GetCancelled() As Boolean
                Return d
            End Function
            Sub SetCancelled(arg0 As Boolean)
                d = arg0
            End Sub
            Sub New(from As Point, mTo As Point, p As NetworkPlayer)
                d = False
                Me.a = from
                b = mTo
                c = p
            End Sub
        End Class
        Public Class PlayerJoinEventArgs
            Dim player As NetworkPlayer
            Function GetPlayer() As NetworkPlayer
                Return player
            End Function
            Sub New(p As NetworkPlayer)
                player = p
            End Sub
        End Class
        Public Class PlayerLeaveEventArgs
            Dim player As NetworkPlayer
            Function GetPlayer() As NetworkPlayer
                Return player
            End Function
            Sub New(p As NetworkPlayer)
                player = p
            End Sub
        End Class
        Public Class PlayerHealthEventArgs
            Dim a As NetworkPlayer
            Dim b As Integer
            Dim c As Integer
            Dim d As Boolean
            Function GetPlayer() As NetworkPlayer
                Return a
            End Function
            Function GetOldHealth() As Integer
                Return b
            End Function
            Function GetNewHealth() As Integer
                Return c
            End Function
            Function GetCancelled() As Boolean
                Return d
            End Function
            Sub SetCancelled(arg0 As Boolean)
                d = arg0
            End Sub
            Sub New(p As NetworkPlayer, oldHealth As Integer, newHealth As Integer)
                a = p
                Me.b = oldHealth
                Me.c = newHealth
                d = False
            End Sub
        End Class
        Public Class PlayerDeathEventArgs
            Dim a As NetworkPlayer
            Dim b As String
            Dim c As Boolean
            Dim d As Point
            Sub New(a As NetworkPlayer, b As String, d As Point)
                Me.a = a
                Me.b = b
                Me.c = False
                Me.d = d
            End Sub
            Sub SetDeathMessage(b As String)
                Me.b = b
            End Sub
            Function GetDeathMessage() As String
                Return Me.b
            End Function
            Sub SetCancelled(a As Boolean)
                c = a
            End Sub
            Function GetCancelled() As Boolean
                Return c
            End Function
            Function GetSpawn() As Point
                Return d
            End Function
            Sub SetSpawn(d As Point)
                Me.d = d
            End Sub
        End Class
    End Namespace
End Namespace