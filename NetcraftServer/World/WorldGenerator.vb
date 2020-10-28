Public Class WorldGenerator
    Shared ReadOnly rnd = New Random()
    Public Shared Function Generate() As WorldServer
        Dim world As WorldServer = New WorldServer
        Dim a = 0
        For i = 0 To 30
            Dim YHeight As Integer = New Random().Next(1, 14)
            For o = 0 To 24
                If o = 17 Then
                    world.WorldBlocks.Add(New Block(New Point(i, o), EnumBlockType.BEDROCK, True, False))
                End If
                If o < 17 Then
                    If o > 7 Then
                        If o < 13 Then
                            If o > 9 Then
                                Dim c = rnd.next(1, 10)
                                If c < 4 Then
                                    world.WorldBlocks.Add(New Block(New Point(i, o), EnumBlockType.IRON_ORE, False, False))
                                ElseIf (c > 4) & (c < 7) = "TrueTrue" Then
                                    world.WorldBlocks.Add(New Block(New Point(i, o), EnumBlockType.COAL_ORE, False, False))
                                Else
                                    world.WorldBlocks.Add(New Block(New Point(i, o), EnumBlockType.STONE, False, False))
                                End If
                                Continue For
                            End If
                        End If
                        If o > 13 Then
                            If o < 17 Then
                                Dim c = rnd.next(1, 10)
                                If c < 3 Then
                                    world.WorldBlocks.Add(New Block(New Point(i, o), EnumBlockType.DIAMOND_ORE, False, False))
                                ElseIf (c > 3) & (c < 6) = "TrueTrue" Then
                                    world.WorldBlocks.Add(New Block(New Point(i, o), EnumBlockType.GOLD_ORE, False, False))
                                Else
                                    world.WorldBlocks.Add(New Block(New Point(i, o), EnumBlockType.STONE, False, False))
                                End If
                                Continue For
                            End If
                        End If
                        world.WorldBlocks.Add(New Block(New Point(i, o), EnumBlockType.STONE, False, False))
                    End If
                End If
                If o < 8 Then
                    If o > 6 Then
                        world.WorldBlocks.Add(New Block(New Point(i, o), EnumBlockType.DIRT, False, False))
                    End If
                End If
                If o = 6 Then
                    Dim r = rnd.Next(1, 10)
                    If r < 6 Then
                        world.WorldBlocks.Add(New Block(New Point(i, o), EnumBlockType.WATER, False, False))
                    Else
                        world.WorldBlocks.Add(New Block(New Point(i, o), EnumBlockType.GRASS_BLOCK, False, False))
                    End If
                End If
                If o = 5 Then
                    If (a + 5) < i Then
                        'If Not world.GetBlockAt(i, o + 1).Type = EnumBlockType.WATER Then 
                        TreeGenerator.GenerateTree(New Point(i, o), world)
                        a = i
                    End If
                End If
            Next
        Next
        Return world
    End Function

    Shared Function GenerateFlat() As WorldServer
        Dim world = New WorldServer
        For i = 0 To 60
            world.WorldBlocks.Add(New Block(New Point(i, 17), EnumBlockType.BEDROCK, False, False))
        Next
        Return world
    End Function
End Class