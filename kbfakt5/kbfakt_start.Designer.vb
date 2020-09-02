<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class kbfakt_start
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(kbfakt_start))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.DateiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BeendenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExtrasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SetupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuImport = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAdmin = New System.Windows.Forms.ToolStripMenuItem
        Me.StammdatenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.ÜberToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.btnAuftrag = New System.Windows.Forms.Button
        Me.btnKunden = New System.Windows.Forms.Button
        Me.btnFahrzeuge = New System.Windows.Forms.Button
        Me.btnSammelDruck = New System.Windows.Forms.Button
        Me.btnAuswertungen = New System.Windows.Forms.Button
        Me.btnArtikelStamm = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnOffeneRechnungen = New System.Windows.Forms.Button
        Me.lblVersion = New System.Windows.Forms.Label
        Me.btnRichtwerte = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.btnSicherungen = New System.Windows.Forms.Button
        Me.btnTermine = New System.Windows.Forms.Button
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DateiToolStripMenuItem, Me.ExtrasToolStripMenuItem, Me.ToolStripMenuItem1})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(494, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'DateiToolStripMenuItem
        '
        Me.DateiToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BeendenToolStripMenuItem})
        Me.DateiToolStripMenuItem.Name = "DateiToolStripMenuItem"
        Me.DateiToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.DateiToolStripMenuItem.Text = "&Datei"
        '
        'BeendenToolStripMenuItem
        '
        Me.BeendenToolStripMenuItem.Name = "BeendenToolStripMenuItem"
        Me.BeendenToolStripMenuItem.Size = New System.Drawing.Size(127, 22)
        Me.BeendenToolStripMenuItem.Text = "Be&enden"
        '
        'ExtrasToolStripMenuItem
        '
        Me.ExtrasToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SetupToolStripMenuItem, Me.StammdatenToolStripMenuItem})
        Me.ExtrasToolStripMenuItem.Name = "ExtrasToolStripMenuItem"
        Me.ExtrasToolStripMenuItem.Size = New System.Drawing.Size(50, 20)
        Me.ExtrasToolStripMenuItem.Text = "E&xtras"
        '
        'SetupToolStripMenuItem
        '
        Me.SetupToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuImport, Me.mnuAdmin})
        Me.SetupToolStripMenuItem.Name = "SetupToolStripMenuItem"
        Me.SetupToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.SetupToolStripMenuItem.Text = "Setup"
        '
        'mnuImport
        '
        Me.mnuImport.Image = Global.kbfakt5.My.Resources.Resources.locked
        Me.mnuImport.Name = "mnuImport"
        Me.mnuImport.Size = New System.Drawing.Size(117, 22)
        Me.mnuImport.Text = "Import"
        '
        'mnuAdmin
        '
        Me.mnuAdmin.Image = Global.kbfakt5.My.Resources.Resources.locked
        Me.mnuAdmin.Name = "mnuAdmin"
        Me.mnuAdmin.Size = New System.Drawing.Size(117, 22)
        Me.mnuAdmin.Text = "Admin"
        '
        'StammdatenToolStripMenuItem
        '
        Me.StammdatenToolStripMenuItem.Name = "StammdatenToolStripMenuItem"
        Me.StammdatenToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.StammdatenToolStripMenuItem.Text = "Stammdaten"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ÜberToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(24, 20)
        Me.ToolStripMenuItem1.Text = "?"
        '
        'ÜberToolStripMenuItem
        '
        Me.ÜberToolStripMenuItem.Name = "ÜberToolStripMenuItem"
        Me.ÜberToolStripMenuItem.Size = New System.Drawing.Size(108, 22)
        Me.ÜberToolStripMenuItem.Text = "Über"
        '
        'btnAuftrag
        '
        Me.btnAuftrag.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAuftrag.Location = New System.Drawing.Point(59, 51)
        Me.btnAuftrag.Name = "btnAuftrag"
        Me.btnAuftrag.Size = New System.Drawing.Size(168, 29)
        Me.btnAuftrag.TabIndex = 1
        Me.btnAuftrag.Text = "Auftrag"
        Me.btnAuftrag.UseVisualStyleBackColor = True
        '
        'btnKunden
        '
        Me.btnKunden.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnKunden.Location = New System.Drawing.Point(59, 86)
        Me.btnKunden.Name = "btnKunden"
        Me.btnKunden.Size = New System.Drawing.Size(168, 29)
        Me.btnKunden.TabIndex = 1
        Me.btnKunden.Text = "Kunden"
        Me.btnKunden.UseVisualStyleBackColor = True
        '
        'btnFahrzeuge
        '
        Me.btnFahrzeuge.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFahrzeuge.Location = New System.Drawing.Point(59, 121)
        Me.btnFahrzeuge.Name = "btnFahrzeuge"
        Me.btnFahrzeuge.Size = New System.Drawing.Size(168, 29)
        Me.btnFahrzeuge.TabIndex = 1
        Me.btnFahrzeuge.Text = "Fahrzeuge"
        Me.btnFahrzeuge.UseVisualStyleBackColor = True
        '
        'btnSammelDruck
        '
        Me.btnSammelDruck.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSammelDruck.Location = New System.Drawing.Point(260, 51)
        Me.btnSammelDruck.Name = "btnSammelDruck"
        Me.btnSammelDruck.Size = New System.Drawing.Size(168, 29)
        Me.btnSammelDruck.TabIndex = 1
        Me.btnSammelDruck.Text = "Sammeldruck"
        Me.btnSammelDruck.UseVisualStyleBackColor = True
        '
        'btnAuswertungen
        '
        Me.btnAuswertungen.Image = Global.kbfakt5.My.Resources.Resources.locked
        Me.btnAuswertungen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAuswertungen.Location = New System.Drawing.Point(260, 121)
        Me.btnAuswertungen.Name = "btnAuswertungen"
        Me.btnAuswertungen.Size = New System.Drawing.Size(168, 29)
        Me.btnAuswertungen.TabIndex = 1
        Me.btnAuswertungen.Text = "Auswertungen"
        Me.btnAuswertungen.UseVisualStyleBackColor = True
        '
        'btnArtikelStamm
        '
        Me.btnArtikelStamm.Location = New System.Drawing.Point(59, 156)
        Me.btnArtikelStamm.Name = "btnArtikelStamm"
        Me.btnArtikelStamm.Size = New System.Drawing.Size(168, 29)
        Me.btnArtikelStamm.TabIndex = 1
        Me.btnArtikelStamm.Text = "Artikelstamm"
        Me.btnArtikelStamm.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(0, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(292, 11)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Label1"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(210, 271)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(65, 27)
        Me.btnExit.TabIndex = 3
        Me.btnExit.Text = "eXit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnOffeneRechnungen
        '
        Me.btnOffeneRechnungen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOffeneRechnungen.Location = New System.Drawing.Point(260, 86)
        Me.btnOffeneRechnungen.Name = "btnOffeneRechnungen"
        Me.btnOffeneRechnungen.Size = New System.Drawing.Size(168, 29)
        Me.btnOffeneRechnungen.TabIndex = 1
        Me.btnOffeneRechnungen.Text = "Offene Rechnungen"
        Me.btnOffeneRechnungen.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(0, 300)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(42, 13)
        Me.lblVersion.TabIndex = 4
        Me.lblVersion.Text = "Version"
        '
        'btnRichtwerte
        '
        Me.btnRichtwerte.Location = New System.Drawing.Point(59, 191)
        Me.btnRichtwerte.Name = "btnRichtwerte"
        Me.btnRichtwerte.Size = New System.Drawing.Size(168, 29)
        Me.btnRichtwerte.TabIndex = 1
        Me.btnRichtwerte.Text = "Richtwerte"
        Me.btnRichtwerte.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(363, 271)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(65, 27)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnSicherungen
        '
        Me.btnSicherungen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSicherungen.Location = New System.Drawing.Point(260, 191)
        Me.btnSicherungen.Name = "btnSicherungen"
        Me.btnSicherungen.Size = New System.Drawing.Size(168, 29)
        Me.btnSicherungen.TabIndex = 6
        Me.btnSicherungen.Text = "Datensicherungen"
        Me.btnSicherungen.UseVisualStyleBackColor = True
        '
        'btnTermine
        '
        Me.btnTermine.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnTermine.Location = New System.Drawing.Point(260, 156)
        Me.btnTermine.Name = "btnTermine"
        Me.btnTermine.Size = New System.Drawing.Size(168, 29)
        Me.btnTermine.TabIndex = 6
        Me.btnTermine.Text = "Terminfälligkeiten"
        Me.btnTermine.UseVisualStyleBackColor = True
        '
        'kbfakt_start
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(494, 319)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnTermine)
        Me.Controls.Add(Me.btnSicherungen)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnOffeneRechnungen)
        Me.Controls.Add(Me.btnAuswertungen)
        Me.Controls.Add(Me.btnRichtwerte)
        Me.Controls.Add(Me.btnArtikelStamm)
        Me.Controls.Add(Me.btnSammelDruck)
        Me.Controls.Add(Me.btnFahrzeuge)
        Me.Controls.Add(Me.btnKunden)
        Me.Controls.Add(Me.btnAuftrag)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "kbfakt_start"
        Me.Text = "kbfakt_start"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents DateiToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BeendenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExtrasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetupToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuImport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdmin As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnAuftrag As System.Windows.Forms.Button
    Friend WithEvents btnKunden As System.Windows.Forms.Button
    Friend WithEvents btnFahrzeuge As System.Windows.Forms.Button
    Friend WithEvents btnSammelDruck As System.Windows.Forms.Button
    Friend WithEvents btnAuswertungen As System.Windows.Forms.Button
    Friend WithEvents btnArtikelStamm As System.Windows.Forms.Button
    Friend WithEvents StammdatenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnOffeneRechnungen As System.Windows.Forms.Button
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ÜberToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents btnRichtwerte As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnSicherungen As System.Windows.Forms.Button
    Friend WithEvents btnTermine As System.Windows.Forms.Button
End Class
