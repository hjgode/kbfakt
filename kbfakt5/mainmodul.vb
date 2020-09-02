Option Explicit On
Option Strict On
Imports System.Data.OleDb
Imports Microsoft.Win32

'version 2.1.0.0: added new field AltteilMwSt to FirmStam and RECH1

'########## How To update a dataset, with datatable and datarowse ###########
'Calling AcceptChanges removes the "marks" behind the scenes in the dataset 
'that tells it which records have been modified.
'To do an update, you need to call the Update method on the Data Adapter or 
'Table Adapter you used to fill the dataset or DataTable. It will update any 
'rows that are marked as new, deleted, or changed.

'Class mainmodul
Module mainmodul
    ''' <summary>
    ''' returns to execution dir, ended with a backslash
    ''' </summary>
    ''' <returns>current exe dir</returns>
    ''' <remarks></remarks>
    Public Function getAppDir() As String
        Dim s As String
        s = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0).FullyQualifiedName)
        If Not s.EndsWith("\") Then s += "\"
        Return s
    End Function
    Public Const provider As String = "provider=Microsoft.Jet.OLEDB.4.0;datasource="
    Public connectionString As String
    Public DBfileName As String = "c:\Programme\kbfakt\kbfakt5.mdb" '"D:\KFZEURO\kbfakt\kbfakt5\kbfakt5.mdb"
    Public dsRech1Rech2 As New DataSet
    Public daRech1 As New OleDbDataAdapter
    Public daRech2 As New OleDbDataAdapter
    'neu 01.06.2006
    Public daFirmstam As New OleDbDataAdapter
    Public daAnreden As New OleDbDataAdapter
    Public daKundfahr As New OleDbDataAdapter

    Public isDebug As Boolean = System.Diagnostics.Debugger.IsAttached
    Public gMwSt As Double
    Public gAltteilMwSt As Double
    Public gSTEUERNR As String

    'mögliche Anreden 1-4         0     1       2       3       4
    Public anreden() As String = {"0", "Herrn", "Frau", "Frl.", "Firma", "5", "6"}
    'mögliche Artikeltypen
    Public ArtTypen() As String = {"Teil", "Schmier", "Lohn", "Austauschteil"}

    'Liste alle verwendeter Tabellen
    Public tableList() As String = {"RECH1", "RECH2", "KUNDFAHR", "ARTSTAMM", "KUNDENST", "FIRMSTAM", "LOHNART"}

#Region "which fields will be imported"
    'which fields will be imported
    '### rechnungsliste
    Public fieldsRech1 As String = "XAUFTR_NR, XKUNDENNR, XAN1, XNAME1, XNAME2, XST, XPL, XOT, XFGSTLLNR, XDATUM, XNETTO, XAN, XSCHMIER, XLOHN, XSONDER, XKMSTAND, XKZ, XZULASS, XTYP, XMWS, XTEL"
    'plus id, gedruckt, werkdatum, aber ohne: XAS, XEND, XM, , XFABR
    '### rechnungs positionen
    Public fieldsRech2 As String = "AUFTR_NR, ARTIKELNR, ARTIKELBEZ, KONTO, MENGE, E_PREIS, RABATT, LOHNART, IST_AW, MITARBCODE, FAHRZEUG, TEXT" ', id, pos, arttyp"
    'plus Pos, id, ArtTyp
    ' SQL für Gesamtpreis
    'menge * e_preis - (RABATT/100 * (menge * e_preis))
    '### Kunden-Fahrzeuge
    Public fieldsKundfahr As String = "KUNDEN_NR, FGSTLLNR, TYP, KENNZEICH, KM_STAND, KFZ_BRF_NR, ZULASSUNG1, TUEV, ASU,  SCHREIBER,  SICHER"
    'plus id aber ohne: , FARBE, TUERSCHLNR, ZUENSCHLNR, ZULASSUNG2, UNTERBODEN, HOLRAUM, UMSATZ100, MOTOR_NR, WARTUNG, WARTUNG1, RICHT_TYP, UMSATZ100V, ORGA,BREMSE, KUEHL, CODE_RADIO, CODE_AUTO, CODE_DIEB,
    '### Artikelstamm
    Public fieldsArtStamm As String = "ARTIKELNR, ARTIKELBEZ, BESTAND, VK, LAGERORT, LETZ_EK, MISCH_EK, RABATTCODE, MIND_MENGE, LETZ_BESTL, LETZ_VK, LETZEKMENG, LIEFER1, EK1, LIEFER2, EK2, LIEFER3, EK3"
    'plus ArtTyp und id
    'Bestimmung Schmiermittel etc aus Clipper Source
    'sum all round(menge * e_preis * (1 - rabatt / 100),2) to an for auftr_nr = auf .and. ;
    '   substr(artikelbez,1,5) = "(AT)-"
    'sum all round(menge * e_preis * (1 - rabatt / 100),2) to schmier for auftr_nr = auf .and. ;
    '   substr(upper(artikelbez),1,6) = "(SCH)-"
    'sum all round(menge * e_preis * (1 - rabatt / 100),2) to lohn for auftr_nr = auf .and. ;
    '   substr(upper(artikelnr),1,4) = "ZEIT"
    '### Kundenstamm
    Public fieldsKundenSt As String = "KUNDENNR, ANREDE , VORNAME , NACHNAME, BRANCHE, PLZ , ORT , STRASSE , ANSPRECHP, TELEFON , KONTO1"
    'plus id, aber ohne: UMSLFDJAHR, UMSVORJAHR, GEBDATUM, KUNDESEIT, DATLETZUMS, L_RECH_NR, KUNDENRA, FALL , BRIEF1 , VERDICHT, SALVOR , , POSTFACH, SAMMEL , SKTONR , KKTONR , BRANCHE1, VERTRETER, BANK , BANKLZ , BANKKTO , MAHNKZ , ZAHLART , TAGE1 , TAGE2 , SKONTO , PERSKZ , PERSNR , PROZ , TANK
    '### Firmenstamm
    Public fieldsFirmStam As String = "FA_ANREDE, NAME1, NAME2, STRASSE, PLZ, ORT, MWST_SATZ1, STEUERNR" ', FUSSTEXT"
    'plus id, aber ohne: FEHLBESTND, , HANDLER_NR, DATEIN, DATEIN1, WAEHRUNG, UMRECH, D1, T1, T2, T3, T4, T5, T6, T7, KASSE, DEBITOREN, T8, ZAEHLER, 
    '### Mitarbeiter "LOHNART.DBF"
    Public fieldsLohnart As String = "MITARBNR, MITARBNAME"
    'plus id aber ohne: , PASSWORT, SOLL01, SOLL02, SOLL03, SOLL04, SOLL05, SOLL06, SOLL07, SOLL08, SOLL09, SOLL10, SOLL11, SOLL12"
#End Region

    'jet sql values for true and false
    Public Const dbTrue As Integer = -1
    Public Const dbFalse As Integer = 0
    Public Function DebugModus() As Boolean
        If System.Diagnostics.Debugger.IsAttached Then
            DebugModus = True
        Else
            DebugModus = False
        End If
    End Function
    Public Function CompactDB(ByVal f As String) As Boolean
        If MessageBox.Show("Datenbank komprimieren?", "Frage", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2) = DialogResult.No Then Exit Function
        Dim jro As JRO.JetEngine
        jro = New JRO.JetEngine
        Dim temp As String = "C:\temp1.mdb"
        System.IO.File.Delete(temp)
        Dim res As Boolean = False
        Dim oldSize, newSize As Long
        'System.IO.File.Copy(f, temp)
        Try
            oldSize = New System.IO.FileInfo(f).Length
            jro.CompactDatabase("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & f & _
            ";Jet OLEDB:Database Password=" + PasswordDecrypt("fkrsshu"), _
            "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & temp)
            'jro.CompactDatabase("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & f & _
            '";Jet OLEDB:Database", _
            '"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & temp & ";" & _
            '"Jet OLEDB:Engine Type=4") ';Jet OLEDB:Database Password=test")
            newSize = New System.IO.FileInfo(temp).Length
            res = True
        Catch ex As OleDbException
            MessageBox.Show(ex.Message)
        Catch ex1 As Exception
            MessageBox.Show(ex1.Message)
        End Try
        If res Then
            System.IO.File.Delete(f + ".bak")
            System.IO.File.Copy(f, f + ".bak")
            System.IO.File.Delete(f)
            System.IO.File.Copy(temp, f)
            MessageBox.Show("Alte Grösse der Datenbankdatei: " + vbCrLf + oldSize.ToString().PadLeft(15) + vbCrLf + _
            "Neue Grösse der Datenbankdatei: " + vbCrLf + newSize.ToString().PadLeft(15), "Datenbank komprimieren")
        End If
        Return res
    End Function
    Public Sub readFirmstamm()
        '"FA_ANREDE, NAME1, NAME2, STRASSE, PLZ, ORT, MWST_SATZ1, STEUERNR"
        Dim cn As New OleDbConnection
        OpenDBConnection(cn)
        'fill the rech1 into ds
        Dim cmd As New OleDbCommand("select * from FIRMSTAM", cn)
        Dim r As OleDbDataReader = cmd.ExecuteReader
        Try
            r.Read()
            If r.HasRows Then
                gMwSt = CDbl(r.Item("MWST_SATZ1").ToString)
                gSTEUERNR = r.Item("STEUERNR").ToString
                gAltteilMwSt = CDbl(r.Item("AltteilMwst").ToString)
            End If
        Catch ex As Exception

        End Try

        cn.Close()
    End Sub
    Public Function initDSRech1Rech2() As Boolean
        Dim cn As New OleDbConnection
        Try
            OpenDBConnection(cn)
            'fill the rech1 into ds
            Dim cmd As New OleDbCommand("select * from RECH1", cn)
            daRech1.SelectCommand = cmd
            dsRech1Rech2.Clear()
            dsRech1Rech2.Relations.Clear()
            Dim cmb As New OleDb.OleDbCommandBuilder(daRech1)
            daRech1.Fill(dsRech1Rech2, "RECH1")

            'Debug.Print("DeleteCommand: " & daRech1.DeleteCommand.ToString & _
            '", UpdateCommand: " & daRech1.UpdateCommand.ToString & _
            '", SelectCommand: " & daRech1.SelectCommand.ToString & vbCrLf)

            Dim dcs(1) As DataColumn
            dcs(0) = dsRech1Rech2.Tables("RECH1").Columns("id")
            dsRech1Rech2.Tables("RECH1").PrimaryKey = dcs

            'fill the rech2 into ds
            cmd.CommandText = "select * from rech2"
            daRech2.SelectCommand = cmd
            Dim cmb2 As New OleDb.OleDbCommandBuilder(daRech2)
            daRech2.Fill(dsRech1Rech2, "RECH2")

            'Add the key columns to the tables
            Dim keys(2) As DataColumn
            keys(0) = dsRech1Rech2.Tables("RECH1").Columns("XAUFTR_NR")
            keys(1) = dsRech1Rech2.Tables("RECH1").Columns("id")
            dsRech1Rech2.Tables("RECH1").PrimaryKey = keys

            Dim keys2(1) As DataColumn
            keys2(0) = dsRech1Rech2.Tables("RECH2").Columns("id")
            dsRech1Rech2.Tables("RECH2").PrimaryKey = keys2

            'Define parent and child columns for
            'a DataRelation object.
            Dim parentcol As DataColumn = _
                dsRech1Rech2.Tables("RECH1").Columns("XAUFTR_NR")
            Dim childcol As DataColumn = _
                dsRech1Rech2.Tables("RECH2").Columns("AUFTR_NR")

            'Instantiate a new DataRelation object.
            Dim drl1 As DataRelation
            drl1 = New _
                DataRelation("Rech1Rech2", _
                parentcol, childcol)

            'Add the DataRelation object to the Relations
            'collection for das1.
            dsRech1Rech2.Relations.Add(drl1)

            'add Firmstam, kundfahr and Anreden
            cmd.CommandText = "Select * from Anreden"
            daAnreden.SelectCommand = cmd
            daAnreden.Fill(dsRech1Rech2, "Anreden")
            Dim keys3(1) As DataColumn
            keys3(0) = dsRech1Rech2.Tables("Anreden").Columns("id")
            dsRech1Rech2.Tables("Anreden").PrimaryKey = keys3

            cmd.CommandText = "Select * from Firmstam"
            daFirmstam.SelectCommand = cmd
            daFirmstam.Fill(dsRech1Rech2, "Firmstam")
            keys3(0) = dsRech1Rech2.Tables("Firmstam").Columns("id")
            dsRech1Rech2.Tables("Firmstam").PrimaryKey = keys3

            cmd.CommandText = "Select * from Kundfahr"
            daKundfahr.SelectCommand = cmd
            daKundfahr.Fill(dsRech1Rech2, "Kundfahr")
            keys2(0) = dsRech1Rech2.Tables("Kundfahr").Columns("FGSTLLNR")
            keys2(1) = dsRech1Rech2.Tables("Kundfahr").Columns("id")
            dsRech1Rech2.Tables("Kundfahr").PrimaryKey = keys2

            'parentcol = dsRech1Rech2.Tables("Rech1").Columns("XFGSTLLNR")
            'childcol = dsRech1Rech2.Tables("Kundfahr").Columns("FGSTLLNR")

            'Dim drl2 As DataRelation
            'drl2 = New _
            '    DataRelation("Rech1Kundfahr", _
            '    parentcol, childcol)

            ''Add the DataRelation object to the Relations
            ''collection for das1.
            'dsRech1Rech2.Relations.Add(drl2)
            dsRech1Rech2.WriteXmlSchema("\Rech1Rech2.xsd")
            Return True
        Catch ex As OleDbException
            MessageBox.Show("Fehler beim Erstellen der Datenbeziehungen. Auftragsbearbeitung kann nicht fortgesetzt werden." & vbCrLf & ex.Message, "AddRelation-OLEDBException", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Catch ar As ArgumentException
            MessageBox.Show("Fehler beim Erstellen der Datenbeziehungen. Wahrscheinlich doppelte Aufragsnumern. Bitte reparieren. Auftragsbearbeitung kann nicht fortgesetzt werden." & vbCrLf & ar.Message, "ArgumentException-AddRelation", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Function selectDBfilename(ByRef file As String) As DialogResult
        Dim dlg As New System.Windows.Forms.OpenFileDialog
        dlg.CheckFileExists = True
        dlg.CheckPathExists = True
        dlg.InitialDirectory = Environment.CurrentDirectory
        dlg.Filter = "MS Access Dateien (*.mdb)|*.mdb"
        Dim dlgRes As DialogResult
        dlgRes = dlg.ShowDialog
        If dlgRes = DialogResult.OK Then
            file = dlg.FileName
        End If
        Return dlgRes

    End Function
    Public Function GetDbFileName() As String
        Return DBfileName
    End Function
    Public Function InitDBfilename() As Boolean
        Dim s As String = readRegDBFileName()
        If (s = "") Then
            DBfileName = "c:\Programme\kbfakt\kbfakt5.mdb"
        Else
            DBfileName = s
        End If
        'If Not DebugModus() Then
        '    DBfileName = Environment.CurrentDirectory + "\kbfakt5.mdb"
        'Else
        '    DBfileName = "d:\kfzeuro\kbfakt\kbfakt5\kbfakt5.mdb"
        'End If
file_start:
        If Not System.IO.File.Exists(DBfileName) Then
            If MessageBox.Show("Datenbankdatei " & DBfileName & " nicht gefunden!" & vbCrLf & "Datenbank löschen und neu erstellen?", DBfileName, MessageBoxButtons.YesNo) = DialogResult.Yes Then
                CreateNewMDBfile()
            Else
                Dim dlg As New System.Windows.Forms.OpenFileDialog
                dlg.CheckFileExists = True
                dlg.CheckPathExists = True
                dlg.InitialDirectory = Environment.CurrentDirectory
                dlg.Filter = "MS Access Dateien (*.mdb)|*.mdb"
                Dim filename As String = ""
                If selectDBfilename(filename) = DialogResult.OK Then 'dlg.ShowDialog = DialogResult.OK Then
                    'DBfileName = dlg.FileName
                    DBfileName = filename
                    GoTo file_start
                Else
                    MessageBox.Show("Ohne Datenbank funktioniert dieses Programm nicht")
                    Return False
                End If
            End If
        Else
            regWriteDBFilename(DBfileName)
            connectionString = provider + DBfileName
            Return True
        End If
    End Function
    Function GetAllControls(ByVal f As System.Windows.Forms.Form) As System.Windows.Forms.Control()
        ' Retrieve all controls and all child controls etc.
        ' Make sure to send controls back at lowest depth first
        ' so that most child controls are checked for things before
        ' container controls, e.g., a TextBox is checked before a
        ' GroupBox control
        Dim allControls As ArrayList = New ArrayList()
        Dim myqueue As Queue = New Queue()
        myqueue.Enqueue(f.Controls)

        Dim mycontrols As System.Windows.Forms.Form.ControlCollection
        Dim current As Object
        Do While myqueue.Count > 0
            current = myqueue.Dequeue()
            If TypeOf current Is System.Windows.Forms.Form.ControlCollection Then
                mycontrols = CType(current, System.Windows.Forms.Form.ControlCollection)
                If mycontrols.Count > 0 Then
                    Dim mycontrol As Control
                    For Each mycontrol In mycontrols
                        allControls.Add(mycontrol)
                        myqueue.Enqueue(mycontrol.Controls)
                    Next
                End If
            End If
        Loop
        Return CType(allControls.ToArray(GetType(System.Windows.Forms.Control)), System.Windows.Forms.Control())
        '(Control())allControls.ToArray(typeof(Control))

        'Sub okButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        '    ' Me.DialogResult = DialogResult.OK
        '    ' Me.Close()

        '    ' Validate each control manually
        '    Dim mycontrol As Control
        '    For Each mycontrol In GetAllControls()
        '        ' Validate this control
        '        mycontrol.Focus()
        '        If Not (Me.Validate()) Then
        '            Me.DialogResult = DialogResult.None
        '            Exit For
        '        End If
        '    Next
        'End Sub
    End Function
    Public Sub clearKontrols(ByVal f As System.Windows.Forms.Form, ByVal tag As String)
        For Each ctrl As System.Windows.Forms.Control In GetAllControls(f)
            If CStr(ctrl.Tag) = tag Then
                ctrl.ResetText()
            End If

        Next ctrl
    End Sub
    Public Sub enableKontrols(ByVal form As System.Windows.Forms.Form, ByVal tag As String, ByVal enable As Boolean)
        For Each ctrl As System.Windows.Forms.Control In form.Controls
            'If TypeOf ctrl Is System.Windows.Forms.Control Then
            Debug.Print(ctrl.Name + vbTab + CStr(ctrl.Tag))
            If CStr(ctrl.Tag) = tag Then
                ctrl.Enabled = enable
            End If
            'End If
        Next
    End Sub

    Public Sub OpenDBConnection(ByRef DBCon As OleDb.OleDbConnection) ', ByRef ConnString As String, ByVal DBPath As String)
        If DBCon.State = ConnectionState.Open Then
            DBCon.Close()
        End If
        Dim provider As String
        Dim datasource As String
        provider = "provider=Microsoft.Jet.OLEDB.4.0;data source = "
        datasource = DBfileName
        'datasource = "kbfakt5.mdb"
        connectionString = provider + datasource '+ "; Mode=12" 'open exclusive
        DBCon.ConnectionString = connectionString

        DBCon.ConnectionString += ";Jet OLEDB:Database Password=" + PasswordDecrypt("fkrsshu")
        'Provider=Microsoft.Jet.OLEDB.4.0; Data Source=d:\Northwind.mdb;User ID=Admin;Password=; 

        Try
            'First try the Application dir for the mdb file
            DBCon.Open()
        Catch ex As System.Data.OleDb.OleDbException
            System.Diagnostics.Debug.WriteLine("Exception in OpenDBConnection:" + ex.Message)
            'now try the settings
            connectionString = My.Settings.kbfaktConnectionString
            Try
                DBCon.ConnectionString = connectionString + "; Mode=12" 'open exclusive
                DBCon.Open()
            Catch ex1 As System.Data.OleDb.OleDbException
                System.Diagnostics.Debug.WriteLine("Exception in OpenDBConnection1:" + ex1.Message)
                MessageBox.Show("Fehler bei connectstring" + vbCrLf + datasource, ex1.Message)
            End Try
        End Try
    End Sub
    Function GetDir(ByVal g_Path As String) As String
        'Dim g_Path As String
        'g_Path = Application.ExecutablePath()
        g_Path = g_Path.Substring(0, g_Path.Length - (g_Path.Length - g_Path.LastIndexOf("\")))
        Return g_Path
    End Function
    Function getName(ByVal fullpath As String) As String
        Dim sTemp As String
        sTemp = fullpath
        '"D:\C-Source\Active\ZugangPC1\ZugangPC1\bin\Debug\ZugangPC1.EXE"
        Dim s As Integer = sTemp.LastIndexOf("\")
        Dim l As Integer = sTemp.Length() - s
        sTemp = sTemp.Substring(s + 1)
        Return sTemp
    End Function
    Function getNameOnly(ByVal name As String) As String
        Dim sTemp As String
        sTemp = name.Substring(0, name.Length - (name.Length - name.LastIndexOf(".")))
        Return sTemp
    End Function

    Sub main()
    End Sub
    Public Sub CreateNewMDBfile()
        'Assign database file name to str1
        Dim str1 As String = DBfileName
        'Delete file if it exists
        If System.IO.File.Exists(str1) Then
            System.IO.File.Delete(str1)
        End If
        'Create cat1 in the file to which str1 points
        Dim cat1 As ADOX.Catalog
        cat1 = New ADOX.Catalog()
        str1 = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
        "Data Source=" & str1
        cat1.Create(str1)
        'Setting to nothing does not cause timely
        'removal from memory
        'Force removal of cat1 from memory by first
        'setting to nothing and second collecting
        'unreferenced objects with the Garbage Collector
        cat1 = Nothing
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub
    'End Class
    Public Function CString(ByVal s As String) As String
        If IsDBNull(s) Then CString = ""
        Try
            CString = CStr(s)
        Catch ex As Exception
            CString = ""
        End Try
    End Function
    Public Function CDouble(ByVal s As Object) As Double
        If IsDBNull(s) Then CDouble = 0
        Try
            CDouble = CDbl(s)
        Catch ex As Exception
            CDouble = 0
        End Try
    End Function
    Public Function CDoubleS(ByVal s As Object) As String
        If IsDBNull(s) Then CDoubleS = "0"
        Try
            CDoubleS = CDbl(s).ToString.Replace(",", ".")
        Catch ex As Exception
            CDoubleS = "0.00"
        End Try
    End Function
    Public Function CInteger(ByVal o As Object) As Integer
        If IsDBNull(o) Then CInteger = 0
        Try
            CInteger = CInt(o)
        Catch ex As Exception
            CInteger = 0
        End Try
    End Function
    Public Function CDatum(ByVal s As String) As Date
        Try
            If s.Length = 0 Or Not IsDate(s) Then
                CDatum = CDate("01.01.1901")
            Else
                CDatum = CDate(s)
            End If
        Catch ex As Exception

        End Try
    End Function
    Public Function CLong(ByVal s As String) As Long
        Try
            CLong = CLng(s)
        Catch ex As Exception
            CLong = 0
        End Try

    End Function
    Public Function IndexOf(ByRef ar() As String, ByVal s As String) As Integer
        For i As Integer = 0 To ar.Length
            If ar(i) = s Then
                IndexOf = i
                Exit Function
            End If
        Next
        IndexOf = 0
    End Function
    Public Function PasswordEncrypt(ByVal Password As String) As String
        Dim i As Integer
        Dim Result As String = ""
        For i = 1 To Len(Password)
            Result = Result & Chr(Asc(Mid(Password, i, 1)) + 3)
        Next
        PasswordEncrypt = Result
    End Function
    Public Function PasswordDecrypt(ByVal password As String) As String
        Dim i As Integer
        Dim Result As String = ""
        For i = 1 To Len(password)
            Result = Result & Chr(Asc(Mid(password, i, 1)) - 3)
        Next
        PasswordDecrypt = Result
    End Function
    Public Function CheckTextLength(ByRef txtbox As TextBox) As Boolean
        If txtbox.Text.Length = 0 Then
            txtbox.BackColor = Color.LightPink
            CheckTextLength = False
        Else
            txtbox.BackColor = Color.White
            CheckTextLength = True
        End If
    End Function
    Public Function CheckTextDouble(ByRef txtbox As TextBox) As Boolean
        Dim l As Double
        Dim l2 As Double
        Dim t As String
        Try
            l = CDbl(txtbox.Text)
            t = CStr(l)
            l2 = CDbl(t)
            If l2 <> l Then
                CheckTextDouble = False
                txtbox.BackColor = Color.LightPink
                Exit Function
            Else
                txtbox.BackColor = Color.White
                CheckTextDouble = True
            End If
        Catch ex As Exception
            txtbox.BackColor = Color.LightPink
            CheckTextDouble = False
        End Try
    End Function
    Public Function CheckTextLong(ByRef txtbox As TextBox) As Boolean
        Dim l As Long
        Dim l2 As Long
        Dim t As String
        Try
            l = CLong(txtbox.Text)
            t = CStr(l)
            l2 = CLong(t)
            If l2 <> l Then
                CheckTextLong = False
                txtbox.BackColor = Color.LightPink
                Exit Function
            Else
                txtbox.BackColor = Color.White
                CheckTextLong = True
            End If
        Catch ex As Exception
            txtbox.BackColor = Color.LightPink
            CheckTextLong = False
        End Try
    End Function
    ''' <summary>
    ''' checks contents of txtbox for valid integer
    ''' </summary>
    ''' <param name="txtbox"></param>
    ''' <returns>true, if valid</returns>
    ''' <remarks></remarks>
    Public Function CheckTextInt(ByRef txtbox As TextBox) As Boolean
        Dim l As Integer
        Dim l2 As Integer
        Dim t As String
        'new if block in 2.0.0.6
        If txtbox.Text.Length = 0 Then
            CheckTextInt = False
            txtbox.BackColor = Color.LightPink
            Exit Function
        End If
        Try
            l = CInt(txtbox.Text)
            t = CStr(l)
            l2 = CInt(t)
            If l2 <> l Then
                CheckTextInt = False
                txtbox.BackColor = Color.LightPink
                Exit Function
            Else
                txtbox.BackColor = Color.White
                CheckTextInt = True
            End If
        Catch ex As Exception
            CheckTextInt = False
            txtbox.BackColor = Color.LightPink
        End Try
    End Function
    Public Function CheckTextDatum(ByRef ctrl As Control) As Boolean
        Dim l As Date
        Dim l2 As Date
        Dim t As String
        Try
            l = CDate(ctrl.Text)
            t = CStr(l)
            l2 = CDate(t)
            If l2 <> l Then
                CheckTextDatum = False
                ctrl.BackColor = Color.LightPink
                Exit Function
            Else
                CheckTextDatum = True
                ctrl.BackColor = Color.White
            End If
        Catch ex As Exception
            ctrl.BackColor = Color.LightPink
            CheckTextDatum = False
        End Try
    End Function
    ''' <summary>
    ''' Prüft, ob ein Text eine gültige Monats und Jahr Angabe enthält
    ''' zB 01/99 oder 12/01
    ''' </summary>
    ''' <param name="ctrl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckMonatJahr(ByRef ctrl As Control) As Boolean
        Dim mon As Integer
        Dim yea As Integer
        Dim t As String
        Dim t1 As String
        Dim t2 As String
        Try
            t = ctrl.Text
            If t.Length <> 5 Then GoTo mjfehler
            t1 = t.Substring(0, 2)
            t2 = t.Substring(3, 2)
            mon = CInt(t1)
            yea = CInt(t2)
            If mon < 0 Or mon > 12 Then GoTo MJFehler
            If yea < 0 Then GoTo MJFehler
            CheckMonatJahr = True
            ctrl.BackColor = Color.White
        Catch ex As Exception
            ctrl.BackColor = Color.LightPink
            CheckMonatJahr = False
        End Try
        Exit Function
