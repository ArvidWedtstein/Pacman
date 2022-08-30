Imports System
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Drawing

Public Class TestForm

#Region " Declares "
    Public CellList As List(Of Cell) = New List(Of Cell)

    Dim rnd As New Random

    Public task As Task = New Task(Sub() GhostUpdate())

    Public gridWidth As Integer = Me.SplitContainer.Panel1.Size.Width
    Dim gridHeight As Integer = Me.SplitContainer.Panel1.Size.Height

    Public numOfCellsHor As Short = 24
    Dim numOfCellsVer As Short = 24
    Public cellSize As Double = gridWidth / numOfCellsHor


    Public cPlayer As Player
    Public cGhost1 As Ghost

    Private isDrawing As Boolean = False
    Private Const inOpened As Integer = 1
    Private Const inClosed As Integer = 2
    Private Heap As New BinaryHeap()
    Private Map(numOfCellsHor, numOfCellsVer) As CellData
    Private StartX, StartY, EndX, EndY As Short
    Private oBuff As New Bitmap(750, 750)
    Private oBuffG As Graphics = Graphics.FromImage(oBuff)
    Private PathFound, PathHunt As Boolean
    Private ParentX, ParentY As Short
#End Region

    Public Structure Neighbours
        Friend Top As Cell
        Friend Bottom As Cell
        Friend Left As Cell
        Friend Right As Cell
    End Structure

