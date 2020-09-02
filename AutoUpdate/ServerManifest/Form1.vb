Imports System.Windows
Imports System.IO
Imports System.Xml

Public Class Form1
    Inherits System.Windows.Forms.Form
    Dim xmlDoc As New XmlDocument   'Xml Document
    Dim oElmntRoot As XmlElement    'Manipulates the root folder node

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
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.lblStatus = New System.Windows.Forms.Label
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(20, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Update File Folder"
        '
        'txtPath
        '
        Me.txtPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPath.Location = New System.Drawing.Point(122, 18)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(262, 20)
        Me.txtPath.TabIndex = 1
        Me.txtPath.Text = ""
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Location = New System.Drawing.Point(390, 18)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(30, 20)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "..."
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Location = New System.Drawing.Point(230, 46)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(154, 22)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "Create Manifest"
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.White
        Me.lblStatus.Location = New System.Drawing.Point(22, 74)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(400, 18)
        Me.lblStatus.TabIndex = 4
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.LightSlateGray
        Me.ClientSize = New System.Drawing.Size(448, 102)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Form1"
        Me.Text = "Server Manifest Utility"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog = Forms.DialogResult.OK Then
            txtPath.Text = FolderBrowserDialog1.SelectedPath
            lblStatus.Text = ""
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If txtPath.Text = "" Then
            Exit Sub
        End If

        Try
            xmlDoc.RemoveAll() 'Clears the xmlDoc for multiple process, because xmlDoc is a class level object
            lblStatus.Text = "Readind directory structure..."
            Application.DoEvents()

            xmlDoc.AppendChild(xmlDoc.CreateProcessingInstruction("xml", "version='1.0' encoding='UTF-8'")) 'First Chid: Header
            createRootNode()

            With SaveFileDialog1
                .AddExtension = True
                .FileName = "ServerManifest.xml"
                .DefaultExt = ".xml"
                .Filter = "xml files (*.xml)|*.xml"
                .FilterIndex = 1
                .RestoreDirectory = True

                If .ShowDialog = Forms.DialogResult.OK Then xmlDoc.Save(.FileName)
            End With
            '            xmlDoc.Save("ServerManifest.xml")

            lblStatus.Text = "The XML document of manifest was created."

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Private Sub createRootNode()
        Try
            oElmntRoot = xmlDoc.CreateElement("update") 'Second Child: update
            xmlDoc.AppendChild(oElmntRoot) 'Add the element to the xml document

            loopNodes(oElmntRoot, txtPath.Text)

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub loopNodes(ByVal oElmntParent As XmlElement, ByVal strPath As String)
        Dim ofs As New DirectoryInfo(strPath & "\")

        'Files Loop ------------------------------------------------------
        Dim oFile As FileInfo
        For Each oFile In ofs.GetFiles
            lblStatus.Text = "Reading file " & oFile.Name
            Application.DoEvents()

            If oFile.Name <> "ServerManifest.xml" And _
                Not oFile.Name.EndsWith(".pdb") And _
                Not oFile.Name.EndsWith(".config") Then
                Dim oElmntLeaf1 As XmlElement    'Manipulates the files nodes
                Dim oElmntFileName As XmlElement
                Dim oElmntFileVersion As XmlElement
                Dim oElmntFileModified As XmlElement

                oElmntLeaf1 = xmlDoc.CreateElement("name")
                oElmntLeaf1.SetAttribute("file", oFile.Name)
                oElmntParent.AppendChild(oElmntLeaf1)

                oElmntFileName = xmlDoc.CreateElement("filename")
                oElmntFileName.InnerText = oFile.Name
                oElmntLeaf1.AppendChild(oElmntFileName)

                Dim fv As FileVersionInfo = FileVersionInfo.GetVersionInfo(oFile.FullName)
                oElmntFileVersion = xmlDoc.CreateElement("fileversion")
                oElmntFileVersion.InnerText = fv.FileVersion
                oElmntLeaf1.AppendChild(oElmntFileVersion)

                oElmntFileModified = xmlDoc.CreateElement("filelastmodified")
                oElmntFileModified.InnerText = oFile.LastWriteTimeUtc
                oElmntLeaf1.AppendChild(oElmntFileModified)
            End If
        Next

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblStatus.Text = ""
    End Sub
End Class
