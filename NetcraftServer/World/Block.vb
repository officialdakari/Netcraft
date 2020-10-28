Public Class Block

    Property Position As Point
    Property Type As EnumBlockType
    Property Unbreakable As Boolean
    Property IsBackground As Boolean
    Property Rectangle As Rectangle = New Rectangle(Position, New Size(32, 32))

    Public Sub New(arg0 As Point, arg1 As EnumBlockType, arg2 As Boolean, arg3 As Boolean)
        Position = arg0
        Type = arg1
        Unbreakable = arg2
        IsBackground = arg3
    End Sub

End Class
