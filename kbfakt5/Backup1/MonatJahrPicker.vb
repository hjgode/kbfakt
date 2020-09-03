Public Class MonatJahrPicker
    Public month As Integer
    Public year As Integer
    Private Sub MonatJahrPicker_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Location = New System.Drawing.Point(54, 128)
    End Sub

    Private Sub MonatJahrPicker_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Monat.Value = month
        Jahr.Value = year
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        month = Monat.Value
        year = Jahr.Value
        Me.Close()
    End Sub

    Private Sub btnAbbrechen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbbrechen.Click
        Me.Close()
    End Sub
End Class