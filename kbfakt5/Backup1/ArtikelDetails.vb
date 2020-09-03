Imports System.Data.OleDb
Imports System.Reflection
Public Class ArtikelDetails
    'kbfakt_start.FillGrid(GridAuftraege, "Select ArtikelNR, Anreden.ANREDE , VORNAME, " + _
    '                                    "NACHNAME, BRANCHE, PLZ, " + _
    '                                    "ORT , STRASSE , ANSPRECHP, TELEFON," + _
    '                                    "KONTO1 from ArtikelST, Anreden " + _
    '                                    "where ArtikelST.Anrede=Anreden.id " + _
    '                                    "order by ArtikelNR")
    Private mIsChanged As Boolean = False
    Private mIsNeuArtikel As Boolean = False
    Private firstrun As Boolean = True
    Private ArtikelTypenListBox As New ComboBox
    Public lst As New ComboBox
    Private ArtTypenTxtBoxID As Integer = 0
    Private FzgTypenTxtBoxID As Integer = 0

    Private Structure sArtikel
        Dim inhalt As String
        Dim FeldName As String
        Dim displayName As String
        Dim tblName As String
        Dim Feldtyp As Type
        Dim id As Integer
        Dim AllowNull As Boolean
    End Structure
    Private Artikel() As sArtikel
    Public ArtikelNr As String
    Public ArtikelID As Long = 0

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If mIsChanged Then
            If MessageBox.Show("Datenänderungen verwerfen?", "Artikel", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then Me.Close()
        Else
            Me.Close()
        End If
    End Sub
    Private Sub MaskeErstellen(ByVal artnr As String)
        Debug.Print("################################" + vbCr & Panel1.Controls.Count & vbCr)
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim dbCmd As New OleDbCommand
        'Dim dbda As OleDbDataAdapter
        'dbda = New OleDbDataAdapter
        dbCmd.Connection = cn
        Dim queryString As String
        '"ARTIKELNR, ARTIKELBEZ, BESTAND, VK, LAGERORT, LETZ_EK, MISCH_EK, RABATTCODE, MIND_MENGE, LETZ_BESTL, LETZ_VK, LETZEKMENG, LIEFER1, EK1, LIEFER2, EK2, LIEFER3, EK3"
        'plus ArtTyp und id

        queryString = "Select ARTIKELNR, ARTIKELBEZ, VK, EK1, BESTAND, ArtTyp, FzgTyp, LAGERORT, LETZ_EK, " & _
                            "MISCH_EK, RABATTCODE, MIND_MENGE, LETZ_BESTL, LETZ_VK, " & _
                            "LETZEKMENG, LIEFER1, LIEFER2, EK2, LIEFER3, EK3, id " + _
                                    "from ARTSTAMM where ARTIKELNR='" & artnr & "'"
        '"order by ArtikelNR"
        Dim i As Integer
        Dim colNr As Integer = 0
        Dim ofsCol As Integer = 13
        Dim rowNr As Integer = 0
        Dim cmd As New OleDbCommand(queryString, cn)
        Dim ds As New DataSet
        Dim da As New OleDbDataAdapter
        Dim cmb As New OleDbCommandBuilder(da)
        da.SelectCommand = cmd
        da.Fill(ds, "ARTSTAMM") 'ds.tables("KundFahr") enthält nun die Daten
        Dim table As System.Data.DataTable
        table = ds.Tables("ARTSTAMM")
        ReDim Artikel(table.Columns.Count)
        Dim txtBox(table.Columns.Count) As TextBox
        Dim lblText(table.Columns.Count) As Label
        'connection.Open() is already opened with OpenDBConnection
        Debug.Print("### ArtikelDetails - MaskeErstellen")
        For i = 0 To table.Columns.Count - 1
            Artikel(i).Feldtyp = table.Columns(i).DataType
            Artikel(i).FeldName = table.Columns(i).ColumnName.ToLower
            Artikel(i).id = i
            Artikel(i).AllowNull = table.Columns(i).AllowDBNull
            Debug.Print("Feldname(" & i & "): " & Artikel(i).FeldName)
            txtBox(i) = New TextBox
            lblText(i) = New Label
            If Artikel(i).Feldtyp.ToString = "System.Double" Or Artikel(i).Feldtyp.ToString = "System.Int32" Or _
                Artikel(i).Feldtyp.ToString = "System.DateTime" Or Artikel(i).Feldtyp.ToString = "System.Int16" _
                Then txtBox(i).TextAlign = HorizontalAlignment.Right

            Debug.Print("Feld " & Artikel(i).FeldName & " = " & Artikel(i).Feldtyp.ToString)

            lblText(i).Width = 80
            lblText(i).Left = colNr * Panel1.Width / 2
            Panel1.Controls.Add(lblText(i))

            lblText(i).Top = rowNr * 26
            lblText(i).Text = table.Columns(i).ColumnName + ":"
            Panel1.Controls.Add(txtBox(i))

            txtBox(i).Top = rowNr * 26
            txtBox(i).Width = 160
            txtBox(i).Left = 80 + (colNr * Panel1.Width / 2)

            If Artikel(i).FeldName.ToLower = "arttyp" Then
                txtBox(i).Width = 80
                txtBox(i).Enabled = False
                FillArtTypenListbox(ArtikelTypenListBox)
                ArtikelTypenListBox.Left = txtBox(i).Left + txtBox(i).Width + 13 + (colNr * Panel1.Width / 2)
                ArtikelTypenListBox.Top = txtBox(i).Top
                ArtikelTypenListBox.Width = 60
                ArtikelTypenListBox.DropDownStyle = ComboBoxStyle.DropDownList
                ArtTypenTxtBoxID = i
                AddHandler ArtikelTypenListBox.SelectedIndexChanged, AddressOf ArtTypenListBoxChanged
                Panel1.Controls.Add(ArtikelTypenListBox)
            End If
            If Artikel(i).FeldName = "fzgtyp" Then
                txtBox(i).Width = 60
                txtBox(i).Enabled = False
                Panel1.Controls.Add(lst)
                lst.Left = txtBox(i).Left + txtBox(i).Width + 13 + (colNr * Panel1.Width / 2)
                lst.Top = txtBox(i).Top
                lst.Width = 80
                lst.DropDownStyle = ComboBoxStyle.DropDownList
                FzgTypenTxtBoxID = i
                FillFzgTypenListe(lst)
                AddHandler lst.SelectedIndexChanged, AddressOf FzgTypenListBoxChanged
            End If

            txtBox(i).Tag = Artikel(i).FeldName.ToString.ToLower
            If txtBox(i).Tag = "id" Or txtBox(i).Tag = "artikelnr" Then
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
        mIsNeuArtikel = False
        DataChanged(False)
        DataChangedStatus.Text = "Artikel geladen"
    End Sub
    Private Sub ReadArtikel(ByVal ArtNr As String)
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
        queryString = "Select ARTIKELNR, ARTIKELBEZ, VK, EK1, BESTAND, ArtTyp, FzgTyp, LAGERORT, LETZ_EK, " & _
                            "MISCH_EK, RABATTCODE, MIND_MENGE, LETZ_BESTL, LETZ_VK, " & _
                            "LETZEKMENG, LIEFER1, LIEFER2, EK2, LIEFER3, EK3, id " + _
                                    "from ARTSTAMM where ARTIKELNR='" & ArtNr & "'"
        '"order by ArtikelNR"
        Dim i As Integer
        Dim colNr As Integer = 0
        Dim ofsCol As Integer = 13
        Dim rowNr As Integer = 0
        Dim FzgTyp As String = ""
        Dim command As New OleDbCommand(queryString, cn)
        'connection.Open() is already opened with OpenDBConnection
        Dim reader As OleDbDataReader = command.ExecuteReader()
        reader.Read() 'try to read first Artikel
        If reader.HasRows Then
            For Each c As Control In Panel1.Controls
                If TypeOf c Is TextBox Then
                    i = findIdforArtikel(Artikel, c.Tag) 'get the id to access the fahrzeug data
                    Artikel(i).inhalt = reader(c.Tag).ToString 'read column data into right fahrzeug data

                    'special for zulassung1, which is real dateTime field
                    If Artikel(i).Feldtyp.ToString = "System.DateTime" Then
                        Try
                            c.Text = Format(CDate(Artikel(i).inhalt), "d")
                        Catch
                            c.Text = ""
                        End Try
                    Else
                        c.Text = Artikel(i).inhalt
                    End If
                    If c.Tag = "id" Then
                        ArtikelID = CLong(c.Text)
                        btnDelete.Enabled = True
                    End If
                    If c.Tag = "arttyp" Then
                        ArtikelTypenListBox.SelectedIndex = CInt(c.Text)
                    End If
                    If c.Tag = "fzgtyp" Then
                        Dim x As Integer
                        FzgTyp = c.Text
                        x = lst.FindString(FzgTyp.TrimEnd) 'FzgTyp
                        If x > -1 Then lst.SelectedIndex = x
                    End If

                    'If Artikel(i).Feldtyp.ToString = "System.Double" Then c.Text = String.Format("0.00", CDoubleS(c.Text))

                    Debug.Print("ReadArtikel: " & Artikel(i).FeldName & " - " & Artikel(i).Feldtyp.ToString)

                End If
            Next c 'next control
        End If
        reader.Close()
        mIsNeuArtikel = False
        DataChanged(False)
        DataChangedStatus.Text = "Artikel geladen"
    End Sub
    Private Function findIdforArtikel(ByRef f() As sArtikel, ByVal s As String) As Integer
        Dim x
        For x = 0 To f.Length - 1
            If f(x).FeldName = s Then
                findIdforArtikel = x
                Exit Function
            End If
        Next
        findIdforArtikel = 0
    End Function

    Private Sub ArtikelDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnDelete.Enabled = False
        If ArtikelNr = "" Then
            ArtikelNr = "Neuer Artikel" 'GetMaxArtikelNr()
            MaskeErstellen(ArtikelNr)
            NeuArtikel()
        Else
            MaskeErstellen(ArtikelNr)
            ReadArtikel(ArtikelNr)
        End If
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
        If mIsChanged And mIsNeuArtikel = False Then
            If MessageBox.Show("Datenänderungen verwerfen?", "Schliessen", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                ReadArtikel(ArtikelNr)
            End If
        End If
        If mIsNeuArtikel Then
            If MessageBox.Show("Eingaben verwerfen?", "Neuer Artikel", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                ReadArtikel(ArtikelNr)
            End If
        End If
    End Sub
    Private Function ArtikelNeuSpeichern() As Integer
        'test if ArtikelNr is unique
        For Each ctrl As Control In Panel1.Controls
            If TypeOf ctrl Is TextBox Then
                'save the new Artikelnr
                If ctrl.Tag = "artikelnr" Then ArtikelNr = ctrl.Text
            End If
        Next
        If ExistsData("select ArtikelNr from ArtStamm where ArtikelNr='" & ArtikelNr & "'") Then
            MessageBox.Show("Artikel >" & ArtikelNr & "< existiert bereits und kann nicht neu angelegt werden!")
            Return 0
        End If

        'we need a connection, a dataset and a dataAdapter
        Dim anzahl As Integer
        Dim cn As New OleDbConnection
        OpenDBConnection(cn)
        Dim cmd As New OleDbCommand("select * from artstamm", cn)
        Dim ds As New DataSet
        Dim da As New OleDbDataAdapter
        Dim cmb As New OleDbCommandBuilder(da)
        da.SelectCommand = cmd
        da.Fill(ds, "ArtStamm")

        'add the primary key to the dataset
        Dim dcs(1) As DataColumn
        dcs(0) = ds.Tables("ArtStamm").Columns("id")
        ds.Tables("ArtStamm").PrimaryKey = dcs

        'Add the row locally
        Dim drw1 As DataRow = _
            ds.Tables("ArtStamm").NewRow()
        drw1("ID") = Int32.MaxValue

        'Include an event to fill in the autonumber value
        AddHandler daRech1.RowUpdated, _
            New OleDb.OleDbRowUpdatedEventHandler( _
            AddressOf OnRowUpdated)

        For Each ctrl As Control In Panel1.Controls
            If TypeOf ctrl Is TextBox Then
                If ctrl.Tag <> "id" Then
                    Try
                        drw1.Item(ctrl.Tag) = ctrl.Text
                    Catch ax As ArgumentException
                        Try
                            drw1.Item(ctrl.Tag) = CDouble(ctrl.Text)
                        Catch
                            drw1.Item(ctrl.Tag) = CDatum(ctrl.Text)
                        End Try
                    End Try
                End If
            End If
        Next
        ds.Tables("artstamm").Rows.Add(drw1)
        'Update the Access database file
        anzahl = da.Update(ds, "artStamm")
        ds.AcceptChanges()

        cn.Close()
        ArtikelNeuSpeichern = anzahl
        DataChangedStatus.Text = anzahl & " Artikel gespeichert"
        mIsNeuArtikel = False
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

        Dim coll As New Microsoft.VisualBasic.Collection()
        'queryString = "Select ARTIKELNR, ARTIKELBEZ, BESTAND, VK, LAGERORT, LETZ_EK, " & _
        '            "MISCH_EK, RABATTCODE, MIND_MENGE, LETZ_BESTL, LETZ_VK, " & _
        '            "LETZEKMENG, LIEFER1, EK1, LIEFER2, EK2, LIEFER3, EK3 " + _
        '                    "from ARTSTAMM where ARTIKELNR='" & ArtNr & "'"
        coll.Clear()
        coll.Add("artikelnr", "artikelnr")
        coll.Add("artikelbez", "artikelbez")
        coll.Add("vk", "vk")
        coll.Add("ek1", "ek1")

        Dim AnzahlFehler As Integer = 0
        DatenPruefen = True
        For Each ctrl As Control In Panel1.Controls
            If TypeOf ctrl Is TextBox Then
                If coll.Contains(ctrl.Tag) Then
                    If ctrl.Tag = "artikelnr" Or ctrl.Tag = "artikelbez" Then
                        If Not CheckTextLength(ctrl) Then AnzahlFehler = AnzahlFehler + 1
                    End If
                    If ctrl.Tag = "ek1" Or ctrl.Tag = "vk" Then
                        If Not CheckTextDouble(ctrl) Then AnzahlFehler = AnzahlFehler + 1
                    End If
                Else

                End If
            End If
        Next
        If AnzahlFehler > 0 Then Return False
        Return True
    End Function
    Private Sub NeuArtikel()
        For Each ctrl As Control In Panel1.Controls
            If TypeOf ctrl Is TextBox Then
                ctrl.Text = ""
                If ctrl.Tag = "artikelnr" Then
                    ctrl.Enabled = True
                    ctrl.Text = "Neuer Artikel" 'GetMaxArtikelNr() + 1
                End If
            End If
            btnSpeichern.Enabled = True
            btnAbbrechen.Enabled = True
            mIsChanged = True
            DataChangedStatus.Text = "Neuer Artikel"
        Next
        mIsNeuArtikel = True
        btnNeu.Enabled = False
        btnSuchen.Enabled = False
        btnDelete.Enabled = False
    End Sub
    Private Sub btnNeu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNeu.Click
        NeuArtikel()
    End Sub
    Private Function ArtikelSpeichern()
        'we need a connection, a dataset and a dataAdapter
        Dim anzahl As Integer
        Dim cn As New OleDbConnection
        OpenDBConnection(cn)
        Dim cmd As New OleDbCommand("select * from ArtStamm", cn)
        Dim ds As New DataSet
        Dim da As New OleDbDataAdapter
        Dim cmb As New OleDbCommandBuilder(da)
        da.SelectCommand = cmd
        da.Fill(ds, "ArtStamm")

        'add the primary key to the dataset
        Dim dcs(1) As DataColumn
        dcs(0) = ds.Tables("ArtStamm").Columns("id")
        ds.Tables("ArtStamm").PrimaryKey = dcs

        'find the row locally
        Dim ArtikelID As Long = 0
        For Each ctrl As Control In Panel1.Controls
            If ctrl.Tag = "id" Then
                ArtikelID = CLong(ctrl.Text)
            End If
        Next
        If ArtikelID = 0 Then
            ArtikelSpeichern = 0
            Exit Function
        End If

        Dim drw1 As DataRow = ds.Tables("ArtStamm").Rows.Find(ArtikelID)
        'Dim drw1 As DataRow = ds.Tables("ArtikelSt").NewRow()
        'drw1("ID") = Int32.MaxValue
        For Each ctrl As Control In Panel1.Controls
            If TypeOf ctrl Is TextBox Then
                If ctrl.Tag <> "id" Then
                    Try
                        'Dim myTypeString As Type = Type.GetType("System.String")
                        'fType = drw1.GetType()
                        'If Typeof(fType) is Type.GetType("System.String") Then
                        'End If
                        drw1.Item(ctrl.Tag) = ctrl.Text
                    Catch ax As ArgumentException
                        Try
                            drw1.Item(ctrl.Tag) = CDouble(ctrl.Text)
                        Catch
                            drw1.Item(ctrl.Tag) = CDatum(ctrl.Text)
                        End Try
                    End Try
                End If
            End If
        Next

        'Update the Access database file
        anzahl = da.Update(ds, "ArtStamm")
        ds.AcceptChanges()
        cn.Close()
        ArtikelSpeichern = anzahl
        DataChangedStatus.Text = anzahl & " Artikel gespeichert"
        mIsNeuArtikel = False
    End Function
    Private Sub btnSpeichern_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSpeichern.Click
        If DatenPruefen() Then
            If mIsNeuArtikel Then
                ArtikelNeuSpeichern()
            Else
                ArtikelSpeichern()
            End If
            DataChanged(False)
        Else
            DataChangedStatus.Text = "Eingaben unvollständig"
            DataChangedStatus.BackColor = Color.Red
        End If
    End Sub
    Function GetMaxArtikelNr() As String
        'finde max Artikel
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim dbCmd As New OleDbCommand
        'Dim dbda As OleDbDataAdapter
        'dbda = New OleDbDataAdapter
        dbCmd.Connection = cn
        dbCmd.CommandText = "select MAX(ArtikelNR) from artstamm"
        Dim MaxArtikelNr As String
        MaxArtikelNr = CStr(dbCmd.ExecuteScalar())
        cn.Close()
        Return MaxArtikelNr
    End Function

    Private Sub btnSuchen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuchen.Click
        'Artikelliste.ShowDialog()
        'If Artikelliste.Artikel_nr <> 0 Then
        '    ArtikelNr = Artikelliste.Artikel_nr
        '    ReadArtikel(ArtikelNr)
        'End If
        'Artikelliste.Dispose()
    End Sub
    Sub FillArtTypenListbox(ByVal ctl As ComboBox)
        Dim queryString As String
        Dim idx As Integer
        queryString = "Select ID, ArtikelTyp from ARTTYPEN ORDER by ID"
        Dim cn As New OleDbConnection
        OpenDBConnection(cn)
        ctl.Items.Clear()
        Using cn
            Dim command As New OleDbCommand(queryString, cn)
            Dim reader As OleDbDataReader = command.ExecuteReader()
            While reader.Read()
                idx = ctl.Items.Add(reader("ArtikelTyp").ToString)
            End While
            reader.Close()
        End Using
        ctl.SelectedIndex = 0
        cn.Close()
    End Sub
    Private Sub ArtTypenListBoxChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ctrl As Control
        For Each ctrl In Me.Panel1.Controls
            If TypeOf ctrl Is TextBox Then
                If ctrl.Tag = "arttyp" Then
                    ctrl.Text = sender.SelectedIndex  'SelectedItem.ToString
                    DataChanged(True)
                End If
            End If
        Next
    End Sub
    Private Sub FzgTypenListBoxChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ctrl As Control
        Dim cb As ComboBox
        cb = CType(sender, ComboBox)
        For Each ctrl In Me.Panel1.Controls
            If TypeOf ctrl Is TextBox Then
                If ctrl.Tag = "fzgtyp" Then
                    ctrl.Text = cb.Items(cb.SelectedIndex)
                    DataChanged(True)
                End If
            End If
        Next
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If mIsNeuArtikel Then Return
        Dim ant As DialogResult
        ant = MessageBox.Show("Artikel >" & ArtikelNr & "< wirklich löschen?", "Artikel löschen", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If ant = Windows.Forms.DialogResult.Yes Then
            Dim anzahl As Integer
            Dim cn As New OleDbConnection
            OpenDBConnection(cn)
            Dim cmd As New OleDbCommand("delete from ArtStamm where id=" & ArtikelID, cn)
            Try
                anzahl = cmd.ExecuteNonQuery()
            Catch ox As OleDbException
            End Try
            cn.Close()
            If anzahl > 0 Then
                MessageBox.Show("Artikel wurde gelöscht")
                ReadArtikel(GetMaxArtikelNr())
            Else
                MessageBox.Show("Fehler beim Löschen", "ArtikelDetails - Artikel Löschen")
            End If
        End If
    End Sub
End Class