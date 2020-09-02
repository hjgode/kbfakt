' ----- Enhanced Print Preview Application
'       Written by Tim Patrick
'       For Visual Studio Magazine

Option Explicit On 
Option Strict On

Imports MVB = Microsoft.VisualBasic

Public Class MainForm
    Inherits System.Windows.Forms.Form

    Private Const stateNames As String = _
        "Alabama,Alaska,Arizona,Arkansas,California,Colorado," & _
        "Connecticut,Delaware,Florida,Georgia,Hawaii,Idaho," & _
        "Illinois,Indiana,Iowa,Kansas,Kentucky,Louisiana,Maine," & _
        "Maryland,Massachusetts,Michigan,Minnesota,Mississippi," & _
        "Missouri,Montana,Nebraska,Nevada,New Hampshire,New Jersey," & _
        "New Mexico,New York,North Carolina,North Dakota,Ohio," & _
        "Oklahoma,Oregon,Pennsylvania,Rhode Island,South Carolina," & _
        "South Dakota,Tennessee,Texas,Utah,Vermont,Virginia," & _
        "Washington,West Virginia,Wisconsin,Wyoming"
    Private StateArray() As String
    Private PageSoFar As Integer
    Private WithEvents BarcodeDoc As PrintingTest.DocumentWithTOC

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
    Friend WithEvents TestPreview As System.Windows.Forms.PrintPreviewDialog
    Friend WithEvents TestByDialog As System.Windows.Forms.Button
    Friend WithEvents TestByControl As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.TestByDialog = New System.Windows.Forms.Button
        Me.TestByControl = New System.Windows.Forms.Button
        Me.TestPreview = New System.Windows.Forms.PrintPreviewDialog
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'TestByDialog
        '
        Me.TestByDialog.Location = New System.Drawing.Point(32, 128)
        Me.TestByDialog.Name = "TestByDialog"
        Me.TestByDialog.Size = New System.Drawing.Size(88, 24)
        Me.TestByDialog.TabIndex = 2
        Me.TestByDialog.Text = "By &Dialog"
        '
        'TestByControl
        '
        Me.TestByControl.Location = New System.Drawing.Point(176, 128)
        Me.TestByControl.Name = "TestByControl"
        Me.TestByControl.Size = New System.Drawing.Size(88, 24)
        Me.TestByControl.TabIndex = 3
        Me.TestByControl.Text = "By &Control"
        '
        'TestPreview
        '
        Me.TestPreview.AutoScrollMargin = New System.Drawing.Size(0, 0)
        Me.TestPreview.AutoScrollMinSize = New System.Drawing.Size(0, 0)
        Me.TestPreview.ClientSize = New System.Drawing.Size(400, 300)
        Me.TestPreview.Enabled = True
        Me.TestPreview.Icon = CType(resources.GetObject("TestPreview.Icon"), System.Drawing.Icon)
        Me.TestPreview.Location = New System.Drawing.Point(30, 22)
        Me.TestPreview.MinimumSize = New System.Drawing.Size(375, 250)
        Me.TestPreview.Name = "TestPreview"
        Me.TestPreview.TransparencyKey = System.Drawing.Color.Empty
        Me.TestPreview.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(210, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Enhanced Print Preview Demonstration"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(16, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(304, 80)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "This previews a 50-page document using both the PrintPreviewDialog control, and a" & _
        " custom form that includes the PrintPreviewControl component and a table-of-cont" & _
        "ents feature. To view each type of print preview, click on one of the buttons be" & _
        "low."
        Me.Label2.UseMnemonic = False
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(330, 160)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TestByControl)
        Me.Controls.Add(Me.TestByDialog)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Print Preview Demonstration"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub TestByDialog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestByDialog.Click
        ' ----- Display a sample document using the PrintPreviewDialog.
        PageSoFar = 0
        BarcodeDoc = New PrintingTest.DocumentWithTOC
        TestPreview.Document = BarcodeDoc
        TestPreview.ShowDialog()
        BarcodeDoc = Nothing
    End Sub

    Private Sub TestByControl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestByControl.Click
        ' ----- Display a sample document using the PrintPreviewControl.
        Dim previewForm As PrintingTest.ControlMethod
        Dim timeToPrint As DialogResult

        PageSoFar = 0
        BarcodeDoc = New PrintingTest.DocumentWithTOC
        previewForm = New ControlMethod
        previewForm.Document = BarcodeDoc

        ' ----- Show the dialog. If timeToPrint is DialogResult.OK, then
        '       initiate the printing process (not included in this test
        '       program).
        timeToPrint = previewForm.ShowDialog()
        previewForm = Nothing
        BarcodeDoc = Nothing
    End Sub

    Private Sub BarcodeDoc_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles BarcodeDoc.PrintPage
        ' ----- Print one page of the document.
        Dim theDocument As PrintingTest.DocumentWithTOC
        Dim thisGroup As String
        Static lastGroup As String = ""

        ' ----- Display the name of one state on the top of this page.
        e.Graphics.DrawString(StateArray(PageSoFar), (New Font("Arial", 36)), _
            Brushes.Black, 100, 100)
        thisGroup = UCase(MVB.Left(StateArray(PageSoFar), 1))
        PageSoFar += 1

        ' ----- Add one or two entries to the table of contents. Add one
        '       entry for the state itself, and if this state starts with
        '       a letter not yet encountered, add a group-by-letter entry.
        theDocument = CType(sender, PrintingTest.DocumentWithTOC)
        If (thisGroup <> lastGroup) Then _
            theDocument.AddTOCEntry("'" & thisGroup & "' States", 1, PageSoFar)
        lastGroup = thisGroup
        theDocument.AddTOCEntry(StateArray(PageSoFar - 1), 2, PageSoFar)

        ' ----- See if we have reached the last page.
        If (PageSoFar > StateArray.GetUpperBound(0)) Then e.HasMorePages = False Else _
            e.HasMorePages = True
    End Sub

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' ----- Build an array of state names to be printed later.
        StateArray = Split(stateNames, ",")
    End Sub
End Class
