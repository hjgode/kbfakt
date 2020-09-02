<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnStart = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.radioCSV = New System.Windows.Forms.RadioButton
        Me.radioExcel = New System.Windows.Forms.RadioButton
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnStart
        '
        Me.btnStart.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnStart.Location = New System.Drawing.Point(101, 180)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(84, 25)
        Me.btnStart.TabIndex = 0
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(101, 211)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(84, 25)
        Me.btnCancel.TabIndex = 0
        Me.btnCancel.Text = "Abbrechen"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.radioExcel)
        Me.Panel1.Controls.Add(Me.radioCSV)
        Me.Panel1.Location = New System.Drawing.Point(10, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(270, 156)
        Me.Panel1.TabIndex = 1
        '
        'radioCSV
        '
        Me.radioCSV.AutoSize = True
        Me.radioCSV.Checked = True
        Me.radioCSV.Location = New System.Drawing.Point(38, 20)
        Me.radioCSV.Name = "radioCSV"
        Me.radioCSV.Size = New System.Drawing.Size(139, 17)
        Me.radioCSV.TabIndex = 0
        Me.radioCSV.TabStop = True
        Me.radioCSV.Text = "Export in Textdatei (csv)"
        Me.radioCSV.UseVisualStyleBackColor = True
        '
        'radioExcel
        '
        Me.radioExcel.AutoSize = True
        Me.radioExcel.Location = New System.Drawing.Point(38, 43)
        Me.radioExcel.Name = "radioExcel"
        Me.radioExcel.Size = New System.Drawing.Size(111, 17)
        Me.radioExcel.TabIndex = 0
        Me.radioExcel.Text = "Export nach Excel"
        Me.radioExcel.UseVisualStyleBackColor = True
        '
        'frmExport
        '
        Me.AcceptButton = Me.btnStart
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(292, 273)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnStart)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmExport"
        Me.Text = "Tabellen-Export"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents radioExcel As System.Windows.Forms.RadioButton
    Friend WithEvents radioCSV As System.Windows.Forms.RadioButton
End Class
