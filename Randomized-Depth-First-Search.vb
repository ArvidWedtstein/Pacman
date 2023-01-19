''' <summary>
''' 1. Given a current cell as a parameter
''' 2. Mark the current cell As visited
''' 3. While the current cell has any unvisited neighbour cells
'''     1. Choose one Of the unvisited neighbours
'''     2. Remove the wall between the current cell And the chosen cell
'''     3. Invoke the routine recursively For a chosen cell
''' </summary>
Public Class Randomized_Depth_First_Search
    Public Enum Directions As Integer
        N = 1 << 0
        S = 1 << 1
        W = 1 << 2
        E = 1 << 3
    End Enum
    Public Structure CellData
        Public index As Integer
        Public direction As Directions
    End Structure

    Public width As Integer = TestForm.cGridPictureBox.Size.Width
    Public height As Integer = TestForm.cGridPictureBox.Size.Height

    Public fillColor As Color = Color.Red

    Public cellSize As Integer = CInt(TestForm.cellSize),
        cellSpacing As Integer = CInt(TestForm.cellSize),
        cellWidth2 As Integer = CInt(Math.Floor((width - cellSpacing) / (cellSize + cellSpacing))),
        cellHeight2 As Integer = CInt(Math.Floor((height - cellSpacing) / (cellSize + cellSpacing))),
        cellWidth As Integer = TestForm.numOfCellsHor,
        cellHeight As Integer = TestForm.numOfCellsVer,
        cells As Integer() = New Integer(cellWidth * cellHeight) {},
        frontier As List(Of CellData) = New List(Of CellData)


    Public Sub New()

        'TestForm.oBuffG.ScaleTransform(0.5, 0.5)
        'TestForm.oBuffG.TranslateTransform(
        '    CInt(Math.Round((width - cellWidth * cellSize - (cellWidth + 1) * cellSpacing) / 2)),
        '    CInt(Math.Round((height - cellHeight * cellSize - (cellHeight + 1) * cellSpacing) / 2))
        ')

        'Set Every Cell to invisisble
        For i = 0 To cells.Length - 1
            cells(i) = -1
        Next
        Dim start = (cellHeight - 1) * cellWidth
        cells(start) = 0

        fillCell(start)
        Dim cell = New CellData
        cell.index = start
        cell.direction = Directions.N
        frontier.Add(cell)

        Dim cell2 = New CellData
        cell2.index = start
        cell2.direction = Directions.E
        frontier.Add(cell2)


        Dim done As Boolean = False,
            k As Integer = 0

        While (k < 50 And Not done = True)
            k += 1
            done = exploreFrontier()
            If Not (done = False) Then

                Exit While
            End If

        End While
        TestForm.Render()
    End Sub

    Public Function exploreFrontier() As Boolean
        Dim edge As CellData

        If frontier.Count = 0 Then
            Debug.WriteLine($"BRRRRROOOo")
            Return True
        End If
        edge = frontier(frontier.Count - 1)

        Dim i0 As Integer = edge.index,
            d0 As Directions = edge.direction,
            i1 As Integer = i0 + If(d0 = Directions.N, -cellWidth, If(d0 = Directions.S, cellWidth, If(d0 = Directions.W, -1, +1))),
            x0 As Integer = i0 Mod cellWidth,
            y0 As Integer = CInt(i0 / cellWidth) Or 0,
            x1 As Integer,
            y1 As Integer,
            d1 As Integer,
            open As Boolean = cells(i1) = -1


        frontier.RemoveAt(frontier.Count - 1)

        fillColor = If(open = True, Color.White, Color.Red)
        If CInt(d0) = CInt(Directions.N) Then
            fillSouth(i1)
            x1 = x0
            y1 = y0 - 1
            d1 = CInt(Directions.S)
        ElseIf CInt(d0) = CInt(Directions.S) Then
            fillSouth(i0)
            x1 = x0
            y1 = y0 + 1
            d1 = CInt(Directions.N)
        ElseIf CInt(d0) = CInt(Directions.W) Then
            fillEast(i1)
            x1 = x0 - 1
            y1 = y0
            d1 = CInt(Directions.E)
        Else
            fillEast(i0)
            x1 = x0 + 1
            y1 = y0
            d1 = CInt(Directions.W)
        End If

        If open = True Then
            fillCell(i1)
            cells(i0) = cells(i0) Or d0 'If(cells(i0) = -1, d0, cells(i0))
            cells(i1) = cells(i1) Or d1 ' If(cells(i1) = -1, d1, cells(-i1))
            fillColor = Color.Magenta

            Dim m As Integer = 0
            If y1 > 0 AndAlso cells(i1 - cellWidth) = -1 Then
                fillSouth(i1 - cellWidth)
                addCell(i1, Directions.N)
                m = m.Incr
            End If

            If y1 < cellHeight - 1 AndAlso cells(i1 + cellWidth) = -1 Then
                fillSouth(i1)
                addCell(i1, Directions.S)
                m = m.Incr
            End If

            If x1 > 0 AndAlso cells(i1 - 1) = -1 Then
                addCell(i1, Directions.W)
                m = m.Incr
            End If

            If x1 < cellWidth - 1 AndAlso cells(i1 + 1) = -1 Then
                fillEast(i1)
                addCell(i1, Directions.E)
                m = m.Incr
            End If

            'shuffle(frontier, frontier.Count - m, frontier.Count)
            frontier = Shuffle2(frontier)
        End If
        Return False
    End Function

    Public Function addCell(index As Integer, direction As Directions) As CellData
        Dim cell = New CellData
        cell.index = index
        cell.direction = direction
        frontier.Add(cell)
        Return cell
    End Function


    Public Sub fillCell(index As Integer)

        Dim row = index Mod cellWidth
        Dim col = CInt(index / cellWidth) Or 0

        'Debug.WriteLine($"{Reflection.MethodInfo.GetCurrentMethod().Name}: {index} - X: [{row}], Y: [{col}] - W: [{cellSize}]")

        'TestForm.Map(i, j).Wall = True

        Dim xPos = row * cellSize + (row + 1)
        Dim yPos = col * cellSize + (col + 1) * cellSpacing

        Debug.WriteLine($"{Reflection.MethodInfo.GetCurrentMethod().Name}: {index} - X: [{xPos}], Y: [{yPos}] - W: [{cellSize}]")
        TestForm.oBuffG.FillRectangle(New SolidBrush(fillColor), xPos, yPos, cellSize, cellSize)
    End Sub
    Public Sub fillEast(index As Integer)
        Dim row = index Mod TestForm.numOfCellsVer
        Dim col = CInt(index / TestForm.numOfCellsHor) Or 0

        'Debug.WriteLine($"{Reflection.MethodInfo.GetCurrentMethod().Name}: {index} - X: [{row}], Y: [{col}] - W: [{cellSize}]")

        'TestForm.Map(col, row).Wall = True

        Dim xPos = (row + 1) * (cellSize + cellSpacing)
        Dim yPos = col * cellSize + (col + 1) * cellSpacing

        Debug.WriteLine($"{Reflection.MethodInfo.GetCurrentMethod().Name}: {index} - X: [{xPos}], Y: [{yPos}] - W: [{cellSize}]")
        TestForm.oBuffG.FillRectangle(New SolidBrush(fillColor), xPos, yPos, cellSpacing, cellSize)
    End Sub
    Public Sub fillSouth(index As Integer)
        Dim row = index Mod TestForm.numOfCellsHor
        Dim col = CInt(index / TestForm.numOfCellsHor) Or 0

        'Debug.WriteLine($"{Reflection.MethodInfo.GetCurrentMethod().Name}: {index} - X: [{row}], Y: [{col}] - W: [{cellSize}]")


        'TestForm.Map(i, j).Wall = True

        Dim xPos = row * cellSize + (row + 1) * cellSpacing
        Dim yPos = (col + 1) * (cellSize + cellSpacing)

        Debug.WriteLine($"{Reflection.MethodInfo.GetCurrentMethod().Name}: {index} - X: [{xPos}], Y: [{yPos}] - W: [{cellSize}]")
        TestForm.oBuffG.FillRectangle(New SolidBrush(fillColor), xPos, yPos, cellSize, cellSpacing)
    End Sub

    Public Function Shuffle2(Of T)(list As IList(Of T)) As List(Of T)
        Dim r As Random = New Random()
        For i = 0 To list.Count - 1
            Dim index As Integer = r.Next(i, list.Count)
            If i <> index Then
                ' swap list(i) and list(index)
                Dim temp As T = list(i)
                list(i) = list(index)
                list(index) = temp
            End If
        Next
        Return New List(Of T)(list)
    End Function
    Public Function shuffle(array As List(Of CellData), i0 As Integer, i1 As Integer) As List(Of CellData)
        Dim m = i1 - i0,
            t As CellData,
            i As Integer ',
        'j As Integer

        While (m <> 0)
            Debug.WriteLine($"M1: {m}")
            i = CInt(New Random().NextDouble() * If(m.Decr() <= 0, 0, m))
            Debug.WriteLine($"M2: {m}")
            t = array(m + i0)
            array(m + i0) = array(i + i0)
            array(i + i0) = t
        End While
        Return array
    End Function
End Class

