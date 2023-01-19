Public Class Player
    Private rand As New Random
    Public Gold As Integer
    Public LastLocation As Point
    Public CurrentDirection As Keys = Keys.W
    Public PosX As Integer = 4
    Public PosY As Integer = 7
    Public Icon As Bitmap = New Bitmap(CType(Image.FromFile("C:\Users\arvid.wedtstein\Pictures\Memes\dennys2.png", True), Bitmap), New Size(CInt(TestForm.cellSize - (TestForm.cellSize / 4)), CInt(TestForm.cellSize - (TestForm.cellSize / 4))))
    Public Icon2 As Icon = New Icon("C:\Users\arvid.wedtstein\Pictures\Memes\dennys2.ico")


    Public Function IntersectsWithWall(oldPosX As Integer, oldPosY As Integer, posX As Integer, posY As Integer) As Boolean
        Dim CellSize = CShort(TestForm.cGridPictureBox.Size.Width / TestForm.numOfCellsHor)
        Dim posOnMapX = posX * CellSize
        Dim posOnMapY = posY * CellSize
        Dim oldPosOnMapX = oldPosX * CellSize
        Dim oldPosOnMapY = oldPosY * CellSize


        Dim diffX = posOnMapX.CompareTo(oldPosOnMapX)
        Dim diffY = posOnMapY.CompareTo(oldPosOnMapY)

        Dim location = New Point(CInt(Math.Min(oldPosOnMapX, posOnMapX)),
                                 CInt(Math.Min(oldPosOnMapY, posOnMapY)))
        Dim size = New Size(
            CInt(If(diffX = 0, CellSize - (CellSize / 4), Math.Abs(CellSize * (diffX * 2)) - (Math.Abs(CellSize * (diffX * 2)) / 4))),
            CInt(If(diffY = 0, CellSize - (CellSize / 4), Math.Abs(CellSize * (diffY * 2)) - (Math.Abs(CellSize * (diffY * 2)) / 4))))

        Dim playerPath = New Rectangle(location, size)
        playerPath.X = CInt(playerPath.X + (playerPath.Width / 8))
        playerPath.Y = CInt(playerPath.Y + (playerPath.Height / 8))

        If TestForm.ShowCollision = True Then
            TestForm.oBuffG.DrawRectangle(New Pen(Color.Red, 5), playerPath)
            TestForm.cGridPictureBox.Image = CType(TestForm.oBuff, Bitmap)
            TestForm.cGridPictureBox.Refresh()
        End If


        For Each wall In TestForm.Walls
            'If playerPath.Contains(CInt(wall.Left), CInt(Math.Abs(wall.Bottom - wall.Top) / 2)) = True Then Return True
            If playerPath.IntersectsWith(wall) = True Then Return True
        Next

        Return False
    End Function

    Public Sub New()
        'Threading.Tasks.Task.Factory.StartNew(Sub() Me.MoveInDirection(TestForm))
        TestForm.cPosXDigitalGauge.Text = String.Format("{00:00#}", Me.PosX)
        TestForm.cPosYDigitalGauge.Text = String.Format("{00:00#}", Me.PosY)
    End Sub

    Public Sub MoveWASD(keyCode As Keys)
        Dim oldPosX = PosX
        Dim oldPosY = PosY

        Select Case keyCode
            Case Keys.W
                PosX = PosX
                PosY = PosY - 1
            Case Keys.A
                PosX = PosX - 1
                PosY = PosY
            Case Keys.S
                PosX = PosX
                PosY = PosY + 1
            Case Keys.D
                PosX = PosX + 1
                PosY = PosY
        End Select


        If TestForm.OutOfBounds(PosX, PosY) = True Then
            PosX = oldPosX
            PosY = oldPosY
            Exit Sub
        End If

        If IntersectsWithWall(oldPosX, oldPosY, PosX, PosY) = True Then
            PosX = oldPosX
            PosY = oldPosY
            Exit Sub
        End If


        If TestForm.Map(PosX, PosY).Wall = True Then
            PosX = oldPosX
            PosY = oldPosY
            Exit Sub
        End If

        TestForm.cPosXDigitalGauge.Text = String.Format("{00:00#}", Me.PosX)
        TestForm.cPosYDigitalGauge.Text = String.Format("{00:00#}", Me.PosY)

        TestForm.Map(oldPosX, oldPosY).Player = Nothing
        If PosX <= 0 Then
            PosX = 0
        End If
        If PosX >= TestForm.numOfCellsHor Then
            PosX = TestForm.numOfCellsHor
        End If
        If PosY <= 0 Then
            PosY = 0
        End If
        If PosY >= TestForm.numOfCellsVer Then
            PosY = TestForm.numOfCellsVer
        End If

        Me.CollectGold(TestForm.Map(PosX, PosY))


        TestForm.Map(PosX, PosY).Player = Me
        TestForm.Render()

    End Sub

    Private Sub CollectGold(cell As CellData)
        If cell.Coin = True Then
            cell.Coin = False
            Me.Gold += 1
            TestForm.Map(PosX, PosY).Coin = False
            TestForm.cGoldDigitalGauge.Text = String.Format("{000:0000#}", Me.Gold)
            'TestForm.cGoldSkinBarSubItem.Caption = $"Gold: {Me.Gold}"
            'If Not cell.BackgroundWorker.IsBusy Then
            '    cell.BackgroundWorker.RunWorkerAsync()
            'End If
        End If
    End Sub
    Public Sub MoveInDirection(instance As TestForm)
        Dim oldDirection = Me.CurrentDirection
        While True
            Dim oldPosX = PosX
            Dim oldPosY = PosY
            instance.Map(oldPosX, oldPosY).Player = Nothing
            If Not oldDirection = Me.CurrentDirection Then
                Debug.WriteLine("Direction changed")
                Exit While
                Exit Sub
            End If
            Select Case CurrentDirection
                Case Keys.W
                    PosX = PosX
                    PosY = PosY - 1
                Case Keys.A
                    PosX = PosX - 1
                    PosY = PosY
                Case Keys.S
                    PosX = PosX
                    PosY = PosY + 1
                Case Keys.D
                    PosX = PosX + 1
                    PosY = PosY
            End Select

            If instance.Map(PosX, PosY).Wall = True Then
                PosX = oldPosX
                PosY = oldPosY

                Debug.WriteLine("End bc wall")
                Exit While
                Exit Sub
            End If




            If PosX <= 0 Then
                PosX = 0
                instance.playerPosX = PosX
                instance.playerPosY = PosY
                Exit While
                Exit Sub
            End If
            If PosX >= instance.numOfCellsHor Then
                PosX = instance.numOfCellsHor
                instance.playerPosX = PosX
                instance.playerPosY = PosY
                Exit While
                Exit Sub
            End If
            If PosY <= 0 Then
                PosY = 0
                instance.playerPosX = PosX
                instance.playerPosY = PosY
                Exit While
                Exit Sub
            End If
            If PosY >= instance.numOfCellsVer Then
                PosY = instance.numOfCellsVer
                instance.playerPosX = PosX
                instance.playerPosY = PosY
                Exit While
                Exit Sub
            End If
            Debug.WriteLine($"X: {PosX} Y: {PosY}")

            Me.CollectGold(instance.Map(PosX, PosY))
            instance.Map(PosX, PosY).Player = Me

            'Render(instance, oldPosX, oldPosY)
            'instance.Render()

            System.Threading.Thread.Sleep(1000)
        End While
    End Sub

    Public Sub Render(instance As TestForm, oldPosX As Integer, oldPosY As Integer)

        Dim xCnt, yCnt As Int16

        Dim cellSize As Int16 = CShort(instance.cGridPictureBox.Size.Width / instance.numOfCellsHor)

        'Clear the background
        instance.oBuffG.Clear(Color.Black)

        'instance.oBuffG.DrawIcon(Me.Icon, Me.PosX * cellSize, Me.PosY * cellSize)

        ' Ghost Pos can't be rendered from player. Solution = Layers?
        'instance.oBuffG.DrawIcon(instance.cGhost1.Icon, instance.cGhost1.PosX * cellSize, instance.cGhost1.PosY * cellSize)

        'Draw Walls
        'For yCnt = 0 To instance.numOfCellsVer
        '    For xCnt = 0 To instance.numOfCellsHor
        '        If instance.Map(xCnt, yCnt).Wall = True Then instance.oBuffG.FillRectangle(New SolidBrush(ColorTranslator.FromHtml("#000066")), xCnt * cellSize, yCnt * cellSize, cellSize, cellSize)

        '        If instance.Map(xCnt, yCnt).DrawPath = True Then
        '            instance.oBuffG.FillEllipse(New SolidBrush(Color.White), xCnt * cellSize + CShort((cellSize / 2) / 2 - 1), yCnt * cellSize + CShort((cellSize / 2) / 2), CShort(cellSize / 2), CShort(cellSize / 2))
        '        End If
        '        If instance.Map(xCnt, yCnt).Coin = True AndAlso instance.Map(xCnt, yCnt).Wall = False Then
        '            instance.oBuffG.FillEllipse(New SolidBrush(Color.Gold), xCnt * cellSize + CShort((cellSize / 2) / 2 - 1), yCnt * cellSize + CShort((cellSize / 2) / 2), CShort(cellSize / 2), CShort(cellSize / 2))
        '        End If
        '    Next
        'Next

        instance.cGridPictureBox.Image = CType(instance.oBuff, Bitmap)
        'instance.cGoldSkinBarSubItem.Caption = $"{If(Me.Gold > 1, "Coins", "Coin")}: {Me.Gold}"
    End Sub
End Class
