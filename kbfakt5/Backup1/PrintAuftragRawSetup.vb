Public Class PrintAuftragRawSetup
    Public m_TopOffset As Integer = 10
    Public m_LinesPerPage As Integer = 70
    Public m_LeftMarginChars As Integer = 0
    Public m_InitString As String = ""

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If CheckDaten() Then
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
    Function CheckDaten() As Boolean
        'changed in 2.0.1.1
        If Not mainmodul.CheckTextInt(txtboxLeftMargin) Then
            Return False
        Else
            m_LeftMarginChars = CInt(txtboxLeftMargin.Text)
        End If
        If Not mainmodul.CheckTextInt(txtTopOffset) Then
            Return False
        Else
            m_TopOffset = CInt(txtTopOffset.Text)
        End If
        If Not mainmodul.CheckTextInt(txtPageLength) Then
            Return False
        Else
            m_LinesPerPage = CInt(txtPageLength.Text)
        End If

        If Not (txtInitString.Text.Length Mod 2 = 0) Then
            txtInitString.BackColor = Color.LightPink
            Return False
        Else
            m_InitString = txtInitString.Text
            txtInitString.BackColor = Color.White
        End If
        Return True
        'Dim top As Integer
        'Dim leng As Integer
        'Try
        '    top = CInt(txtTopOffset.Text)
        '    txtTopOffset.BackColor = Color.White
        '    m_TopOffset = top
        'Catch ex As Exception
        '    txtTopOffset.BackColor = Color.Pink
        '    top = -1
        'End Try
        'Try
        '    leng = CInt(txtPageLength.Text)
        '    txtPageLength.BackColor = Color.White
        '    m_LinesPerPage = leng
        'Catch ex As Exception
        '    txtPageLength.BackColor = Color.Pink
        '    leng = -1
        'End Try
        'If leng = -1 Or top = -1 Then
        '    Return False
        'Else
        '    Return True
        'End If
    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub PrintAuftragRawSetup_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtPageLength.Text = m_LinesPerPage.ToString()
        txtTopOffset.Text = m_TopOffset.ToString()
        txtboxLeftMargin.Text = m_LeftMarginChars.ToString()
        txtInitString.Text = m_InitString
    End Sub
End Class