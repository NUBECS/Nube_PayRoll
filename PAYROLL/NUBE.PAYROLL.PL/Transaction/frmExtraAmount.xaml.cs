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

namespace NUBE.PAYROLL.PL.Transaction
{
    /// <summary>
    /// Interaction logic for frmExtraAmount.xaml
    /// </summary>
    public partial class frmExtraAmount : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        public frmExtraAmount()
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

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cbxCase_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cbxCase_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void rptStartWith_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rptContain_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rptEndWith_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgExtraAmount_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgExtraAmount.SelectedItem != null))
                {
                    if (!string.IsNullOrEmpty(dtMonth.Text))
                    {
                        DataRowView drv = (DataRowView)dgExtraAmount.SelectedItem;
                        DateTime dt;
                        dt = string.IsNullOrEmpty(drv["ENTRYDATE"].ToString()) ? Convert.ToDateTime(dtMonth.SelectedDate) : Convert.ToDateTime(drv["ENTRYDATE"]);

                        frmYearAllowance frm = new frmYearAllowance(dt, Convert.ToInt32(drv["ID"]), Convert.ToInt32(drv["ALLOWANCEID"]), 0, 1);
                        frm.Title = "Bonus & Ex-gratia";
                        frm.txtPCBorBonus.Text = "Bonus";
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

        #endregion

        #region Function

        void LoadWindow()
        {
            try
            {
                var emp = (from x in db.ViewManualPayments select x).ToList();
                DataTable dt = AppLib.LINQResultToDataTable(emp);
                if (dt.Rows.Count > 0)
                {
                    dgExtraAmount.ItemsSource = dt.DefaultView;
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
                    DataTable dt = new DataTable();
                    using (SqlConnection con = new SqlConnection("Data Source =.\\sqlexpress;Initial Catalog = payroll; Integrated Security = True"))
                    {
                        SqlCommand cmd;
                        string str = string.Format("SELECT ROW_NUMBER() OVER(ORDER BY ME.EMPLOYEENAME ASC) AS RNO," +
                            "ME.ID,ME.MEMBERSHIPNO,ME.EMPLOYEENAME,ME.SHORTNAME,ME.GENDER, ME.NRIC," +
                            "ISNULL(YA.ID, 0)ALLOWANCEID,YA.ENTRYDATE,ISNULL(YA.BONUS, 0)BONUS," +
                            "ISNULL(YA.EXGRATIA, 0)EXGRATIA FROM MASTEREMPLOYEE ME(NOLOCK)" +
                            "LEFT JOIN YEARLYALLOWANCE YA(NOLOCK) ON YA.EMPLOYEEID = ME.ID AND MONTH(YA.ENTRYDATE) = MONTH('{0:dd/MMM/yyyy}')", dtDOB);
                        cmd = new SqlCommand(str, con);
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        con.Open();
                        adp.Fill(dt);
                        con.Close();
                    }

                    //var bns = (from x in db.VIEWYEARLYALLOWANCEs where x.ENTRYDATE == dtDOB select x).ToList();
                    if (dt.Rows.Count > 0)
                    {
                        dgExtraAmount.ItemsSource = dt.DefaultView;
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

        #endregion


    }
}
