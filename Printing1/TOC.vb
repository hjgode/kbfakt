' ----- Enhanced Print Preview Application
'       Written by Tim Patrick
'       For Visual Studio Magazine

Option Explicit On 
Option Strict On

Public Class TableOfContentsEntry
    ' ----- This class identifies a single entry in the Table of Contents list
    '       that appears on the left-side of the enhanced Print Preview form.
    Public EntryText As String   ' The display text.
    Public Indent As Byte        ' How far to indent the display text.
    Public PageNumber As Integer ' Where to jump in the document.

    Public Overrides Function ToString() As String
        ' ----- Display the text as an indented string. Use groups of five
        '       spaces as the indent. Limit the indent between 1 and 10.
        Return (New String(" "c, 5 * (Math.Min(Math.Max(1, Indent), 10) - 1))) & EntryText
    End Function
End Class
