Namespace Mx

    Public Class dbConfigInput
        Private objAPPCONFIG As Mx2.Mock.NameValueCollection

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New(ur_appconfig As Mx2.Mock.NameValueCollection)
            Me.objAPPCONFIG = ur_appconfig
        End Sub 'New

        Public ReadOnly Property Keys As componentKeysCollection
            <System.Diagnostics.DebuggerHidden()>
            Get
                Keys = Nothing
                If Me.objAPPCONFIG IsNot Nothing Then
                    Keys = New componentKeysCollection(Me.objAPPCONFIG.Keys)
                End If
            End Get
        End Property 'Keys

        Public ReadOnly Property Item(name As String) As String
            <System.Diagnostics.DebuggerHidden()>
            Get
                Item = Nothing
                If Me.objAPPCONFIG IsNot Nothing Then
                    Item = Me.objAPPCONFIG.Item(name)
                End If
            End Get
        End Property 'Item

        <System.Diagnostics.DebuggerHidden()>
        Public Shared Widening Operator CType(b As Mx2.Mock.NameValueCollection) As Mx.dbConfigInput
            Return New Mx.dbConfigInput(b)
        End Operator

        Public Class componentKeysCollection
            Private objKEYS As Mx2.Mock.NameObjectCollectionBase.KeysCollection

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As Mx2.Mock.NameObjectCollectionBase.KeysCollection)
                Me.objKEYS = ur_component
            End Sub

            Public ReadOnly Property Count As Integer
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Count = Nothing
                    If Me.objKEYS IsNot Nothing Then
                        Count = Me.objKEYS.Count
                    End If
                End Get
            End Property 'Keys

            Public ReadOnly Property Item(index As Integer) As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Item = Nothing
                    If Me.objKEYS IsNot Nothing Then
                        Item = Me.objKEYS.Item(index)
                    End If
                End Get
            End Property 'Item
        End Class 'componentKeysCollection
    End Class 'dbConfigInput

    Public Class dbUserInput
        Public Class ztxtNewVal
            Inherits componentTextBox
            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As Mx2.Mock.TextBox)
                Call MyBase.New(ur_component)
            End Sub
        End Class 'ztxtNewVal

        Public Class ztxtRowKey
            Inherits componentTextBox
            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As Mx2.Mock.TextBox)
                Call MyBase.New(ur_component)
            End Sub
        End Class 'ztxtRowKey

        Public Class ztxtUserFile
            Inherits componentTextBox
            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As Mx2.Mock.TextBox)
                Call MyBase.New(ur_component)
            End Sub
        End Class 'ztxtUserFile

        Private objINPUT_FORM As Mx2.Mock.Form
        Public grdSetting As componentDataGridView
        Public btnSaveAllChanges As componentButton
        Public txtRowKey As ztxtRowKey
        Public txtNewVal As ztxtNewVal
        Public txtUserFile As ztxtUserFile

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New(ur_input_form As Mx2.Mock.Form, ur_main_grid As Mx2.Mock.DataGridView, ur_saveall_btn As Mx2.Mock.Button, ur_row_key As Mx2.Mock.TextBox, ur_new_val As Mx2.Mock.TextBox, ur_user_file As Mx2.Mock.TextBox)
            Me.objINPUT_FORM = ur_input_form
            Me.grdSetting = New componentDataGridView(ur_main_grid)
            Me.btnSaveAllChanges = New componentButton(ur_saveall_btn)
            Me.txtRowKey = New ztxtRowKey(ur_row_key)
            Me.txtNewVal = New ztxtNewVal(ur_new_val)
            Me.txtUserFile = New ztxtUserFile(ur_user_file)
        End Sub 'New

        Public Class componentDataGridView
            Private objDATA_GRID_VIEW As Mx2.Mock.DataGridView

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As Mx2.Mock.DataGridView)
                Me.objDATA_GRID_VIEW = ur_component
            End Sub

            Public ReadOnly Property Columns As componentDataGridViewColumnCollection
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Columns = Nothing
                    If Me.objDATA_GRID_VIEW IsNot Nothing Then
                        Columns = New componentDataGridViewColumnCollection(Me.objDATA_GRID_VIEW.Columns)
                    End If
                End Get
            End Property 'Columns

            Public ReadOnly Property CurrentRow As componentDataGridViewRow
                <System.Diagnostics.DebuggerHidden()>
                Get
                    CurrentRow = Nothing
                    If Me.objDATA_GRID_VIEW IsNot Nothing Then
                        CurrentRow = New componentDataGridViewRow(Me.objDATA_GRID_VIEW.CurrentRow)
                    End If
                End Get
            End Property 'Rows

            Public ReadOnly Property Rows As componentDataGridViewRowCollection
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Rows = Nothing
                    If Me.objDATA_GRID_VIEW IsNot Nothing Then
                        Rows = New componentDataGridViewRowCollection(Me.objDATA_GRID_VIEW.Rows)
                    End If
                End Get
            End Property 'Rows

            <System.Diagnostics.DebuggerHidden()>
            Public Sub [Select]()
                If Me.objDATA_GRID_VIEW IsNot Nothing Then
                    Call Me.objDATA_GRID_VIEW.Select()
                End If
            End Sub 'Select

            Public Property CurrentCell As componentDataGridViewCell
                <System.Diagnostics.DebuggerHidden()>
                Get
                    CurrentCell = Nothing
                    If Me.objDATA_GRID_VIEW IsNot Nothing Then
                        CurrentCell = New componentDataGridViewCell(Me.objDATA_GRID_VIEW.CurrentCell)
                    End If
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As componentDataGridViewCell)
                    If Me.objDATA_GRID_VIEW IsNot Nothing Then
                        Me.objDATA_GRID_VIEW.CurrentCell = value.objCUR_CELL
                    End If
                End Set
            End Property 'Text
        End Class 'componentDataGridView

        Public Class componentDataGridViewColumnCollection
            Private objDATA_GRID_COLS As Mx2.Mock.DataGridViewColumnCollection

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As Mx2.Mock.DataGridViewColumnCollection)
                Me.objDATA_GRID_COLS = ur_component
            End Sub 'New

            Public ReadOnly Property Count As Integer
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Count = 0
                    If Me.objDATA_GRID_COLS IsNot Nothing Then
                        Count = Me.objDATA_GRID_COLS.Count
                    End If
                End Get
            End Property 'Rows

            Default Public ReadOnly Property Item(index As Integer) As componentDataGridViewColumn
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Item = Nothing
                    If Me.objDATA_GRID_COLS IsNot Nothing Then
                        Item = New componentDataGridViewColumn(Me.objDATA_GRID_COLS.Item(index))
                    End If
                End Get
            End Property 'Item
        End Class 'componentDataGridViewColumnCollection

        Public Class componentDataGridViewRowCollection
            Private objDATA_GRID_ROWS As Mx2.Mock.DataGridViewRowCollection

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As Mx2.Mock.DataGridViewRowCollection)
                Me.objDATA_GRID_ROWS = ur_component
            End Sub 'New

            <System.Diagnostics.DebuggerHidden()>
            Public Function Add() As Integer
                Add = 0
                If Me.objDATA_GRID_ROWS IsNot Nothing Then
                    Add = Me.objDATA_GRID_ROWS.Add()
                End If
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Sub Clear()
                If Me.objDATA_GRID_ROWS IsNot Nothing Then
                    Call Me.objDATA_GRID_ROWS.Clear()
                End If
            End Sub 'Clear

            Public ReadOnly Property Count As Integer
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Count = 0
                    If Me.objDATA_GRID_ROWS IsNot Nothing Then
                        Count = Me.objDATA_GRID_ROWS.Count
                    End If
                End Get
            End Property 'Rows

            Default Public ReadOnly Property Item(index As Integer) As componentDataGridViewRow
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Item = Nothing
                    If Me.objDATA_GRID_ROWS IsNot Nothing Then
                        Item = New componentDataGridViewRow(Me.objDATA_GRID_ROWS.Item(index))
                    End If
                End Get
            End Property 'Item
        End Class 'componentDataGridRows

        Public Class componentDataGridViewColumn
            Private objGRID_COL As Mx2.Mock.DataGridViewColumn

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As Mx2.Mock.DataGridViewColumn)
                Me.objGRID_COL = ur_component
            End Sub

            Public ReadOnly Property Index() As Integer
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Index = Nothing
                    If Me.objGRID_COL IsNot Nothing Then
                        Index = Me.objGRID_COL.Index
                    End If
                End Get
            End Property 'Index

            Public ReadOnly Property Name() As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Name = Nothing
                    If Me.objGRID_COL IsNot Nothing Then
                        Name = Me.objGRID_COL.Name
                    End If
                End Get
            End Property 'Name

            Public Property Visible() As Boolean
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Visible = False
                    If Me.objGRID_COL IsNot Nothing Then
                        Visible = Me.objGRID_COL.Visible
                    End If
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As Boolean)
                    If Me.objGRID_COL IsNot Nothing Then
                        Me.objGRID_COL.Visible = value
                    End If
                End Set
            End Property 'Index
        End Class 'ccomponentDataGridViewColumn

        Public Class componentDataGridViewRow
            Private objGRID_ROW As Mx2.Mock.DataGridViewRow

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As Mx2.Mock.DataGridViewRow)
                Me.objGRID_ROW = ur_component
            End Sub

            Public ReadOnly Property Cells() As componentDataGridViewCellCollection
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Cells = Nothing
                    If Me.objGRID_ROW IsNot Nothing Then
                        Cells = New componentDataGridViewCellCollection(Me.objGRID_ROW.Cells)
                    End If
                End Get
            End Property 'Cells
        End Class 'componentDataGridViewRow

        Public Class componentDataGridViewCellCollection
            Private objGRID_CELLS As Mx2.Mock.DataGridViewCellCollection

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As Mx2.Mock.DataGridViewCellCollection)
                Me.objGRID_CELLS = ur_component
            End Sub

            Default Public ReadOnly Property Item(index As Integer) As componentDataGridViewCell
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Item = Nothing
                    If Me.objGRID_CELLS IsNot Nothing Then
                        Item = New componentDataGridViewCell(Me.objGRID_CELLS.Item(index))
                    End If
                End Get
            End Property 'Item
        End Class 'componentDataGridViewCellCollection

        Public Class componentDataGridViewCell
            Public objCUR_CELL As Mx2.Mock.DataGridViewCell

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As Mx2.Mock.DataGridViewCell)
                Me.objCUR_CELL = ur_component
            End Sub

            Public ReadOnly Property Style As componentDataGridViewCellStyle
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Style = Nothing
                    If Me.objCUR_CELL IsNot Nothing Then
                        Style = New componentDataGridViewCellStyle(Me.objCUR_CELL.Style)
                    End If
                End Get
            End Property 'BackgroundColor

            Public Property Value As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Value = Nothing
                    If Me.objCUR_CELL IsNot Nothing Then
                        Value = CType(Me.objCUR_CELL.Value, String)
                    End If
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    If Me.objCUR_CELL IsNot Nothing Then
                        Me.objCUR_CELL.Value = value
                    End If
                End Set
            End Property 'Value
        End Class 'componentDataGridViewCell

        Public Class componentDataGridViewCellStyle
            Public objCUR_STYLE As Mx2.Mock.DataGridViewCellStyle

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As Mx2.Mock.DataGridViewCellStyle)
                Me.objCUR_STYLE = ur_component
            End Sub

            Public Property BackColor As System.Drawing.Color
                <System.Diagnostics.DebuggerHidden()>
                Get
                    BackColor = System.Drawing.Color.Empty
                    If Me.objCUR_STYLE IsNot Nothing Then
                        BackColor = Me.objCUR_STYLE.BackColor
                    End If
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As System.Drawing.Color)
                    If Me.objCUR_STYLE IsNot Nothing Then
                        Me.objCUR_STYLE.BackColor = value
                    End If
                End Set
            End Property 'BackgroundColor
        End Class 'componentDataGridViewCellStyle

        Public Class componentButton
            Private objBUTTON As Mx2.Mock.Button

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As Mx2.Mock.Button)
                Me.objBUTTON = ur_component
            End Sub

            Public Property Enabled As Boolean
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Enabled = False
                    If Me.objBUTTON IsNot Nothing Then
                        Enabled = Me.objBUTTON.Enabled
                    End If
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As Boolean)
                    If Me.objBUTTON IsNot Nothing Then
                        Me.objBUTTON.Enabled = value
                    End If
                End Set
            End Property 'Enabled
        End Class 'componentButton

        Public Class componentTextBox
            Private objTEXT_BOX As Mx2.Mock.TextBox

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_component As Mx2.Mock.TextBox)
                Me.objTEXT_BOX = ur_component
            End Sub

            Public Property Enabled As Boolean
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Enabled = False
                    If Me.objTEXT_BOX IsNot Nothing Then
                        Enabled = Me.objTEXT_BOX.Enabled
                    End If
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As Boolean)
                    If Me.objTEXT_BOX IsNot Nothing Then
                        Me.objTEXT_BOX.Enabled = value
                    End If
                End Set
            End Property 'Enabled

            Public Property Text As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Text = mt
                    If Me.objTEXT_BOX IsNot Nothing Then
                        Text = Me.objTEXT_BOX.Text
                    End If
                End Get
                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    If Me.objTEXT_BOX IsNot Nothing Then
                        Me.objTEXT_BOX.Text = value
                    End If
                End Set
            End Property 'Text
        End Class 'componentTextBox
    End Class 'dbUserInput
End Namespace 'Mx