Option Explicit On
Public Class Auftragsliste
    Public auftrag_nr As Double
    Private Sub Auftragsliste_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        kbfakt_start.FillGrid(GridAuftraege, "Select XAUFTR_NR as Auftragnummer, " + _
                                                "XDATUM as Datum, " + _
                                                "XNAME1 as Name1, " + _
                                                "XNAME2 as Name2, " + _
                                                "XFGSTLLNR as Fahrgestellnummer, " + _
                                                "XKZ as Kennzeichen, " + _
                                                "XKUNDENNR as Kundennummer, " + _
                                                "id " + _
                                                " from Rech1" + _
                                                " ORDER by XAUFTR_NR DESC")
        If GridAuftraege.RowCount > 0 Then
            For i As Integer = 0 To GridAuftraege.RowCount - 1
                If GridAuftraege.Item(0, i).Value = CStr(auftrag_nr) Then
                    'found it
                    'MsgBox("Found " & auftrag_nr & "in Row " & i)
                    Dim row As DataGridViewRow = Me.GridAuftraege.Rows(i)
                    Dim cell As DataGridViewCell = row.Cells(0)
                    GridAuftraege.CurrentCell = cell
                    'FirstDisplayedCell = cell
                End If
            Next
        End If
        If Not IsNothing(GridAuftraege.CurrentCell) Then
            auftrag_nr = GridAuftraege.Item(0, GridAuftraege.CurrentCell.RowIndex).Value
        Else
            auftrag_nr = 0
        End If
        GridAuftraege.ReadOnly = True
    End Sub

    Private Sub btn_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OK.Click
        If Not IsNothing(GridAuftraege.CurrentCell) Then
            'auftrag_nr = CDbl(GridAuftraege.CurrentCell.Value)
            auftrag_nr = GridAuftraege.Item(0, GridAuftraege.CurrentCell.RowIndex).Value
        Else
            auftrag_nr = 0
        End If
        Me.Close()
    End Sub

    Private Sub btn_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        auftrag_nr = 0
        Me.Close()
    End Sub

    Private Sub GridAuftraege_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridAuftraege.Click
        If Not IsNothing(GridAuftraege.CurrentCell) Then
            'auftrag_nr = CDbl(GridAuftraege.CurrentCell.Value)
            auftrag_nr = GridAuftraege.Item(0, GridAuftraege.CurrentCell.RowIndex).Value
        End If

    End Sub

    Private Sub GridAuftraege_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridAuftraege.DoubleClick
        If Not IsNothing(GridAuftraege.CurrentCell) Then
            'auftrag_nr = CDbl(GridAuftraege.CurrentCell.Value)
            auftrag_nr = GridAuftraege.Item(0, GridAuftraege.CurrentCell.RowIndex).Value
        Else
            auftrag_nr = 0
        End If
        Me.Close()
    End Sub

    Private Sub GridAuftraege_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles GridAuftraege.PreviewKeyDown
        If e.KeyCode = Keys.Enter Then
            If Not IsNothing(GridAuftraege.CurrentCell) Then
                'auftrag_nr = CDbl(GridAuftraege.CurrentCell.Value)
                auftrag_nr = GridAuftraege.Item(0, GridAuftraege.CurrentCell.RowIndex).Value
            Else
                auftrag_nr = 0
            End If
            Me.Close()
        Else
            If e.KeyCode = Keys.Escape Then
                auftrag_nr = 0
                Me.Close()
            End If
        End If
    End Sub
End Class