Public Class ArtikelListe
    Public ArtikelNr As String = ""
    Public ARTIKELBEZ As String
    Public ArtTyp As Integer
    Public menge As Double
    Public vk As Double
    Public rabatt As Double
    Public MitarbeiterCode As Integer
    Public DetailsMode As Boolean = False
    Private Sub ArtikelListe_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '"ARTIKELNR, ARTIKELBEZ, BESTAND, VK, LAGERORT, LETZ_EK, MISCH_EK, 
        ' RABATTCODE, MIND_MENGE, LETZ_BESTL, LETZ_VK, LETZEKMENG, LIEFER1, 
        ' EK1, LIEFER2, EK2, LIEFER3, EK3"
        If DetailsMode Then
            btnDetails.Visible = True
        Else
            btnDetails.Visible = False
        End If
        If ArtikelNr.Length > 0 Then
            txtboxFilter.Text = ArtikelNr
        Else
            txtboxFilter.Text = ""
        End If
        FillData()
        Application.DoEvents()
        '+ _
        '                                   "where KUNDENST.Anrede=Anreden.id " + _
        '                                  "order by KUNDENNR")
        If Not IsNothing(GridArtikel.CurrentCell) Then
            ArtikelNr = GridArtikel.Item(0, GridArtikel.CurrentCell.RowIndex).Value
            txtPreis.Text = CStr(CDouble(GridArtikel.Item("VK", GridArtikel.CurrentCell.RowIndex).Value))
            vk = CDouble(GridArtikel.Item("VK", GridArtikel.CurrentCell.RowIndex).Value)
        End If
        FillListBox()
    End Sub
    Private Sub FillListBox()
        ComboBox1.Items.Clear()
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim cmd As New OleDb.OleDbCommand("select * from ArtTypen order by id", cn)
        Dim dr As OleDb.OleDbDataReader = cmd.ExecuteReader
        If dr.HasRows Then
            'Dim i As Integer = 0
            While dr.Read()
                ComboBox1.Items.Add(dr.Item("ArtikelTyp").ToString)
                'i = i + 1
            End While
            ComboBox1.Items.Add("Alles")
            ComboBox1.SelectedIndex = ComboBox1.Items.Count - 1
        End If
        dr.Close()
        cn.Close()
    End Sub
    Private Sub FillData()
        If txtboxFilter.Text.Length > 0 Then
            kbfakt_start.FillGrid(GridArtikel, "Select ARTIKELNR, ARTIKELBEZ, BESTAND, VK, id, ArtTyp" & _
                                        " from ARTSTAMM" & _
                                        " where ARTIKELNR like '" & txtboxFilter.Text & "%' OR " & _
                                        " ARTIKELBEZ like '" & txtboxFilter.Text & "%' order by ARTIKELNR")
        Else
            kbfakt_start.FillGrid(GridArtikel, "Select ARTIKELNR, ARTIKELBEZ, BESTAND, VK, id, ArtTyp" & _
                                        " from ARTSTAMM order by ARTIKELNR")
        End If
        Dim foundIt As Boolean = False
        'Dim intcount As Int32 = 0
        For Each row As DataGridViewRow In GridArtikel.Rows
            If row.Cells(0).Value = ArtikelNr Then
                foundIt = True
                row.Selected = True
                Exit For
                'intcount += 1
            End If
        Next
    End Sub
    Private Sub btn_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OK.Click
        Try
            ArtikelNr = GridArtikel.Item("ARTIKELNR", GridArtikel.CurrentCell.RowIndex).Value
            ARTIKELBEZ = GridArtikel.Item("ARTIKELBEZ", GridArtikel.CurrentCell.RowIndex).Value
            ArtTyp = CInteger(GridArtikel.Item("ArtTyp", GridArtikel.CurrentCell.RowIndex).Value)
            vk = CDouble(txtPreis.Text)
            menge = CDouble(txtMenge.Text)
            rabatt = CDouble(txtRabatt.Text)
            DialogResult = Windows.Forms.DialogResult.OK
        Catch
            Me.Close()
        End Try
    End Sub

    Private Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        ArtikelNr = ""
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub GridArtikel_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridArtikel.DoubleClick
        'update txt fields
        GridArtikel_CellClick(Nothing, Nothing)
        If DetailsMode Then
            btnDetails_Click(Nothing, Nothing)
            Return
        End If
        If Not IsNothing(GridArtikel.CurrentCell) Then
            txtPreis.Text = CStr(CDouble(GridArtikel.Item("VK", GridArtikel.CurrentCell.RowIndex).Value))
            btn_OK_Click(Nothing, Nothing)
        Else
            btn_cancel_Click(Nothing, Nothing)
        End If
        Me.Close()
    End Sub

    Private Sub GridArtikel_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles GridArtikel.PreviewKeyDown
        If e.KeyCode = Keys.Enter Then
            btn_OK_Click(Nothing, Nothing)
            Me.Close()
            Exit Sub
        End If
    End Sub

    Private Sub GridArtikel_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridArtikel.CellClick
        If Not IsNothing(GridArtikel.CurrentCell) Then
            txtPreis.Text = CStr(CDouble(GridArtikel.Item("VK", GridArtikel.CurrentCell.RowIndex).Value))
        End If
    End Sub
    Private Sub GridArtikel_ColumnHeaderMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles GridArtikel.ColumnHeaderMouseClick
        If GridArtikel.Rows.Count = 0 Then
            Exit Sub
        End If
    End Sub

    Private Sub btnFiltern_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFiltern.Click
        ArtikelNr = txtboxFilter.Text
        FillData()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnKeinFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKeinFilter.Click
        txtboxFilter.Text = ""
        ArtikelNr = txtboxFilter.Text
        FillData()
    End Sub

    Private Sub btnDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetails.Click

        If GridArtikel.CurrentCell Is Nothing Then Exit Sub
        Dim dlg As ArtikelDetails
        dlg = New ArtikelDetails
        dlg.ArtikelNr = GridArtikel.Item(0, GridArtikel.CurrentCell.RowIndex).Value
        dlg.ShowDialog()
        dlg.Dispose()
        FillData()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        DataGridViewExport(GridArtikel)
    End Sub

    Private Sub ComboBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
    '2.0.2.4 HGO
    Private Sub btnFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Dim selectstring As String
        selectstring = "Select ARTIKELNR, ARTIKELBEZ, BESTAND, VK, id, ArtTyp, FzgTyp" & _
                                            " from ARTSTAMM "
        If ComboBox1.Items(ComboBox1.SelectedIndex).ToString = "Alles" Then
            ArtTyp = 99
        Else
            ArtTyp = ComboBox1.SelectedIndex
        End If
        'If optTeile.Checked Then ArtTyp = 0
        'If optSchmiermittel.Checked Then ArtTyp = 1
        'If optLohn.Checked Then ArtTyp = 2
        'If optAltteil.Checked Then ArtTyp = 3
        Select Case ArtTyp
            Case 0 To 3
                selectstring = selectstring + " where ArtTyp=" & ArtTyp & " order by ARTIKELNR"
            Case 99
                selectstring = selectstring + " order by ARTIKELNR"
        End Select
        kbfakt_start.FillGrid(GridArtikel, selectstring)
        If Not IsNothing(GridArtikel.CurrentCell) Then
            ArtikelNr = GridArtikel.Item(0, GridArtikel.CurrentCell.RowIndex).Value
        End If

    End Sub
End Class