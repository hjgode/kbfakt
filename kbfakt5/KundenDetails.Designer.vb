<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class KundenDetails
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
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnAbbrechen = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.dgvFahrzeuge = New System.Windows.Forms.DataGridView
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.btnDeleteKunde = New System.Windows.Forms.Button
        Me.btnOffeneRgen = New System.Windows.Forms.Button
        Me.btnSuchen = New System.Windows.Forms.Button
        Me.btnNeu = New System.Windows.Forms.Button
        Me.btn_RechnungenKunde = New System.Windows.Forms.Button
        Me.btnSpeichern = New System.Windows.Forms.Button
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.btnDruckenFahrzeuge = New System.Windows.Forms.Button
        Me.btn_RechnungenKFZ = New System.Windows.Forms.Button
        Me.btnDeleteKfz = New System.Windows.Forms.Button
        Me.btnKfzDetails = New System.Windows.Forms.Button
        Me.btnKfzNeu = New System.Windows.Forms.Button
        Me.DataChangedStatus = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnExport2Csv = New System.Windows.Forms.Button
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.dgvFahrzeuge, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 277.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnOK, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnAbbrechen, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.dgvFahrzeuge, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel2, 2, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel3, 2, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 3)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 5
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(652, 472)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'btnOK
        '
        Me.btnOK.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnOK.Location = New System.Drawing.Point(547, 3)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(102, 20)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "Schliessen"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnAbbrechen
        '
        Me.btnAbbrechen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnAbbrechen.Enabled = False
        Me.btnAbbrechen.Location = New System.Drawing.Point(547, 29)
        Me.btnAbbrechen.Name = "btnAbbrechen"
        Me.btnAbbrechen.Size = New System.Drawing.Size(102, 20)
        Me.btnAbbrechen.TabIndex = 1
        Me.btnAbbrechen.Text = "Abbrechen"
        Me.btnAbbrechen.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.Panel1, 2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 55)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(538, 194)
        Me.Panel1.TabIndex = 2
        '
        'dgvFahrzeuge
        '
        Me.dgvFahrzeuge.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TableLayoutPanel1.SetColumnSpan(Me.dgvFahrzeuge, 2)
        Me.dgvFahrzeuge.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvFahrzeuge.Location = New System.Drawing.Point(3, 275)
        Me.dgvFahrzeuge.Name = "dgvFahrzeuge"
        Me.dgvFahrzeuge.RowHeadersVisible = False
        Me.dgvFahrzeuge.Size = New System.Drawing.Size(538, 194)
        Me.dgvFahrzeuge.TabIndex = 3
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnDeleteKunde)
        Me.Panel2.Controls.Add(Me.btnOffeneRgen)
        Me.Panel2.Controls.Add(Me.btnSuchen)
        Me.Panel2.Controls.Add(Me.btnNeu)
        Me.Panel2.Controls.Add(Me.btn_RechnungenKunde)
        Me.Panel2.Controls.Add(Me.btnSpeichern)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(547, 55)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(102, 194)
        Me.Panel2.TabIndex = 4
        '
        'btnDeleteKunde
        '
        Me.btnDeleteKunde.Location = New System.Drawing.Point(0, 170)
        Me.btnDeleteKunde.Name = "btnDeleteKunde"
        Me.btnDeleteKunde.Size = New System.Drawing.Size(100, 23)
        Me.btnDeleteKunde.TabIndex = 3
        Me.btnDeleteKunde.Text = "Kunde löschen"
        Me.btnDeleteKunde.UseVisualStyleBackColor = True
        '
        'btnOffeneRgen
        '
        Me.btnOffeneRgen.Location = New System.Drawing.Point(1, 108)
        Me.btnOffeneRgen.Name = "btnOffeneRgen"
        Me.btnOffeneRgen.Size = New System.Drawing.Size(101, 37)
        Me.btnOffeneRgen.TabIndex = 2
        Me.btnOffeneRgen.Text = "Offene Rechnungen"
        Me.btnOffeneRgen.UseVisualStyleBackColor = True
        '
        'btnSuchen
        '
        Me.btnSuchen.Location = New System.Drawing.Point(1, 0)
        Me.btnSuchen.Name = "btnSuchen"
        Me.btnSuchen.Size = New System.Drawing.Size(100, 21)
        Me.btnSuchen.TabIndex = 1
        Me.btnSuchen.Text = "Suchen"
        Me.btnSuchen.UseVisualStyleBackColor = True
        '
        'btnNeu
        '
        Me.btnNeu.Location = New System.Drawing.Point(1, 27)
        Me.btnNeu.Name = "btnNeu"
        Me.btnNeu.Size = New System.Drawing.Size(100, 21)
        Me.btnNeu.TabIndex = 0
        Me.btnNeu.Text = "Neu"
        Me.btnNeu.UseVisualStyleBackColor = True
        '
        'btn_RechnungenKunde
        '
        Me.btn_RechnungenKunde.Location = New System.Drawing.Point(1, 81)
        Me.btn_RechnungenKunde.Name = "btn_RechnungenKunde"
        Me.btn_RechnungenKunde.Size = New System.Drawing.Size(100, 21)
        Me.btn_RechnungenKunde.TabIndex = 0
        Me.btn_RechnungenKunde.Text = "Rechnungen"
        Me.btn_RechnungenKunde.UseVisualStyleBackColor = True
        '
        'btnSpeichern
        '
        Me.btnSpeichern.Enabled = False
        Me.btnSpeichern.Location = New System.Drawing.Point(1, 54)
        Me.btnSpeichern.Name = "btnSpeichern"
        Me.btnSpeichern.Size = New System.Drawing.Size(100, 21)
        Me.btnSpeichern.TabIndex = 0
        Me.btnSpeichern.Text = "Speichern"
        Me.btnSpeichern.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.btnExport2Csv)
        Me.Panel3.Controls.Add(Me.btnDruckenFahrzeuge)
        Me.Panel3.Controls.Add(Me.btn_RechnungenKFZ)
        Me.Panel3.Controls.Add(Me.btnDeleteKfz)
        Me.Panel3.Controls.Add(Me.btnKfzDetails)
        Me.Panel3.Controls.Add(Me.btnKfzNeu)
        Me.Panel3.Controls.Add(Me.DataChangedStatus)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(547, 275)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(102, 194)
        Me.Panel3.TabIndex = 6
        '
        'btnDruckenFahrzeuge
        '
        Me.btnDruckenFahrzeuge.Location = New System.Drawing.Point(1, 154)
        Me.btnDruckenFahrzeuge.Name = "btnDruckenFahrzeuge"
        Me.btnDruckenFahrzeuge.Size = New System.Drawing.Size(98, 22)
        Me.btnDruckenFahrzeuge.TabIndex = 8
        Me.btnDruckenFahrzeuge.Text = "Drucken"
        Me.btnDruckenFahrzeuge.UseVisualStyleBackColor = True
        '
        'btn_RechnungenKFZ
        '
        Me.btn_RechnungenKFZ.Location = New System.Drawing.Point(1, 92)
        Me.btn_RechnungenKFZ.Name = "btn_RechnungenKFZ"
        Me.btn_RechnungenKFZ.Size = New System.Drawing.Size(98, 24)
        Me.btn_RechnungenKFZ.TabIndex = 7
        Me.btn_RechnungenKFZ.Text = "Rechnungen"
        Me.btn_RechnungenKFZ.UseVisualStyleBackColor = True
        '
        'btnDeleteKfz
        '
        Me.btnDeleteKfz.Location = New System.Drawing.Point(1, 62)
        Me.btnDeleteKfz.Name = "btnDeleteKfz"
        Me.btnDeleteKfz.Size = New System.Drawing.Size(98, 24)
        Me.btnDeleteKfz.TabIndex = 7
        Me.btnDeleteKfz.Text = "Entfernen"
        Me.btnDeleteKfz.UseVisualStyleBackColor = True
        '
        'btnKfzDetails
        '
        Me.btnKfzDetails.Location = New System.Drawing.Point(1, 32)
        Me.btnKfzDetails.Name = "btnKfzDetails"
        Me.btnKfzDetails.Size = New System.Drawing.Size(98, 24)
        Me.btnKfzDetails.TabIndex = 7
        Me.btnKfzDetails.Text = "Details"
        Me.btnKfzDetails.UseVisualStyleBackColor = True
        '
        'btnKfzNeu
        '
        Me.btnKfzNeu.Location = New System.Drawing.Point(1, 2)
        Me.btnKfzNeu.Name = "btnKfzNeu"
        Me.btnKfzNeu.Size = New System.Drawing.Size(98, 24)
        Me.btnKfzNeu.TabIndex = 6
        Me.btnKfzNeu.Text = "Hinzufügen"
        Me.btnKfzNeu.UseVisualStyleBackColor = True
        '
        'DataChangedStatus
        '
        Me.DataChangedStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DataChangedStatus.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.DataChangedStatus.Location = New System.Drawing.Point(0, 179)
        Me.DataChangedStatus.Name = "DataChangedStatus"
        Me.DataChangedStatus.Size = New System.Drawing.Size(102, 15)
        Me.DataChangedStatus.TabIndex = 5
        Me.DataChangedStatus.Text = "X"
        Me.DataChangedStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(3, 259)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(271, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Kundenfahrzeuge:"
        '
        'btnExport2Csv
        '
        Me.btnExport2Csv.Location = New System.Drawing.Point(1, 126)
        Me.btnExport2Csv.Name = "btnExport2Csv"
        Me.btnExport2Csv.Size = New System.Drawing.Size(98, 22)
        Me.btnExport2Csv.TabIndex = 8
        Me.btnExport2Csv.Text = "Export->CSV"
        Me.btnExport2Csv.UseVisualStyleBackColor = True
        '
        'KundenDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(652, 472)
        Me.ControlBox = False
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(640, 480)
        Me.Name = "KundenDetails"
        Me.Text = "Kunden-Details"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.dgvFahrzeuge, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnAbbrechen As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dgvFahrzeuge As System.Windows.Forms.DataGridView
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnNeu As System.Windows.Forms.Button
    Friend WithEvents btnSpeichern As System.Windows.Forms.Button
    Friend WithEvents btnSuchen As System.Windows.Forms.Button
    Friend WithEvents DataChangedStatus As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents btnKfzNeu As System.Windows.Forms.Button
    Friend WithEvents btnKfzDetails As System.Windows.Forms.Button
    Friend WithEvents btnDeleteKfz As System.Windows.Forms.Button
    Friend WithEvents btn_RechnungenKunde As System.Windows.Forms.Button
    Friend WithEvents btn_RechnungenKFZ As System.Windows.Forms.Button
    Friend WithEvents btnOffeneRgen As System.Windows.Forms.Button
    Friend WithEvents btnDeleteKunde As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnDruckenFahrzeuge As System.Windows.Forms.Button
    Friend WithEvents btnExport2Csv As System.Windows.Forms.Button
End Class
