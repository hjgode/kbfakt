Option Explicit On
Imports System.Runtime.InteropServices

Public Class EscapePrintHelper

    <DllImport("gdi32.dll", EntryPoint:="Escape", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Private Shared Function Escape(ByVal Hdc As IntPtr, ByVal nEscape As Integer, ByVal ncount As Integer, ByVal inData As IntPtr, ByVal outData As IntPtr) As Integer
    End Function

    Private Const PASSTHROUGH As Integer = 19
    Private Handle As IntPtr
    Private graphics As Graphics

    Public Sub New(ByVal Graphics As Graphics)
        Graphics = Graphics
        Handle = Graphics.GetHdc()
        Graphics.ReleaseHdc(Handle)
    End Sub

    Public Function SendPassThroughExt(ByVal PassthroughData As String) As Boolean
        Return sendPassThrough(PassthroughData, Handle)
    End Function

    Private Function sendPassThrough(ByVal Passthroughdata As String, ByVal handle As IntPtr) As Boolean
        Dim grp As IntPtr = handle
        Dim RetVal As Boolean = False
        Dim pData As IntPtr = string2Global(Passthroughdata, True)
        Try
            Dim id As Integer = Escape(grp, PASSTHROUGH, 0, pData, IntPtr.Zero)
            RetVal = id > 0
        Finally
            Marshal.FreeHGlobal(pData)
        End Try
        Return RetVal
    End Function

    Public Function SendPassThrough(ByVal PassthroughData As String) As Boolean
        Dim RetVal As Boolean = False
        Dim grp As IntPtr = graphics.GetHdc()
        Try
            RetVal = SendPassThrough(PassthroughData, grp)
        Finally
            graphics.ReleaseHdc(grp)
        End Try
        Return RetVal
    End Function

    Private Function string2Global(ByVal Data As String, ByVal includeSize As Boolean) As IntPtr
        Dim l As Integer = Data.Length
        Dim offset As Integer = 0
        If includeSize Then
            l += 2
            offset = 2
        End If
        Dim RetVal As IntPtr = Marshal.AllocHGlobal(l)
        Dim s As Short = CShort(Data.Length)
        Dim buf As Byte() = New Byte(l - 1) {}
        If includeSize Then
            buf(1) = CByte((s >> 8))
            buf(0) = CByte(s)
        End If
        System.Text.Encoding.[Default].GetBytes(Data, 0, Data.Length, buf, offset)
        For i As Integer = 0 To buf.Length - 1
            Marshal.WriteByte(RetVal, i, buf(i))
        Next
        Return RetVal
    End Function
End Class

