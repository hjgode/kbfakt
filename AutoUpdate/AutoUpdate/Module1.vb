Imports System.Net
Imports System.xml
Imports System.IO

Module Module1
    Friend myForm As Form1    ' the form interface of update process
    Friend ExeFile As String     ' the main program that called the auto update
    Dim RemoteUri As String    ' the url location of the files
    Dim Key As String             ' the key used by the program when called back 
    ' to know that the program was launched by the 
    ' Auto Update program
    Dim ManifestFile As String  ' the manifest file name
    Dim UserID As String         ' the User ID
    Dim CommandLine As String    ' the command line passed to the original 

    Sub Main()
        Try
            ' Get the parameters sent by the application should be separated by “|”
            Dim param() As String = Split(Microsoft.VisualBasic.Command(), "|")
            ExeFile = param(0)
            RemoteUri = param(1)
            ManifestFile = param(2)
            UserID = param(3)
            CommandLine = Microsoft.VisualBasic.Command()
            ' if Parameter omitted then application exit
            If ExeFile = "" Or RemoteUri = "" Or ManifestFile = "" Then Environment.Exit(1)

            myForm = New Form1
            myForm.Label1.Text = "Checking for application update, please wait!..."
            Application.DoEvents()
            Application.Run(myForm)
        Catch ex As Exception
            MsgBox(ex.ToString)
            Application.Exit()
        End Try
    End Sub

    Sub ProcessUpdate()
        Dim myWebClient As New WebClient
        ' Download manifest file
        Try
            ' get the update file content in manifest file
            myForm.Label2.Text = "download manifest..."
            Application.DoEvents()
            myWebClient.DownloadFile(RemoteUri & ManifestFile, ManifestFile)
            myForm.Label2.Text = "download manifest done"
            Application.DoEvents()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try

        Try
            Dim m_xmld As XmlDocument
            Dim m_nodelist As XmlNodeList
            Dim m_node As XmlNode
            'Create the XML Document
            m_xmld = New XmlDocument
            'Load the Xml file
            m_xmld.Load(Application.StartupPath & "\" & ManifestFile)
            'Get the list of name nodes 
            m_nodelist = m_xmld.SelectNodes("/update/name")

            'Init progressbar
            InitProgress(m_nodelist.Count)

            'Loop through the nodes
            For Each m_node In m_nodelist
                'Get the file Attribute Value
                Dim fileAttribute = m_node.Attributes.GetNamedItem("file").Value
                'Get the fileName Element Value
                Dim fileNameValue = m_node.ChildNodes.Item(0).InnerText
                'Get the fileVersion Element Value
                Dim fileVersionValue = m_node.ChildNodes.Item(1).InnerText
                'Get the fileLastModified Value
                Dim fileLastModiValue = m_node.ChildNodes.Item(2).InnerText

                'Temp file name
                Dim TempFileName As String = Application.StartupPath & "\" & Now.TimeOfDay.TotalMilliseconds
                Dim isToUpgrade As Boolean = False
                Dim RealFileName As String = Application.StartupPath & "\" & fileNameValue
                Dim LastModified As Date = CDate(fileLastModiValue)

                Dim FileExists As Boolean = File.Exists(RealFileName)
                'If file not exist then download file
                If Not FileExists Then
                    isToUpgrade = True
                ElseIf fileVersionValue <> "" Then
                    'verify the file version
                    Dim fv As FileVersionInfo = FileVersionInfo.GetVersionInfo(RealFileName)
                    isToUpgrade = (GetVersion(fileVersionValue) > GetVersion(fv.FileMajorPart & "." & fv.FileMinorPart & "." & fv.FileBuildPart & "." & fv.FilePrivatePart))
                    'check if version not upgrade then check last modified
                    If Not isToUpgrade Then
                        isToUpgrade = (LastModified > File.GetLastWriteTimeUtc(RealFileName))
                    End If
                Else
                    'check last modified file
                    isToUpgrade = (LastModified > File.GetLastWriteTimeUtc(RealFileName))
                End If

                'Download upgrade file
                If isToUpgrade Then
                    myForm.Label2.Text = "Update file " & fileNameValue & "..."
                    Application.DoEvents()
                    ' Download file and name it with temporary name
                    myWebClient.DownloadFile(RemoteUri & fileNameValue, TempFileName)
                    ' Rename temporary file to real file name
                    File.Copy(TempFileName, RealFileName, True)
                    ' Set Last modified 
                    File.SetLastWriteTimeUtc(RealFileName, LastModified)
                    ' Delete temporary file
                    File.Delete(TempFileName)
                End If

                IncrementProgress()
            Next
            'Delete server manifest file
            File.Delete(Application.StartupPath & "\" & ManifestFile)

            myForm.Label1.Text = "Application update complete!"
            Application.DoEvents()

            Dim startInfo As New ProcessStartInfo(Application.StartupPath & "\" & ExeFile)
            startInfo.Arguments = CommandLine
            Process.Start(startInfo)

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Private Function GetVersion(ByVal Version As String) As String
        Dim x() As String = Split(Version, ".")
        Return String.Format("{0:00000}{1:00000}{2:00000}{3:00000}", Int(x(0)), Int(x(1)), Int(x(2)), Int(x(3)))
    End Function

    Private Sub InitProgress(ByVal lMax As Long)
        myForm.ProgressBar1.Value = 0
        myForm.ProgressBar1.Maximum = lMax
        Application.DoEvents()
    End Sub

    Private Sub IncrementProgress()
        With myForm.ProgressBar1
            If .Value < .Maximum Then .Value = .Value + 1
            Application.DoEvents()
        End With
    End Sub

    Sub KillAppExe()
        ' Get MainApp exe name without extension
        Dim AppExe As String = ExeFile.Replace(".exe", "")

        Dim local As Process() = Process.GetProcesses
        Dim i As Integer
        ' Search MainApp process in windows process
        For i = 0 To local.Length - 1
            ' If MainApp process found then close or kill MainApp
            If Strings.UCase(local(i).ProcessName) = Strings.UCase(AppExe) Then
                local(i).CloseMainWindow()
            End If
        Next
    End Sub
End Module

