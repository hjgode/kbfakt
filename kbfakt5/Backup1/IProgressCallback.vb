Imports System



Namespace MWA.Progress
    
    
    ''' <summary>
    
    ''' This defines an interface which can be implemented by UI elements
    
    ''' which indicate the progress of a long operation.
    
    ''' (See ProgressWindow for a typical implementation)
    
    ''' </summary>
    
    Public Interface IProgressCallback
        
        
        ''' <summary>
        
        ''' Call this method from the worker thread to initialize
        
        ''' the progress callback.
        
        ''' </summary>
        
        ''' <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
        
        ''' <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
        
        Sub Begin(ByVal minimum As Integer, ByVal maximum As Integer)
        
        
        
        ''' <summary>
        
        ''' Call this method from the worker thread to initialize
        
        ''' the progress callback, without setting the range
        
        ''' </summary>
        
        Sub Begin()
        
        
        
        ''' <summary>
        
        ''' Call this method from the worker thread to reset the range in the progress callback
        
        ''' </summary>
        
        ''' <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
        
        ''' <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
        
        ''' <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
        
        Sub SetRange(ByVal minimum As Integer, ByVal maximum As Integer)
        
        
        
        ''' <summary>
        
        ''' Call this method from the worker thread to update the progress text.
        
        ''' </summary>
        
        ''' <param name="text">The progress text to display</param>
        
        ''' <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
        
        Sub SetText(ByVal text As [String])
        
        
        
        ''' <summary>
        
        ''' Call this method from the worker thread to increase the progress counter by a specified value.
        
        ''' </summary>
        
        ''' <param name="val">The amount by which to increment the progress indicator</param>
        
        ''' <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
        
        Sub StepTo(ByVal val As Integer)
        
        
        
        ''' <summary>
        
        ''' Call this method from the worker thread to step the progress meter to a particular value.
        
        ''' </summary>
        
        ''' <param name="val">The value to which to step the meter</param>
        
        ''' <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
        
        Sub Increment(ByVal val As Integer)
        
        
        
        ''' <summary>
        
        ''' If this property is true, then you should abort work
        
        ''' </summary>
        
        ''' <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
        
        ReadOnly Property IsAborting() As Boolean
        
        
        
        
        
        
        ''' <summary>
        
        ''' Call this method from the worker thread to finalize the progress meter
        
        ''' </summary>
        
        ''' <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
        
        Sub [End]()
        
    End Interface
    
End Namespace


