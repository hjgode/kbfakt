Private Declare Function SQLConfigDataSource Lib "ODBCCP32.DLL" (ByVal hwndParent As Integer, ByVal fRequest As Integer, ByVal lpszDriver As String, ByVal lpszAttributes As String) As Integer

Private Sub Command1_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command1.Click
       If TextBox1.Text = "[Server Name]" Or TextBox1.Text = "" Then
           MsgBox("Inserte Server Name.", MsgBoxStyle.Information)
           Exit Sub
       End If
       If TextBox2.Text = "[Database Name]" Or TextBox2.Text = "" Then
           MsgBox("Insert Database Name.", MsgBoxStyle.Information)
           Exit Sub
       End If

       Dim iReturn As Integer
       Dim Attr As String
       Dim machName As String = Trim(TextBox1.Text)          'Server Name.
       Dim catelog As String = Trim(TextBox2.Text)

       Attr = "SERVER=" & machName & "" & Chr(0)
       Attr = Attr & "DSN=" & catelog & Chr(0)
       Attr = Attr & "DESCRIPTION=DSN For HOSPITAL" & Chr(0)
       Attr = Attr & "DATABASE=HOSPITAL" & Chr(0)
       Attr = Attr & "TRUSTED_CONNECTION=YES" & Chr(0)
       iReturn = SQLConfigDataSource(0, 1, "SQL Server", Attr)
       If iReturn Then
           MsgBox("DSN setup complete successfully.", MsgBoxStyle.OKOnly + MsgBoxStyle.Information)
       Else
           MsgBox("DSN setup can not complete successfully.")
       End If

End Sub
