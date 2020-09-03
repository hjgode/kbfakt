Public Class OffeneRechnungen
    Public SqlString As String = "Select XAUFTR_NR as Auftrag, XNAME1 as Name1, XNAME2 as Name2, XDATUM as RgDatum, XFGSTLLNR as FahrgestNr, XNETTO as Netto from Rech1 where Bezahlt=FALSE and Gedruckt=TRUE AND Gutschrift=FALSE"
    Public SqlStringSumme As String = "Select Sum(xnetto) from rech1 where Bezahlt=FALSE and Gedruckt=TRUE AND Gutschrift=FALSE"
    Private Sub OffeneRechnungen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        kbfakt_start.FillGrid(DataGridView1, SqlString)
        DataGridView1.ReadOnly = True
        refreshSumme()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click
        Dim d As DateTime
        d = DateTimePicker1.Value
        Dim ds As String = d.ToShortDateString
        Dim anr As Long
        If Not IsNothing(DataGridView1.CurrentCell) Then
            anr = DataGridView1.Item(0, DataGridView1.CurrentCell.RowIndex).Value
        End If
        Dim ant As DialogResult
        ant = MessageBox.Show("Bitte bestätigen Sie:" & vbCrLf & "Rechnung Nr: " & _
            anr & " wurde am " & ds & " bezahlt", "Als Bezahlt markieren", _
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If ant = Windows.Forms.DialogResult.OK Then
            Dim AccessConn As New System.Data.OleDb.OleDbConnection
            OpenDBConnection(AccessConn)
            Dim dt As DateTime = DateTime.Parse(d.Date)
            Dim AccessCommand As New System.Data.OleDb.OleDbCommand
            Dim c As Integer
            AccessCommand.Connection = AccessConn
            Try
                'update rech1 set bezahlt=true, bezdatum='25.06.2007' where xauftr_nr=1005450;
                AccessCommand.CommandText = "UPDATE RECH1 set Bezahlt=TRUE, BezDatum='" & ds & "' where XAUFTR_NR=" & anr
                c = AccessCommand.ExecuteNonQuery()
                Debug.Print(AccessCommand.CommandText & " abgeschlossen")
                MessageBox.Show("Rechnung Nr: " & _
                    anr & " wurde für den " & ds & " als bezahlt gespeichert.", "Offene Rechnungen", MessageBoxButtons.OK, MessageBoxIcon.Information)
                RefreshData()
            Catch ex2 As System.Data.OleDb.OleDbException
                Debug.Print(ex2.Message & " OffeneRechnungen AlsBezahltMarkieren")
                MessageBox.Show(ex2.Message & " ### Fehler", "OffeneRechnungen AlsBezahltMarkieren")
            End Try
            AccessConn.Close()
        End If
    End Sub
    Public Sub RefreshData()
        Try
            DataGridView1.DataSource = Nothing
        Catch
        End Try
        kbfakt_start.FillGrid(DataGridView1, SqlString)
    End Sub
    Public Sub refreshSumme()
        Dim AccessConn As New System.Data.OleDb.OleDbConnection
        OpenDBConnection(AccessConn)
        Dim AccessCommand As New System.Data.OleDb.OleDbCommand
        Dim c
        AccessCommand.Connection = AccessConn
        Try
            'update rech1 set bezahlt=true, bezdatum='25.06.2007' where xauftr_nr=1005450;
            AccessCommand.CommandText = SqlStringSumme
            c = AccessCommand.ExecuteScalar
            Debug.Print(AccessCommand.CommandText & " abgeschlossen")
            txtSumme.Text = c.ToString
        Catch ex2 As System.Data.OleDb.OleDbException
            Debug.Print("Fehler in refreshSumme OffeneRechnungen")
            txtSumme.Text = "###"
        End Try
        AccessConn.Close()
    End Sub
End Class