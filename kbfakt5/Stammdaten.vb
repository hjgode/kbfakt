Imports System.Windows.Forms
Imports System.Data.OleDb

Public Class Stammdaten
    Private m_isChanged As Boolean = False
    Private Sub SaveData()
        If m_isChanged Then
            If MessageBox.Show("Geänderte Daten speichern?", "FirmStam", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                WriteDaten()
            End If
        End If
    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If CheckData() Then
            SaveData()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
    Private Function CheckData() As Boolean
        'check line length in FussText
        Dim s() As String
        Dim count As Integer = 0
        Dim i As Integer
        For i = 0 To txtFussText.Text.Length - 1
            If txtFussText.Text.Substring(i, 1) = vbCrLf Then count += 1
        Next
        If count > 3 Then
            MessageBox.Show("Zuviele Zeilen im Fusstext. Maximum ist 3!")
            Return False
        End If
        s = Split(txtFussText.Text, vbCrLf, 3)
        For i = 0 To s.Length - 1
            If s(i).Length > 80 Then
                MessageBox.Show("Zeile " & i + 1 & " zu lang. Maximal 80 Zeichen!")
                Return False
            End If
        Next
        If Not System.IO.File.Exists(myDbFilename) Then
            txtDBfilename.BackColor = Color.Red
            Return False
        Else
            txtDBfilename.BackColor = Color.Gray
            DBfileName = myDbFilename
            InitDBfilename()
        End If
        Return True
    End Function
    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        SaveData()
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub WriteDaten()
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim dbCmd As New OleDbCommand
        dbCmd.Connection = cn
        Dim queryString As String
        queryString = "update firmstam set FA_ANREDE='" & txtFA_Anrede.Text & "', " & _
                     "NAME1='" & txtName1.Text & "', " & _
                     "NAME2='" & txtName2.Text & "'," & _
                     "STRASSE='" & txtStrasse.Text & "'," & _
                     "PLZ='" & txtPLZ.Text & "'," & _
                     "ORT='" & txtOrt.Text & "'," & _
                     "MWST_SATZ1=" & CDoubleS(txtMwSt_Satz1.Text) & " ," & _
                     "AltTeilMwSt=" & CDoubleS(txtAltTeileMwSt.Text) & " ," & _
                     "STEUERNR='" & txtSteuerNr.Text & "', " & _
                     "FUSSTEXT='" & txtFussText.Text & "' where id=" & CLong(txtId.Text)
        dbCmd.CommandText = queryString
        dbCmd.CommandType = CommandType.Text
        Dim anz = dbCmd.ExecuteNonQuery()
        If anz = 0 Then
            MessageBox.Show("Fehler beim Update", "FirmStam")
        Else
            m_isChanged = False
        End If
        DBfileName = myDbFilename
        cn.Close()
        ReadDaten()
    End Sub
    Private Sub ReadDaten()
        Dim cn As New OleDb.OleDbConnection
        OpenDBConnection(cn)
        Dim dbCmd As New OleDbCommand
        Dim rdr As OleDbDataReader
        dbCmd.Connection = cn
        Dim queryString As String
        queryString = "Select id, FA_ANREDE, NAME1, NAME2, STRASSE, PLZ, ORT, MWST_SATZ1, AltteilMwSt, STEUERNR, FUSSTEXT from firmstam"
        dbCmd.CommandText = queryString
        rdr = dbCmd.ExecuteReader()
        If rdr.HasRows Then
            rdr.Read() 'try to read 
            txtId.Text = rdr.Item("id").ToString
            txtFA_Anrede.Text = rdr.Item("FA_Anrede").ToString
            txtName1.Text = rdr.Item("Name1").ToString
            txtName2.Text = rdr.Item("Name2").ToString
            txtStrasse.Text = rdr.Item("Strasse").ToString
            txtPLZ.Text = rdr.Item("PLZ").ToString
            txtOrt.Text = rdr.Item("Ort").ToString
            txtMwSt_Satz1.Text = rdr.Item("MWST_SATZ1").ToString
            txtAltTeileMwSt.Text = rdr.Item("AltteilMwSt").ToString
            txtSteuerNr.Text = rdr.Item("STEUERNR").ToString
            txtFussText.Text = rdr.Item("FUSSTEXT").ToString.Trim()
            txtDBfilename.Text = DBfileName
        End If
        rdr.Close()
        cn.Close()
        'id, FA_ANREDE, NAME1, NAME2, STRASSE, PLZ, ORT, MWST_SATZ1, STEUERNR, FUSSTEXT
    End Sub
    Private Sub Stammdaten_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ReadDaten()
        For Each c As Control In Me.Controls
            If TypeOf c Is TextBox Then
                AddHandler c.TextChanged, AddressOf TextChanged
            End If
        Next
    End Sub
    Private Shadows Sub TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim c As TextBox
        c = CType(sender, TextBox)
        c.BackColor = Color.Yellow
        m_isChanged = True
    End Sub
    Private myDbFilename As String = ""
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectDBfilename.Click
        Dim s As String = DBfileName
        If selectDBfilename(s) = Windows.Forms.DialogResult.OK Then
            myDbFilename = s
            txtDBfilename.Text = myDbFilename
        End If
    End Sub
End Class
