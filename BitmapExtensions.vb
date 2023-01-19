Imports System.Runtime.CompilerServices
Module BitmapExtensions
    <Extension()>
    Public Function Merge(ByRef img1 As Bitmap, ByVal bmp As Bitmap) As Bitmap
        Dim bmpNew As New Bitmap(img1.Width, img1.Height)
        Dim g As Graphics = Graphics.FromImage(bmpNew)

        g.DrawImage(img1, 0, 0, img1.Width, img1.Height)
        g.DrawImage(bmp, 0, 0, img1.Width, img1.Height)
        g.Dispose()
        Return bmp
    End Function
End Module
