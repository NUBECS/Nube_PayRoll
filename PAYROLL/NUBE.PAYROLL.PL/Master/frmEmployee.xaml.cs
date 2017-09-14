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
using System.Data.SqlClient;

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
                if (Id != 0)
                {
                    if (MessageBox.Show("Do you want to Delete ?", "Delete Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {

                        var mb = (from x in db.MasterEmployees where x.Id == Id select x).FirstOrDefault();
                        mb.IsCancel = true;
                        mb.CancelOn = DateTime.Now;                      
                        db.SaveChanges();
                        MessageBox.Show("Deleted Sucessfully");
                        LoadWindow();
                    }
                }
            }
            catch (Exception ex)
            {
                //ExceptionLogging.SendErrorToText(ex);
                MessageBox.Show(ex.Message, "You Can't Delete");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
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
                    LoadWindow();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void chkIsResigned_Click(object sender, RoutedEventArgs e)
        {
            Filteration();
        }

        private void txtSearch_PreviewKeyUp(object sender, KeyEventArgs e)
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
                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    string str = " SELECT ROW_NUMBER() OVER(ORDER BY EM.MEMBERSHIPNO ASC) AS RNO,EM.ID,EM.MEMBERSHIPNO,\r" +
                                 " EM.EMPLOYEENAME,EM.ISRESIGNED RESIGNED,EM.NRIC,MP.POSITIONNAME,EM.GENDER \r" +
                                 " FROM MASTEREMPLOYEE EM(NOLOCK) \r " +
                                 " LEFT JOIN MASTERPOSITION MP(NOLOCK)ON MP.ID=EM.POSITIONID \r" +
                                 " WHERE EM.ISCANCEL=0 \r" +
                                 " GROUP BY EM.ID,EM.MEMBERSHIPNO,EM.EMPLOYEENAME,EM.ISRESIGNED,EM.NRIC,MP.POSITIONNAME,EM.GENDER";
                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    con.Open();
                    cmd.CommandTimeout = 0;
                    dtEmployee.Rows.Clear();
                    adp.Fill(dtEmployee);
                    if (dtEmployee.Rows.Count > 0)
                    {
                        dgEmployee.ItemsSource = dtEmployee.DefaultView;
                        Filteration();
                    }
                }
                //var st = (from x in db.MasterEmployees select x).ToList();
                ////st.Select(x => x.MasterPosition.PositionName)               
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
                if (!string.IsNullOrEmpty(txtSearch.Text))
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

                if (chkIsResigned.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(sWhere))
                    {
                        sWhere = sWhere + "RESIGNED=TRUE";
                    }
                    else
                    {
                        sWhere = "RESIGNED=TRUE";
                    }
                }

                if (!string.IsNullOrEmpty(sWhere))
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

