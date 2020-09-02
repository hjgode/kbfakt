' PrintDirect.vb
' Shows how to write data directly to the printer using Win32 API's
'Adapted from Microsoft Support article Q298141
'This code assumes you have a printer at share file://192.168.1.101/hpl
'This code sends Hewlett Packard PCL5 codes to the printer to print
' out a rectangle in the middle of the page. 
Imports System
Imports System.Text
Imports System.Runtime.InteropServices
<StructLayout(LayoutKind.Sequential)> _
Public Structure DOCINFO<MarshalAs(UnmanagedType.LPWStr)> 
Public pDocName As String<MarshalAs(UnmanagedType.LPWStr)> 
Public pOutputFile As String<MarshalAs(UnmanagedType.LPWStr)> 
Public pDataType As String
End Structure 'DOCINFO
Public Class PrintDirect
<DllImport("winspool.drv", CharSet := CharSet.Unicode, ExactSpelling := False, CallingConvention := CallingConvention.StdCall)> _
Public Shared Function OpenPrinter(pPrinterName As String, ByRef phPrinter As IntPtr, pDefault As Integer) As Long
<DllImport("winspool.drv", CharSet := CharSet.Unicode, ExactSpelling := False, CallingConvention := CallingConvention.StdCall)> _
Public Shared Function StartDocPrinter(hPrinter As IntPtr, Level As Integer, ByRef pDocInfo As DOCINFO) As Long
<DllImport("winspool.drv", CharSet := CharSet.Unicode, ExactSpelling := True, CallingConvention := CallingConvention.StdCall)> _
Public Shared Function StartPagePrinter(hPrinter As IntPtr) As Long
<DllImport("winspool.drv", CharSet := CharSet.Ansi, ExactSpelling := True, CallingConvention := CallingConvention.StdCall)> _
Public Shared Function WritePrinter(hPrinter As IntPtr, data As String, buf As Integer, ByRef pcWritten As Integer) As Long
<DllImport("winspool.drv", CharSet := CharSet.Unicode, ExactSpelling := True, CallingConvention := CallingConvention.StdCall)> _
Public Shared Function EndPagePrinter(hPrinter As IntPtr) As Long
<DllImport("winspool.drv", CharSet := CharSet.Unicode, ExactSpelling := True, CallingConvention := CallingConvention.StdCall)> _
Public Shared Function EndDocPrinter(hPrinter As IntPtr) As Long
<DllImport("winspool.drv", CharSet := CharSet.Unicode, ExactSpelling := True, CallingConvention := CallingConvention.StdCall)> _
Public Shared Function ClosePrinter(hPrinter As IntPtr) As Long
End Class 'PrintDirect
Public Class App
Public Shared Sub Main()
Dim lhPrinter As New System.IntPtr()
Dim di As New DOCINFO()
Dim pcWritten As Integer = 0
Dim st1 As String
' text to print with a form feed character 
st1 = "This is an example of printing directly to a printer" + ControlChars.FormFeed
di.pDocName = "my test document"
di.pDataType = "RAW"
' the \x1b means an ascii escape character
st1 = ChrW(27) + "*c600a6b0P" + ControlChars.FormFeed
'lhPrinter contains the handle for the printer opened
'If lhPrinter is 0 then an error has occured
PrintDirect.OpenPrinter("\\192.168.1.101\hpl", lhPrinter, 0)
PrintDirect.StartDocPrinter(lhPrinter, 1, di)
PrintDirect.StartPagePrinter(lhPrinter)
Try
' Moves the cursor 900 dots (3 inches at 300 dpi) in from the left margin, and
' 600 dots (2 inches at 300 dpi) down from the top margin.
st1 = ChrW(27) + "*p900x600Y"
PrintDirect.WritePrinter(lhPrinter, st1, st1.Length, pcWritten)
' Using the print model commands for rectangle dimensions, "600a" specifies a rectangle
' with a horizontal size or width of 600 dots, and "6b" specifies a vertical
' size or height of 6 dots. The 0P selects the solid black rectangular area fill.
st1 = ChrW(27) + "*c600a6b0P"
PrintDirect.WritePrinter(lhPrinter, st1, st1.Length, pcWritten)
' Specifies a rectangle with width of 6 dots, height of 600 dots, and a
' fill pattern of solid black.
st1 = ChrW(27) + "*c6a600b0P"
PrintDirect.WritePrinter(lhPrinter, st1, st1.Length, pcWritten)
' Moves the current cursor position to 900 dots, from the left margin and
' 1200 dots down from the top margin.
st1 = ChrW(27) + "*p900x1200Y"
PrintDirect.WritePrinter(lhPrinter, st1, st1.Length, pcWritten)
' Specifies a rectangle with a width of 606 dots, a height of 6 dots and a // fill pattern of solid black.
st1 = ChrW(27) + "*c606a6b0P"
PrintDirect.WritePrinter(lhPrinter, st1, st1.Length, pcWritten)
' Moves the current cursor position to 1500 dots from the left margin and
' 600 dots down from the top margin.
st1 = ChrW(27) + "*p1500x600Y"
PrintDirect.WritePrinter(lhPrinter, st1, st1.Length, pcWritten)
' Specifies a rectangle with a width of 6 dots, a height of 600 dots and a
' fill pattern of solid black.
st1 = ChrW(27) + "*c6a600b0P"
PrintDirect.WritePrinter(lhPrinter, st1, st1.Length, pcWritten) ' Send a form feed character to the printer
st1 = ControlChars.FormFeed
PrintDirect.WritePrinter(lhPrinter, st1, st1.Length, pcWritten)
Catch e As Exception
Console.WriteLine(e.Message)
End Try
PrintDirect.EndPagePrinter(lhPrinter)
PrintDirect.EndDocPrinter(lhPrinter)
PrintDirect.ClosePrinter(lhPrinter)
End Sub 'Main
End Class 'App