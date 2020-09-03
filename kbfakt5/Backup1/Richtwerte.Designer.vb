<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Richtwerte
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
        Me.btnDeleteRichtwert = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.btnAddRichtwert = New System.Windows.Forms.Button
        Me.btnSaveRichtwert1 = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtRichtwertNr = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtText = New System.Windows.Forms.TextBox
        Me.dgv1 = New System.Windows.Forms.DataGridView
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lblID1 = New System.Windows.Forms.Label
        Me.dgv2 = New System.Windows.Forms.DataGridView
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.lblID2 = New System.Windows.Forms.Label
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel
        Me.btnSaveFzgTyp = New System.Windows.Forms.Button
        Me.btnDeleteFzgTyp = New System.Windows.Forms.Button
        Me.btnAddFzgTyp = New System.Windows.Forms.Button
        Me.txtMenge = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtPreis = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtTypKlasse = New System.Windows.Forms.TextBox
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.dgv1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.dgv2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnDeleteRichtwert, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnAddRichtwert, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnSaveRichtwert1, 0, 2)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(440, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(175, 93)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'btnDeleteRichtwert
        '
        Me.btnDeleteRichtwert.BackColor = System.Drawing.SystemColors.Control
        Me.btnDeleteRichtwert.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDeleteRichtwert.Location = New System.Drawing.Point(90, 34)
        Me.btnDeleteRichtwert.Name = "btnDeleteRichtwert"
        Me.btnDeleteRichtwert.Size = New System.Drawing.Size(82, 25)
        Me.btnDeleteRichtwert.TabIndex = 3
        Me.btnDeleteRichtwert.Text = "Löschen"
        Me.btnDeleteRichtwert.UseVisualStyleBackColor = False
        '
        'OK_Button
        '
        Me.OK_Button.BackColor = System.Drawing.SystemColors.Control
        Me.OK_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(81, 25)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        Me.OK_Button.UseVisualStyleBackColor = False
        '
        'Cancel_Button
        '
        Me.Cancel_Button.BackColor = System.Drawing.SystemColors.Control
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Cancel_Button.Location = New System.Drawing.Point(90, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(82, 25)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Schliessen"
        Me.Cancel_Button.UseVisualStyleBackColor = False
        '
        'btnAddRichtwert
        '
        Me.btnAddRichtwert.BackColor = System.Drawing.SystemColors.Control
        Me.btnAddRichtwert.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnAddRichtwert.Location = New System.Drawing.Point(3, 34)
        Me.btnAddRichtwert.Name = "btnAddRichtwert"
        Me.btnAddRichtwert.Size = New System.Drawing.Size(81, 25)
        Me.btnAddRichtwert.TabIndex = 2
        Me.btnAddRichtwert.Text = "NEU"
        Me.btnAddRichtwert.UseVisualStyleBackColor = False
        '
        'btnSaveRichtwert1
        '
        Me.btnSaveRichtwert1.BackColor = System.Drawing.SystemColors.Control
        Me.btnSaveRichtwert1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnSaveRichtwert1.Location = New System.Drawing.Point(3, 65)
        Me.btnSaveRichtwert1.Name = "btnSaveRichtwert1"
        Me.btnSaveRichtwert1.Size = New System.Drawing.Size(81, 25)
        Me.btnSaveRichtwert1.TabIndex = 4
        Me.btnSaveRichtwert1.Text = "Speichern"
        Me.btnSaveRichtwert1.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(0, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Richtwert-Nr.:"
        '
        'txtRichtwertNr
        '
        Me.txtRichtwertNr.Location = New System.Drawing.Point(3, 29)
        Me.txtRichtwertNr.Name = "txtRichtwertNr"
        Me.txtRichtwertNr.ReadOnly = True
        Me.txtRichtwertNr.Size = New System.Drawing.Size(114, 20)
        Me.txtRichtwertNr.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(118, 10)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(31, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Text:"
        '
        'txtText
        '
        Me.txtText.Location = New System.Drawing.Point(121, 29)
        Me.txtText.Name = "txtText"
        Me.txtText.Size = New System.Drawing.Size(313, 20)
        Me.txtText.TabIndex = 2
        '
        'dgv1
        '
        Me.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgv1.Location = New System.Drawing.Point(3, 143)
        Me.dgv1.Name = "dgv1"
        Me.dgv1.Size = New System.Drawing.Size(626, 143)
        Me.dgv1.TabIndex = 3
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.dgv1, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.dgv2, 0, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.Panel2, 0, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.Label7, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label8, 0, 3)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 6
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(632, 579)
        Me.TableLayoutPanel2.TabIndex = 4
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.lblID1)
        Me.Panel1.Controls.Add(Me.TableLayoutPanel1)
        Me.Panel1.Controls.Add(Me.txtText)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.txtRichtwertNr)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(626, 114)
        Me.Panel1.TabIndex = 5
        '
        'lblID1
        '
        Me.lblID1.AutoSize = True
        Me.lblID1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblID1.Location = New System.Drawing.Point(395, 10)
        Me.lblID1.Name = "lblID1"
        Me.lblID1.Size = New System.Drawing.Size(15, 13)
        Me.lblID1.TabIndex = 3
        Me.lblID1.Text = "id"
        '
        'dgv2
        '
        Me.dgv2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgv2.Location = New System.Drawing.Point(3, 432)
        Me.dgv2.Name = "dgv2"
        Me.dgv2.Size = New System.Drawing.Size(626, 144)
        Me.dgv2.TabIndex = 6
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.lblID2)
        Me.Panel2.Controls.Add(Me.TableLayoutPanel4)
        Me.Panel2.Controls.Add(Me.txtMenge)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.ComboBox1)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.txtPreis)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.txtTypKlasse)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 312)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(626, 114)
        Me.Panel2.TabIndex = 7
        '
        'lblID2
        '
        Me.lblID2.AutoSize = True
        Me.lblID2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblID2.Location = New System.Drawing.Point(395, 10)
        Me.lblID2.Name = "lblID2"
        Me.lblID2.Size = New System.Drawing.Size(15, 13)
        Me.lblID2.TabIndex = 12
        Me.lblID2.Text = "id"
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel4.ColumnCount = 2
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.btnSaveFzgTyp, 0, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.btnDeleteFzgTyp, 1, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.btnAddFzgTyp, 0, 0)
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(440, 10)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 2
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(175, 62)
        Me.TableLayoutPanel4.TabIndex = 11
        '
        'btnSaveFzgTyp
        '
        Me.btnSaveFzgTyp.BackColor = System.Drawing.SystemColors.Control
        Me.btnSaveFzgTyp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnSaveFzgTyp.Location = New System.Drawing.Point(3, 34)
        Me.btnSaveFzgTyp.Name = "btnSaveFzgTyp"
        Me.btnSaveFzgTyp.Size = New System.Drawing.Size(81, 25)
        Me.btnSaveFzgTyp.TabIndex = 5
        Me.btnSaveFzgTyp.Text = "Speichern"
        Me.btnSaveFzgTyp.UseVisualStyleBackColor = False
        '
        'btnDeleteFzgTyp
        '
        Me.btnDeleteFzgTyp.BackColor = System.Drawing.SystemColors.Control
        Me.btnDeleteFzgTyp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDeleteFzgTyp.Location = New System.Drawing.Point(90, 3)
        Me.btnDeleteFzgTyp.Name = "btnDeleteFzgTyp"
        Me.btnDeleteFzgTyp.Size = New System.Drawing.Size(82, 25)
        Me.btnDeleteFzgTyp.TabIndex = 0
        Me.btnDeleteFzgTyp.Text = "Löschen"
        Me.btnDeleteFzgTyp.UseVisualStyleBackColor = False
        '
        'btnAddFzgTyp
        '
        Me.btnAddFzgTyp.BackColor = System.Drawing.SystemColors.Control
        Me.btnAddFzgTyp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnAddFzgTyp.Location = New System.Drawing.Point(3, 3)
        Me.btnAddFzgTyp.Name = "btnAddFzgTyp"
        Me.btnAddFzgTyp.Size = New System.Drawing.Size(81, 25)
        Me.btnAddFzgTyp.TabIndex = 1
        Me.btnAddFzgTyp.Text = "NEU"
        Me.btnAddFzgTyp.UseVisualStyleBackColor = False
        '
        'txtMenge
        '
        Me.txtMenge.Location = New System.Drawing.Point(7, 76)
        Me.txtMenge.Name = "txtMenge"
        Me.txtMenge.Size = New System.Drawing.Size(114, 20)
        Me.txtMenge.TabIndex = 10
        Me.txtMenge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(4, 57)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(43, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Menge:"
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(127, 28)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(128, 21)
        Me.ComboBox1.TabIndex = 8
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(122, 10)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(32, 13)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "Filter:"
        '
        'txtPreis
        '
        Me.txtPreis.Location = New System.Drawing.Point(127, 76)
        Me.txtPreis.Name = "txtPreis"
        Me.txtPreis.Size = New System.Drawing.Size(114, 20)
        Me.txtPreis.TabIndex = 6
        Me.txtPreis.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(124, 57)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "E-Preis:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Typklasse:"
        '
        'txtTypKlasse
        '
        Me.txtTypKlasse.Location = New System.Drawing.Point(6, 28)
        Me.txtTypKlasse.Name = "txtTypKlasse"
        Me.txtTypKlasse.ReadOnly = True
        Me.txtTypKlasse.Size = New System.Drawing.Size(114, 20)
        Me.txtTypKlasse.TabIndex = 4
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.Button1, 0, 0)
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(200, 100)
        Me.TableLayoutPanel3.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button1.Location = New System.Drawing.Point(3, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(94, 94)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button2.Location = New System.Drawing.Point(90, 3)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(82, 25)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(3, 127)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(626, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Richtwertliste:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(3, 296)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(626, 13)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "Richtwerte nach Fahrzeugtypen:"
        '
        'Richtwerte
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(632, 579)
        Me.ControlBox = False
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(640, 400)
        Me.Name = "Richtwerte"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Richtwerte"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.dgv1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dgv2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtRichtwertNr As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtText As System.Windows.Forms.TextBox
    Friend WithEvents dgv1 As System.Windows.Forms.DataGridView
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnAddRichtwert As System.Windows.Forms.Button
    Friend WithEvents btnDeleteRichtwert As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents dgv2 As System.Windows.Forms.DataGridView
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents txtMenge As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtPreis As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtTypKlasse As System.Windows.Forms.TextBox
    Friend WithEvents lblID1 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnDeleteFzgTyp As System.Windows.Forms.Button
    Friend WithEvents btnAddFzgTyp As System.Windows.Forms.Button
    Friend WithEvents btnSaveRichtwert1 As System.Windows.Forms.Button
    Friend WithEvents lblID2 As System.Windows.Forms.Label
    Friend WithEvents btnSaveFzgTyp As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label

End Class
