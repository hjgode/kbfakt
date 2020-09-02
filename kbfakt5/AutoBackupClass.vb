Public Class AutoBackupClass
    Private m_pfad As String
    Private m_BackupNameTemplate As String = "kbfakt5backup"
    'Private Sub CreateAutoBackup()
    '    Dim dts As String
    '    Dim ret As Boolean = False
    '    Dim pfad As String = getAppDir()
    '    If Not pfad.EndsWith("\") Then pfad = pfad + "\"
    '    dts = String.Format("kbfakt5_{0:dd.MM.yyyy_HHmmss}.mdb", DateTime.Now)

    'End Sub
    Public Property BackupPath() As String
        Get
            Return m_pfad
        End Get
        Set(ByVal value As String)
            If System.IO.Directory.Exists(value) Then
                If Not m_pfad.EndsWith("\") Then m_pfad = m_pfad + "\"
                m_pfad = value
            Else
                Throw New ArgumentOutOfRangeException("Path does not exist: " + value)
            End If
        End Set
    End Property
    Public Sub New()
        'm_pfad = getAppDir()
        m_pfad = System.IO.Path.GetDirectoryName(GetDbFileName())
        If Not m_pfad.EndsWith("\") Then m_pfad = m_pfad + "\"

    End Sub
    Public Function MakeBackup() As Boolean
        ' a progressWindow
        Dim my_progressWindow As New MWA.Progress.ProgressWindow()
        my_progressWindow.Text = "Erstelle Autobackup"
        System.Threading.ThreadPool.QueueUserWorkItem(New System.Threading.WaitCallback(AddressOf DoSomeWork), my_progressWindow)
        my_progressWindow.ShowDialog()
    End Function
    Private Sub DoSomeWork(ByVal status As Object)
        Dim callback As MWA.Progress.IProgressCallback = status
        Try
            'look in backup path for files starting with BackupNameXX
            'where XX is from 01 to 07 for days of week
            'and 00 is the latest backup
            Dim DayOfWeekString As String = Weekday(Date.Today, FirstDayOfWeek.Monday).ToString("00")
            'check if we already did a backup for today
            Dim fname As String = m_pfad + m_BackupNameTemplate + DayOfWeekString + ".mdb"
            Dim fname00 As String = m_pfad + m_BackupNameTemplate + "00.mdb"

            'Begion general backup
            'create a new general backup
            Try
                callback.Begin(0, 5)
                callback.StepTo(iStep())
                If (callback.IsAborting()) Then
                    Return
                End If
                callback.SetText("Backup nach " + fname00)
                FileCopy(DBfileName, fname00)
                callback.StepTo(iStep())
                If (callback.IsAborting()) Then
                    Return
                End If
                callback.SetText("Entferne Passwort aus " + fname00)
                If (Not DBdeletePassword(fname00)) Then
                    MessageBox.Show("DBDeletePassword failed", "Autobackup")
                    callback.SetText("DBDeletePassword failed")
                End If
            Catch exc As Exception
                callback.SetText("AutoBackup Exception: " + exc.Message)
            End Try
            'end general backup

            'daily backup
            callback.StepTo(iStep())
            If (callback.IsAborting()) Then
                Return
            End If
            'test for current day backup
            callback.SetText("Prüfe vorhandenes Backup: '" + fname + "'")
            If System.IO.File.Exists(fname) Then
                If System.IO.File.GetCreationTime(fname) = Date.Today Then
                    callback.StepTo(iStep())
                    If (callback.IsAborting()) Then
                        Return
                    End If
                    callback.SetText("Vorhandenes Backup ist aktuell: '" + fname + "'")
                    'we did already make a backup today
                End If
            Else
                Try
                    callback.StepTo(iStep())
                    If (callback.IsAborting()) Then
                        Return
                    End If
                    callback.SetText("Erstelle aktuelles Backup: '" + fname + "'")
                    FileCopy(DBfileName, fname)
                    callback.StepTo(iStep())
                    If (callback.IsAborting()) Then
                        Return
                    End If
                    callback.SetText("Entferne Passwort aus: '" + fname + "'")
                    If (DBdeletePassword(fname)) Then
                        callback.SetText("Backup nach '" + fname + "' erfolgreich")
                        'MessageBox.Show("Datensicherung in die Datei " + vbCrLf + fname + vbCrLf + "war erfolgreich")
                        'Return True
                    Else
                        callback.SetText("Backup nach '" + fname + "' ist fehlgeschlagen")
                        'MessageBox.Show("Datensicherung in die Datei " + vbCrLf + fname + vbCrLf + "war nicht erfolgreich (Password)")
                    End If
                Catch ax As ArgumentException
                    callback.SetText("AutoBackup Exception: " + ax.Message)
                    'MessageBox.Show("Exception: " + ax.Message, "MakeBackup()")
                Catch fx As System.IO.FileNotFoundException
                    callback.SetText("AutoBackup Exception: " + fx.Message)
                    'MessageBox.Show("Exception: " + fx.Message, "MakeBackup()")
                Catch io As System.IO.IOException
                    callback.SetText("AutoBackup Exception: " + io.Message)
                    'MessageBox.Show("Exception: " + io.Message, "MakeBackup()")
                End Try

            End If

        Catch sax As System.Threading.ThreadAbortException
            callback.SetText("AutoBackup Exception: " + sax.Message)
        Catch six As System.Threading.ThreadInterruptedException
            callback.SetText("AutoBackup Exception: " + six.Message)
        Finally
            If (Not IsNothing(callback)) Then
                callback.End()
            End If
        End Try
    End Sub
    Private Function iStep() As Integer
        Static iVal = 0
        iVal += 1
        System.Threading.Thread.Sleep(250)
        Return iVal
    End Function
End Class
