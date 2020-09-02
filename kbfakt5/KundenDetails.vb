Imports System.Data.OleDb
Public Class KundenDetails
    'kbfakt_start.FillGrid(GridAuftraege, "Select KUNDENNR, Anreden.ANREDE , VORNAME, " + _
    '                                    "NACHNAME, BRANCHE, PLZ, " + _
    '                                    "ORT , STRASSE , ANSPRECHP, TELEFON," + _
    '                                    "KONTO1 from KUNDENST, Anreden " + _
    '                                    "where KUNDENST.Anrede=Anreden.id " + _
    '                                    "order by KUNDENNR")
    Private mIsChanged As Boolean = False
    Private mIsNeuKunde As Boolean = False
    Private mKundenName2 As String = ""
    Private mKundenName As String = ""
    Private mKundenNr As Long
    Private AnredenTxtBoxID As Integer = 0
    Private AnredenListBox As New ComboBox
    Private firstrun As Boolean = True
    Private Structure Kunde
        Dim inhalt As String
        Dim FeldName As String
        Dim displayName As String
        Dim tblName As String
        Dim Feldtyp As Type
        Dim id As Integer
        Dim AllowNull As Boolean
    End Structure
    Private kunden() As Kunde
    Public KundenNr As Double

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If mIsChanged Then
            If MessageBox.Show("Datenänderungen verwerfen?", "Kunden", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then Me.Close()
        Else
            Me.Close()
        End If
    End Sub
    Private Sub MaskeErstellen(ByVal knr As Long)
        Debug.Print("################################" + vbCr & Panel1.Controls.Count & vbCr)
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim dbCmd As New OleDbCommand
        'Dim dbda As OleDbDataAdapter
        'dbda = New OleDbDataAdapter
        dbCmd.Connection = cn
        Dim queryString As String
        queryString = "Select KUNDENNR, Anreden.ANREDE , VORNAME, " + _
                                    "NACHNAME, BRANCHE, PLZ, " + _
                                    "ORT , STRASSE , ANSPRECHP, TELEFON," + _
                                    "KONTO1, kundenst.id from KUNDENST, Anreden " + _
                                    "where KUNDENNR=" & knr & _
                                    " AND KUNDENST.Anrede=Anreden.id "
        '"order by KUNDENNR"
        Dim i As Integer
        Dim colNr As Integer = 0
        Dim ofsCol As Integer = 13
        Dim rowNr As Integer = 0
        Dim cmd As New OleDbCommand(queryString, cn)
        Dim ds As New DataSet
        Dim da As New OleDbDataAdapter
        Dim cmb As New OleDbCommandBuilder(da)
        da.SelectCommand = cmd
        da.Fill(ds, "KUNDENST") 'ds.tables("KundFahr") enthält nun die Daten
        Dim table As System.Data.DataTable
        table = ds.Tables("KUNDENST")
        ReDim kunden(table.Columns.Count)
        Dim txtBox(table.Columns.Count) As TextBox
        Dim lblText(table.Columns.Count) As Label
        'connection.Open() is already opened with OpenDBConnection
        For i = 0 To table.Columns.Count - 1
            kunden(i).Feldtyp = table.Columns(i).DataType
            kunden(i).FeldName = table.Columns(i).ColumnName.ToLower
            kunden(i).id = i
            kunden(i).AllowNull = table.Columns(i).AllowDBNull

            txtBox(i) = New TextBox
            lblText(i) = New Label

            lblText(i).Width = 80
            lblText(i).Left = colNr * Panel1.Width / 2
            Panel1.Controls.Add(lblText(i))

            lblText(i).Top = rowNr * 26
            lblText(i).Text = table.Columns(i).ColumnName + ":"
            Panel1.Controls.Add(txtBox(i))

            txtBox(i).Top = rowNr * 26
            txtBox(i).Width = 160
            txtBox(i).Left = 80 + (colNr * Panel1.Width / 2)

            If kunden(i).FeldName.ToLower = "anrede" Then
                txtBox(i).Width = 80
                txtBox(i).Enabled = False
                FillAnredenListbox(AnredenListBox)
                AnredenListBox.Left = txtBox(i).Left + txtBox(i).Width + 13 + (colNr * Panel1.Width / 2)
                AnredenListBox.Top = txtBox(i).Top
                AnredenListBox.Width = 60
                AnredenListBox.DropDownStyle = ComboBoxStyle.DropDownList
                AnredenTxtBoxID = i
                AddHandler AnredenListBox.SelectedIndexChanged, AddressOf AnredeListBoxChanged
                Panel1.Controls.Add(AnredenListBox)
            End If
            txtBox(i).Tag = kunden(i).FeldName.ToString.ToLower
            If txtBox(i).Tag = "id" Or txtBox(i).Tag = "kundennr" Then
                txtBox(i).Enabled = False
            End If
            rowNr += 1
            If txtBox(i).Top + txtBox(i).Height + 26 > Panel1.Height Then
                colNr += 1
                rowNr = 0
            End If

        Next i
        cn.Close()
        addChangedHandler()
        mIsNeuKunde = False
        DataChanged(False)
        DataChangedStatus.Text = "Kunde geladen"
    End Sub
    Private Sub ReadKunde(ByVal kNr As Integer)
        Debug.Print("################################" + vbCr & Panel1.Controls.Count & vbCr)
        'For Each ctrl As Control In Panel1.Controls
        '    If Not TypeOf (ctrl) Is ListBox Then
        '        Panel1.Controls.Remove(ctrl)
        '        ctrl.Dispose()
        '    End If
        'Next
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim dbCmd As New OleDbCommand
        'Dim dbda As OleDbDataAdapter
        'dbda = New OleDbDataAdapter
        dbCmd.Connection = cn
        Dim queryString As String
        queryString = "Select KUNDENNR, Anreden.ANREDE , VORNAME, " + _
                                    "NACHNAME, BRANCHE, PLZ, " + _
                                    "ORT , STRASSE , ANSPRECHP, TELEFON," + _
                                    "KONTO1, kundenst.id from KUNDENST, Anreden " + _
                                    "where KUNDENNR=" & kNr & _
                                    " AND KUNDENST.Anrede=Anreden.id "
        '"order by KUNDENNR"
        Dim i As Integer
        Dim colNr As Integer = 0
        Dim ofsCol As Integer = 13
        Dim rowNr As Integer = 0
        Dim command As New OleDbCommand(queryString, cn)
        'connection.Open() is already opened with OpenDBConnection
        Dim reader As OleDbDataReader = command.ExecuteReader()
        reader.Read() 'try to read first Kunde
        mKundenName2 = ""
        mKundenName = ""
        mKundenNr = 0
        If reader.HasRows Then
            mKundenName = reader.Item("VORNAME").ToString
            mKundenName2 = reader.Item("NACHNAME").ToString
            mKundenNr = CLong(reader.Item("KundenNr").ToString)
            For Each c As Control In Panel1.Controls
                If TypeOf c Is TextBox Then
                    i = findIdforKunde(kunden, c.Tag) 'get the id to access the fahrzeug data
                    kunden(i).inhalt = reader(c.Tag).ToString 'read column data into right fahrzeug data

                    'special for zulassung1, which is real dateTime field
                    If kunden(i).Feldtyp.ToString = "System.DateTime" Then
                        Try
                            c.Text = Format(CDate(kunden(i).inhalt), "d")
                        Catch
                            c.Text = ""
                        End Try
                    Else
                        c.Text = kunden(i).inhalt
                    End If

                    Debug.Print("ReadKunde: " & kunden(i).FeldName & " - " & kunden(i).Feldtyp.ToString)

                End If
            Next c 'next control
        End If
        reader.Close()
        FillFahrzeuge()
        mIsNeuKunde = False
        DataChanged(False)
        DataChangedStatus.Text = "Kunde geladen"
    End Sub
    Private Function findIdforKunde(ByRef f() As Kunde, ByVal s As String) As Integer
        Dim x
        For x = 0 To f.Length - 1
            If f(x).FeldName = s Then
                findIdforKunde = x
                Exit Function
            End If
        Next
        findIdforKunde = 0
    End Function

    Private Sub KundenDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If KundenNr = 0 Then
            KundenNr = GetMaxKundenNr()
            MaskeErstellen(KundenNr)
            NeuKunde()
        Else
            MaskeErstellen(KundenNr)
            ReadKunde(KundenNr)
        End If
    End Sub

    Sub FillFahrzeuge()
        Dim fields As String = "KENNZEICH as Kennzeichen, FGSTLLNR as Fahrgestellnr, TYP as Typ, KM_STAND as [km Stand], KFZ_BRF_NR as [Kfz-Brief], ZULASSUNG1 as Zulassung, TUEV as TÜV, ASU,  SCHREIBER,  SICHER"
        kbfakt_start.FillGrid(dgvFahrzeuge, "Select " & fields & _
                                            " from KundFahr where Kunden_Nr=" & KundenNr & " order by KENNZEICH")
        dgvFahrzeuge.ReadOnly = True
        dgvFahrzeuge.RowHeadersVisible = False
        If dgvFahrzeuge.RowCount = 0 Then
            btnKfzDetails.Enabled = False
            btnDeleteKfz.Enabled = False
        Else
            btnKfzDetails.Enabled = True
            btnDeleteKfz.Enabled = True
        End If
    End Sub
    Sub FillAnredenListbox(ByVal ctl As ComboBox)
        Dim queryString As String
        Dim idx As Integer
        queryString = "Select ID, Anrede from Anreden ORDER by ID"
        Dim cn As New OleDbConnection
        OpenDBConnection(cn)
        ctl.Items.Clear()
        Using cn
            Dim command As New OleDbCommand(queryString, cn)
            Dim reader As OleDbDataReader = command.ExecuteReader()
            While reader.Read()
                idx = ctl.Items.Add(reader("Anrede").ToString)

            End While
            reader.Close()
        End Using
        ctl.SelectedIndex = 4
        cn.Close()
    End Sub

    ''' <summary>
    ''' will add the changed event handler to every text control
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addChangedHandler()
        Dim txtbox As TextBox = Nothing
        'Dim dgv As DataGridView = Nothing
        Dim ctrl As Control
        For Each ctrl In Me.Panel1.Controls
            If TypeOf ctrl Is TextBox Then
                txtbox = CType(ctrl, TextBox)
                AddHandler txtbox.ModifiedChanged, AddressOf TextBoxChanged
                'If txtbox.Name = "txtXName1" Then
                '    Debug.Print("txtXName1")
                'End If
                'ElseIf TypeOf ctrl Is DataGridView Then
                '    dgv = CType(ctrl, DataGridView)
                '    AddHandler dgv.CellValueChanged, AddressOf DataGridChanged
            End If
        Next
    End Sub
    ''' <summary>
    ''' TextBoxChanged event handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TextBoxChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DataChanged(True)
    End Sub
    Private Sub AnredeListBoxChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ctrl As Control
        For Each ctrl In Me.Panel1.Controls
            If TypeOf ctrl Is TextBox Then
                If ctrl.Tag = "anrede" Then
                    ctrl.Text = sender.SelectedItem.ToString
                    DataChanged(True)
                End If
            End If
        Next
    End Sub
    Private Sub DataChanged(ByVal b As Boolean)
        If b Then
            DataChangedStatus.BackColor = Color.Red
            mIsChanged = True
        Else
            DataChangedStatus.BackColor = Color.Green
            mIsChanged = False
        End If
        btnNeu.Enabled = Not mIsChanged
        btnAbbrechen.Enabled = mIsChanged
        btnSpeichern.Enabled = mIsChanged
        btnSuchen.Enabled = Not mIsChanged
    End Sub

    Private Sub btnAbbrechen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbbrechen.Click
        If mIsChanged And mIsNeuKunde = False Then
            If MessageBox.Show("Datenänderungen verwerfen?", "Schliessen", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                ReadKunde(KundenNr)
            End If
        End If
        If mIsNeuKunde Then
            If MessageBox.Show("Eingaben verwerfen?", "Neukunde", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                ReadKunde(KundenNr) 'mIsNeuKunde = False
            End If
        End If
    End Sub
    Private Function KundenNeuSpeichern() As Integer
        'we need a connection, a dataset and a dataAdapter
        Dim anzahl As Integer
        Dim cn As New OleDbConnection
        OpenDBConnection(cn)
        Dim cmd As New OleDbCommand("select * from kundenst", cn)
        Dim ds As New DataSet
        Dim da As New OleDbDataAdapter
        Dim cmb As New OleDbCommandBuilder(da)
        da.SelectCommand = cmd
        da.Fill(ds, "KundenSt")

        'add the primary key to the dataset
        Dim dcs(1) As DataColumn
        dcs(0) = ds.Tables("KundenSt").Columns("id")
        ds.Tables("KundenSt").PrimaryKey = dcs

        'Add the row locally
        Dim drw1 As DataRow = _
            ds.Tables("KundenSt").NewRow()
        drw1("ID") = Int32.MaxValue
        For Each ctrl As Control In Panel1.Controls
            If TypeOf ctrl Is TextBox And ctrl.Tag <> "id" Then
                If ctrl.Tag <> "anrede" Then
                    drw1(ctrl.Tag) = ctrl.Text
                Else
                    drw1(ctrl.Tag) = AnredenListBox.SelectedIndex
                End If
                'save the new kundennr
                If ctrl.Tag = "kundennr" Then KundenNr = CDbl(ctrl.Text)
            End If
        Next

        'Include an event to fill in the autonumber value
        AddHandler daRech1.RowUpdated, _
            New OleDb.OleDbRowUpdatedEventHandler( _
            AddressOf OnRowUpdated)

        ds.Tables("KundenSt").Rows.Add(drw1)
        'Update the Access database file
        anzahl = da.Update(ds, "KundenSt")
        ds.AcceptChanges()
        cn.Close()
        KundenNeuSpeichern = anzahl
        DataChangedStatus.Text = anzahl & " Kunde gespeichert"
        mIsNeuKunde = False
        btnKfzDetails.Enabled = False
        btnKfzNeu.Enabled = True
        btn_RechnungenKunde.Enabled = True
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
    Function DatenPruefen() As Boolean
        DatenPruefen = True
        For Each ctrl As Control In Panel1.Controls
            If TypeOf ctrl Is TextBox Then
                If ctrl.Tag <> "id" And _
                    ctrl.Tag <> "vorname" And _
                    ctrl.Tag <> "branche" And _
                    ctrl.Tag <> "telefon" And _
                    ctrl.Tag <> "konto1" Then
                    If ctrl.Text = "" Then
                        ctrl.Focus()
                        ctrl.BackColor = Color.Red
                        DatenPruefen = False
                        Exit For
                    Else
                        ctrl.BackColor = Color.Empty
                    End If
                End If
            End If
        Next
    End Function
    Private Sub NeuKunde()
        AnredenListBox.SelectedIndex = 4
        For Each ctrl As Control In Panel1.Controls
            If TypeOf ctrl Is TextBox Then
                ctrl.Text = ""
                If ctrl.Tag = "kundennr" Then
                    ctrl.Text = GetMaxKundenNr() + 1
                End If
                If ctrl.Tag = "anrede" Then ctrl.Text = AnredenListBox.SelectedItem.ToString
            End If
            btnSpeichern.Enabled = True
            btnAbbrechen.Enabled = True
            mIsChanged = True
            DataChangedStatus.Text = "Neuer Kunde"
            dgvFahrzeuge.DataSource = Nothing
        Next
        mIsNeuKunde = True
        btnNeu.Enabled = False
        btnSuchen.Enabled = False
        btnKfzDetails.Enabled = False
        btnKfzNeu.Enabled = False
        btn_RechnungenKunde.Enabled = False
    End Sub
    Private Sub btnNeu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNeu.Click
        NeuKunde()
    End Sub
    Private Function KundeSpeichern()
        'we need a connection, a dataset and a dataAdapter
        Dim anzahl As Integer
        Dim cn As New OleDbConnection
        OpenDBConnection(cn)
        Dim cmd As New OleDbCommand("select * from kundenst", cn)
        Dim ds As New DataSet
        Dim da As New OleDbDataAdapter
        Dim cmb As New OleDbCommandBuilder(da)
        da.SelectCommand = cmd
        da.Fill(ds, "KundenSt")

        'add the primary key to the dataset
        Dim dcs(1) As DataColumn
        dcs(0) = ds.Tables("KundenSt").Columns("id")
        ds.Tables("KundenSt").PrimaryKey = dcs

        'find the row locally
        Dim KundenID As Long = 0
        For Each ctrl As Control In Panel1.Controls
            If ctrl.Tag = "id" Then
                KundenID = CLong(ctrl.Text)
            End If
        Next
        If KundenID = 0 Then
            KundeSpeichern = 0
            Exit Function
        End If

        Dim drw1 As DataRow = ds.Tables("kundenst").Rows.Find(KundenID)
        'Dim drw1 As DataRow = ds.Tables("KundenSt").NewRow()
        'drw1("ID") = Int32.MaxValue
        For Each ctrl As Control In Panel1.Controls
            If TypeOf ctrl Is TextBox And ctrl.Tag <> "id" Then
                If ctrl.Tag <> "anrede" Then
                    drw1(ctrl.Tag) = ctrl.Text
                Else
                    drw1(ctrl.Tag) = AnredenListBox.SelectedIndex
                End If
            End If
        Next

        'Update the Access database file
        anzahl = da.Update(ds, "KundenSt")
        ds.AcceptChanges()
        cn.Close()
        KundeSpeichern = anzahl
        DataChangedStatus.Text = anzahl & " Kunde gespeichert"
        mIsNeuKunde = False
        btnKfzNeu.Enabled = True
    End Function
    Private Sub btnSpeichern_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSpeichern.Click
        If DatenPruefen() Then
            If mIsNeuKunde Then
                KundenNeuSpeichern()
            Else
                KundeSpeichern()
            End If
            DataChanged(False)
        Else
            DataChangedStatus.Text = "Eingaben unvollständig"
            DataChangedStatus.BackColor = Color.Red
        End If
    End Sub
    Function GetMaxKundenNr() As Integer
        'finde max auftragsnummer
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim dbCmd As New OleDbCommand
        'Dim dbda As OleDbDataAdapter
        'dbda = New OleDbDataAdapter
        dbCmd.Connection = cn
        dbCmd.CommandText = "select MAX(KundenNR) from KundenSt"
        Dim MaxKundenNr As Integer
        MaxKundenNr = CInt(dbCmd.ExecuteScalar())
        cn.Close()
        Return MaxKundenNr
    End Function

    Private Sub btnSuchen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuchen.Click
        Kundenliste.ShowDialog()
        If Kundenliste.Kunden_nr <> 0 Then
            KundenNr = Kundenliste.Kunden_nr
            ReadKunde(KundenNr)
        End If
        Kundenliste.Dispose()
    End Sub

    Private Sub btnKfzDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKfzDetails.Click
        Dim FgstellNr As String = ""
        If Not IsNothing(dgvFahrzeuge.CurrentCell) Then
            FgstellNr = dgvFahrzeuge.Item(1, dgvFahrzeuge.CurrentCell.RowIndex).Value
        End If
        FahrzeugDetails.KundenNr = KundenNr
        If FgstellNr <> "" Then
            FahrzeugDetails.FahrgestellNr = FgstellNr 'if FahrgestellNr is empty, the neu fahrzeug dialog will be shown
            FahrzeugDetails.ShowDialog()
            ReadKunde(KundenNr)
            FahrzeugDetails.Dispose()
        End If
    End Sub

    Private Sub btnDeleteKfz_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteKfz.Click
        Dim FgstellNr As String = ""
        If Not IsNothing(dgvFahrzeuge.CurrentCell) Then
            FgstellNr = dgvFahrzeuge.Item(1, dgvFahrzeuge.CurrentCell.RowIndex).Value
            If MessageBox.Show("Daten für Fahrzeug " & FgstellNr & " wirklich löschen?", "Fahrzeug löschen", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then Exit Sub
            Dim anzahl As Integer
            Dim cn As New OleDbConnection
            OpenDBConnection(cn)
            Dim cmd As New OleDbCommand("delete from KundFahr where fgstllnr='" & FgstellNr & "'", cn)
            anzahl = cmd.ExecuteNonQuery
            cn.Close()
            MessageBox.Show(anzahl & " Fahrzeug gelöscht")
            ReadKunde(KundenNr)
        End If

    End Sub

    Private Sub btnKfzNeu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKfzNeu.Click
        FahrzeugDetails.KundenNr = KundenNr
        FahrzeugDetails.FahrgestellNr = ""
        FahrzeugDetails.ShowDialog()
        ReadKunde(KundenNr)
        FahrzeugDetails.Dispose()
        FillFahrzeuge()
    End Sub

    Private Sub btn_RechnungenKunde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_RechnungenKunde.Click
        ListenAnsicht.KundenNr = KundenNr
        ListenAnsicht.ShowDialog()
        ListenAnsicht.Dispose()
    End Sub

    Private Sub btn_RechnungenKFZ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_RechnungenKFZ.Click
        Dim FgstellNr As String = ""
        If Not IsNothing(dgvFahrzeuge.CurrentCell) Then
            FgstellNr = dgvFahrzeuge.Item(1, dgvFahrzeuge.CurrentCell.RowIndex).Value
            Dim dlg As ListenAnsicht
            dlg = New ListenAnsicht
            dlg.RechnungsNr = 0
            dlg.KundenNr = 0
            dlg.FahrgestellNr = FgstellNr
            dlg.Show()
        End If
    End Sub

    Private Sub btnOffeneRgen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOffeneRgen.Click
        Dim dlg As OffeneRechnungen
        dlg = New OffeneRechnungen
        dlg.DateTimePicker1.Enabled = False
        dlg.btnChange.Enabled = False
        dlg.SqlString = "Select XAUFTR_NR as Auftrag, XNAME1 as Name1, XNAME2 as Name2, XDATUM as RgDatum," & _
        " XFGSTLLNR as FahrgestNr, XNETTO as Netto from Rech1 where Bezahlt=FALSE and Gedruckt=TRUE and XKundenNr=" & KundenNr
        dlg.SqlStringSumme = "Select sum(xnetto) from rech1 where Bezahlt=FALSE and Gedruckt=TRUE and XKundenNr=" & KundenNr
        dlg.ShowDialog()
        dlg.Dispose()

    End Sub

    Private Sub btnDeleteKunde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteKunde.Click
        If mKundenNr = 0 Then
            MessageBox.Show("KundenNr ungültig. Bitte Daten prüfen.")
            Return
        End If
        If mIsChanged Or mIsNeuKunde Then
            MessageBox.Show("Bitte Vorgang erst abschliessen (Speichern oder Abbrechen).", "Kunde löschen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If
        If MessageBox.Show("Kunde >" & mKundenName2 & ", " & mKundenNr & "< und ALLE Fahrrzeuge dazu wirklich löschen?", "Kunde löschen", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Return
        End If
        'Delete Kunde
        Dim trn As OleDbTransaction
        Dim cn As New OleDb.OleDbConnection
        Dim dbCmd As New OleDbCommand
        OpenDBConnection(cn)
        'transaction
        trn = cn.BeginTransaction()
        Try
            dbCmd.Connection = cn
            dbCmd.Transaction = trn

            dbCmd.CommandText = "delete from KundenSt where KundenNr=" & mKundenNr
            Dim anz As Integer = dbCmd.ExecuteNonQuery()
            MessageBox.Show("Anzahl gelöschter Daten=" & anz)
            dbCmd.CommandText = "delete from KundFahr where Kunden_Nr=" & mKundenNr
            Dim anz2 = dbCmd.ExecuteNonQuery()
            MessageBox.Show("Anzahl gelöschter Kunden=" & anz & vbCrLf & "Anzahl gelöschter Fahrzeuge = " & anz2 & vbCrLf & "KundenNr war " & mKundenNr)
            trn.Commit()
            cn.Close()
            ReadKunde(GetMaxKundenNr())
        Catch x As OleDb.OleDbException
            trn.Rollback()
            MessageBox.Show("Fehler " & x.Message & vbCrLf & "Transaktion abgebrochen!", "Exception in DeleteKunde")
        End Try
    End Sub

    Private Sub btnDruckenFahrzeuge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDruckenFahrzeuge.Click
        If SetupThePrinting() Then
            Dim MyPrintPreviewDialog As New PrintPreviewDialog()
            MyPrintPreviewDialog.Document = MyPrintDocument
            ' Create a Rectangle object that will be used as the bound of the form.
            'Dim scr As Screen = System.Windows.Forms.Screen.PrimaryScreen
            'MyPrintPreviewDialog.DesktopBounds = scr.WorkingArea
            MyPrintPreviewDialog.WindowState = FormWindowState.Maximized
            MyPrintPreviewDialog.ShowDialog()
        End If
    End Sub
    Private WithEvents MyPrintDocument As System.Drawing.Printing.PrintDocument
    'The class that will do the printing process.
    Private MyDataGridViewPrinter As DataGridViewPrinter
    'Private WithEvents printdlg As System.Windows.Forms.PrintPreviewDialog

    ' The PrintPage action for the PrintDocument control
    Private Sub MyPrintDocument_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim more As Boolean = MyDataGridViewPrinter.DrawDataGridView(e.Graphics)
        If more = True Then
            e.HasMorePages = True
        End If
    End Sub
    ' The Print Preview Button
    'Private Sub btnPrintPreview_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    If SetupThePrinting() Then
    '        Dim MyPrintPreviewDialog As New PrintPreviewDialog()
    '        MyPrintPreviewDialog.Document = MyPrintDocument
    '        MyPrintPreviewDialog.ShowDialog()
    '    End If
    'End Sub

    Private Function SetupThePrinting() As Boolean
        Dim MyPrintDialog As New PrintDialog()

        MyPrintDocument = New System.Drawing.Printing.PrintDocument
        AddHandler MyPrintDocument.PrintPage, AddressOf Me.MyPrintDocument_PrintPage

        MyPrintDialog.AllowCurrentPage = False
        MyPrintDialog.AllowPrintToFile = False
        MyPrintDialog.AllowSelection = False
        MyPrintDialog.AllowSomePages = False
        MyPrintDialog.PrintToFile = False
        MyPrintDialog.ShowHelp = False
        MyPrintDialog.ShowNetwork = False


        If (MyPrintDialog.ShowDialog() <> Windows.Forms.DialogResult.OK) Then
            Return False
        End If

        MyPrintDocument.DocumentName = "Kundenfahrzeuge für " + mKundenName + " " + mKundenName2
        MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings
        MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings
        MyPrintDocument.DefaultPageSettings.Margins = New System.Drawing.Printing.Margins(40, 40, 40, 40)
        MyPrintDocument.DefaultPageSettings.Landscape = True

        MyDataGridViewPrinter = New DataGridViewPrinter(Me.dgvFahrzeuge, MyPrintDocument, True, True, "Kundenfahrzeuge für " + mKundenName + " " + mKundenName2, New Font("Tahoma", 18, FontStyle.Bold, GraphicsUnit.Point), _
        Color.Black, True)
        'If MessageBox.Show("Do you want the report to be centered on the page", "InvoiceManager - Center on Page", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
        '    MyDataGridViewPrinter = New DataGridViewPrinter(Me.DataGridView1, MyPrintDocument, True, True, "Customers", New Font("Tahoma", 18, FontStyle.Bold, GraphicsUnit.Point), _
        '    Color.Black, True)
        'Else
        '    MyDataGridViewPrinter = New DataGridViewPrinter(Me.DataGridView1, MyPrintDocument, False, True, "Customers", New Font("Tahoma", 18, FontStyle.Bold, GraphicsUnit.Point), _
        '    Color.Black, True)
        'End If

        Return True
    End Function

    Private Sub btnExport2Csv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport2Csv.Click
        DataGridViewExport(Me.dgvFahrzeuge)
    End Sub
End Class