Public Class PlayerInfoSaveLoad

    Public Shared Function Save(arg0 As NetworkPlayer) As String
        Dim data As String = ""
        For Each i In arg0.PlayerInventory.Items
            data += $"{i.Type}?{i.Count};"
        Next
        data = data.TrimEnd(";") + "^"
        data += arg0.IsAdmin.ToString
        Return data
    End Function

    Public Shared Sub Load(arg0 As NetworkPlayer, arg1 As String)
        Try
            '  Dim point As Point = New Point(arg1.Split("^")(1).Split(";")(0), arg1.Split("^")(1).Split(";")(1))
            ' arg0.Teleport(point.X, point.Y)
            arg0.IsAdmin = arg1.Split("^")(1)

            For Each i In arg1.Split("^")(0).Split(";")
                'arg1.Split("^")(0).Split(";")
                arg0.PlayerInventory.AddItem(New ItemStack([Enum].Parse(GetType(Material), i.Split("?")(0)), CInt(i.Split("?")(1).TrimEnd("^"))))
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub

End Class
