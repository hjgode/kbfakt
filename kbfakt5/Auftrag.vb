Option Explicit On
Option Strict Off
Imports System.Data.OleDb

Public Class Auftrag
    Public auftragsnummer As Double = 0
    Public isNeuerAuftrag As Boolean = False
    Public mIsChanged As Boolean = False
    ''' <summary>
    ''' Global var to remember, if Artikle was searched and found with dialog
    ''' </summary>
    ''' <remarks></remarks>
    Private g_ArtikelFound = False

    ''' <summary>
    ''' will add the changed event handler to every text control
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addChangedHandler()
        Dim txtbox As TextBox = Nothing
        Dim dgv As DataGridView = Nothing
        Dim mTxtBox As MaskedTextBox = Nothing
        Dim mDateTimePicker As DateTimePicker = Nothing 'v 2.0.3.0
        Dim ctrl As Control
        For Each ctrl In Me.Controls
            If TypeOf ctrl Is TextBox Then
                txtbox = CType(ctrl, TextBox)
                AddHandler txtbox.TextChanged, AddressOf TextBoxChanged ' ModifiedChanged
                'If txtbox.Name = "txtXName1" Then
                '    Debug.Print("txtXName1")
                'End If
            ElseIf TypeOf ctrl Is MaskedTextBox Then
                mTxtBox = CType(ctrl, MaskedTextBox)
                AddHandler mTxtBox.TextChanged, AddressOf TextBoxChanged
            ElseIf TypeOf ctrl Is DataGridView Then
                dgv = CType(ctrl, DataGridView)
                AddHandler dgv.CellValueChanged, AddressOf DataGridChanged
            ElseIf TypeOf ctrl Is DateTimePicker Then
                mDateTimePicker = CType(ctrl, DateTimePicker)
                AddHandler mDateTimePicker.ValueChanged, AddressOf DateTimePickerChanged
            End If
        Next
    End Sub
    ''' <summary>
    ''' DateTimePicker ValueChanged event handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DateTimePickerChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DataChanged(True)
        Debug.Print("DataChanged: " + sender.ToString)
    End Sub
    ''' <summary>
    ''' TextBoxChanged event handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TextBoxChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DataChanged(True)
        Debug.Print(sender.ToString)
    End Sub
    ''' <summary>
    ''' DataGridView Cell changed event handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DataGridChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        DataChanged(True)
    End Sub
    ''' <summary>
    ''' Change buttons enabled/disabled etc according to Changed Status
    ''' </summary>
    ''' <param name="b">changed or not?</param>
    ''' <remarks></remarks>
    Private Sub DataChanged(ByVal b As Boolean)
        If b Then
            DataChangedStatus.BackColor = Color.Red
            mIsChanged = True
        Else
            For Each ctrl As Control In Me.Controls
                If (TypeOf ctrl Is TextBox) Or _
                    (TypeOf ctrl Is MaskedTextBox) Or _
                    (TypeOf ctrl Is DataGridView) Then
                    ctrl.BackColor = Color.White
                End If
            Next
            'btnDrucken.Enabled = Not mIsChanged
            DataChangedStatus.BackColor = Color.Green
            mIsChanged = False
        End If
        btnAbbrechen.Enabled = mIsChanged
        btnSpeichern.Enabled = mIsChanged
    End Sub

    Private Sub Auftrag_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If System.Diagnostics.Debugger.IsAttached Then
            idAuftrag.Visible = True
            idFahrzeug.Visible = True
        Else
            idAuftrag.Visible = False
            idFahrzeug.Visible = False
        End If
        FillAnredenListbox()
        NeuerAuftrag()
        ''enter new auftragsnummer
        'txtAuftragNummer.Text = (GetMaxAuftragNr() + 1).ToString
        ''init listbox anreden
        'FillAnredenListbox()
        ''init the dataset
        'initDSRech1Rech2()
        ''read stamm data
        'readFirmstamm()
        ''apply actual MwSt
        'txtMwStSatz.Text = String.Format("{0}", gMwSt)
        ''for debug always enable printing
        If DebugModus() Then
            btnBuchenTest.Visible = True
            chkGedruckt.Enabled = True
            Button1.Visible = True
        Else
            btnBuchenTest.Visible = False
            chkGedruckt.Enabled = False
            Button1.Visible = False
        End If

        ''test auftragsnummer
        'checkAuftragsnummer()
        ''add changed handler event to different controls
        addChangedHandler()
        DataChangedStatus.BackColor = Color.Green
        ShowWaitCursor(False)
        'HGO TEST
        DataGridView1.EditMode = DataGridViewEditMode.EditOnEnter
        'DataGridView1.EditMode =DataGridViewEditMode.EditOnF2
        'new in version, limit text entry
        txtXName1.MaxLength = 15
        txtXName2.MaxLength = 30
        txtXST.MaxLength = 30
        txtXPL.MaxLength = 6
        txtXOT.MaxLength = 24
        txtXTel.MaxLength = 20

    End Sub
    Sub FillAnredenListbox()
        Dim queryString As String
        Dim idx As Integer
        queryString = "Select ID, Anrede from Anreden ORDER by ID"
        Dim cn As New OleDbConnection
        OpenDBConnection(cn)
        AnredenListBox.Items.Clear()
        Using cn
            Dim command As New OleDbCommand(queryString, cn)
            Dim reader As OleDbDataReader = command.ExecuteReader()
            While reader.Read()
                idx = AnredenListBox.Items.Add(reader("Anrede").ToString)

            End While
            reader.Close()
        End Using
        AnredenListBox.SelectedIndex = 4
        cn.Close()
    End Sub
    Function GetMaxAuftragNr() As Integer
        'finde max auftragsnummer
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim dbCmd As New OleDbCommand
        'Dim dbda As OleDbDataAdapter
        'dbda = New OleDbDataAdapter
        dbCmd.Connection = cn
        dbCmd.CommandText = "select MAX(XAUFTR_NR) from RECH1"
        Dim MaxAuftrage As Integer
        MaxAuftrage = CInt(dbCmd.ExecuteScalar())
        cn.Close()
        'change in 2.0.0.3
        'removed in 2.0.0.4
        'If (MaxAuftrage < 1007930) Then
        '    MaxAuftrage = 1007930
        'End If
        Return MaxAuftrage
    End Function
    Private Sub XAuftr_Nr_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAuftragNummer.GotFocus
        markAuftrag_NR()
    End Sub
    ''' <summary>
    ''' search Kunde for a given Kunden_nr
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub KundeSuchen()
        Try
            Kundenliste.Kunden_nr = CDbl(txtXKundenNr.Text)
        Catch ex As Exception
            Kundenliste.Kunden_nr = 0
        End Try
        Kundenliste.ShowDialog()

        If Kundenliste.Kunden_nr <> 0 Then
            txtXKundenNr.Text = CStr(Kundenliste.Kunden_nr)
            ReadKunde(Kundenliste.Kunden_nr)
            'Clear Fahrzeug
            If FGSTLLNR.Text <> "999999" Then
                Dim mycontrol As Control
                For Each mycontrol In mainmodul.GetAllControls(Me)
                    If Not IsNothing(mycontrol.Tag) Then
                        If mycontrol.Tag.ToString = """fahrzeug""" Then mycontrol.Text = ""
                        If mycontrol.Tag.ToString = """fahrzeugVars""" Then mycontrol.Text = ""
                    End If
                Next
            End If
            DataChanged(True)
            FGSTLLNR.Focus()
            FGSTLLNR.Enabled = True
            btnFahrzeugSuchen.Enabled = True
        Else
            txtXKundenNr.Focus()
        End If
        Kundenliste.Close()
    End Sub
    Private Sub AuftragSuchen()
        Try
            Auftragsliste.auftrag_nr = CDbl(txtAuftragNummer.Text)
        Catch ex As Exception
            Auftragsliste.auftrag_nr = 0
        End Try
        Auftragsliste.ShowDialog()
        If Auftragsliste.auftrag_nr <> 0 Then
            txtAuftragNummer.Text = CStr(Auftragsliste.auftrag_nr)
            checkAuftragsnummer()
        End If
        markAuftrag_NR()
        Auftragsliste.Close()
    End Sub
    Private Sub markAuftrag_NR()
        txtAuftragNummer.SelectionStart = 0
        txtAuftragNummer.SelectionLength = txtAuftragNummer.Text.Length
    End Sub

    Private Sub XAuftr_Nr_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAuftragNummer.KeyUp
        If e.KeyCode = Keys.F3 Then
            AuftragSuchen()
        End If
        If e.KeyCode = Keys.Enter Then
            checkAuftragsnummer()
        End If
        If e.KeyCode = Keys.F4 Then
            txtAuftragNummer.Text = CStr(GetMaxAuftragNr() + 1)
            checkAuftragsnummer()
        End If
    End Sub
    Private Sub GedrucktChanged()
        If Not chkGedruckt.Checked Then
            enableKontrols(Me, """kunde""", True)
            enableKontrols(Me, """fahrzeug""", True)
            enableKontrols(Me, """auftrag""", True)
            Status.Text = "Auftrag geladen"
            'DataGridView1.AllowUserToAddRows = True
            DataGridView1.ReadOnly = False
            btnFahrzeugSuchen.Enabled = True
        Else
            enableKontrols(Me, """kunde""", False)
            enableKontrols(Me, """fahrzeug""", False)
            enableKontrols(Me, """auftrag""", False)
            Status.Text = "Gedruckter Auftrag geladen"
            'DataGridView1.AllowUserToAddRows = False
            DataGridView1.ReadOnly = True
            btnFahrzeugSuchen.Enabled = False
        End If
        If chkGedruckt.Checked Then
            enableKontrols(Me, """fahrzeugVars""", False)
        Else
            enableKontrols(Me, """fahrzeugVars""", True)
        End If
        PanelArtikel.Enabled = Not chkGedruckt.Checked

    End Sub
    ''' <summary>
    ''' will test auftragsnummer
    ''' try to read auftrag with txtAuftragNummer
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub checkAuftragsnummer()
        Status.Text = "Prüfe Auftragsnummer..."
        Dim nummer As Double
        Try
            nummer = CDbl(txtAuftragNummer.Text)
            If ReadAuftrag(nummer) > 0 Then
                'Alter Auftrag
                btnDrucken.Enabled = True
                btnAuftragDrucken.Enabled = True
                btnKVdrucken.Enabled = True
                btnLieferscheinDrucken.Enabled = True
                btnGutschriftDrucken.Enabled = True
                isNeuerAuftrag = False
                chkIsNeuerAuftrag.Checked = isNeuerAuftrag
                'enable disable controls
                GedrucktChanged()
            Else
                'Neuer Auftrag
                btnDrucken.Enabled = False
                btnGutschriftDrucken.Enabled = False
                btnAuftragDrucken.Enabled = False
                btnKVdrucken.Enabled = False
                btnLieferscheinDrucken.Enabled = False
                isNeuerAuftrag = True
                chkIsNeuerAuftrag.Checked = isNeuerAuftrag
                If nummer = GetMaxAuftragNr() + 1 Then
                    enableKontrols(Me, """kunde""", False)
                    enableKontrols(Me, """fahrzeug""", False)
                    clearKontrols(Me, """kunde""")
                    clearKontrols(Me, """fahrzeug""")
                    clearKontrols(Me, """auftrag""")
                    DataGridView1.DataSource = Nothing
                    FGSTLLNR.Enabled = True
                    txtXKundenNr.Enabled = True
                    'knöppe freischalten
                    btnKundeSuchen.Enabled = True
                    btnFahrzeugSuchen.Enabled = True
                    'btnDrucken.Enabled = False
                    Status.Text = "Neuer Auftrag mit Nummer " & nummer
                    DataGridView1.AllowUserToAddRows = True
                    DataChanged(False)
                Else
                    nummer = GetMaxAuftragNr() + 1
                    Status.Text = "Neuen Auftrag mit Nummer " & nummer & " anlegen"
                End If
            End If
        Catch ex As Exception
            nummer = 0
            Status.Text = "Fehler in CheckAufgtragNummer:" & vbCrLf & ex.Message
        End Try
        txtAuftragNummer.Text = CStr(nummer)

    End Sub
    Private Sub showStatus(ByVal s As String)
        StatusStrip1.Items(0).ForeColor = Color.Black
        StatusStrip1.Items(0).Text = s
    End Sub
    Private Sub showStatus(ByVal s As String, ByVal warn As Boolean)
        StatusStrip1.Items(0).Text = s
        If warn Then
            StatusStrip1.Items(0).BackColor = Color.Red
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'SaveChanged(true)
        If mIsChanged = False Then
            Me.Close()
        Else
            If mIsChanged = True Then
                If MessageBox.Show("Auftragsfenster schliessen und Änderungen verwerfen?", _
                                    "Änderungen nicht gespeichert", _
                                    MessageBoxButtons.YesNo, _
                                    MessageBoxIcon.Warning, _
                                    MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    Me.Close()
                End If
            End If
        End If
    End Sub
    ''' <summary>
    ''' CalcNetto will be executed by ReadAuftrag only
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CalcNetto() As Integer
        '"AUFTR_NR, ARTIKELNR, ARTIKELBEZ, KONTO, MENGE, E_PREIS, RABATT, LOHNART, IST_AW, MITARBCODE, FAHRZEUG, TEXT"
        'calculate the netto field
        Dim xnetto As Double, gesamt As Double
        'total = RechnungenRechinhalt_Dataset1.RECHINHALT.Compute("sum (ges_preis)", "AUFTR_NR=" & auftragsnummer)
        Dim mwst As Double
        Dim queryString As String = ""
        Dim Anzahl As Integer = 0
        Dim connection As New OleDbConnection()
        Dim o As Object
        Try
            Dim cmd As New OleDbCommand() '(queryString, connection)
            OpenDBConnection(connection)
            cmd.Connection = connection
            ''is there any data?
            'queryString = "select * from rech2 where AUFTR_NR=" & txtAuftragNummer.Text
            'Dim cmd1 As New OleDbCommand(queryString, connection)
            'Dim rd As OleDbDataReader = cmd1.ExecuteReader
            'Anzahl = cmd1.ExecuteNonQuery()
            'If Not rd.HasRows Then
            'CalcNetto = 0
            '    connection.Close()
            '    Exit Function
            'End If
            CalcNetto = 1 'Anzahl

            'Gesamt berechnen, Netto egal ob AltTeil oder Normal
            'queryString = "select sum (menge * e_preis - (RABATT/100 * (menge * e_preis))) from rech2 where AUFTR_NR=" & txtAuftragNummer.Text
            'Gesamt NETTO berechnen mit Altteile! version 2.1.0.0
            queryString = "select sum (menge * e_preis - (RABATT/100 * (menge * e_preis))) from rech2 " & _
                            " where AUFTR_NR=" & txtAuftragNummer.Text '& " AND NOT ArtTyp=3"
            cmd.CommandText = queryString
            o = cmd.ExecuteScalar
            If Not IsDBNull(o) Then
                xnetto = CDbl(o)
            Else
                xnetto = 0
            End If

            'Altteile berechnen
            queryString = "select sum (menge * e_preis - (RABATT/100 * (menge * e_preis))) from rech2" & _
                          " where AUFTR_NR=" & txtAuftragNummer.Text & _
                          " and ArtTyp=3"
            cmd.CommandText = queryString
            o = cmd.ExecuteScalar
            'changed for version 2.1.0.0
            Dim xnettoAltteile = 0
            If Not IsDBNull(o) Then
                xnettoAltteile = CDbl(o)
                txtXSONDER.Text = String.Format("{0:0.00}", xnettoAltteile)
                xnettoAltteile = String.Format("{0:0.00}", CDbl(o))
                'txtAltTeilMwst.Text = CStr(CDbl(txtAltTeilMwStSatz.Text) * CDbl(txtXSONDER.Text))
            Else
                txtXSONDER.Text = "0"
                txtAltTeilMwst.Text = "0"
            End If

            'Die MwSt über alles MIT Altteile!
            txtXNetto.Text = String.Format("{0:0.00}", xnetto)
            mwst = xnetto / 100 * CDbl(txtMwStSatz.Text) 'changed back in 2.1.3.0
            txtMwStBetrag.Text = String.Format("{0:0.00}", mwst)

            Dim AltTeileMwst As Double
            AltTeileMwst = CDbl(txtXSONDER.Text) / 100 * CDbl(txtAltTeilMwStSatz.Text)
            txtAltTeilMwst.Text = String.Format("{0:0.00}", AltTeileMwst)

            gesamt = xnetto + mwst + AltTeileMwst
            txtGesamtbetrag.Text = String.Format("{0:0.00}", gesamt)

            'schmiermittel berechnen
            queryString = "select sum (menge * e_preis - (RABATT/100 * (menge * e_preis))) from rech2" & _
                          " where AUFTR_NR=" & txtAuftragNummer.Text & _
                          " and ArtTyp=1"
            cmd.CommandText = queryString
            o = cmd.ExecuteScalar
            If Not IsDBNull(o) Then
                txtXSCHMIER.Text = String.Format("{0:0.00}", CDbl(o))
            Else
                txtXSCHMIER.Text = "0"
            End If

            'Lohn berechnen
            queryString = "select sum (menge * e_preis - (RABATT/100 * (menge * e_preis))) from rech2" & _
                          " where AUFTR_NR=" & txtAuftragNummer.Text & _
                          " and ArtTyp=2"
            cmd.CommandText = queryString
            o = cmd.ExecuteScalar
            If Not IsDBNull(o) Then
                txtXLOHN.Text = String.Format("{0:0.00}", CDbl(o))
            Else
                txtXLOHN.Text = "0"
            End If

        Catch ex As OleDbException
            MessageBox.Show("Fehler " & ex.Message & vbCrLf & queryString, "CalcNetto")
        Catch ex As ArgumentException
            MessageBox.Show("Fehler " & ex.Message & vbCrLf & queryString, "CalcNetto")
        Catch ex As Exception
            MessageBox.Show("Fehler " & ex.Message & vbCrLf & queryString, "CalcNetto")
        Finally
            connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' check if a valid Kunde
    ''' </summary>
    ''' <param name="KundenNr"></param>
    ''' <returns>true for Kunde is valid
    ''' false for invalid Kunde</returns>
    ''' <remarks></remarks>
    Private Function ExistsKunde(ByVal KundenNr As Double) As Boolean
        Dim queryString As String
        Dim Anzahl As Integer = 0
        'freie Kundeneingabe
        If txtXKundenNr.Text = "999999" Then Return True
        queryString = "Select * from KundenSt where KundenNr=" & KundenNr
        Dim connection As New OleDbConnection()
        Dim command As New OleDbCommand(queryString, connection)
        OpenDBConnection(connection)
        Dim reader As OleDbDataReader = command.ExecuteReader()
        If reader.HasRows Then
            reader.Close()
            connection.Close()
            Return True
        Else
            reader.Close()
            connection.Close()
            Return False
        End If
    End Function
    ''' <summary>
    ''' Fill the Kunden fields with data from KundenSt
    ''' </summary>
    ''' <param name="KundenNr"></param>
    ''' <returns>number of found data rows</returns>
    ''' <remarks></remarks>
    Private Function ReadKunde(ByVal KundenNr As Double) As Integer
        'tries to read the kunde and returns the number of found rows
        '
        '"KUNDENNR, ANREDE , VORNAME , NACHNAME, BRANCHE, PLZ , ORT , STRASSE, 
        'ANSPRECHP, TELEFON , KONTO1"
        Dim queryString As String
        Dim Anzahl As Integer = 0
        'freie Kundeneingabe
        If txtXKundenNr.Text = "999999" Then Return 1
        queryString = "Select * from KundenSt where KundenNr=" & KundenNr
        Dim connection As New OleDbConnection()
        Dim anredeNr As Double
        Dim command As New OleDbCommand(queryString, connection)
        OpenDBConnection(connection)
        Dim reader As OleDbDataReader = command.ExecuteReader()
        If reader.HasRows Then
            reader.Read() 'we are only interested in the first (and only!?) row
            Anzahl += 1
            anredeNr = CDbl(reader("Anrede").ToString)
            txtXKundenNr.Text = reader("KUNDENNR").ToString
            txtXName1.Text = reader("VORNAME").ToString
            txtXName2.Text = reader("NACHNAME").ToString
            txtXST.Text = reader("STRASSE").ToString
            txtXPL.Text = reader("PLZ").ToString
            txtXOT.Text = reader("ORT").ToString
            txtXTel.Text = reader("Telefon").ToString
            'Console.WriteLine(reader(0).ToString())
        End If

        'Anrede-ID durch Text ersetzen
        AnredenListBox.SelectedIndex = CInt(anredeNr)
        txtXAN1.Text = anredeNr.ToString 'AnredenListBox.SelectedItem.ToString

        reader.Close()
        connection.Close()
        'lock the kunden fields
        If (Anzahl > 0) Then
            enableKontrols(Me, """kunde""", False)
            txtXKundenNr.Enabled = True
            btnKundeSuchen.Enabled = True
        End If

        ReadKunde = Anzahl
    End Function
    Private Function ReadAuftrag(ByVal AUFTR_Nr As Double) As Integer
        Dim queryString As String
        Dim Anzahl As Integer = 0
        'XAUFTR_NR, XKUNDENNR, XAN1, XNAME1, XNAME2, XST, XPL, XOT, 
        'XFGSTLLNR, XDATUM, 
        'XNETTO, XAN, XSCHMIER, XLOHN, XSONDER, XKMSTAND, XKZ, XZULASS, XTYP, XMWS, XTEL
        'gedruckt, werkdatum
        queryString = "Select * from Rech1 where XAUFTR_Nr=" & AUFTR_Nr
        Dim connection As New OleDbConnection
        Try
            lblGutschrift.Visible = False
            OpenDBConnection(connection)
            Dim anredeNr As Double
            Dim command As New OleDbCommand(queryString, connection)
            'connection.Open() is already opened with OpenDBConnection
            Dim reader As OleDbDataReader = command.ExecuteReader()
            reader.Read()
            If reader.HasRows Then
                Anzahl += 1
                If reader("Gutschrift") = dbTrue Then
                    lblGutschrift.Visible = True
                End If
                idAuftrag.Text = reader("id").ToString
                anredeNr = CDbl(reader("XAn1").ToString)
                'Anrede-ID durch Text ersetzen
                AnredenListBox.SelectedIndex = CInt(anredeNr)
                txtXAN1.Text = anredeNr.ToString 'AnredenListBox.SelectedItem.ToString

                txtXKundenNr.Text = reader("XKUNDENNR").ToString
                txtXName1.Text = reader("XNAME1").ToString
                txtXName2.Text = reader("XNAME2").ToString
                txtXST.Text = reader("XST").ToString
                txtXPL.Text = reader("XPL").ToString
                txtXOT.Text = reader("XOT").ToString
                txtXTel.Text = reader("XTEL").ToString

                txtDatum.Text = reader("XDATUM").ToString
                txtWerkdatum.Text = reader("werkdatum").ToString
                txtLeistDatum.Text = reader("LDatum").ToString ' new with version 2.0.3.0

                chkGedruckt.Checked = CBool(reader("Gedruckt").ToString)
                GedrucktChanged()

                txtXSCHMIER.Text = String.Format("{0:0.00}", reader("XSCHMIER"))
                txtXLOHN.Text = String.Format("{0:0.00}", reader("XLOHN"))
                txtXSONDER.Text = String.Format("{0:0.00}", reader("XSONDER"))

                txtXZULASS.Text = reader("XZULASS").ToString
                txtXTYP.Text = reader("XTYP").ToString

                'txtMwStSatz.Text = reader("XMWS").ToString 'we dont read this, we calc this
                'BUT we need the actual MwSt-Satz!
                txtMwStSatz.Text = String.Format("{0:0.00}", reader("MWSTsatz"))
                txtAltTeilMwStSatz.Text = String.Format("{0:0.00}", reader("AltteilMwst")) '.ToString 'version 2.1.0.0

                TextBox1.Text = reader("XNETTO").ToString 'only for testing!

                FGSTLLNR.Text = reader("XFGSTLLNR").ToString
                txtXKZ.Text = reader("XKZ").ToString
                'Neue Fahrzeugdaten lesen
                readFahrzeug(reader("XFGSTLLNR").ToString)
                txtXKMSTAND.Text = reader("XKMSTAND").ToString
                If txtXKMSTAND.Text.Length = 0 Then txtXKMSTAND.Text = "0"

                If CalcNetto() > 0 Then
                    '"AUFTR_NR, ARTIKELNR, ARTIKELBEZ, KONTO, MENGE, E_PREIS, RABATT, LOHNART, IST_AW, MITARBCODE, FAHRZEUG, TEXT"
                    '+ Pos und ArtTyp
                    kbfakt_start.FillGrid(DataGridView1, "Select Pos, ARTIKELNR, ARTIKELBEZ, ArtTyp, MENGE, E_PREIS, RABATT, " + _
                    "(menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt from RECH2 where AUFTR_NR=" & AUFTR_Nr & " order by pos")
                    DataGridView1.RowTemplate.Height = 44 'changed in 2.0.0.5 (see Designer)
                    DataGridView1.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True 'changed in 2.0.0.5 (see Designer)
                    DataGridView1.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft 'changed in 2.0.0.5 (see Designer)

                    PositionenAktualisieren()
                End If

                'Console.WriteLine(reader(0).ToString())
                'Knöppe freischalten

            End If
            reader.Close()
            DataChanged(False)
        Catch ex As OleDbException
            'Catch ex As Exception
            Status.Text = ex.Message & vbCrLf & "ReadAuftrag(" & AUFTR_Nr & ")"
        Finally
            connection.Close()
        End Try
        ReadAuftrag = Anzahl
    End Function
    ''' <summary>
    ''' check if there is data saved for a given FGSTLLNR number
    ''' </summary>
    ''' <param name="FahrzNr"></param>
    ''' <returns>true, if data found
    ''' false, if no data found</returns>
    ''' <remarks></remarks>
    Private Function ExistsFahrzeug(ByVal FahrzNr As String) As Boolean
        'removed in 2.0.0.6: If FahrzNr = "999999" Then Return True
        If (txtXKundenNr.Text = "999999") Then Return True 'added in 2.0.0.6

        Dim queryString As String
        Dim Anzahl As Integer = 0
        '"KUNDEN_NR, FGSTLLNR, TYP, KENNZEICH, KM_STAND, KFZ_BRF_NR, 
        'ZULASSUNG1, TUEV, ASU,  SCHREIBER,  SICHER"
        queryString = "Select * from KUNDFAHR where FGSTLLNR='" & FahrzNr & "'"
        Dim connection As New OleDbConnection
        OpenDBConnection(connection)
        Dim command As New OleDbCommand(queryString, connection)
        'connection.Open() is already opened with OpenDBConnection
        Dim reader As OleDbDataReader = command.ExecuteReader()
        reader.Read()
        If reader.HasRows Then
            reader.Close()
            connection.Close()
            Return True
        Else
            reader.Close()
            connection.Close()
            Return False
        End If
    End Function
    ''' <summary>
    ''' read the data of a Fahrzeug and fill in the fields
    ''' </summary>
    ''' <param name="FahrzNr"></param>
    ''' <returns>number of matching data rows</returns>
    ''' <remarks></remarks>
    Private Function readFahrzeug(ByVal FahrzNr As String) As Integer
        'removed 2.0.0.6 If FahrzNr = "999999" Then Return 1
        If txtXKundenNr.Text = "999999" Then Return 1 'added in 2.0.0.6
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
        FGSTLLNR.Text = FahrzNr
        'idFahrzeug.Text = ""
        'txtXKZ.Text = ""
        'txtXTYP.Text = ""
        'txtXKMSTAND.Text = ""
        'txtXZULASS.Text = ""
        'TUEV.Text = ""
        'txtSchreiber.Text = ""
        'txtASU.Text = ""
        'txtSicher.Text = ""

        Try
            OpenDBConnection(connection)
            Dim command As New OleDbCommand(queryString, connection)
            'connection.Open() is already opened with OpenDBConnection
            Dim reader As OleDbDataReader = command.ExecuteReader()
            reader.Read()
            If reader.HasRows Then
                Anzahl += 1
                'FGSTLLNR.Text = reader("FGSTLLNR").ToString
                idFahrzeug.Text = reader("id").ToString
                txtXKZ.Text = reader("KENNZEICH").ToString
                txtXTYP.Text = reader("TYP").ToString
                txtXKMSTAND.Text = reader("KM_STAND").ToString
                txtXZULASS.Text = reader("ZULASSUNG1").ToString
                TUEV.Text = reader("TUEV").ToString
                txtSchreiber.Text = reader("SCHREIBER").ToString
                txtASU.Text = reader("ASU").ToString
                txtSicher.Text = reader("Sicher").ToString

                'Console.WriteLine(reader(0).ToString())
            End If
            reader.Close()
        Catch ex As OleDbException
            MessageBox.Show(ex.Message, "readFahrzeug(" & FahrzNr & ")")
        Finally
            connection.Close()
        End Try
        If Anzahl > 0 Then
            enableKontrols(Me, """fahrzeug""", False)
            'removed in 2.0.0.8
            'FGSTLLNR.Enabled = True
            'btnFahrzeugSuchen.Enabled = True
        End If
        readFahrzeug = Anzahl

    End Function

    Private Sub txtXKundenNr_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtXKundenNr.KeyUp
        If e.KeyCode = Keys.F3 Or e.KeyCode = Keys.Enter Then
            KundeSuchen()
        End If
    End Sub
    ''' <summary>
    ''' searches Fahrzeug database for a given Kunden_Nr
    ''' </summary>
    ''' <returns>fahrgestell_nr of selected Fahrzeug</returns>
    ''' <remarks>uses Fahrzeugliste dialog</remarks>
    Private Function FahrzeugSuchen() As String
        If txtXKundenNr.Text = "999999" Then Return ""
        'Do we have a KundenNr?
        Try
            Fahrzeugliste.Kunden_nr = CDbl(txtXKundenNr.Text)
        Catch ex As Exception
            Fahrzeugliste.Kunden_nr = 0
        End Try
        Fahrzeugliste.ShowDialog()
        If Fahrzeugliste.fahrgestell_nr <> "" Then
            FGSTLLNR.Text = CStr(Fahrzeugliste.fahrgestell_nr)
            readFahrzeug(Fahrzeugliste.fahrgestell_nr)
        End If
        'if there is not already a Kunde assigned
        If Fahrzeugliste.Kunden_nr <> 0 And txtXKundenNr.Text = "" Then ReadKunde(Fahrzeugliste.Kunden_nr)
        Return Fahrzeugliste.fahrgestell_nr
    End Function
    Private Sub FGSTLLNR_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles FGSTLLNR.KeyUp
        If e.KeyCode = Keys.F3 Or e.KeyCode = Keys.Enter Then
            FahrzeugSuchen()
        End If

    End Sub

    Private Sub txtXKundenNr_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtXKundenNr.TextChanged
        If txtXKundenNr.Text = "999999" Then
            enableKontrols(Me, """kunde""", True)
            enableKontrols(Me, """fahrzeug""", True)
        Else
            enableKontrols(Me, """kunde""", False)
            enableKontrols(Me, """fahrzeug""", False)
            txtXKundenNr.Enabled = True
        End If
        txtXKundenNr.Focus()
    End Sub

    Private Sub btnAuftragSuchen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuftragSuchen.Click, btnAufSuchen2.Click
        If mIsChanged Then
            Status.Text = "Bitte erst Daten speichern oder verwerfen"
        Else
            AuftragSuchen()
        End If

    End Sub

    Private Sub btnKundeSuchen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKundeSuchen.Click, btnKundeSuchen2.Click
        KundeSuchen()
    End Sub

    Private Sub btnFahrzeugSuchen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFahrzeugSuchen.Click
        'added if block in 2.0.0.6
        If txtXKundenNr.Text = "999999" Then
            Beep()
            Status.Text = "Keine Fahrzeugsuche bei Einmalkunden!"
        Else
            FahrzeugSuchen()
        End If
    End Sub

    Private Sub txtAuftragNummer_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAuftragNummer.Enter
        oldStatus = Status.Text
        Status.Text = "F3 zum Suchen, F4 für neuen Auftrag, Eingabe=Auftrag aufrufen"
    End Sub
    Private oldStatus As String = "Status"
    Private Sub txtAuftragNummer_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAuftragNummer.Leave
        Status.Text = oldStatus
    End Sub

    Private Sub txtXKundenNr_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtXKundenNr.Enter
        oldStatus = Status.Text
        Status.Text = "F3 zum Suchen, Eingabe=Kunde abfrufen"

    End Sub

    Private Sub txtXKundenNr_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtXKundenNr.Leave
        Status.Text = oldStatus
    End Sub

    Private Sub chkGedruckt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGedruckt.CheckedChanged
        btnSpeichern.Enabled = Not chkGedruckt.Checked
        GedrucktChanged()
    End Sub
    Private Sub NeuerAuftrag()
        lblGutschrift.Visible = False
        txtAuftragNummer.Text = CStr(GetMaxAuftragNr() + 1)
        checkAuftragsnummer()
        readFirmstamm()
        txtMwStSatz.Text = String.Format("{0:0.00}", gMwSt)
        txtAltTeilMwStSatz.Text = String.Format("{0:0.00}", gAltteilMwSt) 'version 2.1.0.0
        ClearEntries()

        AddEmptyLine()
        DataChanged(False)
        chkGedruckt.Checked = False
        txtXKZ.Enabled = True
    End Sub
    Private Sub ClearEntries()
        For Each ctrl As Control In Me.Controls
            If TypeOf (ctrl) Is TextBox Then
                If ctrl.Tag = """kunde""" Or ctrl.Tag = """fahrzeug""" Or ctrl.Tag = """fahrzeugVars""" Then
                    ctrl.Text = ""
                End If
            End If
        Next
    End Sub
    Private Sub btnNeuerAuftrag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNeuerAuftrag.Click, btnAuftragNeu2.Click
        If mIsChanged Then
            Status.Text = "Bitte erst Daten speichern oder verwerfen"
        Else
            NeuerAuftrag()
        End If
    End Sub

    Private Sub btnAbbrechen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbbrechen.Click
        If mIsChanged Then
            If MessageBox.Show("Wirklich abbrechen?", "Auftrag bearbeiten", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                NeuerAuftrag()
            End If
        Else
            NeuerAuftrag()
        End If
    End Sub

    Private Sub btnSpeichern_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSpeichern.Click
        'bla bla bla ....
        If chkGedruckt.Checked Then
            Status.Text = "Gedruckte Aufträge können nicht erneut gespeichert werden"
            MessageBox.Show(Status.Text) ' new in 2.0.1.6
            Exit Sub
        End If
        If Not DatenPruefen() Then Exit Sub
        If ExistsKunde(CDouble(txtXKundenNr.Text)) And ExistsFahrzeug(FGSTLLNR.Text) Then
            'changed in 2.0.0.8:
            If Not ExistsData("select XAUFTR_NR from RECH1 where XAUFTR_NR=" + txtAuftragNummer.Text) Then 'chkIsNeuerAuftrag.Checked 
                If AuftragNeuSpeichern() > 0 Then DataChanged(False)
            Else
                If AuftragAltSpeichern() > 0 Then DataChanged(False)
            End If
            PositionenSpeichern()
            'Einmalkunde? changed in 2.0.0.6
            If (txtXKundenNr.Text <> "999999") Then
                If FahrzeugBuchen(FGSTLLNR.Text, CLong(txtXKMSTAND.Text), txtASU.Text, TUEV.Text, txtSicher.Text, txtSchreiber.Text) Then
                    'Alles OK
                    DataChanged(False)
                    btnDrucken.Enabled = True
                    btnAuftragDrucken.Enabled = True
                    btnGutschriftDrucken.Enabled = True
                    btnKVdrucken.Enabled = True
                    btnLieferscheinDrucken.Enabled = True
                    'txtAuftragNummer.Text = (GetMaxAuftragNr() + 1).ToString
                    ReadAuftrag(CDouble(txtAuftragNummer.Text))
                Else
                    MessageBox.Show("Konnte Fahrzeugdaten nicht einbuchen")
                End If
            Else
                DataChanged(False)
                btnDrucken.Enabled = True
                btnAuftragDrucken.Enabled = True
                btnKVdrucken.Enabled = True
                btnGutschriftDrucken.Enabled = True
                btnLieferscheinDrucken.Enabled = True
                'txtAuftragNummer.Text = (GetMaxAuftragNr() + 1).ToString
                ReadAuftrag(CDouble(txtAuftragNummer.Text))
            End If

        Else
            Status.Text = "Kunde oder Fahrzeug ungültig"
            'sorry, keine daten = kein speichern
            btnDrucken.Enabled = False
            btnAuftragDrucken.Enabled = False
            btnGutschriftDrucken.Enabled = False
            btnKVdrucken.Enabled = False
            btnLieferscheinDrucken.Enabled = False
        End If
    End Sub
    Private Function DatenPruefen() As Boolean
        Dim AnzFehler As Integer
        AnzFehler = 0
        DatenPruefen = False
        If DataGridView1.Rows.Count = 0 Then
            DataGridView1.BackgroundColor = Color.LightPink
            AnzFehler = AnzFehler + 1
        Else
            DataGridView1.BackgroundColor = Color.White
        End If
        Dim d As Double = CDbl(txtAuftragNummer.Text)
        If d > GetMaxAuftragNr() + 1 Then txtAuftragNummer.BackColor = Color.LightPink
        'Kunde
        If Not CheckTextLength(txtXKundenNr) Then AnzFehler = AnzFehler + 1
        If Not CheckTextInt(txtXAN1) Then
            AnzFehler = AnzFehler + 1
            AnredenListBox.BackColor = Color.LightPink 'new in 2.0.0.6
        Else
            AnredenListBox.BackColor = Color.White 'new in 2.0.0.6
        End If
        If Not CheckTextLength(txtXName1) Then AnzFehler = AnzFehler + 1
        If Not CheckTextLength(txtXName2) Then AnzFehler = AnzFehler + 1
        If Not CheckTextLength(txtXST) Then AnzFehler = AnzFehler + 1
        If Not CheckTextLength(txtXPL) Then AnzFehler = AnzFehler + 1
        If Not CheckTextLength(txtXOT) Then AnzFehler = AnzFehler + 1
        'If Not CheckTextLength(txtXTel) Then AnzFehler = AnzFehler + 1

        'Fahrzeug
        If Not CheckTextLength(FGSTLLNR) Then AnzFehler = AnzFehler + 1
        If Not CheckTextLength(txtXKZ) Then AnzFehler = AnzFehler + 1
        If Not CheckTextLong(txtXKMSTAND) Then AnzFehler = AnzFehler + 1

        'If Not CheckTextDatum(txtXZULASS) Then AnzFehler = AnzFehler + 1

        If Not CheckTextLength(txtXTYP) Then AnzFehler = AnzFehler + 1

        If Not CheckMonatJahr(TUEV) Then AnzFehler = AnzFehler + 1
        If Not CheckMonatJahr(txtASU) Then AnzFehler = AnzFehler + 1
        If Not CheckMonatJahr(txtSchreiber) Then AnzFehler = AnzFehler + 1
        If Not CheckMonatJahr(txtSicher) Then AnzFehler = AnzFehler + 1

        ''Auftrag
        'drw1("XNETTO") = CDouble(txtXNetto.Text) 'überflüssig
        'drw1("XSCHMIER") = CDouble(txtXSCHMIER.Text) 'überflüssig
        'drw1("XLOHN") = CDouble(txtXLOHN.Text) 'überflüssig
        'drw1("XSONDER") = CDouble(txtXSONDER.Text) 'überflüssig
        'drw1("XDATUM") = CDatum(txtDatum.Text)
        'drw1("WerkDATUM") = CDatum(txtWerkdatum.Text)
        'drw1("MWSTsatz") = CDouble(txtMwStSatz.Text)
        'drw1("gedruckt") = chkGedruckt.Checked

        If AnzFehler = 0 Then
            DatenPruefen = True
        Else
            DatenPruefen = False
            Status.Text = "Bitte Eingabefelder vervollständigen!"
        End If
    End Function
    Private Function AuftragAltSpeichern() As Integer
        'rech1
        '"XAUFTR_NR, XKUNDENNR, XAN1, XNAME1, XNAME2, XST, XPL, XOT, XFGSTLLNR, XDATUM, XNETTO, XAN, XSCHMIER, XLOHN, XSONDER, XKMSTAND, XKZ, XZULASS, XTYP, XMWS, XTEL"
        'plus id, gedruckt, werkdatum, MWSTsatz, aber ohne: XAS, XEND, XM, , XFABR
        '### rechnungs positionen
        '"AUFTR_NR, ARTIKELNR, ARTIKELBEZ, KONTO, MENGE, E_PREIS, RABATT, LOHNART, IST_AW, MITARBCODE, FAHRZEUG, TEXT"
        'plus Pos, id
        'we need a connection, a dataset and a dataAdapter
        Dim anzahl As Integer
        Dim cn As New OleDbConnection
        OpenDBConnection(cn)
        Dim cmd As New OleDbCommand("select * from rech1", cn)
        Dim ds As New DataSet
        Dim da As New OleDbDataAdapter
        Dim cmb As New OleDbCommandBuilder(da)
        da.SelectCommand = cmd
        da.Fill(ds, "RECH1")

        'Add the row locally
        Dim AuftragID As Long = CLong(idAuftrag.Text)
        If AuftragID = 0 Then
            AuftragAltSpeichern = 0
            Exit Function
        End If
        'add the primary key to the dataset
        Dim dcs(1) As DataColumn
        dcs(0) = ds.Tables("RECH1").Columns("id")
        ds.Tables("Rech1").PrimaryKey = dcs

        Dim drw1 As DataRow = ds.Tables("Rech1").Rows.Find(AuftragID)
        '        drw1("ID") = Int32.MaxValue
        drw1("XAUFTR_NR") = CDbl(txtAuftragNummer.Text)
        'Kunde
        drw1("XKUNDENNR") = txtXKundenNr.Text
        drw1("XAN1") = CInt(txtXAN1.Text)
        drw1("XNAME1") = txtXName1.Text
        drw1("XNAME2") = txtXName2.Text
        drw1("XST") = txtXST.Text
        drw1("XPL") = txtXPL.Text
        drw1("XOT") = txtXOT.Text
        drw1("XTEL") = txtXTel.Text

        'Fahrzeug
        drw1("XFGSTLLNR") = FGSTLLNR.Text
        drw1("XKZ") = txtXKZ.Text
        drw1("XKMSTAND") = CLong(txtXKMSTAND.Text)
        drw1("XZULASS") = CDatum(txtXZULASS.Text)
        drw1("XTYP") = txtXTYP.Text
        'drw1("XTUEV") = CDatum(TUEV.Text) 'TÜV kommt aus FAHRZEUG DB

        'Auftrag
        drw1("XNETTO") = CDouble(txtXNetto.Text) 'überflüssig
        drw1("XSCHMIER") = CDouble(txtXSCHMIER.Text) 'überflüssig
        drw1("XLOHN") = CDouble(txtXLOHN.Text) 'überflüssig
        'wichtig fuer AltteilMwSt
        drw1("XSONDER") = CDouble(txtXSONDER.Text)

        drw1("XDATUM") = CDatum(txtDatum.Text)
        drw1("WerkDATUM") = CDatum(txtWerkdatum.Text)
        drw1("LDatum") = CDatum(txtLeistDatum.Text) 'new with 2.0.3.0
        drw1("MWSTsatz") = CDouble(txtMwStSatz.Text)
        drw1("AltteilMWST") = CDouble(txtAltTeilMwStSatz.Text)
        drw1("gedruckt") = chkGedruckt.Checked

        'ds.Tables("Rech1").Rows.Add(drw1) ' we down add a row,  we update it
        'Update the Access database file
        anzahl = da.Update(ds, "Rech1")
        ds.AcceptChanges()
        cn.Close()
        AuftragAltSpeichern = anzahl
        Status.Text = anzahl & " Auftrag/Aufträge gespeichert"

    End Function
    Private Function AuftragNeuSpeichern() As Integer
        'rech1
        '"XAUFTR_NR, XKUNDENNR, XAN1, XNAME1, XNAME2, XST, XPL, XOT, XFGSTLLNR, XDATUM, XNETTO, XAN, XSCHMIER, XLOHN, XSONDER, XKMSTAND, XKZ, XZULASS, XTYP, XMWS, XTEL"
        'plus id, gedruckt, werkdatum, MWSTsatz, aber ohne: XAS, XEND, XM, , XFABR
        '### rechnungs positionen
        '"AUFTR_NR, ARTIKELNR, ARTIKELBEZ, KONTO, MENGE, E_PREIS, RABATT, LOHNART, IST_AW, MITARBCODE, FAHRZEUG, TEXT"
        'plus Pos, id
        'we need a connection, a dataset and a dataAdapter
        Dim anzahl As Integer
        Dim cn As New OleDbConnection
        OpenDBConnection(cn)
        Dim cmd As New OleDbCommand("select * from rech1", cn)
        Dim ds As New DataSet
        Dim da As New OleDbDataAdapter
        Dim cmb As New OleDbCommandBuilder(da)
        da.SelectCommand = cmd
        da.Fill(ds, "RECH1")

        'add the primary key to the dataset
        Dim dcs(1) As DataColumn
        dcs(0) = ds.Tables("RECH1").Columns("id")
        ds.Tables("Rech1").PrimaryKey = dcs

        'Add the row locally
        Dim drw1 As DataRow = _
            ds.Tables("Rech1").NewRow()
        drw1("ID") = Int32.MaxValue
        drw1("XAUFTR_NR") = CDbl(txtAuftragNummer.Text)
        'Kunde
        drw1("XKUNDENNR") = txtXKundenNr.Text
        drw1("XAN1") = CInt(txtXAN1.Text)
        drw1("XNAME1") = txtXName1.Text
        drw1("XNAME2") = txtXName2.Text
        drw1("XST") = txtXST.Text
        drw1("XPL") = txtXPL.Text
        drw1("XOT") = txtXOT.Text
        drw1("XTEL") = txtXTel.Text

        'Fahrzeug
        drw1("XFGSTLLNR") = FGSTLLNR.Text
        drw1("XKZ") = txtXKZ.Text
        drw1("XKMSTAND") = CLong(txtXKMSTAND.Text)
        drw1("XZULASS") = CDatum(txtXZULASS.Text)
        drw1("XTYP") = txtXTYP.Text
        'drw1("XTUEV") = CDatum(TUEV.Text) 'TÜV kommt aus FAHRZEUG DB

        'Auftrag
        drw1("XNETTO") = CDouble(txtXNetto.Text) 'überflüssig
        drw1("XSCHMIER") = CDouble(txtXSCHMIER.Text) 'überflüssig
        drw1("XLOHN") = CDouble(txtXLOHN.Text) 'überflüssig
        'wichtig fuer AltTeilMwSt
        drw1("XSONDER") = CDouble(txtXSONDER.Text)

        drw1("XDATUM") = CDatum(txtDatum.Text)
        drw1("WerkDATUM") = CDatum(txtWerkdatum.Text)
        drw1("LDATUM") = CDatum(txtLeistDatum.Text) 'new with version 2.0.3.0
        drw1("MWSTsatz") = CDouble(txtMwStSatz.Text)
        drw1("AltteilMWST") = CDouble(txtAltTeilMwStSatz.Text)
        drw1("gedruckt") = chkGedruckt.Checked

        'Include an event to fill in the autonumber value
        AddHandler daRech1.RowUpdated, _
            New OleDb.OleDbRowUpdatedEventHandler( _
            AddressOf OnRowUpdated)

        ds.Tables("Rech1").Rows.Add(drw1)
        'Update the Access database file
        anzahl = da.Update(ds, "Rech1")
        ds.AcceptChanges()
        cn.Close()
        AuftragNeuSpeichern = anzahl
        Status.Text = anzahl & " Auftrag/Aufträge gespeichert"
    End Function
    Private Sub OnRowUpdated(ByVal sender As Object, _
    ByVal args As OleDb.OleDbRowUpdatedEventArgs)

        'Include a variable and a command to retrieve the
        'identity value from the Access database
        Dim int1 As Integer = 0
        Dim con As New OleDbConnection
        OpenDBConnection(con)
        Dim cmd1 As OleDb.OleDbCommand = _
            New OleDb.OleDbCommand("SELECT @@IDENTITY", con)

        If args.StatementType = StatementType.Insert Then
            'Retrieve the identity value and
            'store it in the ShipperID column
            int1 = CInt(cmd1.ExecuteScalar())
            args.Row("id") = int1
        End If
        con.Close()
    End Sub
    Function FahrzeugSpeichern(ByVal nr As String) As Integer
        '"KUNDEN_NR, FGSTLLNR, TYP, KENNZEICH, KM_STAND, KFZ_BRF_NR, 
        'ZULASSUNG1, TUEV, ASU,  SCHREIBER,  SICHER"
        Dim anzahl As Integer
        Dim id As Long
        'find the car
        Dim cn As New OleDbConnection
        OpenDBConnection(cn)
        Dim cmd As New OleDbCommand()
        cmd.Connection = cn
        cmd.CommandText = "select * from KUNDFAHR where FGSTLLNR='" & nr & "'"
        Dim reader As OleDbDataReader = cmd.ExecuteReader()
        While reader.Read()
            anzahl = anzahl + 1
            id = CLong(reader("id").ToString)
        End While
        reader.Close()
        If anzahl = 0 Then
            Status.Text = "FahrzeugSpeichern: Kein Fahrzeug für FgstNr: " & nr & " gefunden!"
            cn.Close()
            Return 0
            Exit Function
        End If
        Dim ds As New DataSet
        Dim da As New OleDbDataAdapter
        Dim cmb As New OleDbCommandBuilder(da)
        cmd.CommandText = "Select * from KUNDFAHR where id=" & id
        da.SelectCommand = cmd
        da.Fill(ds, "KUNDFAHR")

        'add the primary key to the dataset
        Dim dcs(1) As DataColumn
        dcs(0) = ds.Tables("KUNDFAHR").Columns("id")
        ds.Tables("KUNDFAHR").PrimaryKey = dcs

        Dim drw1 As DataRow = ds.Tables("KUNDFAHR").Rows.Find(id)
        '        drw1("ID") = Int32.MaxValue
        drw1("KM_STAND") = CDouble(txtXKMSTAND.Text)
        drw1("ZULASSUNG1") = txtXZULASS.Text
        drw1("TUEV") = TUEV.Text
        drw1("ASU") = txtASU.Text
        drw1("Schreiber") = txtSchreiber.Text
        drw1("Sicher") = txtSicher.Text

        anzahl = da.Update(ds, "KUNDFAHR")
        ds.AcceptChanges()
        cn.Close()

        Return anzahl
    End Function
    Private Sub PositionenSpeichern()
        Dim AuftragNr As Long = CLong(txtAuftragNummer.Text)
        If AuftragNr = 0 Then Exit Sub
        'Get the source table from DataGridView1
        Dim tbl As DataTable
        Dim bs As BindingSource
        bs = CType(DataGridView1.DataSource, BindingSource)
        If IsNothing(bs) Then 'this is a new auftrag
            kbfakt_start.FillGrid(DataGridView1, "Select Pos, ARTIKELNR, ARTIKELBEZ, ArtTyp, MENGE, E_PREIS, RABATT, " + _
            "(menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt from RECH2 where AUFTR_NR=" & txtAuftragNummer.Text & " order by pos")
            bs = CType(DataGridView1.DataSource, BindingSource)

            'Exit Sub 'there is no Auftrag loaded
        End If
        tbl = CType(bs.DataSource, DataTable)

        'Remove empty rows, new with 2.0.1.6
        Dim ir As Integer
        For ir = tbl.Rows.Count - 1 To 0 Step -1
            Dim r As DataRow = tbl.Rows(ir)
            If r("ARTIKELNR").ToString.Length = 0 And r("ARTIKELBEZ").ToString.Length = 0 Then
                tbl.Rows.Remove(r)
            End If
        Next ir
        tbl.AcceptChanges()

        '
        Dim cn As New OleDbConnection
        Dim cmd As New OleDbCommand
        Dim trn As OleDbTransaction
        Dim i As Integer
        OpenDBConnection(cn)
        trn = cn.BeginTransaction()
        cmd.Connection = cn
        cmd.Transaction = trn
        Try
            cmd.CommandText = "Delete from RECH2 where AUFTR_NR=" & AuftragNr
            cmd.ExecuteNonQuery()
            For i = 0 To tbl.Rows.Count - 1
                With cmd
                    '                    CInt(tbl.Rows(i).Item("pos").ToString) & ", " & _
                    .CommandText = "Insert INTO rech2 (AUFTR_NR, pos, ARTIKELNR, " & _
                    "ARTIKELBEZ, ArtTyp, MENGE, E_PREIS, RABATT) " & _
                    "VALUES (" & AuftragNr & ", " & _
                    CInt(i + 1).ToString & ", " & _
                    "'" & tbl.Rows(i).Item("ARTIKELNR").ToString & "', " & _
                    "'" & tbl.Rows(i).Item("ARTIKELBEZ").ToString & "', " & _
                    CInt(tbl.Rows(i).Item("ArtTyp").ToString) & ", " & _
                    CDoubleS(tbl.Rows(i).Item("MENGE")) & ", " & _
                    CDoubleS(tbl.Rows(i).Item("E_PREIS")) & ", " & _
                    CDoubleS(tbl.Rows(i).Item("RABATT")) & _
                    ")"
                    Debug.Print("PositionenSpeichern:" & vbCrLf & .CommandText)
                    .ExecuteNonQuery()
                End With
            Next
            trn.Commit()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "PositionenSpeichern")
            trn.Rollback()
        Finally
            cn.Close()
        End Try
    End Sub
    Private Sub PositionenAktualisieren()
        Dim dgvc As DataGridViewCell
        dgvc = DataGridView1.CurrentCell
        Dim iPos As Integer
        With DataGridView1
            If .Rows.Count = 0 Then Exit Sub
            For iPos = 0 To .Rows.Count - 1
                .Rows(iPos).Cells("Pos").Value = CStr(iPos + 1)
                .CommitEdit(DataGridViewDataErrorContexts.Commit)
            Next
        End With
    End Sub
    Private Sub btn_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Up.Click
        Dim idx As Integer
        Dim dgvc As DataGridViewCell
        dgvc = DataGridView1.CurrentCell
        Dim iPos As Integer

        'simply exchange the pos numbers and reload data to sort
        If Not IsNothing(dgvc) Then
            idx = dgvc.RowIndex()

            If idx = 0 Then Exit Sub 'we are already at top
            'exchange Pos number for row(idx) and row(idx-1)
            'row(idx-1): pos 1  ->   pos 2
            'row(idx)  : pos 2  ->   pos 1

            iPos = DataGridView1.Rows(idx).Cells("Pos").Value

            Debug.WriteLine("iPos ist " & iPos & ", idx ist " & idx)
            DataGridView1.Rows(idx).Cells("Pos").Value = iPos - 1
            Debug.WriteLine("DataGridView1.Rows(idx - 1).Cells(Pos).Value ist " & DataGridView1.Rows(idx - 1).Cells("Pos").Value)
            DataGridView1.Rows(idx - 1).Cells("Pos").Value = iPos
            Debug.WriteLine("DataGridView1.Rows(idx ).Cells(Pos).Value ist " & DataGridView1.Rows(idx).Cells("Pos").Value)

            'commit changes
            DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)

            'tbl.AcceptChanges()
            'bs.EndEdit()
            Debug.WriteLine("#######AcceptChages()########")
            Debug.WriteLine("DataGridView1.Rows(idx - 1).Cells(Pos).Value ist " & DataGridView1.Rows(idx - 1).Cells("Pos").Value)
            Debug.WriteLine("DataGridView1.Rows(idx ).Cells(Pos).Value ist " & DataGridView1.Rows(idx).Cells("Pos").Value)

            DataGridView1.Sort(DataGridView1.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
            DataGridView1.Item(1, idx - 1).Selected = True
            DataGridView1.CurrentCell = DataGridView1.Rows(idx - 1).Cells(1)
            'CalcNetto2(tbl)
            DataChanged(True)

        End If
    End Sub
    Private Sub btn_down_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_down.Click
        Dim idx As Integer
        Dim dgvc As DataGridViewCell
        dgvc = DataGridView1.CurrentCell
        Dim iPos As Integer

        'simply exchange the pos numbers and reload data to sort
        If Not IsNothing(dgvc) Then
            idx = dgvc.RowIndex()

            If idx = DataGridView1.Rows.Count - 1 Then Exit Sub 'we are already at bottom
            'exchange Pos number for row(idx) and row(idx-1)
            'row(idx-1): pos 1  ->   pos 2
            'row(idx)  : pos 2  ->   pos 1

            iPos = DataGridView1.Rows(idx).Cells("Pos").Value

            Debug.WriteLine("btnDown:")
            Debug.WriteLine("iPos ist " & iPos & ", idx ist " & idx)

            DataGridView1.Rows(idx).Cells("Pos").Value = iPos + 1
            Debug.WriteLine("DataGridView1.Rows(idx).Cells(Pos).Value ist " & DataGridView1.Rows(idx).Cells("Pos").Value)

            DataGridView1.Rows(idx + 1).Cells("Pos").Value = iPos
            Debug.WriteLine("DataGridView1.Rows(idx +1 ).Cells(Pos).Value ist " & DataGridView1.Rows(idx + 1).Cells("Pos").Value)

            'commit changes
            DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
            'DataGridView1.CurrentCell = dgvc.RowIndex - 1

            'tbl.AcceptChanges()
            'bs.EndEdit()
            Debug.WriteLine("#######AcceptChages()########")
            Debug.WriteLine("DataGridView1.Rows(idx + 1).Cells(Pos).Value ist " & DataGridView1.Rows(idx + 1).Cells("Pos").Value)
            Debug.WriteLine("DataGridView1.Rows(idx ).Cells(Pos).Value ist " & DataGridView1.Rows(idx).Cells("Pos").Value)

            DataGridView1.Sort(DataGridView1.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
            'DataGridView1.Item(1, idx + 1).Selected = True
            DataGridView1.CurrentCell = DataGridView1.Rows(idx + 1).Cells(1)
            'CalcNetto2(tbl)
            DataChanged(True)

        End If

    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        ArtikelListe.ShowDialog()
        If ArtikelListe.DialogResult = Windows.Forms.DialogResult.OK Then
            Dim x As String
            Dim v(7) As String
            'init a tbl to the datasource of the grid
            Dim tbl As DataTable
            Dim bs As BindingSource
            bs = CType(DataGridView1.DataSource, BindingSource)
            If IsNothing(bs) Then 'this is a new auftrag
                kbfakt_start.FillGrid(DataGridView1, "Select Pos, ARTIKELNR, ARTIKELBEZ, ArtTyp, MENGE, E_PREIS, RABATT, " + _
                "(menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt from RECH2 where AUFTR_NR=" & txtAuftragNummer.Text & " order by pos")
                bs = CType(DataGridView1.DataSource, BindingSource)

                'Exit Sub 'there is no Auftrag loaded
            End If
            tbl = CType(bs.DataSource, DataTable)
            With ArtikelListe
                x = .ArtikelNr & vbTab & .menge & vbTab & .ArtTyp & vbTab & .vk & vbTab & .rabatt
                v(0) = CStr(tbl.Rows.Count + 1) 'pos
                v(1) = .ArtikelNr
                v(2) = .ARTIKELBEZ
                v(3) = .ArtTyp.ToString
                v(4) = .menge.ToString
                v(5) = .vk.ToString
                v(6) = .rabatt.ToString
                v(7) = CStr(.menge * .vk - (.rabatt / 100 * (.menge * .vk)))
                tbl.Rows.Add(v)
                Debug.Print("Artikelauswahl = " & x)
            End With
            '"AUFTR_NR, ARTIKELNR, ARTIKELBEZ, KONTO, MENGE, E_PREIS, RABATT, LOHNART, IST_AW, MITARBCODE, FAHRZEUG, TEXT"
            '+ Pos und ArtTyp
            'kbfakt_start.FillGrid(DataGridView1, "Select Pos, ARTIKELNR, ARTIKELBEZ, ArtTyp, MENGE, E_PREIS, RABATT, " + _
            '"(menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt from RECH2 where AUFTR_NR=" & AUFTR_Nr & " order by pos")
            CalcNetto2(tbl)
        Else
            AddEmptyLine()
        End If
        ArtikelListe.Close()
        DataChanged(True)
        DataGridView1.ClearSelection()
        DataGridView1.Rows(DataGridView1.Rows.Count - 1).Selected = True

        scrollGrid(DataGridView1)
        DataGridView1.CurrentCell = DataGridView1(1, DataGridView1.Rows.Count - 1)
        DataGridView1.BeginEdit(True)
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Dim idx As Integer
        Dim dgvc As DataGridViewCell
        dgvc = DataGridView1.CurrentCell
        If Not IsNothing(dgvc) Then
            If (MessageBox.Show("Markierte Zeile wirklich löschen?", "Sicherheitsfrage", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes) Then
                idx = dgvc.RowIndex()
                Dim tbl As DataTable
                Dim bs As BindingSource
                bs = CType(DataGridView1.DataSource, BindingSource)
                tbl = CType(bs.DataSource, DataTable)
                tbl.Rows.RemoveAt(idx)
                PositionenAktualisieren()
                If dgvc.RowIndex > 0 Then DataGridView1.Item(1, dgvc.RowIndex - 1).Selected = True
                CalcNetto2(tbl)
            End If
        Else
            Beep()
        End If
    End Sub
    ''' <summary>
    ''' CalcNetto2 is executed for changes in the actual dataset
    ''' </summary>
    ''' <param name="tbl"></param>
    ''' <remarks></remarks>
    Sub CalcNetto2(ByRef tbl As DataTable)
        Dim betrag As Double
        Try
            betrag = CDbl(tbl.Compute("sum(gesamt)", ""))
        Catch ex As Exception
            betrag = 0
        End Try
        txtXNetto.Text = String.Format("{0:0.00}", betrag)

        Dim altTeile As Double
        Try
            altTeile = CDbl(tbl.Compute("sum(gesamt)", "ArtTyp = 3"))
        Catch ex As Exception
            altTeile = 0
        End Try

        'add 19% MwSt for ALL icluding AltTeile, Altteil-MwSt (1.9%) will be added later
        Dim mwst As Double = betrag / 100 * CDouble(txtMwStSatz.Text) 'changed back in 2.1.3.0
        txtMwStBetrag.Text = String.Format("{0:0.00}", mwst)

        'add 20,9% MwSt for all AltTeile
        Dim AltTeilMwst As Double = altTeile / 100 * CDouble(txtAltTeilMwStSatz.Text)
        txtAltTeilMwst.Text = String.Format("{0:0.00}", AltTeilMwst)

        Dim gesamt As Double = betrag + mwst + AltTeilMwst
        txtGesamtbetrag.Text = String.Format("{0:0.00}", gesamt)

        'schmiermittel berechnen
        betrag = CDouble(tbl.Compute("sum(gesamt)", "ArtTyp = 1"))
        txtXSCHMIER.Text = String.Format("{0:0.00}", CDbl(betrag))

        'Lohnkosten
        betrag = CDouble(tbl.Compute("sum(gesamt)", "ArtTyp = 2"))
        txtXLOHN.Text = String.Format("{0:0.00}", CDbl(betrag))

        'Altteile berechnen
        betrag = CDouble(tbl.Compute("sum(gesamt)", "ArtTyp = 3"))
        txtXSONDER.Text = String.Format("{0:0.00}", CDbl(betrag))

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        PositionenSpeichern()
    End Sub
    ''' <summary>
    ''' search an ArtikelNr and return the first matching DataRow
    ''' </summary>
    ''' <param name="artikelNr"></param>
    ''' <returns>
    ''' DataRow
    ''' </returns>
    ''' <remarks></remarks>
    Private Function SearchArtikel(ByVal artikelNr As String) As DataRow
        If g_ArtikelFound Then Return Nothing
        Debug.Print("Searching for ArtikelNr='" + artikelNr + "'")
        Dim dr As DataRow = Nothing
        Dim ArtNr As String = ""
        Dim query As String = "Select ArtikelNr, ArtikelBez, ArtTyp, VK as E_PREIS from ARTSTAMM where ARTIKELNR='" + artikelNr + "'"
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim da As New OleDb.OleDbDataAdapter(query, cn)
        Dim tbl As New DataTable
        Try
            da.Fill(tbl)
            If tbl.Rows.Count >= 1 Then ' changed from =1 to >=1 as many artikel-nr are not unique
                Debug.Print("Found {0} Artikel(s) matching: ", tbl.Rows.Count)
                dr = tbl.Rows(0)
                g_ArtikelFound = True
            Else
                Debug.Print("Found no Artikel match")
                dr = Nothing
            End If
        Catch ox As OleDbException
            MessageBox.Show(ox.Message, "SearchArtikel")
        Finally
            cn.Close()
        End Try
        Debug.Print("SearchArtikel returns")
        Return dr
    End Function
    Private Function SearchRichtwert(ByVal artikelNr As String) As DataRow
        Dim dr As DataRow = Nothing
        Dim ArtNr As String = ""
        'try to find ArtNr in combo with FzgTyp and read data
        Dim query As String
        Dim FzgTyp As String
        If txtXTYP.Text.Length >= 6 Then 'changed in 2.0.0.6, was Fahrgstnr
            FzgTyp = txtXTYP.Text.Substring(0, 6)
        Else
            FzgTyp = txtXTYP.Text
        End If
        '= "Select r1.ArtNr as ArtikelNr, r1.AwText as ARTIKELBEZ, r2.PREIS from richtwerte1 as r1, richtwerte2 as r2 where ArtNr='" + artikelNr + "' AND FzgTyp='" + txtXTYP.Text + "'"
        query = "SELECT r2.ArtNr AS ArtikelNr, r1.AWText AS ARTIKELBEZ, r2.FzgTyp, r2.Preis as E_PREIS FROM Richtwerte2 AS r2 LEFT JOIN Richtwerte1 AS r1 ON r2.ArtNr = r1.ArtNr WHERE (((r2.ArtNr)='" & artikelNr & "') AND ((r2.FzgTyp) LIKE '" & txtXTYP.Text & "%'))"
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim da As New OleDb.OleDbDataAdapter(query, cn)
        Dim tbl As New DataTable
        Try
            da.Fill(tbl)
            If tbl.Rows.Count > 0 Then
                dr = tbl.Rows(0)
            End If
        Catch ox As OleDbException
            MessageBox.Show(ox.Message, "SearchRichtwert")
        Finally
            cn.Close()
        End Try
        da.Dispose()
        Return dr
    End Function

    Private Sub DataGridView1_CancelRowEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.QuestionEventArgs) Handles DataGridView1.CancelRowEdit

    End Sub
    Private Sub DataGridView1_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles DataGridView1.CellBeginEdit

        btn_down.Enabled = False
        btn_Up.Enabled = False
        btnAdd.Enabled = False
        btnRemove.Enabled = False
        btnArtikelFreiEingabe.Enabled = False
        g_ArtikelFound = False
    End Sub
    ''' <summary>
    ''' process ENTER keydown event for datagridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' if in POS column -> new POS number
    ''' if in ArtikelNR and EndsWith "*" -> do not SearchArtikel 
    '''    ELSE SearchArtikel
    ''' </remarks>
    Private Sub DataGridView1_CellEndEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        Dim dr As DataRow = Nothing
        Dim curr As DataGridViewCell
        curr = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex)
        Dim oldValue As String
        Try
            oldValue = curr.Value.ToString
        Catch
            oldValue = ""
        End Try
        Dim s As String = oldValue
        Dim c As Integer = e.ColumnIndex
        Dim r As Integer = e.RowIndex
        Dim aTyp As Integer = 0 'Teil

        'check if we are in the artikelnr column
        If DataGridView1.Columns(e.ColumnIndex).Name.ToUpper = "ARTIKELNR" Then
            'MessageBox.Show(DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString)
            s = s.ToUpper 'new in 2.0.0.6
            'set aTyp to a special char entered at the beginning of ArtikelNr
            If s.Length > 0 Then
                If s.StartsWith(",") Then aTyp = 3 'Austauschteile
                If s.StartsWith(";") Then aTyp = 1 'Schmierstoff
                If s.StartsWith("ZEIT") Then aTyp = 2 'Lohn
            End If
            'cut first char if aTyp=3 or =1
            If aTyp = 3 Or aTyp = 1 Then s = s.Substring(1)

            'try to find the record in ArtStamm
            'check if there is anything in the string to search for
            If (s.Length > 0) And Not s.EndsWith("*") And Not s.EndsWith("+") Then
                'if starting with 'ZEIT/' we have to search a richtwert
                If s.StartsWith("ZEIT/") And s.Length > 5 Then
                    dr = SearchRichtwert(s)
                Else
                    If Not g_ArtikelFound Then
                        'search for it in ArtStamm
                        dr = SearchArtikel(s) 'search the article
                    End If
                End If
            End If
            'set the type into the grid
            DataGridView1.Rows(curr.RowIndex).Cells("ART-TYP").Value = aTyp

            'if already found (dr is NOT nothing)
            If Not IsNothing(dr) Then
                'Debug.Print("### dr DUMP:" + vbCrLf)
                'Dim i
                'For i = 0 To dr.Table.Columns.Count - 1
                '    Debug.Print("'" + dr(i).ToString + "'" + vbCr)
                'Next
                'set artikelnr and artikelbez according to dr (either richtwert or artstamm)
                curr.Value = dr("ARTIKELNR")
                DataGridView1.Rows(curr.RowIndex).Cells("ARTIKELBEZ").Value = dr("ARTIKELBEZ")
                DataGridView1.Rows(curr.RowIndex).Cells("E_PREIS").Value = dr("E_PREIS")
                DataGridView1.Rows(curr.RowIndex).Cells("MENGE").Value = 1
                c = c + 1
                DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
            ElseIf s.EndsWith("*") Or s.EndsWith("+") Then
                'process one time article
                c = c + 1
                Try
                    DataGridView1.CurrentCell = Me.DataGridView1(c, r)
                Catch
                End Try
                DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
            ElseIf Not s.StartsWith("ZEIT/") And Not g_ArtikelFound Then
                'show artikel selection dialog (article not found and no one time article
                ArtikelListe.ArtikelNr = oldValue
                ' ############# in v2.0.2.7 disabled autolaunch of ArtikelListe for unknown ArtikelNr
                'If ArtikelListe.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    Dim x As String
                '    Dim v(7) As String
                '    'kbfakt_start.FillGrid(DataGridView1, "Select Pos, ARTIKELNR, ARTIKELBEZ, ArtTyp, MENGE, E_PREIS, RABATT, " + _
                '    '"(menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt from RECH2 where AUFTR_NR=" & AUFTR_Nr & " order by pos")
                '    With ArtikelListe
                '        'get all values from dialog
                '        x = .ArtikelNr & vbTab & .menge & vbTab & .ArtTyp & vbTab & .vk & vbTab & .rabatt
                '        v(0) = CStr(r + 1) 'pos
                '        DataGridView1.Rows(curr.RowIndex).Cells("POS").Value = v(0)
                '        v(1) = .ArtikelNr
                '        DataGridView1.Rows(curr.RowIndex).Cells("ARTIKELNR").Value = v(1)
                '        v(2) = .ARTIKELBEZ
                '        DataGridView1.Rows(curr.RowIndex).Cells("ARTIKELBEZ").Value = v(2)
                '        v(3) = .ArtTyp.ToString
                '        DataGridView1.Rows(curr.RowIndex).Cells("ART-TYP").Value = v(3)
                '        v(4) = .menge.ToString
                '        DataGridView1.Rows(curr.RowIndex).Cells("MENGE").Value = v(4)
                '        v(5) = .vk.ToString
                '        DataGridView1.Rows(curr.RowIndex).Cells("E_PREIS").Value = v(5)
                '        v(6) = .rabatt.ToString
                '        DataGridView1.Rows(curr.RowIndex).Cells("RABATT").Value = v(6)
                '        v(7) = CStr(.menge * .vk - (.rabatt / 100 * (.menge * .vk)))
                '        DataGridView1.Rows(curr.RowIndex).Cells("Gesamt").Value = v(7)
                '        DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
                '        Debug.Print("Artikelauswahl = " & x)
                '        ArtikelListe.Close()
                '        'Move Cursor is done with ProcessKey...
                '        'DataGridView1.CurrentCell = Me.DataGridView1(c+1, r)
                '    End With
                'End If
                ' ############# end change for 2.0.2.7
            ElseIf s = "ZEIT/" Then
                'neuer Richtwert via Dialog
                Dim dlg As New Richtwerte
                dlg.FzgTyp = txtXTYP.Text
                dlg.AwNr = s
                If Richtwerte.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    DataGridView1.Rows(curr.RowIndex).Cells("ARTIKELNR").Value = dlg.AwNr
                    DataGridView1.Rows(curr.RowIndex).Cells("ARTIKELBEZ").Value = dlg.AwText
                    DataGridView1.Rows(curr.RowIndex).Cells("MENGE").Value = 1
                    DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
                    DataGridView1.CurrentCell = Me.DataGridView1(c + 2, r)
                    'Me.DataGridView1(c + 2, r).Selected = True
                Else
                    DataGridView1.CurrentCell = Me.DataGridView1(c, r)
                    'Me.DataGridView1(c, r).Selected = True
                End If
                dlg.Dispose()
            ElseIf s.StartsWith("ZEIT/") Then
                RichtwertEingabe(DataGridView1.Rows(curr.RowIndex))
                Me.DataGridView1(c, r).Selected = True
                '============================================
            End If 'IsNothing(dr)
weiter:
        ElseIf DataGridView1.Columns(e.ColumnIndex).Name.ToUpper = "MENGE" Or _
               DataGridView1.Columns(e.ColumnIndex).Name.ToUpper = "RABATT" Or _
               DataGridView1.Columns(e.ColumnIndex).Name.ToUpper = "E_PREIS" Then
            'calculate the row
            DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
            Dim gesamt As Double = 0
            Dim gesamtLohn As Double = 0
            Dim gesamtSchmier As Double = 0
            Dim gesamtAltteile As Double = 0
            Dim netto As Double = 0
            'calc total price incl rabatt
            With DataGridView1.Rows(curr.RowIndex)
                'CStr(.menge * .vk - (.rabatt / 100 * (.menge * .vk)))
                gesamt = CDbl(.Cells("MENGE").Value) * CDbl(.Cells("E_PREIS").Value) - (CDbl(.Cells("RABATT").Value) / 100 * CDbl(.Cells("MENGE").Value) * CDbl(.Cells("E_PREIS").Value))
                .Cells("Gesamt").Value = CStr(gesamt)
            End With
            'calculate sums
            Dim ri As Integer
            gesamt = 0
            For ri = 0 To DataGridView1.RowCount - 1
                With DataGridView1.Rows(ri)
                    gesamt = CDbl(.Cells("gesamt").Value)
                    netto += gesamt
                    'Public ArtTypen() As String = {"Teil", "Schmier", "Lohn", "Austauschteil"}
                    Select Case CInt(.Cells("ArtTyp").Value)
                        Case 1
                            gesamtSchmier += gesamt
                        Case 3
                            gesamtAltteile += gesamt
                        Case 2
                            gesamtLohn += gesamt
                        Case 3
                            gesamtAltteile += gesamt
                    End Select
                End With
            Next
            txtXNetto.Text = String.Format("{0:0.00}", netto)
            'version 2.1.0.0
            'version 2.1.3.0
            txtMwStBetrag.Text = String.Format("{0:0.00}", CDbl(txtMwStSatz.Text) / 100 * (netto)) 'changed in v 2.1.3.0 - gesamtAltteile))
            txtXSCHMIER.Text = String.Format("{0:0.00}", gesamtSchmier)
            txtXLOHN.Text = String.Format("{0:0.00}", gesamtLohn)
            txtXSONDER.Text = String.Format("{0:0.00}", gesamtAltteile)
            'new with version 2.1.0.0
            txtAltTeilMwst.Text = String.Format("{0:0.00}", CDbl(txtAltTeilMwStSatz.Text) / 100 * gesamtAltteile)
            txtGesamtbetrag.Text = String.Format("{0:0.00}", CDbl(txtXNetto.Text) + CDbl(txtMwStBetrag.Text) + CDbl(txtAltTeilMwst.Text))
        ElseIf DataGridView1.Columns(e.ColumnIndex).Name.ToUpper = "GESAMT" Then
            'Move to next row
            DataGridView1.CurrentCell = Me.DataGridView1(1, r + 1)

        End If
        DataChanged(True)
        btn_down.Enabled = True
        btn_Up.Enabled = True
        btnAdd.Enabled = True
        btnRemove.Enabled = True
        btnArtikelFreiEingabe.Enabled = True
    End Sub

    Private Sub DataGridView1_DefaultValuesNeeded(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles DataGridView1.DefaultValuesNeeded
        With e.Row
            .Cells("POS").Value = e.Row.Index + 1
            .Cells("MENGE").Value = "1"
            .Cells("E_PREIS").Value = "0"
            .Cells("RABATT").Value = "0"
            .Cells("GESAMT").Value = "0"
            .Cells("Art-Typ").Value = 0
        End With
    End Sub

    Private Sub DataGridView1_DataError(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError
        Return 'test started with 2.0.2.6
        'enhanced in version 2.0.1.1
        If (MessageBox.Show("Wert ungültig" & vbCrLf & e.Context.ToString() & vbCrLf & _
                        "Zeile " & e.RowIndex & vbCrLf & _
                        "Spalte " & e.ColumnIndex & _
                        "Ausnahme " & e.Exception.ToString() & _
                        "Ausnahme weiter bearbeiten?", _
                        "Fehler in der Eingabe", _
                        MessageBoxButtons.OKCancel, _
                        MessageBoxIcon.Error) = Windows.Forms.DialogResult.Cancel) Then
            e.ThrowException = False
        End If

        Dim context As String = ""
        Select Case (e.Context)
            Case DataGridViewDataErrorContexts.Commit
                context = "Commit error"
            Case DataGridViewDataErrorContexts.ClipboardContent
                context = "ClipboardContent error"
            Case DataGridViewDataErrorContexts.CurrentCellChange
                context = "CurrentCellChange"
            Case DataGridViewDataErrorContexts.Display
                context = "Display"
            Case DataGridViewDataErrorContexts.Formatting
                context = "Formatting"
            Case DataGridViewDataErrorContexts.InitialValueRestoration
                context = "InitialValueRestoration"
            Case DataGridViewDataErrorContexts.LeaveControl
                context = "LeaveControl"
            Case DataGridViewDataErrorContexts.Parsing
                context = "Parsing"
            Case DataGridViewDataErrorContexts.PreferredSize
                context = "PreferredSize"
            Case DataGridViewDataErrorContexts.RowDeletion
                context = "RowDeletion"
            Case DataGridViewDataErrorContexts.Scroll
                context = "Scroll"
            Case Else
                context = "Unknown context"
        End Select

        If (TypeOf (e.Exception) Is ConstraintException) Then
            Dim view As DataGridView = CType(sender, DataGridView)
            view.Rows(e.RowIndex).ErrorText = "an error"
            view.Rows(e.RowIndex).Cells(e.ColumnIndex).ErrorText = "an error"

            e.ThrowException = False
        End If
        'removed in 2.0.0.5: e.Cancel = True
    End Sub
    Private Sub DataGridView1_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGridView1.KeyUp
        Dim s As Object
        s = sender
        Dim dg As DataGridView
        dg = CType(s, DataGridView)
        If dg.CurrentCell.ColumnIndex = 1 Then
            If e.KeyCode = Keys.F3 Then
                SearchForArtikel(dg)
            End If
        End If

    End Sub
    Private Sub DataGridView1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGridView1.KeyDown
        Dim s As Object
        s = sender
        Dim dg As DataGridView
        dg = CType(s, DataGridView)
        If dg.CurrentCell.ColumnIndex = 1 Then
            If e.KeyCode = Keys.F3 Then
                SearchForArtikel(dg)
            End If
        End If
    End Sub
    Private Sub SearchForArtikel(ByRef dg As DataGridView)
        Dim dr As DataRow = Nothing
        Dim curr As DataGridViewCell
        curr = DataGridView1.CurrentCell
        Dim oldValue As String
        Try
            oldValue = curr.Value.ToString
        Catch
            oldValue = ""
        End Try
        Dim s As String = oldValue
        Dim c As Integer = curr.ColumnIndex
        Dim r As Integer = curr.RowIndex
        'show list
        Dim dlg As New ArtikelListe
        dlg.ArtikelNr = oldValue
        If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
            g_ArtikelFound = True
            Dim x As String
            Dim v(7) As String
            'kbfakt_start.FillGrid(DataGridView1, "Select Pos, ARTIKELNR, ARTIKELBEZ, ArtTyp, MENGE, E_PREIS, RABATT, " + _
            '"(menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt from RECH2 where AUFTR_NR=" & AUFTR_Nr & " order by pos")
            With dlg
                Dim bT As Boolean = DataGridView1.CurrentCell.IsInEditMode ' HGO TEST
                x = .ArtikelNr & vbTab & .menge & vbTab & .ArtTyp & vbTab & .vk & vbTab & .rabatt
                v(0) = CStr(r + 1) 'pos
                DataGridView1.Rows(curr.RowIndex).Cells("POS").Value = v(0)
                v(1) = .ArtikelNr
                DataGridView1.Rows(curr.RowIndex).Cells("ARTIKELNR").Value = v(1)
                v(2) = .ARTIKELBEZ
                DataGridView1.Rows(curr.RowIndex).Cells("ARTIKELBEZ").Value = v(2)
                v(3) = .ArtTyp.ToString
                DataGridView1.Rows(curr.RowIndex).Cells("ART-TYP").Value = v(3)
                v(4) = .menge.ToString
                DataGridView1.Rows(curr.RowIndex).Cells("MENGE").Value = v(4)
                v(5) = .vk.ToString
                DataGridView1.Rows(curr.RowIndex).Cells("E_PREIS").Value = v(5)
                v(6) = .rabatt.ToString
                DataGridView1.Rows(curr.RowIndex).Cells("RABATT").Value = v(6)
                v(7) = CStr(.menge * .vk - (.rabatt / 100 * (.menge * .vk)))
                DataGridView1.Rows(curr.RowIndex).Cells("Gesamt").Value = v(7)
                'Dim i As Integer
                'For i = 0 To v.Length - 1
                '    DataGridView1.Rows(curr.RowIndex).Cells(0).Value = v(i)
                'Next i
                'commit changes
                DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
                DataGridView1.RefreshEdit() 'new in 2.0.2.0
                DataGridView1.EndEdit()  'new in 2.0.2.0
                'tbl.Rows.Add(v)
                Debug.Print("Artikelauswahl = " & x)
                dlg.Close()
            End With
        Else
            curr.Value = oldValue
            DataGridView1.CancelEdit()
        End If
    End Sub

    Private Sub DataGridView1_EditingControlShowing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles DataGridView1.EditingControlShowing
        If TypeOf e.Control Is DataGridViewTextBoxEditingControl Then
            'e.Control.KeyPress += New KeyPressEventHandler(AddressOf Control_KeyPress)
            Try
                RemoveHandler e.Control.KeyPress, AddressOf Control_Keypress
                RemoveHandler e.Control.KeyDown, AddressOf Control_KeyDown
            Catch ex As Exception
            Finally
                AddHandler e.Control.KeyDown, AddressOf Control_KeyDown
                AddHandler e.Control.KeyPress, AddressOf Control_Keypress
            End Try
            'check if this is the ArtikelBez Column
            Dim c As DataGridViewTextBoxEditingControl = DirectCast(e.Control, DataGridViewTextBoxEditingControl)
            Dim dgv As DataGridView = DirectCast(sender, DataGridView)
            If dgv.CurrentCell.ColumnIndex = 1 Then
                c.MaxLength = 17
            End If
            If dgv.CurrentCell.ColumnIndex = 2 Then
                c.MaxLength = 60
                'change size to match 30 chars in a line
                c.Anchor = AnchorStyles.None
                ' Create a Graphics object for the Control.
                Dim gr As Graphics = c.CreateGraphics()
                ' measure the string
                ' Get the Size needed to accommodate the formatted Text.
                Dim preferredSize As Size = gr.MeasureString("012345678901234567890123456789", c.Font).ToSize()
                ''Dim stringSize As SizeF = gr.MeasureString("012345678901234567890123456789", New Font("Courier New", 10))
                c.ClientSize = New Size(preferredSize.Width, preferredSize.Height)
                gr.Dispose()
            End If
        End If
    End Sub
    Private Sub Control_Keypress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'new in 2.0.0.7
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) And _
           (Control.ModifierKeys And Keys.Shift) = Keys.Shift Then
            Beep()
            e.Handled = True
        End If

        If DataGridView1.CurrentCell.ColumnIndex = 2 Then
            Dim code As Integer
            code = Microsoft.VisualBasic.AscW(e.KeyChar)
            Dim c As DataGridViewTextBoxEditingControl = DirectCast(sender, DataGridViewTextBoxEditingControl)
            System.Diagnostics.Debug.WriteLine("ControlKeypress: c.TextLength=" + c.TextLength.ToString())
            System.Diagnostics.Debug.WriteLine("ControlKeypress: c.Text ='" + c.Text + "'")
            If code >= 32 And code <= 255 Then
                If c.TextLength > 60 Then
                    Beep()
                    DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex).ErrorText = "Maximale Textlänge erreicht!"
                    'DataGridView1.CurrentCell.ErrorText = "Textende erreicht"
                    e.Handled = True
                End If 'code >= 32
            End If
            If c.TextLength <= 60 Then
                DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex).ErrorText = ""
            End If
        End If
    End Sub

    Private Sub Control_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        'e.KeyChar = e.KeyChar.ToString().ToUpper()(0)
        'check if this is the ArtikelBez Column
        Dim c As DataGridViewTextBoxEditingControl = DirectCast(sender, DataGridViewTextBoxEditingControl)
        'Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        If DataGridView1.CurrentCell.ColumnIndex = 1 Then
            If e.KeyCode = Keys.F3 Then
                DataGridView1.EndEdit()
                SearchForArtikel(DataGridView1)
            End If
        End If
    End Sub

    Private Sub btnDrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDrucken.Click
        'Currently disabled
        'MessageBox.Show("Drucken zur Zeit nicht möglich", "In Bearbeitung")
        'Exit Sub
        Dim dlg
        If readRegUseRawPrinter() = 1 Then
            dlg = New PrintAuftragRAW 'AuftragDrucken
        Else
            dlg = New PrintAuftrag 'AuftragDrucken
        End If
        Dim AuftrNr As Long = CLng(txtAuftragNummer.Text)
        If Not mIsChanged Then
            dlg.AuftragsNummern.Clear()
            dlg.AuftragsNummern.Add(AuftrNr)
            dlg.DruckTyp = "Rechnung"
            dlg.Text = "Rechnung drucken"
            ShowWaitCursor(True)
            dlg.ShowDialog()
            'Buchen
            Dim ant As Integer = MessageBox.Show("Ausdruck OK?", "Aufträge buchen", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If ant = System.Windows.Forms.DialogResult.Yes Then
                Dim c As Long
                c = AuftragBuchen(AuftrNr, txtXTYP.Text, False)
                If c <> -2 Then
                    If c >= 0 Then
                        Status.Text = c & " Buchungen oder Lohn durchgeführt"
                        ReadAuftrag(AuftrNr)
                    Else
                        MessageBox.Show("Konnte Auftrag nicht einbuchen")
                    End If
                Else
                    Debug.Print("Einmal gedruckte Aufträge werden nicht nochmal gebucht")
                    Status.Text = "Einmal gedruckte Aufträge werden nicht nochmal gebucht"
                End If
            End If
            dlg.Dispose()
        Else
            MessageBox.Show("Bitte erst speichern.", "Drucken nicht möglich")
        End If
    End Sub

    Private Sub AnredenListBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AnredenListBox.Click
        txtXAN1.Text = AnredenListBox.SelectedIndex
    End Sub
    ''' <summary>
    ''' enables free entry for Kunden data
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnKundeFREI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKundeFREI.Click, btnKundeFrei2.Click
        If txtXKundenNr.Text.Length = 0 Then
            txtXKundenNr.Text = "999999"
        Else
            Status.Text = "Bitte erst KundenNr entfernen!"
        End If
    End Sub
    ''' <summary>
    ''' enables free entry for Fahrzeug data
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>disabled in v 2.0.0.6</remarks>
    Private Sub btnFahrzeugFREI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFahrzeugFREI.Click
        If FGSTLLNR.Text.Length = 0 Then
            FGSTLLNR.Text = "999999"
        Else
            Status.Text = "Bitte erst FahrgstellNr entfernen!"
        End If
    End Sub

    Private Sub btnArtikelFreiEingabe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnArtikelFreiEingabe.Click
        Dim x As String
        Dim v(7) As String
        'init a tbl to the datasource of the grid
        Dim tbl As DataTable
        Dim bs As BindingSource
        bs = CType(DataGridView1.DataSource, BindingSource)
        If IsNothing(bs) Then 'this is a new auftrag
            kbfakt_start.FillGrid(DataGridView1, "Select Pos, ARTIKELNR, ARTIKELBEZ, ArtTyp, MENGE, E_PREIS, RABATT, " + _
            "(menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt from RECH2 where AUFTR_NR=" & txtAuftragNummer.Text & " order by pos")
            bs = CType(DataGridView1.DataSource, BindingSource)

            'Exit Sub 'there is no Auftrag loaded
        End If
        tbl = CType(bs.DataSource, DataTable)
        With ArtikelListe
            x = "Frei*" & vbTab & 0 & vbTab & 0 & vbTab & 0 & vbTab & 0
            v(0) = CStr(tbl.Rows.Count + 1) 'pos
            v(1) = "Frei*"
            v(2) = "Frei"
            v(3) = 0
            v(4) = "0"
            v(5) = "0"
            v(6) = "0"
            v(7) = 0 'CStr(.menge * .vk - (.rabatt / 100 * (.menge * .vk)))
            tbl.Rows.Add(v)
            Debug.Print("Artikelauswahl = " & x)
        End With
        '"AUFTR_NR, ARTIKELNR, ARTIKELBEZ, KONTO, MENGE, E_PREIS, RABATT, LOHNART, IST_AW, MITARBCODE, FAHRZEUG, TEXT"
        '+ Pos und ArtTyp
        'kbfakt_start.FillGrid(DataGridView1, "Select Pos, ARTIKELNR, ARTIKELBEZ, ArtTyp, MENGE, E_PREIS, RABATT, " + _
        '"(menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt from RECH2 where AUFTR_NR=" & AUFTR_Nr & " order by pos")
        CalcNetto2(tbl)
        DataChanged(True)

    End Sub

    Private Sub btnBuchenTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuchenTest.Click
        Dim ant As Integer = MessageBox.Show("Ausdruck OK?", "Aufträge buchen", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If ant = System.Windows.Forms.DialogResult.Yes Then
            Dim c As Long
            c = AuftragBuchen(txtAuftragNummer.Text, txtXTYP.Text, False)
            If c <> -2 Then
                If c >= 0 Then
                    Status.Text = c & " Buchungen oder Lohn durchgeführt"
                    ReadAuftrag(txtAuftragNummer.Text)
                Else
                    MessageBox.Show("Konnte Auftrag nicht einbuchen")
                End If
            Else
                Debug.Print("Einmal gedruckte Aufträge werden nicht nochmal gebucht")
                Status.Text = "Einmal gedruckte Aufträge werden nicht nochmal gebucht"
            End If
        End If
    End Sub

    Private Sub txtXKZ_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtXKZ.KeyUp
        If e.KeyCode = Keys.F3 Or e.KeyCode = Keys.Enter Then
            KFZSuchen()
        End If
    End Sub
    Private Sub KFZSuchen()
        Dim connection As New OleDbConnection
        Dim queryString As String = "Select FGSTLLNR, KUNDEN_NR from KundFahr where Kennzeich='" & txtXKZ.Text & "'"
        Dim FahrgestellNr As String
        Dim kfzKundenNr As Double
        'Suche FahrgestellNr für Kennzeichen und fülle Kundendaten
        If Not ExistsData(queryString) Then
            FahrzeugSuchen()
        Else
            'FahrgstllNr holen
            Try
                OpenDBConnection(connection)
                Dim command As New OleDbCommand(queryString, connection)
                'connection.Open() is already opened with OpenDBConnection
                Dim reader As OleDbDataReader = command.ExecuteReader()
                reader.Read()
                If reader.HasRows Then
                    FahrgestellNr = reader.Item("FGSTLLNR").ToString
                    kfzKundenNr = CDouble(reader.Item("KUNDEN_NR").ToString)
                    readFahrzeug(FahrgestellNr)
                    ReadKunde(kfzKundenNr)
                    txtXKMSTAND.Focus()
                End If
                reader.Close()
                connection.Close()
            Catch
            End Try
        End If
    End Sub

    Private Sub btnAuftragDrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuftragDrucken.Click
        Dim dlg1
        If readRegUseRawPrinter() = 1 Then
            dlg1 = New PrintAuftragRAW 'AuftragDrucken
        Else
            dlg1 = New PrintAuftrag 'AuftragDrucken
        End If
        If Not mIsChanged Then
            dlg1.AuftragsNummern.Clear()
            dlg1.AuftragsNummern.Add(txtAuftragNummer.Text)
            dlg1.DruckTyp = "Auftrag"
            dlg1.Text = "Auftrag drucken"
            dlg1.ShowDialog()
            dlg1.Dispose()
            'Buchen entfällt bei Aufträgen
        Else
            MessageBox.Show("Bitte erst speichern.", "Drucken nicht möglich")
        End If

    End Sub

    Private Sub btnKVdrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKVdrucken.Click
        Dim dlg
        If readRegUseRawPrinter() = 1 Then
            dlg = New PrintAuftragRAW 'AuftragDrucken
        Else
            dlg = New PrintAuftrag 'AuftragDrucken
        End If
        Dim AuftrNr As Long = CLng(txtAuftragNummer.Text)
        If Not mIsChanged Then
            dlg.AuftragsNummern.Clear()
            dlg.AuftragsNummern.Add(AuftrNr)
            dlg.DruckTyp = "Kostenvoranschlag"
            ShowWaitCursor(True)
            dlg.Text = "Kostenvoranschlag drucken"
            dlg.ShowDialog()
            dlg.Dispose()
            'Buchen entfällt bei Kostenvoranschlägen
        Else
            MessageBox.Show("Bitte erst speichern.", "Drucken nicht möglich")
        End If

    End Sub

    Private Sub btnLieferscheinDrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLieferscheinDrucken.Click
        Dim dlg
        If readRegUseRawPrinter() = 1 Then
            dlg = New PrintAuftragRAW 'AuftragDrucken
        Else
            dlg = New PrintAuftrag 'AuftragDrucken
        End If
        Dim AuftrNr As Long = CLng(txtAuftragNummer.Text)
        If Not mIsChanged Then
            dlg.PreiseAnzeigen = False
            dlg.AuftragsNummern.Clear()
            dlg.AuftragsNummern.Add(AuftrNr)
            dlg.DruckTyp = "Lieferschein"
            ShowWaitCursor(True)
            dlg.Text = "Lieferschein drucken"
            dlg.ShowDialog()
            dlg.Dispose()
            'Buchen entfällt ausser bei Rechnung
        Else
            MessageBox.Show("Bitte erst speichern.", "Drucken nicht möglich")
        End If

    End Sub
    ''' <summary>
    ''' The user entered ZEIT as ArtikelNr
    ''' we now need to know the RichtwertNummer
    ''' </summary>
    ''' <param name="row"></param>
    ''' <remarks>
    ''' Richtwertnummer eingeben
    '''  Richtwertnummer vorhanden
    '''   AW-Menge eingeben
    '''   E-Preis eingeben
    '''   Arbeit wird aufgelistet
    '''  Richtwertnummer nicht vorhanden
    '''   AW-Menge eingeben
    '''   AW-Preis eingeben
    '''   Arbeitsart eingeben
    '''   E-Preis eingeben
    ''' </remarks>
    Private Function RichtwertEingabe(ByVal row As DataGridViewRow) As Boolean
        'determine current cells position
        Dim currCellRectangle As Rectangle = _
            DataGridView1.GetCellDisplayRectangle(DataGridView1.CurrentCell.ColumnIndex, DataGridView1.CurrentCell.RowIndex, True)
        Dim p As Point = New Point(currCellRectangle.Right - 15, currCellRectangle.Top + 8)
        Dim c As Control = DataGridView1.EditingControl

        If FGSTLLNR.Text = "" Then
            MessageBox.Show("Aktueller Fahrzeugtyp ungültig!", "Richtwerte")
            Return False
        End If
        Dim FzgTyp As String
        If FGSTLLNR.Text.Length >= 6 Then
            FzgTyp = txtXTYP.Text.Substring(0, 6)
        Else
            FzgTyp = txtXTYP.Text
        End If

        Dim oben As Integer = Me.Top + DataGridView1.Top '+ p.X
        Dim links As Integer = Me.Left + DataGridView1.Left + p.Y
        Dim rwNr As String
        Dim AwText As String
        Dim Preis As Double
        Dim sPreis As String
        rwNr = fEingabeBox("Richtwert-Nr.:", "Richtwert-Daten", row.Cells("ARTIKELNR").Value, oben, links)

        If rwNr <> "" Then
            If Not ExistsData("select * from Richtwerte2 where ArtNr='" & rwNr & "' AND FzgTyp LIKE '" & FzgTyp & "%'") Then
                'MessageBox.Show("NICHT GEFUNDEN", rwNr)
                AwText = fEingabeBox("Arbeitstext für Richtwert " & rwNr & ":", "Richtwert-Daten 2", "", oben, links)
                If AwText <> "" Then
                    sPreis = fEingabeBox("Preis für Richtwert " & rwNr & ":", "Richtwert-Daten 3", "", oben, links)
                    If sPreis <> "" Then
                        Preis = CDouble(sPreis)
                        row.Cells("ARTIKELNR").Value = rwNr '"ZEIT/" & rwNr
                        row.Cells("ARTIKELBEZ").Value = AwText
                        row.Cells("E_PREIS").Value = Preis.ToString
                        UpdateRichtwerte(rwNr, AwText, sPreis, FzgTyp)
                    End If
                End If
            End If
        Else
            MessageBox.Show("Abgebrochen!")
            Return False
        End If

    End Function

    Private Sub btnRichtwerte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRichtwerte.Click
        Dim dlg As New Richtwerte
        dlg.FzgTyp = txtXTYP.Text
        dlg.ShowDialog()
        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
            Dim x As String
            Dim v(7) As String
            'init a tbl to the datasource of the grid
            Dim tbl As DataTable
            Dim bs As BindingSource
            bs = CType(DataGridView1.DataSource, BindingSource)
            If IsNothing(bs) Then 'this is a new auftrag
                kbfakt_start.FillGrid(DataGridView1, "Select Pos, ARTIKELNR, ARTIKELBEZ, ArtTyp, MENGE, E_PREIS, RABATT, " + _
                "(menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt from RECH2 where AUFTR_NR=" & txtAuftragNummer.Text & " order by pos")
                bs = CType(DataGridView1.DataSource, BindingSource)

                'Exit Sub 'there is no Auftrag loaded
            End If
            tbl = CType(bs.DataSource, DataTable)
            With dlg
                x = .AwNr & vbTab & .AwText & vbTab & .FzgTyp & vbTab & .Menge & .Preis & "0" & vbTab
                v(0) = CStr(tbl.Rows.Count + 1) 'pos
                v(1) = .AwNr
                v(2) = .AwText
                v(3) = "2"
                v(4) = .Menge.ToString
                v(5) = .Preis.ToString
                v(6) = "0" 'rabatt
                v(7) = CStr(.Menge * .Preis)
                tbl.Rows.Add(v)
                Debug.Print("Artikelauswahl = " & x)
            End With
            '"AUFTR_NR, ARTIKELNR, ARTIKELBEZ, KONTO, MENGE, E_PREIS, RABATT, LOHNART, IST_AW, MITARBCODE, FAHRZEUG, TEXT"
            '+ Pos und ArtTyp
            'kbfakt_start.FillGrid(DataGridView1, "Select Pos, ARTIKELNR, ARTIKELBEZ, ArtTyp, MENGE, E_PREIS, RABATT, " + _
            '"(menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt from RECH2 where AUFTR_NR=" & AUFTR_Nr & " order by pos")
            CalcNetto2(tbl)
            DataChanged(True)
        End If
        dlg.Close()

    End Sub

    Private Sub btnRichtwertEingabe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRichtwertEingabe.Click
        Dim x As String
        Dim v(7) As String
        'init a tbl to the datasource of the grid
        Dim tbl As DataTable
        Dim bs As BindingSource
        bs = CType(DataGridView1.DataSource, BindingSource)
        If IsNothing(bs) Then 'this is a new auftrag
            kbfakt_start.FillGrid(DataGridView1, "Select Pos, ARTIKELNR, ARTIKELBEZ, ArtTyp, MENGE, E_PREIS, RABATT, " + _
            "(menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt from RECH2 where AUFTR_NR=" & txtAuftragNummer.Text & " order by pos")
            bs = CType(DataGridView1.DataSource, BindingSource)
        End If
        tbl = CType(bs.DataSource, DataTable)
        With ArtikelListe
            x = "ZEIT/" & vbTab & 0 & vbTab & 0 & vbTab & 0 & vbTab & 0
            v(0) = CStr(tbl.Rows.Count + 1) 'pos
            v(1) = "ZEIT/"
            v(2) = "Richtwerttext"
            v(3) = 0
            v(4) = "0"
            v(5) = "0"
            v(6) = "0"
            v(7) = 0 'CStr(.menge * .vk - (.rabatt / 100 * (.menge * .vk)))
            tbl.Rows.Add(v)
            Debug.Print("btnRichtwertEingabe = " & x)
        End With
        '"AUFTR_NR, ARTIKELNR, ARTIKELBEZ, KONTO, MENGE, E_PREIS, RABATT, LOHNART, IST_AW, MITARBCODE, FAHRZEUG, TEXT"
        '+ Pos und ArtTyp
        'kbfakt_start.FillGrid(DataGridView1, "Select Pos, ARTIKELNR, ARTIKELBEZ, ArtTyp, MENGE, E_PREIS, RABATT, " + _
        '"(menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt from RECH2 where AUFTR_NR=" & AUFTR_Nr & " order by pos")
        CalcNetto2(tbl)
        DataChanged(True)
    End Sub

    Private Sub btnArtikelHilfe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnArtikelHilfe.Click
        MessageBox.Show("Artikel mit + am Ende werden beim Buchen neu angelegt, " & vbCrLf & _
            "Artikel mit * am Ende sind frei, " & vbCrLf & _
            "F3 für Artikelsuche, " & vbCrLf & _
            "'ZEIT/' plus Kennung (zB 'ZEIT/33.21' ermöglicht die Neuanlage eines Richtwerts," & vbCrLf & _
            "'ZEIT/' öffnet den Richtwertauswahldialog", "Artikeleingabe")
    End Sub

    Private Sub AddEmptyLine()
        'Dim x As String
        Dim v(7) As String
        'init a tbl to the datasource of the grid
        Dim tbl As DataTable
        Dim bs As BindingSource
        bs = CType(DataGridView1.DataSource, BindingSource)
        If IsNothing(bs) Then 'this is a new auftrag
            kbfakt_start.FillGrid(DataGridView1, "Select Pos, ARTIKELNR, ARTIKELBEZ, ArtTyp, MENGE, E_PREIS, RABATT, " + _
            "(menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt from RECH2 where AUFTR_NR=" & txtAuftragNummer.Text & " order by pos")
            bs = CType(DataGridView1.DataSource, BindingSource)
            tbl = CType(bs.DataSource, DataTable)
            Dim i
            Debug.Print("### AddEmptyLine -  tbl namen:" + vbCrLf)
            For i = 0 To tbl.Columns.Count - 1
                Debug.Print(tbl.Columns(i).ToString + vbCrLf)
            Next
            v(0) = CStr(tbl.Rows.Count + 1) 'pos
            v(1) = ""
            v(2) = ""
            v(3) = "0"
            v(4) = "0"
            v(5) = "0"
            v(6) = "0"
            v(7) = "0"
            tbl.Rows.Add(v)
            CalcNetto2(tbl)
            'DataChanged(True)
        Else
            tbl = CType(bs.DataSource, DataTable)
            Dim i
            Debug.Print("### AddEmptyLine -  tbl namen:" + vbCrLf)
            For i = 0 To tbl.Columns.Count - 1
                Debug.Print(tbl.Columns(i).ToString + vbCrLf)
            Next
            v(0) = CStr(tbl.Rows.Count + 1) 'pos
            v(1) = ""
            v(2) = ""
            v(3) = "0"
            v(4) = "0"
            v(5) = "0"
            v(6) = "0"
            v(7) = "0"
            tbl.Rows.Add(v)
            CalcNetto2(tbl)
            'DataChanged(True)
        End If

    End Sub

    Private Sub DataGridView1_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles DataGridView1.KeyPress
    End Sub

    Private Sub btnGutschriftDrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGutschriftDrucken.Click
        'Currently disabled
        'MessageBox.Show("Drucken zur Zeit nicht möglich", "In Bearbeitung")
        'Exit Sub
        Dim dlg
        If readRegUseRawPrinter() = 1 Then
            dlg = New PrintAuftragRAW 'AuftragDrucken
        Else
            dlg = New PrintAuftrag 'AuftragDrucken
        End If
        Dim AuftrNr As Long = CLng(txtAuftragNummer.Text)
        If Not mIsChanged Then
            dlg.AuftragsNummern.Clear()
            dlg.AuftragsNummern.Add(AuftrNr)
            dlg.DruckTyp = "Gutschrift"
            ShowWaitCursor(True)
            dlg.Text = "Gutschrift drucken"
            dlg.ShowDialog()
            'Buchen
            Dim ant As Integer = MessageBox.Show("Ausdruck OK?", "Guschrift buchen", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If ant = System.Windows.Forms.DialogResult.Yes Then
                Dim c As Long
                c = AuftragBuchen(AuftrNr, txtXTYP.Text, True)
                If c <> -2 Then
                    If c >= 0 Then
                        Status.Text = c & " Buchungen oder Lohn durchgeführt"
                        ReadAuftrag(AuftrNr)
                    Else
                        MessageBox.Show("Konnte Auftrag nicht einbuchen")
                    End If
                Else
                    Debug.Print("Einmal gedruckte Aufträge werden nicht nochmal gebucht")
                    Status.Text = "Einmal gedruckte Aufträge werden nicht nochmal gebucht"
                End If
            End If
            dlg.Dispose()
        Else
            MessageBox.Show("Bitte erst speichern.", "Drucken nicht möglich")
        End If
    End Sub


    Private Sub picGedruckt_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picGedruckt.DoubleClick
        If chkGedruckt.Checked Then
            If (LoginForm2.ShowDialog = Windows.Forms.DialogResult.OK) Then
                chkGedruckt.Checked = False
            End If
        End If
    End Sub
    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim dlg As New PrintAuftrag ' PrintAuftrag 'AuftragDrucken
        Dim AuftrNr As Long = CLng(txtAuftragNummer.Text)
        If Not mIsChanged Then
            dlg.AuftragsNummern.Clear()
            dlg.AuftragsNummern.Add(AuftrNr)
            dlg.DruckTyp = "Rechnung"
            dlg.Text = "Rechnung drucken"
            ShowWaitCursor(True)
            dlg.ShowDialog()
            'Buchen
            Dim ant As Integer = MessageBox.Show("Ausdruck OK?", "Aufträge buchen", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If ant = System.Windows.Forms.DialogResult.Yes Then
                Dim c As Long
                c = AuftragBuchen(AuftrNr, txtXTYP.Text, False)
                If c <> -2 Then
                    If c >= 0 Then
                        Status.Text = c & " Buchungen oder Lohn durchgeführt"
                        ReadAuftrag(AuftrNr)
                    Else
                        MessageBox.Show("Konnte Auftrag nicht einbuchen")
                    End If
                Else
                    Debug.Print("Einmal gedruckte Aufträge werden nicht nochmal gebucht")
                    Status.Text = "Einmal gedruckte Aufträge werden nicht nochmal gebucht"
                End If
            End If
            dlg.Dispose()
        Else
            MessageBox.Show("Bitte erst speichern.", "Drucken nicht möglich")
        End If
    End Sub

End Class
