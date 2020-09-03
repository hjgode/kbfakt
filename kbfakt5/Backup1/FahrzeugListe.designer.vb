<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Fahrzeugliste
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
        Me.btn_cancel = New System.Windows.Forms.Button
        Me.btn_OK = New System.Windows.Forms.Button
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.GridFahrzeuge = New System.Windows.Forms.DataGridView
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnDetails = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnRechnungen = New System.Windows.Forms.Button
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.GridFahrzeuge, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btn_cancel
        '
        Me.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_cancel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn_cancel.Location = New System.Drawing.Point(521, 35)
        Me.btn_cancel.Name = "btn_cancel"
        Me.btn_cancel.Size = New System.Drawing.Size(90, 30)
        Me.btn_cancel.TabIndex = 2
        Me.btn_cancel.Text = "Abbrechen"
        Me.btn_cancel.UseVisualStyleBackColor = True
        '
        'btn_OK
        '
        Me.btn_OK.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn_OK.Location = New System.Drawing.Point(521, 3)
        Me.btn_OK.Name = "btn_OK"
        Me.btn_OK.Size = New System.Drawing.Size(90, 26)
        Me.btn_OK.TabIndex = 1
        Me.btn_OK.Text = "OK"
        Me.btn_OK.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.36482!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.63518!))
        Me.TableLayoutPanel1.Controls.Add(Me.btn_OK, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btn_cancel, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.GridFahrzeuge, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 152.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(614, 392)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'GridFahrzeuge
        '
        Me.GridFahrzeuge.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TableLayoutPanel1.SetColumnSpan(Me.GridFahrzeuge, 2)
        Me.GridFahrzeuge.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridFahrzeuge.Location = New System.Drawing.Point(3, 71)
        Me.GridFahrzeuge.Name = "GridFahrzeuge"
        Me.GridFahrzeuge.ReadOnly = True
        Me.GridFahrzeuge.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GridFahrzeuge.Size = New System.Drawing.Size(608, 318)
        Me.GridFahrzeuge.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(512, 32)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Fahrzeug auswählen. Eingabe übernimmt Auswahl, Escape beendet die Auswahl ohne Fa" & _
            "hrzeug."
        '
        'btnDetails
        '
        Me.btnDetails.Location = New System.Drawing.Point(417, 0)
        Me.btnDetails.Name = "btnDetails"
        Me.btnDetails.Size = New System.Drawing.Size(92, 30)
        Me.btnDetails.TabIndex = 4
        Me.btnDetails.Text = "Details"
        Me.btnDetails.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnRechnungen)
        Me.Panel1.Controls.Add(Me.btnDetails)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 35)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(512, 30)
        Me.Panel1.TabIndex = 5
        '
        'btnRechnungen
        '
        Me.btnRechnungen.Location = New System.Drawing.Point(319, 0)
        Me.btnRechnungen.Name = "btnRechnungen"
        Me.btnRechnungen.Size = New System.Drawing.Size(92, 30)
        Me.btnRechnungen.TabIndex = 4
        Me.btnRechnungen.Text = "Rechnungen"
        Me.btnRechnungen.UseVisualStyleBackColor = True
        '
        'Fahrzeugliste
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(614, 392)
        Me.ControlBox = False
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(600, 400)
        Me.Name = "Fahrzeugliste"
        Me.Text = "Fahrzeugliste"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.GridFahrzeuge, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btn_cancel As System.Windows.Forms.Button
    Friend WithEvents btn_OK As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents GridFahrzeuge As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnDetails As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnRechnungen As System.Windows.Forms.Button
End Class