MJfehler:
        CheckMonatJahr = False
        ctrl.BackColor = Color.LightPink
    End Function
    ''' <summary>
    ''' AuftragBuchen mit AuftragNummer: Nur der ausgewählte Auftrag wird gebucht
    ''' der ArtikelStamm wird aktualisiert
    ''' </summary>
    ''' <param name="auftrag"></param>
    ''' <returns>LONG=Anzahl gebuchter Aufträge
    ''' return=-2 bedeutet Gedruckt war True und Buchung wurde nicht nochmal durchgeführt
    ''' -1 bedeutet exception</returns>
    ''' <remarks>buchen bedeutet gedruckt=true und Änderung der Artikel-Bestände</remarks>
    Public Function AuftragBuchen(ByRef auftrag As Long, ByRef FzgTyp As String, Optional ByVal bGutschrift As Boolean = False) As Long
        If Not DebugModus() Then
            'gedruckte aufträge nicht nochmal buchen!
            If CheckGedruckt(auftrag) Then Return -2 'changed in 2.0.0.6 from -1 to -2
        End If
        Dim queryString As String
        Dim Anzahl As Integer = 0
        Dim cmd As New OleDbCommand
        Dim trn As OleDbTransaction
        Dim cn As New OleDbConnection
        Dim rdr As OleDbDataReader
        Dim artikel As String
        Dim menge As Long
        Dim count As Long = 0

        Dim ArtBez As String
        Dim EK As Double
        Dim VK As Double
        Dim ArtTyp As Long

        queryString = "Select * from Rech2 where AUFTR_Nr=" & auftrag
        OpenDBConnection(cn)
        trn = cn.BeginTransaction()
        cmd.Connection = cn
        cmd.Transaction = trn
        cmd.CommandText = queryString
        Try
            'Update the artikel database by using Rech2 values
            rdr = cmd.ExecuteReader()
            'rdr.Read()
            If rdr.HasRows Then
                While rdr.Read()
                    artikel = rdr.Item("ARTIKELNR").ToString
                    ArtBez = rdr.Item("ARTIKELBEZ").ToString
                    ArtTyp = CLong(rdr.Item("ArtTyp").ToString)
                    VK = CDouble(rdr.Item("E_PREIS"))
                    'do not update artikel ending with asteriks
                    If artikel.EndsWith("*") Then Continue While
                    If artikel.EndsWith("+") Then
                        'Add Artikel to db
                        '"ARTIKELNR, ARTIKELBEZ, BESTAND, VK, LAGERORT, LETZ_EK, MISCH_EK, RABATTCODE, MIND_MENGE, LETZ_BESTL, LETZ_VK, LETZEKMENG, LIEFER1, EK1, LIEFER2, EK2, LIEFER3, EK3"
                        'plus ArtTyp und id
                        ArtikelAnlegen(artikel, ArtBez, EK, VK, FzgTyp, ArtTyp)
                    End If
                    'changed for 2.0.1.6
                    If Not artikel.StartsWith("ZEIT", StringComparison.OrdinalIgnoreCase) Then
                        menge = CLong(rdr.Item("MENGE").ToString)
                        'update artstamm
                        If Not ArtikelBuchen(artikel, menge) Then
                            Debug.Print(" Error in ArtikelBuchen " & artikel)
                        Else
                            count = count + 1
                        End If
                    Else
                        count = count + 1
                    End If
                End While
            End If
            '
            rdr.Close()

            cmd.CommandText = "UPDATE RECH1 set Gedruckt=True where xAuftr_nr=" & auftrag
            cmd.CommandType = CommandType.Text
            menge = cmd.ExecuteNonQuery
            If bGutschrift Then
                cmd.CommandText = "UPDATE RECH1 set Gutschrift=True where xAuftr_nr=" & auftrag
                cmd.CommandType = CommandType.Text
                menge = cmd.ExecuteNonQuery
            End If
            If menge = 0 Then count = 0
            trn.Commit()

        Catch ex As OleDbException
            MessageBox.Show(ex.Message, "AuftragBuchen >" & auftrag & "<")
            trn.Rollback()
            count = -1
        End Try
        cn.Close()
        Return count
    End Function
    Public Function ArtikelBuchen(ByRef artnr As String, ByRef anzahl As Long) As Boolean
        Dim Anz As Integer = 0
        Dim cmd As New OleDbCommand
        Dim cn As New OleDbConnection
        Try
            OpenDBConnection(cn)
            cmd.CommandText = "Update ArtStamm set BESTAND=BESTAND-" & anzahl & " where ARTIKELNR='" & artnr & "'"
            cmd.Connection = cn
            Anz = cmd.ExecuteNonQuery
        Catch dx As OleDbException
            MessageBox.Show(dx.Message, "ArtikelBuchen >" & artnr & "<")
        Finally
        End Try
        cn.Close()
        If Anz = 0 Then Return False
        Return True
    End Function
    Public Function ArtikelAnlegen(ByRef artnr As String, ByVal ArtBez As String, ByVal ek As Double, ByVal vk As Double, ByVal FzgTyp As String, ByVal ArtTyp As Long) As Integer
        Dim Anz As Integer = 0
        Dim cmd As New OleDbCommand
        Dim cn As New OleDbConnection
        If artnr.EndsWith("+") Then artnr = artnr.Substring(0, artnr.Length - 1)
        If artnr = "" Then Return 0
        If ExistsData("select artikelnr from ArtStamm where artikelnr='" & artnr & "'") Then
            MessageBox.Show("Artikel >" & artnr & "< existiert bereits und wird nicht neu angelegt!", "ArtikelAnglegen()")
            Return 0
        End If

        Try
            OpenDBConnection(cn)
            '"ARTIKELNR, ARTIKELBEZ, BESTAND, VK, LAGERORT, LETZ_EK, MISCH_EK, RABATTCODE, MIND_MENGE, LETZ_BESTL, LETZ_VK, LETZEKMENG, LIEFER1, EK1, LIEFER2, EK2, LIEFER3, EK3"
            'plus ArtTyp und id
            ' "Insert Into (ARTIKELNR, ARTIKELBEZ, BESTAND, VK, EK1, ArtTyp) VALUES ('hjgode+', 'Frei', 0, 0, 00) "
            'convert VK to Double with '.' instead of ','
            Dim sVK As String = CDoubleS(vk) 'new in v 2.0.3.0
            cmd.CommandText = "Insert Into ArtStamm (ARTIKELNR, ARTIKELBEZ, BESTAND, VK, EK1, ArtTyp, FzgTyp) VALUES ('" _
                            & artnr & "', '" & ArtBez & "', 0, " & sVK & ", " & ek & ", " & ArtTyp & ", '" & FzgTyp & "') "
            cmd.Connection = cn
            Anz = cmd.ExecuteNonQuery
        Catch dx As OleDbException
            MessageBox.Show(dx.Message, "ArtikelAnlegen >" & artnr & "<")
        Finally
        End Try
        cn.Close()
        ArtikelAnlegen = Anz
    End Function
    ''' <summary>
    ''' will update the Fahrzeug database with data from Rechnung
    ''' </summary>
    ''' <param name="fgstllnr"></param>
    ''' <param name="km"></param>
    ''' <param name="nASU"></param>
    ''' <param name="nTuev"></param>
    ''' <param name="nSicher"></param>
    ''' <param name="nSchreib"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FahrzeugBuchen(ByVal fgstllnr As String, ByVal km As Long, _
        ByVal nASU As String, ByVal nTuev As String, ByVal nSicher As String, ByVal nSchreib As String) As Boolean

        If fgstllnr = "999999" Then Return True

        Dim Anz As Integer = 0
        Dim cmd As New OleDbCommand
        Dim cn As New OleDbConnection
        '"KUNDEN_NR, FGSTLLNR, TYP, KENNZEICH, KM_STAND, KFZ_BRF_NR, ZULASSUNG1, 
        'TUEV, ASU,  SCHREIBER,  SICHER"
        If nASU = "" Then nASU = "00/00"
        If nTuev = "" Then nTuev = "00/00"
        If nSicher = "" Then nSicher = "00/00"
        If nSchreib = "" Then nSicher = "00/00"

        Try
            OpenDBConnection(cn)
            cmd.CommandText = "Update KundFahr set km_stand=" & km & "," & _
                                " TUEV='" & nTuev & "'," & _
                                " ASU='" & nASU & "'," & _
                                " Schreiber='" & nSchreib & "'," & _
                                " SICHER='" & nSicher & "'" & _
                                " where FGSTLLNR='" & fgstllnr & "'"
            cmd.Connection = cn
            Anz = cmd.ExecuteNonQuery
        Catch dx As OleDbException
            MessageBox.Show(dx.Message, "FahrzeugBuchen >" & fgstllnr & "<")
        Finally
        End Try
        cn.Close()
        If Anz = 0 Then Return False
        Return True
    End Function
    Public Function GetAnrede(ByVal idx As Integer) As String
        Dim cmd As New OleDbCommand
        Dim cn As New OleDbConnection
        Dim rdr As OleDbDataReader = Nothing
        Dim anrede As String
        anrede = ""
        OpenDBConnection(cn)
        cmd.Connection = cn
        cmd.CommandText = "select id, Anrede from Anreden where id=" & idx
        Try
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If rdr.Read() Then
                    anrede = rdr("Anrede").ToString
                End If
            End If
        Catch ex As Exception
            anrede = ""
        End Try
        Return anrede
    End Function
    Public Function CheckGedruckt(ByVal a As Long) As Boolean
        Dim bGedruckt As Boolean = False
        Dim fType As Type
        Dim cmd As New OleDbCommand
        Dim cn As New OleDbConnection
        Dim rdr As OleDbDataReader = Nothing
        OpenDBConnection(cn)
        cmd.Connection = cn
        cmd.CommandText = "select gedruckt from rech1 where XAUFTR_NR=" & a
        Try
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                If rdr.Read() Then
                    fType = rdr.GetFieldType(0)
                    If rdr.GetBoolean(0) = True Then
                        bGedruckt = True
                    Else
                        bGedruckt = False
                    End If
                End If
            Else
                bGedruckt = False
            End If
        Catch dx As OleDbException
            Debug.Print("Exception in CheckGedruckt(): " & dx.Message)
        Finally
            rdr.Close()
            cn.Close()
        End Try
        Return bGedruckt
    End Function
    ''' <summary>
    ''' Test if a value already exists in a field of Database db
    ''' </summary>
    ''' <param name="sql">the sql select string</param>
    ''' <returns>true if value exists, false if not</returns>
    ''' <remarks></remarks>
    Public Function ExistsData(ByVal sql As String) As Boolean
        Dim Anzahl As Integer = 0
        Dim cmd As New OleDbCommand
        Dim cn As New OleDbConnection
        Dim rdr As OleDbDataReader = Nothing
        OpenDBConnection(cn)
        cmd.Connection = cn
        cmd.CommandText = sql
        Try
            cmd.CommandText = sql
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then Anzahl = 1
        Catch dx As OleDbException
            Debug.Print("Exception in ExistsData(): " & dx.Message)
        Finally
            If IsNothing(rdr) = False Then rdr.Close()
            cn.Close()
        End Try
        If Anzahl > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub FormSettingsSave(ByRef f As Form)
        Dim config As IsolatedStorage.ConfigurationManager
        config = IsolatedStorage.ConfigurationManager.GetConfigurationManager("KBFakt")
        config.WriteFormSettings(f)
        config.Persist()
    End Sub
    Public Sub FormSettingsLoad(ByRef f As Form)
        Dim config As IsolatedStorage.ConfigurationManager
        config = IsolatedStorage.ConfigurationManager.GetConfigurationManager("KBFakt")
        config.ReadFormSettings(f)
    End Sub
    'Private Sub SimpleDataSet(ByVal Choice As Integer)
    '    Dim myReport As New SimpleDataset
    '    Dim myDataSet As New DataSet
    '    Select Case Choice
    '        Case 1
    '            SimpleAccessDataset(myDataSet)
    '        Case 2
    '            SimpleSQLDataset(myDataSet)
    '    End Select
    '    myReport.SetDataSource(myDataSet)
    '    CrystalReportViewer1.ReportSource = myReport
    'End Sub
    Private Sub SimpleAccessDataset(ByVal myDataSet As DataSet)
        Dim myConnectionString As String
        Dim myConnection As OleDb.OleDbConnection
        Dim myDataAdapter As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter
        Dim SQL As String
        SQL = "SELECT [Order Id] as OrderId FROM ORDERS"
        myConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=C:\Xtreme.mdb"
        myConnection = New OleDb.OleDbConnection(myConnectionString)
        myDataAdapter.SelectCommand = New OleDb.OleDbCommand(SQL, myConnection)
        myDataAdapter.Fill(myDataSet, "Orders")
    End Sub
    Private Sub SimpleSQLDataset(ByVal myDataSet As DataSet)
        Dim myConnectionString As String
        Dim myConnection As SqlClient.SqlConnection
        Dim myDataAdapter As SqlClient.SqlDataAdapter
        myConnectionString = "Data Source=(local);UID=sa;Database=Northwind"
        myConnection = New SqlClient.SqlConnection(myConnectionString)
        Dim cmdCustomers As New SqlClient.SqlCommand("SELECT OrderId FROM ORDERS", myConnection)
        cmdCustomers.CommandType = CommandType.Text
        myDataAdapter = New SqlClient.SqlDataAdapter(cmdCustomers)
        myDataAdapter.Fill(myDataSet, "Orders")
    End Sub
    Public Sub ShowWaitCursor(ByRef show As Boolean)
        If show Then
            Cursor.Current = Cursors.WaitCursor
        Else
            Cursor.Current = Cursors.Default
        End If
    End Sub
    Public Sub PatchRichtwerte()
        If ExistsData("Select * from richtwerte1") And ExistsData("Select * from richtwerte2") Then
            Exit Sub
        Else
            datei_import.CreateRichtwerteTables()
            datei_import.RichtwerteImport()
        End If
    End Sub
    ''' <summary>
    ''' Patch1 für ArtStamm
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Patch1ArtStamm()
        'fieldsArtStamm As String = "ARTIKELNR, ARTIKELBEZ, BESTAND, VK, LAGERORT, 
        'LETZ_EK, MISCH_EK, RABATTCODE, MIND_MENGE, LETZ_BESTL, LETZ_VK, LETZEKMENG, 
        'LIEFER1, EK1, LIEFER2, EK2, LIEFER3, EK3"
        'plus ArtTyp und id
        'PLUS FzgTyp

        Dim NeedPatch1 As Boolean = False
        Dim AccessConn As New System.Data.OleDb.OleDbConnection
        OpenDBConnection(AccessConn)

        Dim AccessCommand As New System.Data.OleDb.OleDbCommand
        Dim c As Integer
        AccessCommand.Connection = AccessConn
        Try
            AccessCommand.CommandText = "Select FzgTyp from ArtStamm"
            c = AccessCommand.ExecuteNonQuery()
            Debug.Print("Patch1ArtStamm will NOT be applied")
        Catch ex2 As OleDbException
            'the field does not exist
            NeedPatch1 = True
            Debug.Print("Patch1ArtStamm MUST be applied")
        End Try
        If NeedPatch1 Then
            Try
                AccessCommand.CommandText = "ALTER table " & "ArtStamm" & " ADD COLUMN FzgTyp char(20)"
                c = AccessCommand.ExecuteNonQuery()
                Debug.Print("ALTER table ADD Column FzgTyp für ArtStamm abgeschlossen")
            Catch ex2 As OleDbException
                Debug.Print(ex2.Message & " Patch1ArtStamm")
                MessageBox.Show(ex2.Message & " ### Fehler", "Patch1ArtStamm")
            End Try
        End If
        AccessConn.Close()
    End Sub
    Public Sub Patch4Rech1()
        'Add field LDATUM to Rech1 'L = Leistungs-/Lieferdatum
        Dim NeedPatch4 As Boolean = False
        Dim AccessConn As New System.Data.OleDb.OleDbConnection
        OpenDBConnection(AccessConn)

        Dim AccessCommand As New System.Data.OleDb.OleDbCommand
        Dim c As Integer
        AccessCommand.Connection = AccessConn
        Try
            AccessCommand.CommandText = "Select LDATUM from rech1"
            c = AccessCommand.ExecuteNonQuery()
            Debug.Print("Patch4Rech (LDATUM) will NOT be applied")
        Catch ex2 As OleDbException
            'the field does not exist
            NeedPatch4 = True
            Debug.Print("Patch4Rech (LDATUM) MUST be applied")
        End Try
        If NeedPatch4 Then
            Try
                AccessCommand.CommandText = "ALTER table " & "rech1" & " ADD COLUMN LDatum DATETIME"
                c = AccessCommand.ExecuteNonQuery()
                Debug.Print("'ADD COLUMN LDatum DATETIME' abgeschlossen")
                'Set LDATUM for existing data
                AccessCommand.CommandText = "UPDATE rech1 set LDATUM=XDATUM ;"
                c = AccessCommand.ExecuteNonQuery()
                Debug.Print("Set LDatum = XDatum for existing data OK")
            Catch ex2 As OleDbException
                Debug.Print(ex2.Message & " Patch4Rech1")
                MessageBox.Show(ex2.Message & " ### Fehler", "Patch4Rech1")
            End Try
        End If
        AccessConn.Close()
    End Sub
    Public Sub Patch3Rech1()
        'Add field Gutschrift to Rech1
        Dim NeedPatch3 As Boolean = False
        Dim AccessConn As New System.Data.OleDb.OleDbConnection
        OpenDBConnection(AccessConn)

        Dim AccessCommand As New System.Data.OleDb.OleDbCommand
        Dim c As Integer
        AccessCommand.Connection = AccessConn
        Try
            AccessCommand.CommandText = "Select Gutschrift from rech1"
            c = AccessCommand.ExecuteNonQuery()
            Debug.Print("Patch3Rech will NOT be applied")
        Catch ex2 As OleDbException
            'the field does not exist
            NeedPatch3 = True
            Debug.Print("Patch3Rech MUST be applied")
        End Try
        If NeedPatch3 Then
            Try
                AccessCommand.CommandText = "ALTER table " & "rech1" & " ADD COLUMN Gutschrift YESNO DEFAULT FALSE"
                c = AccessCommand.ExecuteNonQuery()
                Debug.Print("'ADD COLUMN Gutschrift YESNO DEFAULT FALSE' abgeschlossen")
            Catch ex2 As OleDbException
                Debug.Print(ex2.Message & " Patch3Rech1")
                MessageBox.Show(ex2.Message & " ### Fehler", "Patch3Rech1")
            End Try
        End If
        AccessConn.Close()
    End Sub
    Public Sub Patch2Rech1()
        'fieldsArtStamm As String = "ARTIKELNR, ARTIKELBEZ, BESTAND, VK, LAGERORT, 
        'LETZ_EK, MISCH_EK, RABATTCODE, MIND_MENGE, LETZ_BESTL, LETZ_VK, LETZEKMENG, 
        'LIEFER1, EK1, LIEFER2, EK2, LIEFER3, EK3"
        'plus ArtTyp und id
        'PLUS FzgTyp

        Dim NeedPatch1 As Boolean = False
        Dim AccessConn As New System.Data.OleDb.OleDbConnection
        OpenDBConnection(AccessConn)

        Dim AccessCommand As New System.Data.OleDb.OleDbCommand
        Dim c As Integer
        AccessCommand.Connection = AccessConn
        Try
            AccessCommand.CommandText = "Select Bezahlt, BezDatum from rech1"
            c = AccessCommand.ExecuteNonQuery()
            Debug.Print("Patch2Rech will NOT be applied")
        Catch ex2 As OleDbException
            'the field does not exist
            NeedPatch1 = True
            Debug.Print("Patch2Rech MUST be applied")
        End Try
        If NeedPatch1 Then
            Try
                AccessCommand.CommandText = "ALTER table " & "rech1" & " ADD COLUMN Bezahlt YESNO DEFAULT FALSE, BezDatum DATETIME"
                c = AccessCommand.ExecuteNonQuery()
                Debug.Print("'ADD COLUMN Bezahlt YESNO DEFAULT FALSE, BezDatum DATETIME' abgeschlossen")
            Catch ex2 As OleDbException
                Debug.Print(ex2.Message & " Patch2Rech1")
                MessageBox.Show(ex2.Message & " ### Fehler", "Patch2Rech")
            End Try
        End If
        AccessConn.Close()
    End Sub
    ''' <summary>
    ''' Change AltTeilMwSt to use only MwSt of 10th of Altteile
    ''' changed applied with version 2.1.2.0
    ''' </summary>
    ''' <remarks>
    ''' AltteilMwSt maybe 20.9% in total
    ''' this was calulated outside the normal MwSt calculation
    ''' Now, this will be split in normal MwSt of 19%
    ''' and additionaly 19% of Altteil-value divided by 10 (10% of value)
    ''' so we calc 1.9% additionally to 19%</remarks>
    Public Sub Patch5AltTeilMwSt2()
        Dim NeedPatch5 As Boolean = False
        Dim AccessConn As New System.Data.OleDb.OleDbConnection
        OpenDBConnection(AccessConn)

        Dim AccessCommand As New System.Data.OleDb.OleDbCommand
        Dim c As Integer
        AccessCommand.Connection = AccessConn

        Try
            Dim rdr As OleDbDataReader = Nothing
            AccessCommand.CommandText = "Select AltTeilMwSt, MwSt_Satz1 from FIRMSTAM"
            rdr = AccessCommand.ExecuteReader
            If rdr.HasRows Then
                rdr.Read()
                Dim dAltTeilMwSt As Double
                dAltTeilMwSt = CDouble(rdr.Item("AltTeilMwSt").ToString())
                Dim dMwStSatz As Double = CDouble(rdr.Item("MwSt_Satz1").ToString())
                If dAltTeilMwSt = dMwStSatz / 10 Then
                    NeedPatch5 = False
                    Debug.Print("Patch5AltTeilMwSt2 (AltTeilMwSt) will NOT be applied")
                Else
                    NeedPatch5 = True
                    Debug.Print("Patch5AltTeilMwSt2 (AltTeilMwSt) will be applied")
                End If
            Else
                NeedPatch5 = True
                Debug.Print("Patch5AltTeilMwSt2 (AltTeilMwSt) will be applied")
            End If
            rdr.Close()
        Catch ex2 As OleDbException
            'the field does not exist
            NeedPatch5 = True
            Debug.Print("Exception: " + ex2.Message + vbCrLf + " Patch1FirmStam (AltTeilMwSt) MUST be applied")
        End Try

        If NeedPatch5 Then
            'Change existing AltTeilMwSt
            Try
                AccessCommand.CommandText = "Update FirmStam Set AltTeilMwSt=1.9 where id=1"
                c = AccessCommand.ExecuteNonQuery()
                Debug.Print("'" & AccessCommand.CommandText & "' abgeschlossen")
            Catch ex2 As OleDbException
                Debug.Print(ex2.Message & " Patch5AltTeilMwSt2")
                MessageBox.Show(ex2.Message & " ### Fehler", "Patch5AltTeilMwSt2")
            End Try
            '1.9% als AltTeilMwSt für bestehende Rechnungen in RECH1 einsetzen
            Try
                AccessCommand.CommandText = "Update Rech1 Set AltTeilMwSt=AltTeilMwSt/10"
                c = AccessCommand.ExecuteNonQuery()
                Debug.Print("'" & AccessCommand.CommandText & "' abgeschlossen")
            Catch ex2 As OleDbException
                Debug.Print(ex2.Message & " Patch5AltTeilMwSt2")
                MessageBox.Show(ex2.Message & " ### Fehler", "Patch5AltTeilMwSt2")
            End Try
        End If
        AccessConn.Close()
    End Sub
    ''' <summary>
    ''' Patch FirmStamm Daten to incl. field for AltTeilMwSt
    ''' Patch Rech1 add one field for AltTeilMwSt and set defaults for existing data
    ''' </summary>
    ''' <remarks>Can you read this</remarks>
    Public Sub Patch2FirmStam()
        Dim NeedPatch2 As Boolean = False
        Dim AccessConn As New System.Data.OleDb.OleDbConnection
        OpenDBConnection(AccessConn)

        Dim AccessCommand As New System.Data.OleDb.OleDbCommand
        Dim c As Integer
        AccessCommand.Connection = AccessConn
        Try
            AccessCommand.CommandText = "Select AltTeilMwSt from FIRMSTAM"
            c = AccessCommand.ExecuteNonQuery()
            Debug.Print("Patch2FirmStam (AltTeilMwSt) will NOT be applied")
        Catch ex2 As OleDbException
            'the field does not exist
            NeedPatch2 = True
            Debug.Print("Patch1FirmStam (AltTeilMwSt) MUST be applied")
        End Try
        If NeedPatch2 Then
            Try
                AccessCommand.CommandText = "ALTER table " & "FirmStam" & " ADD COLUMN AltTeilMwSt DOUBLE"
                c = AccessCommand.ExecuteNonQuery()
                Debug.Print("'" & AccessCommand.CommandText & "' abgeschlossen")
            Catch ex2 As OleDbException
                Debug.Print(ex2.Message & " Patch2FirmStam")
                MessageBox.Show(ex2.Message & " ### Fehler", "Patch2FirmStam")
            End Try
            'Add Standard Fusstext
            Try
                AccessCommand.CommandText = "Update FirmStam Set AltTeilMwSt=20.9 where id=1"
                c = AccessCommand.ExecuteNonQuery()
                Debug.Print("'" & AccessCommand.CommandText & "' abgeschlossen")
            Catch ex2 As OleDbException
                Debug.Print(ex2.Message & " Patch2FirmStam")
                MessageBox.Show(ex2.Message & " ### Fehler", "Patch2FirmStam")
            End Try

            'add new column into RECH1 for saving AltTeilMwSt
            Try
                AccessCommand.CommandText = "ALTER table " & "RECH1" & " ADD COLUMN AltTeilMwSt DOUBLE"
                c = AccessCommand.ExecuteNonQuery()
                Debug.Print("'" & AccessCommand.CommandText & "' abgeschlossen")
            Catch ex2 As OleDbException
                Debug.Print(ex2.Message & " Patch2FirmStam")
                MessageBox.Show(ex2.Message & " ### Fehler", "Patch2FirmStam")
            End Try

            '19% als AltTeilMwSt für bestehende Rechnungen in RECH1 einsetzen
            Try
                AccessCommand.CommandText = "Update Rech1 Set AltTeilMwSt=19"
                c = AccessCommand.ExecuteNonQuery()
                Debug.Print("'" & AccessCommand.CommandText & "' abgeschlossen")
            Catch ex2 As OleDbException
                Debug.Print(ex2.Message & " Patch2FirmStam")
                MessageBox.Show(ex2.Message & " ### Fehler", "Patch2FirmStam")
            End Try
        End If
        AccessConn.Close()
    End Sub
    Public Sub Patch1FirmStam()

        Dim NeedPatch1 As Boolean = False
        Dim AccessConn As New System.Data.OleDb.OleDbConnection
        OpenDBConnection(AccessConn)

        Dim AccessCommand As New System.Data.OleDb.OleDbCommand
        Dim c As Integer
        AccessCommand.Connection = AccessConn
        Try
            AccessCommand.CommandText = "Select FUSSTEXT from FIRMSTAM"
            c = AccessCommand.ExecuteNonQuery()
            Debug.Print("Patch1FirmStam will NOT be applied")
        Catch ex2 As OleDbException
            'the field does not exist
            NeedPatch1 = True
            Debug.Print("Patch1FirmStam MUST be applied")
        End Try
        If NeedPatch1 Then
            Try
                AccessCommand.CommandText = "ALTER table " & "FirmStam" & " ADD COLUMN FussText char(240)"
                c = AccessCommand.ExecuteNonQuery()
                Debug.Print("'" & AccessCommand.CommandText & "' abgeschlossen")
            Catch ex2 As OleDbException
                Debug.Print(ex2.Message & " Patch1FirmStam")
                MessageBox.Show(ex2.Message & " ### Fehler", "Patch1FirmStam")
            End Try
        End If
        'Add Standard Fusstext
        Try
            AccessCommand.CommandText = "Update FirmStam Set FussText='" + _
            "Bei Zahlungen und Rückfragen bitte Rechnungsnummer angeben." + vbCrLf + _
            "Der Auftrag wurde unter Anerkennung der Bedingungen" + vbCrLf + _
            "für die Ausführung von Arbeiten erteilt." + _
            "' where id=1"
            '"Betrag dankend erhalten      Datum:____________________  Unterschrift:________________________________________________'" + _
            c = AccessCommand.ExecuteNonQuery()
            Debug.Print("'" & AccessCommand.CommandText & "' abgeschlossen")
        Catch ex2 As OleDbException
            Debug.Print(ex2.Message & " Patch1FirmStam")
            MessageBox.Show(ex2.Message & " ### Fehler", "Patch1FirmStam")
        End Try

        AccessConn.Close()
    End Sub
    Public Sub FillFzgTypenListe(ByVal lst As Windows.Forms.ComboBox)
        Dim Anzahl As Integer = 0
        Dim cmd As New OleDbCommand
        Dim cn As New OleDbConnection
        Dim rdr As OleDbDataReader = Nothing
        Dim sql As String = "Select distinct XTyp from rech1"
        OpenDBConnection(cn)
        cmd.Connection = cn
        cmd.CommandText = sql
        Try
            lst.Items.Clear()
            cmd.CommandText = sql
            rdr = cmd.ExecuteReader
            If rdr.HasRows Then
                While rdr.Read
                    lst.Items.Add(rdr.Item(0).ToString)
                End While
            End If
        Catch dx As OleDbException
            Debug.Print("Exception in FillFzgTypenListe(): " & dx.Message)
        Finally
            If IsNothing(rdr) = False Then rdr.Close()
            cn.Close()
        End Try

    End Sub
    Public Function fEingabeBox(ByVal prompt As String, ByVal titel As String, ByVal defaultstring As String, ByVal oben As Integer, ByVal links As Integer) As String
        Dim dlg As EingabeBox = New EingabeBox
        Dim rueckgabe As String
        dlg.Text = titel
        dlg.txtWert.Text = defaultstring
        dlg.prompt.Text = prompt
        dlg.Top = oben
        dlg.Left = links
        If dlg.ShowDialog = DialogResult.OK Then
            rueckgabe = dlg.txtWert.Text
            dlg.Dispose()
            Return rueckgabe
        Else
            dlg.Dispose()
            Return ""
        End If
    End Function
    Public Sub UpdateRichtwerte(ByVal rwNr As String, ByVal AwText As String, ByVal Preis As Double, ByVal FzgTyp As String)
        Dim Anzahl As Integer = 0
        Dim cmd As New OleDbCommand
        Dim cn As New OleDbConnection
        Dim rdr As OleDbDataReader = Nothing
        Dim sql As String = ""
        Dim fehler As Integer = 0
        OpenDBConnection(cn)
        cmd.Connection = cn
        'update richtwerte1
        If ExistsData("Select * from richtwerte1 where ArtNr='" & rwNr & "'") Then
            'Update
            Try
                sql = "Update richtwerte1 set AwText='" & AwText & "' where ArtNr='" & rwNr & "'"
                cmd.CommandText = sql
                Anzahl = cmd.ExecuteNonQuery
            Catch x As OleDbException
                fehler = fehler + 1
                Debug.Print("Exception in UpdateRichtwerte 1: " & vbCrLf & sql & vbCrLf & x.Message)
            End Try
        Else
            'Insert
            Try
                sql = "Insert Into richtwerte1 (AwText, ArtNr) VALUES ('" & AwText & "', '" & rwNr & "')"
                cmd.CommandText = sql
                Anzahl = cmd.ExecuteNonQuery
            Catch x As OleDbException
                fehler = fehler + 1
                Debug.Print("Exception in UpdateRichtwerte 2: " & vbCrLf & sql & vbCrLf & x.Message)
            End Try
        End If
        'update richtwerte2 (ArtNr, FzgTyp, Preis
        If ExistsData("Select * from richtwerte2 where ArtNr='" & rwNr & "' AND FzgTyp='" & FzgTyp & "'") Then
            'Update
            Try
                sql = "Update richtwerte2 set Preis=" & CDoubleS(Preis) & " where ArtNr='" & rwNr & "' AND FzgTyp='" & FzgTyp & "'"
                cmd.CommandText = sql
                Anzahl = cmd.ExecuteNonQuery
            Catch x As OleDbException
                Debug.Print("Exception in UpdateRichtwerte 3: " & vbCrLf & sql & vbCrLf & x.Message)
                fehler = fehler + 1
            End Try
        Else
            'Insert
            Try
                sql = "Insert Into richtwerte2 (ArtNr, FzgTyp, Preis) VALUES ('" & rwNr & "', '" & FzgTyp & "', " & CDoubleS(Preis) & ")"
                cmd.CommandText = sql
                Anzahl = cmd.ExecuteNonQuery
            Catch x As OleDbException
                Debug.Print("Exception in UpdateRichtwerte 4: " & vbCrLf & sql & vbCrLf & x.Message)
                fehler = fehler + 1
            End Try
        End If
        cn.Close()
        If fehler > 0 Then MessageBox.Show("Es sind " & fehler & " aufgetreten!", "UpdateRichtwerte")
    End Sub
    Sub DumpTbl(ByVal tbl As System.Data.DataTable)
        Dim i As Integer
        Debug.Print("Table DUMP: ")
        For i = 0 To tbl.Rows(0).Table.Columns.Count - 1
            Debug.Print(tbl.Rows(0).Table.Columns(i).Caption)
        Next

    End Sub
    Function AuswertungenReadDS() As DataSet
        Dim ds As DataSet
        ds = New DataSet("Auswertungen")
        Dim xm As XmlReadMode
        Try
            xm = ds.ReadXml(getAppDir() + "auswertungen.xml")
        Catch ex As Exception
            ds = Nothing
        End Try
        Return ds
    End Function
    Function AuswertungenWriteDS(ByVal ds As DataSet) As Boolean
        Try
            ds.WriteXml(getAppDir() + "auswertungen.xml")
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Function AuswertungenCreateDS() As DataSet
        Dim ds As DataSet
        ds = New DataSet("Auswertungen")
        Dim dt As DataTable
        dt = New DataTable("Auswertungen")
        Dim dr As DataRow
        Dim dc As DataColumn

        dc = New DataColumn
        dc.Caption = "StringUI"
        dc.ColumnName = "StringUI"
        dc.DataType = Type.GetType("System.String")
        dt.Columns.Add(dc)

        dt.Columns.Add("stringSQL", Type.GetType("System.String"))
        dt.Columns.Add("stringSQLprint", Type.GetType("System.String"))
        '"select * from (SELECT kundenst.nachname as Name, SUM(rech1.xnetto), rech1.gutschrift AS Gesamt FROM RECH1 INNER JOIN KUNDENST ON RECH1.XKUNDENNR=KUNDENST.KUNDENNR GROUP BY kundenst.nachname ) order by Gesamt desc"
        dr = dt.NewRow
        dr("StringUI") = "Umsatz nach Kunden"
        'dr("stringSQL") = "select * from (SELECT kundenst.nachname as Name, SUM(rech1.xnetto) AS Gesamt " & _
        '                "FROM RECH1 where RECH1.XKUNDENNR=KUNDENST.KUNDENNR AND rech1.gutschrift=FALSE " & _
        '                "GROUP BY kundenst.nachname ) order by Gesamt desc"
        dr("StringSQL") = "select * from (SELECT kundenst.nachname as Name, SUM(rech1.xnetto) AS Gesamt FROM RECH1, KundenSt where RECH1.XKUNDENNR=KUNDENST.KUNDENNR  AND rech1.gutschrift=FALSE GROUP BY kundenst.nachname ) order by Gesamt desc"
        dr("stringSQLprint") = "select 'Name', 0, 30, 'L', 'N', '', 1, NAME, " & _
                   "'Gesamt', 31, 45, 'R', 'Y', '#,##0.00', 1, GESAMT" & _
                   " from (SELECT kundenst.nachname as Name, SUM(rech1.xnetto) AS Gesamt, rech1.gutschrift " & _
            "FROM RECH1 INNER JOIN KUNDENST ON RECH1.XKUNDENNR=KUNDENST.KUNDENNR " & _
            "GROUP BY kundenst.nachname ) order by Gesamt desc"
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("StringUI") = "Umsatz nach Monat"
        dr("stringSQL") = "SELECT YEAR(XDATUM) as Jahr, MONTH(XDATUM) as Monat, " & _
                        "SUM(XNETTO) as Gesamt FROM RECH1 group by YEAR(XDATUM), MONTH(XDATUM)"
        dr("stringSQLprint") = "select 'Jahr', 0, 15, 'L', 'N', '', 1, Jahr, " & _
                   "'Monat', 16, 30, 'L', 'N', '', 1, Monat, " & _
                   "'Gesamt', 31, 45, 'R', 'Y', '#,##0.00', 1, GESAMT" & _
                   " from (" & _
                   "SELECT YEAR(XDATUM) as Jahr, MONTH(XDATUM) as Monat, " & _
                   "SUM(XNETTO) as Gesamt FROM RECH1 group by YEAR(XDATUM), MONTH(XDATUM)" & _
                        " WHERE Gutschrift=FALSE" & _
                        ")"
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("StringUI") = "Umsatz nach Artikel"
        dr("stringSQL") = "select ARTIKELNR, ARTIKELBEZ, Gesamt from (" & _
                        "SELECT ARTIKELNR, ARTIKELBEZ, " & _
                        "sum (menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt " & _
                        "FROM RECH2 group by ARTIKELNR, ARTIKELBEZ ) WHERE GESAMT>100 order by gesamt desc" 
        dr("stringSQLprint") = "select 'ArtikelNr', 0, 15, 'L', 'N', '', 1, ARTIKELNR, " & _
                        "'Artikelbez', 16, 40, 'L', 'N', '', 1, ARTIKELBEZ, " & _
                        "'Gesamt', 41, 55, 'R', 'Y', '#,##0.00', 1, GESAMT" & _
                        " from (" & _
                        "select ARTIKELNR, ARTIKELBEZ, Gesamt from (" & _
                        "SELECT ARTIKELNR, ARTIKELBEZ, " & _
                        "sum (menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt " & _
                        "FROM RECH2 group by ARTIKELNR, ARTIKELBEZ ) WHERE GESAMT>100 order by gesamt desc" & _
                        ")"
        dt.Rows.Add(dr)

        'fällige Termine
        dr = dt.NewRow
        Dim dieserMonat As String
        Dim diesesJahr As String
        dieserMonat = Now.ToString("MM") ' String.Format("dd", Now())
        diesesJahr = Now.ToString("yy") 'String.Format("yy", Now())
        Dim testString As String
        testString = """" + dieserMonat + "_" + diesesJahr + """" 'use ANSI SQL _ instead of ?
        dr("StringUI") = "Diesen Monat fällige Termine"
        dr("StringSQL") = "select f.Kennzeich, " & _
                                    "f.TUEV, " & _
                                    "f.ASU, " & _
                                    "f.Schreiber, " & _
                                    "f.SICHER, " & _
                                    "k.kundennr, " & _
                                    "k.VORNAME, " & _
                                    "k.NACHNAME, " & _
                                    "k.strasse, " & _
                                    "k.PLZ, " & _
                                    "k.ort " & _
                                    " from KUNDFAHR as f, KUNDENST as k" & _
                                    " where k.KUNDENNR=f.KUNDEN_NR AND " & _
                                    " (f.TUEV LIKE " + testString + _
                                    " OR f.ASU LIKE " + testString + _
                                    " OR f.Schreiber LIKE " + testString + _
                                    " OR f.Sicher LIKE " + testString + ")" + _
                                    " order by k.NACHNAME;"
        'select 'Kennzeichen', 0,15, 'L', 'N', '', 1, f.Kennzeich, 'TÃœV', 15, 25, 'L', 'N', '', 1, f.TUEV, 'ASU', 25, 35, 'L', 'N', '', 1, f.ASU, 'Schreiber', 35, 45, 'L', 'N', '', 1, f.schreiber  FROM (select f.Kennzeich, f.TUEV, f.ASU, f.Schreiber, f.SICHER, k.kundennr, k.VORNAME, k.NACHNAME, k.strasse, k.PLZ, k.ort  from KUNDFAHR as f, KUNDENST as k where k.KUNDENNR=f.KUNDEN_NR AND  (f.TUEV LIKE "06_08" OR f.ASU LIKE "06_08" OR f.Schreiber LIKE "06_08" OR f.Sicher LIKE "06_08") order by k.NACHNAME)
        dr("stringSQLprint") = "select 'Kennzeichen', 0, 15, 'L', 'N', '', 1, f.Kennzeich, " & _
                                      "'TÜV', 15, 25, 'L', 'N', '', 1, f.TUEV, " & _
                                      "'ASU', 25, 35, 'L', 'N', '', 1, f.ASU, " & _
                                      "'Schreiber', 35, 45, 'L', 'N', '', 1, f.schreiber, " & _
                                      "'Sicher', 45, 55, 'L', 'N', '', 1, f.SICHER, " & _
                                      "'KNR', 0, 16, 'L', 'N', '', 2, k.kundennr, " & _
                                      "'Vorname', 20, 40, 'L', 'N', '', 2, k.VORNAME, " & _
                                      "'Nachnahme', 41, 80, 'L', 'N', '', 2, k.NACHNAME, " & _
                                      "'Strasse', 11, 40, 'L', 'N', '', 3, k.strasse, " & _
                                      "'PLZ', 41, 50, 'L', 'N', '', 3, k.PLZ, " & _
                                      "'ORT', 51, 71, 'L', 'N', '', 3, k.ORT " & _
                                " FROM (" & _
                                    "select f.Kennzeich, " & _
                                    "f.TUEV, " & _
                                    "f.ASU, " & _
                                    "f.Schreiber, " & _
                                    "f.SICHER, " & _
                                    "k.kundennr, " & _
                                    "k.VORNAME, " & _
                                    "k.NACHNAME, " & _
                                    "k.strasse, " & _
                                    "k.PLZ, " & _
                                    "k.ort " & _
                                    " from KUNDFAHR as f, KUNDENST as k" & _
                                    " where k.KUNDENNR=f.KUNDEN_NR AND " & _
                                    " (f.TUEV LIKE " + testString + _
                                    " OR f.ASU LIKE " + testString + _
                                    " OR f.Schreiber LIKE " + testString + _
                                    " OR f.Sicher LIKE " + testString + ")" + _
                                    " order by k.NACHNAME, f.kennzeich" + _
                                ")"
        dt.Rows.Add(dr)
        '----- Set SQL string for report by this format (1 set per 1 report column):
        '----- Column 1 -> column name
        '----- Column 2 -> Start position on paper, I set all width has range between 0-99
        '----- Column 3 -> End position on paper, I set all width has range between 0-99
        '----- Column 4 -> Justify (L-Left, R-Right, C-Center)
        '----- Column 5 -> Has summarize in this column (Y/N)
        '----- Column 6 -> Display format(such as #,##0.00)
        '----- Column 7 -> Rest in line? Begin with 1
        '----- Column 8 -> Data Column

        dr = dt.NewRow
        dr("StringUI") = "Umsatz nach Artikel ohne ZEIT"
        dr("stringSQL") = "select ARTIKELNR, ARTIKELBEZ, Gesamt from (" & _
                        "SELECT ARTIKELNR, ARTIKELBEZ, " & _
                        "sum (menge * e_preis - (RABATT/100 * (menge * e_preis))) as Gesamt " & _
                        "FROM RECH2 where ARTIKELNR NOT Like 'ZEIT%' group by ARTIKELNR, ARTIKELBEZ ) order by gesamt desc"
        dr("stringSQLprint") = ""
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("StringUI") = "Fahrzeugumsatz nach Kennzeichen"
        dr("stringSQL") = "SELECT kundenst.nachname as Name, RECH1.XKZ as Kennzeichen, sum(XNETTO) as Gesamt FROM RECH1, kundenst where kundenst.kundennr=rech1.xkundennr GROUP BY kundenst.nachname, RECH1.XKZ order by xkz"
        dr("stringSQLprint") = ""
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("StringUI") = "Fahrzeugumsatz nach Kunden"
        dr("stringSQL") = "SELECT kundenst.nachname as Name, RECH1.XKZ as Kennzeichen, sum(XNETTO) as Gesamt FROM RECH1, kundenst where kundenst.kundennr=rech1.xkundennr GROUP BY kundenst.nachname, RECH1.XKZ order by kundenst.nachname"
        dr("stringSQLprint") = ""
        dt.Rows.Add(dr)

        ds.Tables.Add(dt)
        Return ds
    End Function
    'new in 2.0.0.8
    Public Function WordWrap(ByVal strText As String, ByVal iWidth As Integer) As String
        Dim chs() As Char = {" "c, "."c, "-"c, "/"c, ","c}

        If strText.Length <= iWidth Then Return strText ' dont need to do anything
        Dim sResult As String = strText
        Dim sChar As String             ' temp holder for current string char
        Dim iEn As Integer
        Dim iLineNO As Integer = iWidth
        Do While sResult.Length > iLineNO       'changed from >= to > in 2.0.1.4
            For iEn = iLineNO To 1 Step -1         ' work backwards from the max len to 1 looking for a space
                sChar = sResult.Chars(iEn)
                If sChar = " " Then             ' found a space
                    sResult = sResult.Remove(iEn, 1)     ' Remove the space
                    sResult = sResult.Insert(iEn, vbCr)     ' insert a line feed here,
                    iLineNO += iWidth             ' increment
                    Exit For
                End If
            Next
        Loop
        Return sResult
    End Function
    Public Function String2Hex(ByVal TheString As String) As String
        Dim StringString As String = ""
        Dim HexString As String = ""

        On Error GoTo String2HexError
        StringString = TheString
        Do Until StringString = ""
            If Len(Hex$(Asc(Mid$(StringString, 1, 1)))) < 2 Then
                HexString = HexString & "0" & Hex$(Asc(Mid$(StringString, 1, 1)))
                StringString = Mid$(StringString, 2)
            Else
                HexString = HexString & Hex$(Asc(Mid$(StringString, 1, 1)))
                StringString = Mid$(StringString, 2)
            End If
        Loop
        String2Hex = UCase$(HexString)

        Exit Function

String2HexError:
        String2Hex = "ERROR!"
        'ErrorOccurred = True

    End Function
    Public Sub scrollGrid(ByRef dataGridView As DataGridView)
        If dataGridView.SelectedRows.Count = 0 Then Exit Sub
        If dataGridView.SelectedRows(0).Index = -1 Then Exit Sub
        Dim halfWay As Integer = CInt(dataGridView.DisplayedRowCount(False) / 2)
        If dataGridView.FirstDisplayedScrollingRowIndex + halfWay > dataGridView.SelectedRows(0).Index OrElse _
           (dataGridView.FirstDisplayedScrollingRowIndex + dataGridView.DisplayedRowCount(False) - halfWay) <= dataGridView.SelectedRows(0).Index Then
            Dim targetRow As Integer = dataGridView.SelectedRows(0).Index

            targetRow = Math.Max(targetRow - halfWay, 0)

            dataGridView.FirstDisplayedScrollingRowIndex = targetRow
        End If
    End Sub
    Public Function DBassignPassword() As Boolean
        Dim provider As String
        Dim ocn As New OleDbConnection
        Dim datasource As String
        Dim cmd As OleDbCommand = New OleDbCommand
        Dim b_ret As Boolean = True
        provider = "provider=Microsoft.Jet.OLEDB.4.0;data source = "
        datasource = DBfileName
        'datasource = "kbfakt5.mdb"
        connectionString = provider + datasource + "; Mode=12" 'open exclusive
        ocn.ConnectionString = connectionString
        'DBCon.Properties("Jet OLEDB:Database Password") = "jason"
        'Provider=Microsoft.Jet.OLEDB.4.0; Data Source=d:\Northwind.mdb;User ID=Admin;Password=; 

        Try
            'First try the Application dir for the mdb file
            ocn.Open()
        Catch ex As System.Data.OleDb.OleDbException
            b_ret = False
            System.Diagnostics.Debug.WriteLine("Exception in OpenDBConnection:" + ex.Message)
        End Try
        If (b_ret) Then
            Try
                Dim PassNew As String = PasswordDecrypt("fkrsshu")
                Dim PassOld As String = "NULL"
                cmd.Connection = ocn
                cmd.CommandText = "ALTER DATABASE PASSWORD " + PassNew + " " + PassOld 'change password
                cmd.ExecuteNonQuery()
                'we have to close the db and reopen it with a password
                ocn.Close()
                'now check with password to read some data
                ocn.ConnectionString += ";Jet OLEDB:Database Password=" + PasswordDecrypt("fkrsshu")
                ocn.Open()
                cmd.Connection = ocn
                cmd.CommandText = "select count (XAUFTR_NR) from RECH1"
                Dim i As Integer = System.Convert.ToInt32(cmd.ExecuteScalar())
                If i = 0 Then
                    b_ret = False
                Else
                    b_ret = True
                End If
            Catch ox As OleDb.OleDbException
                System.Diagnostics.Debug.WriteLine("Exception in DBassignPassword: " + ox.Message)
                b_ret = False
            End Try
        End If
        ocn.Close()
        ocn.Dispose()
        Return b_ret
    End Function

    Public Function DBdeletePassword(ByVal dbFile As String) As Boolean
        Dim conn As ADODB.Connection
        Dim strPath As String = dbFile
        Dim b_ret As Boolean = False
        conn = New ADODB.Connection
        Try
            With conn
                .Mode = ADODB.ConnectModeEnum.adModeShareExclusive
                .Open("provider=Microsoft.Jet.OleDb.4.0; Data Source =" + strPath + ";Jet OLEDB:Database Password = " + PasswordDecrypt("fkrsshu") + ";")
                .Execute("ALTER DATABASE PASSWORD NULL [" + PasswordDecrypt("fkrsshu") + "];")
            End With
            b_ret = True
            System.Diagnostics.Debug.WriteLine("DBdeletePassword: succeeded")
        Catch ex As Exception
            b_ret = False
            System.Diagnostics.Debug.WriteLine("Exception in DBdeletePassword: " + ex.Message)
            If Not conn Is Nothing Then
                'If conn.State = adStateOpen Then 
                conn.Close()
            End If
        End Try
        If conn.State = ConnectionState.Open Then conn.Close()
        conn = Nothing
        Return b_ret
    End Function
    Function DBdeletePassword() As Boolean
        Dim conn As ADODB.Connection
        Dim strPath As String = DBfileName
        Dim b_ret As Boolean = False
        conn = New ADODB.Connection
        Try
            With conn
                .Mode = ADODB.ConnectModeEnum.adModeShareExclusive
                .Open("provider=Microsoft.Jet.OleDb.4.0; Data Source =" + strPath + ";Jet OLEDB:Database Password = " + PasswordDecrypt("fkrsshu") + ";")
                .Execute("ALTER DATABASE PASSWORD NULL [" + PasswordDecrypt("fkrsshu") + "];")
            End With
            b_ret = True
            System.Diagnostics.Debug.WriteLine("DBdeletePassword: succeeded")
        Catch ex As Exception
            b_ret = False
            System.Diagnostics.Debug.WriteLine("Exception in DBdeletePassword: " + ex.Message)
            If Not conn Is Nothing Then
                'If conn.State = adStateOpen Then 
                conn.Close()
            End If
        End Try
        If conn.State = ConnectionState.Open Then conn.Close()
        conn = Nothing
        Return b_ret
    End Function

    Public Function DBhasPassword(ByVal dbFile As String) As Boolean
        Dim b_ret As Boolean = True
        Dim provider As String
        Dim ocn As New OleDbConnection
        Dim datasource As String
        Dim cmd As OleDbCommand = New OleDbCommand
        provider = "provider=Microsoft.Jet.OLEDB.4.0;data source = "
        datasource = dbFile
        'datasource = "kbfakt5.mdb"
        connectionString = provider + datasource + "; Mode=12" 'open exclusive
        ocn.ConnectionString = connectionString
        '2.0.2.1 MDB Password
        ' try to open with password
        'ocn.ConnectionString += ";Jet OLEDB:Database Password=" + PasswordDecrypt("fkrsshu")
        Try
            ocn.Open()
            cmd.Connection = ocn
            'check if db has no password
            cmd.CommandText = "ALTER DATABASE PASSWORD NULL NULL"
            cmd.ExecuteNonQuery()
            b_ret = False 'the db has NO password
        Catch ex As System.Data.OleDb.OleDbException
            b_ret = True
            System.Diagnostics.Debug.WriteLine("Exception in DBHasPassword2:" + ex.Message)
        End Try
        'if password, test data reading
        If (b_ret) Then
            Try
                ocn.Close()
                'now check with password to read some data
                ocn.ConnectionString += ";Jet OLEDB:Database Password=" + PasswordDecrypt("fkrsshu")
                ocn.Open()
                cmd.Connection = ocn
                cmd.CommandText = "select count (XAUFTR_NR) from RECH1" '"select count (XAUFTR_NR) from RECH1"
                Dim i As Integer = System.Convert.ToInt32(cmd.ExecuteScalar())
                If i = 0 Then
                    b_ret = False
                Else
                    b_ret = True
                End If
            Catch ox As OleDb.OleDbException
                System.Diagnostics.Debug.WriteLine("Exception in DBHasPassword2: " + ox.Message)
                b_ret = False
            End Try
        End If
        ocn.Close()
        ocn.Dispose()
        Return b_ret
    End Function
    Public Function DBHasPassword() As Boolean
        Dim b_ret As Boolean = True
        Dim provider As String
        Dim ocn As New OleDbConnection
        Dim datasource As String
        Dim cmd As OleDbCommand = New OleDbCommand
        provider = "provider=Microsoft.Jet.OLEDB.4.0;data source = "
        datasource = DBfileName
        'datasource = "kbfakt5.mdb"
        connectionString = provider + datasource + "; Mode=12" 'open exclusive
        ocn.ConnectionString = connectionString
        '2.0.2.1 MDB Password
        ' try to open with password
        'ocn.ConnectionString += ";Jet OLEDB:Database Password=" + PasswordDecrypt("fkrsshu")
        Try
            ocn.Open()
            cmd.Connection = ocn
            'check if db has no password
            cmd.CommandText = "ALTER DATABASE PASSWORD NULL NULL"
            cmd.ExecuteNonQuery()
            b_ret = False 'the db has NO password
        Catch ex As System.Data.OleDb.OleDbException
            b_ret = True
            System.Diagnostics.Debug.WriteLine("Exception in DBHasPassword2:" + ex.Message)
        End Try
        'if password, test data reading
        If (b_ret) Then
            Try
                ocn.Close()
                'now check with password to read some data
                ocn.ConnectionString += ";Jet OLEDB:Database Password=" + PasswordDecrypt("fkrsshu")
                ocn.Open()
                cmd.Connection = ocn
                cmd.CommandText = "select count (XAUFTR_NR) from RECH1" '"select count (XAUFTR_NR) from RECH1"
                Dim i As Integer = System.Convert.ToInt32(cmd.ExecuteScalar())
                If i = 0 Then
                    b_ret = False
                Else
                    b_ret = True
                End If
            Catch ox As OleDb.OleDbException
                System.Diagnostics.Debug.WriteLine("Exception in DBHasPassword2: " + ox.Message)
                b_ret = False
            End Try
        End If
        ocn.Close()
        ocn.Dispose()
        Return b_ret
    End Function
    Public Function PatchPassword() As Boolean
        If DBhasPassword() Then
            System.Diagnostics.Debug.WriteLine("DB has Pw")
            Return True
            'DBdeletePassword()
            'If DBdeletePassword(DBfileName) Then
            '    System.Diagnostics.Debug.WriteLine("Pw removed ok")
            'Else
            '    System.Diagnostics.Debug.WriteLine("Pw removed FAILED")
            'End If
        Else
            System.Diagnostics.Debug.WriteLine("DB has no Pw")
            If DBassignPassword() Then
                System.Diagnostics.Debug.WriteLine("Pw assigned ok")
                Return True
            Else
                System.Diagnostics.Debug.WriteLine("Pw assignment FAILED")
                Return False
            End If
        End If
    End Function
    Private Sub DataGridExport2CSV(ByRef myDataGridView As DataGridView)
        Dim RowsCount As Int32 = 0
        If myDataGridView.SelectedRows.Count > 0 Then
            RowsCount = myDataGridView.SelectedRows.Count
        Else
            RowsCount = myDataGridView.Rows.Count
        End If
        Dim antwort As Integer
        antwort = MessageBox.Show("Wollen Sie wirklich " & (RowsCount).ToString & " Daten exportieren?", "Export nach CSV Textdatei", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        If antwort = DialogResult.No Then
            Return
        End If
        Dim dateiname As String = getAppDir() + "export.csv"
        Dim dlg As System.Windows.Forms.SaveFileDialog = New System.Windows.Forms.SaveFileDialog
        With dlg
            .FileName = getAppDir() + "export.csv"
            .CheckPathExists = True
            .InitialDirectory = getAppDir()
            .RestoreDirectory = True
            .OverwritePrompt = True
            .Filter = "CSV Text files (*.csv)|*.csv|Text files (*.txt)|*.txt|All files (*.*)|*.*"
            .FilterIndex = 1
            .AddExtension = True
            .DefaultExt = "csv"
        End With
        If dlg.ShowDialog = DialogResult.OK Then
            dateiname = dlg.FileName
        Else
            Return
        End If
        Dim ColsCount As Int32 = myDataGridView.ColumnCount
        If RowsCount > -1 Then
            'The array for field names.
            'Dim FldNames() As String = {"ID", "Company", "City", "Region", "Country"}
            Dim FldNames(myDataGridView.ColumnCount) As String

            'The array for the selected records.
            Dim DataArr(RowsCount + 1, ColsCount) As Object
            'the header line
            For i As Int32 = 0 To ColsCount - 1
                FldNames(i) = myDataGridView.Columns(i).Name
                DataArr(0, i) = FldNames(i)
            Next i
            Dim ColsCounter As Int32 = 0

            'Populate the data array - The list is sorted in ascending order.
            For RowsCounter As Int32 = 0 To RowsCount - 1
                If myDataGridView.SelectedRows.Count > 0 Then
                    For Each cell As DataGridViewCell In myDataGridView _
                                                         .SelectedRows(RowsCounter) _
                                                         .Cells
                        DataArr(RowsCounter + 1, ColsCounter) = cell.Value
                        ColsCounter = ColsCounter + 1
                    Next
                Else
                    For Each cell As DataGridViewCell In myDataGridView _
                                                         .Rows(RowsCounter) _
                                                         .Cells
                        DataArr(RowsCounter + 1, ColsCounter) = cell.Value
                        ColsCounter = ColsCounter + 1
                    Next
                End If
                ColsCounter = 0
            Next
            Dim txt As System.IO.StreamWriter
            txt = New System.IO.StreamWriter(dateiname, False, System.Text.Encoding.GetEncoding(1252))
            For r As Int32 = 0 To RowsCount
                For c As Int32 = 0 To ColsCount - 1
                    txt.Write(DataArr(r, c).ToString() + vbTab)
                Next
                txt.WriteLine()
            Next
            txt.Flush()
            txt.Close()

        End If
    End Sub
    Private Sub DataGridViewExport2Excel(ByRef myDataGridView As DataGridView)
        Dim RowsCount As Int32 = 0
        If myDataGridView.SelectedRows.Count > 0 Then
            RowsCount = myDataGridView.SelectedRows.Count
        Else
            RowsCount = myDataGridView.Rows.Count
        End If
        Dim ColsCount As Int32 = myDataGridView.ColumnCount
        If RowsCount > -1 Then
            'The array for field names.
            'Dim FldNames() As String = {"ID", "Company", "City", "Region", "Country"}
            Dim FldNames(myDataGridView.ColumnCount - 1) As String

            For i As Int32 = 0 To ColsCount - 1
                FldNames(i) = myDataGridView.Columns(i).Name
            Next i
            'The array for the selected records.
            Dim DataArr(RowsCount, ColsCount) As Object
            Dim ColsCounter As Int32 = 0

            'Populate the data array - The list is sorted in ascending order.
            Dim RowsCounter As Int32 = 0
            For RowsCounter = 0 To RowsCount - 1
                If myDataGridView.SelectedRows.Count > 0 Then
                    For Each cell As DataGridViewCell In myDataGridView _
                                                         .SelectedRows(RowsCount - 1 - RowsCounter) _
                                                         .Cells
                        DataArr(RowsCounter, ColsCounter) = cell.Value
                        ColsCounter = ColsCounter + 1
                    Next
                Else
                    For Each cell As DataGridViewCell In myDataGridView _
                                                         .Rows(RowsCount - 1 - RowsCounter) _
                                                         .Cells
                        DataArr(RowsCounter, ColsCounter) = cell.Value
                        ColsCounter = ColsCounter + 1
                    Next
                End If
                ColsCounter = 0
            Next
            For RowsCounter = 0 To RowsCount - 1
                System.Diagnostics.Debug.WriteLine("")
                System.Diagnostics.Debug.Write("Row " & RowsCounter & " = ")
                For ColsCounter = 0 To ColsCount - 1
                    System.Diagnostics.Debug.Write(DataArr(RowsCounter, ColsCount).ToString() + " ")
                Next
            Next

            Try
                'Variables for Excel.
                Dim xlApp As New Microsoft.Office.Interop.Excel.Application
                Dim xlWBook As Microsoft.Office.Interop.Excel.Workbook = xlApp.Workbooks.Add( _
                                                Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet)
                Dim xlWSheet As Microsoft.Office.Interop.Excel.Worksheet = CType(xlWBook.Worksheets(1), Microsoft.Office.Interop.Excel.Worksheet)
                Dim xlCalc As Microsoft.Office.Interop.Excel.XlCalculation

                'Save the present setting for Excel's calculation mode and turn it off. 
                With xlApp
                    xlCalc = .Calculation
                    .Calculation = Microsoft.Office.Interop.Excel.XlCalculation.xlCalculationManual
                End With

                'Write the field names and the data to the targeting worksheet.
                With xlWSheet
                    .Range(.Cells(1, 1), .Cells(1, ColsCount)).Value = FldNames
                    .Range(.Cells(2, 1), .Cells(RowsCount + 2, ColsCount)).Value = DataArr
                    .UsedRange.Columns.AutoFit()
                End With

                With xlApp
                    .Visible = True
                    .UserControl = True
                    'Restore the calculation mode. 
                    .Calculation = xlCalc
                End With

                'Relase objects from memory.                         
                xlWSheet = Nothing
                xlWBook = Nothing
                xlApp = Nothing
                GC.Collect()
            Catch sx As SystemException
                MessageBox.Show(sx.Message, "Excel installiert?")
            End Try
        End If
    End Sub
    Sub DataGridViewExport(ByRef myDataGridView As System.Windows.Forms.DataGridView)
        If frmExport.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If frmExport.soption = "CSV" Then
                DataGridExport2CSV(myDataGridView)
            ElseIf frmExport.soption = "XLS" Then
                DataGridViewExport2Excel(myDataGridView)
            End If
        End If
    End Sub
    Public Function readRegDBFileName() As String
        ' The name of the key must include a valid root.
        Const userRoot As String = "HKEY_CURRENT_USER"
        Const subkey As String = "Software\KBFakt"
        Const keyName As String = userRoot & "\" & subkey
        Dim useDBFilename As String = ""

        Try
            useDBFilename = CStr(Registry.GetValue(keyName, "dbFilename", ""))
        Catch x As Exception
            If mainmodul.DebugModus Then
                MessageBox.Show("Exception in readRegDBFileName()" + vbCrLf + x.Message)
            End If
        End Try

        Return useDBFilename

    End Function
    Public Function regWriteDBFilename(ByVal s As String) As Boolean
        ' The name of the key must include a valid root.
        Const userRoot As String = "HKEY_CURRENT_USER"
        Const subkey As String = "Software\KBFakt"
        Const keyName As String = userRoot & "\" & subkey
        Dim useDBFilename As String = ""
        Dim bRet As Boolean = False
        Try
            Registry.SetValue(keyName, "dbFilename", s)
            bRet = True
        Catch ex As Exception
            If mainmodul.DebugModus Then
                MessageBox.Show("Exception in regWriteDBFilename()" + vbCrLf + ex.Message)
            End If
        End Try
        Return bRet
    End Function

    Public Function readRegUseRawPrinter() As Integer
        ' The name of the key must include a valid root.
        Const userRoot As String = "HKEY_CURRENT_USER"
        Const subkey As String = "Software\KBFakt"
        Const keyName As String = userRoot & "\" & subkey
        Dim useRawPrinter As Integer = 1

        Try
            useRawPrinter = CInt(Registry.GetValue(keyName, "useRawPrinter", 1))
        Catch x As Exception
            If mainmodul.DebugModus Then
                MessageBox.Show("Exception in readRegUseRawPrinter()" + vbCrLf + x.Message)
            End If
        End Try

        Return useRawPrinter

    End Function
    'Private Sub WriteReg()
    '    Const userRoot As String = "HKEY_CURRENT_USER"
    '    Const subkey As String = "Software\KBFakt"
    '    Const keyName As String = userRoot & "\" & subkey
    '    Try
    '        Registry.SetValue(keyName, "MarginBottom", m_Bottom)
    '        Registry.SetValue(keyName, "MarginLeft", m_Left)
    '        Registry.SetValue(keyName, "MarginRight", m_Right)
    '        Registry.SetValue(keyName, "MarginTop", m_Top)
    '        Registry.SetValue(keyName, "TopOffsetLines", m_TopOffsetLines)
    '        Registry.SetValue(keyName, "RawPageLength", m_RawPageLength)
    '        Registry.SetValue(keyName, "PaperWidth", m_PaperWidth)
    '        Registry.SetValue(keyName, "PaperHeight", m_PaperHeight)
    '        Registry.SetValue(keyName, "PrinterName", m_PrinterName)
    '        Registry.SetValue(keyName, "iPaperKind", m_iPaperKind)

    '    Catch x As Exception
    '        If mainmodul.DebugModus Then
    '            MessageBox.Show("Exception in WriteReg()" + vbCrLf + x.Message)
    '        End If
    '    End Try

    'End Sub
End Module
