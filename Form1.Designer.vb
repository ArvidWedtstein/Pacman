<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TestForm
    Inherits Appframe3.Win.Forms.afDataFormWithNavGrid

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
        Dim ContextButton1 As DevExpress.Utils.ContextButton = New DevExpress.Utils.ContextButton()
        Me.cGameTimer = New System.Windows.Forms.Timer(Me.components)
        Me.cGameCountLabel = New System.Windows.Forms.Label()
        Me.TileView1 = New DevExpress.XtraGrid.Views.Tile.TileView()
        Me.test = New DevExpress.XtraGrid.Columns.TileViewColumn()
        Me.PacmanDBDataSetBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.PacmanDBDataSet = New App.Test.arwe.PacmanDBDataSet()
        Me.AfGridLookupEditRepositoryItem = New Appframe3.Win.Controls.Utils.afGridLookupEdit.afGridLookupEditRepositoryItem()
        Me.AfGridLookupEditRepositoryItem1View = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.PacmanPlayerName = New Appframe3.Win.Controls.Utils.afGrid.afGridColumn()
        Me.AfGridColumn1 = New Appframe3.Win.Controls.Utils.afGrid.afGridColumn()
        Me.cGridPictureBox = New System.Windows.Forms.PictureBox()
        Me.cGridGroupBox = New System.Windows.Forms.GroupBox()
        Me.cClearButton = New System.Windows.Forms.Button()
        Me.cRunButton = New System.Windows.Forms.Button()
        Me.cEndPosRadioButton = New System.Windows.Forms.RadioButton()
        Me.cStartPosRadioButton = New System.Windows.Forms.RadioButton()
        Me.cWallRadioButton = New System.Windows.Forms.RadioButton()
        CType(Me.SplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer.Panel1.SuspendLayout()
        Me.SplitContainer.SuspendLayout()
        CType(Me.NavGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NavGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TileView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PacmanDBDataSetBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PacmanDBDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AfGridLookupEditRepositoryItem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AfGridLookupEditRepositoryItem1View, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cGridPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cGridGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer
        '
        Me.SplitContainer.Location = New System.Drawing.Point(0, 49)
        '
        'SplitContainer.Panel1
        '
        Me.SplitContainer.Panel1.BackColor = System.Drawing.Color.Black
        Me.SplitContainer.Panel1.Controls.Add(Me.cGridPictureBox)
        Me.SplitContainer.Panel1.Controls.Add(Me.cGameCountLabel)
        Me.SplitContainer.Size = New System.Drawing.Size(1023, 779)
        Me.SplitContainer.SplitterDistance = 845
        '
        'NavGrid
        '
        Me.NavGrid.ColumnFilterBehaviour = Appframe3.Win.Controls.Utils.afGrid.afGridColumnFilterBehaviour.Contains
        Me.NavGrid.IsNavGrid = True
        GridLevelNode1.RelationName = "Level1"
        Me.NavGrid.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
        Me.NavGrid.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.AfGridLookupEditRepositoryItem})
        Me.NavGrid.Size = New System.Drawing.Size(174, 759)
        Me.NavGrid.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.TileView1})
        '
        'NavGridView
        '
        Me.NavGridView.Appearance.ViewCaption.Options.UseTextOptions = True
        Me.NavGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.NavGridView.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.AfGridColumn1, Me.PacmanPlayerName})
        Me.NavGridView.OptionsBehavior.AllowIncrementalSearch = True
        Me.NavGridView.OptionsBehavior.Editable = False
        Me.NavGridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseUp
        Me.NavGridView.OptionsCustomization.AllowGroup = False
        Me.NavGridView.OptionsLayout.Columns.StoreAllOptions = True
        Me.NavGridView.OptionsLayout.Columns.StoreAppearance = True
        Me.NavGridView.OptionsLayout.StoreDataSettings = False
        Me.NavGridView.OptionsMenu.EnableGroupPanelMenu = False
        Me.NavGridView.OptionsNavigation.AutoFocusNewRow = True
        Me.NavGridView.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.NavGridView.OptionsView.ShowAutoFilterRow = True
        Me.NavGridView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
        Me.NavGridView.OptionsView.ShowGroupPanel = False
        '
        'DataNavigator
        '
        Me.DataNavigator.Location = New System.Drawing.Point(0, 828)
        Me.DataNavigator.Size = New System.Drawing.Size(1023, 28)
        '
        'barDockControlTop
        '
        Me.barDockControlTop.Size = New System.Drawing.Size(1023, 49)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 856)
        Me.barDockControlBottom.Size = New System.Drawing.Size(1023, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 49)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 807)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.Location = New System.Drawing.Point(1023, 49)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 807)
        '
        'ToolBar
        '
        Me.ToolBar.OptionsBar.AllowRename = True
        '
        'MainBar
        '
        Me.MainBar.OptionsBar.MultiLine = True
        Me.MainBar.OptionsBar.UseWholeRow = True
        '
        'BarManager
        '
        Me.BarManager.MaxItemId = 2
        '
        'cGameTimer
        '
        Me.cGameTimer.Enabled = True
        Me.cGameTimer.Interval = 1000
        '
        'cGameCountLabel
        '
        Me.cGameCountLabel.AutoSize = True
        Me.cGameCountLabel.ForeColor = System.Drawing.Color.White
        Me.cGameCountLabel.Location = New System.Drawing.Point(3, 3)
        Me.cGameCountLabel.Name = "cGameCountLabel"
        Me.cGameCountLabel.Size = New System.Drawing.Size(46, 13)
        Me.cGameCountLabel.TabIndex = 2
        Me.cGameCountLabel.Text = "Coins: 0"
        '
        'TileView1
        '
        Me.TileView1.Appearance.ViewCaption.Options.UseTextOptions = True
        Me.TileView1.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.TileView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.test})
        ContextButton1.Id = New System.Guid("636fd76a-dbdf-4d27-ab45-8a96e5671729")
        ContextButton1.Name = "ContextButton"
        Me.TileView1.ContextButtons.Add(ContextButton1)
        Me.TileView1.GridControl = Me.NavGrid
        Me.TileView1.Name = "TileView1"
        Me.TileView1.OptionsLayout.Columns.StoreAllOptions = True
        Me.TileView1.OptionsLayout.Columns.StoreAppearance = True
        Me.TileView1.OptionsLayout.StoreDataSettings = False
        Me.TileView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
        '
        'test
        '
        Me.test.Caption = "Test"
        Me.test.Name = "test"
        Me.test.Visible = True
        Me.test.VisibleIndex = 0
        '
        'PacmanDBDataSetBindingSource
        '
        Me.PacmanDBDataSetBindingSource.DataSource = Me.PacmanDBDataSet
        Me.PacmanDBDataSetBindingSource.Position = 0
        '
        'PacmanDBDataSet
        '
        Me.PacmanDBDataSet.DataSetName = "PacmanDBDataSet"
        Me.PacmanDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'AfGridLookupEditRepositoryItem
        '
        Me.AfGridLookupEditRepositoryItem.AutoHeight = False
        Me.AfGridLookupEditRepositoryItem.DataSource = Me.PacmanDBDataSetBindingSource
        Me.AfGridLookupEditRepositoryItem.Name = "AfGridLookupEditRepositoryItem"
        Me.AfGridLookupEditRepositoryItem.NullText = ""
        Me.AfGridLookupEditRepositoryItem.View = Me.AfGridLookupEditRepositoryItem1View
        '
        'AfGridLookupEditRepositoryItem1View
        '
        Me.AfGridLookupEditRepositoryItem1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.AfGridLookupEditRepositoryItem1View.Name = "AfGridLookupEditRepositoryItem1View"
        Me.AfGridLookupEditRepositoryItem1View.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.AfGridLookupEditRepositoryItem1View.OptionsView.ShowGroupPanel = False
        '
        'PacmanPlayerName
        '
        Me.PacmanPlayerName.AppearanceCell.Options.UseTextOptions = True
        Me.PacmanPlayerName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
        Me.PacmanPlayerName.Caption = "AfGridColumn1"
        Me.PacmanPlayerName.ColumnEdit = Me.AfGridLookupEditRepositoryItem
        Me.PacmanPlayerName.FieldName = "PlayerName"
        Me.PacmanPlayerName.Name = "PacmanPlayerName"
        Me.PacmanPlayerName.OptionsFilter.ImmediateUpdateAutoFilter = False
        Me.PacmanPlayerName.OptionsFilter.ImmediateUpdatePopupDateFilterOnCheck = DevExpress.Utils.DefaultBoolean.[False]
        Me.PacmanPlayerName.OptionsFilter.ImmediateUpdatePopupDateFilterOnDateChange = DevExpress.Utils.DefaultBoolean.[False]
        Me.PacmanPlayerName.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom
        Me.PacmanPlayerName.Visible = True
        Me.PacmanPlayerName.VisibleIndex = 1
        '
        'AfGridColumn1
        '
        Me.AfGridColumn1.AppearanceCell.Options.UseTextOptions = True
        Me.AfGridColumn1.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
        Me.AfGridColumn1.Caption = "AfGridColumn1"
        Me.AfGridColumn1.Name = "AfGridColumn1"
        Me.AfGridColumn1.OptionsFilter.ImmediateUpdateAutoFilter = False
        Me.AfGridColumn1.OptionsFilter.ImmediateUpdatePopupDateFilterOnCheck = DevExpress.Utils.DefaultBoolean.[False]
        Me.AfGridColumn1.OptionsFilter.ImmediateUpdatePopupDateFilterOnDateChange = DevExpress.Utils.DefaultBoolean.[False]
        Me.AfGridColumn1.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom
        Me.AfGridColumn1.Visible = True
        Me.AfGridColumn1.VisibleIndex = 0
        '
        'cGridPictureBox
        '
        Me.cGridPictureBox.BackColor = System.Drawing.Color.DimGray
        Me.cGridPictureBox.Location = New System.Drawing.Point(0, 0)
        Me.cGridPictureBox.Name = "cGridPictureBox"
        Me.cGridPictureBox.Size = New System.Drawing.Size(750, 750)
        Me.cGridPictureBox.TabIndex = 3
        Me.cGridPictureBox.TabStop = False
        '
        'cGridGroupBox
        '
        Me.cGridGroupBox.Controls.Add(Me.cClearButton)
        Me.cGridGroupBox.Controls.Add(Me.cRunButton)
        Me.cGridGroupBox.Controls.Add(Me.cEndPosRadioButton)
        Me.cGridGroupBox.Controls.Add(Me.cStartPosRadioButton)
        Me.cGridGroupBox.Controls.Add(Me.cWallRadioButton)
        Me.cGridGroupBox.Location = New System.Drawing.Point(552, 12)
        Me.cGridGroupBox.Name = "cGridGroupBox"
        Me.cGridGroupBox.Size = New System.Drawing.Size(213, 118)
        Me.cGridGroupBox.TabIndex = 8
        Me.cGridGroupBox.TabStop = False
        Me.cGridGroupBox.Text = "GroupBox1"
        '
        'cClearButton
        '
        Me.cClearButton.FlatAppearance.BorderColor = System.Drawing.Color.DarkBlue
        Me.cClearButton.FlatAppearance.BorderSize = 3
        Me.cClearButton.Location = New System.Drawing.Point(132, 77)
        Me.cClearButton.Name = "cClearButton"
        Me.cClearButton.Size = New System.Drawing.Size(75, 23)
        Me.cClearButton.TabIndex = 4
        Me.cClearButton.Text = "Clear"
        Me.cClearButton.UseVisualStyleBackColor = True
        '
        'cRunButton
        '
        Me.cRunButton.Location = New System.Drawing.Point(132, 40)
        Me.cRunButton.Name = "cRunButton"
        Me.cRunButton.Size = New System.Drawing.Size(75, 23)
        Me.cRunButton.TabIndex = 3
        Me.cRunButton.Text = "Run"
        Me.cRunButton.UseVisualStyleBackColor = True
        '
        'cEndPosRadioButton
        '
        Me.cEndPosRadioButton.AutoSize = True
        Me.cEndPosRadioButton.Location = New System.Drawing.Point(47, 80)
        Me.cEndPosRadioButton.Name = "cEndPosRadioButton"
        Me.cEndPosRadioButton.Size = New System.Drawing.Size(60, 17)
        Me.cEndPosRadioButton.TabIndex = 2
        Me.cEndPosRadioButton.TabStop = True
        Me.cEndPosRadioButton.Text = "EndPos"
        Me.cEndPosRadioButton.UseVisualStyleBackColor = True
        '
        'cStartPosRadioButton
        '
        Me.cStartPosRadioButton.AutoSize = True
        Me.cStartPosRadioButton.Location = New System.Drawing.Point(47, 57)
        Me.cStartPosRadioButton.Name = "cStartPosRadioButton"
        Me.cStartPosRadioButton.Size = New System.Drawing.Size(66, 17)
        Me.cStartPosRadioButton.TabIndex = 1
        Me.cStartPosRadioButton.TabStop = True
        Me.cStartPosRadioButton.Text = "StartPos"
        Me.cStartPosRadioButton.UseVisualStyleBackColor = True
        '
        'cWallRadioButton
        '
        Me.cWallRadioButton.AutoSize = True
        Me.cWallRadioButton.Location = New System.Drawing.Point(47, 34)
        Me.cWallRadioButton.Name = "cWallRadioButton"
        Me.cWallRadioButton.Size = New System.Drawing.Size(45, 17)
        Me.cWallRadioButton.TabIndex = 0
        Me.cWallRadioButton.TabStop = True
        Me.cWallRadioButton.Text = "Wall"
        Me.cWallRadioButton.UseVisualStyleBackColor = True
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1023, 856)
        Me.Controls.Add(Me.cGridGroupBox)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.Controls.SetChildIndex(Me.barDockControlTop, 0)
        Me.Controls.SetChildIndex(Me.barDockControlBottom, 0)
        Me.Controls.SetChildIndex(Me.barDockControlRight, 0)
        Me.Controls.SetChildIndex(Me.barDockControlLeft, 0)
        Me.Controls.SetChildIndex(Me.DataNavigator, 0)
        Me.Controls.SetChildIndex(Me.SplitContainer, 0)
        Me.Controls.SetChildIndex(Me.cGridGroupBox, 0)
        Me.SplitContainer.Panel1.ResumeLayout(False)
        Me.SplitContainer.Panel1.PerformLayout()
        CType(Me.SplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer.ResumeLayout(False)
        CType(Me.NavGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NavGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TileView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PacmanDBDataSetBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PacmanDBDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AfGridLookupEditRepositoryItem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AfGridLookupEditRepositoryItem1View, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cGridPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cGridGroupBox.ResumeLayout(False)
        Me.cGridGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents cGameTimer As Timer
    Friend WithEvents cGameCountLabel As Label
    Friend WithEvents TileView1 As DevExpress.XtraGrid.Views.Tile.TileView
    Friend WithEvents test As DevExpress.XtraGrid.Columns.TileViewColumn
    Friend WithEvents PacmanDBDataSetBindingSource As BindingSource
    Friend WithEvents PacmanDBDataSet As PacmanDBDataSet
    Friend WithEvents AfGridLookupEditRepositoryItem As Controls.Utils.afGridLookupEdit.afGridLookupEditRepositoryItem
    Friend WithEvents AfGridLookupEditRepositoryItem1View As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents PacmanPlayerName As Controls.Utils.afGrid.afGridColumn
    Friend WithEvents AfGridColumn1 As Controls.Utils.afGrid.afGridColumn
    Friend WithEvents cGridPictureBox As PictureBox
    Friend WithEvents cGridGroupBox As GroupBox
    Friend WithEvents cClearButton As Button
    Friend WithEvents cRunButton As Button
    Friend WithEvents cEndPosRadioButton As RadioButton
    Friend WithEvents cStartPosRadioButton As RadioButton
    Friend WithEvents cWallRadioButton As RadioButton
End Class
