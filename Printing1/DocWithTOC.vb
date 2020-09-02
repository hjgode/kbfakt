' ----- Enhanced Print Preview Application
'       Written by Tim Patrick
'       For Visual Studio Magazine

Option Explicit On 
Option Strict On

Public Class DocumentWithTOC
    ' ----- This class enhances the .NET PrintDocument class by adding
    '       a Table of Contents index. This allows a user to quickly
    '       jump to a section of the document.
    Inherits System.Drawing.Printing.PrintDocument

    Public TableOfContents As System.Collections.ArrayList

    Public Sub AddTOCEntry(ByVal entryName As String, ByVal indentLevel As Byte, ByVal whichPage As Integer)
        ' ----- Add a new table of contents entry to the document. This is
        '       called by the user during the actual printing of each page.
        Dim tocEntry As TableOfContentsEntry

        tocEntry = New PrintingTest.TableOfContentsEntry
        tocEntry.EntryText = entryName
        tocEntry.Indent = indentLevel
        tocEntry.PageNumber = whichPage
        TableOfContents.Add(tocEntry)
    End Sub

    Protected Overrides Sub OnBeginPrint(ByVal e As System.Drawing.Printing.PrintEventArgs)
        ' ----- Before printing, create room for a table of contents.
        TableOfContents = New System.Collections.ArrayList
        MyBase.OnBeginPrint(e)
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        ' ----- Free the memory associated with the table of conents.
        TableOfContents.Clear()
        TableOfContents = Nothing
        MyBase.Dispose(disposing)
    End Sub
End Class
