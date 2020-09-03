Imports System.Windows.Forms

Public Class Richtwerte
    Public AwNr As String = ""
    Public FzgTyp As String = ""
    Public Preis As Double = 0
    Public AwText As String = ""
    Public Menge As Double = 1

    Public posX As Integer = 0
    Public posY As Integer = 0
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click, Button1.Click
        If CheckTextDouble(txtPreis) And CheckTextDouble(txtMenge) And _
        CheckTextLength(txtText) And CheckTextLength(txtTypKlasse) Then
            AwNr = txtRichtwertNr.Text
            FzgTyp = txtTypKlasse.Text
            AwText = txtText.Text
            Menge = CDbl(txtMenge.Text)
            Preis = CDbl(txtPreis.Text)
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click, Button2.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        AwNr = ""
        Me.Close()
    End Sub

    Private Sub Richtwerte_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        FormSettingsSave(Me)
    End Sub

    Private Sub Richtwerte_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Me.Top = posX
        'Me.Left = posY
        FormSettingsLoad(Me)
        'SELECT r2.ArtNr, r1.AwText, r2.FzgTyp, r2.Preis FROM Richtwerte2 as r2, richtwerte1 as r1 where r2.fzgtyp='MB 815';

        txtRichtwertNr.Enabled = False

        txtRichtwertNr.Text = AwNr
        txtTypKlasse.Text = FzgTyp
        txtPreis.Text = String.Format("{0:0.00}", Preis)
        txtMenge.Text = String.Format("{0:0.00}", Menge)
        txtText.Text = AwText

        dgv1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv1.MultiSelect = False

        dgv2.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv2.MultiSelect = False

        FillTabelle1(AwNr)
        FillTabelle2(FzgTyp)
        fillList()
    End Sub
    Private Sub ResizeGrid1()
        Dim percent As Integer = dgv1.Width \ 100
        Dim i As Integer
        Dim sum As Integer = 0
        With dgv1
            .ReadOnly = True
            .RowHeadersWidth = 5 * percent
            .Columns("ArtNr").Width = 10 * percent
            .Columns("AWText").Width = 55 * percent
            .Columns("id").Width = 5 * percent
            Debug.Print("Tabellenbreite: " & dgv1.Width)
            For i = 0 To .Columns.Count - 1
                Debug.Print("Spalte: " & i & vbTab & "Name: " & .Columns(i).Name & vbTab & "Breite: " & .Columns(i).Width)
                sum += .Columns(i).Width
            Next
            Dim rest As Integer
            rest = .Width - sum - .RowHeadersWidth
            Debug.Print("Summe: " & sum & vbTab & "Rest: " & rest)
            Dim offs As Integer = 18
            If rest > offs Then .Columns("AWText").Width = .Columns("AWText").Width + rest - offs
        End With

    End Sub
    Private Sub ResizeGrid2()
        Dim percent As Integer = dgv2.Width \ 100
        Dim i As Integer
        Dim sum As Integer = 0
        With dgv2
            .ReadOnly = True
            .RowHeadersWidth = 5 * percent
            .Columns("ArtNr").Width = 10 * percent
            .Columns("FzgTyp").Width = 40 * percent
            .Columns("id").Width = 5 * percent
            Debug.Print("Tabellenbreite: " & dgv2.Width)
            For i = 0 To .Columns.Count - 1
                Debug.Print("Spalte: " & i & vbTab & "Name: " & .Columns(i).Name & vbTab & "Breite: " & .Columns(i).Width)
                sum += .Columns(i).Width
            Next
            Dim rest As Integer
            rest = .Width - sum - .RowHeadersWidth
            Debug.Print("Summe: " & sum & vbTab & "Rest: " & rest)
            Dim offs As Integer = 18
            'If rest > offs Then .Columns("AWText").Width = .Columns("AWText").Width + rest - offs
        End With

    End Sub
    Private Sub fillList()
        Dim cn As New OleDb.OleDbConnection
        Dim rdr As OleDb.OleDbDataReader
        Dim cmd As New OleDb.OleDbCommand
        OpenDBConnection(cn)
        cmd.Connection = cn
        cmd.CommandText = "select distinct FzgTyp from richtwerte2 order by FzgTyp"
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            ComboBox1.Items.Clear()
            While rdr.Read
                ComboBox1.Items.Add(rdr.GetValue(0).ToString)
            End While
            ComboBox1.SelectedIndex = 0
            ComboBox1.Items.Insert(0, "===ALLE===")
        Else

        End If
        rdr.Close()
        cn.Close()
    End Sub
    Private Sub FillTabelle1(ByVal art As String)
        dgv1.DataSource = Nothing
        If art = "" Or Not ExistsData("select ArtNr from richtwerte1 where ArtNr='" & art & "'") Then
            'kbfakt_start.FillGrid(dgv1, "SELECT Richtwerte1.ArtNr, Richtwerte1.AWText, Richtwerte2.FzgTyp, Richtwerte2.Preis , richtwerte1.id as id1, richtwerte2.id as id2  FROM Richtwerte1 LEFT JOIN Richtwerte2 ON Richtwerte1.ArtNr = Richtwerte2.ArtNr;")
            kbfakt_start.FillGrid(dgv1, "SELECT id, ArtNr, AWText from Richtwerte1;")
        Else
            kbfakt_start.FillGrid(dgv1, "SELECT id, ArtNr, AWText from Richtwerte1 where artnr='" & art & "';")
        End If
        ResizeGrid1()

    End Sub
    Private Sub FillTabelle2(ByVal art As String)
        dgv2.DataSource = Nothing
        If art = "" Or Not ExistsData("select ArtNr from richtwerte2 where ArtNr='" & art & "'") Then
            'kbfakt_start.FillGrid(dgv1, "SELECT Richtwerte1.ArtNr, Richtwerte1.AWText, Richtwerte2.FzgTyp, Richtwerte2.Preis , richtwerte1.id as id1, richtwerte2.id as id2  FROM Richtwerte1 LEFT JOIN Richtwerte2 ON Richtwerte1.ArtNr = Richtwerte2.ArtNr;")
            kbfakt_start.FillGrid(dgv2, "SELECT id, ArtNr, FzgTyp, Preis from Richtwerte2;")
        Else
            kbfakt_start.FillGrid(dgv2, "SELECT id, ArtNr, FzgTyp, Preis from Richtwerte2 where artnr='" & art & "';")
        End If
        ResizeGrid2()

    End Sub
    Private Sub FillTabelle2typ(ByVal typ As String)
        dgv2.DataSource = Nothing
        If typ = "" Or Not ExistsData("select ArtNr from richtwerte2 where fzgtyp='" & typ & "'") Then
            'kbfakt_start.FillGrid(dgv1, "SELECT Richtwerte1.ArtNr, Richtwerte1.AWText, Richtwerte2.FzgTyp, Richtwerte2.Preis , richtwerte1.id as id1, richtwerte2.id as id2  FROM Richtwerte1 LEFT JOIN Richtwerte2 ON Richtwerte1.ArtNr = Richtwerte2.ArtNr;")
            kbfakt_start.FillGrid(dgv2, "SELECT id, ArtNr, FzgTyp, Preis from Richtwerte2;")
        Else
            kbfakt_start.FillGrid(dgv2, "SELECT id, ArtNr, FzgTyp, Preis from Richtwerte2 where FzgTyp='" & typ & "';")
        End If
        ResizeGrid2()
        dgv2.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv2.MultiSelect = False
    End Sub
    Private Sub RefreshDGV1()
        If Not IsNothing(dgv1.CurrentCell) Then
            txtRichtwertNr.Text = CStr(dgv1.Item("ArtNr", dgv1.CurrentCell.RowIndex).Value)
            If ExistsData("Select * from Richtwerte2 where ArtNr='" & txtRichtwertNr.Text & "'") Then
                FillTabelle2(txtRichtwertNr.Text)
            Else
                FillTabelle2(txtRichtwertNr.Text)
                'FillTabelle2("")
            End If
            lblID1.Text = CStr(dgv1.Item("id", dgv1.CurrentCell.RowIndex).Value)
            'txtTypKlasse.Text = CStr(dgv1.Item("FzgTyp", dgv1.CurrentCell.RowIndex).Value)
            txtText.Text = CStr(dgv1.Item("AwText", dgv1.CurrentCell.RowIndex).Value)
            'txtPreis.Text = CStr(CDouble(dgv1.Item("Preis", dgv1.CurrentCell.RowIndex).Value))
            'Preis = CDbl(txtPreis.Text)
            'txtPreis.Text = String.Format("{0:0.00}", Preis)
        Else
        End If
    End Sub

    Private Sub Richtwerte_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        ResizeGrid1()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex = 0 Then
            FillTabelle2("")
            FillTabelle1("")
        Else
            FillTabelle2typ(ComboBox1.SelectedItem.ToString)
        End If

    End Sub

    Private Sub btnDeleteRichtwert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteRichtwert.Click
        'delete Richtwert1 value and all Richtwert2 entries
        Dim lArtNr As String = ""
        Dim lAwText As String = ""
        Dim id1 As Long
        Dim r = dgv1.CurrentCell.RowIndex
        Dim c = dgv1.CurrentCell.ColumnIndex
        If r >= 0 Then
            With dgv1
                If Not IsDBNull(.Item("ArtNr", r).Value) Then lArtNr = CString(.Item("ArtNr", r).Value)
                If Not IsDBNull(.Item("AwText", r).Value) Then lAwText = CString(.Item("AwText", r).Value)
                id1 = .Item("id", r).Value
            End With
            If MessageBox.Show("Alle Daten zu '" & lArtNr & ":" & lAwText & ":" & _
                vbCrLf & "wirklich löschen?", "Richtwert entfernen", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                DeleteRichtwertID1(id1)
                DeleteRichtwert2(lArtNr)
                FillTabelle2("")
                FillTabelle1("")
            End If
        End If
    End Sub
    Private Function DeleteRichtwertID1(ByVal id1 As Long) As Integer
        Dim cn As New OleDb.OleDbConnection
        Dim cmd As New OleDb.OleDbCommand
        Dim anz As Integer
        OpenDBConnection(cn)
        cmd.Connection = cn
        'delete fzg, ArtNr
        cmd.CommandText = "delete from richtwerte1 where id=" & id1
        anz = cmd.ExecuteNonQuery
        Debug.Print("DeleteRichtwertID1 " & cmd.CommandText & " returned " & anz)
        cn.Close()
        Return anz
    End Function
    Private Function DeleteRichtwert2(ByVal artnr As String) As Integer
        Dim cn As New OleDb.OleDbConnection
        Dim cmd As New OleDb.OleDbCommand
        Dim anz As Integer
        OpenDBConnection(cn)
        cmd.Connection = cn
        'delete fzg, ArtNr
        cmd.CommandText = "delete from richtwerte2 where ArtNr='" & artnr & "'"
        anz = cmd.ExecuteNonQuery
        Debug.Print("DeleteRichtwert2 " & cmd.CommandText & " returned " & anz)
        cn.Close()
        Return anz
    End Function
    Private Function DeleteRichtwert1(ByVal id As Long, ByVal art As String) As Integer
        Dim cn As New OleDb.OleDbConnection
        Dim cmd As New OleDb.OleDbCommand
        Dim anz As Integer
        OpenDBConnection(cn)
        cmd.Connection = cn
        'delete fzg, ArtNr
        cmd.CommandText = "delete from richtwerte2 where id=" & id & " AND ArtNr='" & art & "'"
        anz = cmd.ExecuteNonQuery
        Debug.Print("DeleteRichtwert1 " & cmd.CommandText & " returned " & anz)

        'check if this is only one entry in the richtwert1 table
        cmd.CommandText = "select * from richtwerte1 where ArtNr='" & art & "'"
        anz = cmd.ExecuteNonQuery
        Debug.Print("DeleteRichtwert1 " & cmd.CommandText & " returned " & anz)

        'if there is no other entry for this ArtNr delete the AwText entry too
        If anz = 0 Then
            anz = cmd.CommandText = "delete from richtwerte1 where ArtNr='" & art & "'"
            Debug.Print("DeleteRichtwert2 " & cmd.CommandText & " returned " & anz)
        End If
        cn.Close()
        Return anz
    End Function
    'Private Function DeleteRichtwert2(ByVal typ As String, ByVal art As String) As Integer
    '    Dim cn As New OleDb.OleDbConnection
    '    Dim cmd As New OleDb.OleDbCommand
    '    Dim anz As Integer
    '    OpenDBConnection(cn)
    '    cmd.Connection = cn
    '    'delete fzg, ArtNr
    '    cmd.CommandText = "delete from richtwerte2 where FzgTyp='" & typ & "' AND ArtNr='" & art & "'"
    '    anz = cmd.ExecuteNonQuery
    '    Debug.Print("DeleteRichtwert1 " & cmd.CommandText & " returned " & anz)

    '    'check if this is only one entry in the richtwert1 table
    '    cmd.CommandText = "select * from richtwerte1 where ArtNr='" & art & "'"
    '    anz = cmd.ExecuteNonQuery
    '    Debug.Print("DeleteRichtwert1 " & cmd.CommandText & " returned " & anz)

    '    'if there is no other entry for this ArtNr delete the AwText entry too
    '    If anz = 0 Then
    '        anz = cmd.CommandText = "delete from richtwerte1 where ArtNr='" & art & "'"
    '        Debug.Print("DeleteRichtwert2 " & cmd.CommandText & " returned " & anz)
    '    End If
    '    cn.Close()
    '    Return anz
    'End Function

    Private Sub btnAddRichtwert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddRichtwert.Click
        Dim cn As New OleDb.OleDbConnection
        Dim cmd As New OleDb.OleDbCommand
        OpenDBConnection(cn)
        cmd.Connection = cn
        Dim anz
        Dim AWnr As String = txtRichtwertNr.Text
        Dim AWtxt As String = txtText.Text

        'Richtwertnummer?
        Dim dlg As EingabeBox = New EingabeBox
        Dim rueckgabe As String
nochmal:
        dlg.Text = "Neuer Richtwert"
        dlg.txtWert.Text = AWnr
        dlg.prompt.Text = "Richtwertnummer?:"
        'dlg.Top = oben
        'dlg.Left = links
        If dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            rueckgabe = dlg.txtWert.Text
            AWnr = rueckgabe
        Else
            Return
        End If
        If Not AWnr.StartsWith("ZEIT/") Then
            If MessageBox.Show("Richtwerte sollten immer mit 'ZEIT/' beginnen. Eingabe wiederholen?", "Richtwerteingabe", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                GoTo nochmal
            End If
        End If

        'Richtwerttext?
        dlg.Text = "Neuer Richtwert"
        dlg.txtWert.Text = AWtxt
        dlg.prompt.Text = "Richtwert-Text:"
        'dlg.Top = oben
        'dlg.Left = links
        If dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            rueckgabe = dlg.txtWert.Text
            AWtxt = rueckgabe
        Else
            Return
        End If
        dlg.Dispose()


        If AWnr.Length > 0 And _
           AWtxt.Length > 0 Then
            If ExistsData("Select * from richtwerte1 where ArtNr'" & AWnr & "'") Then
                If MessageBox.Show("Richtwert <" & AWnr & "> existiert bereits. Überschreiben?", "Neuen Richtwert anlegen", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    cmd.CommandText = "update richtwerte1 set ArtNr='" & AWnr & "', AwText='" & AWtxt & "' where id=" & lblID1.Text
                    anz = cmd.ExecuteNonQuery()
                    MessageBox.Show(anz & " Datenzeile(n) geändert", "Richtwerttabelle")
                    cmd.Dispose()
                    FillTabelle1(AWnr)
                End If
            Else
                cmd.CommandText = "insert into richtwerte1 (id, ArtNr, AWText) values (" & _
                lblID2.Text & _
                ", '" & AWnr & "'" & _
                ", '" & AWtxt & "')"
                anz = cmd.ExecuteNonQuery
                MessageBox.Show(anz & " Datenzeile(n) hinzugefügt", "Richtwerttabelle")
                cmd.Dispose()
                FillTabelle1(AWnr)
            End If
        End If
        cn.Close()
    End Sub

    Private Sub dgv2_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv2.CellDoubleClick
        RefreshDGV2()
    End Sub
    Private Sub RefreshDGV2()
        Dim artnr As String = ""
        Try
            If Not IsNothing(dgv2.CurrentCell) Then
                artnr = CStr(dgv2.Item("ArtNr", dgv2.CurrentCell.RowIndex).Value)
                'FillTabelle1(artnr)
                'FindInDGV1(artnr)
                txtRichtwertNr.Text = artnr
                If Not IsNothing(dgv2.CurrentCell) Then
                    txtText.Text = CStr(dgv1.Item("AWText", dgv1.CurrentCell.RowIndex).Value)
                End If
                txtTypKlasse.Text = CStr(dgv2.Item("FzgTyp", dgv2.CurrentCell.RowIndex).Value)
                'txtText.Text = CStr(dgv2.Item("AwText", dgv2.CurrentCell.RowIndex).Value)
                lblID2.Text = CStr(dgv2.Item("id", dgv2.CurrentCell.RowIndex).Value)
                txtPreis.Text = CStr(CDouble(dgv2.Item("Preis", dgv2.CurrentCell.RowIndex).Value))
                Preis = CDbl(txtPreis.Text)
                txtPreis.Text = String.Format("{0:0.00}", Preis)
            Else
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub FindInDGV1(ByVal art As String)
        With dgv1
            .Sort(.Columns("ArtNr"), System.ComponentModel.ListSortDirection.Ascending)
            Dim i As Integer
            For i = 0 To .Rows.Count - 1
                If .Item("ArtNr", i).Value = art Then
                    .CurrentCell = .Rows(i).Cells("ArtNr")
                    .FirstDisplayedScrollingRowIndex = .Rows(i).Index
                    .Refresh()
                    '.CurrentCell = .Rows(i).Cells(1)
                    .Rows(i).Selected = True
                End If
            Next
        End With
    End Sub
    Private Sub dgv2_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgv2.SelectionChanged
        RefreshDGV2()
    End Sub

    Private Sub btnSaveRichtwert1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveRichtwert1.Click
        Dim cn As New OleDb.OleDbConnection
        Dim cmd As New OleDb.OleDbCommand
        Dim anz As Integer
        If CheckTextLength(txtText) _
            And CheckTextLength(txtRichtwertNr) _
            And lblID1.Text <> "" Then
            OpenDBConnection(cn)
            cmd.Connection = cn
            'delete fzg, ArtNr
            cmd.CommandText = "update richtwerte1 set ArtNr='" & txtRichtwertNr.Text & _
            "', AwText='" & txtText.Text & "'  where id=" & lblID1.Text
            anz = cmd.ExecuteNonQuery
            Debug.Print("btnSaveRichtwert1 " & cmd.CommandText & " returned " & anz)
            cn.Close()
            MessageBox.Show(anz & " Datenzeile(n) geändert", "Richtwerttabelle")
            FillTabelle2("")
            FillTabelle1("")
            FindInDGV1(txtRichtwertNr.Text)
        End If
    End Sub

    Private Sub btnDeleteFzgTyp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteFzgTyp.Click
        Dim cn As New OleDb.OleDbConnection
        Dim cmd As New OleDb.OleDbCommand
        Dim anz As Integer
        If MessageBox.Show("Aktuelle Datenzeile löschen?", "Richtwerttabelle", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            OpenDBConnection(cn)
            cmd.Connection = cn
            'delete fzg, ArtNr
            cmd.CommandText = "delete from richtwerte2 where id=" & lblID2.Text
            anz = cmd.ExecuteNonQuery
            Debug.Print("btnDeleteFzgTyp " & cmd.CommandText & " returned " & anz)
            cn.Close()
            MessageBox.Show(anz & " Datenzeile(n) gelöscht", "Richtwerttabelle")
            FillTabelle2("")
            FillTabelle1("")
        End If
    End Sub

    Private Sub dgv1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgv1.SelectionChanged
        If Not IsNothing(dgv1.CurrentCell) Then
            lblID1.Text = CStr(dgv1.Item("id", dgv1.CurrentCell.RowIndex).Value)
        End If
    End Sub

    Private Sub dgv1_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv1.CellClick
        RefreshDGV1()
    End Sub


    Private Sub btnSaveFzgTyp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveFzgTyp.Click
        Dim cn As New OleDb.OleDbConnection
        Dim cmd As New OleDb.OleDbCommand
        Dim anz As Integer
        If CheckTextLength(txtText) _
            And CheckTextLength(txtRichtwertNr) _
            And lblID2.Text <> "" _
            And CheckTextDouble(txtPreis) _
            And CheckTextLength(txtTypKlasse) Then
            OpenDBConnection(cn)
            cmd.Connection = cn
            'delete fzg, ArtNr
            cmd.CommandText = "update richtwerte2 set " & _
            "ArtNr='" & txtRichtwertNr.Text & "'" & _
            ", FzgTyp='" & txtTypKlasse.Text & "'" & _
            ", Preis=" & CDoubleS(txtPreis.Text) & _
            "  where id=" & lblID2.Text
            anz = cmd.ExecuteNonQuery
            Debug.Print("btnSaveFzgTyp " & cmd.CommandText & " returned " & anz)
            cn.Close()
            MessageBox.Show(anz & " Datenzeile(n) geändert", "Richtwerttabelle")
            FillTabelle2("")
            FillTabelle1("")
            FindInDGV1(txtRichtwertNr.Text)
        End If

    End Sub

    Private Sub btnAddFzgTyp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddFzgTyp.Click
        Dim cn As New OleDb.OleDbConnection
        Dim cmd As New OleDb.OleDbCommand
        OpenDBConnection(cn)
        cmd.Connection = cn
        Dim anz
        Dim TypNr As String = txtTypKlasse.Text
        Dim sPreis As String = txtPreis.Text
        Dim Preis As Double
        Dim AwNr As String = txtRichtwertNr.Text

        'FzgTyp?
        Dim dlg As EingabeBox = New EingabeBox
        Dim rueckgabe As String
nochmal2:
        dlg.Text = "Neue Richtwertdaten für <" & AwNr & "> anlegen"
        dlg.txtWert.Text = TypNr
        dlg.prompt.Text = "Fahrzeugtyp?:"
        'dlg.Top = oben
        'dlg.Left = links
        If dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            rueckgabe = dlg.txtWert.Text
            TypNr = rueckgabe
        Else
            Return
        End If
        If Not ExistsData("Select typ from Kundfahr where TYP='" & TypNr & "'") Then
            If MessageBox.Show("Dieser Typ ist in der Fahrzeugdatenbank noch nicht bekannt. Eingabe wiederholen?", "Richtwerteingabe", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                GoTo nochmal2
            End If
        End If

preisfalsch:
        'Preis?
        dlg.Text = "Neue Richtwertdaten für <" & AwNr & "> anlegen"
        dlg.txtWert.Text = sPreis
        dlg.prompt.Text = "Richtwert-Preis:"
        'dlg.Top = oben
        'dlg.Left = links
        If dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            rueckgabe = dlg.txtWert.Text
            sPreis = CDoubleS(rueckgabe)
        Else
            Return
        End If
        Try
            Preis = CDbl(rueckgabe)
        Catch ex As Exception
            Preis = 0
        End Try
        If Preis = 0 Then
            If MessageBox.Show("Preis ungültig. Eingabe wiederholen?", "Richtwerteingabe", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                GoTo preisfalsch
            Else
                cn.Close()
                Return
            End If
        End If
        dlg.Dispose()

        If AwNr.Length > 0 And _
           Preis > 0 Then
            If ExistsData("Select * from richtwerte2 where ArtNr'" & AwNr & "' AND FzgTyp='" & TypNr & "'") Then
                If MessageBox.Show("Richtwertpreis <" & AwNr & "> existiert bereits. Überschreiben?", "Neuen Richtwert anlegen", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    cmd.CommandText = "update richtwerte2 set ArtNr='" & AwNr & "', Preis=" & CDoubleS(Preis) & " where FzgTyp='" & TypNr & "'"
                    anz = cmd.ExecuteNonQuery()
                    MessageBox.Show(anz & " Datenzeile(n) geändert", "Richtwerttabelle")
                    cmd.Dispose()
                    FillTabelle1(AwNr)
                End If
            Else
                cmd.CommandText = "insert into richtwerte2 (ArtNr, FzgTyp, Preis) values (" & _
                "'" & AwNr & "'" & _
                ", '" & TypNr & "'" & _
                ", " & CDoubleS(Preis) & ")"
                anz = cmd.ExecuteNonQuery
                MessageBox.Show(anz & " Datenzeile(n) hinzugefügt", "Richtwerttabelle")
                cmd.Dispose()
                FillTabelle2(AwNr)
            End If
        End If
        cn.Close()

    End Sub
End Class
