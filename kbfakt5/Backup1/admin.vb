Imports System.Data.OleDb
Public Class admin
    Private SQLlistChanged = False
    Public Shared cn As OleDbConnection
    'for dbase files use the dbf filename
    Dim sSQL As String
    Public Shared DBDA As OleDbDataAdapter
    Public Shared DBDR As OleDbDataReader
    Public Shared DBDT As DataTable
    Public Shared DBDS As DataSet
    Private Sub FillTableNames()

        Dim schemaTable As DataTable = _
            cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, _
            New Object() {Nothing, Nothing, Nothing, "TABLE"})
        'ListBox1.DataSource = schemaTable
        Dim DBTableRow As DataRow
        ListBox1.Items.Clear()
        For Each DBTableRow In schemaTable.Rows
            ListBox1.Items.Add(DBTableRow.Item(2))
        Next
    End Sub

    Private Sub admin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FormSettingsLoad(Me)
        cn = New OleDbConnection(mainmodul.connectionString)
        OpenDBConnection(cn)
        FillTableNames()
        FillSqlList()
    End Sub
    Public Function FillDS(ByVal query As String) As Integer
        DBDA = New OleDbDataAdapter
        DBDA.SelectCommand = New OleDbCommand(query, cn)
        DBDS = New DataSet
        'DBDS.Clear()
        Try
            Return DBDA.Fill(DBDS)
        Catch ex As OleDbException
            addlog("### FillDS " & query & " Fehler: " & ex.Message)
            Return 0
        End Try
    End Function
    Public Function FillDT(ByVal query As String) As Integer
        DBDA.SelectCommand = New OleDbCommand(query, cn)
        Return DBDA.Fill(DBDT)
    End Function
    Public Function FillGrid(ByRef grid As DataGrid, ByVal query As String) As Integer
        'grid.ResetBindings()
        'grid.BindingContext = Nothing
        'grid.DataMember = Nothing
        'grid.DataSource = Nothing
        Dim c As Integer
        c = FillDS(query)
        If c > 0 Then
            grid.SetDataBinding(DBDS, "Table") 'use TableMappings.Add("Table", ... to rename table ref
        Else
            grid.DataSource = Nothing
        End If
        grid.ReadOnly = False
        Return c
    End Function
    Public Function FillGrid(ByRef grid As DataGridView, ByVal query As String) As Integer
        Dim c As Integer
        c = FillDS(query)
        Dim bs As New BindingSource
        bs.DataSource = DBDS.Tables("Table")
        If c > 0 Then
            grid.DataSource = bs '(DBDS, "Table") 'use TableMappings.Add("Table", ... to rename table ref
        Else
            grid.DataSource = Nothing
        End If
        grid.ReadOnly = True
        Return c

    End Function
    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim tablename As String
        tablename = ListBox1.SelectedItem.ToString
        Dim c As Integer = FillGrid(DataGridView1, "select * from " & tablename) 'RECHNUNGEN")
        addlog("select * from " & tablename & " returned " & c & " records")
    End Sub

    Private Sub btnExecuteQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExecuteQuery.Click
        Dim c As Integer
        c = FillGrid(DataGridView1, SqlCommand.Text)
        addlog("ExecuteQuery '" & SqlCommand.Text & "' returned " & c & " records")
    End Sub

    Private Sub btn_NoneQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_NoneQuery.Click
        Dim AccessConn As New System.Data.OleDb.OleDbConnection
        Dim cmd As OleDbCommand
        Dim result As Long
        Try
            OpenDBConnection(AccessConn)
            cmd = New OleDbCommand(SqlCommand.Text, AccessConn)
            result = cmd.ExecuteNonQuery
            addlog(SqlCommand.Text & " -> " & result)
        Catch ex As OleDbException
            addlog("### " & SqlCommand.Text & " Fehler in Execute: " & ex.Message)
        Finally
            AccessConn.Close()
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExecuteScalar.Click
        Dim cn As New OleDb.OleDbConnection(connectionString)
        Dim dbCmd As OleDbCommand
        Dim s As String
        dbCmd = New OleDbCommand(sqlCommand.Text, cn)
        cn.Open()
        Dim result As Object
        Try
            result = dbCmd.ExecuteScalar()
            If IsNumeric(result) Then
                s = CDbl(result).ToString
            Else
                'should be a string
                s = result.ToString()
            End If
            addlog(SqlCommand.Text & " -> " & s)
        Catch ex As OleDbException
            addlog("### " & SqlCommand.Text & " Fehler in ExecuteScalar: " & ex.Message)
        Finally
            cn.Close()
        End Try
    End Sub
    Private sqlfile As String = Environment.CurrentDirectory & "\sql.txt"
    Private Sub FillSqlList()
        If System.IO.File.Exists(sqlfile) Then
            SqlCommand.Items.Clear()
            Dim sr As New System.IO.StreamReader(sqlfile)
            Dim line As String
            ' Read and display the lines from the file until the end 
            ' of the file is reached.
            Do
                line = sr.ReadLine()
                If Not IsNothing(line) Then SqlCommand.Items.Add(line)
            Loop Until line Is Nothing
            sr.Close()
        Else
            SqlCommand.Items.Add("Select * from RECH1")
            SqlCommand.Items.Add("Select SUM (XNETTO) from RECH1")
            SqlCommand.Items.Add("Select MAX (XAUFTR_NR) from RECH1")
            SqlCommand.Items.Add("Select * from RECH2")
        End If
    End Sub
    Private Sub saveSqlList()
        Dim sw As New System.IO.StreamWriter(sqlfile)
        Dim i
        For i = 0 To SqlCommand.Items.Count - 1
            sw.WriteLine(SqlCommand.Items(i).ToString)
        Next
        sw.Close()
    End Sub
    Private Sub admin_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If SQLlistChanged Then
            If MessageBox.Show("SQL Liste speichern?", "Admin schliessen", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                saveSqlList()
            End If
        End If
        FormSettingsSave(Me)
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim i = SqlCommand.Items.Add(SqlCommand.Text)
        SqlCommand.SelectedIndex = i
        SQLlistChanged = True

    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If SqlCommand.SelectedIndex = -1 Then Exit Sub
        SqlCommand.Items.RemoveAt(SqlCommand.SelectedIndex)
        SqlCommand.SelectedIndex = 0
        SQLlistChanged = True

    End Sub
    Private Sub addlog(ByVal s As String)
        ListBox2.Items.Add(s)
        ListBox2.SelectedIndex = ListBox2.Items.Count - 1
    End Sub

End Class