Imports System

Imports System.Drawing

Imports System.Collections

Imports System.ComponentModel

Imports System.Windows.Forms



Namespace MWA.Progress
    
    
    ''' <summary>
    
    ''' Summary description for ProgressWindow.
    
    ''' </summary>
    
    Public Class ProgressWindow
        Inherits System.Windows.Forms.Form
        Implements IProgressCallback
        
        Private cancelButton As System.Windows.Forms.Button
        Private label As System.Windows.Forms.Label
        
        Private progressBar As System.Windows.Forms.ProgressBar
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.Container = Nothing
        Public Delegate Sub SetTextInvoker(ByVal text As [String])
        Public Delegate Sub IncrementInvoker(ByVal val As Integer)
        Public Delegate Sub StepToInvoker(ByVal val As Integer)
        Public Delegate Sub RangeInvoker(ByVal minimum As Integer, ByVal maximum As Integer)
        Private titleRoot As [String] = ""
        Private initEvent As New System.Threading.ManualResetEvent(False)
        Private abortEvent As New System.Threading.ManualResetEvent(False)
        Private requiresClose As Boolean = True
        Public Sub New()
            '
            ' Required for Windows Form Designer support
            '
            InitializeComponent()
        End Sub
#Region "Implementation of IProgressCallback"
        ''' <summary>
        ''' Call this method from the worker thread to initialize
        ''' the progress meter.
        ''' </summary>
        ''' <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
        ''' <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
        Public Sub Begin(ByVal minimum As Integer, ByVal maximum As Integer) Implements IProgressCallback.Begin
            initEvent.WaitOne()
            Invoke(New RangeInvoker(AddressOf DoBegin), New Object() {minimum, maximum})
        End Sub
        ''' <summary>
        ''' Call this method from the worker thread to initialize
        ''' the progress callback, without setting the range
        ''' </summary>
        Public Sub Begin() Implements IProgressCallback.Begin
            initEvent.WaitOne()
            Invoke(New MethodInvoker(AddressOf DoBegin))
        End Sub
        ''' <summary>
        ''' Call this method from the worker thread to reset the range in the progress callback
        ''' </summary>
        ''' <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
        ''' <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
        ''' <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
        Public Sub SetRange(ByVal minimum As Integer, ByVal maximum As Integer) Implements IProgressCallback.SetRange
            initEvent.WaitOne()
            Invoke(New RangeInvoker(AddressOf DoSetRange), New Object() {minimum, maximum})
        End Sub
        ''' <summary>
        ''' Call this method from the worker thread to update the progress text.
        ''' </summary>
        ''' <param name="text">The progress text to display</param>
        Public Sub SetText(ByVal text As [String]) Implements IProgressCallback.SetText
            Invoke(New SetTextInvoker(AddressOf DoSetText), New Object() {text})
        End Sub
        ''' <summary>
        ''' Call this method from the worker thread to increase the progress counter by a specified value.
        ''' </summary>
        ''' <param name="val">The amount by which to increment the progress indicator</param>
        Public Sub Increment(ByVal val As Integer) Implements IProgressCallback.Increment
            Invoke(New IncrementInvoker(AddressOf DoIncrement), New Object() {val})
        End Sub
        ''' <summary>
        ''' Call this method from the worker thread to step the progress meter to a particular value.
        ''' </summary>
        ''' <param name="val"></param>
        Public Sub StepTo(ByVal val As Integer) Implements IProgressCallback.StepTo
            Invoke(New StepToInvoker(AddressOf DoStepTo), New Object() {val})
        End Sub
        ''' <summary>
        ''' If this property is true, then you should abort work
        ''' </summary>
        Public ReadOnly Property IsAborting() As Boolean Implements IProgressCallback.IsAborting
            Get
                Return abortEvent.WaitOne(0, False)
            End Get
        End Property
        ''' <summary>
        ''' Call this method from the worker thread to finalize the progress meter
        ''' </summary>
        Public Sub [End]() Implements IProgressCallback.End
            If requiresClose Then
                Invoke(New MethodInvoker(AddressOf DoEnd))
            End If
        End Sub
#End Region
#Region "Implementation members invoked on the owner thread"
        Private Sub DoSetText(ByVal text As [String])
            label.Text = text
        End Sub
        Private Sub DoIncrement(ByVal val As Integer)
            progressBar.Increment(val)
            UpdateStatusText()
        End Sub
        Private Sub DoStepTo(ByVal val As Integer)
            progressBar.Value = val
            UpdateStatusText()
        End Sub
        Private Sub DoBegin(ByVal minimum As Integer, ByVal maximum As Integer)
            DoBegin()
            DoSetRange(minimum, maximum)
        End Sub
        Private Sub DoBegin()
            cancelButton.Enabled = True
            ControlBox = True
        End Sub
        Private Sub DoSetRange(ByVal minimum As Integer, ByVal maximum As Integer)
            progressBar.Minimum = minimum
            progressBar.Maximum = maximum
            progressBar.Value = minimum
            titleRoot = Text
        End Sub
        Private Sub DoEnd()
            Close()
        End Sub
#End Region
#Region "Overrides"
        ''' <summary>
        ''' Handles the form load, and sets an event to ensure that
        ''' intialization is synchronized with the appearance of the form.
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overloads Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)
            ControlBox = False
            initEvent.[Set]()
        End Sub
        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If components IsNot Nothing Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub
        ''' <summary>
        ''' Handler for 'Close' clicking
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overloads Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
            requiresClose = False
            AbortWork()
            MyBase.OnClosing(e)
        End Sub
#End Region
#Region "Implementation Utilities"
        ''' <summary>
        ''' Utility function that formats and updates the title bar text
        ''' </summary>
        Private Sub UpdateStatusText()
            Text = titleRoot + [String].Format(" - {0}% complete", (progressBar.Value * 100) / (progressBar.Maximum - progressBar.Minimum))
        End Sub
        ''' <summary>
        ''' Utility function to terminate the thread
        ''' </summary>
        Private Sub AbortWork()
            abortEvent.[Set]()
        End Sub
#End Region
#Region "Windows Form Designer generated code"
        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.progressBar = New System.Windows.Forms.ProgressBar()
            Me.label = New System.Windows.Forms.Label()
            Me.cancelButton = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            ' 
            ' progressBar
            ' 
            Me.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)
            Me.progressBar.Location = New System.Drawing.Point(8, 80)
            Me.progressBar.Name = "progressBar"
            Me.progressBar.Size = New System.Drawing.Size(192, 23)
            Me.progressBar.TabIndex = 1
            ' 
            ' label
            ' 
            Me.label.Anchor = (((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)
            Me.label.Location = New System.Drawing.Point(8, 8)
            Me.label.Name = "label"
            Me.label.Size = New System.Drawing.Size(272, 64)
            Me.label.TabIndex = 0
            Me.label.Text = "Starting operation..."
            ' 
            ' cancelButton
            ' 
            Me.cancelButton.Anchor = (System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right)
            Me.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cancelButton.Enabled = False
            Me.cancelButton.Location = New System.Drawing.Point(208, 80)
            Me.cancelButton.Name = "cancelButton"
            Me.cancelButton.TabIndex = 2
            Me.cancelButton.Text = "Cancel"
            ' 
            ' ProgressWindow
            ' 
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(290, 114)
            Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.cancelButton, Me.progressBar, Me.label})
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.Name = "ProgressWindow"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "ProgressWindow"
            Me.ResumeLayout(False)
        End Sub
#End Region
    End Class
End Namespace


