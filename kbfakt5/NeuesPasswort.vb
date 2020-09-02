Public Class NeuesPasswort

    ' TODO: Insert code to perform custom authentication using the provided username and password 
    ' (See http://go.microsoft.com/fwlink/?LinkId=35339).  
    ' The custom principal can then be attached to the current thread's principal as follows: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' where CustomPrincipal is the IPrincipal implementation used to perform authentication. 
    ' Subsequently, My.User will return identity information encapsulated in the CustomPrincipal object
    ' such as the username, display name, etc.

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        If PasswortTextBox1.Text = PasswordTextBox2.Text And PasswortTextBox1.Text.Length > 5 Then
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
        If PasswortTextBox1.Text <> PasswordTextBox2.Text Then
            MessageBox.Show("Eingaben stimmen nicht überein. Bitte wiederholen.", "Eingabefehler", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            PasswortTextBox1.Text = ""
            PasswordTextBox2.Text = ""
            PasswortTextBox1.Focus()
            Exit Sub
        End If
        If PasswortTextBox1.Text.Length < 6 Then
            MessageBox.Show("Passwort zu kurz. Bitte korrigieren.")
            PasswortTextBox1.Text = ""
            PasswordTextBox2.Text = ""
            PasswortTextBox1.Focus()
        End If
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
