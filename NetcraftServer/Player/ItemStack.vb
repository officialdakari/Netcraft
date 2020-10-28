Public Class ItemStack
    Property Type As Material
    Property Count As Integer
    Public Sub New(arg0 As Material, arg1 As Integer)
        Type = arg0
        Count = arg1
    End Sub
End Class
