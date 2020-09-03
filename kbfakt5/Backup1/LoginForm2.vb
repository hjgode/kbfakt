Imports System.Configuration
Imports Microsoft.Win32

Public Class LoginForm2

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        'If PasswordEncrypt(PasswordTextBox.Text) = My.Settings.Password Then
        If DebugModus() Then
            System.Diagnostics.Debug.WriteLine(PasswordDecrypt(KBirschelPasswort))
        End If
        If PasswordEncrypt(PasswordTextBox.Text) = KBirschelPasswort Then
            DialogResult = Windows.Forms.DialogResult.OK
            PasswordTextBox.Text = ""
            Me.Close()
        Else
            MessageBox.Show("Falsches Passwort!", "Passwort erforderlich", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            PasswordTextBox.Text = ""
        End If
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        PasswordTextBox.Text = ""
        Me.Close()
    End Sub
    Private KBirschelPasswort As String
    Private Sub ReadReg()
        Const userRoot As String = "HKEY_CURRENT_USER"
        Const subkey As String = "Software\KBFakt"
        Const keyName As String = userRoot & "\" & subkey
        Dim PassClear As String = "KBirschel"
        Dim PassEncrypted As String = PasswordEncrypt(PassClear)
        Try
            PassEncrypted = Registry.GetValue(keyName, "UserPassword", "").ToString
        Catch ex As Exception
            PassEncrypted = PasswordEncrypt(PassClear)
        End Try
        KBirschelPasswort = PassEncrypted
    End Sub
    Private Function WriteReg() As Boolean
        Const userRoot As String = "HKEY_CURRENT_USER"
        Const subkey As String = "Software\KBFakt"
        Const keyName As String = userRoot & "\" & subkey
        Dim PassClear As String = PasswordDecrypt(KBirschelPasswort)
        Dim PassEncrypted As String = KBirschelPasswort
        Try
            Registry.SetValue(keyName, "UserPassword", KBirschelPasswort)
            WriteReg = True
        Catch ex As Exception
            MessageBox.Show("Fehler beim Speichern" + vbCrLf + ex.Message, "WriteReg()")
            WriteReg = False
        End Try
    End Function
    Private Sub LoginForm2_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'KBirschelPasswort = System.Configuration.ConfigurationSettings.AppSettings("Passwort")
        'KBirschelPasswort = My.Settings.Password
        ReadReg()
        'My.Settings.Reload()
        If DebugModus() Then
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
        If KBirschelPasswort = "" Then
            btnChangePassword_Click(sender, e)
        End If
        PasswordTextBox.Focus()
    End Sub

    Private Sub btnChangePassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangePassword.Click
        'If PasswordEncrypt(PasswordTextBox.Text) = My.Settings.Password Or _
        '   My.Settings.Password="" then
        If PasswordEncrypt(PasswordTextBox.Text) = KBirschelPasswort Or _
            KBirschelPasswort = "" Then
            'My.Settings.Password = "" Then
            Dim dlg1 As NeuesPasswort
            dlg1 = New NeuesPasswort
            Dim res1 As Integer
            res1 = dlg1.ShowDialog()
            If res1 = Windows.Forms.DialogResult.OK Then
                'My.Settings.Password = PasswordEncrypt(dlg1.PasswortTextBox1.Text)
                KBirschelPasswort = PasswordEncrypt(dlg1.PasswortTextBox1.Text)
                'My.Settings.Save()
                If WriteReg() Then
                    MessageBox.Show("Passwort erfolgreich geändert.", "Passwortänderung")
                Else
                    MessageBox.Show("Passwort konnte nicht gespeichert werden.", "Passwortänderung")
                End If
                MessageBox.Show("Passwort erfolgreich geändert.", "Passwortänderung")
            End If
            dlg1.Dispose()
            PasswordTextBox.Text = ""
        Else
            MessageBox.Show("Falsches altes Passwort!", "Passwort erforderlich", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            PasswordTextBox.Text = ""
        End If
        PasswordTextBox.Focus()
    End Sub

    Private Sub LogoPictureBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogoPictureBox.Click
        If DebugModus() Then MessageBox.Show(PasswordEncrypt(PasswordTextBox.Text))
    End Sub
End Class
