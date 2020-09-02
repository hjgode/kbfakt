    Private Declare Function SQLDataSources Lib "ODBC32.DLL" (ByVal henv As Integer, ByVal fDirection As Short, _
		ByVal szDSN As String, ByVal cbDSNMax As Short, ByRef pcbDSN As Short, _
		ByVal szDescription As String, ByVal cbDescriptionMax As Short, ByRef pcbDescription As Short) As Short 
    
	Private Declare Function SQLAllocEnv Lib "ODBC32.DLL" (ByRef env As Integer) As Short 
    
	Private Declare Function SQLConfigDataSource Lib "ODBCCP32.DLL" (ByVal hwndParent As Integer, _
		ByVal ByValfRequest As Integer, ByVal lpszDriver As String, ByVal lpszAttributes As String) As Integer 
    Const SQL_SUCCESS As Integer = 0 
    Const SQL_FETCH_NEXT As Integer = 1 
    Private Const ODBC_ADD_DSN As Short = 1 ' Add user data source 
    Private Const ODBC_CONFIG_DSN As Short = 2 ' Configure (edit) data source 
    Private Const ODBC_REMOVE_DSN As Short = 3 ' Remove data source 
    Private Const ODBC_ADD_SYS_DSN As Short = 4 'Add system data source 
    Private Const vbAPINull As Integer = 0 ' NULL Pointer 


   Public Sub FetchDSNs() 
        Dim ReturnValue As Short 
        Dim DSNName As String 
        Dim DriverName As String 
        Dim DSNNameLen As Short 
        Dim DriverNameLen As Short 
        Dim SQLEnv As Integer 'handle to the environment 

        If SQLAllocEnv(SQLEnv) <> -1 Then 
            Do Until ReturnValue <> SQL_SUCCESS 
                DSNName = Space(1024) 
                DriverName = Space(1024) 
                ReturnValue = SQLDataSources(SQLEnv, SQL_FETCH_NEXT, DSNName, 1024, DSNNameLen, DriverName, 1024, DriverNameLen) 
                DSNName = Left(DSNName, DSNNameLen) 
                DriverName = Left(DriverName, DriverNameLen) 

                If DSNName <> Space(DSNNameLen) Then 
                    System.Diagnostics.Debug.WriteLine(DSNName) 
                    System.Diagnostics.Debug.WriteLine(DriverName) 
                End If 
            Loop 
        End If 


    End Sub 


    Public Sub CreateSystemDSN() 
        Dim ReturnValue As Integer 
        Dim Driver As String 
        Dim Attributes As String 

        'strDriver = "Microsoft Access Driver (*.mdb)" 
        'strAttributes = "DESCRIPTION=AccessDSN" & Chr(0) 
        'strAttributes = strAttributes & "DSN=AccessDSN" & Chr(0) 
        'strAttributes = strAttributes & "DBQ=e:\MyDocuments\db1.mdb" & Chr(0) 
        'intRet = SQLConfigDataSource(vbAPINull, ODBC_ADD_SYS_DSN, strDriver, strAttributes) 
        'If intRet <> 0 Then 
        '    MsgBox("DSN Created") 
        'Else 
        '    MsgBox("Create Failed") 
        'End If 

        'Set the driver to SQL Server because it is most common. 
        Driver = "SQL Server" 
        'Set the attributes delimited by null. 
        'See driver documentation for a complete 
        'list of supported attributes. 
        Attributes = "SERVER=SomeServer" & Chr(0) 
        Attributes = Attributes & "DESCRIPTION=New DSN" & Chr(0) 
        Attributes = Attributes & "DSN=DSN_TEMP" & Chr(0) 
        Attributes = Attributes & "DATABASE=pubs" & Chr(0) 
        'To show dialog, use Form1.Hwnd instead of vbAPINull. 
        ReturnValue = SQLConfigDataSource(vbAPINull, ODBC_ADD_SYS_DSN, Driver, Attributes) 
        If ReturnValue <> 0 Then 
            MsgBox("DSN Created") 
        Else 
            MsgBox("Create Failed") 
        End If 
    End Sub 

