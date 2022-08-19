Imports System.Linq
Public Class TestForm
    Public CellList As List(Of Cell) = New List(Of Cell)

    Dim rnd As New Random


    Public gridWidth As Integer = Me.SplitContainer.Panel1.Size.Width
    Dim gridHeight As Integer = Me.SplitContainer.Panel1.Size.Height

    Public numOfCellsHor As Integer = 27
    Dim numOfCellsVer As Integer = 20
    Public cellSize As Double = gridWidth / numOfCellsHor

    Dim cPlayer As Player
    Dim cGhost1 As Ghost

    Private Sub Reset()
        Application.Restart()
        Me.Refresh()
    End Sub
    Private Sub TestForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GenGrid()

        Dim filledCells = CellList.FindAll(Function(p) p.IsPassable = True)
        If Not filledCells.Count < 1 Then
            Dim playerPos = filledCells(rnd.Next(0, filledCells.Count))
            cPlayer = New Player(CInt(cellSize), playerPos.Location)
            playerPos.Hide()


            Me.SplitContainer.Panel1.Controls.Add(cPlayer)
            GenCoins()

            Dim ghostPos = filledCells(rnd.Next(0, filledCells.Count))
            ghostPos.Hide()
            cGhost1 = New Ghost(CInt(cellSize), ghostPos.Location, CellList)
            Me.SplitContainer.Panel1.Controls.Add(cGhost1)
            'cGhost1.FindWayToPlayer(cPlayer, CellList)
            AddHandler cGhost1.Click, AddressOf Move
        End If

        Dim walls = CellList.FindAll(Function(w) w.IsPassable = False)
        Dim wall = walls(rnd.Next(0, walls.Count))

        'While Not (cPlayer.Location = cGhost1.Location)
        '    Threading.Thread.Sleep(500)
        '    cPlayer.MoveWASD(cPlayer.CurrentDirection, CellList)

        'End While
        Me.BackColor = Color.Wheat
    End Sub
    Private Sub TestForm_Shown(sender As Object, e As EventArgs) Handles Me.KeyDown
        Debug.WriteLine("doublekligg")
        cGhost1.FindWayToPlayer(cPlayer, CellList)
    End Sub

    ''' <summary>
    ''' Generate grid for pacman
    ''' </summary>
    Private Sub GenGrid()
        Dim i = 0
        For y = 1 To numOfCellsVer
            For x = 1 To numOfCellsHor

                Dim cell = New Cell(CInt(cellSize), x - 1, y - 1, False)

                AddHandler cell.Click, AddressOf Move
                Me.SplitContainer.Panel1.Controls.Add(cell)
                CellList.Add(cell)

                i += 1
            Next

        Next
        'PrimAlgorithm()
        GenerateFields()
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
        Dim cells = CellList.FindAll(Function(p)
                                         If p.Location.Y = cell.Location.Y Then
                                             If p.Location.X = cell.Location.X + cell.Size.Width Then
                                                 Return True
                                             End If
                                             If p.Location.X = cell.Location.X - cell.Size.Width Then
                                                 Return True
                                             End If
                                         End If
                                         If p.Location.X = cell.Location.X Then
                                             If p.Location.Y = cell.Location.Y + cell.Size.Height Then
                                                 Return True
                                             End If
                                             If p.Location.Y = cell.Location.Y - cell.Size.Height Then
                                                 Return True
                                             End If
                                         End If
                                     End Function)
        Return cells
    End Function

    Private Sub GenerateFields()
        Dim amountRowsVer = rnd.Next(1, CInt(numOfCellsVer / 2))
        Dim rowsWithLineVer = New List(Of List(Of Cell))
        Dim rowNumsY = New List(Of Integer)

        'Debug.WriteLine($"Amount of rowsVer: {amountRowsVer}")
        For i = 1 To amountRowsVer
            Dim rowVerNr = rnd.Next(0, numOfCellsVer)


            If rowNumsY.Contains(rowVerNr) Then ' if row already exist, then reroll
                rowVerNr = rnd.Next(0, numOfCellsVer)
                If rowNumsY.Contains(rowVerNr) Then ' if row already exist, then reroll
                    rowVerNr = rnd.Next(0, numOfCellsVer)
                End If
            End If
            'Debug.WriteLine($"RowNrY: {rowVerNr}")

            Dim rowStartPos = rnd.Next(2, CInt(numOfCellsHor / 4))
            'Debug.WriteLine($"Row{i} StartPosX: {rowStartPos}")
            Dim row = CellList.GetRange(rowStartPos + (rowVerNr * numOfCellsHor), rnd.Next(rowStartPos, numOfCellsHor - rowStartPos))

            For Each cell In row
                cell.IsPassable = True
                cell.BackColor = If(cell.IsPassable = True, Color.Black, Color.Purple)
            Next
            rowNumsY.Add(rowVerNr)
            rowsWithLineVer.Add(row)
        Next

        Dim amountRowsHor = rnd.Next(2, CInt(numOfCellsHor / 2))
        Dim rowsWithLineHor = New List(Of List(Of Cell))
        Dim rowNumsX = New List(Of Integer)

        'Debug.WriteLine($"Amount of rowsHor: {amountRowsHor}")
        For i = 1 To amountRowsHor
            Dim yMin = CellList.Find(Function(g) g.Location.Y = rowsWithLineVer.Find(Function(x) x.Min(Function(c) c.Location.Y) = x.Min(Function(c) c.Location.Y))(0).Location.Y).Location
            Dim yMax = CellList.Find(Function(g) g.Location.Y = rowsWithLineVer.Find(Function(x) x.Max(Function(c) c.Location.Y) = x.Max(Function(c) c.Location.Y))(0).Location.Y).Location
            'Dim yMinIndex = CellList.FindIndex(Function(g) g.Location = rowsWithLineVer.Find(Function(x) x.Min(Function(c) c.Location.Y) = x.Min(Function(c) c.Location.Y))(0).Location)


            Dim rowvalY = New List(Of Point)
            For Each rowline In rowsWithLineVer
                rowvalY.Add(rowline.OrderBy(Function(a) a.Location.X).ToList().First().Location)
            Next
            Dim yMinIndex = CellList.FindIndex(Function(g) g.Location.Y = rowvalY.Min(Function(k) k.Y))
            Dim xMaxIndex = CellList.FindIndex(Function(g) g.Location.Y = rowvalY.Max(Function(k) k.Y) And g.Location.X = CellList.Find(Function(k) k.Location.X = rowvalY.Min(Function(l) l.X)).Location.X)


            'Debug.WriteLine($"highest hor row: {yMin}.{yMax} - {yMinIndex} - {xMaxIndex}")
            Dim rowHorNr = rnd.Next(0, numOfCellsHor) ' number of row along the x axis



            If rowNumsX.Contains(rowHorNr) Then ' if row already exist, then reroll
                rowHorNr = rnd.Next(0, numOfCellsHor)
                If rowNumsX.Contains(rowHorNr) Then ' if row already exist, then reroll
                    rowHorNr = rnd.Next(0, numOfCellsHor)
                End If
            End If

            'Debug.WriteLine($"RowNrX: {rowHorNr}")

            'Dim rowStartPos = CellList.ElementAt(yMinIndex).Location.X
            Dim rowStartPos = rnd.Next(2, CInt(numOfCellsVer / 4))
            Dim row = CellList.FindAll(Function(c) c.Location.X = CellList.ElementAt(rowStartPos + (rowHorNr * numOfCellsVer)).Location.X And c.Location.X <= CellList.ElementAt(rowHorNr).Location.X And (c.Location.Y >= CellList.ElementAt(yMinIndex).Location.Y) And c.Location.Y <= CellList.ElementAt(xMaxIndex).Location.Y)

            For Each cell In row
                cell.IsPassable = True
                cell.BackColor = If(cell.IsPassable = True, Color.Black, Color.Purple)
            Next

            'CellList(yMinIndex).Text = "y"
            'CellList(yMinIndex).ForeColor = Color.Firebrick
            'CellList(xMaxIndex).Text = "y"
            'CellList(xMaxIndex).ForeColor = Color.Firebrick


            rowNumsX.Add(rowHorNr)
            rowsWithLineHor.Add(row)


        Next

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

        Public Sub CalculateStepsToPlayer(player As Player) ' TODO calculate actual doable steps
            Dim testLocation As Point = Me.Location
            Dim stepY As Integer = testLocation.Y.CompareTo(player.Location.Y)
            Dim stepX As Integer = testLocation.X.CompareTo(player.Location.X)
            Dim steps As Integer = 0

            While Not stepY = 0
                If stepY = 1 Then
                    ' Ghost is under player on Y
                    testLocation.Y = testLocation.Y - Me.Size.Height
                ElseIf stepY = -1 Then
                    ' Ghost is over player on Y
                    testLocation.Y = testLocation.Y + Me.Size.Height
                End If
                stepY = testLocation.Y.CompareTo(player.Location.Y)
                steps += 1
            End While
            While Not stepX = 0
                If stepX = 1 Then
                    ' Ghost is under player on Y
                    testLocation.X = testLocation.X - Me.Size.Width
                ElseIf stepX = -1 Then
                    ' Ghost is over player on Y
                    testLocation.X = testLocation.X + Me.Size.Width
                End If
                stepX = testLocation.X.CompareTo(player.Location.X)
                steps += 1
            End While
            Debug.WriteLine($"Steps to player: {steps}")

        End Sub

        Public Sub FindWayToPlayer(player As Player, cellList As List(Of Cell))
            Me.BringToFront()

            Dim ghostCell = cellList.Find(Function(f) f.Location = Me.Location)
            If int >= 50 Then
                Return
            End If
            Dim neighbourCells = GetNeighbours(ghostCell, player, cellList)
            Dim pathToPlayerCells = New List(Of Cell)

            ' Check for blindzones. 
            For Each cell In neighbourCells
                If cell.IsPassable = True Then
                    Me.Refresh()
                    player.BringToFront()
                    'Threading.Thread.Sleep(500)
                    int += 1
                    If cell.Location.Y.CompareTo(player.Location.Y) = 0 And cell.Location.X.CompareTo(player.Location.X) = 0 Then
                        Me.ForeColor = Color.DodgerBlue
                        Exit For
                        Exit Sub
                    End If
                    If cell.Location.Y.CompareTo(player.Location.Y) = -1 Then ' player is under ghost
                        Dim newLoc = New Point(Me.Location.X, Me.Location.Y + Me.Size.Height)
                        If cellList.Find(Function(c) c.Location = newLoc And c.IsPassable = True) IsNot Nothing Then
                            Me.Location = newLoc
                            cell.FlatAppearance.BorderSize = 3
                            cell.FlatAppearance.BorderColor = Color.Gold
                            FindWayToPlayer(player, cellList)
                            pathToPlayerCells.Add(cell)
                        End If

                    ElseIf cell.Location.Y.CompareTo(player.Location.Y) = 1 Then ' player is over ghost
                        Dim newLoc = New Point(Me.Location.X, Me.Location.Y - Me.Size.Height)
                        If cellList.Find(Function(c) c.Location = newLoc And c.IsPassable = True) IsNot Nothing Then
                            Me.Location = newLoc
                            cell.FlatAppearance.BorderSize = 3
                            cell.FlatAppearance.BorderColor = Color.Gold
                            FindWayToPlayer(player, cellList)
                            pathToPlayerCells.Add(cell)
                        End If
                    ElseIf cell.Location.X.CompareTo(player.Location.X) = -1 Then ' player is right of ghost
                        Dim newLoc = New Point(Me.Location.X + Me.Size.Width, Me.Location.Y)
                        If cellList.Find(Function(c) c.Location = newLoc And c.IsPassable = True) IsNot Nothing Then
                            Me.Location = newLoc
                            cell.FlatAppearance.BorderSize = 3
                            cell.FlatAppearance.BorderColor = Color.Coral
                            FindWayToPlayer(player, cellList)
                            pathToPlayerCells.Add(cell)
                        End If

                    ElseIf cell.Location.X.CompareTo(player.Location.X) = 1 Then ' player is left of ghost
                        Dim newLoc = New Point(Me.Location.X - Me.Size.Width, Me.Location.Y)
                        If cellList.Find(Function(c) c.Location = newLoc And c.IsPassable = True) IsNot Nothing Then
                            Me.Location = newLoc
                            cell.FlatAppearance.BorderSize = 3
                            cell.FlatAppearance.BorderColor = Color.Coral
                            FindWayToPlayer(player, cellList)
                            pathToPlayerCells.Add(cell)
                        End If
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
            Debug.WriteLine($"{ghostCell.Location.Y.CompareTo(cPlayer.Location.Y)}") ' -1 = ghost is over player 
            'Dim cells2 = cellList.FindAll(Function(p) p.Location.)
            Return cells
        End Function

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
        cPlayer.CurrentDirection = e.KeyCode
        cPlayer.MoveWASD(cPlayer.CurrentDirection, CellList)

        cGhost1.BringToFront()

        If cGhost1.Location = cPlayer.Location Then
            Dim result As DialogResult = MessageBox.Show("You died!\nYour wanna restart?", "Game Over 🤡", MessageBoxButtons.YesNo)
            If result = DialogResult.Yes Then
                Me.Reset()
            ElseIf result = DialogResult.No Then
                Application.Exit()
            End If
            Return
        End If
        cGameCountLabel.Text = $"Beer: {cPlayer.Gold}"
    End Sub


    Private Sub Move(sender As Object, e As EventArgs)
        If TypeOf sender Is Ghost Then
            Dim result As DialogResult = MessageBox.Show("You died!\nYour wanna restart?", "Game Over 🤡", MessageBoxButtons.YesNo)
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
            cGameCountLabel.Text = $"Beer: {cPlayer.Gold}"
        End If

    End Sub
End Class