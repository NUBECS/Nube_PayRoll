using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NUBE.PAYROLL.CMN;
using System.Data;

namespace NUBE.PAYROLL.PL.Master
{
    /// <summary>
    /// Interaction logic for frmMasterNubeBranch.xaml
    /// </summary>
    public partial class frmMasterNubeBranch : UserControl
    {
        int Id = 0;
        PayrollEntity db = new PayrollEntity();
        DataTable dtNUBE = new DataTable();
        public frmMasterNubeBranch()
        {
            InitializeComponent();
        }

        #region EVENTS

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWindow();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Id != 0)
                {
                    var mb = (from x in db.MasterNubeBranches where x.Id == Id select x).FirstOrDefault();
                    mb.NubeBranchName = txtNubeBranchName.Text;
                    mb.UserCode = txtNubeUserCode.Text;
                    db.SaveChanges();
                    MessageBox.Show("Updated Sucessfully!");
                    LoadWindow();
                }
                else
                {
                    MasterNubeBranch mb = new MasterNubeBranch();
                    mb.NubeBranchName = txtNubeBranchName.Text;
                    mb.UserCode = txtNubeUserCode.Text;
                    db.MasterNubeBranches.Add(mb);
                    db.SaveChanges();
                    MessageBox.Show("Saved Sucessfully!");
                    LoadWindow();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Id != 0)
                {
                    var mb = (from x in db.MasterNubeBranches where x.Id == Id select x).FirstOrDefault();
                    db.MasterNubeBranches.Remove(mb);
                    db.SaveChanges();
                    MessageBox.Show("Deleted Sucessfully");
                    LoadWindow();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            LoadWindow();
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filteration();
        }

        private void cbxCase_Checked(object sender, RoutedEventArgs e)
        {
            Filteration();
        }

        private void cbxCase_Unchecked(object sender, RoutedEventArgs e)
        {
            Filteration();
        }

        private void rptStartWith_Checked(object sender, RoutedEventArgs e)
        {
            Filteration();
        }

        private void rptContain_Checked(object sender, RoutedEventArgs e)
        {
            Filteration();
        }

        private void rptEndWith_Checked(object sender, RoutedEventArgs e)
        {
            Filteration();
        }

        private void dgvNubeBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Id = 0;
                txtNubeBranchName.Text = "";
                txtNubeUserCode.Text = "";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgvNubeBranch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgvNubeBranch.SelectedItem != null))
                {
                    DataRowView drv = (DataRowView)dgvNubeBranch.SelectedItem;
                    Id = Convert.ToInt32(drv["Id"]);
                    txtNubeBranchName.Text = drv["NubeBranchName"].ToString();
                    txtNubeUserCode.Text = drv["UserCode"].ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        #endregion

        #region FUNCTIONS

        void LoadWindow()
        {
            Id = 0;
            txtNubeBranchName.Text = "";
            txtNubeUserCode.Text = "";
            try
            {
                var nube = (from x in db.MasterNubeBranches select x).ToList();
                if (nube != null)
                {
                    dtNUBE = AppLib.LINQResultToDataTable(nube);
                    dgvNubeBranch.ItemsSource = dtNUBE.DefaultView;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        void Filteration()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    string sWhere = "";

                    if (cbxCase.IsChecked == true)
                    {
                        if (rptContain.IsChecked == true)
                        {
                            sWhere = "NubeBranchName LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                        }
                        else if (rptEndWith.IsChecked == true)
                        {
                            sWhere = "NubeBranchName LIKE '%" + txtSearch.Text.ToUpper() + "'";
                        }
                        else if (rptStartWith.IsChecked == true)
                        {
                            sWhere = "NubeBranchName LIKE '" + txtSearch.Text.ToUpper() + "%'";
                        }
                        else
                        {
                            sWhere = "NubeBranchName LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                        }
                    }
                    else
                    {
                        if (rptContain.IsChecked == true)
                        {
                            sWhere = "NubeBranchName LIKE '%" + txtSearch.Text + "%'";
                        }
                        else if (rptEndWith.IsChecked == true)
                        {
                            sWhere = "NubeBranchName LIKE '%" + txtSearch.Text + "'";
                        }
                        else if (rptStartWith.IsChecked == true)
                        {
                            sWhere = "NubeBranchName LIKE '" + txtSearch.Text + "%'";
                        }
                        else
                        {
                            sWhere = "NubeBranchName LIKE '%" + txtSearch.Text + "%'";
                        }
                    }
                    if (!string.IsNullOrEmpty(txtSearch.Text))
                    {
                        DataView dv = new DataView(dtNUBE);
                        dv.RowFilter = sWhere;
                        DataTable dtTemp = new DataTable();
                        dtTemp = dv.ToTable();
                        dgvNubeBranch.ItemsSource = dtTemp.DefaultView;
                    }
                    else
                    {
                        dgvNubeBranch.ItemsSource = dtNUBE.DefaultView;
                    }
                }
                else
                {
                    dgvNubeBranch.ItemsSource = dtNUBE.DefaultView;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        #endregion
    }
}
