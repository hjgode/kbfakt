Imports System.Windows.Forms
Imports System.Data.Odbc
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports kbfakt5.mainmodul
Imports System.Text.RegularExpressions
Public Class datei_import
    'Doppelte entfernen?
    'drop table temp;
    'select * into temp from rech1;
    'delete * from rech1;
    'alter table rech1 ALTER COLUMN XAUFTR_NR DOUBLE UNIQUE;
    'insert  into rech1 select * from temp;
    Private g_AppPath As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub LoadData(ByVal dbf_filename As String)
        Cursor = Cursors.WaitCursor
        'use the path to the dbf file for the connection string
        Dim sConn As String = "Driver={Microsoft dBase Driver (*.dbf)};DBQ=" & mainmodul.GetDir(dbf_filename) 'c:\TestDB\"
        Dim cn As New OdbcConnection(sConn)
        'for dbase files use the dbf filename
        Dim sSQL As String = "select * from " & getName(dbf_filename)
        Dim da As New OdbcDataAdapter(sSQL, cn)
        Dim ds As New DataSet
        Dim table As String
        table = getNameOnly(getName(dbf_filename))
        'dbase files only have one table, where the name is the same as the filename
        da.Fill(ds, table) '"MyTable")
        dg.DataSource = ds
        dg.DataMember = table '"MyTable"
        Cursor = Cursors.Default

        Dim dt As New DataTable
        dt = dg.DataSource.Tables(0)
        Dim dc As New DataColumn
        Dim t As String
        Dim unique As Boolean
        Dim allowDBNull As Boolean
        Dim type As System.Type
        Dim flen As Integer
        For Each dc In dt.Columns
            t = dc.ColumnName.ToString
            unique = dc.Unique
            allowDBNull = dc.AllowDBNull
            type = dc.DataType
            flen = dc.MaxLength
        Next
        'Dim lmax As Integer
        'Dim dr As DataRow
        'For Each dr In dt.Rows
        '    lmax = dr.ToString.Length
        'Next

    End Sub
    Private Sub m_datei_open_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles m_datei_open.Click
        With OpenFileDialog1
            .Filter = "DBASE Dateien (*.dbf)|*.dbf|Alle Dateien (*.*)|*.*"
            .FilterIndex = 0
            .InitialDirectory = Environment.CurrentDirectory ' KBfaktMain.g_AppPath
            .RestoreDirectory = True
            .FileName = "*.dbf"
        End With
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = OpenFileDialog1.FileName
            LoadData(TextBox1.Text)
        End If

    End Sub

    Private Sub btn_Import_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Import.Click
        Try
            System.IO.File.Delete(getAppDir() + "import-log.txt")
        Catch
        End Try
        Dim dbname As String
        If System.IO.File.Exists(TextBox1.Text) Then
            dbname = mainmodul.getNameOnly(mainmodul.getName(TextBox1.Text))
            createDatabase(dbname, False)
        End If
    End Sub
    Private Sub createDatabase(ByVal DataBase As String, ByVal auto As Boolean)
        'DROP TABLE <TABLENAME> 
        'Importing a dBase III file: 

        'Dim AccessConn As New System.Data.OleDb.OleDbConnection(My.Settings.kbfaktConnectionString)
        Dim AccessConn As New System.Data.OleDb.OleDbConnection
        OpenDBConnection(AccessConn)
        Dim dbasePath As String
        dbasePath = mainmodul.GetDir(TextBox1.Text)
        Dim dropTable As Boolean = False

        Dim AccessCommand As New System.Data.OleDb.OleDbCommand
        Dim c As Integer
        AccessCommand.Connection = AccessConn
        If auto = False Then
            Dim ant As Integer = MessageBox.Show("Evtl. vorhanden Tabelle löschen?", DataBase, MessageBoxButtons.YesNoCancel)
            If ant = Windows.Forms.DialogResult.Cancel Then
                AccessConn.Close()
                Exit Sub
            End If
            If ant = Windows.Forms.DialogResult.Yes Then
                AccessCommand.CommandText = _
                "DROP TABLE [" + DataBase + "]"
                Try
                    c = AccessCommand.ExecuteNonQuery()
                    AddLog("Drop Table =" & c & " Daten entfernt <-" & DataBase)
                    dropTable = True
                Catch ex1 As OleDbException
                    AddLog(ex1.Message & " ### Fehler")
                    dropTable = True
                End Try
            End If
        Else
            AccessCommand.CommandText = _
            "DROP TABLE [" + DataBase + "]"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog("Drop Table =" & c & " Daten entfernt <-" & DataBase)
                dropTable = True
            Catch ex1 As OleDbException
                AddLog(ex1.Message & " ### Fehler")
                dropTable = True
            End Try
        End If
        'setup the fields used for import
        Dim fieldset As String = "*"
        If DataBase.ToUpper = "RECH1" Then fieldset = fieldsRech1
        If DataBase.ToUpper = "RECH2" Then fieldset = fieldsRech2
        If DataBase.ToUpper = "KUNDFAHR" Then fieldset = fieldsKundfahr
        If DataBase.ToUpper = "ARTSTAMM" Then fieldset = fieldsArtStamm
        If DataBase.ToUpper = "KUNDENST" Then fieldset = fieldsKundenSt
        If DataBase.ToUpper = "FIRMSTAM" Then fieldset = fieldsFirmStam
        If DataBase.ToUpper = "LOHNART" Then fieldset = fieldsLohnart
        If dropTable Then
            AccessCommand.CommandText = _
            "SELECT " & fieldset & " INTO [" & DataBase & "] FROM [dBase IV;DATABASE=" & _
            dbasePath + "].[" + _
            DataBase + "]"
        Else
            AccessCommand.CommandText = _
            "INSERT " & fieldset & " INTO [" & DataBase & "] FROM [dBase IV;DATABASE=" & _
            dbasePath + "].[" + _
            DataBase + "]"
        End If
        Try
            c = AccessCommand.ExecuteNonQuery()
            AddLog("Import von " & c & " Daten abgeschlossen <-" & DataBase)
        Catch ex2 As OleDbException
            AddLog(ex2.Message & " ### Fehler")
        End Try
        'add an primary key field
        AccessCommand.CommandText = "ALTER table " & DataBase & " ADD COLUMN id IDENTITY(1,1) PRIMARY KEY"
        Try
            c = AccessCommand.ExecuteNonQuery()
            AddLog("ALTER table ADD Column id für " & c & " abgeschlossen <-" & DataBase)
        Catch ex2 As OleDbException
            AddLog(ex2.Message & " ### Fehler")
        End Try

        'FirmStam Patch adds a column FussText
        If DataBase.ToUpper = "FIRMSTAM" Then
            Patch1FirmStam()
        End If

        'bei RECH1 neue Felder für Gedruckt und Werkdatum anhängen
        'neues Feld für verwendeten MwSt-Satz anhängen
        If DataBase.ToUpper = "RECH1" Then
            'add gedruckt column
            AccessCommand.CommandText = "ALTER table " & DataBase & " ADD COLUMN GEDRUCKT YESNO DEFAULT FALSE"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog("ALTER table ADD Column Gedruckt abgeschlossen <-" & TextBox1.Text)
            Catch ex2 As OleDbException
                AddLog(ex2.Message & " ALTER table ADD Column Gedruckt ### Fehler")
            End Try
            'add werkdatum column
            AccessCommand.CommandText = "ALTER table " & DataBase & " ADD COLUMN WERKDATUM DATETIME"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog("ALTER table ADD COLUMN WERKDATUM abgeschlossen <-" & TextBox1.Text)
            Catch ex2 As OleDbException
                AddLog(ex2.Message & " ALTER table ADD COLUMN WERKDATUM ### Fehler")
            End Try
            'add MwStSatz column
            AccessCommand.CommandText = "ALTER table " & DataBase & " ADD MwStSatz INTEGER DEFAULT 16"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog(AccessCommand.CommandText & " erfolgreich <-" & TextBox1.Text)
            Catch ex2 As OleDbException
                AddLog(ex2.Message & AccessCommand.CommandText & " ### Fehler")
            End Try
            'Datum korrigieren wenn vor 1910 (1905 statt 2005)
            'UPDATE RECH1 SET XDatum=DateAdd("yyyy", +100, xdatum) WHERE YEAR(XDatum)<1910;
            AccessCommand.CommandText = "UPDATE RECH1 SET XDatum=DateAdd(""yyyy"", +100, xdatum) WHERE YEAR(XDatum)<1910;"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog(AccessCommand.CommandText & " erfolgreich <-" & TextBox1.Text)
            Catch ex2 As OleDbException
                AddLog(ex2.Message & AccessCommand.CommandText & " ### Fehler")
            End Try
            'Alles als Gedruckt setzen
            AccessCommand.CommandText = "UPDATE RECH1 SET Gedruckt=True;"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog(AccessCommand.CommandText & " erfolgreich <-" & TextBox1.Text)
            Catch ex2 As OleDbException
                AddLog(ex2.Message & AccessCommand.CommandText & " ### Fehler")
            End Try
            'change MwSt for all data before and after 2006
            Try
                AccessCommand.CommandText = "UPDATE RECH1 SET MWSTsatz=16 where year(XDatum)<2007"
                c = AccessCommand.ExecuteNonQuery
                AddLog(AccessCommand.CommandText & " erfolgreich" & vbCrLf & c & " Daten geändert")
            Catch ex As OleDbException
                AddLog(AccessCommand.CommandText & " FEHLER" & vbCrLf & ex.Message)
            End Try
            Try
                AccessCommand.CommandText = "UPDATE RECH1 SET MWSTsatz=19 where year(XDatum)>2006"
                c = AccessCommand.ExecuteNonQuery
                AddLog(AccessCommand.CommandText & " erfolgreich" & vbCrLf & c & " Daten geändert")
            Catch ex As OleDbException
                AddLog(AccessCommand.CommandText & " FEHLER" & vbCrLf & ex.Message)
            End Try
            Patch2Rech1()
            Try
                AccessCommand.CommandText = "UPDATE RECH1 SET Bezahlt=TRUE"
                c = AccessCommand.ExecuteNonQuery
                AddLog(AccessCommand.CommandText & " erfolgreich" & vbCrLf & c & " Daten geändert")
            Catch ex As OleDbException
                AddLog(AccessCommand.CommandText & " FEHLER" & vbCrLf & ex.Message)
            End Try
        End If
        '########## Rechnungspositionen
        If DataBase.ToUpper = "RECH2" Then
            AccessCommand.CommandText = "ALTER table " & DataBase & " ADD COLUMN pos INTEGER"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog("ALTER table ADD Column position abgeschlossen <-" & TextBox1.Text)
            Catch ex2 As OleDbException
                AddLog(ex2.Message & " ALTER table ADD Column position ### Fehler")
            End Try
            'change all POS to 0
            AccessCommand.CommandText = "UPDATE " & DataBase & " SET pos=0"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog("UPDATE " & DataBase & " SET pos=0 abgeschlossen <-" & TextBox1.Text)
            Catch ex2 As OleDbException
                AddLog(ex2.Message & " UPDATE " & DataBase & " SET pos=0 ### Fehler")
            End Try
            'add a field for the type
            AccessCommand.CommandText = "ALTER table " & DataBase & " ADD COLUMN [ArtTyp] INTEGER DEFAULT 0"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog(AccessCommand.CommandText & " abgeschlossen")
            Catch ex2 As OleDbException
                AddLog(ex2.Message & " " & AccessCommand.CommandText & " ### Fehler")
            End Try
            'change all ArtTyp to 0 if not 
            AccessCommand.CommandText = "UPDATE " & DataBase & " SET [ArtTyp]=0"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog(AccessCommand.CommandText & " abgeschlossen")
            Catch ex2 As OleDbException
                AddLog(ex2.Message & " " & AccessCommand.CommandText & " ### Fehler")
            End Try
            'change ArtTyp for artnr starting with 500-
            'we have to use % instead of * for matching everything in ADO jet-sql!
            AccessCommand.CommandText = "update " & DataBase & " SET [ArtTyp]=1 where ARTIKELNR IN (select artikelnr from Rech2 where artikelnr LIKE '500-%')"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog(AccessCommand.CommandText & " abgeschlossen ")
            Catch ex2 As OleDbException
                AddLog(ex2.Message & AccessCommand.CommandText & " ### Fehler")
            End Try
            'change ArtTyp for artnr starting with ZEIT
            'we have to use % instead of * for matching everything in ADO jet-sql!
            AccessCommand.CommandText = "update " & DataBase & " SET [ArtTyp]=2 where ARTIKELNR IN (select artikelnr from rech2 where artikelnr LIKE 'ZEIT%')"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog(AccessCommand.CommandText & " abgeschlossen ")
            Catch ex2 As OleDbException
                AddLog(ex2.Message & AccessCommand.CommandText & " ### Fehler")
            End Try
        End If
        '############ ARTSTAMM
        'position feld in ARTSTAMM anhängen
        If DataBase.ToUpper = "ARTSTAMM" Then
            'ArtikelNr Feld auf 17 Zeichen, wie in RECH2, vergrössern
            AccessCommand.CommandText = "ALTER table " & DataBase & " MODIFY [ArtikelNr] char(17)"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog("MODIFY [ArtikelNr] char(17) für " & DataBase & " abgeschlossen <-" & TextBox1.Text)
            Catch ex2 As OleDbException
                AddLog(ex2.Message & " MODIFY [ArtikelNr] char(17) für " & DataBase & " ### Fehler")
            End Try

            'ArtikelBez Feld auf 60 Zeichen vergrössern, wie in Rech2
            AccessCommand.CommandText = "ALTER table " & DataBase & " MODIFY [ArtikelBez] char(60)"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog("MODIFY [ArtikelBez] char(60) für " & DataBase & " abgeschlossen <-" & TextBox1.Text)
            Catch ex2 As OleDbException
                AddLog(ex2.Message & " MODIFY [ArtikelBez] char(60) für " & DataBase & " ### Fehler")
            End Try

            'add a field for the type
            AccessCommand.CommandText = "ALTER table " & DataBase & " ADD COLUMN [ArtTyp] INTEGER DEFAULT 0"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog("ALTER table ADD Column [ArtTyp] für " & DataBase & " abgeschlossen <-" & TextBox1.Text)
            Catch ex2 As OleDbException
                AddLog(ex2.Message & " ALTER table ADD Column [ArtTyp] für " & DataBase & " ### Fehler")
            End Try
            'Apply default ArtTyp=0
            AccessCommand.CommandText = "update " & DataBase & " SET [ArtTyp]=0"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog(AccessCommand.CommandText & " abgeschlossen <-" & TextBox1.Text)
            Catch ex2 As OleDbException
                AddLog(ex2.Message & AccessCommand.CommandText & " ### Fehler")
            End Try
            'Change NULL values in Bestand
            AccessCommand.CommandText = "update " & DataBase & " SET Bestand=0 where Bestand IS NULL"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog(AccessCommand.CommandText & " abgeschlossen <-" & TextBox1.Text)
            Catch ex2 As OleDbException
                AddLog(ex2.Message & AccessCommand.CommandText & " ### Fehler")
            End Try

            'change ArtTyp for artnr starting with 500-
            'we have to use % instead of * for matching everything in ADO jet-sql!
            AccessCommand.CommandText = "update " & DataBase & " SET [ArtTyp]=1 where ARTIKELNR IN (select artikelnr from artstamm where artikelnr LIKE '500-%')"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog(AccessCommand.CommandText & " abgeschlossen <-" & TextBox1.Text)
            Catch ex2 As OleDbException
                AddLog(ex2.Message & AccessCommand.CommandText & " ### Fehler")
            End Try
            'change ArtTyp for artnr starting with ZEIT
            'we have to use % instead of * for matching everything in ADO jet-sql!
            AccessCommand.CommandText = "update " & DataBase & " SET [ArtTyp]=2 where ARTIKELNR IN (select artikelnr from artstamm where artikelnr LIKE 'ZEIT%')"
            Try
                c = AccessCommand.ExecuteNonQuery()
                AddLog(AccessCommand.CommandText & " abgeschlossen <-" & TextBox1.Text)
            Catch ex2 As OleDbException
                AddLog(ex2.Message & AccessCommand.CommandText & " ### Fehler")
            End Try
            Patch1ArtStamm()
        End If
        AccessConn.Close()
    End Sub
    Private Sub datei_import_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        g_AppPath = Environment.CurrentDirectory
    End Sub
    Private Sub AddLog(ByVal s As String)
        ListBox1.Items.Add(s)
        ListBox1.SelectedIndex = ListBox1.Items.Count - 1
        Using w As System.IO.StreamWriter = System.IO.File.AppendText(getAppDir() + "import-log.txt")
            w.WriteLine(s)
            ' Close the writer and underlying file.
            w.Flush()
            w.Close()
        End Using
        Application.DoEvents()
    End Sub
    Private Sub CreateTypTabelle()
        Dim AccessConn As New System.Data.OleDb.OleDbConnection
        'get an already opened OleDbConnection to our access mdb
        OpenDBConnection(AccessConn)
        Dim AccessCommand As New System.Data.OleDb.OleDbCommand
        Dim c As Integer
        AccessCommand.Connection = AccessConn
        'delete an existing ANREDEN table
        AccessCommand.CommandText = "DROP table ARTTYPEN" ' & DataBase & " ADD COLUMN id IDENTITY(1,1) PRIMARY KEY"
        Try
            c = AccessCommand.ExecuteNonQuery()
            AddLog("DROP table ARTTYPEN abgeschlossen")
        Catch ex2 As OleDbException
            AddLog("### DROP table ARTTYPEN - Fehler: " & ex2.Message)
        End Try
        'create a new ANREDEN table
        AccessCommand.CommandText = "CREATE table ARTTYPEN (id INTEGER PRIMARY KEY, ArtikelTyp TEXT(20))" ' & DataBase & " ADD COLUMN id IDENTITY(1,1) PRIMARY KEY"
        Try
            c = AccessCommand.ExecuteNonQuery()
            AddLog("CREATE table ARTTYPEN abgeschlossen")
        Catch ex2 As OleDbException
            AddLog("### CREATE table ARTTYPEN - Fehler: " & ex2.Message)
        End Try
        'insert data into ANREDEN table
        Dim ix As Integer
        'AccessCommand.CommandText = "INSERT into ANREDEN (id, text) VALUES (id, 'text')"
        Try
            For ix = 0 To ArtTypen.Length - 1
                AccessCommand.CommandText = String.Format("INSERT into ARTTYPEN (id, ArtikelTyp) VALUES ({0}, '{1}')", ix, ArtTypen(ix))
                c = AccessCommand.ExecuteNonQuery()
            Next
            AddLog("CREATE table ARTTYPEN abgeschlossen")
        Catch ex2 As OleDbException
            AddLog("### CREATE table ARTTYPEN - Fehler bei ARTTYPEN(" & ix & "): " & ex2.Message)
        End Try
        AccessConn.Close()

    End Sub
    Private Sub CreateAnredenTabelle()
        Dim AccessConn As New System.Data.OleDb.OleDbConnection
        'get an already opened OleDbConnection to our access mdb
        OpenDBConnection(AccessConn)
        Dim AccessCommand As New System.Data.OleDb.OleDbCommand
        Dim c As Integer
        AccessCommand.Connection = AccessConn
        'delete an existing ANREDEN table
        AccessCommand.CommandText = "DROP table ANREDEN" ' & DataBase & " ADD COLUMN id IDENTITY(1,1) PRIMARY KEY"
        Try
            c = AccessCommand.ExecuteNonQuery()
            AddLog("DROP table ANREDEN abgeschlossen")
        Catch ex2 As OleDbException
            AddLog("### DROP table ANREDEN - Fehler: " & ex2.Message)
        End Try
        'create a new ANREDEN table
        AccessCommand.CommandText = "CREATE table ANREDEN (id INTEGER PRIMARY KEY, Anrede TEXT(20))" ' & DataBase & " ADD COLUMN id IDENTITY(1,1) PRIMARY KEY"
        Try
            c = AccessCommand.ExecuteNonQuery()
            AddLog("CREATE table ANREDEN abgeschlossen")
        Catch ex2 As OleDbException
            AddLog("### CREATE table ANREDEN - Fehler: " & ex2.Message)
        End Try
        'insert data into ANREDEN table
        Dim ix As Integer
        'AccessCommand.CommandText = "INSERT into ANREDEN (id, text) VALUES (id, 'text')"
        Try
            For ix = 0 To 6
                AccessCommand.CommandText = String.Format("INSERT into ANREDEN (id, anrede) VALUES ({0}, '{1}')", ix, anreden(ix))
                c = AccessCommand.ExecuteNonQuery()
            Next
            AddLog("CREATE table ANREDEN abgeschlossen")
        Catch ex2 As OleDbException
            AddLog("### CREATE table ANREDEN - Fehler bei anreden(" & ix & "): " & ex2.Message)
        End Try
        AccessConn.Close()
    End Sub

    Private Sub btnCreateAnreden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateAnreden.Click
        CreateAnredenTabelle()
    End Sub
    Private Sub btnImportAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportAll.Click
        If MessageBox.Show("Wirklich alles neu importieren?", "Alles importieren", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Cancel Then Exit Sub
        'we need the path to the dbf files first
        With OpenFileDialog1
            .Filter = "RECH1 (Rech1.dbf)|RECH1.DBF|DBASE Dateien (*.dbf)|*.dbf|Alle Dateien (*.*)|*.*"
            .FilterIndex = 1
            .InitialDirectory = Environment.CurrentDirectory ' KBfaktMain.g_AppPath
            .RestoreDirectory = True
            .FileName = "Rech1.dbf"
        End With
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = OpenFileDialog1.FileName
            LoadData(TextBox1.Text)
        Else
            Exit Sub
        End If
        Try
            System.IO.File.Delete(getAppDir() + "import-log.txt")
        Catch
        End Try

        'step thru all known dbf files
        Dim s
        For Each s In tableList
            createDatabase(s, True)
        Next s
        CreateAnredenTabelle()
        CreateTypTabelle()
        RemoveDupesRech1()
        FixRech2()
        'Richtwerte

        If MessageBox.Show("Zeit-Artikel aus Rech2 in ArtStamm übernehmen?", "UpdateArtstammFromRech2_Zeit()", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then _
            UpdateArtstammFromRech2_ZeitExtended()
        'UpdateArtstammFromRech2_Zeit()
        If MessageBox.Show("Artikel aus Rech2 in ArtStamm übernehmen?", "UpdateArtStammFromRech2_ohneZeit()", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then _
            UpdateArtStammFromRech2_ohneZeit()
        If MessageBox.Show("ArtStamm Artikel mit * und + korrigieren?", "CleanUpArtStamm()", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then _
            CleanUpArtStamm()
        If MessageBox.Show("ArtStamm Artikel mit FzgTyp ergänzen?", "UpdateArtStammFzgTyp()", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then _
            UpdateArtStammFzgTyp()
        If MessageBox.Show("Umlaute korrigieren?", "UmlauteKorrigierenALLE()", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then _
            UmlauteKorrigierenALLE()
        If MessageBox.Show("Richtwerte umwandeln?", "RichtwerteImport()", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            CreateRichtwerteTables()
            RichtwerteImport()
        End If
        AddLog("### ImportALL() finished ###")
    End Sub
    Public Function CleanUpArtStamm() As Long
        '"update ArtStamm set ArtikelNr=mid(artikelnr,1,Len(artikelnr)-1) where artikelnr LIKE '[0-9a-zA-Z]%[*+]'"
        Dim cn As New System.Data.OleDb.OleDbConnection
        Dim cmd As New System.Data.OleDb.OleDbCommand
        Dim cmd2 As New System.Data.OleDb.OleDbCommand
        Dim c As Long = 0
        'delete all with trailing + or *
        Dim sql As String = "delete from ArtStamm where artikelnr LIKE '[0-9a-zA-Z]%[*+]' "
        'remove trailing + or *
        '"update ArtStamm set ArtikelNr=mid(artikelnr,1,Len(artikelnr)-1) where artikelnr LIKE '[0-9a-zA-Z]%[*+]' "
        AddLog("############## CleanUpArtStamm ###############")
        Try
            OpenDBConnection(cn)
            cmd.Connection = cn
            cmd.CommandText = sql
            c = cmd.ExecuteNonQuery
            AddLog("    Anzahl korrigierter Datensätze=" & c)
            MessageBox.Show("Anzahl korrigierter Artikel: " & c, "CleanUpArtStamm")
        Catch ex As Exception
            AddLog("!Exception: " & ex.Message)
        Finally
            CleanUpArtStamm = c
            cn.Close()
        End Try
    End Function
    ''' <summary>
    ''' ArtStamm Daten, Feld FzgTyp mit Wert aus Rech1:XTyp ergänzen
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateArtStammFzgTyp() As Long
        ShowWaitCursor(True)
        Dim cn As New System.Data.OleDb.OleDbConnection
        Dim rdrRech2 As OleDbDataReader = Nothing
        Dim rdrCmd2 As New System.Data.OleDb.OleDbCommand
        Dim anz As Long = 0

        Dim cmd2 As New System.Data.OleDb.OleDbCommand
        Dim c As Long = 0
        'read thru Rech2 :AuftrNr and ArtikelNr
        'try to find ArtkelNR in ArtStamm, if found read Rech1:XTyp
        'and write ArtStamm:FzgTyp
        '
        'Dim sql As String = "INSERT into artstamm select distinct ArtikelBez, artikelnr, E_Preis as VK, ArtTyp FROM  RECH2 where ArtikelNr LIKE 'ZEIT%' AND E_Preis>0"
        'Dim sql As String = "SELECT distinct RECH2.ARTIKELNR, RECH1.XTYP INTO TEMP2 FROM RECH2 INNER JOIN RECH1 ON RECH2.AUFTR_NR = RECH1.XAUFTR_NR  where Right(RECH2.ARTIKELNR, 1)<>'*' AND Right(RECH2.ARTIKELNR, 1)<>'+'"
        AddLog("## Übernahme von nicht ZEIT Artikeln in ArtStamm ##")
        Try
            OpenDBConnection(cn)
            rdrCmd2.Connection = cn
            cmd2.Connection = cn

            cmd2.CommandText = "Select * from ArtStamm where FzgTyp=''"
            c = cmd2.ExecuteNonQuery()
            If c = 0 Then Throw New System.Exception("Alle ArtStamm Daten haben schon einen FzgTyp")
            'delete temp2 table
            Try
                cmd2.CommandText = "Drop table temp2"
                cmd2.ExecuteNonQuery()
                AddLog("->DropTable() für temp2 OK")
            Catch ox As OleDbException
                AddLog("->DropTable() für temp2 error: " & ox.Message)
            End Try
            'create a temp2 table with distinct values
            rdrCmd2.CommandText = "SELECT distinct RECH2.ARTIKELNR, RECH1.XTYP INTO TEMP2 FROM RECH2 INNER JOIN RECH1 ON RECH2.AUFTR_NR = RECH1.XAUFTR_NR  where Right(RECH2.ARTIKELNR, 1)<>'*' AND Right(RECH2.ARTIKELNR, 1)<>'+'"
            c = rdrCmd2.ExecuteNonQuery()
            AddLog("->Eindeutig gefundene ArtikelNR-XTyp Daten:" & c)

            'read distinct ArtNr from temp2
            rdrCmd2.CommandText = "SELECT XTYP, ArtikelNr from TEMP2 where len(ArtikelNr)>1 AND RIGHT(ArtikelNR,1)<>'*' AND RIGHT(ArtikelNR,1)<>'+' AND LEFT(ArtikelNr,1)<>'*' order by ArtikelNr"
            '"SELECT XTYP, ArtikelNr from TEMP2 order by ArtikelNr where len(ArtikelNr)>1"
            '"SELECT XTYP, ArtikelNr from TEMP2 where len(ArtikelNr)>1 AND RIGHT(ArtikelNR,1)<>'*' AND RIGHT(ArtikelNR,1)<>'+' order by ArtikelNr"
            rdrRech2 = rdrCmd2.ExecuteReader()

            While rdrRech2.Read
                cmd2.CommandText = "update ArtStamm SET FzgTyp='" & rdrRech2.Item("XTYP").ToString & "' where ArtikelNr='" & rdrRech2.Item("ArtikelNr").ToString & "' AND FzgTyp=''"
                anz = anz + cmd2.ExecuteNonQuery()
                Application.DoEvents()
                If anz Mod 100 = 0 Then AddLog("... updating ...(" & anz & ")")
            End While
            AddLog("->Anzahl aktualisierter Datensätze=" & anz)
        Catch ex As Exception
            AddLog("!Exception: " & ex.Message)
        Finally
            UpdateArtStammFzgTyp = anz
            If Not IsNothing(rdrRech2) Then rdrRech2.Close()
            cn.Close()
            '"INSERT  into artstamm select distinct artikelnr, ArtikelBez, E_Preis as VK, ArtTyp FROM  RECH2 where ArtikelNr LIKE 'ZEIT%' AND E_Preis>0"
            ShowWaitCursor(False)
        End Try

    End Function
    Public Function UpdateArtStammFromRech2_ohneZeit() As Long
        'select ArtikelNr, ArtikelBez, E_Preis from rech2 where ArtikelNr in (select distinct ArtikelNr from Rech2) and ArtikelNr not like("ZEIT%")
        Dim cn As New System.Data.OleDb.OleDbConnection
        Dim cmd As New System.Data.OleDb.OleDbCommand
        Dim cmd2 As New System.Data.OleDb.OleDbCommand
        Dim c As Long = 0
        'Dim sql As String = "INSERT into artstamm select distinct ArtikelBez, artikelnr, E_Preis as VK, ArtTyp FROM  RECH2 where ArtikelNr LIKE 'ZEIT%' AND E_Preis>0"
        Dim sql As String = "INSERT INTO ArtStamm select distinct ArtikelBez, artikelnr, E_Preis as VK, ArtTyp FROM  RECH2 where ArtikelNr Not LIKE 'ZEIT%' AND E_Preis>0"
        AddLog("## Übernahme von nicht ZEIT Artikeln in ArtStamm ##")
        Try
            OpenDBConnection(cn)
            cmd.Connection = cn
            cmd.CommandText = sql
            c = cmd.ExecuteNonQuery
            AddLog("    Anzahl kopierter Datensätze=" & c)
        Catch ex As Exception
            AddLog("!Exception: " & ex.Message)
        Finally
            UpdateArtStammFromRech2_ohneZeit = c
            cn.Close()
            '"INSERT  into artstamm select distinct artikelnr, ArtikelBez, E_Preis as VK, ArtTyp FROM  RECH2 where ArtikelNr LIKE 'ZEIT%' AND E_Preis>0"
        End Try

    End Function
    ''' <summary>
    ''' Zeit-Daten aus Rechnungen in den Artikelstamm übernehmen
    ''' wobei ArtikelNr und ArtikelBez abgeschnitten werden!
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateArtstammFromRech2_Zeit() As Long
        Dim cn As New System.Data.OleDb.OleDbConnection
        Dim cmd As New System.Data.OleDb.OleDbCommand
        Dim cmd2 As New System.Data.OleDb.OleDbCommand
        Dim c As Long = 0
        'Dim sql As String = "INSERT into artstamm select distinct ArtikelBez, artikelnr, E_Preis as VK, ArtTyp FROM  RECH2 where ArtikelNr LIKE 'ZEIT%' AND E_Preis>0"
        'Problem ArtikelNr in ArtStamm = 15 Zeichen in Rech2 aber 17 Zeichen
        'Artstamm->ArtikelBez = 30 Zeichen, in Rech2 aber 60 Zeichen
        '"Alter Table ArtStamm ALTER column ArtikelNr Text(17) 
        '"Alter Table ArtStamm ALTER COLUMN ArtikelBez Text(60)
        '"select DISTINCT Left(ArtikelBez,30) as ArtikelBez30, left (artikelnr, 15) as ArtikelNr15, E_Preis as VK, ArtTyp INTO TEMP1 FROM  RECH2 where ArtikelNr like 'ZEIT%' and E_Preis>0"
        '"INSERT INTO ArtStamm SELECT ArtikelNr15 as ArtikelNr, ArtikelBez30 as ArtikelBez, VK, ArtTyp from temp1"
        Dim sql As String
        '= "INSERT INTO ArtStamm select distinct ArtikelBez, artikelnr, E_Preis as VK, ArtTyp FROM  RECH2 where ArtikelNr LIKE 'ZEIT%' AND E_Preis>0"
        AddLog("## Übernahme von ZEIT Artikeln in ArtStamm ##")
        Try
            OpenDBConnection(cn)
            cmd.Connection = cn
            AddLog("Lösche TEMP1")
            sql = "DROP table temp1"
            cmd.CommandText = sql
            c = cmd.ExecuteNonQuery
            AddLog("    Temp1 gelöscht")
            AddLog("Kopiere ZEIT Artikel aus RECH2 nach TEMP1")
            sql = "select DISTINCT Left(ArtikelBez,30) as ArtikelBez30, left (artikelnr, 15) as ArtikelNr15, E_Preis as VK, ArtTyp INTO TEMP1 FROM  RECH2 where ArtikelNr like 'ZEIT%' and E_Preis>0"
            cmd.CommandText = sql
            c = cmd.ExecuteNonQuery
            AddLog("    Anzahl kopierter Datensätze=" & c)
            AddLog("Kopiere Daten aus TEMP1 nach ArtStamm")
            sql = "INSERT INTO ArtStamm SELECT ArtikelNr15 as ArtikelNr, ArtikelBez30 as ArtikelBez, VK, ArtTyp from temp1"
            cmd.CommandText = sql
            c = cmd.ExecuteNonQuery
            AddLog("    Anzahl kopierter Datensätze=" & c)
        Catch ex As Exception
            AddLog("!Exception: " & ex.Message)
        Finally
            UpdateArtstammFromRech2_Zeit = c
            cn.Close()
            '"INSERT  into artstamm select distinct artikelnr, ArtikelBez, E_Preis as VK, ArtTyp FROM  RECH2 where ArtikelNr LIKE 'ZEIT%' AND E_Preis>0"
        End Try
    End Function
    ''' <summary>
    ''' Zeit-Daten aus Rechnungen in den Artikelstamm übernehmen
    ''' wobei ArtikelNr und ArtikelBez NICHT abgeschnitten werden!
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateArtstammFromRech2_ZeitExtended() As Long
        Dim cn As New System.Data.OleDb.OleDbConnection
        Dim cmd As New System.Data.OleDb.OleDbCommand
        Dim cmd2 As New System.Data.OleDb.OleDbCommand
        Dim c As Long = 0
        'Dim sql As String = "INSERT into artstamm select distinct ArtikelBez, artikelnr, E_Preis as VK, ArtTyp FROM  RECH2 where ArtikelNr LIKE 'ZEIT%' AND E_Preis>0"
        'Problem ArtikelNr in ArtStamm = 15 Zeichen in Rech2 aber 17 Zeichen
        'Artstamm->ArtikelBez = 30 Zeichen, in Rech2 aber 60 Zeichen
        '"Alter Table ArtStamm ALTER column ArtikelNr Text(17) 
        '"Alter Table ArtStamm ALTER COLUMN ArtikelBez Text(60)
        '"select DISTINCT Left(ArtikelBez,30) as ArtikelBez30, left (artikelnr, 15) as ArtikelNr15, E_Preis as VK, ArtTyp INTO TEMP1 FROM  RECH2 where ArtikelNr like 'ZEIT%' and E_Preis>0"
        '"INSERT INTO ArtStamm SELECT ArtikelNr15 as ArtikelNr, ArtikelBez30 as ArtikelBez, VK, ArtTyp from temp1"
        Dim sql As String
        '= "INSERT INTO ArtStamm select distinct ArtikelBez, artikelnr, E_Preis as VK, ArtTyp FROM  RECH2 where ArtikelNr LIKE 'ZEIT%' AND E_Preis>0"
        AddLog("## Übernahme von ZEIT Artikeln in ArtStamm ##")
        Try
            OpenDBConnection(cn)
            cmd.Connection = cn
            AddLog("Grössenanpassung ArtStamm ArtikelNr 17 Zeichen")
            sql = "Alter Table ArtStamm ALTER column ArtikelNr Text(17)"
            cmd.CommandText = sql
            c = cmd.ExecuteNonQuery
            AddLog("    Anzahl geänderter Datensätze=" & c)
            AddLog("Grössenänderung ArtStamm ArtikelBez 60 Zeichen")
            sql = "Alter Table ArtStamm ALTER COLUMN ArtikelBez Text(60)"
            cmd.CommandText = sql
            c = cmd.ExecuteNonQuery
            AddLog("    Anzahl geänderter Datensätze=" & c)
            AddLog("Übernahme der Zeit-Artikel aus RECH2 nach ArtStamm")
            'change in 1.4.0.0:
            'LIKE 'ZEIT%' is now LIKE 'ZEIT'
            sql = "INSERT INTO ArtStamm select distinct ArtikelBez, artikelnr, E_Preis as VK, ArtTyp FROM  RECH2 where ArtikelNr LIKE 'ZEIT' AND E_Preis>0"
            cmd.CommandText = sql
            c = cmd.ExecuteNonQuery
            AddLog("    Anzahl kopierter Datensätze=" & c)
        Catch ex As Exception
            AddLog("!Exception: " & ex.Message)
        Finally
            UpdateArtstammFromRech2_ZeitExtended = c
            cn.Close()
            '"INSERT  into artstamm select distinct artikelnr, ArtikelBez, E_Preis as VK, ArtTyp FROM  RECH2 where ArtikelNr LIKE 'ZEIT%' AND E_Preis>0"
        End Try
    End Function
    Public Sub FixRech2()
        Dim cn As New System.Data.OleDb.OleDbConnection
        Dim cmd As New System.Data.OleDb.OleDbCommand
        Dim c As Long = 0
        Dim sql As String = "Select * from RECH2 order by AUFTR_NR"
        Dim lastAuftrNr As Double = 0
        'get an already opened OleDbConnection to our access mdb
        OpenDBConnection(cn)
        cmd.Connection = cn
        'we need a dataAdapter
        Dim dap As New OleDb.OleDbDataAdapter(sql, cn)
        Dim dr As DataRow
        Dim tbl As New DataTable
        Dim cmb As New OleDbCommandBuilder(dap)
        dap.Fill(tbl)
        For Each dr In tbl.Rows
            If dr.Item("AUFTR_NR").ToString = "" Then
                dr.Delete()
                c = c + 1
            End If
        Next
        If c > 0 Then
            If MessageBox.Show(c & " defekte RECH2-Datensätze (ohne AUFTRNR) gefunden. Löschen?", "FixRech2", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                dap.Update(tbl)
            End If
        Else
            MessageBox.Show("Keine Probleme mit AUFTRNR in Rech2 gefunden")
        End If
        cn.Close()
    End Sub
    Public Sub RemoveDupesRech1()
        Dim cn As New System.Data.OleDb.OleDbConnection
        Dim cmd As New System.Data.OleDb.OleDbCommand
        Dim c As Long = 0
        Dim sql As String = "Select * from RECH1 order by xauftr_nr"
        Dim lastAuftrNr As Double = 0
        'get an already opened OleDbConnection to our access mdb
        OpenDBConnection(cn)
        cmd.Connection = cn
        'we need a dataAdapter
        Dim dap As New OleDb.OleDbDataAdapter(sql, cn)
        Dim dr As DataRow
        Dim tbl As New DataTable
        Dim cmb As New OleDbCommandBuilder(dap)
        dap.Fill(tbl)
        For Each dr In tbl.Rows
            If dr.Item("XAUFTR_NR") = lastAuftrNr Then
                dr.Delete()
                c = c + 1
                Continue For
            End If
            lastAuftrNr = dr.Item("XAUFTR_NR")
        Next
        If c > 0 Then
            If MessageBox.Show(c & " doppelte Daten gefunden. Löschen?", "Doppelte entfernen", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                dap.Update(tbl)
            End If
        Else
            MessageBox.Show("Keine doppelte Daten gefunden.", "Doppelte entfernen", MessageBoxButtons.OK)
        End If
        cn.Close()
    End Sub

    Private Sub btnRemoveDupes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub

    Private Sub DatenbankKomprimierenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DatenbankKomprimierenToolStripMenuItem.Click
        If System.IO.File.Exists(DBfileName) Then
            CompactDB(DBfileName)
        Else
            Dim openfile As New Windows.Forms.OpenFileDialog
            If openfile.ShowDialog = Windows.Forms.DialogResult.OK Then
                DBfileName = openfile.FileName
                CompactDB(DBfileName)
            End If
        End If
    End Sub

    Private Sub Rech2ReparierenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rech2ReparierenToolStripMenuItem.Click
        FixRech2()
    End Sub

    Private Sub DoppelteAufträgeAusRECH1LöschenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DoppelteAufträgeAusRECH1LöschenToolStripMenuItem.Click
        RemoveDupesRech1()
    End Sub

    Private Sub ZEITEinträgeAusRechnungenInArtstammÜbernehmenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZEITEinträgeAusRechnungenInArtstammÜbernehmenToolStripMenuItem.Click
        UpdateArtstammFromRech2_Zeit()
    End Sub

    Private Sub ArtikelMitSternchenUndPlusInArtStammÜbernehmenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArtikelMitSternchenUndPlusInArtStammÜbernehmenToolStripMenuItem.Click
        UpdateArtStammFromRech2_ohneZeit()
    End Sub

    Private Sub ArtikelstammKorrigierenUndAmEndeEntfernenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArtikelstammKorrigierenUndAmEndeEntfernenToolStripMenuItem.Click
        CleanUpArtStamm()
    End Sub

    Private Sub UmlauteKorrigierenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UmlauteKorrigierenToolStripMenuItem.Click

        Me.Cursor = Cursors.WaitCursor
        UmlauteKorrigierenALLE()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub UmlauteKorrigierenALLE()
        Dim s() As String = {"RECH1", "RECH2", "KUNDFAHR", "ARTSTAMM", "KUNDENST", "FIRMSTAM"}
        For Each t As String In s
            AddLog("Umlaute korrigieren für " & t)
            UmlauteKorrigieren(t)
        Next
    End Sub
    ''' <summary>
    ''' Korrigiert dBase Umlaute in der Tabelle sTable
    ''' </summary>
    ''' <param name="sTable">eine geöffnete Tabelle</param>
    ''' <remarks></remarks>
    Public Sub UmlauteKorrigieren(ByVal sTable As String)
        Dim ds As DataSet = New DataSet
        Dim cn As New System.Data.OleDb.OleDbConnection
        Dim c As Long = 0
        Dim sql As String = "Select * from " & sTable
        Dim lastAuftrNr As Double = 0

        'get an already opened OleDbConnection to our access mdb
        OpenDBConnection(cn)

        'we need a dataAdapter
        Dim dap As OleDbDataAdapter = New OleDb.OleDbDataAdapter
        dap.SelectCommand = New OleDbCommand(sql, cn)
        Dim cmb As OleDbCommandBuilder = New OleDbCommandBuilder(dap) 'must use this although we dont use the cmb
        Dim tbl As DataTable = New DataTable

        dap.Fill(ds, sTable)

        Dim keys(1) As DataColumn
        keys(0) = ds.Tables(sTable).Columns("id")
        ds.Tables(sTable).PrimaryKey = keys

        CorrectGermanChars(ds.Tables(sTable))
        Dim ds2 As DataSet = New DataSet
        ds2 = ds.GetChanges
        If Not ds2 Is Nothing Then
            'c = dap.Update(ds2)
            'ds.Merge(ds2)
            AddLog(" ## Updating table: " & sTable & "...")
            c = dap.Update(ds, sTable)
            ds.AcceptChanges()
            AddLog(" .. updated " & c & " records")
        End If
        cn.Close()
    End Sub
    Private Sub CorrectGermanChars(ByRef t As DataTable)
        'char table: 
        '"Ž", "Ä"
        '"š", "Ü"
        '"š", "Ü"
        '"™", "Ö"
        '"õ", "§"
        '"", "§"
        Dim i As Integer
        Dim s1 As String = ""
        Dim s2 As String = ""
        Dim sChr() As Char = {"Ž", "š", "š", "™", "õ", "", "", Chr(148), "á", "„"} ' Chr(214), 
        Dim rowchanged As Boolean
        AddLog("########## CorrectGermanChars #############")
        For Each r As DataRow In t.Rows
            rowchanged = False
            For i = 0 To r.Table.Columns.Count - 1
                If r.Table.Columns(i).DataType.Equals(System.Type.GetType("System.String")) Then
                    If Not (IsDBNull(r.Item(i))) Then
                        s1 = r(i)
                        'If s1.IndexOf("LFILTER") > 0 Then
                        '    Debug.Print("=============================")
                        '    Debug.Print(s1)
                        '    For c As Integer = 0 To s1.Length - 1
                        '        Debug.Print("<" & s1.Substring(c, 1) & ">=" & Asc(s1.Substring(c, 1)))
                        '    Next
                        'End If
                        If s1.IndexOfAny(sChr) > 0 Then
                            AddLog("String: " & s1)
                            s2 = s1.Replace("Ž", "Ä")
                            s2 = s2.Replace("š", "Ü")
                            s2 = s2.Replace("š", "Ü")
                            s2 = s2.Replace("™", "Ö")
                            's2 = s2.Replace(Chr(214), "Ö")
                            s2 = s2.Replace("õ", "ß")
                            s2 = s2.Replace("", "§")
                            s2 = s2.Replace("", "ü")
                            s2 = s2.Replace(Chr(148), "ö") '”
                            s2 = s2.Replace("á", "ß")
                            s2 = s2.Replace("„", "ä")
                            rowchanged = True
                            r.BeginEdit()
                            r(i) = s2
                            AddLog(vbTab & "converted to: " & s2)
                            'r.AcceptChanges() 
                            'if you accept changes for the row, the row status changes to 
                            'unmodified and will not be visible as changed to 
                            'table.acceptchanges or dataset.acceptchanges
                        End If
                    End If
                End If
            Next 'next column
            'Update row
            If rowchanged Then
                r.EndEdit() '!!!!!
                'r.AcceptChanges()
            End If
        Next 'next row
        't.AcceptChanges()
        'do NOT accept changes here, so the dataset will see the changes
    End Sub

    Private Sub ZeitEinträgeAusRechnungenInArtStammÜbernehmenvollToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZeitEinträgeAusRechnungenInArtStammÜbernehmenvollToolStripMenuItem.Click
        UpdateArtstammFromRech2_ZeitExtended()
    End Sub

    Private Sub ArtStammUmToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArtStammUmToolStripMenuItem.Click
        UpdateArtStammFzgTyp()
    End Sub
    Public Sub CreateRichtwerteTables()
        Dim cn As New OleDbConnection
        Dim cmd As New OleDbCommand
        Dim c As Integer
        'create a table with 
        'ArtNr(AW-Nr)   AWText    ID
        ' t17           t60     auto
        OpenDBConnection(cn)
        cmd.Connection = cn
        AddLog("Creating Richtwerte1 table...")
        KillTable("Richtwerte1")
        cmd.CommandText = "CREATE table Richtwerte1 (id AUTOINCREMENT, ArtNr TEXT(17), AWText TEXT(60))"
        Try
            c = cmd.ExecuteNonQuery()
            AddLog(vbTab & "Success: " & cmd.CommandText)
        Catch ex As Exception
            AddLog(vbTab & "FAILED: " & cmd.CommandText)
        End Try
        'create a table with 
        'ArtNr(AW-Nr)   FzgTyp    Preis    ID
        ' t17           t20       double   auto
        KillTable("Richtwerte2")
        AddLog("Creating Richtwerte2 table...")
        'cmd.CommandText = "CREATE table Richtwerte2 (id INTEGER PRIMARY KEY, ArtNr TEXT(17), FzgTyp TEXT(20), Preis DOUBLE)"
        cmd.CommandText = "CREATE table Richtwerte2 (id AUTOINCREMENT, ArtNr TEXT(17), FzgTyp TEXT(20), Preis DOUBLE)"
        Try
            c = cmd.ExecuteNonQuery()
            AddLog(vbTab & "Success: " & cmd.CommandText)
        Catch ex As Exception
            AddLog(vbTab & "FAILED: " & cmd.CommandText)
        End Try
        cn.Close()
    End Sub
    Public Sub RichtwerteImport()
        Dim cn As New OleDbConnection
        Dim cmd1 As New OleDbCommand
        Dim cmd2 As New OleDbCommand
        Dim rdrRech1 As OleDbDataReader = Nothing
        Dim rdrRech2 As OleDbDataReader = Nothing
        Dim cmdRicht As New OleDbCommand

        OpenDBConnection(cn)
        cmd1.Connection = cn
        cmd2.Connection = cn
        cmdRicht.Connection = cn

        Dim rech1FzgTyp As String
        Dim rech1AuftragNr As Long
        Dim rech2ArtNr As String = ""
        Dim rech2ArtikelBez As String = ""
        Dim rech2Preis As Double
        Dim c As String

        'read rech1 to get XAuftr_Nr and FzgTyp
        cmd1.CommandText = "Select XAuftr_Nr, XTyp from Rech1 where LEN(XTyp)>0"
        rdrRech1 = cmd1.ExecuteReader
        AddLog("Starting import for Richtwerte")
        'read rech1 for XTyp, XAuftr_Nr
        'read rech2 for Auftr_Nr, ArtikelNR="ZEIT/", ArtikelBez, E_Preis
        'write richtwerte1: Rech2.ArtikelNr->ArtNr; Rech2.ArtikelBez->AWText
        'write richtwerte2: Rech2.ArtikelNr->ArtNr; Rech1.XTyp->FzgTyp; Rech2.E_Preis->Preis
        If rdrRech1.HasRows Then
            While rdrRech1.Read
                If Not IsDBNull(rdrRech1.Item("XTyp")) And rdrRech1.Item("XTyp").ToString.Length > 0 Then
                    rech1AuftragNr = rdrRech1.Item("XAuftr_Nr")
                    rech1FzgTyp = rdrRech1.Item("XTyp")
                    AddLog(vbTab & "reading Auftrag " & rech1AuftragNr)
                    Application.DoEvents()
                    cmd2.CommandText = "Select ArtikelNr, ArtikelBez, E_Preis from rech2 where Auftr_Nr=" & _
                        rech1AuftragNr & " AND ArtikelNr LIKE 'ZEIT/%' and not isnull(ArtikelBez) and E_Preis>0"
                    rdrRech2 = cmd2.ExecuteReader
                    If rdrRech2.HasRows Then
                        While rdrRech2.Read
                            rech2ArtNr = rdrRech2.Item("ArtikelNr")
                            rech2ArtikelBez = rdrRech2.Item("ArtikelBez")
                            rech2Preis = rdrRech2.Item("E_Preis")
                            If Not ExistsData("Select ArtNr from Richtwerte1 where ArtNr='" & rech2ArtNr & "'") Then
                                '@@Identity
                                cmdRicht.CommandText = "Insert into Richtwerte1 (ArtNr, AWText) VALUES ('" _
                                        & rech2ArtNr & "', '" & rech2ArtikelBez & "')"
                                c = cmdRicht.ExecuteNonQuery
                                AddLog(vbTab & "added " & rech2ArtNr & " to richtwerte1")
                            Else
                                AddLog(vbTab & vbTab & "skipped adding '" & rech2ArtNr & "'")
                            End If
                            If Not ExistsData("Select ArtNr, FzgTyp from richtwerte2 where ArtNr='" & rech2ArtNr & _
                                    "' AND FzgTyp='" & rech1FzgTyp & "'") Then
                                cmdRicht.CommandText = "insert into richtwerte2 (ArtNr, FzgTyp, Preis) VALUES('" _
                                        & rech2ArtNr & "', '" & rech1FzgTyp & "', " & CDoubleS(rech2Preis) & ")"
                                c = cmdRicht.ExecuteNonQuery
                                AddLog(vbTab & "added " & rech1FzgTyp & ", " & rech2Preis & " to richtwerte2")
                            Else
                                AddLog(vbTab & vbTab & "skipped " & rech1FzgTyp & ", " & rech2Preis & " to richtwerte2")
                            End If
                        End While
                    End If
                    rdrRech2.Close()
                End If
            End While
        End If
        rdrRech1.Close()
        cn.Close()
    End Sub
    Public Function KillTable(ByVal s As String)
        Dim cn As New OleDbConnection
        Dim cmd As New OleDbCommand
        Dim c As Integer
        OpenDBConnection(cn)
        cmd.Connection = cn
        Try
            cmd.CommandText = "Drop table " & s
            c = cmd.ExecuteNonQuery
            cn.Close()
            Return True
        Catch ex As Exception
            cn.Close()
            Return False
        End Try

    End Function

    Private Sub CreateRichtwerteTablesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateRichtwerteTablesToolStripMenuItem.Click
        CreateRichtwerteTables()
        RichtwerteImport()
    End Sub

    Private Sub mnu_Patch1FirmStam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Patch1FirmStam.Click
        Patch1FirmStam()
    End Sub
End Class
