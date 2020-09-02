Imports kbfakt5.DataGridViewPrinter

Public Class Auswertungen
    Public SelectString As String = ""
    Structure sSQL_type
        Dim t As String
        Dim sSQL As String
        Dim printSQL As String
    End Structure
    Dim sSQL() As sSQL_type
    Public bSammelDruck As Boolean = False
    Public TerminDruck As Boolean = False
    Public sqlPrint As String

    Private Sub Auswertungen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FormSettingsLoad(Me)
        Dim s As String = ""
        If SelectString = "" Then
            's = "select * from (SELECT kundenst.nachname as Name, SUM(rech1.xnetto) AS Gesamt, rech1.Gutschrift " & _
            '"FROM RECH1 INNER JOIN KUNDENST ON RECH1.XKUNDENNR=KUNDENST.KUNDENNR " & _
            '"GROUP BY kundenst.nachname ) order by Gesamt desc "
            s = "select * from (SELECT kundenst.nachname as Name, SUM(rech1.xnetto) AS Gesamt FROM RECH1, KundenSt where RECH1.XKUNDENNR=KUNDENST.KUNDENNR  AND rech1.gutschrift=FALSE GROUP BY kundenst.nachname ) order by Gesamt desc"
        Else
            s = SelectString
            ListBox1.Visible = False
        End If
        If (Not bSammelDruck And Not TerminDruck) Then
            If Not System.IO.File.Exists(getAppDir() + "auswertungen.xml") Then
                Dim ds As DataSet
                ds = AuswertungenCreateDS()
                AuswertungenWriteDS(ds)
            End If
            Init_sSQL()
        End If
        ReadData(s)
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersVisible = False
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub
    Private Sub ReadData(ByVal s As String)
        Dim cGesamt As Integer
        kbfakt_start.FillGrid(DataGridView1, s)
        cGesamt = s.IndexOf("Gesamt")
        If cGesamt <> -1 Then
            Dim sum As Double = 0
            Dim ri As Integer
            For ri = 0 To DataGridView1.RowCount - 1
                sum += CDouble(DataGridView1.Item("Gesamt", ri).Value)
            Next
            txtSumme.Text = Format(sum, "#.00")
        Else
            txtSumme.Text = "--"
        End If
    End Sub
    Private Sub Init_sSQL()
        Dim ds As DataSet
        ds = AuswertungenReadDS()
        If IsNothing(ds) Then
            Dim maxCount As Long = 6
            Dim i As Integer
            ReDim sSQL(maxCount - 1)
            sSQL(0).t = "Umsatz nach Kunden"
            sSQL(0).sSQL = "select * from (SELECT kundenst.nachname as Name, SUM(rech1.xnetto) AS Gesamt, rech1.Gutschrift " & _
                "FROM RECH1 INNER JOIN KUNDENST ON RECH1.XKUNDENNR=KUNDENST.KUNDENNR " & _
                "GROUP BY kundenst.nachname) order by Gesamt desc"

            sSQL(1).t = "Umsatz nach Monat"
            sSQL(1).sSQL = "SELECT YEAR(XDATUM) as Jahr, MONTH(XDATUM) as Monat, " & _
                            "SUM(XNETTO) as Gesamt FROM RECH1 group by YEAR(XDATUM), MONTH(XDATUM)  WHERE Gutschrift=FALSE"

            sSQL(2).t = "Umsatz nach Artikel"
            sSQL(2).sSQL = "select ARTIKELNR, ARTIKELBEZ, Gesamt from (" & _
                            "SELECT ARTIKELNR, ARTIKELBEZ, " & _
                            "sum (menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt " & _
                            "FROM RECH2 group by ARTIKELNR, ARTIKELBEZ ) WHERE Gutschrift=FALSE order by gesamt desc"

            sSQL(3).t = "Umsatz nach Artikel ohne ZEIT"
            sSQL(3).sSQL = "select ARTIKELNR, ARTIKELBEZ, Gesamt from (" & _
                            "SELECT ARTIKELNR, ARTIKELBEZ, " & _
                            "sum (menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt " & _
                            "FROM RECH2 where ARTIKELNR NOT Like 'ZEIT%' group by ARTIKELNR, ARTIKELBEZ ) WHERE Gutschrift=FALSE order by gesamt desc"

            sSQL(4).t = "Fahrzeugumsatz nach Kennzeichen"
            sSQL(4).sSQL = "SELECT kundenst.nachname as Name, RECH1.XKZ as Kennzeichen, sum(XNETTO) as Gesamt FROM RECH1, kundenst where kundenst.kundennr=rech1.xkundennr AND Gutschrift=FALSE GROUP BY kundenst.nachname, RECH1.XKZ order by xkz"

            sSQL(5).t = "Fahrzeugumsatz nach Kunden"
            sSQL(5).sSQL = "SELECT kundenst.nachname as Name, RECH1.XKZ as Kennzeichen, sum(XNETTO) as Gesamt FROM RECH1, kundenst where kundenst.kundennr=rech1.xkundennr GROUP BY kundenst.nachname, RECH1.XKZ  WHERE Gutschrift=FALSE order by kundenst.nachname"

            ListBox1.Items.Clear()
            For i = 0 To maxCount - 1
                ListBox1.Items.Insert(i, sSQL(i).t)
            Next
            ds = AuswertungenCreateDS()
            AuswertungenWriteDS(ds)
        Else
            ds = AuswertungenReadDS()
            ReDim sSQL(ds.Tables(0).Rows.Count)
            ListBox1.Items.Clear()
            Dim s As String
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                s = ds.Tables(0).Rows(i).Item("stringUI")
                ListBox1.Items.Insert(i, s)
                sSQL(i).sSQL = ds.Tables(0).Rows(i).Item("stringSQL")
                sSQL(i).t = ds.Tables(0).Rows(i).Item("stringUI")
                sSQL(i).printSQL = ds.Tables(0).Rows(i).Item("stringSQLprint")
            Next
        End If
        ListBox1.SelectedIndex = 0
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        FormSettingsSave(Me)
        Me.Close()
    End Sub
    Private Sub DruckenAuswertungSQL(ByVal sql As String)
        '----- Set SQL string for report by this format (1 set per 1 report column):
        '----- Column 1 -> column name
        '----- Column 2 -> Start position on paper, I set all width has range between 0-99
        '----- Column 3 -> End position on paper, I set all width has range between 0-99
        '----- Column 4 -> Justify (L-Left, R-Right, C-Center)
        '----- Column 5 -> Has summarize in this column (Y/N)
        '----- Column 6 -> Display format(such as #,##0.00)
        '----- Column 7 -> Rest in line? Begin with 1
        '----- Column 8 -> Data Column

        Dim dlg As New frmPrintDynamicForm
        Dim ix As Integer = ListBox1.SelectedIndex()

        Dim bs As BindingSource
        bs = CType(DataGridView1.DataSource, BindingSource)
        'Dim tbl As DataTable = CType(bs.DataSource, DataTable)
        Dim ds As DataSet = New DataSet()
        'ds.Tables.Add(tbl)
        Dim cn As New Data.OleDb.OleDbConnection
        OpenDBConnection(cn)

        Dim da As New Data.OleDb.OleDbDataAdapter(sql, cn)
        Try
            da.Fill(ds)
            dlg.dbSet = ds
            dlg.PrintPreview()
            dlg.Dispose()
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message, "Ausdruck nicht möglich")
        Finally
            dlg.Dispose()
        End Try
    End Sub

    Private Sub DruckenAuswertung()
        '----- Set SQL string for report by this format (1 set per 1 report column):
        '----- Column 1 -> column name
        '----- Column 2 -> Start position on paper, I set all width has range between 0-99
        '----- Column 3 -> End position on paper, I set all width has range between 0-99
        '----- Column 4 -> Justify (L-Left, R-Right, C-Center)
        '----- Column 5 -> Has summarize in this column (Y/N)
        '----- Column 6 -> Display format(such as #,##0.00)
        '----- Column 7 -> Rest in line? Begin with 1
        '----- Column 8 -> Data Column

        'Currently disabled
        'MessageBox.Show("Drucken zur Zeit nicht möglich", "In Bearbeitung")
        'Exit Sub
        Dim dlg As New frmPrintDynamicForm
        Dim ix As Integer = ListBox1.SelectedIndex()

        Dim bs As BindingSource
        bs = CType(DataGridView1.DataSource, BindingSource)
        'Dim tbl As DataTable = CType(bs.DataSource, DataTable)
        Dim ds As DataSet = New DataSet()
        'ds.Tables.Add(tbl)
        Dim cn As New Data.OleDb.OleDbConnection
        OpenDBConnection(cn)

        Dim s As String = "select * from (SELECT kundenst.nachname as Name, SUM(rech1.xnetto) AS Gesamt " & _
            "FROM RECH1 INNER JOIN KUNDENST ON RECH1.XKUNDENNR=KUNDENST.KUNDENNR " & _
            "GROUP BY kundenst.nachname) order by Gesamt desc"
        's = "select 'Name', 0, 30, 'L', 'N', '', 1, NAME, " & _
        '           "'Gesamt', 31, 45, 'R', 'Y', '#,##0.00', 1, GESAMT" & _
        '           " from (SELECT kundenst.nachname as Name, SUM(rech1.xnetto) AS Gesamt " & _
        '    "FROM RECH1 INNER JOIN KUNDENST ON RECH1.XKUNDENNR=KUNDENST.KUNDENNR " & _
        '    "GROUP BY kundenst.nachname) order by Gesamt desc"
        s = sSQL(ix).printSQL
        Dim da As New Data.OleDb.OleDbDataAdapter(s, cn)
        Try
            da.Fill(ds)
            dlg.dbSet = ds
            dlg.PrintPreview()
            dlg.Dispose()
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message, "Ausdruck nicht vorgesehen")
        Finally
            dlg.Dispose()
        End Try
    End Sub
    Private Sub btnDrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDrucken.Click
        If (bSammelDruck And Not TerminDruck) Then
            DruckenAuftrag()
            Return
        End If
        If (Not bSammelDruck And Not TerminDruck) Then
            'DruckenAuswertung()
            Dim txt As String = ListBox1.SelectedItem.ToString()
            If SetupThePrinting(txt) Then
                Dim MyPrintPreviewDialog As New PrintPreviewDialog()
                MyPrintPreviewDialog.Document = MyPrintDocument
                ' Create a Rectangle object that will be used as the bound of the form.
                'Dim scr As Screen = System.Windows.Forms.Screen.PrimaryScreen
                'MyPrintPreviewDialog.DesktopBounds = scr.WorkingArea
                MyPrintPreviewDialog.WindowState = FormWindowState.Maximized
                MyPrintPreviewDialog.ShowDialog()
            End If
        End If
        If (Not bSammelDruck And TerminDruck) Then
            'DruckenAuswertungSQL(sqlPrint)
            If SetupThePrinting("Terminfälligkeiten") Then
                Dim MyPrintPreviewDialog As New PrintPreviewDialog()
                MyPrintPreviewDialog.Document = MyPrintDocument
                ' Create a Rectangle object that will be used as the bound of the form.
                'Dim scr As Screen = System.Windows.Forms.Screen.PrimaryScreen
                'MyPrintPreviewDialog.DesktopBounds = scr.WorkingArea
                MyPrintPreviewDialog.WindowState = FormWindowState.Maximized
                MyPrintPreviewDialog.ShowDialog()
            End If
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim i As Integer
        i = ListBox1.SelectedIndex
        ReadData(sSQL(i).sSQL)
        Debug.WriteLine("sSQL: " + sSQL(i).sSQL)
        Debug.WriteLine("printSQL: " + sSQL(i).printSQL)
        'If sSQL(i).printSQL = String.Empty Then
        '    btnDrucken.Enabled = False
        'Else
        '    btnDrucken.Enabled = True
        'End If
    End Sub
    Private Sub DruckenAuftrag()
        Dim cn As New OleDb.OleDbConnection
        Dim cmd As New OleDb.OleDbCommand
        Dim rdr As OleDb.OleDbDataReader
        Dim i As Integer
        OpenDBConnection(cn)
        cmd.Connection = cn
        cmd.CommandText = "select XAUFTR_NR from Rech1 where Not gedruckt"
        Dim AuftragsNr As New Collection
        rdr = cmd.ExecuteReader
        If rdr.HasRows Then
            While rdr.Read
                AuftragsNr.Add(rdr.Item("XAUFTR_NR"))
            End While
        Else
            rdr.Close()
            cn.Close()
            MessageBox.Show("Keine Aufträge zu drucken")
            Exit Sub
        End If
        rdr.Close()
        Dim dlg
        If readRegUseRawPrinter() = 1 Then
            dlg = New PrintAuftragRAW 'AuftragDrucken
        Else
            dlg = New PrintAuftrag 'AuftragDrucken
        End If
        dlg.AuftragsNummern.Clear()
        For i = 1 To AuftragsNr.Count
            dlg.AuftragsNummern.Add(CLng(AuftragsNr.Item(i)).ToString())
        Next
        'dlg.bSammelDruck = True
        'dlg.sSelectionString = "{Rech1.gedruckt} = false"
        dlg.DruckTyp = "Rechnung"
        dlg.Text = "Sammeldruck Rechnungen"
        dlg.ShowDialog()
        dlg.Dispose()
        Dim ant
        ant = MessageBox.Show("Sind die Ausdrucke OK und können sie gebucht werden", "Sammeldruck", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        If ant = Windows.Forms.DialogResult.Yes Then
            'OpenDBConnection(cn)
            'cmd.Connection = cn
            cmd.CommandText = "select * from Rech1 where Not Gedruckt"
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    AuftragBuchen(rdr.Item("XAUFTR_NR"), rdr.Item("XTYP"))
                    Debug.Print("Auftrag gebucht: " & rdr.Item("XAUFTR_NR"))
                End While
            Else
                Debug.Print("Keine Aufträge zum Buchen!")
            End If
            rdr.Close()
            cn.Close()
            ReadData("select * from Rech1 where Not Gedruckt")
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

    Private Function SetupThePrinting(ByVal title As String) As Boolean
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


        If Not (MyPrintDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Return False
        End If

        MyPrintDocument.DocumentName = "Auswertungsliste"
        MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings
        MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings
        MyPrintDocument.DefaultPageSettings.Margins = New System.Drawing.Printing.Margins(40, 40, 40, 40)
        MyPrintDocument.DefaultPageSettings.Landscape = True

        MyDataGridViewPrinter = New DataGridViewPrinter(Me.DataGridView1, MyPrintDocument, True, True, title, New Font("Tahoma", 18, FontStyle.Bold, GraphicsUnit.Point), _
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


    Private Sub btnExport2Xls_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport2Xls.Click
        'DataGridView2Excel(DataGridView1)
        DataGridViewExport(DataGridView1)
        'If frmExport.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '    If frmExport.soption = "CSV" Then
        '        DataGridExport2CSV(DataGridView1)
        '    ElseIf frmExport.soption = "XLS" Then
        '        DataGridViewExport2Excel(DataGridView1)
        '    End If
        'End If

    End Sub
End Class