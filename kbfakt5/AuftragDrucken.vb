Imports System.Windows.Forms
Imports CrystalDecisions.CrystalReports

Public Class AuftragDrucken
    Public AuftragNummer As Long = 0
    Public sSelectionString As String
    Public bSammelDruck As Boolean
    Public DruckTyp As String = "Rechnung"

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub AuftragDrucken_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Try to find report dir
            Dim curDir As String
            Dim rptFile As String
            Dim sRepFile As String
            curDir = Environment.CurrentDirectory
            If bSammelDruck Then
                sRepFile = "AuftragRechnung.rpt" '"AuftragRechnungSammel.rpt"
            Else
                sRepFile = "AuftragRechnung.rpt"
            End If

            If System.IO.File.Exists(curDir + "\" + sRepFile) Then
                rptFile = curDir + "\" + sRepFile
            Else
                rptFile = curDir + "\..\" + sRepFile
                If Not System.IO.File.Exists(rptFile) Then
                    rptFile = curDir + "\..\..\" + sRepFile
                End If
            End If

            'Me.CrystalReportViewer1.ReportSource = rptFile

            'Crystal Report's report document object
            Dim objReport As New _
                        CrystalDecisions.CrystalReports.Engine.ReportDocument

            objReport.Load(sRepFile, CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault)
            objReport.FileName = rptFile
            If initDSRech1Rech2() Then
                objReport.SetDataSource(dsRech1Rech2)
                Debug.Print("Report Database= " & objReport.Database.ToString)
            Else
                MessageBox.Show("initDsRech1Rech2 failed", "Report drucken")
            End If

            ''Print
            '//Get the Copy times
            'Int(nCopy = Me.printDocument1.PrinterSettings.Copies)
            '//Get the number of Start Page
            'int sPage = this.printDocument1.PrinterSettings.FromPage;
            '//Get the number of End Page
            'int ePage = this.printDocument1.PrinterSettings.ToPage;
            '//Get the printer name
            'string PrinterName = this.printDocument1.PrinterSettings.PrinterName;

            objReport.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4

            If DruckTyp = "Rechnung" Then
                objReport.DataDefinition.FormulaFields("Aufdruck").Text = """Rechnung"""
            ElseIf DruckTyp = "Auftrag" Then
                objReport.DataDefinition.FormulaFields("Aufdruck").Text = """Auftrag"""
            ElseIf DruckTyp = "Kostenvoranschlag" Then
                objReport.DataDefinition.FormulaFields("Aufdruck").Text = """Kostenvoranschlag"""
            ElseIf DruckTyp = "Lieferschein" Then
                objReport.DataDefinition.FormulaFields("Aufdruck").Text = """Lieferschein"""
            End If
            If AuftragNummer <> 0 Then
                objReport.RecordSelectionFormula = "{RECH1.XAUFTR_NR} = " + AuftragNummer.ToString '1005844.00"
            Else
                objReport.RecordSelectionFormula = "{Rech1.gedruckt} = false" 'print Sammelrechnung
            End If

            With Me.CrystalReportViewer1
                .ReportSource = objReport
            End With

            Me.CrystalReportViewer1.RefreshReport()
            'Me.CrystalReportViewer1.PrintReport()
        Catch ex As CrystalDecisions.CrystalReports.Engine.DataSourceException
            MessageBox.Show(ex.Message, "DataSourceException")
        Catch ex1 As CrystalDecisions.CrystalReports.Engine.EngineException
            MessageBox.Show(ex1.Message, "EngineException")
        Catch sx As System.Exception
            MessageBox.Show(sx.Message, "SystemException")
        Finally
            ShowWaitCursor(False)
        End Try

    End Sub

    Private Sub OK_Button_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        CrystalReportViewer1.PrintReport()
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
        'Dim ant As DialogResult
        'ant = MessageBox.Show("Ausdruck OK?", "Auftrag drucken", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        'If ant = 1 Then 'DialogResult.OK Then
        '    If AuftragBuchen(AuftragNummer) = 0 Then
        '        MessageBox.Show("Fehler beim Buchen der Aufträge")
        '    End If
        '    Me.Close()
        'End If
    End Sub
End Class
