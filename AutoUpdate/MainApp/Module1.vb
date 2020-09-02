Module Module1
    Public g_login As String
    Public isUpdate As Boolean = False

    Sub Main()
        ' Get the parameters sent by the application
        Dim param() As String = Split(Microsoft.VisualBasic.Command(), "|")
        If param.Length > 1 Then isUpdate = True

        Dim frmLogin As New Form1
        frmLogin.ShowDialog()
    End Sub
End Module
