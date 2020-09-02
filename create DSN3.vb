Option Explicit

Private Declare Function SQLConfigDataSource Lib "ODBCCP32.DLL" _
   (ByVal hwndParent As Long, ByVal fRequest As Long, _
   ByVal lpszDriver As String, ByVal lpszAttributes As String) _
   As Long

Private Const ODBC_ADD_SYS_DSN = 4

Public Function CreateSQLServerDSN(DSNName As String, _
   ServerName As String, Database As String) As Boolean

'PURPOSE: 'CREATES A SYSTEM DSN FOR AN SQL SERVER DATABASE
'PARAMETERS: 'DSNName = DSN Name
             'ServerName = Name of Server
             'Database = Database to Use
'RETURNS: True if successful, false otherwise
'EXAMPLE: CreateSQLServerDSN "MyDSN", "MyServer", "MyDatabase"

Dim sAttributes As String

sAttributes = "DSN=" & DSNName & Chr(0)
sAttributes = sAttributes & "Server=" & ServerName & Chr(0)
sAttributes = sAttributes & "Database=" & Database & Chr(0)
CreateSQLServerDSN = CreateDSN("SQL Server", sAttributes)

End Function

Public Function CreateAccessDSN(DSNName As String, _
  DatabaseFullPath As String) As Boolean

'PURPOSE: 'CREATES A SYSTEM DSN FOR AN ACCESS DATABASE
'PARAMETERS: 'DSNName = DSN Name
             'DatabaseFullPath = Full Path to .mdb file
'RETURNS: True if successful, false otherwise
'EXAMPLE: CreateAccessDSN "MyDSN", "C:\MyDb.mdb"

    Dim sAttributes As String
    
    'TEST TO SEE IF FILE EXISTS: YOU CAN REMOVE IF YOU
    'DON'T WANT IT
    If Dir(DatabaseFullPath) = "" Then Exit Function
    
sAttributes = "DSN=" & DSNName & Chr(0)
sAttributes = sAttributes & "DBQ=" & DatabaseFullPath & Chr(0)
CreateAccessDSN = CreateDSN("Microsoft Access Driver (*.mdb)", _
   sAttributes)

End Function

Public Function CreateDSN(Driver As String, Attributes As _
  String) As Boolean

'PURPOSE: CREATES A SYSTEM DSN
'PARAMETERS: 'Driver = DriverName
'ATTRIBUTES: 'Attributes; varies as a function
             'of the Driver
'EXAMPLE: Refer to Code Above

    CreateDSN = SQLConfigDataSource(0&, ODBC_ADD_SYS_DSN, _
      Driver, Attributes)
        
End Function
