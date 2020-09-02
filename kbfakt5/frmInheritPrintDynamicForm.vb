Public Class frmInheritPrintDynamicForm
    Inherits kbfakt5.frmPrintDynamicForm

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCond As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtCond = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        CType(Me.dbSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnPrnDlg
        '
        Me.btnPrnDlg.Name = "btnPrnDlg"
        Me.btnPrnDlg.Size = New System.Drawing.Size(100, 24)
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(336, 8)
        Me.btnPrint.Name = "btnPrint"
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(416, 8)
        Me.btnClose.Name = "btnClose"
        '
        'printdlg
        '
        Me.printdlg.Location = New System.Drawing.Point(197, 17)
        '
        'Panel1
        '
        Me.Panel1.Location = New System.Drawing.Point(0, 53)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(504, 40)
        '
        'btnDocSet
        '
        Me.btnDocSet.Name = "btnDocSet"
        Me.btnDocSet.Size = New System.Drawing.Size(96, 24)
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(208, 16)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Amount of unit in stock more than"
        '
        'txtCond
        '
        Me.txtCond.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCond.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtCond.ForeColor = System.Drawing.Color.Red
        Me.txtCond.Location = New System.Drawing.Point(224, 8)
        Me.txtCond.Name = "txtCond"
        Me.txtCond.Size = New System.Drawing.Size(72, 22)
        Me.txtCond.TabIndex = 16
        Me.txtCond.Text = "0"
        Me.txtCond.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label2.Location = New System.Drawing.Point(312, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 16)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Units"
        '
        'frmInheritPrintDynamicForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(504, 93)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtCond)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmInheritPrintDynamicForm"
        Me.Text = "Report for select product information by amount of unit in stock condition"
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.txtCond, 0)
        Me.Controls.SetChildIndex(Me.Label2, 0)
        Me.Controls.SetChildIndex(Me.Panel1, 0)
        Me.Panel1.ResumeLayout(False)
        CType(Me.dbSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private SqlStr As String
    Public SqlStr1 As String
    Public dbSet1 As DataSet
    Private Sub frmInheritPrintDynamicForm()
        SqlStr = " select 'Product Name',0,30,'L','N','',1,ProductName," & _
                 " 'Unit Price',31,50,'R','N','#,##0.00$',1,UnitPrice, " & _
                 " 'Units In Stock',51,70,'R','N','#,##0.00',1,UnitsInStock, " & _
                 " 'Value',71,90,'R','Y','#,##0.00$',1,(UnitPrice*UnitsInStock) as ProductValue " & _
                 " from Products" & _
                 " where UnitsInStock > " & txtCond.Text & _
                 " order by ProductName"
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not IsNothing(dbSet1) Then
            dbSet = dbSet1
        End If
        If SqlStr1.Length > 0 Then
            SqlStr = SqlStr1
        End If
        Try
            '----- Set SQL string for report by this format (1 set per 1 report column):
            '----- Column 1 -> column name
            '----- Column 2 -> Start position on paper, I set all width has range between 0-99
            '----- Column 3 -> End position on paper, I set all width has range between 0-99
            '----- Column 4 -> Justify (L-Left, R-Right, C-Center)
            '----- Column 5 -> Has summarize in this column (Y/N)
            '----- Column 6 -> Display format(such as #,##0.00)
            '----- Column 7 -> Rest in line? Begin with 1
            '----- Column 8 -> Data Column
            '----- Please look at this example-------------------------------------------
            If txtCond.Text.Trim = "" Then txtCond.Text = 0
            If IsNothing(dbSet) Then
                Dim oledbcon As New OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=Products.mdb;Mode=Share Deny None;Extended Properties="""";Jet OLEDB:System database="""";Jet OLEDB:Registry Path="""";Jet OLEDB:Engine Type=5;Jet OLEDB:Database Locking Mode=1;Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Global Bulk Transactions=1;Jet OLEDB:Create System Database=False;Jet OLEDB:Encrypt Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;Jet OLEDB:SFP=False")
                Dim oleda As New OleDb.OleDbDataAdapter(SqlStr, oledbcon)

                dbSet.Tables.Clear()
                oleda.Fill(dbSet)
            End If
            '--- We can set properties of PageSetDlg and prnSetDlg object for print option 
            'PageSetDlg.PageSettings.Landscape = True

            hasSum = True
            FontName = "Tahoma"
            FontSize = 12
            FontSizeHead = 16
            FontSizeHead2 = 14
            LineHeight = 1
            InitPrint()
            HeadFirstString = "Report for Product Detail"
            HeadSecondString = "Test Report"
            DateString = Now.ToString
            PrintPreview()
        Catch ex As Exception
            MessageBox.Show(Err.Description)
        End Try
    End Sub
End Class
