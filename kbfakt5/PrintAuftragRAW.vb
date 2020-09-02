Option Explicit On
Imports System.Data.OleDb
Imports System.IO
Imports System.Drawing.Printing
Imports Microsoft.Win32

Public Class PrintAuftragRAW
    Public AuftragsNummern As New Collection
    'Public AUFTR_Nr As String = ""
    Public DruckTyp As String = "Rechnung"
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
    Private KopfZeilen As New Collection
    Private FussZeilen As New Collection
    Private m_InitString As String

    Dim currentPage As Integer = 1
    Dim totalPages As Integer = 1
    Dim numberOfItems As Integer = 0

    Private m_CharsPerLine As Integer = 80
    Private m_LinesPerPage As Integer = 70 'was 60
    'Printing Margins
    Private m_Bottom As Integer
    Private m_Left As Integer
    Private m_Right As Integer
    Private m_Top As Integer
    Private m_TopOffsetLines As Integer = 0
    Private m_LeftMarginChars As Integer = 0

    Private m_PrinterName As String
    Private m_iPaperKind As Integer 'PaperKind
    Dim pWidth As Long = 827 'CInt(21 / 2.54 * 100) '827
    Dim pHeight As Long = 1200 'CInt(30.5 / 2.54 * 100) '1200
    Dim m_PaperWidth As Long
    Dim m_PaperHeight As Long
    Private streamToPrint As StreamReader

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        WriteReg()
        FormSettingsSave(Me)
        If Not IsNothing(streamToPrint) Then streamToPrint.Close()
        Me.Dispose()
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
        Dim LeftMarginChars As Integer = 0
        Dim RawPageLength As Integer = 80
        Dim PrinterName As String = "OKI Rechnungsdrucker"
        Dim iPaperKind As Integer = 129 'custom paper!
        Dim InitString As String = "1B401B4D"

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
            m_LeftMarginChars = Registry.GetValue(keyName, "LeftMarginChars", LeftMarginChars)
            m_InitString = Registry.GetValue(keyName, "InitString", InitString)
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
        m_LinesPerPage = RawPageLength
        m_InitString = InitString
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
            Registry.SetValue(keyName, "RawPageLength", m_LinesPerPage)
            Registry.SetValue(keyName, "PaperWidth", m_PaperWidth)
            Registry.SetValue(keyName, "PaperHeight", m_PaperHeight)
            Registry.SetValue(keyName, "PrinterName", m_PrinterName)
            Registry.SetValue(keyName, "iPaperKind", m_iPaperKind)
            Registry.SetValue(keyName, "LeftMarginChars", m_LeftMarginChars)
            Registry.SetValue(keyName, "InitString", m_InitString)

        Catch x As Exception
            MessageBox.Show("Exception in WriteReg()" + vbCrLf + x.Message, "Fehler")
        End Try

    End Sub

    Private Sub ListPrinters()
        'try to find the right printer
        ' Add list of installed printers found to the combo box.
        ' The pkInstalledPrinters string will be used to provide the display string.
        Dim pkInstalledPrinters As String
        Dim foundIt As Boolean = False
        Debug.WriteLine("Listing installed Printers: ")
        Dim i As Integer
        For i = 0 To PrinterSettings.InstalledPrinters.Count - 1
            listboxPrinters.Items.Insert(i, PrinterSettings.InstalledPrinters.Item(i))
            pkInstalledPrinters = PrinterSettings.InstalledPrinters.Item(i)
            Debug.WriteLine(vbTab & pkInstalledPrinters)
            If pkInstalledPrinters = m_PrinterName Then
                'Apply the OKI printer to PrintDoc
                'PrintDocument1.PrinterSettings.PrinterName = m_PrinterName
                Debug.WriteLine("Found and assigned '" & m_PrinterName & "'")
                listboxPrinters.SelectedIndex = i
                foundIt = True
                'Exit For
            End If
        Next

    End Sub
    ''' <summary>
    ''' prints the header lines into filestream
    ''' </summary>
    ''' <returns>number of header lines</returns>
    ''' <remarks></remarks>
    Private Function printKopf(ByVal FStream As StreamWriter) As Integer
        Dim lCount As Integer = 0
        For Each s As Object In KopfZeilen
            FStream.WriteLine(s.ToString)
            lCount += 1
        Next
        lCount += 1
        Return lCount
    End Function
    ''' <summary>
    ''' prints the footer to fstream
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub printFuss(ByVal FStream As StreamWriter)
        For Each s As Object In FussZeilen
            FStream.WriteLine(s.ToString)
        Next
    End Sub
    ''' <summary>
    ''' adds the footer text to a collection
    ''' </summary>
    ''' <returns>number of lines added</returns>
    ''' <remarks></remarks>
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
    ''' <summary>
    ''' adds n lines in the header collection
    ''' </summary>
    ''' <param name="nOfLines"></param>
    ''' <remarks></remarks>
    Private Sub SkipLines(ByVal nOfLines As Integer)
        Dim i As Integer
        For i = 1 To nOfLines
            KopfZeilen.Add("")
        Next
    End Sub
    ''' <summary>
    ''' adds the header lines to a collection
    ''' </summary>
    ''' <param name="AuftragsNummer">a collection with order numbers</param>
    ''' <remarks></remarks>
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

        'add some lines at top
        Dim i As Integer
        For i = 0 To m_TopOffsetLines
            KopfZeilen.Add("")
        Next
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
        'add if then for with / without printing of Leistungsdatum
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

            KopfZeilen.Add("TÜV:".PadLeft(15) + KFZ.TUEV.PadLeft(6) + _
                ", ASU:" + KFZ.ASU.PadLeft(6) + _
                ", Tacho:" + KFZ.Schreiber.PadLeft(6) + _
                ", Sicherh.:" + KFZ.Sicher.PadLeft(6))
            'KopfZeilen.Add("KM-Stand:".PadLeft(15) + txtXKMSTAND.PadLeft(28 - 15))

            connection.Close()

    End Sub
    ''' <summary>
    ''' fills fstream with data from order
    ''' </summary>
    ''' <param name="AuftragsNummern">a collection of order numbers</param>
    ''' <remarks></remarks>
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
        Dim AnzAufträge As Integer = AuftragsNummern.Count
        Dim AuftragID As Integer
        Dim AuftragsNummer As Object
        lCountFooter = FillFussZeilen()
        'empty file
        'FStream.Close()
        Dim FStream As StreamWriter
        FStream = New StreamWriter(TempFile, False, System.Text.Encoding.Default) ' System.Text.Encoding.GetEncoding(437))

        For AuftragID = 1 To AuftragsNummern.Count
            AuftragsNummer = AuftragsNummern.Item(AuftragID)
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
            AnzKopfzeilen = printKopf(FStream)
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
            iTemp = WriteDetailsHeader(FStream, PageCurrent, totalPages, ZwischenSumme)
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
                    LineCount = printKopf(FStream) + WriteDetailsHeader(FStream, PageCurrent, totalPages, ZwischenSumme)
                    System.Diagnostics.Debug.WriteLine("LineCount=" & LineCount)
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
            'change in Version 2.1.2.0
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
                'changed in version 2.1.2.0
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
            printFuss(FStream)
            If AuftragID < AuftragsNummern.Count Then FStream.WriteLine(Chr(12))
            dbset.Clear() 'new in 2.0.1.6, without the details data is mixed up from different bills
        Next AuftragID
        ' cleanup!
        dbset.Dispose() ' added in 2.0.1.6
        oleda.Dispose()
        connection.Close()
        'show the text
        FStream.Close()
        'System.Text.Encoding.ASCII
        Dim t As String
        Dim s As StreamReader
        s = New StreamReader(TempFile, System.Text.Encoding.Default)
        t = s.ReadToEnd()
        s.Close()
        RichTextBox1.Text = t
        'find attribut and set text
        Dim start, ende As Integer
        start = RichTextBox1.Text.IndexOf("^E")
        Do While start > 0
            ende = RichTextBox1.Text.IndexOf(Chr(10), start)
            If ende > 0 Then
                RichTextBox1.Text = RichTextBox1.Text.Remove(start, 2)
                RichTextBox1.Select(start, ende - start - 2)
                RichTextBox1.SelectionFont = New Font(RichTextBox1.Font.Name, RichTextBox1.Font.Size + 2, FontStyle.Bold)
            End If
            'selectionFont only works on last selection, so now exit
            Exit Do
            start = RichTextBox1.Text.IndexOf("^E")
        Loop
        'RichTextBox1.LoadFile(TempFile, RichTextBoxStreamType.PlainText) 'LoadFile(TempFile, RichTextBoxStreamType.PlainText)
    End Sub

    Private Function WriteDetailsHeader(ByVal fstream As StreamWriter, ByVal PageCurrent As Integer, ByVal PagesTotal As Integer, ByVal zwischenSumme As Double) As Integer
        Dim lCount As Integer = 0
        fstream.WriteLine("-".PadRight(m_CharsPerLine, "-"))
        lCount += 1
        fstream.WriteLine(("Seite(n): " & PageCurrent & "/" & PagesTotal).PadLeft(m_CharsPerLine))
        lCount += 1
        If zwischenSumme <> 0 Then
            fstream.WriteLine(("Zwischensumme: " + Format(zwischenSumme, "#.00")).PadLeft(m_CharsPerLine))
        Else
            fstream.WriteLine()
        End If
        lCount += 1
        fstream.WriteLine("-".PadRight(m_CharsPerLine, "-"))
        lCount += 1
        If m_CharsPerLine >= 80 Then _
            fstream.Write("Pos".PadRight(3) + " ") ' 4
        fstream.Write("Artikelnr.".PadRight(17) + " ")          '18
        fstream.Write("Artikelbezeichnung".PadRight(30) + " ")  '31
        fstream.Write("Menge".PadRight(6) + " ")                ' 7
        fstream.Write("E-Preis".PadRight(7) + " ")              ' 8
        fstream.Write("Rab".PadRight(3) + " ")                  ' 4
        fstream.Write("G-Preis".PadRight(8))                    ' 9
        fstream.WriteLine()
        lCount += 1
        fstream.WriteLine("=".PadRight(m_CharsPerLine, "="))
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
        Dim queryString As String = _
        "select count(*) from RECH2 where Auftr_Nr=" + auftragsnummer
        Dim DetailsLineCount As Integer = 0
        Dim DetailsLineCount2 As Integer = 0
        Try
            OpenDBConnection(connection)
            Dim command As New OleDbCommand(queryString, connection)
            'connection.Open() is already opened with OpenDBConnection
            DetailsLineCount = command.ExecuteScalar()
            queryString = _
            "select count(*) from RECH2 where Auftr_Nr=" + _
            auftragsnummer + _
            " AND LEN(ARTIKELBEZ)>30"
            command.CommandText = queryString
            DetailsLineCount2 = command.ExecuteScalar()
        Catch ex As Exception
            MessageBox.Show("Exception in GetDetailsLineCount: " + vbCrLf + ex.Message)
        End Try
        connection.Close()
        GetDetailsLineCount = DetailsLineCount + DetailsLineCount2
    End Function
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
    Private Function GetCharsPerLine() As Integer
        ''Dim g As Graphics
        ''Dim u = printFont.Unit 'points?
        ''g = PrintDocument1.PrinterSettings.CreateMeasurementGraphics(PrintDocument1.DefaultPageSettings, True)
        ''g.PageUnit = u
        ''Dim cpi As SizeF = g.MeasureString("W", printFont) ', 0, StringFormat.GenericTypographic)
        ''Dim w = PrintDocument1.DefaultPageSettings.PrintableArea.Width
        ''GetCharsPerLine = CInt(w \ cpi.Width)
        If GetCharsPerLine < 80 Then
            Debug.Print("Warning: Forcing CharsPerLine to 80")
            GetCharsPerLine = 80
        End If
        ''g.Dispose()
    End Function
    Private Function GetLinesPerPage() As Integer
        'Dim g As Graphics
        'Dim u = printFont.Unit 'points?
        'g = PrintDocument1.PrinterSettings.CreateMeasurementGraphics(True)
        'g.PageUnit = u
        Dim lpp As Integer = 70 'printFont.Height ' GetHeight(g)

        'Dim h = PrintDocument1.DefaultPageSettings.Bounds.Height - _
        '        PrintDocument1.DefaultPageSettings.Margins.Top - _
        '        PrintDocument1.DefaultPageSettings.Margins.Bottom
        'GetLinesPerPage = lpp 'CInt(h \ lpp)
        'g.Dispose()
    End Function
    Private Sub printJobInit(ByRef f As StreamWriter)
        Dim ints As Integer()
        ints = hex2int(m_InitString)
        Dim i As Integer
        For i = 0 To ints.Length - 1
            f.Write(Chr(ints(i)))
        Next
        Exit Sub
        'Send InitPage: <esc>@ = reset   +   set page length in inches
        f.Write(Chr(27) + Chr(64)) ' + Chr(27) + Chr(67) + Chr(0) + Chr(12))
        '15 cpi = 1B 67
        f.Write(Chr(27) + Chr(103))
        '12 cpi = 1B 4D

    End Sub
    Private Function hex2int(ByVal s As String) As Integer()
        Dim i As Integer = 0
        Dim ix As Integer = 0
        Dim t As String
        Dim out As Integer()
        ReDim out((s.Length / 2) - 1)
        While i < s.Length - 1
            t = s.Substring(i, 2)
            out(ix) = CInt("&H" + t)
            i += 2
            ix += 1
        End While
        Return out
    End Function

    Private Sub btnPrintRAW_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDrucken.Click
        ''With PrintDialog1
        ''    .Document = PrintDocument1
        ''    .Document.DocumentName = DruckTyp
        ''    .AllowCurrentPage = False
        ''    .AllowSelection = False
        ''    .AllowSomePages = False
        ''    .ShowHelp = False
        ''End With
        Dim count As Integer = 0
        Dim ESC As String = Chr(27)
        'Recalculate pages
        m_CharsPerLine = GetCharsPerLine()
        PrintForm(AuftragsNummern)

        ''PrintDialog1.PrinterSettings = New PrinterSettings()

        If True Then ''(PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Dim RawFile As StreamWriter
            ' System.Text.Encoding.GetEncoding(437))
            'write a cp437 encoded file
            RawFile = New StreamWriter(TempFile + ".raw", False, System.Text.Encoding.GetEncoding(437))
            ' oder CodePages 850, 852, 28591 oder System.Text.Encoding.ASCII (nur 7 Bit!))

            'Convert the file to a raw file, read the ANSI encoded file
            streamToPrint = New StreamReader(TempFile, System.Text.Encoding.Default)
            'Send InitPage: <esc>@ = reset   +   set page length in inches
            printJobInit(RawFile)
            Dim line As String
            Do
                line = streamToPrint.ReadLine()
                'System.Diagnostics.Debug.WriteLine(line)
                'System.Diagnostics.Debug.WriteLine(String2Hex(line))
                If line Is Nothing Then Exit Do
                'paragraphen zeichen ANSI=0167 ASCII cp437 = 0x15 = 21
                'line.Replace("§", Chr(21))
                If line.StartsWith("^B") Then
                    line = ESC + "E" + line.Substring(2) + ESC + "F"
                    RawFile.WriteLine(Space(m_LeftMarginChars) + line)
                ElseIf line.StartsWith("^E") Then
                    'line = ESC + "W1" + line.Substring(2) + ESC + "W0"
                    line = ESC + "E" + line.Substring(2) + ESC + "F"
                    RawFile.WriteLine(Space(m_LeftMarginChars) + line)
                ElseIf line = Chr(12) Then 'moved block 2.0.1.1
                    RawFile.Write(Chr(12)) 'write the FormFeed
                    count = 0
                Else
                    RawFile.WriteLine(Space(m_LeftMarginChars) + line)
                End If
                count += 1
            Loop Until line Is Nothing

            RawFile.Write(Chr(12))
            streamToPrint.Close()
            RawFile.Close()
            If DebugModus() Then _
                If MessageBox.Show("Drucken?", "Auftrag drucken", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
            If Not RawPrinterHelper.SendFileToPrinter(listboxPrinters.Items(listboxPrinters.SelectedIndex), TempFile + ".raw") Then
                System.Diagnostics.Debug.WriteLine("RawPrinterHelper failed")
                MessageBox.Show("Ausdruck fehlgeschlagen!", "Fehler beim Zugriff auf Drucker")
            Else
                MessageBox.Show("Ausdruck erfolgt", "Kein Fehler")
            End If
        End If
    End Sub
    Private Sub PrintAuftragRAW_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        readReg()
        ListPrinters()
        AppPath = mainmodul.getAppDir()
        TempFile = AppPath + TempFile
        'TEST
        If DebugModus() And AuftragsNummern.Count = 0 Then AuftragsNummern.Add("1005647")
        PrintForm(AuftragsNummern)
        currentPreviewPage = 1
        FormSettingsLoad(Me)
        RichTextBox1.Focus()
    End Sub

    Private Sub listboxPrinters_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles listboxPrinters.SelectedIndexChanged
        m_PrinterName = listboxPrinters.SelectedItem.ToString()
    End Sub

    Private Sub btnSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetup.Click
        Dim dlg As New PrintAuftragRawSetup
        With dlg
            .m_LeftMarginChars = m_LeftMarginChars
            .m_LinesPerPage = m_LinesPerPage
            .m_TopOffset = m_TopOffsetLines
            .m_InitString = m_InitString
        End With
        If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
            With dlg
                m_LeftMarginChars = .m_LeftMarginChars
                m_LinesPerPage = .m_LinesPerPage
                m_TopOffsetLines = .m_TopOffset
                m_InitString = .m_InitString
                WriteReg()
                PrintForm(AuftragsNummern)
            End With
        End If
    End Sub
End Class