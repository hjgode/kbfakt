<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MonatJahrPicker
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Monat = New System.Windows.Forms.NumericUpDown
        Me.Jahr = New System.Windows.Forms.NumericUpDown
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnAbbrechen = New System.Windows.Forms.Button
        CType(Me.Monat, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Jahr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(34, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Monat:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(34, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(30, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Jahr:"
        '
        'Monat
        '
        Me.Monat.Location = New System.Drawing.Point(94, 7)
        Me.Monat.Maximum = New Decimal(New Integer() {12, 0, 0, 0})
        Me.Monat.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.Monat.Name = "Monat"
        Me.Monat.Size = New System.Drawing.Size(50, 20)
        Me.Monat.TabIndex = 1
        Me.Monat.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Jahr
        '
        Me.Jahr.Location = New System.Drawing.Point(94, 34)
        Me.Jahr.Maximum = New Decimal(New Integer() {2050, 0, 0, 0})
        Me.Jahr.Minimum = New Decimal(New Integer() {1949, 0, 0, 0})
        Me.Jahr.Name = "Jahr"
        Me.Jahr.Size = New System.Drawing.Size(50, 20)
        Me.Jahr.TabIndex = 1
        Me.Jahr.Value = New Decimal(New Integer() {1950, 0, 0, 0})
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(94, 73)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(73, 21)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnAbbrechen
        '
        Me.btnAbbrechen.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnAbbrechen.Location = New System.Drawing.Point(15, 73)
        Me.btnAbbrechen.Name = "btnAbbrechen"
        Me.btnAbbrechen.Size = New System.Drawing.Size(73, 21)
        Me.btnAbbrechen.TabIndex = 2
        Me.btnAbbrechen.Text = "Abbrechen"
        Me.btnAbbrechen.UseVisualStyleBackColor = True
        '
        'MonatJahrPicker
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(184, 113)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnAbbrechen)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.Jahr)
        Me.Controls.Add(Me.Monat)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MonatJahrPicker"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "MonatJahrPicker"
        Me.TopMost = True
        CType(Me.Monat, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Jahr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Monat As System.Windows.Forms.NumericUpDown
    Friend WithEvents Jahr As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnAbbrechen As System.Windows.Forms.Button
End Class
