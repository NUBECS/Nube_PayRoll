using System;
using System.Collections.Generic;
using System.Data;
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
using System.Data.SqlClient;

namespace NUBE.PAYROLL.PL.Transaction
{
    /// <summary>
    /// Interaction logic for frmMonthlyDeductions.xaml
    /// </summary>
    public partial class frmMonthlyDeductions : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        DataTable dtMonthlyDeductions = new DataTable();
        public frmMonthlyDeductions()
        {
            InitializeComponent();
        }

        #region EVENTS

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWindow();
        }

        private void dtMonth_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FormFill();
        }

        private void dgMonthlyDeductions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgMonthlyDeductions.SelectedItem != null))
                {
                    if (!string.IsNullOrEmpty(dtMonth.Text))
                    {
                        DataRowView drv = (DataRowView)dgMonthlyDeductions.SelectedItem;
                        DateTime dt;
                        dt = string.IsNullOrEmpty(drv["ENTRYDATE"].ToString()) ? Convert.ToDateTime(dtMonth.SelectedDate) : Convert.ToDateTime(drv["ENTRYDATE"]);

                        frmYearAllowance frm = new frmYearAllowance(dt, Convert.ToInt32(drv["ID"]), 0, 0, Convert.ToInt32(drv["MLYDEDUCTID"]), 3);
                        frm.Title = "Monthly Allowance";
                        frm.txtPCBorBonus.Text = "Allowance Adv";
                        frm.txtExgr.Text = "Other Deductions";
                        frm.txtDispatchAllowance.Visibility = Visibility.Visible;
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
        #endregion

        #region FUNCTION

        void LoadWindow()
        {
            try
            {
                dtMonthlyDeductions.Rows.Clear();
                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    SqlCommand cmd;
                    string str = string.Format("SELECT ROW_NUMBER() OVER(ORDER BY ME.EMPLOYEENAME ASC) AS RNO,ME.ID,ME.MEMBERSHIPNO, \r" +
                        " ME.EMPLOYEENAME,ME.GENDER,ME.NRIC,0 PCBID,'' ENTRYDATE, \r" +
                        " 0 ALLOWANCEINADVANCED,0 OTHERDEDUCTIONS,0 DISPATCHALLOWANCE \r" +
                        " FROM MASTEREMPLOYEE ME(NOLOCK) ");

                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    con.Open();
                    dtMonthlyDeductions.Rows.Clear();
                    adp.Fill(dtMonthlyDeductions);
                    Filteration();
                    con.Close();
                }

                if (dtMonthlyDeductions.Rows.Count > 0)
                {
                    dgMonthlyDeductions.ItemsSource = dtMonthlyDeductions.DefaultView;
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
                        string str = string.Format("SELECT ROW_NUMBER() OVER(ORDER BY ME.EMPLOYEENAME ASC) AS RNO,ME.ID,ME.MEMBERSHIPNO, \r" +
                            " ME.EMPLOYEENAME,ME.SHORTNAME,ME.GENDER,ME.NRIC,ISNULL(PC.ID, 0)MLYDEDUCTID,PC.ENTRYDATE, \r" +
                            " ISNULL(PC.ALLOWANCEINADVANCED, 0)ALLOWANCEINADVANCED,ISNULL(OTHERDEDUCTIONS,0)OTHERDEDUCTIONS, \r" +
                            " ISNULL(PC.DISPATCHALLOWANCE,0)DISPATCHALLOWANCE \r" +
                            " FROM MASTEREMPLOYEE ME(NOLOCK) \r" +
                            " LEFT JOIN MONTHLYDEDUCTIONS PC(NOLOCK) ON PC.EMPLOYEEID = ME.ID AND MONTH(PC.ENTRYDATE)= MONTH('{0:dd/MMM/yyyy}')", dtDOB);
                        cmd = new SqlCommand(str, con);
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        con.Open();
                        dtMonthlyDeductions.Rows.Clear();
                        adp.Fill(dtMonthlyDeductions);
                        Filteration();
                        con.Close();
                    }

                    if (dtMonthlyDeductions.Rows.Count > 0)
                    {
                        // dgMonthlyDeductions.ItemsSource = dtMonthlyDeductions.DefaultView;
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
                        DataView dv = new DataView(dtMonthlyDeductions);
                        dv.RowFilter = sWhere;
                        DataTable dtTemp = new DataTable();
                        dtTemp = dv.ToTable();
                        dgMonthlyDeductions.ItemsSource = dtTemp.DefaultView;
                    }
                    else
                    {
                        dgMonthlyDeductions.ItemsSource = dtMonthlyDeductions.DefaultView;
                    }
                }
                else
                {
                    dgMonthlyDeductions.ItemsSource = dtMonthlyDeductions.DefaultView;
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
