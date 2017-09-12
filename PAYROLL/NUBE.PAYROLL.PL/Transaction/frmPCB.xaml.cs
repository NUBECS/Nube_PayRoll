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
using System.Data;
using NUBE.PAYROLL.CMN;
using System.Data.SqlClient;

namespace NUBE.PAYROLL.PL.Transaction
{
    /// <summary>
    /// Interaction logic for frmPCB.xaml
    /// </summary>
    public partial class frmPCB : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        DataTable dtPCB = new DataTable();
        public frmPCB()
        {
            InitializeComponent();
        }

        #region EVENTS

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWindow();
        }

        private void dgPCB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgPCB.SelectedItem != null))
                {
                    if (!string.IsNullOrEmpty(dtMonth.Text))
                    {
                        DataRowView drv = (DataRowView)dgPCB.SelectedItem;
                        DateTime dt;
                        dt = string.IsNullOrEmpty(drv["ENTRYDATE"].ToString()) ? Convert.ToDateTime(dtMonth.SelectedDate) : Convert.ToDateTime(drv["ENTRYDATE"]);

                        frmYearAllowance frm = new frmYearAllowance(dt, Convert.ToInt32(drv["ID"]), 0, Convert.ToInt32(drv["PCBID"]), 2);
                        frm.Title = "PCB";
                        frm.txtPCBorBonus.Text = "PCB";
                        frm.txtExgratia.Visibility = Visibility.Hidden;
                        frm.ShowDialog();
                        FormFill();
                    }
                    else
                    {
                        MessageBox.Show("Please Select Month", "PAYROLL");
                        dtMonth.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dtMonth_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FormFill();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtSearch.Text = "";
                dtMonth.Text = "";
                LoadWindow();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
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

        #region FUNCTION

        void LoadWindow()
        {
            try
            {
                var emp = (from x in db.ViewManualPayments select x).ToList();
                dtPCB.Rows.Clear();
                dtPCB = AppLib.LINQResultToDataTable(emp);
                if (dtPCB.Rows.Count > 0)
                {
                    dgPCB.ItemsSource = dtPCB.DefaultView;
                    Filteration();
                }
            }
            catch (Exception e)
            {
                ExceptionLogging.SendErrorToText(e);
            }
        }

        void FormFill()
        {
            try
            {
                if (!string.IsNullOrEmpty(dtMonth.Text))
                {
                    DateTime dtDOB = Convert.ToDateTime(dtMonth.SelectedDate);
                    using (SqlConnection con = new SqlConnection(Config.connStr))
                    {
                        SqlCommand cmd;
                        string str = string.Format("SELECT ROW_NUMBER() OVER(ORDER BY ME.EMPLOYEENAME ASC) AS RNO,\r" +
                            " ME.ID,ME.MEMBERSHIPNO,ME.EMPLOYEENAME,ME.SHORTNAME,ME.GENDER,\r" +
                            " ME.NRIC,ISNULL(PC.ID, 0)PCBID,PC.ENTRYDATE,ISNULL(PC.PCB, 0)PCB\r" +
                            " FROM MASTEREMPLOYEE ME(NOLOCK)\r" +
                            " LEFT JOIN PCB PC(NOLOCK) ON PC.EMPLOYEEID = ME.ID AND MONTH(PC.ENTRYDATE)= MONTH('{0:dd/MMM/yyyy}')", dtDOB);
                        cmd = new SqlCommand(str, con);
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        con.Open();
                        dtPCB.Rows.Clear();
                        adp.Fill(dtPCB);
                        Filteration();
                        con.Close();
                    }

                    if (dtPCB.Rows.Count > 0)
                    {
                        //dgPCB.ItemsSource = dtPCB.DefaultView;
                    }
                    else
                    {
                        MessageBox.Show("No Records Found");
                        LoadWindow();
                    }
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
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    if (rptContain.IsChecked == true)
                    {
                        sWhere = " EMPLOYEENAME LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else if (rptEndWith.IsChecked == true)
                    {
                        sWhere = " EMPLOYEENAME LIKE '%" + txtSearch.Text.ToUpper() + "'";
                    }
                    else if (rptStartWith.IsChecked == true)
                    {
                        sWhere = " EMPLOYEENAME LIKE '" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else
                    {
                        sWhere = " EMPLOYEENAME LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }

                    if (!string.IsNullOrEmpty(txtSearch.Text))
                    {
                        DataView dv = new DataView(dtPCB);
                        dv.RowFilter = sWhere;
                        DataTable dtTemp = new DataTable();
                        dtTemp = dv.ToTable();
                        dgPCB.ItemsSource = dtTemp.DefaultView;
                    }
                    else
                    {
                        dgPCB.ItemsSource = dtPCB.DefaultView;
                    }
                }
                else
                {
                    dgPCB.ItemsSource = dtPCB.DefaultView;
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
