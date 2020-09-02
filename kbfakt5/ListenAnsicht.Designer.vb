<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListenAnsicht
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
        Me.panel = New System.Windows.Forms.TableLayoutPanel
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.btn_Close = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnDetails = New System.Windows.Forms.Button
        Me.PanelGesamt = New System.Windows.Forms.Panel
        Me.LabelGesamt = New System.Windows.Forms.Label
        Me.txtXNetto = New System.Windows.Forms.TextBox
        Me.panel.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.PanelGesamt.SuspendLayout()
        Me.SuspendLayout()
        '
        'panel
        '
        Me.panel.ColumnCount = 2
        Me.panel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.panel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.panel.Controls.Add(Me.DataGridView1, 0, 1)
        Me.panel.Controls.Add(Me.btn_Close, 1, 0)
        Me.panel.Controls.Add(Me.Panel1, 1, 1)
        Me.panel.Controls.Add(Me.PanelGesamt, 0, 2)
        Me.panel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panel.Location = New System.Drawing.Point(0, 0)
        Me.panel.Name = "panel"
        Me.panel.RowCount = 3
        Me.panel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.panel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.panel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.panel.Size = New System.Drawing.Size(701, 373)
        Me.panel.TabIndex = 0
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(3, 29)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(595, 315)
        Me.DataGridView1.TabIndex = 0
        '
        'btn_Close
        '
        Me.btn_Close.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn_Close.Location = New System.Drawing.Point(604, 3)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(94, 20)
        Me.btn_Close.TabIndex = 1
        Me.btn_Close.Text = "Schliessen"
        Me.btn_Close.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnDetails)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(604, 29)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(94, 315)
        Me.Panel1.TabIndex = 2
        '
        'btnDetails
        '
        Me.btnDetails.Location = New System.Drawing.Point(0, 3)
        Me.btnDetails.Name = "btnDetails"
        Me.btnDetails.Size = New System.Drawing.Size(94, 22)
        Me.btnDetails.TabIndex = 0
        Me.btnDetails.Text = "Details"
        Me.btnDetails.UseVisualStyleBackColor = True
        '
        'PanelGesamt
        '
        Me.PanelGesamt.Controls.Add(Me.LabelGesamt)
        Me.PanelGesamt.Controls.Add(Me.txtXNetto)
        Me.PanelGesamt.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelGesamt.Location = New System.Drawing.Point(3, 350)
        Me.PanelGesamt.Name = "PanelGesamt"
        Me.PanelGesamt.Size = New System.Drawing.Size(595, 20)
        Me.PanelGesamt.TabIndex = 4
        '
        'LabelGesamt
        '
        Me.LabelGesamt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelGesamt.Location = New System.Drawing.Point(294, 3)
        Me.LabelGesamt.Name = "LabelGesamt"
        Me.LabelGesamt.Size = New System.Drawing.Size(92, 17)
        Me.LabelGesamt.TabIndex = 4
        Me.LabelGesamt.Text = "Gesamt:"
        Me.LabelGesamt.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtXNetto
        '
        Me.txtXNetto.Enabled = False
        Me.txtXNetto.Location = New System.Drawing.Point(392, 0)
        Me.txtXNetto.Name = "txtXNetto"
        Me.txtXNetto.Size = New System.Drawing.Size(94, 20)
        Me.txtXNetto.TabIndex = 3
        Me.txtXNetto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ListenAnsicht
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(701, 373)
        Me.Controls.Add(Me.panel)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(600, 400)
        Me.Name = "ListenAnsicht"
        Me.Text = "ListenAnsicht"
        Me.panel.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.PanelGesamt.ResumeLayout(False)
        Me.PanelGesamt.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents panel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnDetails As System.Windows.Forms.Button
    Friend WithEvents txtXNetto As System.Windows.Forms.TextBox
    Friend WithEvents PanelGesamt As System.Windows.Forms.Panel
    Friend WithEvents LabelGesamt As System.Windows.Forms.Label
End Class
