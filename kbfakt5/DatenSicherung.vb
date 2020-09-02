Option Explicit On
Imports Microsoft.Win32
Imports System.Windows.Forms
Imports System.IO

Public Class DatenSicherung
    Private m_BackupPath As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        WriteReg()
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub WriteReg()
        Const userRoot As String = "HKEY_CURRENT_USER"
        Const subkey As String = "Software\KBFakt"
        Const keyName As String = userRoot & "\" & subkey
        Try
            Registry.SetValue(keyName, "LastBackupPath", m_BackupPath)
        Catch x As Exception
            MessageBox.Show("Exception in WriteReg()" + vbCrLf + x.Message, "Fehler")
        End Try

    End Sub
    Private Sub ReadReg()
        Const userRoot As String = "HKEY_CURRENT_USER"
        Const subkey As String = "Software\KBFakt"
        Const keyName As String = userRoot & "\" & subkey
        Dim BackupPath As String
        Try
            BackupPath = Registry.GetValue(keyName, "LastBackupPath", DBfileName)
            m_BackupPath = BackupPath
            txtFolder.Text = m_BackupPath
            RefreshList(m_BackupPath)
        Catch x As Exception
            System.Diagnostics.Debug.WriteLine("Exception: " + x.Message, "ReadReg()")
            BackupPath = System.IO.Path.GetDirectoryName(GetDbFileName())
            m_BackupPath = BackupPath
        End Try
    End Sub
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Dim ant As Integer
        With FolderBrowserDialog1
            '.RootFolder = 
            .SelectedPath = txtFolder.Text
            .ShowNewFolderButton = False
            ant = .ShowDialog()
        End With
        If ant = Windows.Forms.DialogResult.OK Then
            txtFolder.Text = FolderBrowserDialog1.SelectedPath
            m_BackupPath = txtFolder.Text
            WriteReg()
        End If
    End Sub

    Private Sub DatenSicherung_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ReadReg()
    End Sub
    Private Sub RefreshList(ByVal pfad As String)

        lstDateien.Items.Clear()
        Dim di As New DirectoryInfo(pfad)
        ' Create an array representing the files in the current directory.
        Dim fi As FileInfo() = di.GetFiles("kbfakt5*.mdb")
        ' Print out the names of the files in the current directory.
        Dim fiTemp As FileInfo
        For Each fiTemp In fi
            lstDateien.Items.Add(fiTemp.Name)
        Next fiTemp
    End Sub
    Private Sub txtFolder_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFolder.TextChanged
        m_BackupPath = txtFolder.Text
        RefreshList(m_BackupPath)
    End Sub
    Private Function BackupDB(ByVal pfad As String) As Boolean
        Dim dts As String
        Dim ret As Boolean = False
        If Not pfad.EndsWith("\") Then pfad = pfad + "\"
        dts = String.Format("kbfakt5_{0:dd.MM.yyyy_HHmmss}.mdb", DateTime.Now)
        If MessageBox.Show("Aktuelle Daten in die Datei " + vbCrLf + pfad + dts + vbCrLf + "sichern?", "Datensicherung", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
            Try
                FileCopy(DBfileName, pfad + "\" + dts)
                If (DBdeletePassword(pfad + "\" + dts)) Then
                    MessageBox.Show("Datensicherung in die Datei " + vbCrLf + pfad + dts + vbCrLf + "war erfolgreich")
                    RefreshList(pfad)
                    ret = True
                Else
                    MessageBox.Show("Datensicherung in die Datei " + vbCrLf + pfad + dts + vbCrLf + "war nicht erfolgreich (Password)")
                End If
            Catch ax As ArgumentException
                MessageBox.Show("Exception: " + ax.Message, "btnBackup_Click()")
            Catch fx As FileNotFoundException
                MessageBox.Show("Exception: " + fx.Message, "btnBackup_Click()")
            Catch io As IOException
                MessageBox.Show("Exception: " + io.Message, "btnBackup_Click()")
            End Try
        End If
        Return ret
    End Function
    Private Sub btnBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackup.Click
        BackupDB(txtFolder.Text)
    End Sub

    Private Sub lstDateien_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstDateien.Click
        If lstDateien.SelectedIndex = -1 Then Return
        Dim di As New DirectoryInfo(m_BackupPath)
        ' Create an array representing the files in the current directory.
        Dim fi As FileInfo() = di.GetFiles(lstDateien.SelectedItem)
        Dim fiTemp As FileInfo
        For Each fiTemp In fi
            txtSize.Text = fiTemp.Length
        Next fiTemp
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If lstDateien.SelectedIndex = -1 Then Return
        If lstDateien.SelectedItem = "kbfakt5.mdb" Then
            MessageBox.Show("Diese Datei können Sie hiermit nicht löschen!", "Datenbank löschen")
            Return
        End If
        Dim deletefile As String
        Dim pfad As String
        pfad = txtFolder.Text
        If Not pfad.EndsWith("\") Then pfad = pfad + "\"
        deletefile = pfad + lstDateien.SelectedItem
        If MessageBox.Show("Wollen Sie die Datei" + vbCrLf + deletefile + vbCrLf + "wirklich löschen?", "Sicherung löschen", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Try
                Kill(deletefile)
                RefreshList(txtFolder.Text)
            Catch fx As FileNotFoundException
                MessageBox.Show("Exception: " + fx.Message, "btnDelete_Click()")
            Catch ix As IOException
                MessageBox.Show("Exception: " + ix.Message, "btnDelete_Click()")
            Catch sx As System.Security.SecurityException
                MessageBox.Show("Exception: " + sx.Message, "btnDelete_Click()")
            End Try

        End If
    End Sub

    Private Sub btnRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestore.Click
        If lstDateien.SelectedIndex = -1 Then Return
        If lstDateien.SelectedItem = "kbfakt5.mdb" Then
            MessageBox.Show("Diese Datei können Sie hiermit nicht wiederherstellen!", "Datenbank wiederherstellen")
            Return
        End If
        Dim restorefile As String
        Dim pfad As String
        pfad = txtFolder.Text
        If Not pfad.EndsWith("\") Then pfad = pfad + "\"
        restorefile = pfad + lstDateien.SelectedItem
        If MessageBox.Show("Sind Sie wirklich sicher?", "Daten wiederherstellen", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Try
                If MessageBox.Show("Aktuelle Daten aus der Datei " + vbCrLf + restorefile + vbCrLf + "wiederherstellen?", "Datensicherung wiederherstellen", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                    FileCopy(restorefile, DBfileName)
                    If (PatchPassword()) Then
                        MessageBox.Show("Datenwiederherstellung aus der Datei " + vbCrLf + restorefile + vbCrLf + "wahr erfolgreich")
                    Else
                        MessageBox.Show("Datenwiederherstellung aus der Datei " + vbCrLf + restorefile + vbCrLf + "wahr NICHT erfolgreich (Password)")
                    End If
                    RefreshList(pfad)
                End If
            Catch ax As ArgumentException
                MessageBox.Show("Exception: " + ax.Message, "btnRestore_Click()")
            Catch fx As FileNotFoundException
                MessageBox.Show("Exception: " + fx.Message, "btnRestore_Click()")
            Catch io As IOException
                MessageBox.Show("Exception: " + io.Message, "btnRestore_Click()")
            End Try
        End If

    End Sub
    Private Function kompressDB() As Boolean
        Try
            Return mainmodul.CompactDB(DBfileName)
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message, "btnCompact_Click()")
            Return False
        End Try

    End Function
    Private Sub btnCompact_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompact.Click
        If MessageBox.Show("Haben Sie eine Sicherung der Datenbank?", "Datenbank komprimieren", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            kompressDB()
        Else
            MessageBox.Show("Fertigen Sie erst eine Sicherung an...", "Datenbank komprimieren")
            If BackupDB(txtFolder.Text) Then
                kompressDB()
            End If
        End If
    End Sub
End Class
