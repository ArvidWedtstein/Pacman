Imports System
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Drawing
Imports DevExpress.XtraScheduler
Imports System.Threading
Public Class TestForm

#Region " Declares "
    Dim rnd As New Random

    Public Walls As New List(Of Rectangle)

    Public gridWidth As Integer = Me.SplitContainer.Panel1.Size.Width
    Dim gridHeight As Integer = Me.SplitContainer.Panel1.Size.Height

    Public numOfCellsHor As Short = 40
    Public numOfCellsVer As Short = 40
    Public cellSize As Double = gridWidth / numOfCellsHor ' 8

    Public playerPosX As Integer = 0
    Public playerPosY As Integer = 0

    Public cPlayer As Player
    Public cGhost1 As Ghost


    Public Map(numOfCellsHor, numOfCellsVer) As CellData
    Public oBuff As New Bitmap(750, 750)

    Public oBuffG As Graphics = Graphics.FromImage(oBuff)

    Public ShowCollision As Boolean = False

    Public AldousBroderMaze As AldousBroderAlgorithm
    Public DepthOfRecursion As DepthOfRecursion
#End Region

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
        'oBuff = New Bitmap(cGridPictureBox.Width, cGridPictureBox.Height)
        cGameTimer.Start()
        'Debug.WriteLine(cPerformanceCounter.NextValue())
        cPlayer = New Player()
        cGhost1 = New Ghost()
        'GenGrid()

        Render()
        'Dim primMaze = New PrimAlgorithm()

        AldousBroderMaze = New AldousBroderAlgorithm()

        cFPSCircularGauge.Scales(0).Value = 40

        'DepthOfRecursion = New DepthOfRecursion()
        'Dim kruskalMaze = New KruskalsAlgorithm()
        'cGridPictureBox.Bounds = New Rectangle(0, 0, cGridPictureBox.Width, cGridPictureBox.Height)
        Render()

        'Dim maze = New Randomized_Depth_First_Search()

        ' Super Mario 
        Console.Beep(659, 125)
        Console.Beep(659, 125)
        Thread.Sleep(125)
        Console.Beep(659, 125)
        Thread.Sleep(167)
        Console.Beep(523, 125)
        Console.Beep(659, 125)
        Thread.Sleep(125)
        Console.Beep(784, 125)
        Thread.Sleep(375)
        Console.Beep(392, 125)
        Thread.Sleep(375)
        Console.Beep(523, 125)
        Thread.Sleep(250)
        Console.Beep(392, 125)
        Thread.Sleep(250)
        Console.Beep(330, 125)
        Thread.Sleep(250)
        Console.Beep(440, 125)
        Thread.Sleep(125)
        Console.Beep(494, 125)
        Thread.Sleep(125)
        Console.Beep(466, 125)
        Thread.Sleep(42)
        Console.Beep(440, 125)
        Thread.Sleep(125)
        Console.Beep(392, 125)
        Thread.Sleep(125)
        Console.Beep(659, 125)
        Thread.Sleep(125)
        Console.Beep(784, 125)
        Thread.Sleep(125)
        Console.Beep(880, 125)
        Thread.Sleep(125)
        Console.Beep(698, 125)
        Console.Beep(784, 125)
        Thread.Sleep(125)
        Console.Beep(659, 125)
        Thread.Sleep(125)
        Console.Beep(523, 125)
        Thread.Sleep(125)
        Console.Beep(587, 125)
        Console.Beep(494, 125)
        Thread.Sleep(125)
        Console.Beep(523, 125)
        Thread.Sleep(250)
        Console.Beep(392, 125)
        Thread.Sleep(250)
        Console.Beep(330, 125)
        Thread.Sleep(250)
        Console.Beep(440, 125)
        Thread.Sleep(125)
        Console.Beep(494, 125)
        Thread.Sleep(125)
        Console.Beep(466, 125)
        Thread.Sleep(42)
        Console.Beep(440, 125)
        Thread.Sleep(125)
        Console.Beep(392, 125)
        Thread.Sleep(125)
        Console.Beep(659, 125)
        Thread.Sleep(125)
        Console.Beep(784, 125)
        Thread.Sleep(125)
        Console.Beep(880, 125)
        Thread.Sleep(125)
        Console.Beep(698, 125)
        Console.Beep(784, 125)
        Thread.Sleep(125)
        Console.Beep(659, 125)
        Thread.Sleep(125)
        Console.Beep(523, 125)
        Thread.Sleep(125)
        Console.Beep(587, 125)
        Console.Beep(494, 125)
        Thread.Sleep(375)
        Console.Beep(784, 125)
        Console.Beep(740, 125)
        Console.Beep(698, 125)
        Thread.Sleep(42)
        Console.Beep(622, 125)
        Thread.Sleep(125)
        Console.Beep(659, 125)
        Thread.Sleep(167)
        Console.Beep(415, 125)
        Console.Beep(440, 125)
        Console.Beep(523, 125)
        Thread.Sleep(125)
        Console.Beep(440, 125)
        Console.Beep(523, 125)
        Console.Beep(587, 125)
        Thread.Sleep(250)
        Console.Beep(784, 125)
        Console.Beep(740, 125)
        Console.Beep(698, 125)
        Thread.Sleep(42)
        Console.Beep(622, 125)
        Thread.Sleep(125)
        Console.Beep(659, 125)
        Thread.Sleep(167)
        Console.Beep(698, 125)
        Thread.Sleep(125)
        Console.Beep(698, 125)
        Console.Beep(698, 125)
        Thread.Sleep(625)
        Console.Beep(784, 125)
        Console.Beep(740, 125)
        Console.Beep(698, 125)
        Thread.Sleep(42)
        Console.Beep(622, 125)
        Thread.Sleep(125)
        Console.Beep(659, 125)
        Thread.Sleep(167)
        Console.Beep(415, 125)
        Console.Beep(440, 125)
        Console.Beep(523, 125)
        Thread.Sleep(125)
        Console.Beep(440, 125)
        Console.Beep(523, 125)
        Console.Beep(587, 125)
        Thread.Sleep(250)
        Console.Beep(622, 125)
        Thread.Sleep(250)
        Console.Beep(587, 125)
        Thread.Sleep(250)
        Console.Beep(523, 125)
        Thread.Sleep(1125)
        Console.Beep(784, 125)
        Console.Beep(740, 125)
        Console.Beep(698, 125)
        Thread.Sleep(42)
        Console.Beep(622, 125)
        Thread.Sleep(125)
        Console.Beep(659, 125)
        Thread.Sleep(167)
        Console.Beep(415, 125)
        Console.Beep(440, 125)
        Console.Beep(523, 125)
        Thread.Sleep(125)
        Console.Beep(440, 125)
        Console.Beep(523, 125)
        Console.Beep(587, 125)
        Thread.Sleep(250)
        Console.Beep(784, 125)
        Console.Beep(740, 125)
        Console.Beep(698, 125)
        Thread.Sleep(42)
        Console.Beep(622, 125)
        Thread.Sleep(125)
        Console.Beep(659, 125)
        Thread.Sleep(167)
        Console.Beep(698, 125)
        Thread.Sleep(125)
        Console.Beep(698, 125)
        Console.Beep(698, 125)
        Thread.Sleep(625)
        Console.Beep(784, 125)
        Console.Beep(740, 125)
        Console.Beep(698, 125)
        Thread.Sleep(42)
        Console.Beep(622, 125)
        Thread.Sleep(125)
        Console.Beep(659, 125)
        Thread.Sleep(167)
        Console.Beep(415, 125)
        Console.Beep(440, 125)
        Console.Beep(523, 125)
        Thread.Sleep(125)
        Console.Beep(440, 125)
        Console.Beep(523, 125)
        Console.Beep(587, 125)
        Thread.Sleep(250)
        Console.Beep(622, 125)
        Thread.Sleep(250)
        Console.Beep(587, 125)
        Thread.Sleep(250)
        Console.Beep(523, 125)
    End Sub
    Public Shared Function Lerp(ByVal color1 As Color, ByVal color2 As Color, ByVal amount As Single) As Color
        Const bitmask As Single = 65536.0!
        Dim n As UInteger = CUInt(Math.Round(CDbl(Math.Max(Math.Min((amount * bitmask), bitmask), 0.0!))))
        Dim r As Integer = (CInt(color1.R) + (((CInt(color2.R) - CInt(color1.R)) * CInt(n)) >> 16))
        Dim g As Integer = (CInt(color1.G) + (((CInt(color2.G) - CInt(color1.G)) * CInt(n)) >> 16))
        Dim b As Integer = (CInt(color1.B) + (((CInt(color2.B) - CInt(color1.B)) * CInt(n)) >> 16))
        Dim a As Integer = (CInt(color1.A) + (((CInt(color2.A) - CInt(color1.A)) * CInt(n)) >> 16))
        Return Color.FromArgb(a, r, g, b)
    End Function


    Private Sub PicGrid_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles cGridPictureBox.MouseDown
        Dim cellSizeX = CShort(cGridPictureBox.Size.Width / numOfCellsHor)
        Dim cellSizeY = CShort(cGridPictureBox.Size.Height / numOfCellsVer)

        If e.Button <> MouseButtons.Left Then Exit Sub

        Dim xPos As Short = CShort(e.X \ cellSizeX)
        Dim yPos As Short = CShort(e.Y \ cellSizeY)


        If cWallBarCheckItem.Checked Then
            If Map(xPos, yPos).Wall = True Then
                Map(xPos, yPos).Wall = False
            Else
                If Map(xPos, yPos).Coin = True Then Map(xPos, yPos).Coin = False
                Map(xPos, yPos).Wall = True
            End If
            cPlayerPosBarCheckItem.Checked = False
            cGhostPosBarCheckItem.Checked = False
            cCoinBarCheckItem.Checked = False
        ElseIf cPlayerPosBarCheckItem.Checked Then
            cPlayer.PosX = xPos
            cPlayer.PosY = yPos
            cWallBarCheckItem.Checked = False
            cGhostPosBarCheckItem.Checked = False
            cCoinBarCheckItem.Checked = False
        ElseIf cGhostPosBarCheckItem.Checked Then
            cGhost1.PosX = xPos
            cGhost1.PosY = yPos
            cWallBarCheckItem.Checked = False
            cPlayerPosBarCheckItem.Checked = False
            cCoinBarCheckItem.Checked = False
        ElseIf cCoinBarCheckItem.Checked Then
            If Map(xPos, yPos).Coin = True Then
                Map(xPos, yPos).Coin = False
            ElseIf Map(xPos, yPos).Wall = False Then
                Map(xPos, yPos).Coin = True
            End If
            cWallBarCheckItem.Checked = False
            cPlayerPosBarCheckItem.Checked = False
            cGhostPosBarCheckItem.Checked = False
        End If

        Render()
    End Sub

    Public Function OutOfBounds(xPos As Integer, yPos As Integer) As Boolean
        If xPos.IsBetween(0, numOfCellsHor) = True AndAlso yPos.IsBetween(0, numOfCellsVer) = True Then
            Return False
        End If
        Return True
    End Function

    Public Sub Render()
        ' https://stackoverflow.com/questions/36099231/clear-a-bitmap-in-net

        Dim xCnt, yCnt As Int16
        Dim cellSize As Int16 = CShort(cGridPictureBox.Size.Width / numOfCellsHor)

        'Clear the background
        oBuffG.Clear(Color.Black)


        'oBuffG.FillRectangle(New Drawing2D.LinearGradientBrush(Me.ClientRectangle, Color.DarkGray, Color.Black, Drawing2D.LinearGradientMode.BackwardDiagonal), Me.ClientRectangle)

        If cPlayer IsNot Nothing Then oBuffG.DrawImage(cPlayer.Icon, CInt((Me.cPlayer.PosX * cellSize) + (cellSize / 8)), CInt((Me.cPlayer.PosY * cellSize) + (cellSize / 8)), CInt(cellSize - (cellSize / 4)), CInt(cellSize - (cellSize / 4)))
        If cGhost1 IsNot Nothing Then oBuffG.DrawIcon(cGhost1.Icon, cGhost1.PosX * cellSize, cGhost1.PosY * cellSize)
        Dim pointsPath() As Point = {}
        Dim bytes() As Byte = {}
        'Draw Walls
        For yCnt = 0 To numOfCellsVer
            For xCnt = 0 To numOfCellsHor
                Map(xCnt, yCnt).PosX = xCnt
                Map(xCnt, yCnt).PosY = yCnt
                If Map(xCnt, yCnt).Wall = True Then oBuffG.FillRectangle(New SolidBrush(ColorTranslator.FromHtml("#000066")), xCnt * cellSize, yCnt * cellSize, cellSize, cellSize)

                If Map(xCnt, yCnt).Coin = True AndAlso Map(xCnt, yCnt).Wall = False Then
                    oBuffG.FillEllipse(New SolidBrush(Color.Gold), xCnt * cellSize + CShort((cellSize / 2) / 2 - 1), yCnt * cellSize + CShort((cellSize / 2) / 2), CShort(cellSize / 2), CShort(cellSize / 2))
                End If


                If Map(xCnt, yCnt).DrawPath = True Then
                    pointsPath.Add(New Point(xCnt * cellSize + CShort((cellSize / 2) - 1), yCnt * cellSize + CShort(cellSize / 2)))

                    If pointsPath.Length = 1 Then bytes.Add(0) Else bytes.Add(1)

                    oBuffG.FillEllipse(New SolidBrush(Color.White), xCnt * cellSize + CShort((cellSize / 2) / 2 - 1), yCnt * cellSize + CShort((cellSize / 2) / 2), CShort(cellSize / 2), CShort(cellSize / 2))
                End If
            Next
        Next

        ' Draw path 
        If pointsPath.Length > 0 And bytes.Length > 0 Then
            Dim path = New Drawing2D.GraphicsPath(pointsPath, bytes)
            oBuffG.DrawPath(New Pen(Color.Lime), path)
        End If

        ' Draw Walls
        If Walls.Count > 0 Then oBuffG.DrawRectangles(New Pen(ColorTranslator.FromHtml("#2D44F5"), If(AldousBroderMaze Is Nothing, DepthOfRecursion.WallThiccness, AldousBroderMaze.WallThiccness)), Walls.ToArray())

        'Draw grid
        'If cToggleGridBarCheckItem.Checked Then
        '    For xCnt = 0 To CShort(numOfCellsHor + 1)
        '        oBuffG.DrawLine(New Pen(Color.WhiteSmoke, 4), 0, xCnt * cellSize, cGridPictureBox.Size.Height, xCnt * cellSize) ' X
        '        oBuffG.DrawLine(New Pen(Color.WhiteSmoke, 4), xCnt * cellSize, 0, xCnt * cellSize, cGridPictureBox.Size.Width) ' Y
        '    Next
        'End If

        cGridPictureBox.Image = CType(oBuff, Bitmap)
    End Sub


    ''' <summary>
    ''' Generate grid for pacman
    ''' </summary>
    Private Sub GenGrid()
        Me.SplitContainer.Panel1.SuspendLayout()

        'GenerateFields()
        GenPerlinNoise()
        Render()
        Me.SplitContainer.Panel1.ResumeLayout(False)
    End Sub


    Private Sub GenPerlinNoise()
        Dim min As Double = Double.MaxValue
        Dim max As Double = Double.MinValue

        Dim ratio As Double = 3.0F ' 1.5F

        For x = 0 To numOfCellsHor
            For y = 0 To numOfCellsVer

                Dim p As Double = New PerlinNoise().Perlin2D(x * ratio, y * ratio)
                If p < min Then min = p
                If p > max Then max = p

                If p > 0.47F Then
                    Map(x, y).Coin = False
                Else
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
        'GenPerlinNoise()
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

    Private Sub cRunBarLargeButtonItem_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles cRunBarLargeButtonItem.ItemClick
        If Not cGhost1.PathFound Then
            If Not cGhost1.WalkTask.IsCompleted Then cGhost1.FindPath()
        Else
            Dim xCnt, yCnt As Integer

            cGhost1.Heap.ResetHeap()

            For yCnt = 0 To numOfCellsVer
                For xCnt = 0 To numOfCellsHor
                    With Map(xCnt, yCnt)
                        .PosX = xCnt
                        .PosY = yCnt
                        .DrawPath = False
                        .FCost = 0
                        .GCost = 0
                        .HCost = 0
                        .OCList = 0
                        .ParentX = 0
                        .ParentY = 0
                    End With
                Next
            Next
            cGhost1.FindPath()
        End If
    End Sub
    Private Sub cClearBarLargeButtonItem_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles cClearBarLargeButtonItem.ItemClick
        Dim xCnt, yCnt As Integer
        cGhost1.Heap.ResetHeap()

        For yCnt = 0 To numOfCellsVer
            For xCnt = 0 To numOfCellsHor
                With Map(xCnt, yCnt)
                    .PosX = xCnt
                    .PosY = yCnt
                    .DrawPath = False
                    .FCost = 0
                    .GCost = 0
                    .HCost = 0
                    .OCList = 0
                    .ParentX = 0
                    .ParentY = 0
                    '.Wall = True
                    .Coin = False
                End With
            Next
        Next
        Render()
    End Sub
#End Region



    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        If (e.Alt AndAlso (e.KeyCode = Keys.P)) Then

            cBuildMenuRadialMenu.ShowPopup(Me.Location)

            cToastNotificationsManager.ShowNotification("6b650f83-ca7d-4c81-94fd-9053e7626124")
        End If
        If e.KeyCode = Keys.W Or e.KeyCode = Keys.A Or e.KeyCode = Keys.S Or e.KeyCode = Keys.D Then

            If cPlayer Is Nothing Then Exit Sub
            If Not e.KeyCode = cPlayer.CurrentDirection Then
                'cPlayer.CurrentDirection = e.KeyCode
                'Threading.Tasks.Task.Factory.StartNew(Sub() cPlayer.MoveInDirection(Me))

            End If
            cPlayer.CurrentDirection = e.KeyCode
            cPlayer.MoveWASD(cPlayer.CurrentDirection)

        End If

    End Sub

    Private Sub cShowCollisionBoxBarCheckItem_CheckedChanged(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles cShowCollisionBoxBarCheckItem.CheckedChanged
        ShowCollision = cShowCollisionBoxBarCheckItem.Checked
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

    Private Sub cGameTimer_Tick(sender As Object, e As EventArgs) Handles cGameTimer.Tick
        cTimeDigitalGauge.Text = CStr(Environment.TickCount)
    End Sub

    Private Sub cDrawTabNavigationPage_Paint(sender As Object, e As PaintEventArgs) Handles cDrawTabNavigationPage.Paint

    End Sub


    'Private Sub Move(sender As Object, e As EventArgs)
    '    If TypeOf sender Is Ghost Then
    '        Dim result As DialogResult = MessageBox.Show("You died!\nYou wanna restart?", "Game Over 🤡", MessageBoxButtons.YesNo)
    '        If result = DialogResult.Yes Then
    '            Me.Reset()
    '        ElseIf result = DialogResult.No Then

    '        End If
    '        Return
    '    Else
    '        Debug.WriteLine($"- {sender.GetType()}")
    '        Dim btn As Cell = CType(sender, Cell)
    '        Debug.WriteLine($"{btn.Name} - {sender.GetType()}")
    '        cPlayer.Move(btn, CellList)
    '        cGameCountLabel.Text = $"{If(cPlayer.Gold > 1, "Coins", "Coin")}: {cPlayer.Gold}"
    '    End If

    'End Sub


End Class

