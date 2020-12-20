Public Class OpaqueRichTextBox
    Inherits RichTextBox

    Private Shared ReadOnly DefaultBackground As Color = Color.Transparent
    Private TransparentImage As Image

    Public Sub New()
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.Opaque, True)
        Me.SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        BackColor = DefaultBackground
    End Sub

    Public Overrides Property BackColor() As System.Drawing.Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            MyBase.BackColor = value
        End Set
    End Property

    Public Property Image() As Image
        Get
            Return TransparentImage
        End Get
        Set(ByVal value As Image)
            TransparentImage = value
            Invalidate()
        End Set
    End Property

    Protected Overrides ReadOnly Property CreateParams() As System.Windows.Forms.CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H20
            Return cp
        End Get
    End Property

    ' Infrastructure to cause the default background to be transparent
    Public Function ShouldSerializeBackColor() As Boolean
        Return BackColor = DefaultBackground
    End Function

    ' Infrastructure to cause the default background to be transparent
    Public Sub ResetBackground()
        BackColor = DefaultBackground
    End Sub

End Class