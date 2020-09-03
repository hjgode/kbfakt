<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class admin
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
        Me.ListBox2 = New System.Windows.Forms.ListBox
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.SqlCommand = New System.Windows.Forms.ComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnRemove = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnExecuteScalar = New System.Windows.Forms.Button
        Me.btn_NoneQuery = New System.Windows.Forms.Button
        Me.btnExecuteQuery = New System.Windows.Forms.Button
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.32169!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 82.67831!))
        Me.TableLayoutPanel1.Controls.Add(Me.ListBox2, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.DataGridView1, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.SqlCommand, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox1, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ListBox1, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 107.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(592, 373)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'ListBox2
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.ListBox2, 2)
        Me.ListBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.HorizontalScrollbar = True
        Me.ListBox2.Location = New System.Drawing.Point(3, 269)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(586, 95)
        Me.ListBox2.TabIndex = 3
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TableLayoutPanel1.SetColumnSpan(Me.DataGridView1, 2)
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(3, 109)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(586, 154)
        Me.DataGridView1.TabIndex = 0
        '
        'SqlCommand
        '
        Me.SqlCommand.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SqlCommand.FormattingEnabled = True
        Me.SqlCommand.Location = New System.Drawing.Point(105, 3)
        Me.SqlCommand.Name = "SqlCommand"
        Me.SqlCommand.Size = New System.Drawing.Size(484, 21)
        Me.SqlCommand.TabIndex = 3
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnRemove)
        Me.GroupBox1.Controls.Add(Me.btnAdd)
        Me.GroupBox1.Controls.Add(Me.btnExecuteScalar)
        Me.GroupBox1.Controls.Add(Me.btn_NoneQuery)
        Me.GroupBox1.Controls.Add(Me.btnExecuteQuery)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(105, 29)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(484, 74)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "SQL:"
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(36, 46)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(24, 20)
        Me.btnRemove.TabIndex = 4
        Me.btnRemove.Text = "-"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(6, 46)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(24, 20)
        Me.btnAdd.TabIndex = 4
        Me.btnAdd.Text = "+"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnExecuteScalar
        '
        Me.btnExecuteScalar.Location = New System.Drawing.Point(126, 46)
        Me.btnExecuteScalar.Name = "btnExecuteScalar"
        Me.btnExecuteScalar.Size = New System.Drawing.Size(114, 24)
        Me.btnExecuteScalar.TabIndex = 1
        Me.btnExecuteScalar.Text = "ExecuteScalar"
        Me.btnExecuteScalar.UseVisualStyleBackColor = True
        '
        'btn_NoneQuery
        '
        Me.btn_NoneQuery.Location = New System.Drawing.Point(126, 16)
        Me.btn_NoneQuery.Name = "btn_NoneQuery"
        Me.btn_NoneQuery.Size = New System.Drawing.Size(114, 24)
        Me.btn_NoneQuery.TabIndex = 1
        Me.btn_NoneQuery.Text = "ExecuteNonQuery"
        Me.btn_NoneQuery.UseVisualStyleBackColor = True
        '
        'btnExecuteQuery
        '
        Me.btnExecuteQuery.Location = New System.Drawing.Point(6, 16)
        Me.btnExecuteQuery.Name = "btnExecuteQuery"
        Me.btnExecuteQuery.Size = New System.Drawing.Size(114, 24)
        Me.btnExecuteQuery.TabIndex = 1
        Me.btnExecuteQuery.Text = "ExecuteQuery"
        Me.btnExecuteQuery.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(3, 3)
        Me.ListBox1.Name = "ListBox1"
        Me.TableLayoutPanel1.SetRowSpan(Me.ListBox1, 2)
        Me.ListBox1.Size = New System.Drawing.Size(96, 95)
        Me.ListBox1.TabIndex = 1
        '
        'admin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(592, 373)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(600, 400)
        Me.Name = "admin"
        Me.Text = "admin"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btn_NoneQuery As System.Windows.Forms.Button
    Friend WithEvents btnExecuteQuery As System.Windows.Forms.Button
    Friend WithEvents btnExecuteScalar As System.Windows.Forms.Button
    Friend WithEvents SqlCommand As System.Windows.Forms.ComboBox
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents ListBox2 As System.Windows.Forms.ListBox
End Class
