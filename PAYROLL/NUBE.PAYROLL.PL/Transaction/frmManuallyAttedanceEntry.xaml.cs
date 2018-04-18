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
    /// Interaction logic for frmManuallyAttedanceEntry.xaml
    /// </summary>
    public partial class frmManuallyAttedanceEntry : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        DataTable dtDailyAttedance = new DataTable();
        int iEmployeeId = 0;
        int iAtt_Year = 0;
        int iAtt_Month = 0;
        decimal dPerHourAmount = 0;
        Boolean bIsValid = false;

        public frmManuallyAttedanceEntry()
        {
            InitializeComponent();
        }

        #region Events

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadForm();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            PersonDetailsClear();
            QueryExec();

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fClear();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgManualAttendance_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgManualAttendance.SelectedItem != null))
                {
                    PersonDetailsClear();
                    DataRowView drv = (DataRowView)dgManualAttendance.SelectedItem;
                    iEmployeeId = Convert.ToInt32(drv["ID"]);
                    iAtt_Year = Convert.ToInt32(drv["ATT_YEAR"]);
                    iAtt_Month = Convert.ToInt32(drv["ATT_MONTH"]);
                    txtEmployeeNo.Text = drv["MEMBERSHIPNO"].ToString();
                    txtEmployeeName.Text = drv["EMPLOYEENAME"].ToString();
                    txtTotalAmount.Text = drv["OT_AMOUNT"].ToString();
                    dPerHourAmount = Convert.ToDecimal(drv["OT_PER_HOUR"]);
                    txtTotalOTHours.Text = drv["TOTAL_OT"].ToString();
                    txtRemarks.Text = drv["REMARKS"].ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Validate();
                if (bIsValid == false)
                {
                    var da = (from x in db.ManualOTEntries where x.EmployeeId == iEmployeeId && x.Year == iAtt_Year && x.Month == iAtt_Month select x).FirstOrDefault();
                    if (da != null)
                    {
                        db.ManualOTEntries.Remove(da);
                        db.SaveChanges();
                    }

                    ManualOTEntry OT = new ManualOTEntry();
                    OT.Year = iAtt_Year;
                    OT.Month = iAtt_Month;
                    OT.EmployeeId = iEmployeeId;
                    OT.TotalOtHours = Convert.ToDecimal(txtTotalOTHours.Text);
                    OT.TotalOtAmount = Convert.ToDecimal(txtTotalAmount.Text);
                    OT.EntryDate = DateTime.Now;
                    OT.Remarks = txtRemarks.Text;
                    db.ManualOTEntries.Add(OT);
                    db.SaveChanges();
                    PersonDetailsClear();
                    QueryExec();                   
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }
        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            Config.CheckIsNumeric(e);
        }
        private void txtTotalOTHours_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void txtTotalOTHours_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void dgManualAttendance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                PersonDetailsClear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Function

        void QueryExec()
        {
            try
            {
                string sWhere = "";


                if (!string.IsNullOrEmpty(cmbEmployee.Text))
                {
                    sWhere = " AND EMP.ID=" + Convert.ToInt32(cmbEmployee.SelectedValue);
                }

                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    string str = string.Format(" SELECT EMP.ID,EMP.MEMBERSHIPNO,EMP.EMPLOYEENAME,WK.ATT_YEAR,WK.ATT_MONTH,WK.WORKINGDAYS,  \r" +
                                " CASE WHEN ISNULL(MT.TOTALOTHOURS,0)<>0 THEN ISNULL(MT.TOTALOTHOURS,0) ELSE ISNULL(OT.TOTAL_OT,0.0) END TOTAL_OT,  \r" +
                                " CONVERT(NUMERIC(18, 2), ((EMP.BASICSALARY / WK.WORKINGDAYS) / 8) + ((((EMP.BASICSALARY / WK.WORKINGDAYS) / 8) * 10) / 100))OT_PER_HOUR, \r" +
                                " CASE WHEN ISNULL(MT.TOTALOTAMOUNT,0)<>0 THEN ISNULL(MT.TOTALOTAMOUNT,0) ELSE CONVERT(NUMERIC(18, 2), \r" +
                                " ISNULL(OT.TOTAL_OT, 0) * (((EMP.BASICSALARY / WK.WORKINGDAYS) / 8) + ((((EMP.BASICSALARY / WK.WORKINGDAYS) / 8) * 10) / 100))) END OT_AMOUNT,' ' REMARKS  \r" +
                                " FROM MASTEREMPLOYEE EMP(NOLOCK)  \r" +
                                " CROSS APPLY DBO.[MONTHLYATT]('{0:dd/MMM/yyyy}',EMP.ID) WK \r" +
                                " LEFT JOIN VIEWTOTALOT OT(NOLOCK) ON OT.EMPLOYEEID = EMP.ID AND OT.ATT_YEAR = WK.ATT_YEAR AND OT.ATT_MONTH = WK.ATT_MONTH  \r" +
                                " LEFT JOIN MANUALOTENTRY MT(NOLOCK) ON MT.EMPLOYEEID=EMP.ID AND MT.YEAR=WK.ATT_YEAR AND MT.MONTH=WK.ATT_MONTH \r" +
                                " WHERE EMP.ISRESIGNED = 0 \r" + sWhere +
                                " GROUP BY EMP.ID,EMP.MEMBERSHIPNO,EMP.EMPLOYEENAME,WK.ATT_YEAR,WK.ATT_MONTH,WK.WORKINGDAYS,MT.TOTALOTHOURS,OT.TOTAL_OT, \r" +
                                " EMP.BASICSALARY,WK.WORKINGDAYS,MT.TOTALOTAMOUNT ", dtpEntryDate.SelectedDate);

                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    con.Open();
                    cmd.CommandTimeout = 0;
                    dtDailyAttedance.Rows.Clear();
                    adp.Fill(dtDailyAttedance);
                    if (dtDailyAttedance.Rows.Count > 0)
                    {
                        dgManualAttendance.ItemsSource = dtDailyAttedance.DefaultView;
                    }
                    else
                    {
                        dgManualAttendance.ItemsSource = null;
                        MessageBox.Show("No Records Found!");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        void Validate()
        {
            bIsValid = false;
            if (string.IsNullOrEmpty(txtEmployeeNo.Text))
            {
                MessageBox.Show("Employee No is Empty!", "Empty");
                txtEmployeeNo.Focus();
                bIsValid = true;
            }
            else if (string.IsNullOrEmpty(txtEmployeeName.Text))
            {
                MessageBox.Show("Employee Name is Empty!", "Empty");
                txtEmployeeName.Focus();
                bIsValid = true;
            }
            else if (string.IsNullOrEmpty(txtTotalOTHours.Text))
            {
                MessageBox.Show("Total OT Hours is Empty!", "Empty");
                txtTotalOTHours.Focus();
                bIsValid = true;
            }
            else if (string.IsNullOrEmpty(txtTotalAmount.Text))
            {
                MessageBox.Show("Total Amount is Empty!", "Empty");
                txtTotalAmount.Focus();
                bIsValid = true;
            }
            else if (iEmployeeId == 0)
            {
                MessageBox.Show("Please Try Again! Employee Code is Empty", "Employee Id Not Match");
                bIsValid = true;
                PersonDetailsClear();
            }
            else if (iAtt_Year == 0)
            {
                MessageBox.Show("Please Try Again! Year is Empty", "Year Not Match");
                bIsValid = true;
                PersonDetailsClear();
            }
            else if (iAtt_Month == 0)
            {
                MessageBox.Show("Please Try Again! Month is Empty", "Month Not Match");
                bIsValid = true;
                PersonDetailsClear();
            }
        }

        void PersonDetailsClear()
        {
            iEmployeeId = 0;
            iAtt_Year = 0;
            iAtt_Month = 0;
            txtEmployeeNo.Text = "";
            txtEmployeeName.Text = "";
            txtTotalAmount.Text = "";
            txtTotalOTHours.Text = "";
            txtRemarks.Text = "";
            dPerHourAmount = 0;
        }

        void fClear()
        {
            PersonDetailsClear();
            dtpEntryDate.Text = "";
            dgManualAttendance.ItemsSource = null;
            cmbEmployee.Text = "";
            bIsValid = false;
        }

        void LoadForm()
        {
            fClear();
            var cm = (from x in db.MasterEmployees orderby x.EmployeeName select x).ToList();
            if (cm != null)
            {
                cmbEmployee.ItemsSource = cm.ToList();
                cmbEmployee.SelectedValuePath = "Id";
                cmbEmployee.DisplayMemberPath = "EmployeeName";
            }
        }



        #endregion

        private void txtTotalOTHours_KeyUp_1(object sender, KeyEventArgs e)
        {

        }

        private void txtTotalOTHours_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtTotalOTHours_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTotalOTHours.Text))
            {
                decimal dOtHour = Convert.ToDecimal(txtTotalOTHours.Text);
                if (dOtHour > 0 && dPerHourAmount > 0)
                {
                    dOtHour = Math.Abs((dOtHour * dPerHourAmount) + (((dOtHour * dPerHourAmount) * 10) / 100));
                    txtTotalAmount.Text = dOtHour.ToString();
                }
            }
        }
    }
}
