Public Class EntityPlayer
    Property Name As String
    Property UUID As String
    Property ItemInHand As String = ""
    Property ItemInImage As Image
    Property ItemInImageFlipped As Image
    Property Location As Point
    Property R1 As TransparentPicBox
    Property LastWalk As Integer = 0
    WithEvents Render As PictureBox
    Public Sub New(arg0 As String, arg1 As String, arg2 As Point, arg3 As PictureBox)
        Name = arg0
        UUID = arg1
        Location = arg2
        Render = arg3

        R1 = New TransparentPicBox With {
        .Name = "Testrender",
        .BackColor = Color.Transparent,
        .SizeMode = PictureBoxSizeMode.StretchImage
        }

        Form1.Controls.Add(R1)
    End Sub
    Private Sub Test() Handles Render.LocationChanged
        If LastWalk = 1 Then
            Render.Image = Form1.playerSkin
        Else
            Render.Image = Form1.playerSkinFlip
        End If
        Try
            If IsNothing(ItemInImageFlipped) Then
                R1.Hide()
                Exit Sub
            End If
            If IsNothing(ItemInImage) Then
                R1.Hide()
                Exit Sub
            End If
            R1.Show()
            Dim lc = Render.Location
            If ItemInImage.Equals(Nothing) Then Exit Sub
            If LastWalk = 1 Then
                lc.X += Render.Width - 10
                R1.Image = ItemInImage
            Else
                lc.X -= R1.Width - 10
                R1.Image = ItemInImageFlipped
            End If
            lc.Y += 55 - (R1.Height / 2)
            R1.Size = New Size(32, 32)
            R1.SizeMode = PictureBoxSizeMode.StretchImage
            R1.BringToFront()

            R1.Location = lc
        Catch ex As Exception
            R1.Hide()
        End Try
    End Sub
    Sub SetItemInHand(i As Image, iflipped As Image, str As String)
        If Not IsNothing(i) Then R1.Image = i
        ItemInImage = i
        ItemInImageFlipped = iflipped
        ItemInHand = str
        Render.Update()
        R1.Update()
    End Sub

    Sub Remove()
        If Form1.InvokeRequired Then
            Form1.Invoke(New MethodInvoker(AddressOf Remove))
        Else
            Form1.Controls.Remove(R1)
        End If
    End Sub

End Class
