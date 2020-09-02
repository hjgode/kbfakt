Public Class Form1
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtUSER_ID As System.Windows.Forms.TextBox
    Friend WithEvents txtPASS As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents txtSERVER_URL As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtUSER_ID = New System.Windows.Forms.TextBox
        Me.txtPASS = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtSERVER_URL = New System.Windows.Forms.TextBox
        Me.btnLogin = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(29, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "User ID:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Password:"
        '
        'txtUSER_ID
        '
        Me.txtUSER_ID.Location = New System.Drawing.Point(82, 22)
        Me.txtUSER_ID.Name = "txtUSER_ID"
        Me.txtUSER_ID.Size = New System.Drawing.Size(144, 21)
        Me.txtUSER_ID.TabIndex = 2
        Me.txtUSER_ID.Text = ""
        '
        'txtPASS
        '
        Me.txtPASS.Location = New System.Drawing.Point(82, 50)
        Me.txtPASS.Name = "txtPASS"
        Me.txtPASS.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.txtPASS.Size = New System.Drawing.Size(144, 21)
        Me.txtPASS.TabIndex = 3
        Me.txtPASS.Text = ""
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(21, 130)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 17)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Server URL:"
        '
        'txtSERVER_URL
        '
        Me.txtSERVER_URL.Location = New System.Drawing.Point(82, 128)
        Me.txtSERVER_URL.Name = "txtSERVER_URL"
        Me.txtSERVER_URL.Size = New System.Drawing.Size(144, 21)
        Me.txtSERVER_URL.TabIndex = 5
        Me.txtSERVER_URL.Text = "http://localhost/MainApp/"
        '
        'btnLogin
        '
        Me.btnLogin.Location = New System.Drawing.Point(84, 84)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(140, 20)
        Me.btnLogin.TabIndex = 6
        Me.btnLogin.Text = "OK"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(292, 179)
        Me.Controls.Add(Me.btnLogin)
        Me.Controls.Add(Me.txtSERVER_URL)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtPASS)
        Me.Controls.Add(Me.txtUSER_ID)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "Login"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Dim MainForm As New Form2
        If txtUSER_ID.Text.Trim = "" Then
            MsgBox("Please enter your User ID!", MsgBoxStyle.Information)
            txtUSER_ID.Focus()
            Exit Sub
        ElseIf txtPASS.Text.Trim = "" Then
            MsgBox("Please enter your Password!", MsgBoxStyle.Information)
            txtPASS.Focus()
            Exit Sub
        ElseIf txtSERVER_URL.Text.Trim = "" Then
            MsgBox("Please enter Server URL!", MsgBoxStyle.Information)
            txtSERVER_URL.Focus()
            Exit Sub
        End If

        'Check is valid login
        If txtUSER_ID.Text.Trim = "User" And txtPASS.Text.Trim = "123" Then
            Me.Hide()
            'Check if first run then call auto update
            If Not isUpdate Then
                Dim startInfo As New ProcessStartInfo(Application.StartupPath & "\" & "AutoUpdate.exe")
                Dim cmdLine As String
                cmdLine = "MainApp.exe|" & txtSERVER_URL.Text.Trim & "|ServerManifest.xml|" & txtUSER_ID.Text.Trim
                startInfo.Arguments = cmdLine
                Process.Start(startInfo)
                Environment.Exit(1)
            Else
                MainForm.ShowDialog()
            End If
        Else
            MsgBox("Invalid Password!", MsgBoxStyle.Information)
            txtPASS.Focus()
            Exit Sub
        End If
    End Sub
End Class
