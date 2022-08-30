Public Class MazeBuilder
    Public width As Integer
    Public height As Integer
    Public cols As Integer
    Public rows As Integer

    Public maze As List(Of List(Of String))

    Public totalSteps As Integer

    Public Sub New(width As Integer, height As Integer)
        Me.width = width
        Me.height = height

        Me.cols = 2 * Me.width + 1
        Me.rows = 2 * Me.height + 1

        Me.maze = Me.initArray(".")

        Dim tempMaze = New List(Of List(Of String))(Me.maze)
        Debug.WriteLine(tempMaze)
        For Each row In tempMaze
            Dim r = tempMaze.IndexOf(row)
            For Each col In row
                Dim c = row.IndexOf(col)

                Select Case r
                    Case 0

                    Case Me.rows - 1
                        'Me.maze(r)(c) = "Wall"
                    Case Else
                        If (r Mod 2) = 1 Then
                            If c = 0 Or c = Me.cols - 1 Then
                                'Me.maze(r)(c) = "Wall"
                            End If
                        ElseIf (c Mod 2) = 0 Then
                            'Me.maze(r)(c) = "Wall"
                        End If
                End Select
            Next
            'If r = 0 Then
            '    Dim doorPos = Me.posToSpace(Me.rand(1, Me.width))
            '    Me.maze(r)(doorPos) = "door"
            'End If

            'If r = Me.rows - 1 Then
            '    Dim doorPos = Me.posToSpace(Me.rand(1, Me.width))
            '    Me.maze(r)(doorPos) = "door"
            'End If
        Next

        'Me.partition(1, Me.height - 1, 1, Me.width - 1)
    End Sub

    Public Function initArray(value As String) As List(Of List(Of String))
        Dim l = New List(Of List(Of String))
        For r = 0 To Me.rows
            Dim ll = New List(Of String)
            For c = 0 To Me.cols
                ll.Add(value)
            Next
            l.Add(ll)
        Next

        Return l
    End Function
    Public Function rand(min As Integer, max As Integer) As Integer
        Return New Random().Next(min, max)
    End Function
    Public Function posToSpace(x As Integer) As Integer
        Return 2 * (x - 1) + 1
    End Function
    Public Function posToWall(x As Integer) As Integer
        Return 2 * x
    End Function
    Public Function inBounds(row As Integer, col As Integer) As Boolean
        If Me.maze(row) Is Nothing Or Me.maze(row)(col) Is Nothing Then
            Return False
        End If
        Return True
    End Function
    Public Function shuffle(array As List(Of Boolean)) As List(Of Boolean)
        For i = array.Count - 1 To 0 Step -1
            Dim j = Math.Floor(New Random().Next() * (i + 1))
            array(i) = array(CInt(j))
            array(CInt(j)) = array(i)
        Next
        Return array
    End Function

    Public Function partition(r1 As Integer, r2 As Integer, c1 As Integer, c2 As Integer) As Boolean
        Dim horiz As Integer,
            vert As Integer,
            x As Integer,
            y As Integer,
            startPos As Integer,
            endPos As Integer

        If r2 < r1 Or c2 < c1 Then
            Return False
        End If

        If r1 = r2 Then
            horiz = r1
        Else
            x = r1 + 1
            y = r2 - 1

            startPos = CInt(Math.Round(x + (y - x) / 4))
            endPos = CInt(Math.Round(x + 3 * (y - x) / 4))
            horiz = Me.rand(startPos, endPos)
        End If

        If c1 = c2 Then
            vert = c1
        Else
            x = c1 + 1
            y = c2 - 1

            startPos = CInt(Math.Round(x + (y - x) / 3))
            endPos = CInt(Math.Round(x + 2 * (y - x) / 3))
            vert = Me.rand(startPos, endPos)
        End If

        For i = Me.posToWall(r1) - 1 To Me.posToWall(r2) + 1 Step 1
            For j = Me.posToWall(c1) - 1 To Me.posToWall(c2) + 1 Step 1
                If i = Me.posToWall(horiz) Or j = Me.posToWall(vert) Then
                    Me.maze(i)(j) = "Wall"
                End If
            Next
        Next

        Dim gaps = Me.shuffle(New List(Of Boolean)({True, True, True, False}))

        If gaps(0) Then
            Dim gapPosition = Me.rand(c1, vert)
            Me.maze(Me.posToWall(horiz))(Me.posToSpace(gapPosition)) = ""
        End If

        If gaps(1) Then
            Dim gapPosition = Me.rand(vert + 1, c2 + 1)
            Me.maze(Me.posToWall(horiz))(Me.posToSpace(gapPosition)) = ""
        End If

        If gaps(2) Then
            Dim gapPosition = Me.rand(r1, horiz)
            Me.maze(Me.posToSpace(gapPosition))(Me.posToWall(vert)) = ""
        End If

        If gaps(3) Then
            Dim gapPosition = Me.rand(horiz + 1, r2 + 1)
            Me.maze(Me.posToSpace(gapPosition))(Me.posToWall(vert)) = ""
        End If

        ' recursively partition newly created chambers
        Me.partition(r1, horiz - 1, c1, vert - 1)
        Me.partition(horiz + 1, r2, c1, vert - 1)
        Me.partition(r1, horiz - 1, vert + 1, c2)
        Me.partition(horiz + 1, r2, vert + 1, c2)

    End Function

    Public Function isGap(cells As List(Of String)) As Boolean
        Return cells.TrueForAll(Function(cell)
                                    Dim row As Integer,
                                col As Integer
                                    row = cells.IndexOf(cell)
                                    col = cells.IndexOf(cell)

                                    If Me.maze(row)(col) IsNot Nothing Then
                                        If Not Me.maze(row)(col).Contains("door") Then
                                            Return False
                                        End If
                                    End If
                                    Return True
                                End Function)
        'For Each cell In cells
        '    Dim row As Integer,
        '        col As Integer
        '    row = cells.IndexOf(cell)
        '    col = cells.IndexOf(cell)

        '    If Me.maze(row)(col) IsNot Nothing Then
        '        If Not Me.maze(row)(col).Contains("door") Then
        '            Return False
        '        End If
        '    End If
        '    Return True
        'Next
    End Function

    Public Function countSteps(array As List(Of List(Of String)), row As Integer, col As Integer, val As Integer, stopYes As String) As Boolean
        If Not Me.inBounds(row, col) Then
            Return False
        End If

        If CInt(array(row)(col)) <= val Then
            Return False
        End If

        If Not Me.isGap(New List(Of String)({CStr(row), CStr(col)})) Then
            Return False
        End If

        array(row)(col) = CStr(val)

        If Me.maze(row)(col).Contains(stopYes) Then
            Return True
        End If

        Me.countSteps(array, row - 1, col, val + 1, stopYes)
        Me.countSteps(array, row, col + 1, val + 1, stopYes)
        Me.countSteps(array, row + 1, col, val + 1, stopYes)
        Me.countSteps(array, row, col - 1, val + 1, stopYes)
    End Function

    Public Function getKeyLocation() As Tuple(Of Integer, Integer)
        Dim fromEntrance = Me.initArray("")
        Dim fromExit = Me.initArray("")

        Me.totalSteps = -1

        For j = 1 To Me.cols - 1
            If Me.maze(Me.rows - 1)(j).Contains("entrance") Then
                Me.countSteps(fromEntrance, Me.rows - 1, j, 0, "exit")
            End If
            If Me.maze(0)(j).Contains("exit") Then
                Me.countSteps(fromExit, 0, j, 0, "entrance")
            End If
        Next

        Dim fc = -1, fr = -1

        For Each row In Me.maze
            For Each cell In row
                Dim r = Me.maze.IndexOf(row)
                Dim c = row.IndexOf(cell)

                If fromEntrance(r)(c) Is Nothing Then
                    Return Nothing
                End If

                Dim stepCount = CInt(fromEntrance(r)(c)) + CInt(fromExit(r)(c))

                If stepCount > Me.totalSteps Then
                    fr = r
                    fc = c
                    Me.totalSteps = stepCount
                End If
            Next
        Next

        Return Tuple.Create(fr, fc)
    End Function

    Public Sub placeKey()
        Dim fr, fc As Integer
        With Me.getKeyLocation()
            fr = .Item1
            fc = .Item2
        End With

        Me.maze(fr)(fc) = "key"
    End Sub

    Public Function display(id As Integer) As Boolean
        Dim str = New List(Of List(Of String))
        For Each row In Me.maze
            str.AddRange({row})
        Next
        Debug.WriteLine(str)
        Return False
    End Function

End Class
