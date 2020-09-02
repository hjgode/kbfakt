' ----- Enhanced Print Preview Application
'       Written by Tim Patrick
'       For Visual Studio Magazine

Option Explicit On 
Option Strict On

Public Class ControlMethod
    ' ----- This form emulates the PrintPreviewDialog control, but adds some
    '       additional features. To use it, create an instance, set the Document
    '       property, and call ShowDialog. The return value is DialogResult.OK
    '       if the user wants to print the document, or DialogResult.Cancel to
    '       simply end the printing process.
    Inherits System.Windows.Forms.Form

    Private WithEvents InternalDoc As System.Drawing.Printing.PrintDocument
    Private TotalPages As Integer

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
    Protected WithEvents PageRange As System.Windows.Forms.NumericUpDown
    Protected WithEvents ToolbarImages As System.Windows.Forms.ImageList
    Protected WithEvents ZoomMenu As System.Windows.Forms.ContextMenu
    Protected WithEvents LabelPageRange As System.Windows.Forms.Label
    Protected WithEvents ActClose As System.Windows.Forms.Button
    Protected WithEvents ActPrint As System.Windows.Forms.Button
    Protected WithEvents ActZoomAuto As System.Windows.Forms.Button
    Protected WithEvents ActOnePage As System.Windows.Forms.Button
    Protected WithEvents ActTwoPages As System.Windows.Forms.Button
    Protected WithEvents ActThreePages As System.Windows.Forms.Button
    Protected WithEvents ActFourPages As System.Windows.Forms.Button
    Protected WithEvents ActSixPages As System.Windows.Forms.Button
    Protected WithEvents SmallImages As System.Windows.Forms.ImageList
    Protected WithEvents MenuAuto As System.Windows.Forms.MenuItem
    Protected WithEvents Menu200 As System.Windows.Forms.MenuItem
    Protected WithEvents Menu150 As System.Windows.Forms.MenuItem
    Protected WithEvents Menu100 As System.Windows.Forms.MenuItem
    Protected WithEvents Menu75 As System.Windows.Forms.MenuItem
    Protected WithEvents Menu50 As System.Windows.Forms.MenuItem
    Protected WithEvents Menu25 As System.Windows.Forms.MenuItem
    Protected WithEvents Menu10 As System.Windows.Forms.MenuItem
    Protected WithEvents Menu500 As System.Windows.Forms.MenuItem
    Protected WithEvents ActZoom As System.Windows.Forms.Button
    Protected WithEvents PrintToolbar As System.Windows.Forms.Panel
    Protected WithEvents PreviewArea As System.Windows.Forms.PrintPreviewControl
    Protected WithEvents TOCList As System.Windows.Forms.ListBox
    Protected WithEvents TOCSplitter As System.Windows.Forms.Splitter
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ControlMethod))
        Me.PrintToolbar = New System.Windows.Forms.Panel
        Me.ActZoom = New System.Windows.Forms.Button
        Me.SmallImages = New System.Windows.Forms.ImageList(Me.components)
        Me.ActSixPages = New System.Windows.Forms.Button
        Me.ToolbarImages = New System.Windows.Forms.ImageList(Me.components)
        Me.ActFourPages = New System.Windows.Forms.Button
        Me.ActThreePages = New System.Windows.Forms.Button
        Me.ActTwoPages = New System.Windows.Forms.Button
        Me.ActOnePage = New System.Windows.Forms.Button
        Me.ActZoomAuto = New System.Windows.Forms.Button
        Me.ActPrint = New System.Windows.Forms.Button
        Me.ActClose = New System.Windows.Forms.Button
        Me.PageRange = New System.Windows.Forms.NumericUpDown
        Me.LabelPageRange = New System.Windows.Forms.Label
        Me.ZoomMenu = New System.Windows.Forms.ContextMenu
        Me.MenuAuto = New System.Windows.Forms.MenuItem
        Me.Menu500 = New System.Windows.Forms.MenuItem
        Me.Menu200 = New System.Windows.Forms.MenuItem
        Me.Menu150 = New System.Windows.Forms.MenuItem
        Me.Menu100 = New System.Windows.Forms.MenuItem
        Me.Menu75 = New System.Windows.Forms.MenuItem
        Me.Menu50 = New System.Windows.Forms.MenuItem
        Me.Menu25 = New System.Windows.Forms.MenuItem
        Me.Menu10 = New System.Windows.Forms.MenuItem
        Me.TOCList = New System.Windows.Forms.ListBox
        Me.TOCSplitter = New System.Windows.Forms.Splitter
        Me.PreviewArea = New System.Windows.Forms.PrintPreviewControl
        Me.PrintToolbar.SuspendLayout()
        CType(Me.PageRange, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PrintToolbar
        '
        Me.PrintToolbar.Controls.Add(Me.ActZoom)
        Me.PrintToolbar.Controls.Add(Me.ActSixPages)
        Me.PrintToolbar.Controls.Add(Me.ActFourPages)
        Me.PrintToolbar.Controls.Add(Me.ActThreePages)
        Me.PrintToolbar.Controls.Add(Me.ActTwoPages)
        Me.PrintToolbar.Controls.Add(Me.ActOnePage)
        Me.PrintToolbar.Controls.Add(Me.ActZoomAuto)
        Me.PrintToolbar.Controls.Add(Me.ActPrint)
        Me.PrintToolbar.Controls.Add(Me.ActClose)
        Me.PrintToolbar.Controls.Add(Me.PageRange)
        Me.PrintToolbar.Controls.Add(Me.LabelPageRange)
        Me.PrintToolbar.Dock = System.Windows.Forms.DockStyle.Top
        Me.PrintToolbar.Location = New System.Drawing.Point(0, 0)
        Me.PrintToolbar.Name = "PrintToolbar"
        Me.PrintToolbar.Size = New System.Drawing.Size(536, 36)
        Me.PrintToolbar.TabIndex = 1
        '
        'ActZoom
        '
        Me.ActZoom.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ActZoom.ImageIndex = 0
        Me.ActZoom.ImageList = Me.SmallImages
        Me.ActZoom.Location = New System.Drawing.Point(60, 4)
        Me.ActZoom.Name = "ActZoom"
        Me.ActZoom.Size = New System.Drawing.Size(12, 24)
        Me.ActZoom.TabIndex = 14
        '
        'SmallImages
        '
        Me.SmallImages.ImageSize = New System.Drawing.Size(5, 3)
        Me.SmallImages.ImageStream = CType(resources.GetObject("SmallImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.SmallImages.TransparentColor = System.Drawing.Color.Red
        '
        'ActSixPages
        '
        Me.ActSixPages.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ActSixPages.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.ActSixPages.ImageIndex = 6
        Me.ActSixPages.ImageList = Me.ToolbarImages
        Me.ActSixPages.Location = New System.Drawing.Point(200, 4)
        Me.ActSixPages.Name = "ActSixPages"
        Me.ActSixPages.Size = New System.Drawing.Size(24, 24)
        Me.ActSixPages.TabIndex = 11
        '
        'ToolbarImages
        '
        Me.ToolbarImages.ImageSize = New System.Drawing.Size(16, 16)
        Me.ToolbarImages.ImageStream = CType(resources.GetObject("ToolbarImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ToolbarImages.TransparentColor = System.Drawing.Color.Red
        '
        'ActFourPages
        '
        Me.ActFourPages.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ActFourPages.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.ActFourPages.ImageIndex = 5
        Me.ActFourPages.ImageList = Me.ToolbarImages
        Me.ActFourPages.Location = New System.Drawing.Point(172, 4)
        Me.ActFourPages.Name = "ActFourPages"
        Me.ActFourPages.Size = New System.Drawing.Size(24, 24)
        Me.ActFourPages.TabIndex = 10
        '
        'ActThreePages
        '
        Me.ActThreePages.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ActThreePages.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.ActThreePages.ImageIndex = 4
        Me.ActThreePages.ImageList = Me.ToolbarImages
        Me.ActThreePages.Location = New System.Drawing.Point(144, 4)
        Me.ActThreePages.Name = "ActThreePages"
        Me.ActThreePages.Size = New System.Drawing.Size(24, 24)
        Me.ActThreePages.TabIndex = 9
        '
        'ActTwoPages
        '
        Me.ActTwoPages.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ActTwoPages.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.ActTwoPages.ImageIndex = 3
        Me.ActTwoPages.ImageList = Me.ToolbarImages
        Me.ActTwoPages.Location = New System.Drawing.Point(116, 4)
        Me.ActTwoPages.Name = "ActTwoPages"
        Me.ActTwoPages.Size = New System.Drawing.Size(24, 24)
        Me.ActTwoPages.TabIndex = 8
        '
        'ActOnePage
        '
        Me.ActOnePage.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ActOnePage.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.ActOnePage.ImageIndex = 2
        Me.ActOnePage.ImageList = Me.ToolbarImages
        Me.ActOnePage.Location = New System.Drawing.Point(88, 4)
        Me.ActOnePage.Name = "ActOnePage"
        Me.ActOnePage.Size = New System.Drawing.Size(24, 24)
        Me.ActOnePage.TabIndex = 7
        '
        'ActZoomAuto
        '
        Me.ActZoomAuto.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ActZoomAuto.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.ActZoomAuto.ImageIndex = 1
        Me.ActZoomAuto.ImageList = Me.ToolbarImages
        Me.ActZoomAuto.Location = New System.Drawing.Point(36, 4)
        Me.ActZoomAuto.Name = "ActZoomAuto"
        Me.ActZoomAuto.Size = New System.Drawing.Size(24, 24)
        Me.ActZoomAuto.TabIndex = 6
        '
        'ActPrint
        '
        Me.ActPrint.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ActPrint.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.ActPrint.ImageIndex = 0
        Me.ActPrint.ImageList = Me.ToolbarImages
        Me.ActPrint.Location = New System.Drawing.Point(8, 4)
        Me.ActPrint.Name = "ActPrint"
        Me.ActPrint.Size = New System.Drawing.Size(24, 24)
        Me.ActPrint.TabIndex = 5
        '
        'ActClose
        '
        Me.ActClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ActClose.Location = New System.Drawing.Point(240, 4)
        Me.ActClose.Name = "ActClose"
        Me.ActClose.Size = New System.Drawing.Size(64, 24)
        Me.ActClose.TabIndex = 4
        Me.ActClose.Text = "Close"
        '
        'PageRange
        '
        Me.PageRange.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PageRange.Location = New System.Drawing.Point(464, 8)
        Me.PageRange.Name = "PageRange"
        Me.PageRange.Size = New System.Drawing.Size(64, 20)
        Me.PageRange.TabIndex = 3
        Me.PageRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.PageRange.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'LabelPageRange
        '
        Me.LabelPageRange.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelPageRange.AutoSize = True
        Me.LabelPageRange.Location = New System.Drawing.Point(416, 10)
        Me.LabelPageRange.Name = "LabelPageRange"
        Me.LabelPageRange.Size = New System.Drawing.Size(31, 16)
        Me.LabelPageRange.TabIndex = 0
        Me.LabelPageRange.Text = "Page"
        '
        'ZoomMenu
        '
        Me.ZoomMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuAuto, Me.Menu500, Me.Menu200, Me.Menu150, Me.Menu100, Me.Menu75, Me.Menu50, Me.Menu25, Me.Menu10})
        '
        'MenuAuto
        '
        Me.MenuAuto.Checked = True
        Me.MenuAuto.Index = 0
        Me.MenuAuto.Text = "Auto"
        '
        'Menu500
        '
        Me.Menu500.Index = 1
        Me.Menu500.Text = "500%"
        '
        'Menu200
        '
        Me.Menu200.Index = 2
        Me.Menu200.Text = "200%"
        '
        'Menu150
        '
        Me.Menu150.Index = 3
        Me.Menu150.Text = "150%"
        '
        'Menu100
        '
        Me.Menu100.Index = 4
        Me.Menu100.Text = "100%"
        '
        'Menu75
        '
        Me.Menu75.Index = 5
        Me.Menu75.Text = "75%"
        '
        'Menu50
        '
        Me.Menu50.Index = 6
        Me.Menu50.Text = "50%"
        '
        'Menu25
        '
        Me.Menu25.Index = 7
        Me.Menu25.Text = "25%"
        '
        'Menu10
        '
        Me.Menu10.Index = 8
        Me.Menu10.Text = "10%"
        '
        'TOCList
        '
        Me.TOCList.DisplayMember = "PageNumber"
        Me.TOCList.Dock = System.Windows.Forms.DockStyle.Left
        Me.TOCList.IntegralHeight = False
        Me.TOCList.Location = New System.Drawing.Point(0, 36)
        Me.TOCList.Name = "TOCList"
        Me.TOCList.Size = New System.Drawing.Size(136, 338)
        Me.TOCList.TabIndex = 3
        Me.TOCList.ValueMember = "ToValue"
        '
        'TOCSplitter
        '
        Me.TOCSplitter.Location = New System.Drawing.Point(136, 36)
        Me.TOCSplitter.Name = "TOCSplitter"
        Me.TOCSplitter.Size = New System.Drawing.Size(8, 338)
        Me.TOCSplitter.TabIndex = 4
        Me.TOCSplitter.TabStop = False
        '
        'PreviewArea
        '
        Me.PreviewArea.AutoZoom = False
        Me.PreviewArea.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PreviewArea.Location = New System.Drawing.Point(144, 36)
        Me.PreviewArea.Name = "PreviewArea"
        Me.PreviewArea.Size = New System.Drawing.Size(392, 338)
        Me.PreviewArea.TabIndex = 5
        Me.PreviewArea.Zoom = 1
        '
        'ControlMethod
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(536, 374)
        Me.Controls.Add(Me.PreviewArea)
        Me.Controls.Add(Me.TOCSplitter)
        Me.Controls.Add(Me.TOCList)
        Me.Controls.Add(Me.PrintToolbar)
        Me.MinimumSize = New System.Drawing.Size(475, 350)
        Me.Name = "ControlMethod"
        Me.Text = "Preview By Control"
        Me.PrintToolbar.ResumeLayout(False)
        CType(Me.PageRange, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Property Document() As System.Drawing.Printing.PrintDocument
        ' ----- Gets or sets the internal version of the document.
        Get
            Return InternalDoc
        End Get
        Set(ByVal Value As System.Drawing.Printing.PrintDocument)
            InternalDoc = Value
            PreviewArea.Document = Value
        End Set
    End Property
    Public Property UseAntiAlias() As Boolean
        ' ----- Gets or sets the anti-alias flag on the preview control.
        Get
            Return PreviewArea.UseAntiAlias
        End Get
        Set(ByVal Value As Boolean)
            PreviewArea.UseAntiAlias = Value
        End Set
    End Property

    Private Sub InternalDoc_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles InternalDoc.BeginPrint
        ' ----- Prepare for a new print job by clearing any old values.
        TOCList.Items.Clear()
        TotalPages = 0
    End Sub
    Private Sub InternalDoc_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles InternalDoc.PrintPage
        ' ----- Advance to the next page. There is likely another PrintPage event
        '       somewhere else in the program that adds content to the page.
        TotalPages += 1
    End Sub
    Private Sub InternalDoc_EndPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles InternalDoc.EndPrint
        ' ----- Finished printing. We can now display the table of contents.
        Dim scanTOC As System.Collections.IEnumerator
        Dim oneEntry As PrintingTest.TableOfContentsEntry
        Dim tocDoc As PrintingTest.DocumentWithTOC

        ' ----- Move to the first page at the default (auto) size.
        PageRange.Minimum = 1
        PageRange.Maximum = TotalPages
        ZoomDisplay(0.0, MenuAuto)

        ' ----- Build the table of contents if available.
        If (TypeOf InternalDoc Is PrintingTest.DocumentWithTOC) Then
            ' ----- Scan through the TOC entries.
            tocDoc = CType(InternalDoc, PrintingTest.DocumentWithTOC)
            scanTOC = tocDoc.TableOfContents.GetEnumerator()
            Do While scanTOC.MoveNext()
                ' ----- Add this one entry. The ToString method on the instance
                '       will ensure that the item displays properly.
                oneEntry = CType(scanTOC.Current, PrintingTest.TableOfContentsEntry)
                TOCList.Items.Add(oneEntry)
            Loop
        End If
    End Sub

    Private Sub PageRange_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageRange.ValueChanged
        ' ----- Display the requested starting page. PageRange is base-1, but
        '       PreviewArea.StartPage is base-0.
        PreviewArea.StartPage = CInt(PageRange.Value) - 1
    End Sub

    Private Sub ActClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActClose.Click
        ' ----- Close the preview.
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub ActPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActPrint.Click
        ' ----- Close the preview, and inform the caller to print.
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub ActOnePage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActOnePage.Click
        ' ----- Display 1x1 pages.
        PreviewArea.Columns = 1
        PreviewArea.Rows = 1
    End Sub
    Private Sub ActTwoPages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActTwoPages.Click
        ' ----- Display 2x1 pages.
        PreviewArea.Columns = 2
        PreviewArea.Rows = 1
    End Sub
    Private Sub ActThreePages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActThreePages.Click
        ' ----- Display 3x1 pages.
        PreviewArea.Columns = 3
        PreviewArea.Rows = 1
    End Sub
    Private Sub ActFourPages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActFourPages.Click
        ' ----- Display 2x2 pages.
        PreviewArea.Columns = 2
        PreviewArea.Rows = 2
    End Sub
    Private Sub ActSixPages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActSixPages.Click
        ' ----- Display 3x2 pages.
        PreviewArea.Columns = 3
        PreviewArea.Rows = 2
    End Sub

    Private Sub MenuAuto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAuto.Click
        ' ----- Auto-size the preview area.
        ZoomDisplay(0.0, MenuAuto)
    End Sub
    Private Sub Menu500_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Menu500.Click
        ' ----- Zoom the preview area to 500%.
        ZoomDisplay(5.0, Menu500)
    End Sub
    Private Sub Menu200_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Menu200.Click
        ' ----- Zoom the preview area to 200%.
        ZoomDisplay(2.0, Menu200)
    End Sub
    Private Sub Menu150_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Menu150.Click
        ' ----- Zoom the preview area to 150%.
        ZoomDisplay(1.5, Menu150)
    End Sub
    Private Sub Menu100_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Menu100.Click
        ' ----- Zoom the preview area to 100%.
        ZoomDisplay(1.0, Menu100)
    End Sub
    Private Sub Menu75_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Menu75.Click
        ' ----- Zoom the preview area to 75%.
        ZoomDisplay(0.75, Menu75)
    End Sub
    Private Sub Menu50_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Menu50.Click
        ' ----- Zoom the preview area to 50%.
        ZoomDisplay(0.5, Menu50)
    End Sub
    Private Sub Menu25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Menu25.Click
        ' ----- Zoom the preview area to 25%.
        ZoomDisplay(0.25, Menu25)
    End Sub
    Private Sub Menu10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Menu10.Click
        ' ----- Zoom the preview area to 10%.
        ZoomDisplay(0.1, Menu10)
    End Sub
    Private Sub ZoomDisplay(ByVal zoomFactor As Double, ByVal selectedMenu As Windows.Forms.MenuItem)
        ' ----- Remove the check from all menus.
        MenuAuto.Checked = False
        Menu500.Checked = False
        Menu200.Checked = False
        Menu150.Checked = False
        Menu100.Checked = False
        Menu75.Checked = False
        Menu50.Checked = False
        Menu25.Checked = False
        Menu10.Checked = False

        ' ----- Check the selected menu.
        selectedMenu.Checked = True

        ' ----- Set the auto-zoom as needed.
        PreviewArea.AutoZoom = CBool(zoomFactor = 0.0)

        ' ----- Zoom the display.
        If (zoomFactor > 0.0) Then PreviewArea.Zoom = zoomFactor
    End Sub

    Private Sub ActZoomAuto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActZoomAuto.Click
        ' ---- Same as the "auto" menu.
        ZoomDisplay(0.0, MenuAuto)
    End Sub

    Private Sub ActZoom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActZoom.Click
        ' ----- Show the zoom menu.
        ZoomMenu.Show(ActZoom, (New System.Drawing.Point(0, ActZoom.Height)))
    End Sub

    Private Sub TOCList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TOCList.SelectedIndexChanged
        ' ----- Jump to a table-of-contents entry as indicated by the user.
        Dim tocEntry As PrintingTest.TableOfContentsEntry

        If (TOCList.SelectedIndex = -1) Then Exit Sub
        tocEntry = CType(TOCList.SelectedItem, PrintingTest.TableOfContentsEntry)
        If (tocEntry.PageNumber > 0) And (tocEntry.PageNumber <= PageRange.Maximum) _
            Then PageRange.Value = tocEntry.PageNumber
    End Sub
End Class
