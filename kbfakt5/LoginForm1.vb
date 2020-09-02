Public Class LoginForm1

    ' TODO: Insert code to perform custom authentication using the provided username and password 
    Private Function VerifyPassword(ByVal user As String, ByVal pass As String) As Boolean
        UsernameTextBox.Text = ""
        PasswordTextBox.Text = ""
        If user.Length = 0 Or pass.Length = 0 Then Return False
        Dim i As Integer
        Dim s As String
        'verify user is "kmjrgh"
        s = ""
        For i = 1 To user.Length
            s = s & Chr(Asc(Mid(user, i, 1)) + 3)
        Next
        If Not s = "kmjrgh" Then Return False
        'verify password is "fkrsshu"
        s = ""
        For i = 1 To pass.Length
            s = s & Chr(Asc(Mid(pass, i, 1)) + 3)
        Next
        If Not s = "fkrsshu" Then Return False
        Return True
    End Function
    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        If DebugModus() Then
            DialogResult = Windows.Forms.DialogResult.OK
        Else
            If VerifyPassword(UsernameTextBox.Text, PasswordTextBox.Text) Then DialogResult = Windows.Forms.DialogResult.OK Else DialogResult = Windows.Forms.DialogResult.Cancel
        End If
        Me.Close()
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub LoginForm1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If System.Diagnostics.Debugger.IsAttached Then
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub LoginForm1_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        UsernameTextBox.Focus()
    End Sub
End Class
