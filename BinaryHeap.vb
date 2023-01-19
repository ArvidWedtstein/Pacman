
Public Structure CellData
    Public PosX As Int32
    Public PosY As Int32
    Public OCList As Int16
    Public GCost As Int16
    Public HCost As Int16
    Public FCost As Int16
    Public ParentX, ParentY As Int16
    Public Wall As Boolean
    Public DrawPath As Boolean
    Public Coin As Boolean
    Public Player As Player
End Structure

Public Structure BinHeapData
    Friend Score As Int16
    Friend X As Int16
    Friend Y As Int16
End Structure

Public Class BinaryHeap
    Private lSize As Int16
    Private Heap() As BinHeapData

    Public ReadOnly Property Count() As Int16
        Get
            Return lSize
        End Get
    End Property
    Public ReadOnly Property GetX() As Int16
        Get
            Return Heap(1).X
        End Get
    End Property
    Public ReadOnly Property GetY() As Int16
        Get
            Return Heap(1).Y
        End Get
    End Property
    Public ReadOnly Property GetScore() As Int16
        Get
            Return Heap(1).Score
        End Get
    End Property

    Public Sub ResetHeap()
        lSize = 0
        ReDim Heap(0)
    End Sub
    Public Sub RemoveRoot()
        If lSize <= 1 Then
            ResetHeap()
            Exit Sub
        End If

        ' First copy the very bottom object to the top
        Heap(1) = Heap(lSize)

        Dim one As Short = 1
        lSize -= one

        'Shrink the array
        ReDim Preserve Heap(lSize)

        Dim Parent As Int16 = 1
        Dim ChildIndex As Int16 = 1

        While True
            ChildIndex = Parent
            If 2 * ChildIndex + 1 <= lSize Then
                'Find the lowest value of the 2 child nodes
                If Heap(ChildIndex).Score >= Heap(2 * ChildIndex).Score Then Parent = CShort(2 * ChildIndex)
                If Heap(Parent).Score >= Heap(2 * ChildIndex + 1).Score Then Parent = CShort(2 * ChildIndex + 1)
            Else 'Just process the one node
                If 2 * ChildIndex <= lSize Then
                    If Heap(ChildIndex).Score >= Heap(2 * ChildIndex).Score Then Parent = CShort(2 * ChildIndex)
                End If
            End If

            'Swap out the child/parent
            If Parent <> ChildIndex Then
                Dim tHeap As BinHeapData = Heap(ChildIndex)
                Heap(ChildIndex) = Heap(Parent)
                Heap(Parent) = tHeap
            Else
                Exit While
            End If
        End While
    End Sub
    Public Sub Add(ByVal inScore As Int16, ByVal inX As Int16, ByVal inY As Int16)
        '**ignore the (0) place in the heap array because
        '**it's easier to handle the heap with a base of (1..?)

        'Increment the array count
        Dim one As Short = 1
        lSize += one

        'Make room in the array
        ReDim Preserve Heap(lSize)

        'Store the data
        With Heap(lSize)
            .Score = inScore
            .X = inX
            .Y = inY
        End With

        'Bubble the item to its correct location
        Dim sPos As Int16 = lSize

        While sPos <> 1
            If Heap(sPos).Score <= Heap(CShort(sPos / 2)).Score Then
                Dim tHeap As BinHeapData = Heap(CShort(sPos / 2))
                Heap(CShort(sPos / 2)) = Heap(sPos)
                Heap(sPos) = tHeap
                sPos = CShort(sPos / 2)
            Else
                Exit While
            End If
        End While
    End Sub
End Class