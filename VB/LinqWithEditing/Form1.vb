Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Data.Linq
Imports DevExpress.XtraGrid.Views.Base

Namespace LinqWithEditing
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private dc As NorthwindProductsDataContext

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			BindGrid()
		End Sub

		Private Sub BindGrid()
			dc = New NorthwindProductsDataContext()
			Dim categories As Table(Of Category) = dc.GetTable(Of Category)()
			categoryBindingSource.DataSource = categories
		End Sub
		Private Sub gridControl1_EmbeddedNavigator_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.NavigatorButtonClickEventArgs) Handles gridControl1.EmbeddedNavigator.ButtonClick
			If e.Button.ButtonType = DevExpress.XtraEditors.NavigatorButtonType.EndEdit Then
				Dim view As ColumnView = CType(gridControl1.FocusedView, ColumnView)
				view.CloseEditor()
				If view.UpdateCurrentRow() Then
					Try
						dc.SubmitChanges(ConflictMode.FailOnFirstConflict)
					Catch ex As ChangeConflictException
						MessageBox.Show(ex.Message)
						For Each occ As ObjectChangeConflict In dc.ChangeConflicts
							occ.Resolve(RefreshMode.OverwriteCurrentValues)
						Next occ
						'view.RefreshData();
					Catch ex As Exception
						MessageBox.Show(ex.Message)
						BindGrid()
					End Try
				End If
				e.Handled = True
			End If
		End Sub
	End Class
End Namespace
