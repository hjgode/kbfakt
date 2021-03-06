Public Class kbfakt_start

    Private Sub BeendenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BeendenToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub ImportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuImport.Click
        If LoginForm1.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
        datei_import.ShowDialog()
    End Sub

    Private Sub kbfakt_start_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
        InitDBfilename()
        'If DebugModus() Then Label1.Text = DBfileName Else Label1.Visible = False
        If True Then Label1.Text = DBfileName Else Label1.Visible = False
        If PatchPassword() Then
            System.Diagnostics.Debug.WriteLine("PatchPassword success")
        Else
            System.Diagnostics.Debug.WriteLine("PatchPassword failed")
        End If
        Patch1ArtStamm()
        Patch2Rech1()
        Patch3Rech1() 'add Gutschrift field
        'FirmStam Patch adds a column FussText
        Patch1FirmStam()
        PatchRichtwerte()
        'patch: insert LDatum (Leistungsdatum)
        Patch4Rech1()
        Patch2FirmStam() ' AltTeilMwSt Vorgabewert
        Patch5AltTeilMwSt2() ' change AltTeilMwSt calculation
        If DebugModus() Then
            Button1.Visible = True
        Else
            Button1.Visible = False
        End If
    End Sub

    Private Sub AdminToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdmin.Click
        If LoginForm1.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
        admin.ShowDialog()
    End Sub

    Private Sub btnAuftrag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuftrag.Click
        ShowWaitCursor(True)
        Auftrag.ShowDialog()
        Auftrag.Dispose()
    End Sub
    Public Shared Function FillGrid(ByRef grid As DataGrid, ByVal query As String) As Integer
        'grid.ResetBindings()
        'grid.BindingContext = Nothing
        'grid.DataMember = Nothing
        'grid.DataSource = Nothing
        Dim c As Integer
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim da As New OleDb.OleDbDataAdapter(query, cn)
        Dim tbl As New DataTable
        c = da.Fill(tbl)
        Dim bs As New BindingSource
        bs.DataSource = tbl 'DBDS.Tables("Table")
        If c > 0 Then
            grid.DataSource = bs 'use TableMappings.Add("Table", ... to rename table ref
        Else
            grid.DataSource = Nothing
        End If
        grid.ReadOnly = True
        cn.Close()
        Return c
    End Function
    Public Shared Function FillGrid(ByRef grid As DataGridView, ByVal query As String) As Integer
        Dim c As Integer
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        'c = FillDS(query)
        Dim da As New OleDb.OleDbDataAdapter(query, cn)
        Dim tbl As New DataTable
        c = da.Fill(tbl)
        Dim bs As New BindingSource
        bs.DataSource = tbl 'DBDS.Tables("Table")
        Dim bAddColumn As Integer = -1
        If c >= 0 Then
            grid.DataSource = bs '(DBDS, "Table") 'use TableMappings.Add("Table", ... to rename table ref
            'column formatting
            Dim colname As String
            For Each dc As DataColumn In tbl.Columns
                'If System.Type.GetTypeCode(dc.DataType) = TypeCode.Decimal _
                'Or System.Type.GetTypeCode(dc.DataType) = TypeCode.Double Then
                colname = dc.ColumnName.ToUpper
                If colname = "E_PREIS" Or colname = "VK" Or colname = "PREIS" Then
                    grid.Columns(dc.ColumnName).Width = 60
                    grid.Columns(dc.ColumnName).DefaultCellStyle.Format = "0.00"
                    grid.Columns(dc.ColumnName).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf colname = "ARTIKELNR" Then
                    grid.Columns(dc.ColumnName).ToolTipText = "Artikel mit + werden beim Buchen neu angelegt, Artikel mit * sind frei, F3 zum Suchen"

                ElseIf colname = "GESAMT" Then
                    grid.Columns(dc.ColumnName).Width = 60
                    grid.Columns(dc.ColumnName).DefaultCellStyle.Format = "0.00"
                    grid.Columns(dc.ColumnName).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    grid.Columns(dc.ColumnName).ReadOnly = True
                ElseIf colname = "MENGE" Then
                    grid.Columns(dc.ColumnName).Width = 60
                    grid.Columns(dc.ColumnName).DefaultCellStyle.Format = "0.000"
                    grid.Columns(dc.ColumnName).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf colname = "ARTIKELBEZ" Then
                    grid.Columns(dc.ColumnName).Width = 260
                    Dim FixedFont As New Font("Courier New", 10)

                    'New in 2.0.0.5
                    ' create a new bitmap
                    Dim bmp As New Bitmap(400, 200, Drawing.Imaging.PixelFormat.Format16bppRgb565)
                    ' get the underlying Graphics object
                    Dim gr As Graphics = Graphics.FromImage(bmp)
                    ' measure the string
                    Dim stringSize As SizeF = gr.MeasureString("012345678901234567890123456789", _
                        New Font("Courier New", 10))
                    grid.Columns(dc.ColumnName).Width = stringSize.Width '+ stringSize.Width / 30

                    grid.Columns(dc.ColumnName).DefaultCellStyle.Font = New Font("Courier New", 10) 'new in 2.0.0.5

                ElseIf colname.ToUpper = "POS" Then
                    grid.Columns(dc.ColumnName).Width = 30
                    grid.Columns(dc.ColumnName).DefaultCellStyle.Format = "0"
                    grid.Columns(dc.ColumnName).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    grid.Columns(dc.ColumnName).ReadOnly = True
                ElseIf colname = "ID" Then
                    grid.Columns(dc.ColumnName).Width = 30
                    grid.Columns(dc.ColumnName).DefaultCellStyle.Format = "0"
                    grid.Columns(dc.ColumnName).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    grid.Columns(dc.ColumnName).ReadOnly = True
                    If Not isDebug Then grid.Columns(dc.ColumnName).Visible = False
                ElseIf colname = "ARTTYP" Then
                    grid.Columns(dc.ColumnName).Width = 30
                    grid.Columns(dc.ColumnName).DefaultCellStyle.Format = "0"
                    grid.Columns(dc.ColumnName).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    grid.Columns(dc.ColumnName).Visible = False
                    bAddColumn = dc.Ordinal
                ElseIf colname = "RABATT" Then
                    grid.Columns(dc.ColumnName).Width = 60
                    grid.Columns(dc.ColumnName).DefaultCellStyle.Format = "0"
                    grid.Columns(dc.ColumnName).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
            Next dc
            If bAddColumn >= 0 Then
                Dim found As Boolean = False
                For Each h As DataGridViewColumn In grid.Columns
                    If h.HeaderText = "Art-Typ" Then found = True
                Next h
                If Not found Then AddComboBoxColumnsArtikelTyp(grid, bAddColumn)
            End If
        Else
            grid.DataSource = Nothing
        End If
        ' grid.ReadOnly = True
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.EditingPanel.BorderStyle = BorderStyle.Fixed3D
        cn.Close()
        Return c
    End Function
    Private Shared Sub AddComboBoxColumnsArtikelTyp(ByRef grid As DataGridView, ByVal pos As Integer)
        Dim comboboxColumn As New DataGridViewComboBoxColumn()

        comboboxColumn = CreateComboBoxColumn("ArtTyp")

        SetAlternateChoicesUsingDataSource(comboboxColumn)
        comboboxColumn.HeaderText = "Art-Typ"
        comboboxColumn.Name = "Art-Typ"
        'comboboxColumn.DataSource = RetrieveAlternativeArtTypen()
        grid.Columns.Insert(pos, comboboxColumn)

        'comboboxColumn = CreateComboBoxColumn("ArtTyp", "ArtTyp")
        'SetAlternateChoicesUsingItems(comboboxColumn)
        'comboboxColumn.HeaderText = _
        '    "TitleOfCourtesy (via Items property)"
        '' Tack this example column onto the end.
        'grid.Columns.Add(comboboxColumn)
    End Sub
    Private Shared Sub SetAlternateChoicesUsingDataSource(ByRef comboboxColumn As DataGridViewComboBoxColumn)
        With comboboxColumn
            .DataSource = RetrieveAlternativeArtTypen()
            .ValueMember = "id" 'ColumnName.TitleOfCourtesy.ToString()
            .DisplayMember = "ArtikelTyp"
        End With
    End Sub

    Private Shared Function CreateComboBoxColumn(ByVal header As String) As DataGridViewComboBoxColumn
        Dim column As New DataGridViewComboBoxColumn()

        With column
            .DataPropertyName = "ArtTyp"
            .HeaderText = header
            .DropDownWidth = 100
            .Width = 90
            .MaxDropDownItems = 4
            .FlatStyle = FlatStyle.Flat
        End With
        Return column
    End Function
    Private Shared Function RetrieveAlternativeArtTypen() As DataTable
        Return Populate("SELECT  ArtikelTyp , id FROM ArtTypen")
    End Function
    Private Shared Function Populate(ByVal cmd As String) As DataTable
        Dim cn As New OleDb.OleDbConnection()
        OpenDBConnection(cn)
        Dim command As New OleDb.OleDbCommand(cmd, cn)
        Dim adapter As New OleDb.OleDbDataAdapter()
        adapter.SelectCommand = command
        Dim table As New DataTable()
        table.Locale = System.Globalization.CultureInfo.InvariantCulture
        adapter.Fill(table)

        Return table
    End Function

    Public Shared Function FillDS(ByVal query As String) As Integer
        'DBDA = New OleDbDataAdapter
        'DBDA.SelectCommand = New OleDbCommand(query, cn)
        'DBDS = New DataSet
        ''DBDS.Clear()
        'Try
        '    Return DBDA.Fill(DBDS)
        'Catch
        '    Return 0
        'End Try
    End Function

    Private Sub btnKunden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKunden.Click
        Dim dlg As Kundenliste = New Kundenliste
        dlg.InvokedByStart = True
        dlg.btn_cancel.Visible = False
        dlg.btn_OK.Text = "Schliessen"
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub btnFahrzeuge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFahrzeuge.Click
        Dim dlg As ListenAnsicht
        dlg = New ListenAnsicht
        dlg.RechnungsNr = 0
        dlg.KundenNr = 0
        dlg.FahrgestellNr = ""
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub btnAuswertungen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuswertungen.Click
        If LoginForm2.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Auswertungen.ShowDialog()
        End If
    End Sub

    Private Sub btnSammelDruck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSammelDruck.Click
        Dim aw As Auswertungen
        aw = New Auswertungen
        aw.bSammelDruck = True
        aw.Text = "Sammeldruck"
        aw.SelectString = "select * from RECH1 where Not Gedruckt"
        aw.btnDrucken.Visible = True
        aw.ShowDialog()
        aw.Dispose()
    End Sub

    Private Sub btnArtikelStamm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnArtikelStamm.Click
        Dim dlg As ArtikelListe
        dlg = New ArtikelListe
        dlg.txtboxFilter.Text = ""
        dlg.DetailsMode = True
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub StammdatenToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StammdatenToolStripMenuItem.Click
        Dim dlg As Stammdaten
        dlg = New Stammdaten
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        'Create an AutoBackup
        Dim my_AutoBackup As New AutoBackupClass
        Try
            my_AutoBackup.MakeBackup()
        Catch ax As System.ArgumentOutOfRangeException
        Catch ex As Exception
        End Try

        Application.Exit()
    End Sub

    Private Sub btnOffeneRechnungen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOffeneRechnungen.Click
        Dim dlg As OffeneRechnungen
        dlg = New OffeneRechnungen
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub ÜberToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ÜberToolStripMenuItem.Click
        Dim dlg As New AboutBox1
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub btnRichtwerte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRichtwerte.Click
        Dim dlg As New Richtwerte
        dlg.ShowDialog()
        dlg.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'PrintAuftragRAW.ShowDialog()

    End Sub
    Private Sub btnSicherungen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSicherungen.Click
        DatenSicherung.ShowDialog()
    End Sub

    Private Sub btnTermine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTermine.Click
        Dim aw As Auswertungen
        Dim dieserMonat As String
        Dim diesesJahr As String
        dieserMonat = Now.ToString("MM") ' String.Format("dd", Now())
        diesesJahr = Now.ToString("yy") 'String.Format("yy", Now())
        Dim testString As String
        testString = """" + dieserMonat + "_" + diesesJahr + """" 'use ANSI SQL _ instead of ?
        aw = New Auswertungen
        aw.bSammelDruck = False
        aw.TerminDruck = True
        aw.Text = "Anstehende Terminfälligkeiten"
        'SELECT KUNDFAHR.KENNZEICH, KUNDFAHR.TUEV, KUNDENST.VORNAME, KUNDENST.NACHNAME
        'FROM KUNDFAHR, KUNDENST WHERE KUNDFAHR.KUNDEN_NR = KUNDENST.KUNDENNR AND KUNDFAHR.TUEV LIKE "06?08";
        aw.SelectString = "select f.Kennzeich, f.TUEV, f.ASU, f.Schreiber, f.SICHER, k.kundennr, k.VORNAME, k.NACHNAME, k.strasse, k.PLZ, k.ort " & _
            " from KUNDFAHR as f, KUNDENST as k" & _
            " where k.KUNDENNR=f.KUNDEN_NR AND " & _
            " (f.TUEV LIKE " + testString + _
            " OR f.ASU LIKE " + testString + _
            " OR f.Schreiber LIKE " + testString + _
            " OR f.Sicher LIKE " + testString + ")" + _
            " order by k.NACHNAME;"
        aw.sqlPrint = "select 'Kennzeichen', 0, 15, 'L', 'N', '', 1, f.Kennzeich, " & _
                                      "'TÜV', 15, 25, 'L', 'N', '', 1, f.TUEV, " & _
                                      "'ASU', 25, 35, 'L', 'N', '', 1, f.ASU, " & _
                                      "'Schreiber', 35, 45, 'L', 'N', '', 1, f.schreiber, " & _
                                      "'Sicher', 45, 55, 'L', 'N', '', 1, f.SICHER, " & _
                                      "'KNR', 0, 16, 'L', 'N', '', 2, k.kundennr, " & _
                                      "'Vorname', 20, 40, 'L', 'N', '', 2, k.VORNAME, " & _
                                      "'Nachnahme', 41, 60, 'L', 'N', '', 2, k.NACHNAME, " & _
                                      "'Strasse', 61, 80, 'L', 'N', '', 2, k.strasse, " & _
                                      "'PLZ', 41, 50, 'L', 'N', '', 3, k.PLZ, " & _
                                      "'ORT', 51, 71, 'L', 'N', '', 3, k.ORT " & _
                                " FROM (" & _
                                    "select f.Kennzeich, " & _
                                    "f.TUEV, " & _
                                    "f.ASU, " & _
                                    "f.Schreiber, " & _
                                    "f.SICHER, " & _
                                    "k.kundennr, " & _
                                    "k.VORNAME, " & _
                                    "k.NACHNAME, " & _
                                    "k.strasse, " & _
                                    "k.PLZ, " & _
                                    "k.ort " & _
                                    " from KUNDFAHR as f, KUNDENST as k" & _
                                    " where k.KUNDENNR=f.KUNDEN_NR AND " & _
                                    " (f.TUEV LIKE " + testString + _
                                    " OR f.ASU LIKE " + testString + _
                                    " OR f.Schreiber LIKE " + testString + _
                                    " OR f.Sicher LIKE " + testString + ")" + _
                                    " order by k.NACHNAME, f.kennzeich" + _
                                ")"

        aw.btnDrucken.Visible = True
        aw.ShowDialog()
        aw.Dispose()
    End Sub
End Class