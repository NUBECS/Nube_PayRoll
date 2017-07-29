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
    /// Interaction logic for frmPosition.xaml
    /// </summary>
    public partial class frmPosition : UserControl
    {
        int Id = 0;
        DataTable dtMasterPosition = new DataTable();
        PayrollEntity db = new PayrollEntity();
        public frmPosition()
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
                if (MessageBox.Show("Do You want to Save Position ?", "Save Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (Id != 0)
                    {
                        var mb = (from x in db.MasterPositions where x.Id == Id select x).FirstOrDefault();
                        mb.PositionName = txtPositionName.Text;
                        mb.ShortName = txtPositionUserCode.Text;
                        db.SaveChanges();
                        MessageBox.Show("Updated Sucessfully!");
                        LoadWindow();
                    }
                    else
                    {
                        MasterPosition mb = new MasterPosition();
                        mb.PositionName = txtPositionName.Text;
                        mb.ShortName = txtPositionUserCode.Text;
                        db.MasterPositions.Add(mb);
                        db.SaveChanges();
                        MessageBox.Show("Saved Sucessfully!");
                        LoadWindow();
                    }
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
                if (MessageBox.Show("Do You want to Delete Position ?", "Delete Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (Id != 0)
                    {
                        var mb = (from x in db.MasterPositions where x.Id == Id select x).FirstOrDefault();
                        db.MasterPositions.Remove(mb);
                        db.SaveChanges();
                        MessageBox.Show("Deleted Sucessfully");
                        LoadWindow();
                    }
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

        private void dgvPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Id = 0;
            txtPositionName.Text = "";
            txtPositionUserCode.Text = "";
        }

        private void dgvPosition_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgvPosition.SelectedItem != null))
                {
                    DataRowView drv = (DataRowView)dgvPosition.SelectedItem;
                    Id = Convert.ToInt32(drv["Id"]);
                    txtPositionName.Text = drv["PositionName"].ToString();
                    txtPositionUserCode.Text = drv["ShortName"].ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        #endregion

        #region FUNCITONS

        void LoadWindow()
        {
            Id = 0;
            txtPositionName.Text = "";
            txtPositionUserCode.Text = "";
            try
            {
                var pos = (from x in db.MasterPositions select x).ToList();
                if (pos != null)
                {
                    dtMasterPosition = AppLib.LINQResultToDataTable(pos);
                    dgvPosition.ItemsSource = dtMasterPosition.DefaultView;
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
                string sWhere = "";

                if (cbxCase.IsChecked == true)
                {
                    if (rptContain.IsChecked == true)
                    {
                        sWhere = "PositionName LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else if (rptEndWith.IsChecked == true)
                    {
                        sWhere = "PositionName LIKE '%" + txtSearch.Text.ToUpper() + "'";
                    }
                    else if (rptStartWith.IsChecked == true)
                    {
                        sWhere = "PositionName LIKE '" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else
                    {
                        sWhere = "PositionName LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                }
                else
                {
                    if (rptContain.IsChecked == true)
                    {
                        sWhere = "PositionName LIKE '%" + txtSearch.Text + "%'";
                    }
                    else if (rptEndWith.IsChecked == true)
                    {
                        sWhere = "PositionName LIKE '%" + txtSearch.Text + "'";
                    }
                    else if (rptStartWith.IsChecked == true)
                    {
                        sWhere = "PositionName LIKE '" + txtSearch.Text + "%'";
                    }
                    else
                    {
                        sWhere = "PositionName LIKE '%" + txtSearch.Text + "%'";
                    }
                }
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    DataView dv = new DataView(dtMasterPosition);
                    dv.RowFilter = sWhere;
                    DataTable dtTemp = new DataTable();
                    dtTemp = dv.ToTable();
                    dgvPosition.ItemsSource = dtTemp.DefaultView;
                }
                else
                {
                    dgvPosition.ItemsSource = dtMasterPosition.DefaultView;
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
