Public Class Inventory
    Public Items As List(Of ItemStack)
    Public Owner As NetworkPlayer
    Public Sub AddItem(arg0 As ItemStack)
        Items.Add(arg0)
    End Sub
    Sub New(arg0 As NetworkPlayer)
        Owner = arg0
        Items = New List(Of ItemStack)
    End Sub
    Function CountOf(m As Material) As Integer
        For Each i In Items
            If i.Type = m Then
                Return i.Count
            End If
        Next
        Return -1
    End Function
End Class
