Imports System.Data.OleDb
''' <summary>
''' Show a Kunden or Rechnungen list for KundenNr or RechnungsNr
''' Show a list of all KundFahr
''' </summary>
''' <remarks>
''' set KundenNr=0 to use RechnungsNr
''' set RechnungsNr=0 to use KundenNr
''' set KundenNr=0 and RechnungsNr=0 to use FahrgestellNr
''' </remarks>
Public Class ListenAnsicht
    Public KundenNr As Double
    Public RechnungsNr As Double
    Public FahrgestellNr As String

    Private Sub ListenAnsicht_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cmd As New OleDbCommand() '(queryString, connection)
        Dim queryString As String
        Dim connection As New OleDbConnection()
        Dim o As Object
        OpenDBConnection(connection)
        cmd.Connection = connection
        Dim xNetto As Double
        queryString = "select sum (xnetto) from rech1 where XKundenNr=" & KundenNr

        If KundenNr <> 0 Then 'Aufträge des Kunden
            '"XAUFTR_NR, XKUNDENNR, XAN1, XNAME1, XNAME2, XST, XPL, XOT, XFGSTLLNR, XDATUM, XNETTO, XAN, XSCHMIER, XLOHN, XSONDER, XKMSTAND, XKZ, XZULASS, XTYP, XMWS, XTEL"
            kbfakt_start.FillGrid(DataGridView1, "Select XAUFTR_NR as Aufrag, XDATUM as RgDatum, XFGSTLLNR as FahrgestNr, XKMSTAND as KMStand, XNETTO as Netto from Rech1 where XKundenNr=" & _
            KundenNr & _
            "  ORDER BY  XDATUM")
            btnDetails.Enabled = True
            'Gesamt berechnen
            queryString = "select sum (xnetto) from rech1 where XKundenNr=" & KundenNr
        ElseIf RechnungsNr <> 0 Then 'Posten einer Rechnung
            '"AUFTR_NR, ARTIKELNR, ARTIKELBEZ, KONTO, MENGE, E_PREIS, RABATT, LOHNART, IST_AW, MITARBCODE, FAHRZEUG, TEXT"
            kbfakt_start.FillGrid(DataGridView1, "Select ARTIKELNR, ARTIKELBEZ , MENGE , E_Preis, RABATT, menge * e_preis - (RABATT/100 * (menge * e_preis)) as Gesamt from Rech2 where AUFTR_NR=" & _
            RechnungsNr & _
            "  ORDER BY  pos")
            btnDetails.Enabled = False
            'Gesamt berechnen
            queryString = "select sum (menge * e_preis - (RABATT/100 * (menge * e_preis))) from rech2 where AUFTR_NR=" & RechnungsNr
        ElseIf FahrgestellNr = "" Then
            '"KUNDEN_NR, FGSTLLNR, TYP, KENNZEICH, KM_STAND, KFZ_BRF_NR, ZULASSUNG1, TUEV, ASU,  SCHREIBER,  SICHER"
            kbfakt_start.FillGrid(DataGridView1, "Select FGSTLLNR, KENNZEICH, TYP, KM_STAND, KFZ_BRF_NR, ZULASSUNG1, TUEV, ASU,  SCHREIBER,  SICHER, KUNDEN_NR  from KundFahr" & _
            "  ORDER BY  KENNZEICH")
            btnDetails.Enabled = True
            'Gesamt berechnen
            queryString = ""
        ElseIf FahrgestellNr <> "" Then
            '"KUNDEN_NR, FGSTLLNR, TYP, KENNZEICH, KM_STAND, KFZ_BRF_NR, ZULASSUNG1, TUEV, ASU,  SCHREIBER,  SICHER"
            kbfakt_start.FillGrid(DataGridView1, "Select XAUFTR_NR as Aufrag, XDatum as Datum, XFGSTLLNR as Fahrgestell, XKZ as Kennzeichen, XNETTO as Netto, MwStSatz from Rech1 where XFGSTLLNR='" & _
            FahrgestellNr & _
            "'  ORDER BY  XDATUM")
            btnDetails.Enabled = True
            'Gesamt berechnen
            queryString = "select sum (xnetto) from rech1 where XFGSTLLNR='" & FahrgestellNr & "'"
        End If
        'Gesamt berechnen
        If queryString <> "" Then
            txtXNetto.Visible = True
            LabelGesamt.Visible = True
            cmd.CommandText = queryString
            o = cmd.ExecuteScalar
            If Not IsDBNull(o) Then
                xNetto = CDbl(o)
            Else
                xNetto = 0
            End If
            txtXNetto.Text = String.Format("{0:0.00}", xNetto)
        Else
            txtXNetto.Visible = False
            LabelGesamt.Visible = False
        End If
        connection.Close()
        DataGridView1.RowHeadersVisible = False
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub btnDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetails.Click
        'if Rechnungen-Ansicht (KundenNr<>0)
        If KundenNr <> 0 Then
            If Not IsNothing(DataGridView1.CurrentCell) Then
                RechnungsNr = DataGridView1.Item(0, DataGridView1.CurrentCell.RowIndex).Value
                Dim dlg As ListenAnsicht
                dlg = New ListenAnsicht
                dlg.RechnungsNr = RechnungsNr
                dlg.KundenNr = 0
                dlg.ShowDialog()
                dlg.Dispose()
            End If
        End If
        'details for rechnungliste für eine fahrgestellnr
        If FahrgestellNr <> "" And KundenNr = 0 And RechnungsNr = 0 Then
            If Not IsNothing(DataGridView1.CurrentCell) Then
                RechnungsNr = DataGridView1.Item(0, DataGridView1.CurrentCell.RowIndex).Value
                Dim dlg As ListenAnsicht
                dlg = New ListenAnsicht
                dlg.RechnungsNr = RechnungsNr
                dlg.KundenNr = 0
                dlg.ShowDialog()
                dlg.Dispose()
                RechnungsNr = 0
            End If
        End If
        If FahrgestellNr = "" And KundenNr = 0 And RechnungsNr = 0 Then
            Dim FgstellNr As String = ""
            If Not IsNothing(DataGridView1.CurrentCell) Then
                FgstellNr = DataGridView1.Item(0, DataGridView1.CurrentCell.RowIndex).Value
            End If
            FahrzeugDetails.KundenNr = KundenNr
            If FgstellNr <> "" Then
                FahrzeugDetails.FahrgestellNr = FgstellNr 'if FahrgestellNr is empty, the neu fahrzeug dialog will be shown
                FahrzeugDetails.ShowDialog()
                FahrzeugDetails.Dispose()
                'update
                DataGridView1.DataSource = Nothing
                kbfakt_start.FillGrid(DataGridView1, "Select FGSTLLNR, KENNZEICH, TYP, KM_STAND, KFZ_BRF_NR, ZULASSUNG1, TUEV, ASU,  SCHREIBER,  SICHER, KUNDEN_NR  from KundFahr" & _
                "  ORDER BY  KENNZEICH")
            End If
        End If

    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If btnDetails.Enabled = True Then btnDetails_Click(sender, e)
    End Sub
End Class