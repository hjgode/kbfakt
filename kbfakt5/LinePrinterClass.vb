Imports System.Runtime.InteropServices

Public Class LinePrinter

#Region " Shared Win32 API Calls "

    Public Structure DOCINFO
        <MarshalAs(UnmanagedType.LPWStr)> Public pDocName As String
        <MarshalAs(UnmanagedType.LPWStr)> Public pOutputFile As String
        <MarshalAs(UnmanagedType.LPWStr)> Public pDataType As String
    End Structure

    <DllImport("winspool.drv", CharSet:=CharSet.Unicode, ExactSpelling:=False, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function OpenPrinter(ByVal pPrinterName As String, ByRef phPrinter As IntPtr, ByVal pDefault As Integer) As Long
    End Function

    <DllImport("winspool.drv", CharSet:=CharSet.Unicode, ExactSpelling:=False, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function StartDocPrinter(ByVal hPrinter As IntPtr, ByVal Level As Integer, ByRef pDocInfo As DOCINFO) As Long
    End Function

    <DllImport("winspool.drv", CharSet:=CharSet.Unicode, ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function StartPagePrinter(ByVal hPrinter As IntPtr) As Long
    End Function

    <DllImport("winspool.drv", CharSet:=CharSet.Ansi, ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function WritePrinter(ByVal hPrinter As IntPtr, ByVal data As String, ByVal buf As Integer, ByRef pcWritten As Integer) As Long
    End Function

    <DllImport("winspool.drv", CharSet:=CharSet.Unicode, ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function EndPagePrinter(ByVal hPrinter As IntPtr) As Long
    End Function

    <DllImport("winspool.drv", CharSet:=CharSet.Unicode, ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function EndDocPrinter(ByVal hPrinter As IntPtr) As Long
    End Function

    <DllImport("winspool.drv", CharSet:=CharSet.Unicode, ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function ClosePrinter(ByVal hPrinter As IntPtr) As Long
    End Function

#End Region

#Region " Public Methods "

    Private miHandle As New System.IntPtr() 'handle to open printer

    Public Sub New(ByVal sDeviceName As String, Optional ByVal sDocName As String = "Print Job")
        Dim puDI As DOCINFO
        Dim plReturn As Long

        Try
            plReturn = LinePrinter.OpenPrinter(sDeviceName, miHandle, 0)
            If plReturn = 0 Then
                Err.Raise(513, , "Error opening specified printer")
            Else
                'job started, give it the name and get it ready for output
                puDI.pDocName = sDocName
                puDI.pDataType = vbNullString
                puDI.pOutputFile = vbNullString
                plReturn = LinePrinter.StartDocPrinter(miHandle, 1, puDI)
            End If
        Catch ex As System.Exception
            Err.Raise(Err.Number, , ex.Message)
        End Try
    End Sub

    Public Sub EndJob(Optional ByVal bAppendFormFeed As Boolean = False)
        Dim plReturn As Long

        If bAppendFormFeed Then
            Call WriteChars(ControlChars.FormFeed)
        End If
        If miHandle <> 0 Then
            Try
                plReturn = LinePrinter.EndPagePrinter(miHandle)
                plReturn = LinePrinter.EndDocPrinter(miHandle)
                plReturn = LinePrinter.ClosePrinter(miHandle)
                System.Windows.Forms.Application.DoEvents()
                miHandle = 0
            Catch
                'just skip it
                miHandle = 0
            End Try
        End If
    End Sub

    Public Sub WriteLine(ByVal sLine As String, Optional ByVal bSuppressError As Boolean = True)
        Dim piReturn As Long = 0
        Dim piWritten As Integer = 0

        'append cr/lf pair if not present on the line
        If Right(sLine, 2) <> vbCrLf Then
            sLine = sLine & vbCrLf
        End If
        If miHandle <> 0 Then
            Try
                piReturn = LinePrinter.WritePrinter(miHandle, sLine, sLine.Length, piWritten)
                If Not bSuppressError Then
                    If piWritten <> sLine.Length Then
                        Err.Raise(514, , "Fewer characters written to printer than were supplied.")
                    End If
                End If
            Catch ex As System.Exception
                If Not bSuppressError Then
                    Err.Raise(Err.Number, , ex.Message)
                End If
            End Try
        End If
    End Sub

    Public Sub WriteChars(ByVal sChars As String, Optional ByVal bSuppressError As Boolean = True)
        Dim piReturn As Long = 0
        Dim piWritten As Integer = 0

        If miHandle <> 0 Then
            Try
                piReturn = LinePrinter.WritePrinter(miHandle, sChars, sChars.Length, piWritten)
                If Not bSuppressError Then
                    If piWritten <> sChars.Length Then
                        Err.Raise(514, , "Fewer characters written to printer than were supplied.")
                    End If
                End If
            Catch ex As System.Exception
                If Not bSuppressError Then
                    Err.Raise(Err.Number, , ex.Message)
                End If
            End Try
        End If
    End Sub

#End Region

End Class