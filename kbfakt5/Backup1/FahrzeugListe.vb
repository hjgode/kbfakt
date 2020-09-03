Option Explicit On
Public Class Fahrzeugliste
    Public Kunden_nr As Double = 0
    Public fahrgestell_nr As String
    Private Sub Fahrzeugliste_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '"KUNDEN_NR, FGSTLLNR, TYP, KENNZEICH, KM_STAND, KFZ_BRF_NR, ZULASSUNG1, TUEV, ASU,  SCHREIBER,  SICHER"
        Dim sql As String
        If Kunden_nr = 0 Then
            sql = "Select KUNDEN_NR, FGSTLLNR, KUNDENST.VORNAME as Name1, KUNDENST.NACHNAME as Name2, TYP, KENNZEICH, " + _
                                                "KM_STAND, KFZ_BRF_NR, ZULASSUNG1, TUEV, ASU,  " + _
                                                "SCHREIBER,  SICHER from KUNDFAHR, KUNDENST " + _
                                                "where KUNDFAHR.KUNDEN_NR=KUNDENST.KUNDENNR" + _
                                                " order by KUNDENST.NACHNAME"
        Else
            sql = "Select KUNDEN_NR, FGSTLLNR, TYP, KENNZEICH, KUNDENST.VORNAME as Name1, KUNDENST.NACHNAME as Name2, " + _
                                    "KM_STAND, KFZ_BRF_NR, ZULASSUNG1, TUEV, ASU,  " + _
                                    "SCHREIBER,  SICHER from KUNDFAHR, KUNDENST " + _
                                    " where KUNDFAHR.KUNDEN_NR = KUNDENST.KUNDENNR AND KUNDEN_NR=" & Kunden_nr & _
                                    " order by KENNZEICH"
            '"SELECT KUNDFAHR.KUNDEN_NR, KUNDFAHR.KENNZEICH, KUNDFAHR.FGSTLLNR, KUNDENST.VORNAME, KUNDENST.NACHNAME"
            ' "FROM KUNDFAHR, KUNDENST where  KUNDFAHR.KUNDEN_NR = KUNDENST.KUNDENNR AND KUNDENNR=11003;

            'sql = "Select KUNDEN_NR, FGSTLLNR, KUNDENST.VORNAME as Name1, KUNDENST.NACHNAME as Name2, TYP, KENNZEICH, " + _
            '                                    "KM_STAND, KFZ_BRF_NR, ZULASSUNG1, TUEV, ASU,  " + _
            '                                    "SCHREIBER,  SICHER from KUNDFAHR, KUNDENST " + _
            '                                    " where KUNDFAHR.KUNDEN_NR=" & Kunden_nr & _
            '                                    " order by KUNDEN_NR"
            '  " AND  KUNDFAHR.KUNDEN_NR=KUNDENST.KUNDENNR" & _
        End If
        kbfakt_start.FillGrid(GridFahrzeuge, sql)
        Try
            If GridFahrzeuge.RowCount > 0 And Kunden_nr > 0 Then
                For i As Integer = 0 To GridFahrzeuge.RowCount - 1
                    If GridFahrzeuge.Item(0, i).Value = CStr(Kunden_nr) Then
                        'found it
                        'MsgBox("Found " & auftrag_nr & "in Row " & i)
                        Dim row As DataGridViewRow = Me.GridFahrzeuge.Rows(i)
                        Dim cell As DataGridViewCell = row.Cells(0)
                        GridFahrzeuge.CurrentCell = cell
                        'FirstDisplayedCell = cell
                    End If
                Next
            End If
            SetFahrgstNr()
        Catch
        End Try
    End Sub

    Private Sub btn_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OK.Click
        SetFahrgstNr()
        Me.Close()
    End Sub

    Private Sub btn_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        fahrgestell_nr = ""
        Kunden_nr = 0
        Me.Close()
    End Sub

    Private Sub GridAuftraege_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridFahrzeuge.Click
        SetFahrgstNr()

    End Sub

    Private Sub GridAuftraege_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridFahrzeuge.DoubleClick
        SetFahrgstNr()
        Me.Close()
    End Sub

    Private Sub GridAuftraege_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles GridFahrzeuge.PreviewKeyDown
        If e.KeyCode = Keys.Enter Then
            SetFahrgstNr()
            Me.Close()
        Else
            If e.KeyCode = Keys.Escape Then
                fahrgestell_nr = ""
                Kunden_nr = 0
                Me.Close()
            End If
        End If
    End Sub
    Private Sub SetFahrgstNr()
        If Not IsNothing(GridFahrzeuge.CurrentCell) Then
            'auftrag_nr = CDbl(GridAuftraege.CurrentCell.Value)
            fahrgestell_nr = GridFahrzeuge.Item(1, GridFahrzeuge.CurrentCell.RowIndex).Value
            Kunden_nr = GridFahrzeuge.Item(0, GridFahrzeuge.CurrentCell.RowIndex).Value
        Else
            fahrgestell_nr = ""
            Kunden_nr = 0
        End If

    End Sub

    Private Sub btnDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetails.Click
        Dim dlg As New FahrzeugDetails
        SetFahrgstNr()
        If fahrgestell_nr <> "" And Kunden_nr <> 0 Then
            dlg.FahrgestellNr = fahrgestell_nr
            dlg.KundenNr = Kunden_nr
            dlg.ShowDialog()
        Else
            MessageBox.Show("Ungültige Kunden- oder Fahrgestell-Nr (>" & Kunden_nr & "<, >" & fahrgestell_nr & "<")
        End If
    End Sub

    Private Sub btnRechnungen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRechnungen.Click
        Dim FgstellNr As String = ""
        If Not IsNothing(GridFahrzeuge.CurrentCell) Then
            FgstellNr = GridFahrzeuge.Item(1, GridFahrzeuge.CurrentCell.RowIndex).Value
            Dim dlg As ListenAnsicht
            dlg = New ListenAnsicht
            dlg.RechnungsNr = 0
            dlg.KundenNr = 0
            dlg.FahrgestellNr = FgstellNr
            dlg.Show()
        End If

    End Sub
End Class