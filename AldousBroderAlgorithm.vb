''' <summary>
''' 1. Pick a random cell as the current cell and mark it as visited.
''' 2. While there are unvisited cells: 
'''     1. Pick a random neighbour.
'''     2. If the chosen neighbour has not been visited: 
'''         1. Remove the wall between the current cell and the chosen neighbour.
'''         2. Mark the chosen neighbour as visited.
'''     3. Make the chosen neighbour the current cell.
''' </summary>
Public Class AldousBroderAlgorithm
    Private rand As New Random
    Private UnvisitedCells As New List(Of CellData)
    Public WallThiccness As Integer = 1

    Public task As New Threading.Tasks.Task(Sub() GenMaze())
    Public Structure Neighbours
        Friend Top As CellData
        Friend Bottom As CellData
        Friend Left As CellData
        Friend Right As CellData
        Public ReadOnly Property IsEmpty(ByVal val As CellData) As Boolean
            Get
                Return val.PosX = -1 Or val.PosY = -1
            End Get
        End Property
    End Structure

    Public Sub New()

        task.RunSynchronously()
    End Sub
    Private Sub GenMaze()
        Dim CellSize = CShort(TestForm.cGridPictureBox.Size.Width / TestForm.numOfCellsHor)
        For x = 0 To TestForm.numOfCellsHor
            For y = 0 To TestForm.numOfCellsVer
                UnvisitedCells.Add(TestForm.Map(x, y))
            Next
        Next
        Dim currentCellX, currentCellY As Integer

        ' Pick a random cell as the current cell
        currentCellX = rand.Next(0, TestForm.numOfCellsHor)
        currentCellY = rand.Next(0, TestForm.numOfCellsVer)

        ' Mark the cell as visited
        'TestForm.Map(currentCellX, currentCellY).Wall = True
        UnvisitedCells.Remove(TestForm.Map(currentCellX, currentCellY))

        Dim i = 0
        While UnvisitedCells.Count > 0
            Dim neighbours = GetSurroundingCells(currentCellX, currentCellY)

            ' Pick a random neighbour
            Dim randNeighbour = neighbours(rand.Next(0, neighbours.Count))

            ' if the chosen neighbour hasn't been visited
            If UnvisitedCells.FindAll(Function(uc) uc.PosX = randNeighbour.PosX And uc.PosY = randNeighbour.PosY).Count > 0 Then
                Dim diffX = randNeighbour.PosX.CompareTo(TestForm.Map(currentCellX, currentCellY).PosX)
                Dim diffY = randNeighbour.PosY.CompareTo(TestForm.Map(currentCellX, currentCellY).PosY)

                Dim fromPos As New Point
                Dim size As New Size
                Select Case diffX
                    Case -1 ' Left
                        fromPos = New Point(CInt((TestForm.Map(currentCellX, currentCellY).PosX * CellSize) - (WallThiccness / 2)), CInt((TestForm.Map(currentCellX, currentCellY).PosY * CellSize) - (WallThiccness / 2)))
                        size = New Size(WallThiccness, CInt(CellSize))
                        'Dim toPos = New Point(CInt(TestForm.Map(currentCellX, currentCellY).PosX * CShort(TestForm.cGridPictureBox.Size.Width / TestForm.numOfCellsHor)), CInt((TestForm.Map(currentCellX, currentCellY).PosY * CShort(TestForm.cGridPictureBox.Size.Width / TestForm.numOfCellsHor)) + CShort(TestForm.cGridPictureBox.Size.Width / TestForm.numOfCellsHor)))
                        Exit Select
                    Case 0
                        Select Case diffY
                            Case -1 ' Top
                                fromPos = New Point(CInt((TestForm.Map(currentCellX, currentCellY).PosX * CellSize) - (WallThiccness / 2)), CInt((TestForm.Map(currentCellX, currentCellY).PosY * CellSize) - (WallThiccness / 2)))
                                size = New Size(CInt(CellSize), WallThiccness)
                                'Dim toPos = New Point(CInt((TestForm.Map(currentCellX, currentCellY).PosX * CShort(TestForm.cGridPictureBox.Size.Width / TestForm.numOfCellsHor)) + CShort(TestForm.cGridPictureBox.Size.Width / TestForm.numOfCellsHor)), CInt(TestForm.Map(currentCellX, currentCellY).PosY * CShort(TestForm.cGridPictureBox.Size.Width / TestForm.numOfCellsHor)))
                                Exit Select
                            Case 1 ' Bottom
                                fromPos = New Point(CInt(randNeighbour.PosX * CellSize), CInt((randNeighbour.PosY * CellSize) - (WallThiccness / 2)))
                                size = New Size(CInt(CellSize), WallThiccness)
                                'Dim toPos = New Point(CInt((randNeighbour.PosX + 1) * CShort(TestForm.cGridPictureBox.Size.Width / TestForm.numOfCellsHor)), CInt((randNeighbour.PosY) * CShort(TestForm.cGridPictureBox.Size.Width / TestForm.numOfCellsHor)))
                                Exit Select
                        End Select
                    Case 1 ' Right
                        fromPos = New Point(CInt((randNeighbour.PosX * CellSize) - (WallThiccness / 2)), CInt((randNeighbour.PosY * CellSize) - (WallThiccness / 2)))
                        size = New Size(WallThiccness, CInt(CellSize))
                        'Dim toPos = New Point(CInt(randNeighbour.PosX * CellSize), CInt((randNeighbour.PosY + 1) * CellSize)
                        Exit Select
                End Select
                'TestForm.oBuffG.DrawLine(New Pen(ColorTranslator.FromHtml("#2D44F5"), WallThiccness), fromPos, toPos)
                TestForm.Walls.Add(New Rectangle(fromPos, size))
                TestForm.oBuffG.DrawRectangle(New Pen(ColorTranslator.FromHtml("#2D44F5"), WallThiccness), New Rectangle(fromPos, size))

                'Mark the chosen neighbour as visited
                UnvisitedCells.Remove(randNeighbour)


                'TestForm.oBuffG.DrawLine(New Pen(If(diffX = 1, ColorTranslator.FromHtml("#ff0000"), ColorTranslator.FromHtml("#00ff00")), 1), CInt(x), sizeX, CInt(y), sizeY)
                'TestForm.oBuffG.FillRectangle(New SolidBrush(If(diffX = 1, ColorTranslator.FromHtml("#ff0000"), ColorTranslator.FromHtml("#00ff00"))), CInt(x), CInt(y), sizeX, sizeY)

                TestForm.cGridPictureBox.Image = CType(TestForm.oBuff, Bitmap)
            End If
            currentCellX = randNeighbour.PosX
            currentCellY = randNeighbour.PosY

            i += 1
            If i > 3000 Then Exit While
        End While
        'TestForm.cGridPictureBox.Image = CType(TestForm.oBuff, Bitmap)
    End Sub
    Private Function GetSurroundingCells(xPos As Int32, yPos As Int32) As List(Of CellData)
        Dim neighbours As New List(Of CellData)

        If Not TestForm.OutOfBounds(xPos, yPos - 1) Then
            neighbours.Add(TestForm.Map(xPos, yPos - 1))
        End If
        If Not TestForm.OutOfBounds(xPos, yPos + 1) Then
            neighbours.Add(TestForm.Map(xPos, yPos + 1))
        End If
        If Not TestForm.OutOfBounds(xPos - 1, yPos) Then
            neighbours.Add(TestForm.Map(xPos - 1, yPos))
        End If
        If Not TestForm.OutOfBounds(xPos + 1, yPos) Then
            neighbours.Add(TestForm.Map(xPos + 1, yPos))
        End If
        Return neighbours
    End Function
End Class
