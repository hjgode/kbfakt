Imports Microsoft.Win32
Public Class frmDSN
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "
#End Region

    Private Sub CreateDSN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateDSN.Click
        '"C:\Winnt\System32\sqlsrv32.dll"    ' Sql Server Path
        SetDSN(Trim(txtDBName.Text), Trim(txtDSNName.Text), Trim(txtDSNDesc.Text), Trim(txtDriverName.Text), Trim(txtDriverPath.Text), Trim(txtLastUser.Text), Trim(txtServerName.Text), "")
    End Sub

    Private Function SetDSN(ByVal DB_Name As String, _
                            ByVal DSN As String, _
                            ByVal Description As String, _
                            ByVal Driver_Name As String, _
                            ByVal Driver_Path As String, _
                            ByVal Last_User As String, _
                            ByVal Server_Name As String, _
                            ByRef Status As String _
                            ) As Boolean

        Dim lResult As Long
        Dim hKeyHandle As Long
        Dim msg1 As String

        Dim regHandle As RegistryKey            ' Stores the Handle to Registry in which values need to be set

        Dim reg As RegistryKey = Registry.LocalMachine
        Dim conRegKey1 As String = "SOFTWARE\ODBC\ODBC.INI\" & DSN
        Dim conRegKey2 As String = "SOFTWARE\ODBC\ODBC.INI\ODBC Data Sources"

        Try
            regHandle = reg.CreateSubKey(conRegKey1)
            regHandle.SetValue("Database", DB_Name)
            regHandle.SetValue("Description", Description)
            regHandle.SetValue("Driver", Driver_Path)
            regHandle.SetValue("LastUser", Last_User)
            regHandle.SetValue("Server", Server_Name)
            regHandle.Close()
            reg.Close()

            regHandle = reg.CreateSubKey(conRegKey2)
            regHandle.SetValue(DSN, Driver_Name)
            regHandle.Close()
            reg.Close()
            MsgBox("Successfully created the System DSN." & vbCrLf & "You can view the created DSN by clicking on Get DSN button.", MsgBoxStyle.Information, "Create System DSN")
        Catch err As Exception
            MsgBox(err.Message)
        End Try
    End Function

    Private Sub GetDSN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetDSN.Click
        Call Shell("rundll32.exe shell32.dll,Control_RunDLL ODBCCP32.cpl @2, 5")
    End Sub

    
End Class
