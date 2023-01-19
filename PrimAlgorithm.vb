''' <summary>
''' 1. Start with a grid full of walls.
''' 2. Pick a cell, mark it as part of the maze. Add the walls of the cell to the wall list.
''' 3. While there are walls in the list: 
'''     1. Pick a random wall from the list. If only one of the cells that the wall divides is visited, then: 
'''         1. Make the wall a passage and mark the unvisited cell as part of the maze.
'''         2. Add the neighboring walls of the cell to the wall list.
'''     2. Remove the wall from the list.
''' </summary>
Public Class PrimAlgorithm
    Private rand As New Random
    Private WallList As New List(Of CellData)
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
        ' Start with setting all cells to walls
        For x = 0 To TestForm.numOfCellsHor
            For y = 0 To TestForm.numOfCellsVer
                TestForm.Map(x, y).Wall = True
            Next
        Next
        Dim randX = rand.Next(0, TestForm.numOfCellsHor)
        Dim randY = rand.Next(0, TestForm.numOfCellsVer)

        TestForm.Map(randX, randY).Wall = False
        Dim neighbours As Neighbours = GetSurroundingCells(randX, randY)
        If neighbours.IsEmpty(neighbours.Top) = False Then WallList.Add(neighbours.Top)
        If neighbours.IsEmpty(neighbours.Bottom) = False Then WallList.Add(neighbours.Bottom)
        If neighbours.IsEmpty(neighbours.Left) = False Then WallList.Add(neighbours.Left)
        If neighbours.IsEmpty(neighbours.Right) = False Then WallList.Add(neighbours.Right)

        Debug.WriteLine($"{WallList.Count}")
        While WallList.Count > 0
            Dim randCell = WallList(rand.Next(0, WallList.Count))
            Dim neighbours2 As Neighbours = GetSurroundingCells(randCell.PosX, randCell.PosY)
            Dim exploredNeighbours = 0

            If neighbours2.IsEmpty(neighbours2.Top) = False Then exploredNeighbours += 1
            If neighbours2.IsEmpty(neighbours2.Bottom) = False Then exploredNeighbours += 1
            If neighbours2.IsEmpty(neighbours2.Left) = False Then exploredNeighbours += 1
            If neighbours2.IsEmpty(neighbours2.Right) = False Then exploredNeighbours += 1

            If exploredNeighbours = 1 Then
                TestForm.Map(randCell.PosX, randCell.PosY).Wall = False

                neighbours2 = GetSurroundingCells(randX, randY)
                If neighbours2.IsEmpty(neighbours2.Top) = False AndAlso Not WallList.Contains(neighbours2.Top) Then WallList.Add(neighbours2.Top)
                If neighbours2.IsEmpty(neighbours2.Bottom) = False AndAlso Not WallList.Contains(neighbours2.Bottom) Then WallList.Add(neighbours2.Bottom)
                If neighbours2.IsEmpty(neighbours2.Left) = False AndAlso Not WallList.Contains(neighbours2.Left) Then WallList.Add(neighbours2.Left)
                If neighbours2.IsEmpty(neighbours2.Right) = False AndAlso Not WallList.Contains(neighbours2.Right) Then WallList.Add(neighbours2.Right)
            End If
            WallList.Remove(randCell)
        End While

    End Sub

    Private Function GetSurroundingCells(xPos As Int32, yPos As Int32) As Neighbours
        Dim neighbours As New Neighbours
        'Wall = True

        Dim Invalid = New CellData()
        With Invalid
            .PosX = -1
            .PosY = -1
        End With
        If Not TestForm.OutOfBounds(xPos, yPos - 1) AndAlso TestForm.Map(xPos, yPos - 1).Wall = True Then
            neighbours.Top = TestForm.Map(xPos, yPos - 1)
        Else
            neighbours.Top = Invalid
        End If
        If Not TestForm.OutOfBounds(xPos, yPos + 1) AndAlso TestForm.Map(xPos, yPos + 1).Wall = True Then
            neighbours.Bottom = TestForm.Map(xPos, yPos + 1)
            'TestForm.Map(xPos, yPos + 1).Wall = True
        Else
            neighbours.Bottom = Invalid
        End If
        If Not TestForm.OutOfBounds(xPos - 1, yPos) AndAlso TestForm.Map(xPos - 1, yPos).Wall = True Then
            neighbours.Left = TestForm.Map(xPos - 1, yPos)
            'TestForm.Map(xPos - 1, yPos).Wall = True
        Else
            neighbours.Left = Invalid
        End If
        If Not TestForm.OutOfBounds(xPos + 1, yPos) AndAlso TestForm.Map(xPos + 1, yPos).Wall = True Then
            neighbours.Right = TestForm.Map(xPos + 1, yPos)
            'TestForm.Map(xPos + 1, yPos).Wall = True
        Else
            neighbours.Right = Invalid
        End If
        Return neighbours
    End Function
End Class
