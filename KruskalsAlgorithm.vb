Imports System.Linq
''' <summary>
''' 1. Create a list of all walls, and create a set for each cell, each containing just that one cell.
''' 2. For each wall, in some random order: 
'''     1. If the cells divided by this wall belong to distinct sets: 
'''         1. Remove the current wall.
'''         2. Join the sets of the formerly divided cells.
''' </summary>
Public Class KruskalsAlgorithm
    Public WallList As New List(Of CellData)
    Public Sets As New List(Of List(Of CellData))
    Public Sub New()
        For x = 0 To TestForm.numOfCellsHor - 1
            For y = 0 To TestForm.numOfCellsVer - 1
                TestForm.Map(x, y).Wall = True
                WallList.Add(TestForm.Map(x, y))
                Sets.Add(New List(Of CellData)({TestForm.Map(x, y)}))
            Next
        Next
        TestForm.Map(WallList((5 * 24) + 5).PosX, WallList((5 * 24) + 5).PosY).Wall = False
        TestForm.Render()
        For Each wall In TestForm.Shuffle(WallList)

            Dim divided = GetDividedCells(TestForm.Map(wall.PosX, wall.PosY))

            Dim a As List(Of CellData)
            For Each i In divided
                'TestForm.Map(wall.PosX, wall.PosY).Wall = False
                If Sets.Find(Function(c) c.Find(Function(h) h.PosX = i(0).PosX AndAlso h.PosY = i(0).PosY).PosX = i(0).PosX) IsNot Nothing AndAlso Sets.Find(Function(c) c.FindAll(Function(h) h.PosX = i(1).PosX AndAlso h.PosY = i(1).PosY) IsNot Nothing) IsNot Nothing Then
                    WallList.Remove(wall)
                    TestForm.Map(wall.PosX, wall.PosY).Wall = False

                    ' Join the two divided cells sets
                    Dim item = Sets.Find(Function(c) c.Find(Function(h) h.PosX = i(1).PosX AndAlso h.PosY = i(1).PosY).PosX = i(1).PosX AndAlso c.Find(Function(h) h.PosX = i(1).PosX AndAlso h.PosY = i(1).PosY).PosY = i(1).PosY)
                    Dim currentSett = Sets.Find(Function(c) c.Find(Function(h) h.PosX = i(0).PosX AndAlso h.PosY = i(0).PosY).PosX = i(0).PosX)
                    If currentSett IsNot Nothing AndAlso item IsNot Nothing Then currentSett.Add(item(0))
                    'Sets.Remove(Sets.Find(Function(c) c.Equals(New List(Of CellData)({wall}))))
                    'currentSett.Add(wall)
                    Sets.Remove(item)

                End If
            Next
        Next
        Dim connectedCells = From sett In Sets
                             Where sett.Count > 1
                             Select sett
        For Each sett In connectedCells
            Debug.WriteLine($"ROW__--------------")
            For Each cell In sett
                Debug.WriteLine($"{cell.PosX} - {cell.PosY}")
            Next
            'TestForm.oBuffG.DrawPath(New Pen(Color.Red), New Drawing2D.GraphicsPath().AddLine())
        Next
    End Sub

    Private Function GetDividedCells(cell As CellData) As List(Of List(Of CellData))
        Dim list = New List(Of List(Of CellData))

        If TestForm.OutOfBounds(cell.PosX - 1, cell.PosY) = False AndAlso TestForm.OutOfBounds(cell.PosX + 1, cell.PosY) = False Then
            list.Add(New List(Of CellData)({TestForm.Map(cell.PosX - 1, cell.PosY), TestForm.Map(cell.PosX + 1, cell.PosY)}))
        End If
        If TestForm.OutOfBounds(cell.PosX, cell.PosY - 1) = False AndAlso TestForm.OutOfBounds(cell.PosX, cell.PosY + 1) = False Then
            list.Add(New List(Of CellData)({TestForm.Map(cell.PosX, cell.PosY - 1), TestForm.Map(cell.PosX, cell.PosY + 1)}))
        End If
        Return list
    End Function
End Class
