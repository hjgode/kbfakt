<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrintAuftrag
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
        Me.PrintPreviewControl2 = New System.Windows.Forms.PrintPreviewControl
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog
        Me.btnDrucken = New System.Windows.Forms.Button
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lblPaperList = New System.Windows.Forms.Label
        Me.lblPrinterList = New System.Windows.Forms.Label
        Me.comboPapier = New System.Windows.Forms.ComboBox
        Me.comboPrinter = New System.Windows.Forms.ComboBox
        Me.btnRAWSettings = New System.Windows.Forms.Button
        Me.btnPageSetup = New System.Windows.Forms.Button
        Me.lblPageNumber = New System.Windows.Forms.Label
        Me.btnPageMinus = New System.Windows.Forms.Button
        Me.btnPagePlus = New System.Windows.Forms.Button
        Me.btn_Close = New System.Windows.Forms.Button
        Me.PageSetupDialog1 = New System.Windows.Forms.PageSetupDialog
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PrintPreviewControl2
        '
        Me.PrintPreviewControl2.AutoZoom = False
        Me.PrintPreviewControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PrintPreviewControl2.Document = Me.PrintDocument1
        Me.PrintPreviewControl2.Location = New System.Drawing.Point(3, 65)
        Me.PrintPreviewControl2.Name = "PrintPreviewControl2"
        Me.PrintPreviewControl2.Size = New System.Drawing.Size(673, 406)
        Me.PrintPreviewControl2.TabIndex = 1
        Me.PrintPreviewControl2.Zoom = 0.36783575705731392
        '
        'PrintDocument1
        '
        Me.PrintDocument1.DocumentName = "Auftrag"
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'btnDrucken
        '
        Me.btnDrucken.Location = New System.Drawing.Point(580, 3)
        Me.btnDrucken.Name = "btnDrucken"
        Me.btnDrucken.Size = New System.Drawing.Size(90, 26)
        Me.btnDrucken.TabIndex = 2
        Me.btnDrucken.Text = "Drucken"
        Me.btnDrucken.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.PrintPreviewControl2, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(679, 474)
        Me.TableLayoutPanel1.TabIndex = 3
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblPaperList)
        Me.Panel1.Controls.Add(Me.lblPrinterList)
        Me.Panel1.Controls.Add(Me.comboPapier)
        Me.Panel1.Controls.Add(Me.comboPrinter)
        Me.Panel1.Controls.Add(Me.btnRAWSettings)
        Me.Panel1.Controls.Add(Me.btnPageSetup)
        Me.Panel1.Controls.Add(Me.lblPageNumber)
        Me.Panel1.Controls.Add(Me.btnPageMinus)
        Me.Panel1.Controls.Add(Me.btnPagePlus)
        Me.Panel1.Controls.Add(Me.btn_Close)
        Me.Panel1.Controls.Add(Me.btnDrucken)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(673, 56)
        Me.Panel1.TabIndex = 0
        '
        'lblPaperList
        '
        Me.lblPaperList.AutoSize = True
        Me.lblPaperList.Location = New System.Drawing.Point(191, 33)
        Me.lblPaperList.Name = "lblPaperList"
        Me.lblPaperList.Size = New System.Drawing.Size(40, 13)
        Me.lblPaperList.TabIndex = 9
        Me.lblPaperList.Text = "Papier:"
        '
        'lblPrinterList
        '
        Me.lblPrinterList.AutoSize = True
        Me.lblPrinterList.Location = New System.Drawing.Point(191, 10)
        Me.lblPrinterList.Name = "lblPrinterList"
        Me.lblPrinterList.Size = New System.Drawing.Size(48, 13)
        Me.lblPrinterList.TabIndex = 9
        Me.lblPrinterList.Text = "Drucker:"
        '
        'comboPapier
        '
        Me.comboPapier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboPapier.FormattingEnabled = True
        Me.comboPapier.Location = New System.Drawing.Point(245, 30)
        Me.comboPapier.Name = "comboPapier"
        Me.comboPapier.Size = New System.Drawing.Size(233, 21)
        Me.comboPapier.TabIndex = 8
        '
        'comboPrinter
        '
        Me.comboPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboPrinter.FormattingEnabled = True
        Me.comboPrinter.Location = New System.Drawing.Point(245, 7)
        Me.comboPrinter.Name = "comboPrinter"
        Me.comboPrinter.Size = New System.Drawing.Size(233, 21)
        Me.comboPrinter.TabIndex = 8
        '
        'btnRAWSettings
        '
        Me.btnRAWSettings.Location = New System.Drawing.Point(89, 4)
        Me.btnRAWSettings.Name = "btnRAWSettings"
        Me.btnRAWSettings.Size = New System.Drawing.Size(96, 25)
        Me.btnRAWSettings.TabIndex = 7
        Me.btnRAWSettings.Text = "Einstellungen"
        Me.btnRAWSettings.UseVisualStyleBackColor = True
        '
        'btnPageSetup
        '
        Me.btnPageSetup.Location = New System.Drawing.Point(580, 31)
        Me.btnPageSetup.Name = "btnPageSetup"
        Me.btnPageSetup.Size = New System.Drawing.Size(90, 24)
        Me.btnPageSetup.TabIndex = 5
        Me.btnPageSetup.Text = "Seite einrichten"
        Me.btnPageSetup.UseVisualStyleBackColor = True
        '
        'lblPageNumber
        '
        Me.lblPageNumber.Location = New System.Drawing.Point(9, 30)
        Me.lblPageNumber.Name = "lblPageNumber"
        Me.lblPageNumber.Size = New System.Drawing.Size(64, 22)
        Me.lblPageNumber.TabIndex = 4
        Me.lblPageNumber.Text = "Label1"
        Me.lblPageNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnPageMinus
        '
        Me.btnPageMinus.Location = New System.Drawing.Point(9, 4)
        Me.btnPageMinus.Name = "btnPageMinus"
        Me.btnPageMinus.Size = New System.Drawing.Size(29, 25)
        Me.btnPageMinus.TabIndex = 3
        Me.btnPageMinus.Text = "<"
        Me.btnPageMinus.UseVisualStyleBackColor = True
        '
        'btnPagePlus
        '
        Me.btnPagePlus.Location = New System.Drawing.Point(44, 4)
        Me.btnPagePlus.Name = "btnPagePlus"
        Me.btnPagePlus.Size = New System.Drawing.Size(29, 25)
        Me.btnPagePlus.TabIndex = 3
        Me.btnPagePlus.Text = ">"
        Me.btnPagePlus.UseVisualStyleBackColor = True
        '
        'btn_Close
        '
        Me.btn_Close.Location = New System.Drawing.Point(484, 3)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(90, 26)
        Me.btn_Close.TabIndex = 2
        Me.btn_Close.Text = "Schliessen"
        Me.btn_Close.UseVisualStyleBackColor = True
        '
        'PageSetupDialog1
        '
        Me.PageSetupDialog1.AllowOrientation = False
        Me.PageSetupDialog1.Document = Me.PrintDocument1
        Me.PageSetupDialog1.EnableMetric = True
        '
        'PrintAuftrag
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(679, 474)
        Me.ControlBox = False
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(600, 400)
        Me.Name = "PrintAuftrag"
        Me.Text = "PrintAuftrag"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PrintPreviewControl2 As System.Windows.Forms.PrintPreviewControl
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents btnDrucken As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnPageMinus As System.Windows.Forms.Button
    Friend WithEvents btnPagePlus As System.Windows.Forms.Button
    Friend WithEvents lblPageNumber As System.Windows.Forms.Label
    Friend WithEvents btnPageSetup As System.Windows.Forms.Button
    Friend WithEvents PageSetupDialog1 As System.Windows.Forms.PageSetupDialog
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btnRAWSettings As System.Windows.Forms.Button
    Friend WithEvents comboPapier As System.Windows.Forms.ComboBox
    Friend WithEvents comboPrinter As System.Windows.Forms.ComboBox
    Friend WithEvents lblPaperList As System.Windows.Forms.Label
    Friend WithEvents lblPrinterList As System.Windows.Forms.Label
End Class
