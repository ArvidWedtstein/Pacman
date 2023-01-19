Imports System.Runtime.CompilerServices
Module Int32Extensions
    <Extension()>
    Public Function Decr(ByRef i As Int32) As Int32
        i -= 1
        Return i
    End Function

    <Extension()>
    Public Function Incr(ByRef i As Int32) As Int32
        i += 1
        Return i
    End Function

    <Extension()>
    Public Function IsBetween(ByRef i As Int32, ByVal min As Int32, ByVal max As Int32) As Boolean
        If i >= min And i <= max Then
            Return True
        End If
        Return False
    End Function

    <Extension()>
    Function ToBase36(ByVal i As Integer) As String
        Const chars As String = "0123456789abcdefghijklmnopqrstuvwxyz"
        Return chars(i Mod 36).ToString()
    End Function

    <Extension()>
    Function ToBase64(ByVal i As Integer) As String
        Const chars As String = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ%="
        Return chars(i Mod 64).ToString()
    End Function
End Module
