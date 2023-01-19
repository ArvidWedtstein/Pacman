Imports System.Runtime.CompilerServices
Module ListExtensions

    ''' <summary>
    ''' Removes and returns the last item in an array
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="array"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function Pop(Of T)(ByRef array As List(Of T)) As T
        If array.Count > 0 Then
            Dim returnItem = array(array.Count - 1)
            array.RemoveAt(array.Count - 1)
            Return returnItem
        End If
        Return Nothing
    End Function


    <Extension()>
    Public Sub Add(Of T)(ByRef arr As T(), item As T)
        Array.Resize(arr, arr.Length + 1)
        arr(arr.Length - 1) = item
    End Sub

End Module
