Public Class SaveLoad
    Public Function Save(arg0 As WorldServer) As String
        Dim world As String = ""

        For Each block In arg0.WorldBlocks
            world += $"{block.Type.ToString}?{block.Position.X.ToString}?{block.Position.Y.ToString}?{block.Unbreakable}?{block.IsBackground};"
        Next

        Return world.TrimEnd(";")
    End Function
    Public Function Load(arg0 As String) As WorldServer
        Dim world As WorldServer = New WorldServer()

        For Each block In arg0.Split(";")
            Dim blockdata As String() = block.Split("?")
            world.WorldBlocks.Add(New NetcraftServer.Block(New Point(CInt(blockdata(1)), CInt(blockdata(2))), [Enum].Parse(GetType(EnumBlockType), blockdata(0)), blockdata(3), blockdata(4)))
        Next

        Return world
    End Function
End Class
