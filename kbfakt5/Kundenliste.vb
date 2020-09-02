Option Explicit On
Public Class Kundenliste
    Public Kunden_nr As Double = 0
    Public InvokedByStart As Boolean = False
    Private Sub Auftragsliste_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ReadKundenListe()
    End Sub
    Private Sub ReadKundenListe()
        '"KUNDENNR, ANREDE , VORNAME , NACHNAME, BRANCHE, PLZ , ORT , STRASSE , ANSPRECHP, TELEFON , KONTO1"
        kbfakt_start.FillGrid(GridKundenliste, "Select KUNDENNR, Anreden.ANREDE , VORNAME, " + _
                                            "NACHNAME, BRANCHE, PLZ, " + _
                                            "ORT , STRASSE , ANSPRECHP, TELEFON," + _
                                            "KONTO1, KUNDENST.id from KUNDENST, Anreden " + _
                                            "where KUNDENST.Anrede=Anreden.id " + _
                                            "order by KUNDENNR")
        If GridKundenliste.RowCount > 0 And Kunden_nr > 0 Then
            For i As Integer = 0 To GridKundenliste.RowCount - 1
                If GridKundenliste.Item(0, i).Value = CStr(Kunden_nr) Then
                    'found it
                    'MsgBox("Found " & auftrag_nr & "in Row " & i)
                    Dim row As DataGridViewRow = Me.GridKundenliste.Rows(i)
                    Dim cell As DataGridViewCell = row.Cells(0)
                    GridKundenliste.CurrentCell = cell
                    'FirstDisplayedCell = cell
                End If
            Next
        End If
        If Not IsNothing(GridKundenliste.CurrentCell) Then
            Kunden_nr = GridKundenliste.Item(0, GridKundenliste.CurrentCell.RowIndex).Value
        End If

    End Sub

    Private Sub btn_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OK.Click
        If Not IsNothing(GridKundenliste.CurrentCell) Then
            'auftrag_nr = CDbl(GridAuftraege.CurrentCell.Value)
            Kunden_nr = GridKundenliste.Item(0, GridKundenliste.CurrentCell.RowIndex).Value
        Else
            Kunden_nr = 0
        End If
        Me.Close()
    End Sub

    Private Sub btn_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        Kunden_nr = 0
        Me.Close()
    End Sub

    Private Sub GridAuftraege_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridKundenliste.Click
        If Not IsNothing(GridKundenliste.CurrentCell) Then
            'auftrag_nr = CDbl(GridAuftraege.CurrentCell.Value)
            Kunden_nr = GridKundenliste.Item(0, GridKundenliste.CurrentCell.RowIndex).Value
        End If

    End Sub

    Private Sub GridAuftraege_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridKundenliste.DoubleClick
        If Not IsNothing(GridKundenliste.CurrentCell) Then
            'auftrag_nr = CDbl(GridAuftraege.CurrentCell.Value)
            Kunden_nr = GridKundenliste.Item(0, GridKundenliste.CurrentCell.RowIndex).Value
        Else
            Kunden_nr = 0
        End If
        If InvokedByStart Then
            If Kunden_nr <> 0 Then ShowDetails()
        Else
            Me.Close()
        End If
    End Sub

    Private Sub GridAuftraege_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles GridKundenliste.PreviewKeyDown
        If e.KeyCode = Keys.Enter Then
            If Not IsNothing(GridKundenliste.CurrentCell) Then
                'auftrag_nr = CDbl(GridAuftraege.CurrentCell.Value)
                Kunden_nr = GridKundenliste.Item(0, GridKundenliste.CurrentCell.RowIndex).Value
            Else
                Kunden_nr = 0
            End If
            Me.Close()
        Else
            If e.KeyCode = Keys.Escape Then
                Kunden_nr = 0
                Me.Close()
            End If
        End If
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ShowDetails()
    End Sub
    Private Sub ShowDetails()
        Dim dlg As New KundenDetails
        dlg.KundenNr = Kunden_nr
        If InvokedByStart Then dlg.btnSuchen.Visible = False
        dlg.ShowDialog()
        dlg.Dispose()
        ReadKundenListe()
    End Sub

    Private Sub btnDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetails.Click
        If Not IsNothing(GridKundenliste.CurrentCell) Then
            'auftrag_nr = CDbl(GridAuftraege.CurrentCell.Value)
            Kunden_nr = GridKundenliste.Item(0, GridKundenliste.CurrentCell.RowIndex).Value
            ShowDetails()
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        DataGridViewExport(GridKundenliste)
    End Sub
End Class