#Region " Subs "
    Function Shuffle(Of T)(collection As IEnumerable(Of T)) As List(Of T)
        Dim r As Random = New Random()
        Shuffle = collection.OrderBy(Function(a) r.Next()).ToList()
    End Function

    Private Sub Reset()
        Application.Restart()
        Me.Refresh()
    End Sub
    Private Sub TestForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GenGrid()

        Dim maze = New MazeBuilder(numOfCellsHor, numOfCellsVer)
        Dim d = maze.display(1)
        'Dim filledCells = CellList.FindAll(Function(p) p.IsPassable = True)
        'If Not filledCells.Count < 1 Then
        '    Dim playerPos = filledCells(rnd.Next(0, filledCells.Count))
        '    cPlayer = New Player(CInt(cellSize), playerPos.Location)
        '    'Dim playerPos = New Point(CInt(2 * cellSize), CInt(1 * cellSize))
        '    'cPlayer = New Player(CInt(cellSize), playerPos)
        '    'playerPos.Hide()


        '    Me.SplitContainer.Panel1.Controls.Add(cPlayer)
        '    cPlayer.BringToFront()

        '    'Intersection()
        '    'GenCoins()

        '    Dim ghostPos = filledCells(rnd.Next(0, filledCells.Count))
        '    ghostPos.Hide()
        '    cGhost1 = New Ghost(CInt(cellSize), ghostPos.Location, CellList)
        '    Me.SplitContainer.Panel1.Controls.Add(cGhost1)
        '    cGhost1.BringToFront()
        '    'cGhost1.FindWayToPlayer(cPlayer, CellList)
        '    AddHandler cGhost1.Click, AddressOf Move

        '    task.Start()
        'End If

        'Dim walls = CellList.FindAll(Function(w) w.IsPassable = False)
        'Dim wall = walls(rnd.Next(0, walls.Count))

    End Sub




    Private Sub TestForm_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles MyBase.Paint
        Render()
    End Sub

    Private Sub PicGrid_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles cGridPictureBox.MouseDown
        Dim cellSizeX = CShort(cGridPictureBox.Size.Width / numOfCellsHor)
        Dim cellSizeY = CShort(cGridPictureBox.Size.Height / numOfCellsVer)

        If e.Button <> MouseButtons.Left Then Exit Sub

        Dim xPos As Short = CShort(e.X \ cellSizeX)
        Dim yPos As Short = CShort(e.Y \ cellSizeY)
        Debug.WriteLine($"{xPos} - {yPos}")

        If cWallRadioButton.Checked Then
            If Map(xPos, yPos).Wall = True Then
                Map(xPos, yPos).Wall = False
            Else
                Map(xPos, yPos).Wall = True
            End If
        ElseIf cStartPosRadioButton.Checked Then
            StartX = xPos
            StartY = yPos
        ElseIf cEndPosRadioButton.Checked Then
            EndX = xPos
            EndY = yPos
        End If

        Render()
    End Sub



    Private Sub RunButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cRunButton.Click
        Debug.WriteLine("Find Path")
        FindPath()
    End Sub
    Private Sub ResetButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cClearButton.Click
        Dim xCnt, yCnt As Integer

        Heap.ResetHeap()

        For yCnt = 0 To numOfCellsVer
            For xCnt = 0 To numOfCellsHor
                With Map(xCnt, yCnt)
                    .DrawPath = False
                    .FCost = 0
                    .GCost = 0
                    .HCost = 0
                    .OCList = 0
                    .ParentX = 0
                    .ParentY = 0
                    '.Wall = True
                End With
            Next
        Next
        Render()
    End Sub

    Private Sub Render()
        Dim xCnt, yCnt As Int16
        Dim cellSize As Int16 = CShort(cGridPictureBox.Size.Width / numOfCellsHor)

        'Clear the background
        oBuffG.Clear(Color.Black)

        Dim icon = New Icon("C:\Users\arvid.wedtstein\Pictures\Memes\dennys2.ico")
        Dim iconGhost = New Icon("C:\Users\arvid.wedtstein\Pictures\paggmann.ico")
        oBuffG.DrawIcon(icon, StartX * cellSize, StartY * cellSize)
        oBuffG.DrawIcon(iconGhost, EndX * cellSize, EndY * cellSize)


        'Draw Walls
        For yCnt = 0 To numOfCellsVer
            For xCnt = 0 To numOfCellsHor
                'If Map(xCnt, yCnt).Wall = True Then oBuffG.FillRectangle(New SolidBrush(ColorTranslator.FromHtml("#000066")), xCnt * cellSize, yCnt * cellSize, cellSize, cellSize)
                If Map(xCnt, yCnt).Wall = True Then oBuffG.DrawRectangle(New Pen(ColorTranslator.FromHtml("#000066"), 6), xCnt * cellSize, yCnt * cellSize, cellSize, cellSize)

                If Map(xCnt, yCnt).DrawPath = True Then
                    oBuffG.FillEllipse(New SolidBrush(Color.White), xCnt * cellSize + CShort((cellSize / 2) / 2 - 1), yCnt * cellSize + CShort((cellSize / 2) / 2), CShort(cellSize / 2), CShort(cellSize / 2))
                End If
                If Map(xCnt, yCnt).Coin = True AndAlso Map(xCnt, yCnt).Wall = False Then
                    'Dim coinIcon = New Icon("C:\Users\arvid.wedtstein\Pictures\paggmann.ico")
                    'oBuffG.DrawIcon(coinIcon, StartX * cellSize, StartY * cellSize)
                    oBuffG.FillEllipse(New SolidBrush(Color.Gold), xCnt * cellSize + CShort((cellSize / 2) / 2 - 1), yCnt * cellSize + CShort((cellSize / 2) / 2), CShort(cellSize / 2), CShort(cellSize / 2))
                End If

                ' combine fields that are together
                'rows.Add(row)

                'If row.Count > 1 Then
                '    For i = 0 To row.Count - 1
                '        Dim item = row(i)
                '        If i + 1 <= row.Count - 1 AndAlso row(i + 1).First <= item.First + 1 Then
                '            Debug.WriteLine($"{item.First} - {item.Last}")
                '            If Map(row.First.First, row.First.Last).Wall = True Then oBuffG.DrawRectangle(New Pen(ColorTranslator.FromHtml("#ff0000"), 6), row.First.First * cellSize, row.First.Last * cellSize, cellSize * row.Last.First, row.Last.Last)
                '        End If
                '    Next
                'End If
            Next
        Next

        'Draw grid
        'For xCnt = 0 To CShort(numOfCellsHor + 1)
        '    oBuffG.DrawLine(New Pen(Color.White), xCnt * cellSize, 0, xCnt * cellSize, cGridPictureBox.Size.Width - 1)
        '    oBuffG.DrawLine(New Pen(Color.White), 0, xCnt * cellSize, cGridPictureBox.Size.Height - 1, xCnt * cellSize)
        'Next
        cGridPictureBox.Image = CType(oBuff, Bitmap)
    End Sub
    Private Sub FindPath()
        Dim xCnt, yCnt As Integer

        If (StartX = EndX) And (StartY = EndY) Then Exit Sub

        If Map(StartX, StartY).Wall Then Exit Sub
        If Map(EndX, EndY).Wall Then Exit Sub

        PathFound = False
        PathHunt = True

        Map(StartX, StartY).OCList = inOpened
        Heap.Add(0, StartX, StartY)

        While PathHunt
            If Heap.Count <> 0 Then
                ParentX = Heap.GetX
                ParentY = Heap.GetY

                Map(ParentX, ParentY).OCList = inClosed
                Heap.RemoveRoot()

                For yCnt = (ParentY - 1) To (ParentY + 1)
                    For xCnt = (ParentX - 1) To (ParentX + 1)

                        'Make sure we are not out of bounds
                        If xCnt <> -1 And xCnt <> numOfCellsHor + 1 And yCnt <> -1 And yCnt < numOfCellsVer Then

                            'Make sure its not on the closed list
                            If Map(xCnt, yCnt).OCList <> inClosed Then

                                'Make sure no wall
                                If Map(xCnt, yCnt).Wall = False Then

                                    'do not cut corners
                                    Dim CanWalk As Boolean = True
                                    If xCnt = ParentX - 1 Then
                                        If yCnt = ParentY - 1 Then
                                            'If Map(ParentX - 1, ParentY).Wall = True Or Map(ParentX, ParentY - 1).Wall = True Then CanWalk = False
                                            CanWalk = False
                                        ElseIf yCnt = ParentY + 1 Then
                                            'If Map(ParentX, ParentY + 1).Wall = True Or Map(ParentX - 1, ParentY).Wall = True Then CanWalk = False
                                            CanWalk = False
                                        End If
                                    ElseIf xCnt = ParentX + 1 Then
                                        If yCnt = ParentY - 1 Then
                                            'If Map(ParentX, ParentY - 1).Wall = True Or Map(ParentX + 1, ParentY).Wall = True Then CanWalk = False
                                            CanWalk = False
                                        ElseIf yCnt = ParentY + 1 Then
                                            'If Map(ParentX + 1, ParentY).Wall = True Or Map(ParentX, ParentY + 1).Wall = True Then CanWalk = False
                                            CanWalk = False
                                        End If
                                    End If

                                    If CanWalk = True Then
                                        If Map(xCnt, yCnt).OCList <> inOpened Then

                                            'Calculate GCost
                                            If Math.Abs(xCnt - ParentX) = 1 And Math.Abs(yCnt - ParentY) = 1 Then
                                                Map(xCnt, yCnt).GCost = Map(ParentX, ParentY).GCost + CShort(14)
                                            Else
                                                Map(xCnt, yCnt).GCost = Map(ParentX, ParentY).GCost + CShort(10)
                                            End If

                                            'Calculate HCost
                                            Map(xCnt, yCnt).HCost = CShort(10 * (Math.Abs(xCnt - EndX) + Math.Abs(yCnt - EndY)))
                                            Map(xCnt, yCnt).FCost = CShort(Map(xCnt, yCnt).GCost + Map(xCnt, yCnt).HCost)

                                            'Add the parent value
                                            Map(xCnt, yCnt).ParentX = ParentX
                                            Map(xCnt, yCnt).ParentY = ParentY

                                            'Add the item to the heap
                                            Heap.Add(Map(xCnt, yCnt).FCost, CShort(xCnt), CShort(yCnt))

                                            'Add the item to the open list
                                            Map(xCnt, yCnt).OCList = inOpened

                                        Else

                                            Dim AddedGCost As Integer
                                            If Math.Abs(xCnt - ParentX) = 1 And Math.Abs(yCnt - ParentY) = 1 Then
                                                AddedGCost = 14
                                            Else
                                                AddedGCost = 10
                                            End If

                                            Dim tempCost As Int16 = CShort(Map(ParentX, ParentY).GCost + AddedGCost)

                                            If tempCost < Map(xCnt, yCnt).GCost Then
                                                Map(xCnt, yCnt).GCost = tempCost
                                                Map(xCnt, yCnt).ParentX = ParentX
                                                Map(xCnt, yCnt).ParentY = ParentY
                                                If Map(xCnt, yCnt).OCList = inOpened Then
                                                    Dim NewCost As Integer = Map(xCnt, yCnt).HCost + Map(xCnt, yCnt).GCost
                                                    Heap.Add(CShort(NewCost), CShort(xCnt), CShort(yCnt))
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    Next
                Next
            Else
                PathFound = False
                PathHunt = False
                Exit Sub
            End If

            If Map(EndX, EndY).OCList = inOpened Then
                PathFound = True
                PathHunt = False
            End If
        End While

        If PathFound Then
            Dim tX As Integer = EndX
            Dim tY As Integer = EndY
            Map(tX, tY).DrawPath = True
            While True
                Dim sX As Integer = Map(tX, tY).ParentX
                Dim sY As Integer = Map(tX, tY).ParentY

                Map(sX, sY).DrawPath = True
                tX = sX
                tY = sY

                If tX = StartX And tY = StartY Then Exit While
            End While

            Render()
        End If
    End Sub




    Public Sub GhostUpdate()
        cGhost1.FindWayToPlayer(cPlayer, CellList)
    End Sub

    Private Sub TestForm_Shown(sender As Object, e As EventArgs) Handles Me.KeyDown
        'cGhost1.FindWayToPlayer(cPlayer, CellList)

    End Sub

    ''' <summary>
    ''' Generate grid for pacman
    ''' </summary>
    Private Sub GenGrid()
        Me.SplitContainer.Panel1.SuspendLayout()

        'For y = 0 To numOfCellsVer
        '    For x = 0 To numOfCellsHor

        '        Dim cell = New Cell(CInt(cellSize), x, y, False)

        '        AddHandler cell.Click, AddressOf Move
        '        Me.SplitContainer.Panel1.Controls.Add(cell)
        '        CellList.Add(cell)
        '    Next
        'Next

        'PrimAlgorithm()
        GenerateFields()
        Me.SplitContainer.Panel1.ResumeLayout(False)
    End Sub

    Private Sub GenCoins()
        Dim filledCells = CellList.FindAll(Function(p) p.IsPassable = True)
        For Each cell In filledCells
            If Not cell.Location = cPlayer.Location Then
                cell.Coin()
            End If
        Next
    End Sub
    Public Function GetNeighbours(cell As Cell) As List(Of Cell)
        Dim posatcellX, posatcellY As Integer
        With GetPosOfCell(cell)
            posatcellX = .Item1
            posatcellY = .Item2
        End With
        Dim cells = CellList.FindAll(Function(p) p.Location = GetCellAtPos(posatcellX + 1, posatcellY).Location Or
                                            p.Location = GetCellAtPos(posatcellX - 1, posatcellY).Location Or
                                            p.Location = GetCellAtPos(posatcellX, posatcellY + 1).Location Or
                                            p.Location = GetCellAtPos(posatcellX, posatcellY - 1).Location)
        Return cells
    End Function

    Private Sub GenPerlinNoise()
        Dim min As Double = Double.MaxValue
        Dim max As Double = Double.MinValue

        Dim ratio As Double = 1.5F

        For x = 0 To numOfCellsHor
            For y = 0 To numOfCellsVer

                Dim p As Double = New PerlinNoise().Perlin2D(x * ratio, y * ratio)
                If p < min Then min = p
                If p > max Then max = p

                If p > 0.47F Then
                    Map(x, y).Wall = False
                Else
                    Map(x, y).Wall = True
                    Map(x, y).Coin = True
                End If

            Next
        Next
    End Sub

    ''' <summary>
    ''' Pacman Maze rules
    ''' 1. Paths are only 1 tile thick.
    ''' 2. No sharp turns (i.e. intersections are separated by atleast 2 tiles).
    ''' 3. There are 1 Or 2 tunnels.
    ''' 4. No dead-ends.
    ''' 5. Only I, L, T, or + wall shapes are allowed, including the occasional rectangular wall.
    ''' 6. Any non-rectangular wall pieces must only be 2 tiles thick.
    ''' </summary>
    Private Sub GenerateFields()
        GenPerlinNoise()
        Dim amountRowsVer = rnd.Next(1, CInt(numOfCellsVer / 2))
        Dim rowNumsY = New List(Of Integer)

        For i = 0 To amountRowsVer
            Dim rowVerNr = rnd.Next(0, numOfCellsVer)


            If rowNumsY.Contains(rowVerNr) Then ' if row already exist, then reroll
                rowVerNr = rnd.Next(0, numOfCellsVer)
            End If

            Dim rowStartPos = rnd.Next(2, CInt(numOfCellsHor / 2))
            Dim rowEndPos = rnd.Next(rowStartPos, numOfCellsHor)

            For d = rowStartPos To rowEndPos
                Map(rowVerNr, d).Wall = True
                Map(rowVerNr, d).Coin = True
            Next

            rowNumsY.Add(rowVerNr)
        Next



        Dim amountRowsHor = rnd.Next(2, CInt(numOfCellsHor / 2))
        Dim rowNumsX = New List(Of Integer)


        For i = 0 To amountRowsHor
            Dim rowHorNr = rnd.Next(0, numOfCellsHor)


            If rowNumsX.Contains(rowHorNr) Then ' if row already exist, then reroll
                rowHorNr = rnd.Next(0, numOfCellsHor)
            End If

            Dim rowStartPos = rnd.Next(2, CInt(numOfCellsVer / 4))
            Dim rowEndPos = rnd.Next(rowStartPos, numOfCellsVer)
            For d = rowStartPos To rowEndPos
                Map(d, rowHorNr).Wall = True
                Map(d, rowHorNr).Coin = True
            Next

            rowNumsX.Add(rowHorNr)
        Next

        Render()
    End Sub
    Private Sub PrimAlgorithm()

        Dim chosenCell = CellList(rnd.Next(0, CellList.Count))
        chosenCell.IsPassable = True
        chosenCell.BackColor = Color.Black
        Dim ListOfCells = GetNeighbours(chosenCell)

        For x = 1 To 4000
            Dim RandCell = ListOfCells(rnd.Next(0, ListOfCells.Count)) ' get random neighbor
            Dim neighbours = GetNeighbours(RandCell)


            Dim cellsChecked = neighbours.FindAll(Function(p) p.IsPassable = True).Count

            If cellsChecked < 2 Then
                RandCell.IsPassable = True
                RandCell.BackColor = If(RandCell.IsPassable = True, Color.Black, Color.Purple)

                ListOfCells.AddRange(neighbours)
                RandCell.Refresh()
            End If

            ListOfCells.Remove(chosenCell)
        Next

    End Sub
    Public Function GetCellAtPos(x As Integer, y As Integer) As Cell
        If x > numOfCellsHor Then
            x = numOfCellsHor
        End If
        If y > numOfCellsVer Then
            x = numOfCellsVer
        End If
        If x < 0 Then
            x = 0
        End If
        If y < 0 Then
            y = 0
        End If
        If (y * numOfCellsHor) + x > CellList.Count Then
            Return CellList(CellList.Count)
        End If
        Return CellList((y * numOfCellsHor) + x)
    End Function

    Public Function GetPosOfCell(cell As Cell) As Tuple(Of Integer, Integer)
        Return Tuple.Create(CInt(CellList.FindAll(Function(b) b.Location.Y = cell.Location.Y).IndexOf(cell)), CInt(CellList.IndexOf(cell) / numOfCellsHor))
    End Function

