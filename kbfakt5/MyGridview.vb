
Public Class DataGridViewEnter1
    Inherits DataGridView
    'This override causes the DataGridView to use the enter key in a similar way as
    'the tab key


    Protected Overrides Function ProcessDialogKey(ByVal keyData As Keys) As Boolean
        Dim key As Keys = (keyData And Keys.KeyCode)

        If key = Keys.Enter Then
            Return Me.ProcessRightKey(keyData)
        End If
        Return MyBase.ProcessDialogKey(keyData)
    End Function


    Public Shadows Function ProcessRightKey(ByVal keyData As Keys) As Boolean
        Dim v As Object
        v = MyBase.CurrentCell.Value
        If IsDBNull(MyBase.CurrentCell.Value) Then
            MyBase.CurrentCell.Value = 0
        End If
        Dim taste As Keys = (keyData And Keys.KeyCode)
        'to have this working correctly, the datagridview msut have AllowUsersToAddRows=False!
        If taste = Keys.Enter Then
            'is this the last column in the last row?
            If (MyBase.CurrentCell.ColumnIndex = (MyBase.ColumnCount - 1)) _
            And (MyBase.CurrentCell.RowIndex = (MyBase.RowCount - 1)) Then
                'This causes the last cell to be checked for errors
                MyBase.EndEdit()
                '((BindingSource)base.DataSource).AddNew();
                Dim r As DataRowView = MyBase.DataSource.AddNew()
                Dim rx As DataGridViewRow = MyBase.Rows(MyBase.RowCount - 1)
                With rx
                    .Cells("POS").Value = MyBase.RowCount ' + 1 removed: corrected in version 2.0.1.7
                    .Cells("MENGE").Value = "1"
                    .Cells("E_PREIS").Value = "0"
                    .Cells("RABATT").Value = "0"
                    .Cells("GESAMT").Value = "0"
                    .Cells("Art-Typ").Value = 0
                End With
                'move selection to 'ArtikelNR'
                MyBase.CurrentCell = MyBase.Rows(MyBase.RowCount - 1).Cells(1)
                Return True
            End If

            'is this the last column and not a new row?
            If (MyBase.CurrentCell.ColumnIndex = (MyBase.ColumnCount - 1)) _
            And (MyBase.CurrentCell.RowIndex + 1 <> MyBase.NewRowIndex) Then
                MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex + 1).Cells(0)
                Return True
            End If
            Return MyBase.ProcessRightKey(keyData)
        End If
        Return MyBase.ProcessRightKey(keyData)
    End Function


    Protected Overrides Function ProcessDataGridViewKey(ByVal e As KeyEventArgs) As Boolean
        System.Diagnostics.Debug.WriteLine("Processing ProcessDataGridViewKey")
        'If Not (MyBase.EditMode = DataGridViewEditMode.EditOnEnter) Then
        If e.KeyCode = Keys.Enter Then
            Return Me.ProcessRightKey(e.KeyData)
        End If
        Return MyBase.ProcessDataGridViewKey(e)
        'End If
    End Function


End Class
'Private Sub MyGrid_EditingControlShowing(ByVal sender As Object, ByVal e As DataGridViewEditingControlShowingEventArgs)
'    If TypeOf e.Control Is DataGridViewTextBoxEditingControl Then
'        e.Control.KeyPress += New KeyPressEventHandler(Control_KeyPress)
'    End If
'End Sub



'Private Sub Control_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
'    e.KeyChar = e.KeyChar.ToString().ToUpper()(0)
'End Sub

'----------------------------------------------------------------
' Converted from C# to VB .NET using CSharpToVBConverter(1.2).
' Developed by: Kamal Patel (http://www.KamalPatel.net) 
'----------------------------------------------------------------

'{
'       protected override bool ProcessDialogKey(Keys keyData)
'       {
'           Keys key = (keyData & Keys.KeyCode);
'           if (key == Keys.Enter)
'           {
'               return this.ProcessRightKey(keyData);
'           }
'           return base.ProcessDialogKey(keyData);
'       }

'      public new bool ProcessRightKey(Keys keyData)
'       {
'           Keys key = (keyData & Keys.KeyCode);
'           if (key == Keys.Enter)
'           {
'               if ((base.CurrentCell.ColumnIndex == (base.ColumnCount - 1)) && (base.CurrentCell.RowIndex == (base.RowCount - 1)))
'               {
'                   ((BindingSource)base.DataSource).AddNew();
'                   base.CurrentCell = base.Rows[base.RowCount - 1].Cells[0];
'                   return true;
'               }

'               if ((base.CurrentCell.ColumnIndex == (base.ColumnCount - 1)) && (base.CurrentCell.RowIndex + 1 != base.NewRowIndex))
'               {
'                   base.CurrentCell = base.Rows[base.CurrentCell.RowIndex + 1].Cells[0];
'                   return true;
'               }
'               return base.ProcessRightKey(keyData);
'           }
'           return base.ProcessRightKey(keyData);
'       }

'       protected override bool ProcessDataGridViewKey(KeyEventArgs e)
'       {
'           if (e.KeyCode == Keys.Enter)
'           {
'               return this.ProcessRightKey(e.KeyData);
'           }
'           return base.ProcessDataGridViewKey(e);
'       }


