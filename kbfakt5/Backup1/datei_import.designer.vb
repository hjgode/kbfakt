<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class datei_import
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
        Me.dg = New System.Windows.Forms.DataGridView
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.m_datei_open = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.btn_Import = New System.Windows.Forms.Button
        Me.btnCreateAnreden = New System.Windows.Forms.Button
        Me.btnImportAll = New System.Windows.Forms.Button
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.ExtrasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DatenbankKomprimierenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Rech2ReparierenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DoppelteAuftr�geAusRECH1L�schenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ZEITEintr�geAusRechnungenInArtstamm�bernehmenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ArtikelMitSternchenUndPlusInArtStamm�bernehmenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ArtikelstammKorrigierenUndAmEndeEntfernenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.UmlauteKorrigierenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ZeitEintr�geAusRechnungenInArtStamm�bernehmenvollToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ArtStammUmToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CreateRichtwerteTablesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnu_Patch1FirmStam = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.dg, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.m_datei_open, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ListBox1, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.btn_Import, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnCreateAnreden, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnImportAll, 2, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.MenuStrip1, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 5
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(724, 551)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'dg
        '
        Me.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TableLayoutPanel1.SetColumnSpan(Me.dg, 3)
        Me.dg.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dg.Location = New System.Drawing.Point(3, 71)
        Me.dg.Name = "dg"
        Me.dg.ReadOnly = True
        Me.dg.Size = New System.Drawing.Size(718, 353)
        Me.dg.TabIndex = 5
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBox1.Location = New System.Drawing.Point(3, 41)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(317, 20)
        Me.TextBox1.TabIndex = 3
        '
        'm_datei_open
        '
        Me.m_datei_open.Dock = System.Windows.Forms.DockStyle.Fill
        Me.m_datei_open.Location = New System.Drawing.Point(527, 37)
        Me.m_datei_open.Name = "m_datei_open"
        Me.m_datei_open.Size = New System.Drawing.Size(94, 28)
        Me.m_datei_open.TabIndex = 4
        Me.m_datei_open.Text = "..."
        Me.m_datei_open.UseVisualStyleBackColor = True
        '
        'OK_Button
        '
        Me.OK_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OK_Button.Location = New System.Drawing.Point(527, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(94, 28)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Schliessen"
        '
        'ListBox1
        '
        Me.ListBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(3, 430)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(518, 82)
        Me.ListBox1.TabIndex = 7
        '
        'btn_Import
        '
        Me.btn_Import.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn_Import.Location = New System.Drawing.Point(627, 3)
        Me.btn_Import.Name = "btn_Import"
        Me.btn_Import.Size = New System.Drawing.Size(94, 28)
        Me.btn_Import.TabIndex = 6
        Me.btn_Import.Text = "Import"
        Me.btn_Import.UseVisualStyleBackColor = True
        '
        'btnCreateAnreden
        '
        Me.btnCreateAnreden.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnCreateAnreden.Location = New System.Drawing.Point(627, 37)
        Me.btnCreateAnreden.Name = "btnCreateAnreden"
        Me.btnCreateAnreden.Size = New System.Drawing.Size(94, 28)
        Me.btnCreateAnreden.TabIndex = 8
        Me.btnCreateAnreden.Text = "Anreden erstellen"
        Me.btnCreateAnreden.UseVisualStyleBackColor = True
        '
        'btnImportAll
        '
        Me.btnImportAll.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnImportAll.Location = New System.Drawing.Point(627, 519)
        Me.btnImportAll.Name = "btnImportAll"
        Me.btnImportAll.Size = New System.Drawing.Size(94, 29)
        Me.btnImportAll.TabIndex = 9
        Me.btnImportAll.Text = "Alles importieren"
        Me.btnImportAll.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExtrasToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(58, 24)
        Me.MenuStrip1.TabIndex = 11
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ExtrasToolStripMenuItem
        '
        Me.ExtrasToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlDark
        Me.ExtrasToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DatenbankKomprimierenToolStripMenuItem, Me.Rech2ReparierenToolStripMenuItem, Me.DoppelteAuftr�geAusRECH1L�schenToolStripMenuItem, Me.ZEITEintr�geAusRechnungenInArtstamm�bernehmenToolStripMenuItem, Me.ArtikelMitSternchenUndPlusInArtStamm�bernehmenToolStripMenuItem, Me.ArtikelstammKorrigierenUndAmEndeEntfernenToolStripMenuItem, Me.UmlauteKorrigierenToolStripMenuItem, Me.ZeitEintr�geAusRechnungenInArtStamm�bernehmenvollToolStripMenuItem, Me.ArtStammUmToolStripMenuItem, Me.CreateRichtwerteTablesToolStripMenuItem, Me.mnu_Patch1FirmStam})
        Me.ExtrasToolStripMenuItem.Name = "ExtrasToolStripMenuItem"
        Me.ExtrasToolStripMenuItem.Size = New System.Drawing.Size(50, 20)
        Me.ExtrasToolStripMenuItem.Text = "Extras"
        '
        'DatenbankKomprimierenToolStripMenuItem
        '
        Me.DatenbankKomprimierenToolStripMenuItem.Name = "DatenbankKomprimierenToolStripMenuItem"
        Me.DatenbankKomprimierenToolStripMenuItem.Size = New System.Drawing.Size(427, 22)
        Me.DatenbankKomprimierenToolStripMenuItem.Text = "Datenbank komprimieren"
        '
        'Rech2ReparierenToolStripMenuItem
        '
        Me.Rech2ReparierenToolStripMenuItem.Name = "Rech2ReparierenToolStripMenuItem"
        Me.Rech2ReparierenToolStripMenuItem.Size = New System.Drawing.Size(427, 22)
        Me.Rech2ReparierenToolStripMenuItem.Text = "Rech2 reparieren"
        '
        'DoppelteAuftr�geAusRECH1L�schenToolStripMenuItem
        '
        Me.DoppelteAuftr�geAusRECH1L�schenToolStripMenuItem.Name = "DoppelteAuftr�geAusRECH1L�schenToolStripMenuItem"
        Me.DoppelteAuftr�geAusRECH1L�schenToolStripMenuItem.Size = New System.Drawing.Size(427, 22)
        Me.DoppelteAuftr�geAusRECH1L�schenToolStripMenuItem.Text = "Doppelte Auftr�ge aus RECH1 l�schen"
        '
        'ZEITEintr�geAusRechnungenInArtstamm�bernehmenToolStripMenuItem
        '
        Me.ZEITEintr�geAusRechnungenInArtstamm�bernehmenToolStripMenuItem.Name = "ZEITEintr�geAusRechnungenInArtstamm�bernehmenToolStripMenuItem"
        Me.ZEITEintr�geAusRechnungenInArtstamm�bernehmenToolStripMenuItem.Size = New System.Drawing.Size(427, 22)
        Me.ZEITEintr�geAusRechnungenInArtstamm�bernehmenToolStripMenuItem.Text = "ZEIT Eintr�ge aus Rechnungen in Artstamm �bernehmen (abschneiden)"
        '
        'ArtikelMitSternchenUndPlusInArtStamm�bernehmenToolStripMenuItem
        '
        Me.ArtikelMitSternchenUndPlusInArtStamm�bernehmenToolStripMenuItem.Name = "ArtikelMitSternchenUndPlusInArtStamm�bernehmenToolStripMenuItem"
        Me.ArtikelMitSternchenUndPlusInArtStamm�bernehmenToolStripMenuItem.Size = New System.Drawing.Size(427, 22)
        Me.ArtikelMitSternchenUndPlusInArtStamm�bernehmenToolStripMenuItem.Text = "Artikel aus Rech2 in ArtStamm �bernehmen"
        '
        'ArtikelstammKorrigierenUndAmEndeEntfernenToolStripMenuItem
        '
        Me.ArtikelstammKorrigierenUndAmEndeEntfernenToolStripMenuItem.Name = "ArtikelstammKorrigierenUndAmEndeEntfernenToolStripMenuItem"
        Me.ArtikelstammKorrigierenUndAmEndeEntfernenToolStripMenuItem.Size = New System.Drawing.Size(427, 22)
        Me.ArtikelstammKorrigierenUndAmEndeEntfernenToolStripMenuItem.Text = "Artikelstamm korrigieren (* und + am Ende entfernen)"
        '
        'UmlauteKorrigierenToolStripMenuItem
        '
        Me.UmlauteKorrigierenToolStripMenuItem.Name = "UmlauteKorrigierenToolStripMenuItem"
        Me.UmlauteKorrigierenToolStripMenuItem.Size = New System.Drawing.Size(427, 22)
        Me.UmlauteKorrigierenToolStripMenuItem.Text = "Umlaute korrigieren"
        '
        'ZeitEintr�geAusRechnungenInArtStamm�bernehmenvollToolStripMenuItem
        '
        Me.ZeitEintr�geAusRechnungenInArtStamm�bernehmenvollToolStripMenuItem.Name = "ZeitEintr�geAusRechnungenInArtStamm�bernehmenvollToolStripMenuItem"
        Me.ZeitEintr�geAusRechnungenInArtStamm�bernehmenvollToolStripMenuItem.Size = New System.Drawing.Size(427, 22)
        Me.ZeitEintr�geAusRechnungenInArtStamm�bernehmenvollToolStripMenuItem.Text = "Zeit Eintr�ge aus Rechnungen in ArtStamm �bernehmen (voll)"
        '
        'ArtStammUmToolStripMenuItem
        '
        Me.ArtStammUmToolStripMenuItem.Name = "ArtStammUmToolStripMenuItem"
        Me.ArtStammUmToolStripMenuItem.Size = New System.Drawing.Size(427, 22)
        Me.ArtStammUmToolStripMenuItem.Text = "ArtStamm mit FzgTyp erg�nzen"
        '
        'CreateRichtwerteTablesToolStripMenuItem
        '
        Me.CreateRichtwerteTablesToolStripMenuItem.Name = "CreateRichtwerteTablesToolStripMenuItem"
        Me.CreateRichtwerteTablesToolStripMenuItem.Size = New System.Drawing.Size(427, 22)
        Me.CreateRichtwerteTablesToolStripMenuItem.Text = "Richtwerte �bernehmen"
        '
        'mnu_Patch1FirmStam
        '
        Me.mnu_Patch1FirmStam.Name = "mnu_Patch1FirmStam"
        Me.mnu_Patch1FirmStam.Size = New System.Drawing.Size(427, 22)
        Me.mnu_Patch1FirmStam.Text = "Patch1FirmStam"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'datei_import
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(724, 551)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(1024, 768)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(450, 340)
        Me.Name = "datei_import"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "datei_import"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents m_datei_open As System.Windows.Forms.Button
    Friend WithEvents dg As System.Windows.Forms.DataGridView
    Friend WithEvents btn_Import As System.Windows.Forms.Button
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents btnCreateAnreden As System.Windows.Forms.Button
    Friend WithEvents btnImportAll As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ExtrasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DatenbankKomprimierenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Rech2ReparierenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DoppelteAuftr�geAusRECH1L�schenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ZEITEintr�geAusRechnungenInArtstamm�bernehmenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ArtikelMitSternchenUndPlusInArtStamm�bernehmenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ArtikelstammKorrigierenUndAmEndeEntfernenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UmlauteKorrigierenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ZeitEintr�geAusRechnungenInArtStamm�bernehmenvollToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ArtStammUmToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CreateRichtwerteTablesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnu_Patch1FirmStam As System.Windows.Forms.ToolStripMenuItem

End Class
