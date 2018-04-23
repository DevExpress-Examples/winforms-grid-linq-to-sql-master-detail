using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Linq;
using DevExpress.XtraGrid.Views.Base;

namespace LinqWithEditing {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        NorthwindProductsDataContext dc;

        private void Form1_Load(object sender, EventArgs e) {
            BindGrid();
        }

        private void BindGrid() {
            dc = new NorthwindProductsDataContext();
            Table<Category> categories = dc.GetTable<Category>();
            categoryBindingSource.DataSource = categories;
        }
        private void gridControl1_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e) {
            if(e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.EndEdit) {
                ColumnView view = (ColumnView)gridControl1.FocusedView;
                view.CloseEditor();
                if(view.UpdateCurrentRow()) {
                    try {
                        dc.SubmitChanges(ConflictMode.FailOnFirstConflict);
                    }
                    catch(ChangeConflictException ex) {
                        MessageBox.Show(ex.Message);
                        foreach(ObjectChangeConflict occ in dc.ChangeConflicts) {
                            occ.Resolve(RefreshMode.OverwriteCurrentValues);
                        }
                        //view.RefreshData();
                    }
                    catch(Exception ex) {
                        MessageBox.Show(ex.Message);
                        BindGrid();
                    }
                }
                e.Handled = true;
            }
        }
    }
}
