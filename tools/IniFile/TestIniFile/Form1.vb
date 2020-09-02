Imports System.IO
Imports IniFile

Public Class Form1
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuNew As System.Windows.Forms.MenuItem
    Friend WithEvents mnuOpen As System.Windows.Forms.MenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSaveToXML As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents colKey As System.Windows.Forms.ColumnHeader
    Friend WithEvents colValue As System.Windows.Forms.ColumnHeader
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents sbSections As System.Windows.Forms.StatusBar
    Friend WithEvents lblSections As System.Windows.Forms.Label
    Friend WithEvents sbKeys As System.Windows.Forms.StatusBar
    Friend WithEvents lblKeys As System.Windows.Forms.Label
    Friend WithEvents mnuSection As System.Windows.Forms.MenuItem
    Friend WithEvents cmSections As System.Windows.Forms.ContextMenu
    Friend WithEvents cmKeys As System.Windows.Forms.ContextMenu
    Friend WithEvents mnuCommentSection As System.Windows.Forms.MenuItem
    Friend WithEvents mnuUnCommentSection As System.Windows.Forms.MenuItem
    Friend WithEvents mnuKeys As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAddKey As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCommentKey As System.Windows.Forms.MenuItem
    Friend WithEvents mnuUnCommentKey As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAddSection As System.Windows.Forms.MenuItem
    Friend WithEvents cmnuCommentSection As System.Windows.Forms.MenuItem
    Friend WithEvents cmnuUnCommentSection As System.Windows.Forms.MenuItem
    Friend WithEvents cmnuAddSection As System.Windows.Forms.MenuItem
    Friend WithEvents cmnuAddKey As System.Windows.Forms.MenuItem
    Friend WithEvents cmnuCommentKey As System.Windows.Forms.MenuItem
    Friend WithEvents cmnuUnCommentKey As System.Windows.Forms.MenuItem
    Friend WithEvents mnuDeleteSection As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditSection As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMoveKey As System.Windows.Forms.MenuItem
    Friend WithEvents mnuDeleteKey As System.Windows.Forms.MenuItem
    Friend WithEvents cmnuDeleteSection As System.Windows.Forms.MenuItem
    Friend WithEvents cmnuEditSection As System.Windows.Forms.MenuItem
    Friend WithEvents cmnuMoveKey As System.Windows.Forms.MenuItem
    Friend WithEvents cmnuDeleteKey As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSave As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSort As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuRenameKey As System.Windows.Forms.MenuItem
    Friend WithEvents cmnuRenameKey As System.Windows.Forms.MenuItem
    Friend WithEvents mnuKeyValue As System.Windows.Forms.MenuItem
    Friend WithEvents cmnuKeyValue As System.Windows.Forms.MenuItem
    Friend WithEvents mnuShowText As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Form1))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuNew = New System.Windows.Forms.MenuItem
        Me.mnuOpen = New System.Windows.Forms.MenuItem
        Me.mnuSave = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.mnuSort = New System.Windows.Forms.MenuItem
        Me.mnuShowText = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.mnuSaveToXML = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
        Me.mnuExit = New System.Windows.Forms.MenuItem
        Me.mnuSection = New System.Windows.Forms.MenuItem
        Me.mnuAddSection = New System.Windows.Forms.MenuItem
        Me.mnuEditSection = New System.Windows.Forms.MenuItem
        Me.mnuDeleteSection = New System.Windows.Forms.MenuItem
        Me.mnuCommentSection = New System.Windows.Forms.MenuItem
        Me.mnuUnCommentSection = New System.Windows.Forms.MenuItem
        Me.mnuKeys = New System.Windows.Forms.MenuItem
        Me.mnuAddKey = New System.Windows.Forms.MenuItem
        Me.mnuRenameKey = New System.Windows.Forms.MenuItem
        Me.mnuKeyValue = New System.Windows.Forms.MenuItem
        Me.mnuDeleteKey = New System.Windows.Forms.MenuItem
        Me.mnuMoveKey = New System.Windows.Forms.MenuItem
        Me.mnuCommentKey = New System.Windows.Forms.MenuItem
        Me.mnuUnCommentKey = New System.Windows.Forms.MenuItem
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.cmSections = New System.Windows.Forms.ContextMenu
        Me.cmnuAddSection = New System.Windows.Forms.MenuItem
        Me.cmnuEditSection = New System.Windows.Forms.MenuItem
        Me.cmnuDeleteSection = New System.Windows.Forms.MenuItem
        Me.cmnuCommentSection = New System.Windows.Forms.MenuItem
        Me.cmnuUnCommentSection = New System.Windows.Forms.MenuItem
        Me.sbSections = New System.Windows.Forms.StatusBar
        Me.lblSections = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.colKey = New System.Windows.Forms.ColumnHeader
        Me.colValue = New System.Windows.Forms.ColumnHeader
        Me.cmKeys = New System.Windows.Forms.ContextMenu
        Me.cmnuAddKey = New System.Windows.Forms.MenuItem
        Me.cmnuRenameKey = New System.Windows.Forms.MenuItem
        Me.cmnuKeyValue = New System.Windows.Forms.MenuItem
        Me.cmnuDeleteKey = New System.Windows.Forms.MenuItem
        Me.cmnuMoveKey = New System.Windows.Forms.MenuItem
        Me.cmnuCommentKey = New System.Windows.Forms.MenuItem
        Me.cmnuUnCommentKey = New System.Windows.Forms.MenuItem
        Me.sbKeys = New System.Windows.Forms.StatusBar
        Me.lblKeys = New System.Windows.Forms.Label
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuSection, Me.mnuKeys})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuNew, Me.mnuOpen, Me.mnuSave, Me.MenuItem3, Me.mnuSort, Me.mnuShowText, Me.MenuItem5, Me.mnuSaveToXML, Me.MenuItem7, Me.mnuExit})
        Me.mnuFile.Text = "&File"
        '
        'mnuNew
        '
        Me.mnuNew.Index = 0
        Me.mnuNew.Text = "&New..."
        '
        'mnuOpen
        '
        Me.mnuOpen.Index = 1
        Me.mnuOpen.Text = "&Open..."
        '
        'mnuSave
        '
        Me.mnuSave.Index = 2
        Me.mnuSave.Text = "&Save..."
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 3
        Me.MenuItem3.Text = "-"
        '
        'mnuSort
        '
        Me.mnuSort.Index = 4
        Me.mnuSort.Text = "Sort"
        '
        'mnuShowText
        '
        Me.mnuShowText.Index = 5
        Me.mnuShowText.Text = "Show Text"
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 6
        Me.MenuItem5.Text = "-"
        '
        'mnuSaveToXML
        '
        Me.mnuSaveToXML.Index = 7
        Me.mnuSaveToXML.Text = "Save to &XML..."
        '
        'MenuItem7
        '
        Me.MenuItem7.Index = 8
        Me.MenuItem7.Text = "-"
        '
        'mnuExit
        '
        Me.mnuExit.Index = 9
        Me.mnuExit.Text = "&Exit"
        '
        'mnuSection
        '
        Me.mnuSection.Index = 1
        Me.mnuSection.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAddSection, Me.mnuEditSection, Me.mnuDeleteSection, Me.mnuCommentSection, Me.mnuUnCommentSection})
        Me.mnuSection.Text = "&Section"
        '
        'mnuAddSection
        '
        Me.mnuAddSection.Index = 0
        Me.mnuAddSection.Text = "&Add"
        '
        'mnuEditSection
        '
        Me.mnuEditSection.Index = 1
        Me.mnuEditSection.Text = "&Edit"
        '
        'mnuDeleteSection
        '
        Me.mnuDeleteSection.Index = 2
        Me.mnuDeleteSection.Text = "&Delete"
        '
        'mnuCommentSection
        '
        Me.mnuCommentSection.Index = 3
        Me.mnuCommentSection.Text = "&Comment"
        '
        'mnuUnCommentSection
        '
        Me.mnuUnCommentSection.Index = 4
        Me.mnuUnCommentSection.Text = "&UnComment"
        '
        'mnuKeys
        '
        Me.mnuKeys.Index = 2
        Me.mnuKeys.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAddKey, Me.mnuRenameKey, Me.mnuKeyValue, Me.mnuDeleteKey, Me.mnuMoveKey, Me.mnuCommentKey, Me.mnuUnCommentKey})
        Me.mnuKeys.Text = "&Keys"
        '
        'mnuAddKey
        '
        Me.mnuAddKey.Index = 0
        Me.mnuAddKey.Text = "&Add"
        '
        'mnuRenameKey
        '
        Me.mnuRenameKey.Index = 1
        Me.mnuRenameKey.Text = "&Rename"
        '
        'mnuKeyValue
        '
        Me.mnuKeyValue.Index = 2
        Me.mnuKeyValue.Text = "&Value"
        '
        'mnuDeleteKey
        '
        Me.mnuDeleteKey.Index = 3
        Me.mnuDeleteKey.Text = "&Delete"
        '
        'mnuMoveKey
        '
        Me.mnuMoveKey.Index = 4
        Me.mnuMoveKey.Text = "&Move"
        '
        'mnuCommentKey
        '
        Me.mnuCommentKey.Index = 5
        Me.mnuCommentKey.Text = "&Comment"
        '
        'mnuUnCommentKey
        '
        Me.mnuUnCommentKey.Index = 6
        Me.mnuUnCommentKey.Text = "&UnComment"
        '
        'ImageList1
        '
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.TreeView1)
        Me.Panel1.Controls.Add(Me.sbSections)
        Me.Panel1.Controls.Add(Me.lblSections)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(200, 305)
        Me.Panel1.TabIndex = 4
        '
        'TreeView1
        '
        Me.TreeView1.AllowDrop = True
        Me.TreeView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TreeView1.ContextMenu = Me.cmSections
        Me.TreeView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeView1.ImageList = Me.ImageList1
        Me.TreeView1.Location = New System.Drawing.Point(0, 16)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(200, 267)
        Me.TreeView1.TabIndex = 4
        '
        'cmSections
        '
        Me.cmSections.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.cmnuAddSection, Me.cmnuEditSection, Me.cmnuDeleteSection, Me.cmnuCommentSection, Me.cmnuUnCommentSection})
        '
        'cmnuAddSection
        '
        Me.cmnuAddSection.Index = 0
        Me.cmnuAddSection.Text = "Add"
        '
        'cmnuEditSection
        '
        Me.cmnuEditSection.Index = 1
        Me.cmnuEditSection.Text = "&Edit"
        '
        'cmnuDeleteSection
        '
        Me.cmnuDeleteSection.Index = 2
        Me.cmnuDeleteSection.Text = "&Delete"
        '
        'cmnuCommentSection
        '
        Me.cmnuCommentSection.Index = 3
        Me.cmnuCommentSection.Text = "Comment"
        '
        'cmnuUnCommentSection
        '
        Me.cmnuUnCommentSection.Index = 4
        Me.cmnuUnCommentSection.Text = "UnComment"
        '
        'sbSections
        '
        Me.sbSections.Location = New System.Drawing.Point(0, 283)
        Me.sbSections.Name = "sbSections"
        Me.sbSections.Size = New System.Drawing.Size(200, 22)
        Me.sbSections.SizingGrip = False
        Me.sbSections.TabIndex = 1
        '
        'lblSections
        '
        Me.lblSections.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblSections.Location = New System.Drawing.Point(0, 0)
        Me.lblSections.Name = "lblSections"
        Me.lblSections.Size = New System.Drawing.Size(200, 16)
        Me.lblSections.TabIndex = 0
        Me.lblSections.Text = " All Sections"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.ListView1)
        Me.Panel2.Controls.Add(Me.sbKeys)
        Me.Panel2.Controls.Add(Me.lblKeys)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(200, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(504, 305)
        Me.Panel2.TabIndex = 5
        '
        'ListView1
        '
        Me.ListView1.AllowDrop = True
        Me.ListView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colKey, Me.colValue})
        Me.ListView1.ContextMenu = Me.cmKeys
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.FullRowSelect = True
        Me.ListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ListView1.LargeImageList = Me.ImageList1
        Me.ListView1.Location = New System.Drawing.Point(0, 16)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(504, 267)
        Me.ListView1.SmallImageList = Me.ImageList1
        Me.ListView1.StateImageList = Me.ImageList1
        Me.ListView1.TabIndex = 3
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'colKey
        '
        Me.colKey.Text = "Key"
        Me.colKey.Width = 220
        '
        'colValue
        '
        Me.colValue.Text = "Value"
        Me.colValue.Width = 240
        '
        'cmKeys
        '
        Me.cmKeys.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.cmnuAddKey, Me.cmnuRenameKey, Me.cmnuKeyValue, Me.cmnuDeleteKey, Me.cmnuMoveKey, Me.cmnuCommentKey, Me.cmnuUnCommentKey})
        '
        'cmnuAddKey
        '
        Me.cmnuAddKey.Index = 0
        Me.cmnuAddKey.Text = "Add"
        '
        'cmnuRenameKey
        '
        Me.cmnuRenameKey.Index = 1
        Me.cmnuRenameKey.Text = "&Rename"
        '
        'cmnuKeyValue
        '
        Me.cmnuKeyValue.Index = 2
        Me.cmnuKeyValue.Text = "&Value"
        '
        'cmnuDeleteKey
        '
        Me.cmnuDeleteKey.Index = 3
        Me.cmnuDeleteKey.Text = "&Delete"
        '
        'cmnuMoveKey
        '
        Me.cmnuMoveKey.Index = 4
        Me.cmnuMoveKey.Text = "&Move"
        '
        'cmnuCommentKey
        '
        Me.cmnuCommentKey.Index = 5
        Me.cmnuCommentKey.Text = "Comment"
        '
        'cmnuUnCommentKey
        '
        Me.cmnuUnCommentKey.Index = 6
        Me.cmnuUnCommentKey.Text = "UnComment"
        '
        'sbKeys
        '
        Me.sbKeys.Location = New System.Drawing.Point(0, 283)
        Me.sbKeys.Name = "sbKeys"
        Me.sbKeys.Size = New System.Drawing.Size(504, 22)
        Me.sbKeys.TabIndex = 1
        '
        'lblKeys
        '
        Me.lblKeys.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblKeys.Location = New System.Drawing.Point(0, 0)
        Me.lblKeys.Name = "lblKeys"
        Me.lblKeys.Size = New System.Drawing.Size(504, 16)
        Me.lblKeys.TabIndex = 0
        Me.lblKeys.Text = " Contents of Properties"
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(200, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 305)
        Me.Splitter1.TabIndex = 6
        Me.Splitter1.TabStop = False
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(704, 305)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Menu = Me.MainMenu1
        Me.Name = "Form1"
        Me.Text = "Test IniFile"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Dim myIniFile As IniFile.IniFile
    Dim CurrentSection As String = ""
    Dim FromSection As String = ""

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        Me.Close()
    End Sub

    Private Sub mnuOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOpen.Click
        OpenFileDialog1.InitialDirectory = "c:\"
        OpenFileDialog1.Filter = "INI files (*.ini)|*.ini|All files (*.*)|*.*"
        OpenFileDialog1.FilterIndex = 1
        OpenFileDialog1.RestoreDirectory = True
        Dim myStream As Stream

        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            myStream = OpenFileDialog1.OpenFile()
            If Not (myStream Is Nothing) Then
                myStream.Close()
                myIniFile = New IniFile.IniFile(OpenFileDialog1.FileName, False)
                RefreshSections()
            End If
        End If
    End Sub

    Private Sub TreeView1_AfterSelect_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        sbKeys.Text = ""
        CurrentSection = TreeView1.SelectedNode.Text
        RefreshKeys(CurrentSection)
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        Dim Item As ListViewItem = GetSelectedKey()
        If Not Item Is Nothing Then
            sbKeys.Text = "Key: " & Item.Text & "=" & Item.SubItems(1).Text
        End If
    End Sub

    Private Function GetSelectedKey() As ListViewItem
        Dim Item As ListViewItem
        For Each Item In ListView1.SelectedItems
            Return Item
        Next
    End Function

    Private Sub RefreshSections()
        TreeView1.Nodes.Clear()
        sbKeys.Text = ""
        Dim CurrentNode

        Dim Root As New TreeNode
        Root.Text = OpenFileDialog1.FileName
        Root.ImageIndex = 5

        Dim Sections As ArrayList = myIniFile.GetSections()
        Dim myEnumerator As System.Collections.IEnumerator = Sections.GetEnumerator()
        While myEnumerator.MoveNext()
            Dim myNode As New TreeNode
            myNode.Text = myEnumerator.Current.Name
            If myEnumerator.Current.iscommented Then myNode.ForeColor = Color.Green
            Root.Nodes.Add(myNode)
        End While

        Root.Expand()
        TreeView1.Nodes.Add(Root)
        sbSections.Text = TreeView1.GetNodeCount(True) - 1 & " Item(s)"
        If CurrentSection = "" Then
            CurrentSection = Root.FirstNode.Text
            Root.TreeView.SelectedNode = Root.FirstNode
        End If
    End Sub

    Private Sub RefreshKeys(ByVal SectionName As String)
        ListView1.Items.Clear()
        Dim Keys As ArrayList = myIniFile.GetKeys(SectionName)
        Dim myEnumerator As System.Collections.IEnumerator = Keys.GetEnumerator()
        While myEnumerator.MoveNext()
            Dim myItem As New ListViewItem
            myItem.Text = (myEnumerator.Current.Name)
            myItem.SubItems.Add(myEnumerator.Current.value)
            myItem.ImageIndex = 3
            If myEnumerator.Current.IsCommented Then myItem.ForeColor = Color.Green
            ListView1.Items.Add(myItem)
        End While
    End Sub

    Private Sub CommentSection(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuCommentSection.Click, mnuCommentSection.Click
        If Not TreeView1.SelectedNode Is Nothing Then
            myIniFile.CommentSection(TreeView1.SelectedNode.Text)
            RefreshKeys(CurrentSection)
            RefreshSections()
        End If
    End Sub

    Private Sub UnCommentSection(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuUnCommentSection.Click, mnuUnCommentSection.Click
        If Not TreeView1.SelectedNode Is Nothing Then
            myIniFile.UnCommentSection(TreeView1.SelectedNode.Text)
            RefreshKeys(CurrentSection)
            RefreshSections()
        End If
    End Sub

    Private Sub mnuAddSection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddSection.Click, cmnuAddSection.Click
        Dim SectionName As String = InputBox("Enter a new section name", "Add Section", "", 100, 100)
        If SectionName <> "" Then myIniFile.AddSection(SectionName)
        RefreshSections()
    End Sub

    Private Sub AddKey(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddKey.Click, cmnuAddKey.Click
        If Not TreeView1.SelectedNode Is Nothing Or ListView1.SelectedItems.Count > 0 Then
            Dim KeyName As String = InputBox("Enter a new Key name", "Add Key", "", 100, 100)
            Dim KeyValue As String = InputBox("Enter a new Key value", "Add Key", "", 100, 100)
            If KeyName <> "" And KeyValue <> "" Then
                If Not ListView1.SelectedItems.Count = 0 Then
                    myIniFile.AddKey(KeyName, KeyValue, CurrentSection, False, GetSelectedKey().Text)
                    RefreshKeys(CurrentSection)
                    RefreshSections()
                Else
                    myIniFile.AddKey(KeyName, KeyValue, CurrentSection)
                    RefreshKeys(CurrentSection)
                    RefreshSections()
                End If
            End If
        End If
    End Sub

    Private Sub UnCommentKey(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUnCommentKey.Click, cmnuUnCommentKey.Click
        If ListView1.SelectedItems.Count > 0 Then
            myIniFile.UnCommentKey(GetSelectedKey().Text, CurrentSection)
            RefreshKeys(CurrentSection)
            RefreshSections()
        End If
    End Sub

    Private Sub CommentKey(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCommentKey.Click, cmnuCommentKey.Click
        If ListView1.SelectedItems.Count > 0 Then
            myIniFile.CommentKey(GetSelectedKey().Text, CurrentSection)
            RefreshKeys(CurrentSection)
            RefreshSections()
        End If
    End Sub

    Private Sub EditSection(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditSection.Click, cmnuEditSection.Click
        If Not TreeView1.SelectedNode Is Nothing Then
            Dim SectionName As String = InputBox("Enter a new section name", "Rename Section", "", 100, 100)
            myIniFile.RenameSection(TreeView1.SelectedNode.Text, SectionName)
            RefreshKeys(CurrentSection)
            RefreshSections()
        End If
    End Sub

    Private Sub DeleteSection(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDeleteSection.Click, cmnuDeleteSection.Click
        If Not TreeView1.SelectedNode Is Nothing Then
            myIniFile.DeleteSection(TreeView1.SelectedNode.Text)
            RefreshKeys(CurrentSection)
            RefreshSections()
        End If
    End Sub

    Private Sub RenameKey(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRenameKey.Click, cmnuRenameKey.Click
        If ListView1.SelectedItems.Count > 0 Then
            Dim KeyName As String = InputBox("Enter a new Key name", "Rename Key", "", 100, 100)
            myIniFile.RenameKey(GetSelectedKey().Text, CurrentSection, KeyName)
            RefreshKeys(CurrentSection)
            RefreshSections()
        End If
    End Sub

    Private Sub DeleteKey(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDeleteKey.Click, cmnuDeleteKey.Click
        If ListView1.SelectedItems.Count > 0 Then
            myIniFile.DeleteKey(GetSelectedKey().Text, CurrentSection)
            RefreshKeys(CurrentSection)
            RefreshSections()
        End If
    End Sub

    Private Sub TreeView1_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles TreeView1.ItemDrag
        DoDragDrop(e.Item, DragDropEffects.Move)
    End Sub

    Private Sub TreeView1_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView1.DragEnter
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub TreeView1_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView1.DragDrop
        Dim lvItem As ListViewItem
        If e.Data.GetDataPresent("System.Windows.Forms.ListViewItem", False) Then
            Dim pt As Point
            Dim DestinationNode As TreeNode
            pt = CType(sender, TreeView).PointToClient(New Point(e.X, e.Y))
            DestinationNode = CType(sender, TreeView).GetNodeAt(pt)
            lvItem = CType(e.Data.GetData("System.Windows.Forms.ListViewItem"), ListViewItem)
            myIniFile.MoveKey(lvItem.Text, FromSection, DestinationNode.Text)
            RefreshKeys(CurrentSection)
            RefreshSections()
        End If
    End Sub

    Private Sub ListView1_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles ListView1.ItemDrag
        DoDragDrop(e.Item, DragDropEffects.Move)
    End Sub

    Private Sub TreeView1_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView1.DragOver
        Dim pt As Point
        Dim DestinationNode As TreeNode
        pt = CType(sender, TreeView).PointToClient(New Point(e.X, e.Y))
        DestinationNode = CType(sender, TreeView).GetNodeAt(pt)
        TreeView1.Focus()
        TreeView1.SelectedNode = DestinationNode
    End Sub

    Private Sub ListView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseDown
        FromSection = CurrentSection
    End Sub

    Private Sub Sort(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSort.Click
        myIniFile.Sort()
        RefreshKeys(CurrentSection)
        RefreshSections()
    End Sub

    Private Sub KeyValue(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuKeyValue.Click, cmnuRenameKey.Click
        If ListView1.SelectedItems.Count > 0 Then
            Dim KeyValue As String = InputBox("Enter a new Key value", "Key Value", "", 100, 100)
            myIniFile.ChangeValue(GetSelectedKey().Text, CurrentSection, KeyValue)
            RefreshKeys(CurrentSection)
            RefreshSections()
        End If
    End Sub

    Private Sub mnuSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSave.Click
        Dim myStream As Stream
        Dim saveFileDialog1 As New SaveFileDialog

        saveFileDialog1.Filter = "INI files (*.ini)|*.ini|All files (*.*)|*.*"
        saveFileDialog1.FilterIndex = 1
        saveFileDialog1.RestoreDirectory = True
        saveFileDialog1.FileName = OpenFileDialog1.FileName

        If saveFileDialog1.ShowDialog() = DialogResult.OK Then
            myStream = saveFileDialog1.OpenFile()
            If Not (myStream Is Nothing) Then
                myStream.Close()
                myIniFile.Save(saveFileDialog1.FileName)
            End If
        End If
    End Sub

    Private Sub SaveToXML(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSaveToXML.Click
        Dim myStream As Stream
        Dim saveFileDialog1 As New SaveFileDialog

        saveFileDialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
        saveFileDialog1.FilterIndex = 1
        saveFileDialog1.RestoreDirectory = True
        saveFileDialog1.FileName = OpenFileDialog1.FileName

        If saveFileDialog1.ShowDialog() = DialogResult.OK Then
            myStream = saveFileDialog1.OpenFile()
            If Not (myStream Is Nothing) Then
                myStream.Close()
                myIniFile.SaveXML(saveFileDialog1.FileName)
            End If
        End If
    End Sub

    Private Sub ShowText(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowText.Click
        MsgBox(myIniFile.Text)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim AppIni As New IniFile.IniFile("C:\myApp.ini", True)
        AppIni.GetFormSettings(Me)
    End Sub

    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Dim AppIni As New IniFile.IniFile("C:\myApp.ini")
        AppIni.SaveFormSettings(Me)
    End Sub
End Class