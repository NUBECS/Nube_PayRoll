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
    /// Interaction logic for frmEmployee.xaml
    /// </summary>
    public partial class frmEmployee : UserControl
    {
        int Id = 0;
        DataTable dtEmployee = new DataTable();
        PayrollEntity db = new PayrollEntity();
        public frmEmployee()
        {
            InitializeComponent();
            LoadWindow();
        }

        #region EVENTS

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWindow();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            frmEmployeeDetails frm = new frmEmployeeDetails(0);
            frm.ShowDialog();
            LoadWindow();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Id != 0)
            {
                frmEmployeeDetails frm = new frmEmployeeDetails(Id);
                frm.ShowDialog();
            }           
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do You want to Delete Employee ?", "Delete Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (Id != 0)
                    {
                        var mb = (from x in db.MasterEmployees where x.Id == Id select x).FirstOrDefault();
                        db.MasterEmployees.Remove(mb);
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

        private void dgEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((dgEmployee.SelectedItem != null))
                {
                    DataRowView drv = (DataRowView)dgEmployee.SelectedItem;
                    Id = Convert.ToInt32(drv["Id"]);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgEmployee_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgEmployee.SelectedItem != null))
                {
                    DataRowView drv = (DataRowView)dgEmployee.SelectedItem;
                    Id = Convert.ToInt32(drv["Id"]);
                    frmEmployeeDetails frm = new frmEmployeeDetails(Id);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
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

        #endregion

        #region FUNCTIONS

        void LoadWindow()
        {
            try
            {
                Id = 0;
                var st = (from x in db.ViewMasterEmployees select x).ToList();
                if (st != null)
                {
                    dtEmployee = AppLib.LINQResultToDataTable(st);
                    dgEmployee.ItemsSource = dtEmployee.DefaultView;
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
                        sWhere = "EMPLOYEENAME LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else if (rptEndWith.IsChecked == true)
                    {
                        sWhere = "EMPLOYEENAME LIKE '%" + txtSearch.Text.ToUpper() + "'";
                    }
                    else if (rptStartWith.IsChecked == true)
                    {
                        sWhere = "EMPLOYEENAME LIKE '" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else
                    {
                        sWhere = "EMPLOYEENAME LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                }
                else
                {
                    if (rptContain.IsChecked == true)
                    {
                        sWhere = "EMPLOYEENAME LIKE '%" + txtSearch.Text + "%'";
                    }
                    else if (rptEndWith.IsChecked == true)
                    {
                        sWhere = "EMPLOYEENAME LIKE '%" + txtSearch.Text + "'";
                    }
                    else if (rptStartWith.IsChecked == true)
                    {
                        sWhere = "EMPLOYEENAME LIKE '" + txtSearch.Text + "%'";
                    }
                    else
                    {
                        sWhere = "EMPLOYEENAME LIKE '%" + txtSearch.Text + "%'";
                    }
                }
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    DataView dv = new DataView(dtEmployee);
                    dv.RowFilter = sWhere;
                    DataTable dtTemp = new DataTable();
                    dtTemp = dv.ToTable();
                    dgEmployee.ItemsSource = dtTemp.DefaultView;
                }
                else
                {
                    dgEmployee.ItemsSource = dtEmployee.DefaultView;
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

