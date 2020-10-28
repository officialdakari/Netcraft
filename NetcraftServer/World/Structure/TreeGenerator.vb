Public Class TreeGenerator
    Shared Sub GenerateTree(pos As Point, w As WorldServer)
        For i = 0 To 6
            If i = 0 Then
                w.WorldBlocks.Add(New Block(pos, EnumBlockType.WOOD, False, False))
            End If
            If i = 1 Then
                Dim pos1 = pos
                pos1.Offset(0, -1)
                w.WorldBlocks.Add(New Block(pos1, EnumBlockType.WOOD, False, False))
            End If
            If i = 2 Then
                Dim pos1 = pos
                pos1.Offset(0, -2)
                w.WorldBlocks.Add(New Block(pos1, EnumBlockType.WOOD, False, False))
            End If
            If i = 3 Then
                Dim pos1 = pos
                pos1.Offset(-1, -3)
                w.WorldBlocks.Add(New Block(pos1, EnumBlockType.LEAVES, False, False))
            End If
            If i = 4 Then
                Dim pos1 = pos
                pos1.Offset(0, -3)
                w.WorldBlocks.Add(New Block(pos1, EnumBlockType.LEAVES, False, False))
            End If
            If i = 5 Then
                Dim pos1 = pos
                pos1.Offset(1, -3)
                w.WorldBlocks.Add(New Block(pos1, EnumBlockType.LEAVES, False, False))
            End If
            If i = 6 Then
                Dim pos1 = pos
                pos1.Offset(0, -4)
                w.WorldBlocks.Add(New Block(pos1, EnumBlockType.LEAVES, False, False))
            End If
        Next
    End Sub
End Class
' LLL 
'LLLLL
'  W
'  W
'  W