''' <summary>
''' 1. Choose the initial cell, mark it as visited and push it to the stack
''' 2. While the stack is not empty 
'''     1. Pop a cell from the stack and make it a current cell
'''     2. If the current cell has any neighbours which have not been visited 
'''         1. Push the current cell to the stack
'''         2. Choose one of the unvisited neighbours
'''         3. Remove the wall between the current cell and the chosen cell
'''         4. Mark the chosen cell as visited and push it to the stack
''' </summary>
Public Class DepthOfRecursion
    Private rand As New Random
    Public CellStack As New List(Of CellData)
    Public VisitedCells As New List(Of CellData)
    Public CurrentCell As CellData
    Public WallThiccness As Integer = 1
    Public Sub New()
        Dim CellSize = CShort(TestForm.cGridPictureBox.Size.Width / TestForm.numOfCellsHor)

        For x = 0 To TestForm.numOfCellsHor
            For y = 0 To TestForm.numOfCellsVer
                Dim fromPos = New Point(CInt((x * CellSize) - (WallThiccness / 2)), CInt((y * CellSize) - (WallThiccness / 2)))
                Dim size = New Size(WallThiccness, CInt(CellSize))
                TestForm.Walls.Add(New Rectangle(fromPos, size))
                size = New Size(CInt(CellSize), WallThiccness)
                TestForm.Walls.Add(New Rectangle(fromPos, size))
            Next
        Next

        Dim randX = rand.Next(0, TestForm.numOfCellsHor)
        Dim randY = rand.Next(0, TestForm.numOfCellsVer)

        CurrentCell = TestForm.Map(randX, randY)
        VisitedCells.Add(CurrentCell)
        CellStack.Add(CurrentCell)


        While CellStack.Count > 0
            'CurrentCell = CellStack(rand.Next(0, CellStack.Count))
            'CellStack.Remove(CurrentCell)
            CurrentCell = CellStack.Pop()

            Dim unvistitedNeighbours = GetSurroundingCells()
            If unvistitedNeighbours.Count > 0 Then
                CellStack.Add(CurrentCell)

                Dim chosenCell = unvistitedNeighbours(rand.Next(0, unvistitedNeighbours.Count))


                Dim diffX = CurrentCell.PosX.CompareTo(chosenCell.PosX)
                Dim diffY = CurrentCell.PosY.CompareTo(chosenCell.PosY)

                Dim fromPos As New Point
                Dim size As New Size
                Select Case diffX
                    Case -1 ' Left
                        fromPos = New Point(CInt((CurrentCell.PosX * CellSize) - (WallThiccness / 2)), CInt((CurrentCell.PosY * CellSize) - (WallThiccness / 2)))
                        size = New Size(WallThiccness, CInt(CellSize))
                        Exit Select
                    Case 0
                        Select Case diffY
                            Case -1 ' Top
                                fromPos = New Point(CInt((CurrentCell.PosX * CellSize) - (WallThiccness / 2)), CInt((CurrentCell.PosY * CellSize) - (WallThiccness / 2)))
                                size = New Size(CInt(CellSize), WallThiccness)

                                Exit Select
                            Case 1 ' Bottom
                                fromPos = New Point(CInt(chosenCell.PosX * CellSize), CInt((chosenCell.PosY * CellSize) - (WallThiccness / 2)))
                                size = New Size(CInt(CellSize), WallThiccness)
                                Exit Select
                        End Select
                    Case 1 ' Right
                        fromPos = New Point(CInt((chosenCell.PosX * CellSize) - (WallThiccness / 2)), CInt((chosenCell.PosY * CellSize) - (WallThiccness / 2)))
                        size = New Size(WallThiccness, CInt(CellSize))
                        Exit Select
                End Select
                ' REMOVE the wall between the current cell and the chosen cell

                TestForm.Walls.Remove(New Rectangle(fromPos, size))
                'TestForm.oBuffG.DrawRectangle(New Pen(ColorTranslator.FromHtml("#2D44F5"), WallThiccness), New Rectangle(fromPos, size))

                VisitedCells.Add(chosenCell)
                CellStack.Add(chosenCell)

            End If
        End While
    End Sub
    Private Function GetSurroundingCells() As List(Of CellData)
        Dim neighbours As New List(Of CellData)


        If Not TestForm.OutOfBounds(CurrentCell.PosX, CurrentCell.PosY - 1) AndAlso VisitedCells.FindAll(Function(c) c.PosX = CurrentCell.PosX AndAlso c.PosY = CurrentCell.PosY - 1).Count = 0 Then
            neighbours.Add(TestForm.Map(CurrentCell.PosX, CurrentCell.PosY - 1))
        End If
        If Not TestForm.OutOfBounds(CurrentCell.PosX, CurrentCell.PosY + 1) AndAlso VisitedCells.FindAll(Function(c) c.PosX = CurrentCell.PosX AndAlso c.PosY = CurrentCell.PosY + 1).Count = 0 Then
            neighbours.Add(TestForm.Map(CurrentCell.PosX, CurrentCell.PosY + 1))
        End If
        If Not TestForm.OutOfBounds(CurrentCell.PosX - 1, CurrentCell.PosY) AndAlso VisitedCells.FindAll(Function(c) c.PosX = CurrentCell.PosX - 1 AndAlso c.PosY = CurrentCell.PosY).Count = 0 Then
            neighbours.Add(TestForm.Map(CurrentCell.PosX - 1, CurrentCell.PosY))
        End If
        If Not TestForm.OutOfBounds(CurrentCell.PosX + 1, CurrentCell.PosY) AndAlso VisitedCells.FindAll(Function(c) c.PosX = CurrentCell.PosX + 1 AndAlso c.PosY = CurrentCell.PosY).Count = 0 Then
            neighbours.Add(TestForm.Map(CurrentCell.PosX + 1, CurrentCell.PosY))
        End If
        Return neighbours
    End Function
End Class
