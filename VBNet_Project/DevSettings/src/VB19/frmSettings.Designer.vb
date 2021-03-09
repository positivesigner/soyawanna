<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.grdSetting = New System.Windows.Forms.DataGridView()
        Me.btnSaveAllChanges = New System.Windows.Forms.Button()
        Me.txtNewVal = New System.Windows.Forms.TextBox()
        Me.lblNewVal = New System.Windows.Forms.Label()
        Me.txtUserFile = New System.Windows.Forms.TextBox()
        Me.lblUserFile = New System.Windows.Forms.Label()
        Me.btnAddNewRow = New System.Windows.Forms.Button()
        Me.lblDesignDoc = New System.Windows.Forms.LinkLabel()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnDeleteRow = New System.Windows.Forms.Button()
        Me.lblRowKey = New System.Windows.Forms.Label()
        Me.txtRowKey = New System.Windows.Forms.TextBox()
        Me.btnAuditPaths = New System.Windows.Forms.Button()
        Me.colSetting = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFound = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.grdSetting, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdSetting
        '
        Me.grdSetting.AllowUserToAddRows = False
        Me.grdSetting.AllowUserToDeleteRows = False
        Me.grdSetting.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdSetting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdSetting.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colSetting, Me.colValue, Me.colFound})
        Me.grdSetting.Location = New System.Drawing.Point(0, 133)
        Me.grdSetting.MultiSelect = False
        Me.grdSetting.Name = "grdSetting"
        Me.grdSetting.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.grdSetting.Size = New System.Drawing.Size(800, 317)
        Me.grdSetting.TabIndex = 0
        '
        'btnSaveAllChanges
        '
        Me.btnSaveAllChanges.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveAllChanges.Location = New System.Drawing.Point(244, 28)
        Me.btnSaveAllChanges.Name = "btnSaveAllChanges"
        Me.btnSaveAllChanges.Size = New System.Drawing.Size(108, 29)
        Me.btnSaveAllChanges.TabIndex = 3
        Me.btnSaveAllChanges.Text = "&Save All Changes"
        Me.btnSaveAllChanges.UseVisualStyleBackColor = True
        '
        'txtNewVal
        '
        Me.txtNewVal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNewVal.Location = New System.Drawing.Point(101, 104)
        Me.txtNewVal.Name = "txtNewVal"
        Me.txtNewVal.Size = New System.Drawing.Size(573, 20)
        Me.txtNewVal.TabIndex = 2
        '
        'lblNewVal
        '
        Me.lblNewVal.AutoSize = True
        Me.lblNewVal.Location = New System.Drawing.Point(35, 107)
        Me.lblNewVal.Name = "lblNewVal"
        Me.lblNewVal.Size = New System.Drawing.Size(59, 13)
        Me.lblNewVal.TabIndex = 3
        Me.lblNewVal.Text = "Row Value"
        '
        'txtUserFile
        '
        Me.txtUserFile.Location = New System.Drawing.Point(101, 2)
        Me.txtUserFile.Name = "txtUserFile"
        Me.txtUserFile.ReadOnly = True
        Me.txtUserFile.Size = New System.Drawing.Size(573, 20)
        Me.txtUserFile.TabIndex = 4
        '
        'lblUserFile
        '
        Me.lblUserFile.AutoSize = True
        Me.lblUserFile.Location = New System.Drawing.Point(3, 5)
        Me.lblUserFile.Name = "lblUserFile"
        Me.lblUserFile.Size = New System.Drawing.Size(92, 13)
        Me.lblUserFile.TabIndex = 5
        Me.lblUserFile.Text = "Setttings File Path"
        '
        'btnAddNewRow
        '
        Me.btnAddNewRow.Location = New System.Drawing.Point(101, 69)
        Me.btnAddNewRow.Name = "btnAddNewRow"
        Me.btnAddNewRow.Size = New System.Drawing.Size(108, 29)
        Me.btnAddNewRow.TabIndex = 6
        Me.btnAddNewRow.Text = "&Add New Row"
        Me.btnAddNewRow.UseVisualStyleBackColor = True
        '
        'lblDesignDoc
        '
        Me.lblDesignDoc.AutoSize = True
        Me.lblDesignDoc.Location = New System.Drawing.Point(709, 36)
        Me.lblDesignDoc.Name = "lblDesignDoc"
        Me.lblDesignDoc.Size = New System.Drawing.Size(63, 13)
        Me.lblDesignDoc.TabIndex = 7
        Me.lblDesignDoc.TabStop = True
        Me.lblDesignDoc.Text = "Design Doc"
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(101, 28)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(108, 29)
        Me.btnRefresh.TabIndex = 8
        Me.btnRefresh.Text = "&Refresh Grid"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'btnDeleteRow
        '
        Me.btnDeleteRow.Location = New System.Drawing.Point(244, 69)
        Me.btnDeleteRow.Name = "btnDeleteRow"
        Me.btnDeleteRow.Size = New System.Drawing.Size(108, 29)
        Me.btnDeleteRow.TabIndex = 9
        Me.btnDeleteRow.Text = "&Delete Row"
        Me.btnDeleteRow.UseVisualStyleBackColor = True
        '
        'lblRowKey
        '
        Me.lblRowKey.AutoSize = True
        Me.lblRowKey.Location = New System.Drawing.Point(383, 78)
        Me.lblRowKey.Name = "lblRowKey"
        Me.lblRowKey.Size = New System.Drawing.Size(74, 13)
        Me.lblRowKey.TabIndex = 10
        Me.lblRowKey.Text = "Setting Name:"
        '
        'txtRowKey
        '
        Me.txtRowKey.Location = New System.Drawing.Point(457, 74)
        Me.txtRowKey.Name = "txtRowKey"
        Me.txtRowKey.ReadOnly = True
        Me.txtRowKey.Size = New System.Drawing.Size(217, 20)
        Me.txtRowKey.TabIndex = 11
        '
        'btnAuditPaths
        '
        Me.btnAuditPaths.Location = New System.Drawing.Point(457, 28)
        Me.btnAuditPaths.Name = "btnAuditPaths"
        Me.btnAuditPaths.Size = New System.Drawing.Size(108, 29)
        Me.btnAuditPaths.TabIndex = 12
        Me.btnAuditPaths.Text = "Audit Paths"
        Me.btnAuditPaths.UseVisualStyleBackColor = True
        '
        'colSetting
        '
        Me.colSetting.HeaderText = "Setting"
        Me.colSetting.Name = "colSetting"
        Me.colSetting.ReadOnly = True
        Me.colSetting.Width = 250
        '
        'colValue
        '
        Me.colValue.HeaderText = "Value"
        Me.colValue.Name = "colValue"
        Me.colValue.ReadOnly = True
        Me.colValue.Width = 450
        '
        'colFound
        '
        Me.colFound.HeaderText = "Found"
        Me.colFound.Name = "colFound"
        Me.colFound.ReadOnly = True
        Me.colFound.Visible = False
        Me.colFound.Width = 50
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.btnAuditPaths)
        Me.Controls.Add(Me.txtRowKey)
        Me.Controls.Add(Me.lblRowKey)
        Me.Controls.Add(Me.btnDeleteRow)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.lblDesignDoc)
        Me.Controls.Add(Me.btnAddNewRow)
        Me.Controls.Add(Me.lblUserFile)
        Me.Controls.Add(Me.txtUserFile)
        Me.Controls.Add(Me.lblNewVal)
        Me.Controls.Add(Me.txtNewVal)
        Me.Controls.Add(Me.btnSaveAllChanges)
        Me.Controls.Add(Me.grdSetting)
        Me.Name = "frmSettings"
        Me.Text = "DevSettings"
        CType(Me.grdSetting, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents grdSetting As DataGridView
    Friend WithEvents btnSaveAllChanges As Button
    Friend WithEvents txtNewVal As TextBox
    Friend WithEvents lblNewVal As Label
    Friend WithEvents txtUserFile As TextBox
    Friend WithEvents lblUserFile As Label
    Friend WithEvents btnAddNewRow As Button
    Friend WithEvents lblDesignDoc As LinkLabel
    Friend WithEvents btnRefresh As Button
    Friend WithEvents btnDeleteRow As Button
    Friend WithEvents lblRowKey As Label
    Friend WithEvents txtRowKey As TextBox
    Friend WithEvents btnAuditPaths As Button
    Friend WithEvents colSetting As DataGridViewTextBoxColumn
    Friend WithEvents colValue As DataGridViewTextBoxColumn
    Friend WithEvents colFound As DataGridViewTextBoxColumn
End Class
