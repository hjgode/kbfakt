Imports System.Data.OleDb
Public Class FahrzeugDetails
    Public FahrgestellNr As String
    Public KundenNr As Double
    Private mIsChanged As Boolean = False
    Private mIsNew As Boolean = False
    Private Structure Fahrzeug
        Dim inhalt As String
        Dim FeldName As String
        Dim displayName As String
        Dim tblName As String
        Dim Feldtyp As Type
        Dim id As Integer
        Dim AllowNull As Boolean
    End Structure
    Dim Fahrzeuge() As Fahrzeug
    Private Sub MaskeErstellen(ByVal KundNr As Double)
        Panel1.Controls.Clear()
        'For Each ctrl As Control In Panel1.Controls
        '    Panel1.Controls.Remove(ctrl)
        '    ctrl.Dispose()
        '    Debug.Print("Removed ctrl " & ctrl.Text)
        'Next
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim dbCmd As New OleDbCommand
        'need table
        Dim cmd As New OleDbCommand("select " & fieldsKundfahr & ", id from kundfahr", cn)
        Dim ds As New DataSet
        Dim da As New OleDbDataAdapter
        Dim cmb As New OleDbCommandBuilder(da)
        da.SelectCommand = cmd
        da.Fill(ds, "kundfahr") 'ds.tables("KundFahr") enthält nun die Daten
        Dim table As System.Data.DataTable
        table = ds.Tables("kundfahr")
        Dim i As Integer
        Dim colNr As Integer = 0
        Dim ofsCol As Integer = 13
        Dim rowNr As Integer = 0
        ReDim Fahrzeuge(table.Columns.Count) 'As Fahrzeug
        Dim txtBox(table.Columns.Count) As TextBox
        Dim lblText(table.Columns.Count) As Label
        For i = 0 To table.Columns.Count - 1
            'Fahrzeuge(i).inhalt = table.Columns(i).ToString
            Fahrzeuge(i).Feldtyp = table.Columns(i).DataType
            'reader(i).GetType()
            Fahrzeuge(i).FeldName = table.Columns(i).ColumnName.ToLower
            Fahrzeuge(i).id = i
            Fahrzeuge(i).AllowNull = table.Columns(i).AllowDBNull
            'reader(i).IsDBNull
            'create a new label and textbox
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
            'special for zulassung1, which is real dateTime field
            If Fahrzeuge(i).Feldtyp.ToString = "System.DateTime" Then
                Try
                    txtBox(i).Text = Format(CDate(Fahrzeuge(i).inhalt), "d")
                Catch
                    txtBox(i).Text = ""
                End Try
            Else
                txtBox(i).Text = Fahrzeuge(i).inhalt
            End If

            txtBox(i).Tag = Fahrzeuge(i).FeldName.ToString.ToLower
            If txtBox(i).Tag = "id" Or txtBox(i).Tag = "kunden_nr" Then
                txtBox(i).Enabled = False

            End If
            'fill the KundenNr for new entries
            If txtBox(i).Tag = "kunden_nr" Then
                txtBox(i).Text = CStr(KundNr)
            End If
            Debug.Print("CreateFields: " & Fahrzeuge(i).FeldName & " - " & Fahrzeuge(i).Feldtyp.ToString)
            'If Fahrzeuge(i).Feldtyp.ToString = "System.DateTime" Then
            '    'add a date picker
            '    'disable direct input
            '    txtBox(i).Enabled = False
            'End If

            If txtBox(i).Tag = "tuev" Or _
                txtBox(i).Tag = "zulassung1" Or _
                txtBox(i).Tag = "asu" Or _
                txtBox(i).Tag = "schreiber" Or _
                txtBox(i).Tag = "sicher" Then

                'limit length, new in 2.0.1.6
                If txtBox(i).Tag = "zulassung1" Then
                    txtBox(i).MaxLength = 10
                Else
                    txtBox(i).MaxLength = 5
                End If

                'txtBox(i).Enabled = False
                txtBox(i).Width = 80
                AddHandler txtBox(i).GotFocus, AddressOf TextBoxGotFocus
                'addFocusHandler(txtBox(i))
            Else
                AddHandler txtBox(i).GotFocus, AddressOf TextBoxGotFocus2
            End If
            rowNr += 1
            If txtBox(i).Top + txtBox(i).Height + 26 > Panel1.Height Then
                colNr += 1
                rowNr = 0
            End If
        Next i

        table.Dispose()
        cn.Close()
        addChangedHandler()
        mIsNew = False
        DataChanged(False)
        DataChangedStatus.Text = "Tabellendefinition geladen"
        btnNeu.Enabled = False
    End Sub
    Private Sub ReadFahrzeug(ByVal FahrzNr As String)
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim dbCmd As New OleDbCommand
        'need table
        Dim cmd As New OleDbCommand("select " & fieldsKundfahr & ", id from kundfahr", cn)
        Dim ds As New DataSet
        '        Dim da As New OleDbDataAdapter
        '        Dim cmb As New OleDbCommandBuilder(da)
        '        da.SelectCommand = cmd
        '        da.Fill(ds, "kundfahr")

        dbCmd.Connection = cn
        Dim queryString As String
        'KUNDENNR=" & KundenNr & _
        queryString = "Select " & fieldsKundfahr & ", id " & _
                        " from Kundfahr where " & _
                        " FGSTLLNR='" & FahrgestellNr & "'"
        Dim i As Integer
        Dim command As New OleDbCommand(queryString, cn)
        'connection.Open() is already opened with OpenDBConnection
        Dim reader As OleDbDataReader = command.ExecuteReader()
        reader.Read() 'try to read first Kunde
        If reader.HasRows Then
            For Each c As Control In Panel1.Controls
                If TypeOf c Is TextBox Then
                    i = findIdforFahrzeug(Fahrzeuge, c.Tag) 'get the id to access the fahrzeug data
                    Fahrzeuge(i).inhalt = reader(c.Tag).ToString 'read column data into right fahrzeug data

                    'special for zulassung1, which is real dateTime field
                    If Fahrzeuge(i).Feldtyp.ToString = "System.DateTime" Then
                        Try
                            c.Text = Format(CDate(Fahrzeuge(i).inhalt), "d")
                        Catch
                            c.Text = ""
                        End Try
                    Else
                        c.Text = Fahrzeuge(i).inhalt
                    End If

                    Debug.Print("ReadFahrzeug: " & Fahrzeuge(i).FeldName & " - " & Fahrzeuge(i).Feldtyp.ToString)

                End If
            Next c 'next control
        End If
        reader.Close()
        cn.Close()
        mIsNew = False
        DataChanged(False)
        DataChangedStatus.Text = "Fahrzeugdaten geladen"

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
    Private Sub TextBoxGotFocus2(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DateTimePicker1.Visible = False
        MonatJahrPicker.Hide()
        LastDateControl = Nothing
        btn_changedate.Visible = False
    End Sub
    Private LastDateControl As Control
    Private Sub TextBoxGotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim y, m As Integer
        Dim s As String
        Dim txtbox As TextBox
        txtbox = CType(sender, TextBox)
        Try
            s = txtbox.Text
            y = CInt(s.Substring(3, 2))
            If y < 50 Then y += 2000 Else y += 1900
            m = CInt(s.Substring(1, 2))
            s = "01." + Format("00", m) + "." + Format("0000", y)
            DateTimePicker1.Value = CDate(s)
            DateTimePicker1.Visible = True
        Catch
            DateTimePicker1.Value = CDate("01.01.1961")
        Finally
            DateTimePicker1.Visible = True
            LastDateControl = txtbox
            btn_changedate.Visible = True
        End Try
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
    End Sub

    Private Sub FahrzeugDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'if loaded with empty FahrgestellNr then we have to create a new empty row
        If FahrgestellNr = "" Then
            MaskeErstellen(KundenNr)
            mIsNew = True
        Else
            MaskeErstellen(KundenNr)
            ReadFahrzeug(FahrgestellNr)
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        If MonatJahrPicker.Visible Then MonatJahrPicker.Dispose()
        If mIsChanged Then
            If MessageBox.Show("Datenänderungen verwerfen?", "Abbrechen", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Me.Close()
            End If
        Else
            Me.Close()
        End If
    End Sub

    Private Sub btnAbbrechen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbbrechen.Click
        If mIsChanged And mIsNew = False Then
            If MessageBox.Show("Datenänderungen verwerfen?", "Schliessen", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                ReadFahrzeug(FahrgestellNr)
            End If
        End If
        If mIsNew Then
            If MessageBox.Show("Eingaben verwerfen?", "Neues Fahrzeug", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                If FahrgestellNr = "" Then MaskeErstellen(KundenNr) Else ReadFahrzeug(FahrgestellNr)
            End If
        End If
    End Sub
    Private Sub MaskeLeeren()
        For Each c As Control In Panel1.Controls
            If TypeOf c Is TextBox And c.Tag <> "kunden_nr" And c.Tag <> "id" Then
                c.Text = ""
            End If
            If c.Tag = "kunden_nr" Then
                c.Text = KundenNr
            End If
        Next
    End Sub
    Private Sub btnSpeichern_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSpeichern.Click
        If Not DatenPruefen() Then Exit Sub
        If mIsNew Then
            KfzNeuSpeichern()
        Else
            FahrzeugSpeichern()
        End If
        mIsChanged = False
        mIsNew = False
        btnNeu.Enabled = True
    End Sub

    Private Sub btnNeu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNeu.Click
        mIsNew = True
        DataChangedStatus.Text = "Neue Fahrzeugdaten"
        btnSpeichern.Enabled = True
        btnAbbrechen.Enabled = True
        btnNeu.Enabled = False
        btnDelete.Enabled = False
        MaskeLeeren()
        DataChanged(False)
    End Sub
    Private Function KfzNeuSpeichern() As Integer
        'we need a connection, a dataset and a dataAdapter
        Dim anzahl As Integer
        Dim cn As New OleDbConnection
        OpenDBConnection(cn)
        Dim cmd As New OleDbCommand("select * from kundfahr", cn)
        Dim ds As New DataSet
        Dim da As New OleDbDataAdapter
        Dim cmb As New OleDbCommandBuilder(da)
        da.SelectCommand = cmd
        da.Fill(ds, "KundFahr")

        'add the primary key to the dataset
        Dim dcs(1) As DataColumn
        dcs(0) = ds.Tables("KundFahr").Columns("id")
        ds.Tables("KundFahr").PrimaryKey = dcs

        'Add the row locally
        Dim drw1 As DataRow = _
            ds.Tables("KundFahr").NewRow()
        drw1("ID") = Int32.MaxValue
        For Each ctrl As Control In Panel1.Controls
            'Fahrzeuge(findIdforFahrzeug(Fahrzeuge, ctrl.Tag)).AllowNull = False And _
            If ctrl.Tag <> "id" And TypeOf ctrl Is TextBox Then 'And ctrl.Text <> ""
                'check type
                Debug.Print(ctrl.Tag + vbTab + ds.Tables("KundFahr").Columns(ctrl.Tag).DataType.ToString)
                If ds.Tables("KundFahr").Columns(ctrl.Tag).DataType.ToString = "System.DateTime" Then
                    If ctrl.Text <> "" Then drw1(ctrl.Tag) = CDate(ctrl.Text)
                    'save the kundennr
                    'If ctrl.Tag = "kunden_nr" Then
                    '    KundenNr = CDouble(ctrl.Text)
                    'End If
                ElseIf ds.Tables("KundFahr").Columns(ctrl.Tag).DataType.ToString = "System.Double" Then
                    drw1(ctrl.Tag) = CDouble(ctrl.Text)
                Else
                    drw1(ctrl.Tag) = ctrl.Text
                End If
            End If
        Next

        'Include an event to fill in the autonumber value
        AddHandler da.RowUpdated, _
            New OleDb.OleDbRowUpdatedEventHandler( _
            AddressOf OnRowUpdated)

        ds.Tables("KundFahr").Rows.Add(drw1)
        'Update the Access database file
        anzahl = da.Update(ds, "KundFahr")

        ds.AcceptChanges()
        cn.Close()
        KfzNeuSpeichern = anzahl
        DataChangedStatus.Text = anzahl & " Fahrzeug gespeichert"
        DataChanged(False)
        mIsNew = False
        btnNeu.Enabled = True
        btnDelete.Enabled = True
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
                'kunden_nr is fixed
                'fgstllnr has to be unique for new data
                If mIsNew Then
                    If ctrl.Tag = "fgstllnr" Then
                        If ExistFgstllNr(ctrl.Text) Then
                            ctrl.Focus()
                            ctrl.BackColor = Color.Red
                            DatenPruefen = False
                            DataChangedStatus.Text = "FgstllNr vorhanden"
                            Exit For
                        End If
                    End If
                End If
                'check for empty required fields
                If ctrl.Tag <> "id" Then
                    If Fahrzeuge(findIdforFahrzeug(Fahrzeuge, ctrl.Tag)).Feldtyp.ToString = "System.DateTime" Then
                        Try
                            ctrl.Text = Format(CDate(ctrl.Text), "d")
                        Catch
                            ctrl.Text = ""
                        End Try
                    End If
                    If Fahrzeuge(findIdforFahrzeug(Fahrzeuge, ctrl.Tag)).AllowNull = False And ctrl.Text = "" Then
                        ctrl.Focus()
                        ctrl.BackColor = Color.Red
                        DatenPruefen = False
                        'Exit For
                    Else
                        ctrl.BackColor = Color.Empty
                    End If
                End If
            End If
        Next
    End Function
    Private Function findIdforFahrzeug(ByRef f() As Fahrzeug, ByVal s As String) As Integer
        Dim x
        For x = 0 To f.Length - 1
            If f(x).FeldName = s Then
                findIdforFahrzeug = x
                Exit Function
            End If
        Next
        findIdforFahrzeug = 0
    End Function
    Private Function ExistFgstllNr(ByVal fgstllnr As String) As Boolean
        ExistFgstllNr = False
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim dbCmd As New OleDbCommand
        'Dim dbda As OleDbDataAdapter
        'dbda = New OleDbDataAdapter
        dbCmd.Connection = cn
        Dim queryString As String
        'KUNDENNR=" & KundenNr & _
        queryString = "Select " & fieldsKundfahr & ", id " & _
                        " from Kundfahr where " & _
                        " fgstllnr='" & fgstllnr & "'"
        Dim colNr As Integer = 0
        Dim ofsCol As Integer = 13
        Dim rowNr As Integer = 0
        Dim command As New OleDbCommand(queryString, cn)
        'connection.Open() is already opened with OpenDBConnection
        Dim reader As OleDbDataReader = command.ExecuteReader()
        reader.Read() 'try to read first Kunde
        If reader.HasRows Then
            ExistFgstllNr = True
        End If
        reader.Close()
        cn.Close()
    End Function
    Private Function FahrzeugSpeichern()
        'we need a connection, a dataset and a dataAdapter
        Dim anzahl As Integer
        Dim cn As New OleDbConnection
        OpenDBConnection(cn)
        Dim cmd As New OleDbCommand("select * from kundfahr", cn)
        Dim ds As New DataSet
        Dim da As New OleDbDataAdapter
        Dim cmb As New OleDbCommandBuilder(da)
        da.SelectCommand = cmd
        da.Fill(ds, "kundfahr")

        'add the primary key to the dataset
        Dim dcs(1) As DataColumn
        dcs(0) = ds.Tables("kundfahr").Columns("id")
        ds.Tables("kundfahr").PrimaryKey = dcs

        'find the row locally
        Dim FahrzeugID As Long = 0
        For Each ctrl As Control In Panel1.Controls
            If ctrl.Tag = "id" Then
                FahrzeugID = CLong(ctrl.Text)
            End If
        Next
        If FahrzeugID = 0 Then
            FahrzeugSpeichern = 0
            Exit Function
        End If

        Dim drw1 As DataRow = ds.Tables("kundfahr").Rows.Find(FahrzeugID)
        'ds.Tables("kundfahr").Columns(0).DataType
        'Dim drw1 As DataRow = ds.Tables("KundenSt").NewRow()
        'drw1("ID") = Int32.MaxValue
        For Each ctrl As Control In Panel1.Controls
            If TypeOf ctrl Is TextBox And ctrl.Tag <> "id" Then 'And ctrl.Text <> ""
                'empty fields are checked against Allow.DBNull during DatenPruefen
                If ds.Tables("KundFahr").Columns(ctrl.Tag).DataType.ToString = "System.DateTime" Then
                    If ctrl.Text <> "" Then drw1(ctrl.Tag) = CDate(ctrl.Text) Else drw1(ctrl.Tag) = CDate("01.01.1961")
                    'save the kundennr
                    'If ctrl.Tag = "kunden_nr" Then
                    '    KundenNr = CDouble(ctrl.Text)
                    'End If
                ElseIf ds.Tables("KundFahr").Columns(ctrl.Tag).DataType.ToString = "System.Double" Then
                    If ctrl.Text <> "" Then drw1(ctrl.Tag) = CDouble(ctrl.Text) Else drw1(ctrl.Tag) = Nothing
                Else
                    drw1(ctrl.Tag) = ctrl.Text
                End If
            End If
        Next

        'Update the Access database file
        anzahl = da.Update(ds, "kundfahr")
        ds.AcceptChanges()
        cn.Close()
        FahrzeugSpeichern = anzahl
        DataChangedStatus.Text = anzahl & " Fahrzeug gespeichert"
        DataChangedStatus.BackColor = Color.Empty
        mIsNew = False
        btnAbbrechen.Enabled = False
        btnSpeichern.Enabled = False
        btnDelete.Enabled = True
    End Function

    Private Sub btn_changedate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_changedate.Click
        If Not IsNothing(LastDateControl) Then
            If LastDateControl.Tag = "zulassung1" Then
                'transfer a real date
                LastDateControl.Text = Format(DateTimePicker1.Value, "d")
            Else
                LastDateControl.Text = DateTimePicker1.Text
            End If
            DataChanged(True)
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MessageBox.Show("Soll das Fahrzeug mit der Nummer " + FahrgestellNr + " wirklich gelöscht werden?", "Fahrzeug löschen", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            FahrzeugLoeschen(FahrgestellNr)
        End If
    End Sub
    Sub FahrzeugLoeschen(ByVal FNr As String)
        Dim cn As New OleDbConnection
        Dim cmd As New OleDbCommand()
        Dim anz As Long
        OpenDBConnection(cn)
        cmd.Connection = cn
        cmd.CommandText = "Delete from kundfahr where FGSTLLNR='" + FNr + "'"
        anz = cmd.ExecuteNonQuery
        DataChangedStatus.Text = anz & " Fahrzeug(e) gelöscht"
        cn.Close()
        btnDelete.Enabled = False
    End Sub
End Class