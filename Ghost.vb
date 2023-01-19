Imports System.Linq
Imports System.Threading.Tasks
Public Class Ghost
    Public ID As String
    Public WalkTask As Task = New Task(Sub() MoveTowardsPlayer(TestForm))

    Public PosX, PosY As Integer
    Public ParentX, ParentY As Short
    Public IsChasing As Boolean = False
    Public PathFound As Boolean = False
    Private Const inOpened As Integer = 1
    Private Const inClosed As Integer = 2

    Public Heap As New BinaryHeap()

    Private GhostColors As List(Of Color) = New List(Of Color)({Color.Pink, Color.Turquoise, Color.Orange})
    Public BackgroundWorker As ComponentModel.BackgroundWorker = New ComponentModel.BackgroundWorker
    Public Icon As Icon = New Icon("C:\Users\arvid.wedtstein\Pictures\Memes\dennys2.ico")
    Public Sub New()
        ID = Guid.NewGuid().ToString("N")

        'Threading.Tasks.Task.Factory.StartNew(Sub() Me.MoveTowardsPlayer(TestForm))
        BackgroundWorker.WorkerReportsProgress = True
        BackgroundWorker.WorkerSupportsCancellation = True
        AddHandler BackgroundWorker.DoWork, AddressOf bw_DoWork
        AddHandler BackgroundWorker.RunWorkerCompleted, AddressOf bw_RunWorkerCompleted
    End Sub

    Private Sub bw_DoWork(ByVal sender As Object, ByVal e As ComponentModel.DoWorkEventArgs)
        Dim worker As ComponentModel.BackgroundWorker = CType(sender, ComponentModel.BackgroundWorker)

        Threading.Thread.Sleep(500)
        'Me.MoveTowardsPlayer()
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


    Public Sub FindPath()
        Debug.WriteLine("findpath")
        Dim xCnt, yCnt As Integer

        If (TestForm.cPlayer.PosX = Me.PosX) And (TestForm.cPlayer.PosY = Me.PosY) Then Exit Sub

        If TestForm.Map(TestForm.cPlayer.PosX, TestForm.cPlayer.PosY).Wall = True Then Exit Sub
        If TestForm.Map(Me.PosX, Me.PosY).Wall = True Then Exit Sub

        PathFound = False
        IsChasing = True

        TestForm.Map(Me.PosX, Me.PosY).OCList = inOpened
        Heap.Add(0, CShort(Me.PosX), CShort(Me.PosY))
        'TestForm.Map(TestForm.cPlayer.PosX, TestForm.cPlayer.PosY).OCList = inOpened
        'Heap.Add(0, CShort(TestForm.cPlayer.PosX), CShort(TestForm.cPlayer.PosY))

        While IsChasing
            If Heap.Count <> 0 Then
                ParentX = Heap.GetX
                ParentY = Heap.GetY

                TestForm.Map(ParentX, ParentY).OCList = inClosed
                Heap.RemoveRoot()

                For yCnt = (ParentY - 1) To (ParentY + 1)
                    For xCnt = (ParentX - 1) To (ParentX + 1)

                        'Make sure we are not out of bounds
                        If xCnt <> -1 And xCnt <> TestForm.numOfCellsHor + 1 And yCnt <> -1 And yCnt < TestForm.numOfCellsVer Then

                            'Make sure its not on the closed list
                            If TestForm.Map(xCnt, yCnt).OCList <> inClosed Then

                                'Make sure no wall
                                'Fack. make sure NO wall
                                Dim CellSize = CShort(TestForm.cGridPictureBox.Size.Width / TestForm.numOfCellsHor)
                                Dim diffX = xCnt.CompareTo(ParentX)
                                Dim diffY = yCnt.CompareTo(ParentY)

                                Dim location = New Point(CInt(Math.Min(ParentX * CellSize, xCnt * CellSize)),
                                 CInt(Math.Min(ParentY * CellSize, yCnt * CellSize)))
                                Dim size = New Size(
                                        CInt(If(diffX = 0, CellSize - (CellSize / 4), Math.Abs(CellSize * (diffX * 2)) - (Math.Abs(CellSize * (diffX * 2)) / 4))),
                                        CInt(If(diffY = 0, CellSize - (CellSize / 4), Math.Abs(CellSize * (diffY * 2)) - (Math.Abs(CellSize * (diffY * 2)) / 4))))
                                Dim size2 = New Size(
                                        CInt(If(diffX = 0, CellSize - (CellSize / 4), Math.Abs(CellSize * (diffX * 1)) - (Math.Abs(CellSize * (diffX * 1)) / 4))),
                                        CInt(If(diffY = 0, CellSize - (CellSize / 4), Math.Abs(CellSize * (diffY * 1)) - (Math.Abs(CellSize * (diffY * 1)) / 4))))

                                Dim playerPath = New Rectangle(location, size)
                                Dim playerPath2 = New Rectangle(location, size2)
                                playerPath.X = CInt(playerPath.X + (playerPath.Width / 8))
                                playerPath.Y = CInt(playerPath.Y + (playerPath.Height / 8))


                                Dim intersects = (From wall In TestForm.Walls
                                                  Where wall.IntersectsWith(playerPath)).ToList()


                                Threading.Thread.Sleep(10)

                                If TestForm.Map(xCnt, yCnt).Wall = False AndAlso intersects.Count < 1 Then

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
                                        If TestForm.Map(xCnt, yCnt).OCList <> inOpened Then

                                            TestForm.oBuffG.FillEllipse(New SolidBrush(TestForm.Lerp(Color.Red, Color.Green, If(((xCnt + yCnt / 2) > 0I), (Math.Min(Math.Max(CSng((xCnt + yCnt) / 2), 0.0!), 10.0!) / 10.0!), 0.0!))), playerPath2)
                                            TestForm.cGridPictureBox.Image = CType(TestForm.oBuff, Bitmap)
                                            TestForm.cGridPictureBox.Refresh()

                                            'Calculate GCost
                                            If Math.Abs(xCnt - ParentX) = 1 And Math.Abs(yCnt - ParentY) = 1 Then
                                                TestForm.Map(xCnt, yCnt).GCost = TestForm.Map(ParentX, ParentY).GCost + CShort(14)
                                            Else
                                                TestForm.Map(xCnt, yCnt).GCost = TestForm.Map(ParentX, ParentY).GCost + CShort(10)
                                            End If

                                            'Calculate HCost
                                            'TestForm.Map(xCnt, yCnt).HCost = CShort(10 * (Math.Abs(xCnt - Me.PosX) + Math.Abs(yCnt - Me.PosY)))
                                            TestForm.Map(xCnt, yCnt).HCost = CShort(10 * (Math.Abs(xCnt - TestForm.cPlayer.PosX) + Math.Abs(yCnt - TestForm.cPlayer.PosY)))
                                            TestForm.Map(xCnt, yCnt).FCost = CShort(TestForm.Map(xCnt, yCnt).GCost + TestForm.Map(xCnt, yCnt).HCost)

                                            'Add the parent value
                                            TestForm.Map(xCnt, yCnt).ParentX = ParentX
                                            TestForm.Map(xCnt, yCnt).ParentY = ParentY

                                            'Add the item to the heap
                                            Heap.Add(TestForm.Map(xCnt, yCnt).FCost, CShort(xCnt), CShort(yCnt))



                                            'Add the item to the open list
                                            TestForm.Map(xCnt, yCnt).OCList = inOpened

                                        Else
                                            Dim AddedGCost As Integer
                                            If Math.Abs(xCnt - ParentX) = 1 And Math.Abs(yCnt - ParentY) = 1 Then
                                                AddedGCost = 14
                                            Else
                                                AddedGCost = 10
                                            End If

                                            Dim tempCost As Int16 = CShort(TestForm.Map(ParentX, ParentY).GCost + AddedGCost)

                                            If tempCost < TestForm.Map(xCnt, yCnt).GCost Then
                                                TestForm.Map(xCnt, yCnt).GCost = tempCost
                                                TestForm.Map(xCnt, yCnt).ParentX = ParentX
                                                TestForm.Map(xCnt, yCnt).ParentY = ParentY
                                                If TestForm.Map(xCnt, yCnt).OCList = inOpened Then
                                                    Dim NewCost As Integer = TestForm.Map(xCnt, yCnt).HCost + TestForm.Map(xCnt, yCnt).GCost
                                                    Heap.Add(CShort(NewCost), CShort(xCnt), CShort(yCnt))
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                                TestForm.cHCostCircularGauge.Scales.All(Function(scale)
                                                                            scale.EnableAnimation = True
                                                                            scale.EasingMode = DevExpress.XtraGauges.Core.Model.EasingMode.EaseInOut
                                                                            scale.EasingFunction = New DevExpress.XtraGauges.Core.Model.ElasticEase()
                                                                            scale.Value = TestForm.Map(xCnt, yCnt).HCost
                                                                        End Function)
                                TestForm.cFCostLinearGauge.Scales.All(Function(scale)
                                                                          scale.EnableAnimation = True
                                                                          scale.EasingMode = DevExpress.XtraGauges.Core.Model.EasingMode.EaseInOut
                                                                          scale.EasingFunction = New DevExpress.XtraGauges.Core.Model.ElasticEase()
                                                                          scale.Value = TestForm.Map(xCnt, yCnt).FCost
                                                                      End Function)
                            End If
                        End If
                    Next
                Next
            Else
                PathFound = False
                IsChasing = False
                Exit Sub
            End If

            'If TestForm.Map(Me.PosX, Me.PosY).OCList = inOpened Then
            If TestForm.Map(TestForm.cPlayer.PosX, TestForm.cPlayer.PosY).OCList = inOpened Then
                Debug.WriteLine("Path FOUND")
                PathFound = True
                IsChasing = False
            End If
        End While

        If PathFound Then
            'Dim tX As Integer = Me.PosX
            'Dim tY As Integer = Me.PosY
            Dim tX As Integer = TestForm.cPlayer.PosX
            Dim tY As Integer = TestForm.cPlayer.PosY
            TestForm.Map(tX, tY).DrawPath = True

            If WalkTask.IsCompleted Or WalkTask.IsCanceled Then Me.BackgroundWorker.RunWorkerAsync()
            'If WalkTask.IsCompleted Or WalkTask.IsCanceled Then WalkTask.Start()
            While True
                Dim sX As Integer = TestForm.Map(tX, tY).ParentX
                Dim sY As Integer = TestForm.Map(tX, tY).ParentY

                TestForm.Map(sX, sY).DrawPath = True
                tX = sX
                tY = sY

                If tX = Me.PosX And tY = Me.PosY Then Exit While
                'If tX = TestForm.cPlayer.PosX And tY = TestForm.cPlayer.PosY Then Exit While
                WalkTask.Delay(500)

            End While

            TestForm.Render()
        End If
    End Sub
    Public Sub MoveTowardsPlayer(instance As TestForm)
        Debug.WriteLine("Move to player")

        Try
            Dim targetX As Integer = Me.PosX
            Dim targetY As Integer = Me.PosY
            Dim sX As Integer = instance.Map(targetX, targetY).ParentX
            Dim sY As Integer = instance.Map(targetX, targetY).ParentY

            instance.Map(sX, sY).DrawPath = True
            targetX = sX
            targetY = sY

            Me.PosX = targetX
            Me.PosY = targetY


            Me.PosX = targetX
            Me.PosY = targetY

            If targetX = instance.playerPosX And targetY = instance.playerPosY Then Exit Sub
            TestForm.Render()
        Catch ex As Exception
            Debug.WriteLine($"Error: {ex.Message}")
        End Try


    End Sub
End Class