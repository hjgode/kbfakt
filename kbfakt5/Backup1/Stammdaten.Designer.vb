<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Stammdaten
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.txtId = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.txtFA_Anrede = New System.Windows.Forms.Label
        Me.txtName1 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtName2 = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtStrasse = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtPLZ = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtOrt = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtMwSt_Satz1 = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtSteuerNr = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtFussText = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtAltTeileMwSt = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtDBfilename = New System.Windows.Forms.TextBox
        Me.btnSelectDBfilename = New System.Windows.Forms.Button
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(298, 340)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'txtId
        '
        Me.txtId.Enabled = False
        Me.txtId.Location = New System.Drawing.Point(86, 6)
        Me.txtId.Name = "txtId"
        Me.txtId.Size = New System.Drawing.Size(37, 20)
        Me.txtId.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(19, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Id:"
        '
        'TextBox1
        '
        Me.TextBox1.Enabled = False
        Me.TextBox1.Location = New System.Drawing.Point(86, 32)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(75, 20)
        Me.TextBox1.TabIndex = 1
        '
        'txtFA_Anrede
        '
        Me.txtFA_Anrede.AutoSize = True
        Me.txtFA_Anrede.Location = New System.Drawing.Point(12, 35)
        Me.txtFA_Anrede.Name = "txtFA_Anrede"
        Me.txtFA_Anrede.Size = New System.Drawing.Size(44, 13)
        Me.txtFA_Anrede.TabIndex = 2
        Me.txtFA_Anrede.Text = "Anrede:"
        '
        'txtName1
        '
        Me.txtName1.Location = New System.Drawing.Point(86, 58)
        Me.txtName1.Name = "txtName1"
        Me.txtName1.Size = New System.Drawing.Size(204, 20)
        Me.txtName1.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 61)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Name1:"
        '
        'txtName2
        '
        Me.txtName2.Location = New System.Drawing.Point(86, 84)
        Me.txtName2.Name = "txtName2"
        Me.txtName2.Size = New System.Drawing.Size(204, 20)
        Me.txtName2.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 87)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(44, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Name2:"
        '
        'txtStrasse
        '
        Me.txtStrasse.Location = New System.Drawing.Point(86, 110)
        Me.txtStrasse.Name = "txtStrasse"
        Me.txtStrasse.Size = New System.Drawing.Size(204, 20)
        Me.txtStrasse.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 113)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Strasse:"
        '
        'txtPLZ
        '
        Me.txtPLZ.Location = New System.Drawing.Point(86, 136)
        Me.txtPLZ.Name = "txtPLZ"
        Me.txtPLZ.Size = New System.Drawing.Size(75, 20)
        Me.txtPLZ.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 139)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(30, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "PLZ:"
        '
        'txtOrt
        '
        Me.txtOrt.Location = New System.Drawing.Point(86, 162)
        Me.txtOrt.Name = "txtOrt"
        Me.txtOrt.Size = New System.Drawing.Size(204, 20)
        Me.txtOrt.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 165)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(24, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Ort:"
        '
        'txtMwSt_Satz1
        '
        Me.txtMwSt_Satz1.Location = New System.Drawing.Point(86, 188)
        Me.txtMwSt_Satz1.Name = "txtMwSt_Satz1"
        Me.txtMwSt_Satz1.Size = New System.Drawing.Size(75, 20)
        Me.txtMwSt_Satz1.TabIndex = 1
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 191)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(61, 13)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "MwSt-Satz:"
        '
        'txtSteuerNr
        '
        Me.txtSteuerNr.Location = New System.Drawing.Point(86, 214)
        Me.txtSteuerNr.Name = "txtSteuerNr"
        Me.txtSteuerNr.Size = New System.Drawing.Size(204, 20)
        Me.txtSteuerNr.TabIndex = 1
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(12, 217)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(37, 13)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "UstID:"
        '
        'txtFussText
        '
        Me.txtFussText.AcceptsReturn = True
        Me.txtFussText.AcceptsTab = True
        Me.txtFussText.Location = New System.Drawing.Point(86, 240)
        Me.txtFussText.Multiline = True
        Me.txtFussText.Name = "txtFussText"
        Me.txtFussText.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtFussText.Size = New System.Drawing.Size(355, 55)
        Me.txtFussText.TabIndex = 3
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 240)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(49, 13)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Fusstext:"
        '
        'txtAltTeileMwSt
        '
        Me.txtAltTeileMwSt.Location = New System.Drawing.Point(296, 188)
        Me.txtAltTeileMwSt.Name = "txtAltTeileMwSt"
        Me.txtAltTeileMwSt.Size = New System.Drawing.Size(75, 20)
        Me.txtAltTeileMwSt.TabIndex = 1
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(195, 191)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(95, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "Altteile MwSt-Satz:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(12, 301)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(63, 13)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Datenbank:"
        '
        'txtDBfilename
        '
        Me.txtDBfilename.Location = New System.Drawing.Point(86, 298)
        Me.txtDBfilename.Name = "txtDBfilename"
        Me.txtDBfilename.ReadOnly = True
        Me.txtDBfilename.Size = New System.Drawing.Size(313, 20)
        Me.txtDBfilename.TabIndex = 1
        '
        'btnSelectDBfilename
        '
        Me.btnSelectDBfilename.Location = New System.Drawing.Point(405, 298)
        Me.btnSelectDBfilename.Name = "btnSelectDBfilename"
        Me.btnSelectDBfilename.Size = New System.Drawing.Size(36, 20)
        Me.btnSelectDBfilename.TabIndex = 4
        Me.btnSelectDBfilename.Text = "..."
        Me.btnSelectDBfilename.UseVisualStyleBackColor = True
        '
        'Stammdaten
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(456, 381)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnSelectDBfilename)
        Me.Controls.Add(Me.txtFussText)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtFA_Anrede)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtOrt)
        Me.Controls.Add(Me.txtStrasse)
        Me.Controls.Add(Me.txtName2)
        Me.Controls.Add(Me.txtName1)
        Me.Controls.Add(Me.txtPLZ)
        Me.Controls.Add(Me.txtDBfilename)
        Me.Controls.Add(Me.txtSteuerNr)
        Me.Controls.Add(Me.txtAltTeileMwSt)
        Me.Controls.Add(Me.txtMwSt_Satz1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.txtId)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Stammdaten"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Stammdaten"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents txtId As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents txtFA_Anrede As System.Windows.Forms.Label
    Friend WithEvents txtName1 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtName2 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtStrasse As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtPLZ As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtOrt As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtMwSt_Satz1 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtSteuerNr As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtFussText As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtAltTeileMwSt As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtDBfilename As System.Windows.Forms.TextBox
    Friend WithEvents btnSelectDBfilename As System.Windows.Forms.Button

End Class
