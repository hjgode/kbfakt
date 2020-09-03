<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Auswertungen
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
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.btnClose = New System.Windows.Forms.Button
        Me.ListBox1 = New System.Windows.Forms.ComboBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.txtSumme = New System.Windows.Forms.TextBox
        Me.LabelGesamt = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.btnExport2Xls = New System.Windows.Forms.Button
        Me.btnDrucken = New System.Windows.Forms.Button
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.DataGridView1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnClose, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ListBox1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel2, 1, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(717, 457)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(3, 33)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(612, 390)
        Me.DataGridView1.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnClose.Location = New System.Drawing.Point(621, 3)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(93, 24)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Schliessen"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(3, 3)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(612, 21)
        Me.ListBox1.TabIndex = 3
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txtSumme)
        Me.Panel1.Controls.Add(Me.LabelGesamt)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel1.Location = New System.Drawing.Point(415, 429)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(200, 25)
        Me.Panel1.TabIndex = 7
        '
        'txtSumme
        '
        Me.txtSumme.Enabled = False
        Me.txtSumme.Location = New System.Drawing.Point(93, 3)
        Me.txtSumme.Name = "txtSumme"
        Me.txtSumme.Size = New System.Drawing.Size(107, 20)
        Me.txtSumme.TabIndex = 8
        Me.txtSumme.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelGesamt
        '
        Me.LabelGesamt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelGesamt.Location = New System.Drawing.Point(20, 6)
        Me.LabelGesamt.Name = "LabelGesamt"
        Me.LabelGesamt.Size = New System.Drawing.Size(74, 13)
        Me.LabelGesamt.TabIndex = 7
        Me.LabelGesamt.Text = "Gesamt:"
        Me.LabelGesamt.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnExport2Xls)
        Me.Panel2.Controls.Add(Me.btnDrucken)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(621, 33)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(93, 390)
        Me.Panel2.TabIndex = 8
        '
        'btnExport2Xls
        '
        Me.btnExport2Xls.Location = New System.Drawing.Point(0, 31)
        Me.btnExport2Xls.Name = "btnExport2Xls"
        Me.btnExport2Xls.Size = New System.Drawing.Size(93, 25)
        Me.btnExport2Xls.TabIndex = 2
        Me.btnExport2Xls.Text = "Export"
        Me.btnExport2Xls.UseVisualStyleBackColor = True
        '
        'btnDrucken
        '
        Me.btnDrucken.Location = New System.Drawing.Point(0, 0)
        Me.btnDrucken.Name = "btnDrucken"
        Me.btnDrucken.Size = New System.Drawing.Size(93, 25)
        Me.btnDrucken.TabIndex = 2
        Me.btnDrucken.Text = "Drucken"
        Me.btnDrucken.UseVisualStyleBackColor = True
        '
        'Auswertungen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(717, 457)
        Me.ControlBox = False
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Auswertungen"
        Me.Text = "Auswertungen"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnDrucken As System.Windows.Forms.Button
    Friend WithEvents ListBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents LabelGesamt As System.Windows.Forms.Label
    Friend WithEvents txtSumme As System.Windows.Forms.TextBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnExport2Xls As System.Windows.Forms.Button
End Class
