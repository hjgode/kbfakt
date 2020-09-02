'Title:       Simple Dynamic Report with SQL Query
'Author:      Pakorn Indhatep
'Email:       pakorncs@bizslash.com
'Environment: Visual Baic.NET
'Keywords:    Dynamic Report, Report, printDocument
'Description: Dynamic report by printDocument Component with SQL Query
'Section      VB.NET
'SubSection   Printing

Imports System.Drawing
Imports System.Windows.Forms

Public Class frmPrintDynamicForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Public WithEvents prnDoc As System.Drawing.Printing.PrintDocument
    Public WithEvents printdlg As System.Windows.Forms.PrintPreviewDialog
    Public WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents btnPrint As System.Windows.Forms.Button
    Public WithEvents btnClose As System.Windows.Forms.Button
    Public WithEvents btnPrnDlg As System.Windows.Forms.Button
    Public WithEvents btnDocSet As System.Windows.Forms.Button
    Public WithEvents prnSetDlg As System.Windows.Forms.PrintDialog
    Public WithEvents PageSetDlg As System.Windows.Forms.PageSetupDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPrintDynamicForm))
        Me.prnDoc = New System.Drawing.Printing.PrintDocument
        Me.printdlg = New System.Windows.Forms.PrintPreviewDialog
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnDocSet = New System.Windows.Forms.Button
        Me.btnPrnDlg = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.PageSetDlg = New System.Windows.Forms.PageSetupDialog
        Me.prnSetDlg = New System.Windows.Forms.PrintDialog
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'prnDoc
        '
        '
        'printdlg
        '
        Me.printdlg.AutoScrollMargin = New System.Drawing.Size(0, 0)
        Me.printdlg.AutoScrollMinSize = New System.Drawing.Size(0, 0)
        Me.printdlg.ClientSize = New System.Drawing.Size(400, 300)
        Me.printdlg.Document = Me.prnDoc
        Me.printdlg.Enabled = True
        Me.printdlg.Icon = CType(resources.GetObject("printdlg.Icon"), System.Drawing.Icon)
        Me.printdlg.Name = "printdlg"
        Me.printdlg.Visible = False
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btnDocSet)
        Me.Panel1.Controls.Add(Me.btnPrnDlg)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnClose)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 137)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(504, 40)
        Me.Panel1.TabIndex = 14
        '
        'btnDocSet
        '
        Me.btnDocSet.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnDocSet.Image = CType(resources.GetObject("btnDocSet.Image"), System.Drawing.Image)
        Me.btnDocSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDocSet.Location = New System.Drawing.Point(136, 8)
        Me.btnDocSet.Name = "btnDocSet"
        Me.btnDocSet.Size = New System.Drawing.Size(116, 24)
        Me.btnDocSet.TabIndex = 12
        Me.btnDocSet.Text = "Page Setup"
        Me.btnDocSet.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnPrnDlg
        '
        Me.btnPrnDlg.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnPrnDlg.Image = CType(resources.GetObject("btnPrnDlg.Image"), System.Drawing.Image)
        Me.btnPrnDlg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrnDlg.Location = New System.Drawing.Point(12, 8)
        Me.btnPrnDlg.Name = "btnPrnDlg"
        Me.btnPrnDlg.Size = New System.Drawing.Size(116, 24)
        Me.btnPrnDlg.TabIndex = 11
        Me.btnPrnDlg.Text = "Printer Setup"
        Me.btnPrnDlg.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnPrint
        '
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnPrint.Image = CType(resources.GetObject("btnPrint.Image"), System.Drawing.Image)
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(340, 8)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(72, 24)
        Me.btnPrint.TabIndex = 10
        Me.btnPrint.Text = "Print"
        Me.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnClose
        '
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(420, 8)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(72, 24)
        Me.btnClose.TabIndex = 9
        Me.btnClose.Text = "Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PageSetDlg
        '
        Me.PageSetDlg.Document = Me.prnDoc
        '
        'prnSetDlg
        '
        Me.prnSetDlg.Document = Me.prnDoc
        '
        'frmPrintDynamicForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(504, 177)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmPrintDynamicForm"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    '----------------- Set Report Default Value
    Public HeadFirstString As String
    Public HeadSecondString As String
    Public DateString As String
    Public FontName As String = "Tahoma"
    Public FontSize As Single = 12
    Public FontSizeHead As Single = 16
    Public FontSizeHead2 As Single = 14
    Public LineHeight As Single = 1
    Public SkipLinePerRecord As Byte = 0
    Public WrapTextFlag As Boolean = False
    '------------------ Have Summary?
    Public hasSum As Boolean = False
    '------ Public for manual initial Dataset
    Public dbSet As New DataSet

    '------ Internal Use Counter and Flag 
    Private printSumComplete As Boolean = False
    Private dbAdapt As New Data.SqlClient.SqlDataAdapter
    Private PageNumber As Integer = 0
    Private dbCursor As Long = 0L
    Private SumList As New ArrayList
    Private LineAmountPerRecord As Integer = 0

    Public Const C_column_name As Integer = 0
    Public Const C_start_position As Integer = 1
    Public Const C_end_position As Integer = 2
    Public Const C_justify As Integer = 3
    Public Const C_has_sumarize As Integer = 4
    Public Const C_display_format As Integer = 5
    Public Const C_rest_in_line As Integer = 6
    Public Const C_data As Integer = 7
    Public Const C_amount_of_column = 8

    Function getPositionWidth(ByVal p_width As Single, ByVal p_position As Single) As Single
        Return p_width * p_position / 100
    End Function

    '----- Set SQL string for report by this format (1 set per 1 report column):
    '----- Column 1 -> column name
    '----- Column 2 -> Start position on paper, I set all width has range between 0-99
    '----- Column 3 -> End position on paper, I set all width has range between 0-99
    '----- Column 4 -> Justify (L-Left, R-Right, C-Center)
    '----- Column 5 -> Has summarize in this column (Y/N)
    '----- Column 6 -> Display format(such as #,##0.00)
    '----- Column 7 -> Rest in line? Begin with 1
    '----- Column 8 -> Data Column
    '----- Please look at this example-------------------------------------------
    Private Sub prnDoc_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles prnDoc.PrintPage
        Dim LeftMargin As Integer = e.MarginBounds.Left
        Dim RightMargin As Integer = e.MarginBounds.Right
        Dim TopMargin As Integer = e.MarginBounds.Top
        Dim LinesInPage As Integer = 0
        Dim YPosition As Integer = 0
        Dim CountLine As Integer = 0
        Dim CenterPosition As Integer = 0
        'Dim CurrentLine As String
        Dim myFont As New Font(FontName, FontSize, FontStyle.Regular, GraphicsUnit.Point)
        Dim myHeadFont As New Font(FontName, FontSizeHead, FontStyle.Regular, GraphicsUnit.Point)
        Dim myHead2Font As New Font(FontName, FontSizeHead2, FontStyle.Regular, GraphicsUnit.Point)
        Dim myPen As New Pen(Color.Black)
        Dim StrFormatCenter As New StringFormat
        Dim StrFormatLeft As New StringFormat
        Dim StrFormatRight As New StringFormat
        Dim ColCount, ColCountSum As Integer

        Dim sizeR As New System.Drawing.SizeF
        Dim ColumnWidthWrap As Single = 0
        Dim RowSizeWrap As New System.Drawing.SizeF
        Dim RowHeightWrap As Single = 0
        Dim MaxRowHeightWrap As Single = 0
        Try
            If dbSet.Tables(0).Rows.Count <= 0 Then
                MessageBox.Show("have not data", "have not data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            PageNumber += 1
            StrFormatLeft.Alignment = StringAlignment.Near
            StrFormatRight.Alignment = StringAlignment.Far
            StrFormatCenter.Alignment = StringAlignment.Center
            LinesInPage = e.MarginBounds.Height / myFont.GetHeight(e.Graphics)
            For i As Integer = 0 To dbSet.Tables(0).Columns.Count - 1 Step C_amount_of_column
                If dbSet.Tables(0).Rows(0)(i + C_rest_in_line) > LineAmountPerRecord Then
                    LineAmountPerRecord = dbSet.Tables(0).Rows(0)(i + C_rest_in_line)
                End If
            Next i

            '----- Decrease line in page by header line count --------
            LinesInPage -= 3
            CenterPosition = LeftMargin + e.MarginBounds.Width / 2

            YPosition = TopMargin
            e.Graphics.DrawString(HeadFirstString, myHeadFont, Brushes.Black, CenterPosition, YPosition, StrFormatCenter)
            e.Graphics.DrawString("page " & CStr(PageNumber), myFont, Brushes.Black, LeftMargin, YPosition, StrFormatLeft)
            e.Graphics.DrawString("date " & CStr(Now.Date), myFont, Brushes.Black, RightMargin, YPosition, StrFormatRight)
            CountLine += 1
            YPosition = TopMargin + (CountLine * myHead2Font.GetHeight(e.Graphics))
            e.Graphics.DrawString(HeadSecondString, myHead2Font, Brushes.Black, CenterPosition, YPosition, StrFormatCenter)
            CountLine += 1
            YPosition = TopMargin + (CountLine * myHead2Font.GetHeight(e.Graphics))
            e.Graphics.DrawString(DateString, myFont, Brushes.Black, CenterPosition, YPosition, StrFormatCenter)
            CountLine += 2
            YPosition = TopMargin + (CountLine * myFont.GetHeight(e.Graphics))
            e.Graphics.DrawLine(myPen, LeftMargin, YPosition + (myFont.GetHeight(e.Graphics) / 2), RightMargin, YPosition + (myFont.GetHeight(e.Graphics) / 2))
            CountLine += 1
            YPosition = TopMargin + (CountLine * myFont.GetHeight(e.Graphics))
            '----------- Column Header --------------
            Dim oldRestInLineH As Integer = 1
            For j As Integer = 1 To LineAmountPerRecord
                If oldRestInLineH < j Then
                    oldRestInLineH = j
                    CountLine += LineHeight
                    YPosition = TopMargin + (CountLine * myFont.GetHeight(e.Graphics))
                End If
                For i As Integer = 0 To dbSet.Tables(0).Columns.Count - 1 Step C_amount_of_column
                    If dbSet.Tables(0).Rows(dbCursor)(i + C_rest_in_line) = j Then
                        If dbSet.Tables(0).Rows(0)(i + C_justify) = "L" Then
                            e.Graphics.DrawString(dbSet.Tables(0).Rows(0)(i), myFont, Brushes.Black, _
                                           RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                           YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                           YPosition + myFont.GetHeight(e.Graphics)), _
                                           StrFormatLeft)
                        ElseIf dbSet.Tables(0).Rows(0)(i + C_justify) = "R" Then
                            e.Graphics.DrawString(dbSet.Tables(0).Rows(0)(i), myFont, Brushes.Black, _
                                           RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                           YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                           YPosition + myFont.GetHeight(e.Graphics)), _
                                           StrFormatRight)
                        ElseIf dbSet.Tables(0).Rows(0)(i + C_justify) = "C" Then
                            e.Graphics.DrawString(dbSet.Tables(0).Rows(0)(i), myFont, Brushes.Black, _
                                           RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                           YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                           YPosition + myFont.GetHeight(e.Graphics)), _
                                           StrFormatCenter)
                        End If
                        '---------- Prepare Sum ------------------
                        SumList.Add(0)
                    End If
                Next i
            Next j

            '----------- Report Detail --------------
            CountLine += 1
            YPosition = TopMargin + (CountLine * myFont.GetHeight(e.Graphics))
            e.Graphics.DrawLine(myPen, LeftMargin, YPosition + (myFont.GetHeight(e.Graphics) / 2), RightMargin, YPosition + (myFont.GetHeight(e.Graphics) / 2))
            CountLine += 1

            While (CountLine < LinesInPage) And (dbCursor < dbSet.Tables(0).Rows.Count)
                ColCount = 0
                YPosition = TopMargin + (CountLine * myFont.GetHeight(e.Graphics))
                Dim oldRestInLine As Integer = 1
                For j As Integer = 1 To LineAmountPerRecord
                    If oldRestInLine < j Then
                        oldRestInLine = j
                        CountLine += LineHeight
                        YPosition = TopMargin + (CountLine * myFont.GetHeight(e.Graphics))
                    End If

                    ' -------- Begin For Wrap Text Feature Only ----------  
                    If WrapTextFlag Then
                        MaxRowHeightWrap = 0
                        For i As Integer = 0 To dbSet.Tables(0).Columns.Count - 1 Step C_amount_of_column
                            ColumnWidthWrap = getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(dbCursor)(i + C_end_position) - dbSet.Tables(0).Rows(dbCursor)(i + C_start_position))
                            sizeR.Width = ColumnWidthWrap
                            sizeR.Height = 0
                            If dbSet.Tables(0).Rows(dbCursor)(i + C_display_format) = "" Then '--- Have Format ?
                                RowSizeWrap = e.Graphics.MeasureString(dbSet.Tables(0).Rows(dbCursor)(i + C_data), myFont, sizeR)
                            Else
                                RowSizeWrap = e.Graphics.MeasureString(Format(dbSet.Tables(0).Rows(dbCursor)(i + C_data), dbSet.Tables(0).Rows(dbCursor)(i + C_display_format)), myFont, sizeR)
                            End If
                            RowHeightWrap = Math.Ceiling(RowSizeWrap.Height / myFont.GetHeight(e.Graphics))
                            If RowHeightWrap > MaxRowHeightWrap Then
                                MaxRowHeightWrap = RowHeightWrap
                            End If
                        Next i
                        MaxRowHeightWrap -= 2
                    End If
                    ' -------- End For Wrap Text Feature Only ----------  

                    For i As Integer = 0 To dbSet.Tables(0).Columns.Count - 1 Step C_amount_of_column
                        If dbSet.Tables(0).Rows(dbCursor)(i + C_rest_in_line) = j Then
                            If dbSet.Tables(0).Rows(dbCursor)(i + C_display_format) = "" Then '--- Have Format ?
                                If dbSet.Tables(0).Rows(dbCursor)(i + C_justify) = "L" Then
                                    e.Graphics.DrawString(CString(dbSet.Tables(0).Rows(dbCursor)(i + C_data)), myFont, Brushes.Black, _
                                                           RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                                           YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                                           YPosition + (myFont.GetHeight(e.Graphics) * (LineHeight + MaxRowHeightWrap))), _
                                                           StrFormatLeft)
                                ElseIf dbSet.Tables(0).Rows(dbCursor)(i + C_justify) = "R" Then
                                    e.Graphics.DrawString(CString(dbSet.Tables(0).Rows(dbCursor)(i + C_data)), myFont, Brushes.Black, _
                                                           RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                                           YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                                           YPosition + (myFont.GetHeight(e.Graphics) * (LineHeight + MaxRowHeightWrap))), _
                                                           StrFormatRight)
                                ElseIf dbSet.Tables(0).Rows(dbCursor)(i + C_justify) = "C" Then
                                    e.Graphics.DrawString(CString(dbSet.Tables(0).Rows(dbCursor)(i + C_data)), myFont, Brushes.Black, _
                                                           RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                                           YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                                           YPosition + (myFont.GetHeight(e.Graphics) * (LineHeight + MaxRowHeightWrap))), _
                                                           StrFormatCenter)
                                End If
                            Else '-------- Have Format , Yes --------------------
                                If dbSet.Tables(0).Rows(dbCursor)(i + C_justify) = "L" Then 'HGO
                                    e.Graphics.DrawString(myFormat(dbSet.Tables(0).Rows(dbCursor)(i + C_data), dbSet.Tables(0).Rows(dbCursor)(i + C_display_format)), myFont, Brushes.Black, _
                                                           RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                                           YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                                           YPosition + (myFont.GetHeight(e.Graphics) * (LineHeight + MaxRowHeightWrap))), _
                                                           StrFormatLeft)
                                ElseIf dbSet.Tables(0).Rows(dbCursor)(i + C_justify) = "R" Then
                                    e.Graphics.DrawString(myFormat(dbSet.Tables(0).Rows(dbCursor)(i + C_data), dbSet.Tables(0).Rows(dbCursor)(i + C_display_format)), myFont, Brushes.Black, _
                                                           RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                                           YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                                           YPosition + (myFont.GetHeight(e.Graphics) * (LineHeight + MaxRowHeightWrap))), _
                                                           StrFormatRight)
                                ElseIf dbSet.Tables(0).Rows(dbCursor)(i + C_justify) = "C" Then
                                    e.Graphics.DrawString(myFormat(dbSet.Tables(0).Rows(dbCursor)(i + C_data), dbSet.Tables(0).Rows(dbCursor)(i + C_display_format)), myFont, Brushes.Black, _
                                                           RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                                           YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                                           YPosition + (myFont.GetHeight(e.Graphics) * (LineHeight + MaxRowHeightWrap))), _
                                                           StrFormatCenter)
                                End If
                            End If
                            '------------------ sum ---------------------
                            If dbSet.Tables(0).Rows(dbCursor)(i + C_has_sumarize) = "Y" Then
                                SumList(ColCount) += dbSet.Tables(0).Rows(dbCursor)(i + C_data)
                            End If
                            '------------------ end sum -----------------
                        End If
                        ColCount += 1
                    Next i
                Next j
                CountLine += LineHeight + SkipLinePerRecord + MaxRowHeightWrap
                dbCursor += 1
            End While
            CountLine -= 1

            If dbCursor = dbSet.Tables(0).Rows.Count Then '----------End of Data and Has Sum
                If hasSum Then
                    CountLine += 1
                    YPosition = TopMargin + (CountLine * myFont.GetHeight(e.Graphics))
                    e.Graphics.DrawLine(myPen, LeftMargin, YPosition + (myFont.GetHeight(e.Graphics) / 2), RightMargin, YPosition + (myFont.GetHeight(e.Graphics) / 2))

                    CountLine += 1
                    YPosition = TopMargin + (CountLine * myFont.GetHeight(e.Graphics))
                    Dim oldRestInLine As Integer = 1
                    For j As Integer = 1 To LineAmountPerRecord
                        If oldRestInLine < j Then
                            oldRestInLine = j
                            CountLine += LineHeight
                            YPosition = TopMargin + (CountLine * myFont.GetHeight(e.Graphics))
                        End If
                        ColCountSum = 0
                        For i As Integer = 0 To dbSet.Tables(0).Columns.Count - 1 Step C_amount_of_column
                            If dbSet.Tables(0).Rows(0)(i + C_rest_in_line) = j Then

                                If dbSet.Tables(0).Rows(0)(i + 4) = "Y" Then
                                    If dbSet.Tables(0).Rows(0)(i + C_display_format) = "" Then '------- Have Format ?
                                        If dbSet.Tables(0).Rows(0)(i + C_justify) = "L" Then
                                            e.Graphics.DrawString(SumList(ColCountSum), myFont, Brushes.Black, _
                                                                   RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                                                   YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                                                   YPosition + myFont.GetHeight(e.Graphics)), _
                                                                   StrFormatLeft)
                                        ElseIf dbSet.Tables(0).Rows(0)(i + C_justify) = "R" Then
                                            e.Graphics.DrawString(SumList(ColCountSum), myFont, Brushes.Black, _
                                                                   RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                                                   YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                                                   YPosition + myFont.GetHeight(e.Graphics)), _
                                                                   StrFormatRight)
                                        ElseIf dbSet.Tables(0).Rows(0)(i + C_justify) = "C" Then
                                            e.Graphics.DrawString(SumList(ColCountSum), myFont, Brushes.Black, _
                                                                   RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                                                   YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                                                   YPosition + myFont.GetHeight(e.Graphics)), _
                                                                   StrFormatCenter)
                                        End If
                                    Else '-------- Have Format , Yes --------------------
                                        If dbSet.Tables(0).Rows(0)(i + C_justify) = "L" Then
                                            e.Graphics.DrawString(Format(SumList(ColCountSum), dbSet.Tables(0).Rows(0)(i + C_display_format)), myFont, Brushes.Black, _
                                                                   RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                                                   YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                                                   YPosition + myFont.GetHeight(e.Graphics)), _
                                                                   StrFormatLeft)
                                        ElseIf dbSet.Tables(0).Rows(0)(i + C_justify) = "R" Then
                                            e.Graphics.DrawString(Format(SumList(ColCountSum), dbSet.Tables(0).Rows(0)(i + C_display_format)), myFont, Brushes.Black, _
                                                                   RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                                                   YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                                                   YPosition + myFont.GetHeight(e.Graphics)), _
                                                                   StrFormatRight)
                                        ElseIf dbSet.Tables(0).Rows(0)(i + C_justify) = "C" Then
                                            e.Graphics.DrawString(Format(SumList(ColCountSum), dbSet.Tables(0).Rows(0)(i + C_display_format)), myFont, Brushes.Black, _
                                                                   RectangleF.FromLTRB(LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_start_position)), _
                                                                   YPosition, LeftMargin + getPositionWidth(e.MarginBounds.Width, dbSet.Tables(0).Rows(0)(i + C_end_position)), _
                                                                   YPosition + myFont.GetHeight(e.Graphics)), _
                                                                   StrFormatCenter)
                                        End If
                                    End If
                                End If
                            End If
                            ColCountSum += 1
                        Next i
                    Next j
                    printSumComplete = True

                    CountLine += 1
                    YPosition = TopMargin + (CountLine * myFont.GetHeight(e.Graphics))
                    e.Graphics.DrawLine(myPen, LeftMargin, YPosition + (myFont.GetHeight(e.Graphics) / 2), RightMargin, YPosition + (myFont.GetHeight(e.Graphics) / 2))
                    e.Graphics.DrawLine(myPen, LeftMargin, 5 + YPosition + (myFont.GetHeight(e.Graphics) / 2), RightMargin, 5 + YPosition + (myFont.GetHeight(e.Graphics) / 2))

                End If
            End If

            If hasSum Then
                If Not (dbCursor = dbSet.Tables(0).Rows.Count) And (Not printSumComplete) Then
                    e.HasMorePages = True
                Else
                    e.HasMorePages = False
                End If
            Else
                If Not (dbCursor = dbSet.Tables(0).Rows.Count) Then
                    e.HasMorePages = True
                Else
                    e.HasMorePages = False
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Please send error message to me for better program, Thank you : " & Chr(13) & Chr(10) & Err.Description)
        End Try
    End Sub

    Private Function CString(ByVal o As Object)
        Dim s As String
        Try
            s = CStr(o)
        Catch ex As Exception
            s = ""
        End Try
        Return s
    End Function
    Private Function myFormat(ByVal o As Object, ByVal f As Object) As String
        'Format(
        '    dbSet.Tables(0).Rows(dbCursor)(i + C_data), 
        '    dbSet.Tables(0).Rows(dbCursor)(i + C_display_format))
        Dim res, s, frm As String
        Try
            s = CStr(o)
            frm = CStr(f)
            res = String.Format(s, frm)
        Catch ex As Exception
            res = ""
        End Try
        Return res
    End Function
    Public Overridable Sub InitPrint()
        ClearPage()
        prnDoc.DefaultPageSettings.Margins.Left = 50
        prnDoc.DefaultPageSettings.Margins.Top = 50
        prnDoc.DefaultPageSettings.Margins.Right = 50
        prnDoc.DefaultPageSettings.Margins.Bottom = 50
    End Sub

    Public Overridable Sub PrintPreview()
        printdlg.WindowState = Windows.Forms.FormWindowState.Maximized
        printdlg.ShowDialog()
    End Sub

    Public Overridable Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Close()
    End Sub

    Public Overridable Sub btnPrnDlg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrnDlg.Click
        prnSetDlg.ShowDialog()
    End Sub

    Public Overridable Sub btnDocSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDocSet.Click
        PageSetDlg.ShowDialog()
    End Sub

    Public Sub ClearPage()
        printSumComplete = False
        PageNumber = 0
        dbCursor = 0L
        SumList.Clear()
        LineAmountPerRecord = 0
    End Sub


    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

    End Sub
End Class