#End Region

    Class Cell
        Inherits Button
        Public BackgroundWorker As ComponentModel.BackgroundWorker = New ComponentModel.BackgroundWorker

        Public IsPassable As Boolean   ' If player can go through
        Public HasCoin As Boolean = False
        Public Sub New(cellSize As Integer, x As Integer, y As Integer, Optional IsPassable As Boolean = True)
            Me.IsPassable = IsPassable
            Me.Name = $"cWall{x}{y}Button"
            Me.BackColor = If(IsPassable = True, Color.Transparent, Color.Purple)
            Me.ForeColor = Color.Gold
            Me.Size = New Size(cellSize, cellSize)
            Me.Location = New Point(CInt(Math.Floor(cellSize * x)), CInt(Math.Floor(cellSize * y)))
            Me.FlatStyle = FlatStyle.Flat
            Me.FlatAppearance.BorderSize = 0
            Me.Font = New Font("Bahnschrift", 12)

            BackgroundWorker.WorkerReportsProgress = True
            BackgroundWorker.WorkerSupportsCancellation = True
            AddHandler BackgroundWorker.DoWork, AddressOf bw_DoWork
            AddHandler BackgroundWorker.RunWorkerCompleted, AddressOf bw_RunWorkerCompleted

        End Sub

        Public Sub Coin()
            If Me.HasCoin = True Then
                Me.HasCoin = False
                Me.ResetText()
                Return
            End If
            Me.HasCoin = True
            Me.Text = "📀"
            Me.ForeColor = Color.Gold
        End Sub

        Private Sub bw_DoWork(ByVal sender As Object, ByVal e As ComponentModel.DoWorkEventArgs)
            Dim worker As ComponentModel.BackgroundWorker = CType(sender, ComponentModel.BackgroundWorker)

            Threading.Thread.Sleep(10 * 1000)
            Me.BackgroundWorker.ReportProgress(100)
        End Sub
        Private Sub bw_RunWorkerCompleted(ByVal sender As Object, ByVal e As ComponentModel.RunWorkerCompletedEventArgs)
            If e.Cancelled = True Then
                Debug.WriteLine("Canceled")
            ElseIf e.Error IsNot Nothing Then
                Debug.Fail("Error: " & e.Error.Message)
            Else
                Me.Coin() ' Add coin back after timeout
            End If
        End Sub
    End Class

    Class Player
        Inherits Button
        Public Gold As Integer
        Public LastLocation As Point
        Public CurrentDirection As Keys = Keys.W
        Public Sub New(cellSize As Integer, location As Point)
            Me.Name = $"cPlayerButton"
            Me.BackColor = Color.Red
            Me.Size = New Size(cellSize, cellSize)
            Me.Location = location
            Me.LastLocation = location
            Me.Font = New Font("Bahnschrift", 12)
        End Sub
        Public Sub Move(button As Cell, cellList As List(Of Cell))
            If Not (button.Location.X - Me.Location.X > Me.Size.Width Or 'right
                Me.Location.X - button.Location.X > Me.Size.Width Or 'left 
                Me.Location.Y - button.Location.Y > Me.Size.Height Or ' top
                button.Location.Y - Me.Location.Y > Me.Size.Height) Then
                If button.IsPassable = True Then
                    cellList.Find(Function(p) p.Location = Me.LastLocation).Show() ' show button where player stand last
                    Me.LastLocation = button.Location

                    Me.Location = button.Location
                    Me.Refresh()
                    button.Hide()
                    Me.CollectGold(button)
                End If
            End If
        End Sub

        Public Sub MoveWASD(keyCode As Keys, cellList As List(Of Cell))
            Dim newLocation As Point = New Point(Me.Location.X, Me.Location.Y)
            Select Case keyCode
                Case Keys.W
                    newLocation.Y = Me.Location.Y - Me.Size.Height
                Case Keys.A
                    newLocation.X = Me.Location.X - Me.Size.Width
                Case Keys.S
                    newLocation.Y = Me.Location.Y + Me.Size.Height
                Case Keys.D
                    newLocation.X = Me.Location.X + Me.Size.Width
            End Select

            Dim cellWherePlayerMoves = cellList.Find(Function(c) c.Location = newLocation)

            If cellWherePlayerMoves Is Nothing Then
                Return
            End If

            If cellWherePlayerMoves.IsPassable = True Then
                cellList.Find(Function(p) p.Location = Me.LastLocation).Show() ' show button where player stand last
                Me.Location = cellWherePlayerMoves.Location
                Me.LastLocation = cellWherePlayerMoves.Location
                Me.Refresh()
                cellWherePlayerMoves.Hide()

                Me.CollectGold(cellWherePlayerMoves)
            End If
        End Sub

        Private Sub CollectGold(cell As Cell)
            If cell.HasCoin Then
                cell.Coin()
                Me.Gold += 1

                If Not cell.BackgroundWorker.IsBusy Then
                    cell.BackgroundWorker.RunWorkerAsync()
                End If
            End If
        End Sub
    End Class


    Class Ghost
        Inherits Button

        Public LastLocation As Point
        Public id As String
        Public int As Integer
        Public IsChasing As Boolean = False
        Private GhostColors As List(Of Color) = New List(Of Color)({Color.Pink, Color.Turquoise, Color.Orange})
        Public BackgroundWorker As ComponentModel.BackgroundWorker = New ComponentModel.BackgroundWorker
        Public Sub New(cellSize As Integer, location As Point, cellList As List(Of Cell))
            Name = $"cGhost{Me.id}Button"
            BackColor = Color.Black
            ForeColor = Me.GhostColors(New Random().Next(0, Me.GhostColors.Count))
            Size = New Size(cellSize, cellSize)
            Me.Location = location
            LastLocation = location
            Font = New Font("Bahnschrift", 12)
            Text = "👾"
            id = Guid.NewGuid().ToString("N")


            BackgroundWorker.WorkerReportsProgress = True
            BackgroundWorker.WorkerSupportsCancellation = True
            AddHandler BackgroundWorker.DoWork, AddressOf bw_DoWork
            AddHandler BackgroundWorker.RunWorkerCompleted, AddressOf bw_RunWorkerCompleted
        End Sub

        Public Sub FindWayToPlayer(player As Player, ByRef cellList As List(Of Cell))
            'Me.BringToFront()
            Me.Invoke(Sub()
                          Me.BringToFront()
                      End Sub)

            Dim ghostCell = cellList.Find(Function(f) f.Location = Me.Location)
            'If int >= 100 Then
            '    Return
            'End If
            Dim neighbourCells = GetNeighbours(ghostCell, player, cellList)


            For Each cell In neighbourCells

                'Me.Refresh()
                Me.Invoke(Sub()
                              Me.Refresh()
                          End Sub)
                player.Invoke(Sub()
                                  player.BringToFront()
                              End Sub)
                'player.BringToFront()
                int += 1
                If Me.Location.Y.CompareTo(player.Location.Y) = 0 And Me.Location.X.CompareTo(player.Location.X) = 0 Then
                    Me.ForeColor = Color.DodgerBlue
                    Debug.WriteLine("Player Found")
                    TestForm.EndGame()
                    Exit For
                End If
                Threading.Thread.Sleep(800)
                If cell.Location.Y.CompareTo(player.Location.Y) = -1 And cell.IsPassable = True Then ' player is under ghost
                    Dim newLoc = New Point(Me.Location.X, If(cell.Location.Y.CompareTo(player.Location.Y) = -1, Me.Location.Y + Me.Size.Height, Me.Location.Y - Me.Size.Height))
                    If cellList.Find(Function(c) c.Location = newLoc And c.IsPassable = True) IsNot Nothing Then
                        'Me.Location = newLoc
                        Me.Invoke(Sub()
                                      Me.Location = newLoc
                                  End Sub)
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderColor = Color.Gold
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderSize = 3
                        FindWayToPlayer(player, cellList)
                        Return
                    End If
                ElseIf cell.Location.Y.CompareTo(player.Location.Y) = -1 And cell.IsPassable = False Then
                    Dim newLoc = New Point(If(cell.Location.X.CompareTo(player.Location.X) = -1, Me.Location.X + Me.Size.Width, Me.Location.X - Me.Size.Width), Me.Location.Y)
                    If cellList.Find(Function(c) c.Location = newLoc And c.IsPassable = True) IsNot Nothing Then
                        'Me.Location = newLoc
                        Me.Invoke(Sub()
                                      Me.Location = newLoc
                                  End Sub)
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderColor = Color.Gold
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderSize = 3
                        FindWayToPlayer(player, cellList)
                        Return
                    End If
                End If

                If cell.Location.Y.CompareTo(player.Location.Y) = 1 And cell.IsPassable = True Then ' player is over ghost
                    Dim newLoc = New Point(Me.Location.X, Me.Location.Y - Me.Size.Height)
                    If cellList.Find(Function(c) c.Location = newLoc And c.IsPassable = True) IsNot Nothing Then
                        'Me.Location = newLoc
                        Me.Invoke(Sub()
                                      Me.Location = newLoc
                                  End Sub)
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderColor = Color.Gold
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderSize = 3
                        FindWayToPlayer(player, cellList)
                        Return
                    End If
                ElseIf cell.Location.Y.CompareTo(player.Location.Y) = 1 And cell.IsPassable = False Then
                    Dim newLoc = New Point(If(cell.Location.X.CompareTo(player.Location.X) = -1, Me.Location.X + Me.Size.Width, Me.Location.X - Me.Size.Width), Me.Location.Y)
                    If cellList.Find(Function(c) c.Location = newLoc And c.IsPassable = True) IsNot Nothing Then
                        Me.Invoke(Sub()
                                      Me.Location = newLoc
                                  End Sub)
                        'Me.Location = newLoc
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderColor = Color.Gold
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderSize = 3
                        FindWayToPlayer(player, cellList)
                        Return
                    End If
                End If

                If cell.Location.X.CompareTo(player.Location.X) = -1 And cell.IsPassable = True Then ' player is right of ghost
                    Dim newLoc = New Point(Me.Location.X + Me.Size.Width, Me.Location.Y)
                    If cellList.Find(Function(c) c.Location = newLoc And c.IsPassable = True) IsNot Nothing Then
                        'Me.Location = newLoc
                        Me.Invoke(Sub()
                                      Me.Location = newLoc
                                  End Sub)
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderColor = Color.Gold
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderSize = 3
                        FindWayToPlayer(player, cellList)
                        Return
                    End If
                ElseIf cell.Location.X.CompareTo(player.Location.X) = -1 And cell.IsPassable = False Then
                    Dim newLoc = New Point(Me.Location.X, If(cell.Location.Y.CompareTo(player.Location.Y) = -1, Me.Location.Y + Me.Size.Height, Me.Location.Y - Me.Size.Height))
                    If cellList.Find(Function(c) c.Location = newLoc And c.IsPassable = True) IsNot Nothing Then
                        'Me.Location = newLoc
                        Me.Invoke(Sub()
                                      Me.Location = newLoc
                                  End Sub)
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderColor = Color.Gold
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderSize = 3
                        FindWayToPlayer(player, cellList)
                        Return
                    End If
                End If

                If cell.Location.X.CompareTo(player.Location.X) = 1 And cell.IsPassable = True Then ' player is left of ghost
                    Dim newLoc = New Point(Me.Location.X - Me.Size.Width, Me.Location.Y)
                    If cellList.Find(Function(c) c.Location = newLoc And c.IsPassable = True) IsNot Nothing Then
                        'Me.Location = newLoc
                        Me.Invoke(Sub()
                                      Me.Location = newLoc
                                  End Sub)
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderColor = Color.Gold
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderSize = 3
                        FindWayToPlayer(player, cellList)
                        Return
                    End If
                ElseIf cell.Location.X.CompareTo(player.Location.X) = 1 And cell.IsPassable = False Then
                    Dim newLoc = New Point(Me.Location.X, If(cell.Location.Y.CompareTo(player.Location.Y) = -1, Me.Location.Y + Me.Size.Height, Me.Location.Y - Me.Size.Height))
                    If cellList.Find(Function(c) c.Location = newLoc And c.IsPassable = True) IsNot Nothing Then
                        'Me.Location = newLoc
                        Me.Invoke(Sub()
                                      Me.Location = newLoc
                                  End Sub)
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderColor = Color.Gold
                        cellList.Find(Function(c) c.Location = newLoc).FlatAppearance.BorderSize = 3
                        FindWayToPlayer(player, cellList)
                        Return
                    End If
                End If
            Next

        End Sub
        Public Function GetNeighbours(ghostCell As Cell, cPlayer As Player, cellList As List(Of Cell)) As List(Of Cell)
            Dim cells = cellList.FindAll(Function(p)
                                             If p.Location.Y = ghostCell.Location.Y Then
                                                 If p.Location.X = ghostCell.Location.X + ghostCell.Size.Width Then
                                                     Return True
                                                 End If
                                                 If p.Location.X = ghostCell.Location.X - ghostCell.Size.Width Then
                                                     Return True
                                                 End If
                                             End If
                                             If p.Location.X = ghostCell.Location.X Then
                                                 If p.Location.Y = ghostCell.Location.Y + ghostCell.Size.Height Then
                                                     Return True
                                                 End If
                                                 If p.Location.Y = ghostCell.Location.Y - ghostCell.Size.Height Then
                                                     Return True
                                                 End If
                                             End If
                                         End Function)
            Return cells
        End Function

        Private Sub bw_DoWork(ByVal sender As Object, ByVal e As ComponentModel.DoWorkEventArgs)
            Dim worker As ComponentModel.BackgroundWorker = CType(sender, ComponentModel.BackgroundWorker)

            Threading.Thread.Sleep(500)
            'Me.FindWayToPlayer(Me.Player, Me.CellList)
            Me.BackgroundWorker.ReportProgress(100)
        End Sub
        Private Sub bw_RunWorkerCompleted(ByVal sender As Object, ByVal e As ComponentModel.RunWorkerCompletedEventArgs)
            If e.Cancelled = True Then
                Debug.WriteLine("Canceled")
            ElseIf e.Error IsNot Nothing Then
                Debug.Fail("Error: " & e.Error.Message)
            Else
                Debug.WriteLine("Done!")
            End If
        End Sub
    End Class


    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        'Dim allowedKeys = New List(Of Keys)({Keys.W, Keys.A, Keys.S, Keys.D})
        'If allowedKeys.Contains(e.KeyCode) Then
        '    cPlayer.CurrentDirection = e.KeyCode
        'End If
        If e.KeyCode = Keys.W Or e.KeyCode = Keys.A Or e.KeyCode = Keys.S Or e.KeyCode = Keys.D Then
            cPlayer.CurrentDirection = e.KeyCode
            cPlayer.MoveWASD(cPlayer.CurrentDirection, CellList)

            cGhost1.BringToFront()

            If cGhost1.Location = cPlayer.Location Then
                EndGame()
            End If
            cGameCountLabel.Text = $"{If(cPlayer.Gold > 1, "Coins", "Coin")}: {cPlayer.Gold}"
        End If

    End Sub

    Public Sub EndGame()
        Dim result As DialogResult = MessageBox.Show("You died!\nYour wanna restart?", "Game Over 🤡", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Me.Reset()
        ElseIf result = DialogResult.No Then
            Application.Exit()
        End If
        Return
    End Sub

    Private Sub Move(sender As Object, e As EventArgs)
        If TypeOf sender Is Ghost Then
            Dim result As DialogResult = MessageBox.Show("You died!\nYou wanna restart?", "Game Over 🤡", MessageBoxButtons.YesNo)
            If result = DialogResult.Yes Then
                Me.Reset()
            ElseIf result = DialogResult.No Then

            End If
            Return
        Else
            Debug.WriteLine($"- {sender.GetType()}")
            Dim btn As Cell = CType(sender, Cell)
            Debug.WriteLine($"{btn.Name} - {sender.GetType()}")
            cPlayer.Move(btn, CellList)
            cGameCountLabel.Text = $"{If(cPlayer.Gold > 1, "Coins", "Coin")}: {cPlayer.Gold}"
        End If

    End Sub


End Class

