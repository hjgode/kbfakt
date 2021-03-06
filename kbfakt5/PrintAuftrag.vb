Option Explicit On
Imports System.Drawing.Printing
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Data.OleDb
Imports Microsoft.Win32


Public Class PrintAuftrag
    Public AuftragsNummern As New Collection
    Private currentAuftrNr As String = "0000"

    'Public AUFTR_Nr As String = ""
    Public DruckTyp As String = ""
    Public PreiseAnzeigen As Boolean = True
    Dim txtMwStSatz As String = ""
    Dim txtAltTeilMwStSatz As String = "" 'new with v 2.1.0.0
    Private AppPath As String
    Private currentPreviewPage As Integer = 0
    Private totalPrintedPages As Integer = 1

    Private reader As OleDb.OleDbDataReader
    Private connection As New OleDbConnection
    Private oleda As OleDbDataAdapter
    Private dbset As New DataSet
    Private TempFile As String = "kbfakt.txt"
    Private TempFiles As New System.CodeDom.Compiler.TempFileCollection
    Private FStream As StreamWriter
    Private KopfZeilen As New Collection
    Private FussZeilen As New Collection

    Dim currentPage As Integer = 1
    ''' <summary>
    ''' Printed as page x of y on rechnung
    ''' </summary>
    ''' <remarks></remarks>
    Dim totalPages As Integer = 1
    Dim numberOfItems As Integer = 0

    'Private CharsPerLine As Integer = 80 '12cpi
    'Dim linesPerPage As Single = 60 '????

    Private m_CharsPerLine As Integer = 80
    Private m_LinesPerPage As Integer = 60
    'Printing Margins
    Private m_Bottom As Integer
    Private m_Left As Integer
    Private m_Right As Integer
    Private m_Top As Integer
    Private m_TopOffsetLines As Integer = 0
    Private m_RawPageLength As Integer = 80

    Private m_PrinterName As String
    Private m_iPaperKind As Integer 'PaperKind
    Private m_bUsePrinterSettingsFromReg As Integer = 0

    Private Class myPaperSize
        Inherits PaperSize
        Public Shadows Function ToString() As String
            Return PaperName
        End Function
    End Class

    Dim pWidth As Long = 827 'CInt(21 / 2.54 * 100) '827
    Dim pHeight As Long = CInt(29.7 / 2.54 * 100) '1200 'CInt(30.5 / 2.54 * 100) '1200
    Dim m_PaperWidth As Long
    Dim m_PaperHeight As Long

    Dim pSize As New PaperSize("A4", pWidth, pHeight)

    Private printFont = New Font("Courier New", 10)
    Private printFontBold = New Font("Courier New", 10, FontStyle.Bold)
    Private printFontBigBold = New Font("Courier New", 12, FontStyle.Bold)

    Private streamToPrint As StreamReader
    Private Sub PrintAuftrag_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
    End Sub
    Private Sub readReg()
        ' The name of the key must include a valid root.
        Const userRoot As String = "HKEY_CURRENT_USER"
        Const subkey As String = "Software\KBFakt"
        Const keyName As String = userRoot & "\" & subkey
        Dim bottom As Integer = 50
        Dim left As Integer = 50
        Dim right As Integer = 50
        Dim top As Integer = 150
        Dim topOffsetLines As Integer = 0
        Dim RawPageLength As Integer = 80
        Dim PrinterName As String = "OKI Rechnungsdrucker"
        Dim iPaperKind As Integer = 129 'custom paper!
        Dim iUsePrinterSettingsFromReg = 0

        Try
            bottom = Registry.GetValue(keyName, "MarginBottom", 50)
            left = Registry.GetValue(keyName, "MarginLeft", 50)
            right = Registry.GetValue(keyName, "MarginRight", 50)
            top = Registry.GetValue(keyName, "MarginTop", 150)
            topOffsetLines = Registry.GetValue(keyName, "TopOffsetLines", 0)
            RawPageLength = Registry.GetValue(keyName, "RawPageLength", 80)
            'Dim pWidth As Integer = CInt(21 / 2.54 * 100)
            'Dim pHeight As Integer = CInt(30.5 / 2.54 * 100)
            m_PaperWidth = Registry.GetValue(keyName, "PaperWidth", pWidth)
            m_PaperHeight = Registry.GetValue(keyName, "PaperHeight", pHeight)

            m_PrinterName = Registry.GetValue(keyName, "PrinterName", PrinterName)
            m_iPaperKind = Registry.GetValue(keyName, "iPaperKind", iPaperKind)

            m_bUsePrinterSettingsFromReg = Registry.GetValue(keyName, "UsePrinterSettingsFromReg", iUsePrinterSettingsFromReg)

        Catch x As Exception
            If mainmodul.DebugModus Then
                MessageBox.Show("Exception in ReadReg()" + vbCrLf + x.Message)
            End If
        End Try
        m_Bottom = bottom
        m_Left = left
        m_Right = right
        m_Top = top
        m_TopOffsetLines = topOffsetLines
        m_RawPageLength = RawPageLength
        'txtTopOffset.Text = m_TopOffsetLines.ToString()
    End Sub
    Private Sub WriteReg()
        Const userRoot As String = "HKEY_CURRENT_USER"
        Const subkey As String = "Software\KBFakt"
        Const keyName As String = userRoot & "\" & subkey
        Try
            Registry.SetValue(keyName, "MarginBottom", m_Bottom)
            Registry.SetValue(keyName, "MarginLeft", m_Left)
            Registry.SetValue(keyName, "MarginRight", m_Right)
            Registry.SetValue(keyName, "MarginTop", m_Top)
            Registry.SetValue(keyName, "TopOffsetLines", m_TopOffsetLines)
            Registry.SetValue(keyName, "RawPageLength", m_RawPageLength)
            Registry.SetValue(keyName, "PaperWidth", m_PaperWidth)
            Registry.SetValue(keyName, "PaperHeight", m_PaperHeight)
            Registry.SetValue(keyName, "PrinterName", m_PrinterName)
            Registry.SetValue(keyName, "iPaperKind", m_iPaperKind)

        Catch x As Exception
            If mainmodul.DebugModus Then
                MessageBox.Show("Exception in WriteReg()" + vbCrLf + x.Message)
            End If
        End Try

    End Sub
    Public Shared Function DefaultPrinterName() As String
        Dim oPS As New System.Drawing.Printing.PrinterSettings

        Try
            DefaultPrinterName = oPS.PrinterName
        Catch ex As System.Exception
            DefaultPrinterName = ""
        Finally
            oPS = Nothing
        End Try
    End Function
    Public Shared Function DefaultPageSize() As String
        Dim oPS As New System.Drawing.Printing.PrinterSettings

        Try
            DefaultPageSize = oPS.DefaultPageSettings.PaperSize.PaperName
        Catch ex As System.Exception
            DefaultPageSize = ""
        Finally
            oPS = Nothing
        End Try
    End Function
    Private Sub PrintAuftrag_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.SuspendLayout()
        Try
            btnRAWSettings.Enabled = False
            btnRAWSettings.Visible = False

            AppPath = mainmodul.getAppDir()
            TempFile = AppPath + TempFile
            FStream = New StreamWriter(TempFile)
            readReg()
            'left, right, top, and bottom as read from Registry
            Dim margins As New Margins(m_Left, m_Right, m_Top, m_Bottom)

            'comboPrinter.Items.Insert(0, DefaultPrinterName())
            'comboPrinter.SelectedIndex = 0
            'try to find the right printer
            ' Add list of installed printers found to the combo box.
            ' The pkInstalledPrinters string will be used to provide the display string.
            Dim i As Integer
            Dim pkSize As New PaperSize
            Dim pkInstalledPrinters As String
            Dim foundIt As Boolean = False
            Debug.WriteLine("Listing installed Printers: ")
            For i = 0 To PrinterSettings.InstalledPrinters.Count - 1
                comboPrinter.Items.Insert(i, PrinterSettings.InstalledPrinters.Item(i))
                pkInstalledPrinters = PrinterSettings.InstalledPrinters.Item(i)
                Debug.WriteLine(vbTab & pkInstalledPrinters)
                If pkInstalledPrinters = m_PrinterName Then
                    'Apply the OKI printer to PrintDoc
                    PrintDocument1.PrinterSettings.PrinterName = m_PrinterName
                    Debug.WriteLine("Found and assigned '" & m_PrinterName & "'")
                    comboPrinter.SelectedIndex = i
                    foundIt = True
                    'Exit For
                End If
            Next
            If Not foundIt Then Debug.WriteLine("WARNING: '" & m_PrinterName & "' not found")

            If m_bUsePrinterSettingsFromReg = 1 Then
                'comboPapier.Items.Insert(0, DefaultPageSize())
                'comboPapier.SelectedIndex = 0
                'PrintDocument1.DefaultPageSettings.PaperSize.RawKind = PaperKind.A4 ' GermanStandardFanfold
                ''Dim Papier As myPaperSize
                foundIt = False
                Debug.WriteLine("PrinterSettings-PaperSizes: ")
                For i = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    'Papier = CType(PrintDocument1.PrinterSettings.PaperSizes.Item(i), myPaperSize)
                    'comboPapier.Items.Insert(i, Papier)
                    comboPapier.Items.Insert(i, PrintDocument1.PrinterSettings.PaperSizes.Item(i)) '.PaperName)
                    pkSize = PrintDocument1.PrinterSettings.PaperSizes.Item(i)
                    Debug.WriteLine(vbTab & "PaperSize: " & i & " = " & pkSize.PaperName & " RawKind=" & pkSize.RawKind)
                    If pkSize.RawKind = m_iPaperKind Then
                        comboPapier.SelectedIndex = i
                        PrintDocument1.DefaultPageSettings.PaperSize = PrintDocument1.PrinterSettings.PaperSizes.Item(i)
                        PrintDocument1.DefaultPageSettings.PaperSize.RawKind = PaperKind.A4 ' GermanStandardFanfold
                        'Debug.WriteLine("Found and assigned 'Endlospapier 8.5 x 12 Zoll'")
                        Debug.WriteLine("Found and assigned 'A4'")
                        foundIt = True
                        'Exit For
                    End If
                Next
                If Not foundIt Then Debug.WriteLine("WARNING: PaperSize not found")
            Else
                'lblPrinterList.Visible = False
                lblPaperList.Visible = False
                comboPapier.Visible = False
                'comboPrinter.Visible = False
            End If
            ' Create a new instance of Margins with one inch margins.
            ' left, right, top, bottom
            'pSize.Height = m_PaperHeight 'is already in 1/100 Inch
            'pSize.Width = m_PaperWidth
            'pSize.PaperName = "Endlospapier 8.5 x 12 Zoll"

            'Not used by printing on Epson LQ 24pin printer with Endlos 12" paper!
            'PrintDocument1.DefaultPageSettings.PaperSize = pSize ' pkSize

            Debug.WriteLine("Current Printer for Document is '" + PrintDocument1.PrinterSettings.PrinterName + "'")
            Debug.WriteLine("Current PaperSize for Document is '" + PrintDocument1.DefaultPageSettings.PaperSize.PaperName)
            Try
                PrintDocument1.PrinterSettings.PrinterName = m_PrinterName
                Debug.WriteLine("NEW Printer for Document is '" + PrintDocument1.PrinterSettings.PrinterName + "'")
                Debug.WriteLine("NEW PaperSize for Document is '" + PrintDocument1.DefaultPageSettings.PaperSize.PaperName + "'")
            Catch ex As Exception
                Debug.WriteLine("Exception in changing printer: '" + ex.Message)
            End Try

            PrintDocument1.DefaultPageSettings.Margins = margins
            m_CharsPerLine = GetCharsPerLine()
            m_LinesPerPage = GetLinesPerPage()

            Debug.Print("Margins: " + PrintDocument1.DefaultPageSettings.Margins.ToString)
            Debug.Print("PrintableArea: " + PrintDocument1.DefaultPageSettings.PrintableArea.ToString)
            Debug.Print("Bounds: " + PrintDocument1.DefaultPageSettings.Bounds.ToString())
            Debug.Print("LinesPerPage: " + m_LinesPerPage.ToString())
            Debug.Print("CharsPerLine: " + m_CharsPerLine.ToString())

            PrintPreviewControl2.UseAntiAlias = False
            PrintPreviewControl2.Zoom = 1
            PrintPreviewControl2.StartPage = 0
            'TempFiles.AddFile(TempFile, False)
            'TEST
            'AuftragsNummern.Add("1005647")
            PrintForm(AuftragsNummern)
            currentPreviewPage = 1
            FormSettingsLoad(Me)
        Catch ex As Exception
            MessageBox.Show("Exception in PrintAuftrag_Load: " + ex.Message, "Ausnahmefehler")
        Finally
            Me.ResumeLayout()
        End Try
    End Sub
    Private Function GetCharsPerLine() As Integer
        Try
            Dim g As Graphics
            Dim u = printFont.Unit 'points?
            g = PrintDocument1.PrinterSettings.CreateMeasurementGraphics(PrintDocument1.DefaultPageSettings, True)
            g.PageUnit = u
            Dim cpi As SizeF = g.MeasureString("W", printFont) ', 0, StringFormat.GenericTypographic)
            'Dim w = PrintDocument1.DefaultPageSettings.PrintableArea.Width
            Dim w = PrintDocument1.DefaultPageSettings.Bounds.Width - _
                    PrintDocument1.DefaultPageSettings.Margins.Left - _
                    PrintDocument1.DefaultPageSettings.Margins.Right
            GetCharsPerLine = CInt(w \ cpi.Width)
            If GetCharsPerLine < 80 Then
                Debug.Print("Warning: Forcing CharsPerLine to 80")
                GetCharsPerLine = 80
            End If
            g.Dispose()
        Catch ex As Exception
            MessageBox.Show("Exception in GetCharsPerLine: " + ex.Message, "Ausnahmefehler")
        End Try
    End Function
    Private Function GetLinesPerPage() As Integer
        'Dim g As Graphics
        'Dim u = printFont.Unit 'points?
        'g = PrintDocument1.PrinterSettings.CreateMeasurementGraphics(True)
        'g.PageUnit = u
        Dim lpp As Integer = printFont.Height ' GetHeight(g)

        Dim h = PrintDocument1.DefaultPageSettings.Bounds.Height - _
                PrintDocument1.DefaultPageSettings.Margins.Top - _
                PrintDocument1.DefaultPageSettings.Margins.Bottom
        GetLinesPerPage = CInt(h \ lpp)
        'g.Dispose()
    End Function
    Private Function printKopf() As Integer
        Dim lCount As Integer = 0
        For Each s As Object In KopfZeilen
            FStream.WriteLine(s.ToString)
            lCount += 1
        Next
        lCount += 1
        Return lCount
    End Function
    Private Sub printFuss()
        For Each s As Object In FussZeilen
            FStream.WriteLine(s.ToString)
        Next
    End Sub
    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim yPos As Single = 0
        Dim count As Integer = 0

        Dim leftMargin As Single = e.MarginBounds.Left
        Dim topMargin As Single = e.MarginBounds.Top
        Dim line As String = Nothing
        Try
            ' Calculate the number of lines per page.
            'linesPerPage = e.MarginBounds.Height / printFont.GetHeight(e.Graphics)
            e.Graphics.PageUnit = printFont.unit 'GraphicsUnit.Display
            Dim lpi As Single = e.MarginBounds.Height / printFont.Height 'getHeight(e.Graphics)

            ' Print each line of the file.
            While count < m_LinesPerPage
                line = streamToPrint.ReadLine()
                If line Is Nothing Then
                    Exit While
                End If
                If line = Chr(12) Then
                    Debug.Print("Found Chr(12), adding page. LineCount is=" & count & vbCrLf & _
                        "Lpi=" & lpi & vbCrLf & "m_LinesPerPage=" & m_LinesPerPage)
                    e.HasMorePages = True
                    totalPrintedPages += 1
                    Debug.Print("totalPrintedPages=" & totalPrintedPages)
                    Return
                End If
                yPos = topMargin + count * printFont.GetHeight(e.Graphics)
                If line.StartsWith("^B") Then
                    line = line.Substring(2)
                    e.Graphics.DrawString(line, printFontBold, Brushes.Black, leftMargin, yPos, New StringFormat())
                ElseIf line.StartsWith("^E") Then
                    line = line.Substring(2)
                    e.Graphics.DrawString(line, printFontBigBold, Brushes.Black, leftMargin, yPos, New StringFormat())
                Else
                    e.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, yPos, New StringFormat())
                End If
                count += 1
            End While

            ' If more lines exist, print another page.
            If Not (line Is Nothing) Then
                Debug.Print("Found more lines to print, adding page. LineCount is=" & count & vbCrLf & _
                        "Lpi=" & lpi & vbCrLf & "m_LinesPerPage=" & m_LinesPerPage)
                totalPrintedPages += 1
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
            'TotalPages += 1
        Catch ex As Exception
            MessageBox.Show("Exception in PrintPage: " + ex.Message, "Ausnahmefehler")
        End Try
    End Sub
    'Private Function printLine(ByVal txt As String, ByVal e As System.Drawing.Printing.PrintPageEventArgs) As Single
    '    Static Dim yPos As Single = 0
    '    Static Dim count As Integer = 0
    '    yPos = e.MarginBounds.Top + count * printFont.GetHeight(e.Graphics)
    '    e.Graphics.DrawString(txt, printFont, Brushes.Black, e.MarginBounds.Left, yPos, New StringFormat())
    '    count += 1
    '    Return yPos
    'End Function
    'Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    PrintDocument1.Print()
    'End Sub

    Private Sub btnDrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDrucken.Click
        With PrintDialog1
            .Document = PrintDocument1
            .Document.DocumentName = DruckTyp + "-" + currentAuftrNr
            .AllowCurrentPage = False
            .AllowSelection = False
            .AllowSomePages = False
            .ShowHelp = False
        End With
        PrintDialog1.Document.Print()
        'If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '    PrintDialog1.Document.Print()
        'End If
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Try
            FStream.Close()
            streamToPrint = New StreamReader(TempFile)
            totalPages = 1
            '"D:\KFZEURO\kbfakt\kbfakt5\MyFile.txt", System.Text.Encoding.GetEncoding(1250))
            'InitializePrintPreviewControl()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub PrintDocument1_EndPrint(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.EndPrint
        reader.Close()
        connection.Close()
        streamToPrint.Close()
        lblPageNumber.Text = currentPreviewPage & "/" & totalPrintedPages
        'totalPages = totalPrintedPages
        If CInt(totalPrintedPages) > 1 Then
            btnPagePlus.Enabled = True
            btnPageMinus.Enabled = True
        Else
            btnPagePlus.Enabled = False
            btnPageMinus.Enabled = False
        End If

    End Sub
    Private Function FillFussZeilen() As Integer
        Dim queryString As String = ""
        Dim txtFussZeile As String
        Dim i As Integer
        Dim s() As String
        Dim l As Integer
        Dim iCount As Integer = 0
        FussZeilen.Clear()
        queryString = "Select FussText from FirmStam"
        Dim command As New OleDbCommand(queryString, connection)
        Try
            OpenDBConnection(connection)
            'connection.Open() is already opened with OpenDBConnection
            reader = command.ExecuteReader()
            reader.Read()
            If reader.HasRows Then
                txtFussZeile = reader("FussText").ToString
                s = Split(txtFussZeile, vbCrLf, 3)
                FussZeilen.Add("-".PadRight(m_CharsPerLine, "-"))
                For i = 0 To s.Length - 1
                    If s(i).Length > 80 Then s(i) = s(i).Substring(0, 80)
                    s(i) = s(i).Trim(" ")
                    l = s(i).Length
                    'Zentrieren: charsperline=80, l=20 => PadLeft(40-10)
                    FussZeilen.Add(" ".PadLeft((m_CharsPerLine \ 2) - (l \ 2)) + s(i))
                Next i
            End If
        Catch ex As Exception
            MessageBox.Show("Exception in FillFussZeilen:" + vbCrLf + ex.Message)
        End Try
        reader.Close()
        connection.Close()
        iCount = FussZeilen.Count + 1
        Return iCount
    End Function
    Private Sub FillKopfzeilen(ByVal AuftragsNummer As String)
        'PrintPageHeader
        Dim queryString As String
        Dim txtXAN1 As String = ""
        Dim txtXKundenNr As String = ""
        Dim txtXName1 As String = ""
        Dim txtXName2 As String = ""
        Dim txtXST As String = ""
        Dim txtXPL As String = ""
        Dim txtXOT As String = ""
        Dim txtXTel As String = ""
        Dim txtDatum As String = ""
        Dim txtLDatum As String = "" 'new with v2.0.3.0 'Leistungs-/Liefer-Datum
        Dim txtWerkdatum As String = ""
        Dim txtXSCHMIER As String = ""
        Dim txtXLOHN As String = ""
        Dim txtXSONDER As String = ""
        Dim txtXZULASS As String = ""
        Dim txtXTYP As String = ""
        Dim txtNetto As String = ""
        Dim txtFGSTLLNR As String = ""
        Dim txtXKZ As String = ""
        Dim txtXKMSTAND As String = ""
        Dim STEUERNR As String = "" : Dim FUSSTEXT As String = ""
        Dim LeftOffset As Integer = m_CharsPerLine - 12 'for alignment at left column
        Dim KFZ As New Fahrzeug
        KopfZeilen.Clear()
        queryString = "Select * from Rech1 where XAUFTR_Nr=" & AuftragsNummer
        Try
            OpenDBConnection(connection)
            Dim anredeNr As Double
            Dim command As New OleDbCommand(queryString, connection)
            'connection.Open() is already opened with OpenDBConnection
            reader = command.ExecuteReader()
            reader.Read()
            If reader.HasRows Then
                'Anrede-ID durch Text ersetzen
                anredeNr = CDbl(reader("XAn1").ToString)
                txtXAN1 = mainmodul.GetAnrede(anredeNr)
                txtXKundenNr = reader("XKUNDENNR").ToString
                txtXName1 = reader("XNAME1").ToString
                txtXName2 = reader("XNAME2").ToString
                txtXST = reader("XST").ToString
                txtXPL = reader("XPL").ToString
                txtXOT = reader("XOT").ToString
                txtXTel = reader("XTEL").ToString

                txtDatum = Format(reader("XDATUM"), "dd.MM.yyyy")
                txtLDatum = Format(reader("LDATUM"), "dd.MM.yyyy") 'new with v2.0.3.0 'Leistungs-/Liefer-Datum

                If Not IsDBNull(reader("werkdatum")) Then
                    txtWerkdatum = Format(reader("werkdatum"), "dd.MM.yyyy")
                Else
                    txtWerkdatum = "  .  .    "
                End If

                txtXSCHMIER = reader("XSCHMIER").ToString
                txtXLOHN = reader("XLOHN").ToString
                txtXSONDER = reader("XSONDER").ToString

                If Not IsDBNull(reader("XZULASS")) Then
                    txtXZULASS = Format(reader("XZULASS"), "dd.MM.yyyy")
                Else
                    txtXZULASS = "  .  .    "
                End If
                If txtXZULASS = "01.01.1901" Then txtXZULASS = ""

                txtXTYP = reader("XTYP").ToString
                txtXKMSTAND = reader("XKMSTAND").ToString

                'txtMwStSatz.Text = reader("XMWS").ToString 'we dont read this, we calc this
                'BUT we need the actual MwSt-Satz!
                txtMwStSatz = reader("MWSTsatz").ToString

                txtAltTeilMwStSatz = reader("AltTeilMWST").ToString

                txtNetto = reader("XNETTO").ToString 'only for testing!

                txtFGSTLLNR = reader("XFGSTLLNR").ToString
                KFZ = readFahrzeug(txtFGSTLLNR)

                txtXKZ = reader("XKZ").ToString
            End If
            reader.Close()
            command.CommandText = "Select STEUERNR, FUSSTEXT from FirmStam"
            reader = command.ExecuteReader
            If reader.HasRows() Then
                reader.Read()
                STEUERNR = reader("STEUERNR").ToString
                FUSSTEXT = reader("FUSSTEXT").ToString
            End If
        Catch ex As SystemException
            Debug.Print("Exception in PrintPage" + ex.Message)
        End Try
        reader.Close()

        'print the PageHeader fields
        KopfZeilen.Add(txtXAN1)
        'FStream.WriteLine(txtXAN1)
        KopfZeilen.Add(txtXName1)
        KopfZeilen.Add(txtXName2)
        KopfZeilen.Add(txtXST)
        SkipLines(1)
        KopfZeilen.Add(txtXPL + " " + txtXOT)
        SkipLines(4)
        KopfZeilen.Add("^E" + DruckTyp + " " + (AuftragsNummer) + " vom " + txtWerkdatum) 'exchanged txtDatum with txtWerkdatum
        KopfZeilen.Add(("Steuernummer:").PadLeft(LeftOffset) + (STEUERNR).PadLeft(12))
        'KopfZeilen.Add("^B" + ("Auftragsnr.:").PadLeft(LeftOffset) + (AuftragsNummer).PadLeft(12))
        KopfZeilen.Add(("Auftragsdatum:").PadLeft(LeftOffset) + (txtDatum).PadLeft(12)) 'exchanged txtDatum with txtWerkdatum
        'KopfZeilen.Add(("Ann. Datum:").PadLeft(LeftOffset) + (txtDatum).PadLeft(12))

        'Kundendaten
        'version 2.1.1.0
        If (DruckTyp = "Rechnung" Or DruckTyp = "Lieferschein") Then
            KopfZeilen.Add(("Kunden-Nr.:").PadLeft(15) + _
                txtXKundenNr.PadLeft(28 - 15) + _
                " ".PadRight(25) + _
                "Leistungsdatum:" + _
                (txtLDatum).PadLeft(12)) 'new with v2.0.3.0 'Leistungs-/Liefer-Datum
        Else
            KopfZeilen.Add(("Kunden-Nr.:").PadLeft(15) + _
                txtXKundenNr.PadLeft(28 - 15))
        End If

        KopfZeilen.Add("Telefon:".PadLeft(15) + _
            txtXTel.PadLeft(28 - 15) + _
            " ".PadRight(19) + _
            "Fahrgestellnr.:" + _
            txtFGSTLLNR.PadLeft(18))

        KopfZeilen.Add("Amtl.Kennz.:".PadLeft(15) + _
            txtXKZ.PadLeft(28 - 15) + _
            " ".PadRight(19) + _
            "Typ:".PadLeft(15) + _
            txtXTYP.PadLeft(18))

        KopfZeilen.Add("Zul. Datum:".PadLeft(15) + _
            txtXZULASS.PadLeft(28 - 15) + _
            " ".PadRight(19) + _
            "KM-Stand:".PadLeft(15) + _
            txtXKMSTAND.PadLeft(18))

        KopfZeilen.Add("T�V:".PadLeft(15) + KFZ.TUEV.PadLeft(6) + _
            ", ASU:" + KFZ.ASU.PadLeft(6) + _
            ", Tacho:" + KFZ.Schreiber.PadLeft(6) + _
            ", Sicherh.:" + KFZ.Sicher.PadLeft(6))
        'KopfZeilen.Add("KM-Stand:".PadLeft(15) + txtXKMSTAND.PadLeft(28 - 15))

        connection.Close()

    End Sub
    Private Sub SkipLines(ByVal nOfLines As Integer)
        Dim i As Integer
        For i = 1 To nOfLines
            KopfZeilen.Add("")
        Next
    End Sub
    Private Sub PrintForm(ByVal AuftragsNummern As Collection)
        Static Dim PageCurrent = 1
        Dim queryString As String
        Dim i As Integer
        Dim POS As Integer = 0
        Dim ARTIKELNR As String = ""
        Dim ARTIKELBEZ As String = ""
        Dim ARTIKELBEZ2 As String = ""
        Dim MENGE As Double = 0
        Dim E_PREIS As Double = 0
        Dim RABATT As Double = 0
        Dim G_Preis As Double = 0
        Dim PosCount As Integer = 0
        Dim LineCount As Integer = 1
        Dim ZwischenSumme As Double = 0
        Dim SchmiermittelSumme As Double = 0
        'Dim ErsatzteileSumme As Double = 0
        Dim ErsatzteileSumme As Double = 0
        Dim AltTeilSumme As Double = 0 'new with version 2.1.0.0
        Dim LohnSumme As Double = 0

        Dim AnzKopfzeilen As Integer = 0
        ' a header needs 22 lines
        ' detailsheader needs 5 lines
        ' a subtotal needs 2 lines
        ' the total needs 5 lines
        ' the footer needs 4 lines
        ' a detail line may need 1 or two lines
        Dim DetailsLineCount = 0
        Dim lCountFooter As Integer = 4
        Dim lCountTotal As Integer = 5 'number of lines needed to print Totals
        Dim pCount
        Dim detailsPerPage As Integer
        Dim lCountDetailsHeader As Integer = 6
        Dim AnzAuftr�ge As Integer = AuftragsNummern.Count
        Dim AuftragID As Integer
        Dim AuftragsNummer As Object
        lCountFooter = FillFussZeilen()
        For AuftragID = 1 To AuftragsNummern.Count
            AuftragsNummer = AuftragsNummern.Item(AuftragID)
            currentAuftrNr = AuftragsNummer
            FillKopfzeilen(AuftragsNummer.ToString)
            ZwischenSumme = 0 : SchmiermittelSumme = 0 : ErsatzteileSumme = 0 : AltTeilSumme = 0
            LineCount = 0
            PageCurrent = 1
            pCount = 0
            Try
                OpenDBConnection(connection)
                'read rech2 entries
                queryString = "select pos, ARTIKELNR, ARTIKELBEZ, MENGE, E_PREIS, RABATT, menge * e_preis - (RABATT/100 * (menge * e_preis)) as G_Preis, arttyp, id from rech2 where AUFTR_NR=" + AuftragsNummer.ToString + " order by pos"
                oleda = New OleDbDataAdapter(queryString, connection)
            Catch ex As OleDb.OleDbException
                Debug.Print("Exception in reading oleda: " + ex.Message)
                If Not mainmodul.DebugModus Then MessageBox.Show("Exception in PrintForm(): " + ex.Message)
            End Try
            numberOfItems = oleda.Fill(dbset)
            DetailsLineCount = GetDetailsLineCount(AuftragsNummer.ToString)
            'calc total pages
            AnzKopfzeilen = printKopf()
            detailsPerPage = m_LinesPerPage - AnzKopfzeilen - lCountDetailsHeader - lCountTotal - lCountFooter
            pCount = DetailsLineCount \ detailsPerPage
            If DetailsLineCount Mod detailsPerPage > 0 Then pCount += 1
            'pCount = (AnzKopfzeilen + lCountDetailsHeader + DetailsLineCount + lCountTotal + lCountFooter) / m_LinesPerPage
            'If (AnzKopfzeilen + lCountDetailsHeader + DetailsLineCount + lCountTotal + lCountFooter) Mod m_LinesPerPage > 0 Then
            '    pCount += 1
            'End If
            currentPage = 1
            totalPages = CInt(pCount)

            Dim iTemp As Integer = 0
            iTemp = WriteDetailsHeader(PageCurrent, totalPages, ZwischenSumme)
            lCountDetailsHeader = iTemp
            LineCount = AnzKopfzeilen + lCountDetailsHeader
            System.Diagnostics.Debug.WriteLine("############################")
            System.Diagnostics.Debug.WriteLine("CurrentPage=" & currentPage & vbCrLf & _
                                                "AnzKopfzeilen=" & AnzKopfzeilen & vbCrLf & _
                                                "DetailsLineCount=" & AnzKopfzeilen & vbCrLf & _
                                                "detailsPerPage=" & AnzKopfzeilen & vbCrLf & _
                                                "lCountDetailsHeader=" & lCountDetailsHeader & vbCrLf & _
                                                "lCountTotal=" & lCountTotal & vbCrLf & _
                                                "lCountFooter=" & lCountFooter & vbCrLf & _
                                                "m_LinesPerPage=" & m_LinesPerPage & vbCrLf & _
                                                "pCount=" & pCount)

            For i = 0 To numberOfItems - 1
                PosCount += 1
                If (LineCount + lCountTotal + lCountFooter) > m_LinesPerPage Then
                    'write Zwischensumme
                    FStream.WriteLine("-".PadRight(m_CharsPerLine, "-"))
                    If PreiseAnzeigen Then FStream.WriteLine(("Zwischensumme: " + Format(ZwischenSumme, "#.00")).PadLeft(m_CharsPerLine))

                    System.Diagnostics.Debug.WriteLine("Zwischensumme=" + Format(ZwischenSumme, "#.00"))
                    System.Diagnostics.Debug.WriteLine("Altteilsumme=" + Format(AltTeilSumme, "#.00"))

                    FStream.WriteLine(Chr(12))
                    PageCurrent += 1
                    LineCount = printKopf() + WriteDetailsHeader(PageCurrent, totalPages, ZwischenSumme)
                End If
                With dbset.Tables(0).Rows(i)
                    If .Item("Pos") <> 0 Then 'avoid empty or zero entries
                        POS = .Item("Pos")
                    Else
                        POS = PosCount
                    End If
                    ARTIKELNR = .Item("ARTIKELNR")
                    If ARTIKELNR = "ZEIT" Then ARTIKELNR = "AW"
                    'If ARTIKELNR = .Item("ARTIKELNR") Then 'avoid duplicate numbers
                    '    ARTIKELNR = ""
                    'Else
                    '    ARTIKELNR = .Item("ARTIKELNR")
                    'End If
                    ARTIKELBEZ = .Item("ARTIKELBEZ")
                    If ARTIKELBEZ.Length > 30 Then
                        'new in 2.0.0.8
                        Dim s1 As String
                        s1 = mainmodul.WordWrap(ARTIKELBEZ, 30)
                        Dim p As Integer = s1.LastIndexOf(vbCr)
                        If p > 0 Then
                            ARTIKELBEZ2 = LTrim(ARTIKELBEZ.Substring(p))
                            ARTIKELBEZ = ARTIKELBEZ.Substring(0, p)

                        Else
                            'changed both to 30 in 2.0.0.8
                            ARTIKELBEZ2 = ARTIKELBEZ.Substring(30)
                            ARTIKELBEZ = ARTIKELBEZ.Substring(0, 30)
                        End If
                    Else
                        ARTIKELBEZ2 = ""
                    End If
                    MENGE = CDouble(.Item("Menge"))
                    E_PREIS = CDouble(.Item("E_PREIS"))
                    RABATT = CDouble(.Item("RABATT"))
                    G_Preis = CDouble(.Item("G_PREIS"))
                    ZwischenSumme += G_Preis 'bei Netto egal ob AltTeil

                    System.Diagnostics.Debug.WriteLine("Zwischensumme=" + Format(ZwischenSumme, "#.00"))

                    If CInt(.Item("arttyp")) = 0 Then ErsatzteileSumme += G_Preis 'change in 2.0.0.3
                    If CInt(.Item("arttyp")) = 1 Then SchmiermittelSumme += G_Preis
                    If CInt(.Item("arttyp")) = 2 Then LohnSumme += G_Preis 'change in 2.0.1.3

                    If CInt(.Item("arttyp")) = 3 Then
                        AltTeilSumme += G_Preis 'change in 2.1.0.0
                        System.Diagnostics.Debug.WriteLine("Altteilsumme=" + Format(AltTeilSumme, "#.00"))
                    End If

                    If m_CharsPerLine >= 80 Then _
                        FStream.Write(Format(POS, "###").PadLeft(3) + " ")
                    FStream.Write(ARTIKELNR.PadRight(17) + " ")
                    FStream.Write(ARTIKELBEZ.PadRight(30) + " ")
                    FStream.Write(Format(MENGE, "0.00").PadLeft(6) + " ")
                    If PreiseAnzeigen Then
                        FStream.Write(Format(E_PREIS, "0.00").PadLeft(7) + " ")
                        FStream.Write(Format(RABATT, "##").PadLeft(3) + " ")
                        FStream.Write(Format(G_Preis, "0.00").PadLeft(8))
                    End If
                    FStream.WriteLine()
                    LineCount += 1
                    If ARTIKELBEZ2 <> "" Then
                        FStream.WriteLine(Space(22) + ARTIKELBEZ2)
                        LineCount += 1
                    End If
                End With
            Next i
            'Dim MwSt As Double = (ZwischenSumme - AltTeilSumme) / 100 * CDouble(txtMwStSatz) 'version 2.1.0.0
            'change with version 2.1.2.0
            Dim MwSt As Double = ZwischenSumme / 100 * CDouble(txtMwStSatz) 'version 2.1.0.0
            Dim AltTeilMwSt As Double = AltTeilSumme / 100 * CDouble(txtAltTeilMwStSatz) 'version 2.1.0.0
            Dim Brutto As Double = ZwischenSumme + MwSt + AltTeilMwSt
            'Gesamt-Ausdruck (5 Zeilen)
            FStream.WriteLine("=".PadRight(m_CharsPerLine, "="))
            LineCount += 1
            If PreiseAnzeigen Then
                'Schmiermittel-Anteil
                If (SchmiermittelSumme > 0) Then 'changed in 2.0.1.6
                    FStream.Write("Schmiermittel: ".PadLeft(19) + Format(SchmiermittelSumme, "0.00").PadLeft(8))
                Else
                    FStream.Write("Schmiermittel: ".PadLeft(19) + Format("0,00").PadLeft(8))
                End If
                'Netto-Summe
                FStream.WriteLine(Format(ZwischenSumme, "0.00").PadLeft(m_CharsPerLine - 27))
                LineCount += 1

                'Ersatzteile
                If (ErsatzteileSumme > 0) Then
                    FStream.Write("Ersatzteile: ".PadLeft(19) + Format(ErsatzteileSumme, "0.00").PadLeft(8))
                Else
                    FStream.Write("Ersatzteile: ".PadLeft(19) + Format("0,00").PadLeft(8))
                End If

                'MwSt Betrag
                FStream.WriteLine((txtMwStSatz + "% MwSt " + _
                                    Format(MwSt, "0.00").PadLeft(9)).PadLeft(m_CharsPerLine - 27))
                LineCount += 1
                'AltTeile
                FStream.Write("Altteile: ".PadLeft(19) + Format(AltTeilSumme, "0.00").PadLeft(8))
                'AltTeilMwSt
                'FStream.WriteLine((txtAltTeilMwStSatz + "% AltTeil-MwSt " + Format(AltTeilMwSt, "0.00").PadLeft(9)).PadLeft(m_CharsPerLine - 27))
                'changed in version 2.1.2.0 to
                FStream.WriteLine(("AltTeil-MwSt " + Format(AltTeilMwSt, "0.00").PadLeft(9)).PadLeft(m_CharsPerLine - 27))

                LineCount += 1
                If (LohnSumme > 0) Then
                    FStream.Write("Lohn: ".PadLeft(19) + Format(LohnSumme, "0.00").PadLeft(8))
                Else
                    FStream.Write("Lohn: ".PadLeft(19) + Format("0,00").PadLeft(8))
                End If

                'Gesamtbetrag
                FStream.WriteLine(Format(Brutto, "0.00").PadLeft(m_CharsPerLine - 27))
                LineCount += 1
                FStream.WriteLine("=========".PadLeft(m_CharsPerLine))
                LineCount += 1
            End If
            'Fusstext! (4 Zeilen)
            'move down
            Do While LineCount < m_LinesPerPage - lCountFooter
                FStream.WriteLine()
                LineCount += 1
            Loop
            printFuss()
            If AuftragID < AuftragsNummern.Count Then FStream.WriteLine(Chr(12))
        Next AuftragID

        oleda.Dispose()
        connection.Close()
    End Sub
    Private Function CalcSchmiermittel() As Double

    End Function
    Private Function WriteDetailsHeader(ByVal PageCurrent As Integer, ByVal PagesTotal As Integer, ByVal zwischenSumme As Double) As Integer
        Dim lCount As Integer = 0
        FStream.WriteLine("-".PadRight(m_CharsPerLine, "-"))
        lCount += 1
        FStream.WriteLine(("Seite(n): " & PageCurrent & "/" & PagesTotal).PadLeft(m_CharsPerLine))
        lCount += 1
        If zwischenSumme <> 0 Then
            FStream.WriteLine(("Zwischensumme: " + Format(zwischenSumme, "#.00")).PadLeft(m_CharsPerLine))
        Else
            FStream.WriteLine()
        End If
        lCount += 1
        FStream.WriteLine("-".PadRight(m_CharsPerLine, "-"))
        lCount += 1
        If m_CharsPerLine >= 80 Then _
            FStream.Write("Pos".PadRight(3) + " ") ' 4
        FStream.Write("Artikelnr.".PadRight(17) + " ")          '18
        FStream.Write("Artikelbezeichnung".PadRight(30) + " ")  '31
        FStream.Write("Menge".PadRight(6) + " ")                ' 7
        FStream.Write("E-Preis".PadRight(7) + " ")              ' 8
        FStream.Write("Rab".PadRight(3) + " ")                  ' 4
        FStream.Write("G-Preis".PadRight(8))                    ' 9
        FStream.WriteLine()
        lCount += 1
        FStream.WriteLine("=".PadRight(m_CharsPerLine, "="))
        lCount += 1
        Return lCount
    End Function
    ''' <summary>
    ''' return the number of lines needed to print all details for AuftragNr
    ''' will care about entries with LEN less than 30 and more than 30
    ''' </summary>
    ''' <param name="auftragsnummer"></param>
    ''' <returns>number of lines needed to print with a width of 30</returns>
    ''' <remarks></remarks>
    Private Function GetDetailsLineCount(ByVal auftragsnummer As String) As Integer
        Dim queryString As String = "select count(*) from RECH2 where Auftr_Nr=" + auftragsnummer
        Dim DetailsLineCount As Integer = 0
        Dim DetailsLineCount2 As Integer = 0
        Try
            OpenDBConnection(connection)
            Dim command As New OleDbCommand(queryString, connection)
            'connection.Open() is already opened with OpenDBConnection
            DetailsLineCount = command.ExecuteScalar() 'this is all single lines
            queryString = "select count(*) from RECH2 where Auftr_Nr=" + _
            auftragsnummer + _
            " AND LEN(ARTIKELBEZ)>30"
            command.CommandText = queryString
            DetailsLineCount2 = command.ExecuteScalar() ' this is for all double lines 
        Catch ex As Exception
            MessageBox.Show("Exception in GetDetailsLineCount: " + vbCrLf + ex.Message)
        End Try
        connection.Close()
        GetDetailsLineCount = DetailsLineCount + DetailsLineCount2
    End Function

    Private Sub btnPagePlus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPagePlus.Click
        If currentPreviewPage < totalPrintedPages Then
            currentPreviewPage += 1
            PrintPreviewControl2.StartPage += 1
            lblPageNumber.Text = currentPreviewPage.ToString + "/" & totalPrintedPages
            PrintPreviewControl2.Refresh()
        End If
    End Sub

    Private Sub btnPageMinus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPageMinus.Click
        If currentPreviewPage > 1 Then
            currentPreviewPage -= 1
            If PrintPreviewControl2.StartPage > 0 Then
                PrintPreviewControl2.StartPage -= 1
            End If
            PrintPreviewControl2.Refresh()
            lblPageNumber.Text = currentPreviewPage.ToString + "/" & totalPrintedPages
        End If
    End Sub

    Private Sub btnPageSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPageSetup.Click
        PageSetupDialog1.AllowPrinter = False 'version 2.1.1.0

        If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then

            With PrintDocument1.DefaultPageSettings.Margins
                m_Bottom = .Bottom
                m_Left = .Left
                m_Top = .Top
                m_Right = .Right
            End With
            'not used, EPSON 24 pin printer supports 12" endless paper
            'PrintDocument1.DefaultPageSettings.PaperSize = pSize 'version 2.1.1.0
            m_CharsPerLine = GetCharsPerLine()
            m_LinesPerPage = GetLinesPerPage()
            FStream.Close()
            FStream = New StreamWriter(TempFile)
            PrintForm(AuftragsNummern)
            Debug.Print("Chars per Line=" + m_CharsPerLine.ToString)
            Debug.Print("Lines per page=" + m_LinesPerPage.ToString)
            PrintPreviewControl2.InvalidatePreview()
            'PrintPreviewControl2.Refresh()
        End If
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        WriteReg()
        FormSettingsSave(Me)
        streamToPrint.Close()
        'FStream.Close()
        'TempFiles.Delete()
        Me.Close()
    End Sub
    Private Structure Fahrzeug
        Public Schreiber As String
        Public TUEV As String
        Public ASU As String
        Public Sicher As String
    End Structure
    Private Function readFahrzeug(ByVal FahrzNr As String) As Fahrzeug
        Dim queryString As String
        Dim Anzahl As Integer = 0
        '"KUNDEN_NR, FGSTLLNR, TYP, KENNZEICH, KM_STAND, KFZ_BRF_NR, 
        'ZULASSUNG1, TUEV, ASU,  SCHREIBER,  SICHER"
        queryString = "Select * from KUNDFAHR where FGSTLLNR='" & FahrzNr & "'"
        Dim connection As New OleDbConnection
        For Each c As Control In Me.Controls
            If TypeOf c Is TextBox Then
                If c.Tag = """fahrzeug""" Then
                    c.BackColor = Color.White
                    c.Text = ""
                End If
            End If
        Next
        Dim txtSchreiber As String = ""
        Dim TUEV As String = ""
        Dim txtASU As String = ""
        Dim txtSicher As String = ""
        If FahrzNr <> "999999" Then
            Try
                OpenDBConnection(connection)
                Dim command As New OleDbCommand(queryString, connection)
                'connection.Open() is already opened with OpenDBConnection
                Dim reader As OleDbDataReader = command.ExecuteReader()
                reader.Read()
                If reader.HasRows Then
                    Anzahl += 1
                    'FGSTLLNR.Text = reader("FGSTLLNR").ToString
                    TUEV = reader("TUEV").ToString
                    txtSchreiber = reader("SCHREIBER").ToString
                    txtASU = reader("ASU").ToString
                    txtSicher = reader("Sicher").ToString

                    'Console.WriteLine(reader(0).ToString())
                End If
                reader.Close()
            Catch ex As OleDbException
                MessageBox.Show(ex.Message, "readFahrzeug(" & FahrzNr & ")")
            Finally
                connection.Close()
            End Try
        End If
        readFahrzeug.ASU = txtASU
        readFahrzeug.Schreiber = txtSchreiber
        readFahrzeug.Sicher = txtSicher
        readFahrzeug.TUEV = TUEV
    End Function

    Private Sub PrintPreviewControl2_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PrintPreviewControl2.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            PrintPreviewControl2.Zoom *= 1.25
        End If
        If e.Button = Windows.Forms.MouseButtons.Right Then
            PrintPreviewControl2.Zoom *= 0.25
        End If
    End Sub

    Private Sub PrintPreviewControl2_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintPreviewControl2.DoubleClick
        PrintPreviewControl2.Zoom = 1
    End Sub

    'Private Sub btnPrintRAW_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    With PrintDialog1
    '        .Document = PrintDocument1
    '        .Document.DocumentName = DruckTyp
    '        .AllowCurrentPage = False
    '        .AllowSelection = False
    '        .AllowSomePages = False
    '        .ShowHelp = False
    '    End With
    '    Dim count As Integer = 0
    '    Dim ESC As String = Chr(27)
    '    'FStream.Close()
    '    'Recalculate pages
    '    m_CharsPerLine = GetCharsPerLine()
    '    m_LinesPerPage = m_RawPageLength
    '    'm_TopOffsetLines
    '    FStream.Close()
    '    FStream = New StreamWriter(TempFile)
    '    PrintForm(AuftragsNummern)
    '    FStream.Close()

    '    PrintDialog1.PrinterSettings = New PrinterSettings()

    '    If (PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then
    '        'Convert the file to a raw file
    '        streamToPrint = New StreamReader(TempFile)
    '        Dim RawFile As StreamWriter = New StreamWriter(TempFile + ".raw", False, System.Text.Encoding.GetEncoding(437))
    '        ' oder CodePages 850, 852, 28591 oder System.Text.Encoding.ASCII (nur 7 Bit!))
    '        Dim line As String
    '        Dim i As Integer
    '        For i = 0 To m_TopOffsetLines
    '            RawFile.WriteLine()
    '        Next
    '        Do
    '            line = streamToPrint.ReadLine()
    '            If line Is Nothing Then Exit Do
    '            If line = Chr(12) Or (count + 1) > m_LinesPerPage Then
    '                If line = Chr(12) Then
    '                    count = 0
    '                    RawFile.Write(Chr(12))
    '                    For i = 0 To m_TopOffsetLines
    '                        RawFile.WriteLine()
    '                    Next
    '                Else
    '                    count = 0
    '                    RawFile.Write(Chr(12))
    '                End If
    '            End If
    '            If line.StartsWith("^B") Then
    '                line = ESC + "E" + line.Substring(2) + ESC + "F"
    '                RawFile.WriteLine(line)
    '            ElseIf line.StartsWith("^E") Then
    '                line = ESC + "W1" + line.Substring(2) + ESC + "W0"
    '                RawFile.WriteLine(line)
    '            Else
    '                RawFile.WriteLine(line)
    '            End If
    '            count += 1
    '        Loop Until line Is Nothing

    '        'RawFile.Write(Chr(12))
    '        RawFile.Close()
    '        streamToPrint.Close()
    '        RawPrinterHelper.SendFileToPrinter(PrintDialog1.PrinterSettings.PrinterName, TempFile + ".raw")
    '    End If

    'End Sub

    'Private Sub btnRAWSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRAWSettings.Click
    '    Dim dlg As New PrintAuftragRawSetup
    '    dlg.m_TopOffset = m_TopOffsetLines
    '    dlg.m_LinesPerPage = m_LinesPerPage
    '    If (dlg.ShowDialog() = Windows.Forms.DialogResult.OK) Then
    '        m_RawPageLength = dlg.m_LinesPerPage
    '        m_TopOffsetLines = dlg.m_TopOffset
    '    End If
    '    dlg.Dispose()
    'End Sub

    Private Sub comboPrinter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboPrinter.SelectedIndexChanged
        m_PrinterName = comboPrinter.SelectedItem
        PrintDocument1.PrinterSettings.PrinterName = m_PrinterName
        PrintPreviewControl2.InvalidatePreview()
    End Sub

    Private Sub comboPapier_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboPapier.SelectedIndexChanged
        'Dim paper As PaperSize
        m_iPaperKind = comboPapier.Items(comboPapier.SelectedIndex).RawKind
    End Sub
End Class
'Public Class RawPrinterHelper
'    ' Structure and API declarions:
'    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
'    Structure DOCINFOW
'        <MarshalAs(UnmanagedType.LPWStr)> Public pDocName As String
'        <MarshalAs(UnmanagedType.LPWStr)> Public pOutputFile As String
'        <MarshalAs(UnmanagedType.LPWStr)> Public pDataType As String
'    End Structure

'    <DllImport("winspool.Drv", EntryPoint:="OpenPrinterW", _
'       SetLastError:=True, CharSet:=CharSet.Unicode, _
'       ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
'    Public Shared Function OpenPrinter(ByVal src As String, ByRef hPrinter As IntPtr, ByVal pd As IntPtr) As Boolean 'as long
'    End Function
'    <DllImport("winspool.Drv", EntryPoint:="ClosePrinter", _
'       SetLastError:=True, CharSet:=CharSet.Unicode, _
'       ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
'    Public Shared Function ClosePrinter(ByVal hPrinter As IntPtr) As Boolean
'    End Function
'    <DllImport("winspool.Drv", EntryPoint:="StartDocPrinterW", _
'       SetLastError:=True, CharSet:=CharSet.Unicode, _
'       ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
'    Public Shared Function StartDocPrinter(ByVal hPrinter As IntPtr, ByVal level As Int32, ByRef pDI As DOCINFOW) As Boolean
'    End Function
'    <DllImport("winspool.Drv", EntryPoint:="EndDocPrinter", _
'       SetLastError:=True, CharSet:=CharSet.Unicode, _
'       ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
'    Public Shared Function EndDocPrinter(ByVal hPrinter As IntPtr) As Boolean
'    End Function
'    <DllImport("winspool.Drv", EntryPoint:="StartPagePrinter", _
'       SetLastError:=True, CharSet:=CharSet.Unicode, _
'       ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
'    Public Shared Function StartPagePrinter(ByVal hPrinter As IntPtr) As Boolean
'    End Function
'    <DllImport("winspool.Drv", EntryPoint:="EndPagePrinter", _
'       SetLastError:=True, CharSet:=CharSet.Unicode, _
'       ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
'    Public Shared Function EndPagePrinter(ByVal hPrinter As IntPtr) As Boolean
'    End Function
'    <DllImport("winspool.Drv", EntryPoint:="WritePrinter", _
'       SetLastError:=True, CharSet:=CharSet.Unicode, _
'       ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
'    Public Shared Function WritePrinter(ByVal hPrinter As IntPtr, ByVal pBytes As IntPtr, ByVal dwCount As Int32, ByRef dwWritten As Int32) As Boolean
'    End Function

'    ' SendBytesToPrinter()
'    ' When the function is given a printer name and an unmanaged array of  
'    ' bytes, the function sends those bytes to the print queue.
'    ' Returns True on success or False on failure.
'    Public Shared Function SendBytesToPrinter(ByVal szPrinterName As String, ByVal pBytes As IntPtr, ByVal dwCount As Int32) As Boolean
'        Dim hPrinter As IntPtr      ' The printer handle.
'        Dim dwError As Int32        ' Last error - in case there was trouble.
'        Dim di As DOCINFOW          ' Describes your document (name, port, data type).
'        Dim dwWritten As Int32      ' The number of bytes written by WritePrinter().
'        Dim bSuccess As Boolean     ' Your success code.

'        ' Set up the DOCINFO structure.
'        With di
'            .pDocName = "My Visual Basic .NET RAW Document"
'            .pDataType = "RAW"
'        End With
'        ' Assume failure unless you specifically succeed.
'        bSuccess = False
'        di = Nothing
'        If OpenPrinter(szPrinterName, hPrinter, Nothing) Then
'            If StartDocPrinter(hPrinter, 1, di) Then
'                If StartPagePrinter(hPrinter) Then
'                    ' Write your printer-specific bytes to the printer.
'                    bSuccess = WritePrinter(hPrinter, pBytes, dwCount, dwWritten)
'                    EndPagePrinter(hPrinter)
'                End If
'                EndDocPrinter(hPrinter)
'            End If
'            ClosePrinter(hPrinter)
'        End If
'        ' If you did not succeed, GetLastError may give more information
'        ' about why not.
'        If bSuccess = False Then
'            dwError = Marshal.GetLastWin32Error()
'        End If
'        Return bSuccess
'    End Function ' SendBytesToPrinter()

'    ' SendFileToPrinter()
'    ' When the function is given a file name and a printer name, 
'    ' the function reads the contents of the file and sends the
'    ' contents to the printer.
'    ' Presumes that the file contains printer-ready data.
'    ' Shows how to use the SendBytesToPrinter function.
'    ' Returns True on success or False on failure.
'    Public Shared Function SendFileToPrinter(ByVal szPrinterName As String, ByVal szFileName As String) As Boolean
'        ' Open the file.
'        Dim fs As New FileStream(szFileName, FileMode.Open)
'        ' Create a BinaryReader on the file.
'        Dim br As New BinaryReader(fs)
'        ' Dim an array of bytes large enough to hold the file's contents.
'        Dim bytes(fs.Length) As Byte
'        Dim bSuccess As Boolean
'        ' Your unmanaged pointer.
'        Dim pUnmanagedBytes As IntPtr

'        ' Read the contents of the file into the array.
'        bytes = br.ReadBytes(fs.Length)
'        ' Allocate some unmanaged memory for those bytes.
'        pUnmanagedBytes = Marshal.AllocCoTaskMem(fs.Length)
'        ' Copy the managed byte array into the unmanaged array.
'        Marshal.Copy(bytes, 0, pUnmanagedBytes, fs.Length)
'        ' Send the unmanaged bytes to the printer.
'        bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, fs.Length)
'        ' Free the unmanaged memory that you allocated earlier.
'        Marshal.FreeCoTaskMem(pUnmanagedBytes)
'        Return bSuccess
'    End Function ' SendFileToPrinter()

'    ' When the function is given a string and a printer name,
'    ' the function sends the string to the printer as raw bytes.
'    Public Shared Sub SendStringToPrinter(ByVal szPrinterName As String, ByVal szString As String)
'        Dim pBytes As IntPtr
'        Dim dwCount As Int32
'        ' How many characters are in the string?
'        dwCount = szString.Length()
'        ' Assume that the printer is expecting ANSI text, and then convert
'        ' the string to ANSI text.
'        pBytes = Marshal.StringToCoTaskMemAnsi(szString)
'        ' Send the converted ANSI string to the printer.
'        SendBytesToPrinter(szPrinterName, pBytes, dwCount)
'        Marshal.FreeCoTaskMem(pBytes)
'    End Sub
'End Class