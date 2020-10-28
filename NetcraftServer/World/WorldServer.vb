Public Class WorldServer

    Public Property WorldBlocks As New List(Of Block) ' = New List(Of Block)
    Property UUID As String = Guid.NewGuid.ToString
    Public Function GetBlockAt(x As Integer, y As Integer) As Block
        For Each block In worldBlocks
            If block.Position.X = x Then
                If block.Position.Y = y Then
                    Return block
                End If
            End If
        Next
        Return Nothing
    End Function
    Public Sub New()
    End Sub

End Class
