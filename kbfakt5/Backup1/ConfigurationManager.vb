Imports System
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Xml
Namespace IsolatedStorage
    '''
    ''' 
    ''' <summary>
    ''' IsolatedStorageConfigurationManager
    ''' ===================================
    ''' Read and Write application and formsettings to isolated storage
    ''' Setting are saved as XML in a file named "application-name".config 
    ''' in folder C:\Documents and Settings\user\Local Settings\Application Data\IsolatedStorage\AssemFiles\
    '''
    ''' Class is implemented as a Singleton.
    ''' Example for use: 
    ''' IsolatedStorageConfigurationManager configManager = IsolatedStorageConfigurationManager.ConfigurationManager(Application.ProductName)
    ''' string databaseName = configManager.Read("Database")
    ''' configManager.Write("Database", DatabaseName)
    ''' configManager.Persist()
    '''
    ''' Edwin Roetman, January 2004
    ''' </summary>
    Public NotInheritable Class ConfigurationManager

        'The ConfigurationManager singleton instance
        Private Shared _singleton As ConfigurationManager

        Private _xml As XmlDocument

        Private _xmlOriginal As XmlDocument

        Private _fileName As String

        Private _isoStore As IsolatedStorageFile

        ''' <summary>
        ''' Singleton, do not allow this class to be instantiated by making the contructor private
        ''' </summary>
        ''' <param name="applicationName"></param>
        Private Sub New(ByVal applicationName As String)
            MyBase.New()
            Me.InitializeConfiguration(applicationName)
        End Sub

        'The ConfigurationManager singleton instance
        Public Shared Function GetConfigurationManager(ByVal applicationName As String) As ConfigurationManager
            If IsNothing(_singleton) Then
                'If (_singleton = Nothing) Then
                _singleton = New ConfigurationManager(applicationName)
            End If
            Return _singleton
        End Function

        Public Overloads Function Read(ByVal section As String) As String
            Return Read(section, String.Empty)
        End Function

        Public Overloads Function Read(ByVal section As String, ByVal defaultValue As String) As String
            Try
                If IsNothing(Me._xml) Then
                    Return String.Empty
                End If
                'Select the item node(s)
                Dim sectionNode As XmlNode = Me._xml.SelectSingleNode(("/configuration/" + section))
                If IsNothing(sectionNode) Then
                    Return defaultValue
                End If
                Return sectionNode.FirstChild.Value
            Catch sx As System.Exception
                Return defaultValue
            End Try
        End Function

        Public Overloads Function ReadInteger(ByVal section As String) As Integer
            Return ReadInteger(section, 0)
        End Function

        Public Overloads Function ReadInteger(ByVal section As String, ByVal defaultValue As Integer) As Integer
            Dim valuestring As String = Read(section)
            If (valuestring.Length <= 0) Then
                Return defaultValue
            End If
            Try
                Dim value As Integer = Convert.ToInt32(valuestring)
                Return value
            Catch sx As System.Exception
                Return defaultValue
            End Try
        End Function

        Public Overloads Function ReadBoolean(ByVal section As String) As Boolean
            Return ReadBoolean(section, False)
        End Function

        Public Overloads Function ReadBoolean(ByVal section As String, ByVal defaultValue As Boolean) As Boolean
            Dim value As String = Me.Read(section)
            If (value.Length <= 0) Then
                Return defaultValue
            End If
            Try
                Return Boolean.Parse(value)
            Catch sx As System.Exception
                Return defaultValue
            End Try
        End Function

        Public Sub Write(ByVal section As String, ByVal value As String)
            Try
                If IsNothing(Me._xml) Then
                    Me._xml = New XmlDocument
                    Dim configurationRootNode As XmlNode = Me._xml.CreateElement("configuration")
                    Me._xml.AppendChild(configurationRootNode)
                End If
                'Select the item node(s)
                Dim sectionNode As XmlNode = Me._xml.SelectSingleNode(("/configuration/" + section))
                If IsNothing(sectionNode) Then
                    'if the node does not exist create it
                    sectionNode = Me._xml.CreateElement(section)
                    Dim configurationRootNode As XmlNode = Me._xml.SelectSingleNode("/configuration")
                    configurationRootNode.AppendChild(sectionNode)
                End If
                sectionNode.InnerText = value
            Catch sx As System.Exception

            End Try
        End Sub

        'Read form state, size and position
        Public Sub ReadFormSettings(ByVal form As System.Windows.Forms.Form)
            Dim windowStateString As String = Me.Read((form.Name + "WindowState"))
            Dim windowState As System.Windows.Forms.FormWindowState = System.Windows.Forms.FormWindowState.Normal
            If (windowStateString.Length > 0) Then
                windowState = CType(Convert.ToInt32(windowStateString), System.Windows.Forms.FormWindowState)
            End If
            If (windowState = System.Windows.Forms.FormWindowState.Maximized) Then
                form.WindowState = windowState
            Else
                Dim valuesString As String = Me.Read(form.Name)
                If (valuesString.Length > 0) Then
                    Dim values() As String = valuesString.Split(Convert.ToChar(","))
                    form.Top = Convert.ToInt16(values(0))
                    form.Left = Convert.ToInt16(values(1))
                    Dim width As Integer = Convert.ToInt16(values(2))
                    If (width > 0) Then
                        form.Width = width
                    End If
                    Dim height As Integer = Convert.ToInt16(values(3))
                    If (height > 0) Then
                        form.Height = height
                    End If
                End If
            End If
        End Sub

        'Write form state, size and position
        Public Sub WriteFormSettings(ByVal form As System.Windows.Forms.Form)
            Me.Write((form.Name + "WindowState"), CType(form.WindowState, Integer).ToString)
            'Me.Write(form.Name & "WindowState", (CType(form.WindowState, Integer)).ToString())
            If (form.WindowState = System.Windows.Forms.FormWindowState.Normal) Then
                Dim valuesstring As String = (form.Top.ToString + ("," _
                            + (form.Left.ToString + ("," _
                            + (form.Width.ToString + ("," + form.Height.ToString))))))
                Me.Write(form.Name, valuesstring)
            End If
        End Sub

        Public Sub Persist()
            Try
                Me.WriteBackConfiguration()
            Catch sx As System.Exception

            Finally
                _singleton = Nothing
            End Try
        End Sub

        Private Sub InitializeConfiguration(ByVal applicationName As String)
            Me._fileName = (applicationName + ".config")
            Me._isoStore = IsolatedStorageFile.GetStore((IsolatedStorageScope.User Or IsolatedStorageScope.Assembly), Nothing, Nothing)
            'Check to see if the settings file exists, if so load xml from it
            Dim storeFileNames() As String
            storeFileNames = Me._isoStore.GetFileNames(Me._fileName)
            For Each storeFile As String In storeFileNames
                If (storeFile = Me._fileName) Then
                    'Create isoStorage StreamReader
                    Dim streamReader As StreamReader = New StreamReader(New IsolatedStorageFileStream(Me._fileName, FileMode.Open, Me._isoStore))
                    Me._xml = New XmlDocument
                    Me._xml.Load(streamReader)
                    Me._xmlOriginal = New XmlDocument
                    Me._xmlOriginal.LoadXml(Me._xml.OuterXml)
                    streamReader.Close()
                End If
            Next
        End Sub

        Private Sub WriteBackConfiguration()
            'if no config information is present write null
            If IsNothing(Me._xml) Then
                Return
            End If
            'if config is unchanged write null
            If (Not (Me._xmlOriginal) Is Nothing) Then
                If (Me._xml.OuterXml = Me._xmlOriginal.OuterXml) Then
                    Return
                End If
            End If
            'Save the document
            Dim streamWriter As StreamWriter = Nothing
            Try
                streamWriter = New StreamWriter(New IsolatedStorageFileStream(Me._fileName, FileMode.Create, Me._isoStore))
                Me._xml.Save(streamWriter)
                streamWriter.Flush()
                streamWriter.Close()
                If IsNothing(Me._xmlOriginal) Then
                    Me._xmlOriginal = New XmlDocument
                End If
                Me._xmlOriginal.LoadXml(Me._xml.OuterXml)
            Catch sx As System.Exception
                'throw;
            Finally
                If (Not (streamWriter) Is Nothing) Then
                    streamWriter.Flush()
                    streamWriter.Close()
                End If
            End Try
        End Sub
    End Class
End Namespace
