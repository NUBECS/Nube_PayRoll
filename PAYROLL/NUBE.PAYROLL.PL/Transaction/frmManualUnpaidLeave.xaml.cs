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
using System.Windows.Shapes;
using NUBE.PAYROLL.CMN;
using System.Data.SqlClient;

namespace NUBE.PAYROLL.PL.Transaction
{
    /// <summary>
    /// Interaction logic for frmManualUnpaidLeave.xaml
    /// </summary>
    public partial class frmManualUnpaidLeave : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        DataTable dtDailyAttedance = new DataTable();
        int iEmployeeId = 0;
        int iTableId = 0;
        DateTime? dtSalaryDate = null;
        Boolean bIsValid = false;
        public frmManualUnpaidLeave()
        {
            InitializeComponent();
        }

        private void ManualAttendance_Loaded(object sender, RoutedEventArgs e)
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

        #region Events

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

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
                    iEmployeeId = Convert.ToInt32(drv["EMPLOYEEID"]);
                    iTableId = Convert.ToInt32(drv["ID"]);
                    dtSalaryDate = Convert.ToDateTime(drv["SALARYMONTH"]);

                    txtEmployeeNo.Text = drv["EMPLOYEENO"].ToString();
                    txtEmployeeName.Text = drv["EMPLOYEENAME"].ToString();
                    txtTotalAmount.Text = drv["LOP_LEAVE"].ToString();
                    txtTotalDaysAbsent.Text = drv["DAYSABSENT"].ToString();
                    txtLOPLate.Text = drv["LOP_LATE"].ToString();
                    txtRemarks.Text = drv["REMARKS"].ToString();
                    //QueryExec();
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
                    decimal dLeave = 0;
                    decimal dLate = 0;
                    decimal dDaysAbsent = 0;
                    DateTime dtEntryDate = Convert.ToDateTime(dtSalaryDate);

                    if (!string.IsNullOrEmpty(txtTotalAmount.Text))
                    {
                        dLeave = Convert.ToDecimal(txtTotalAmount.Text);
                    }
                    if (!string.IsNullOrEmpty(txtLOPLate.Text))
                    {
                        dLate = Convert.ToDecimal(txtLOPLate.Text);
                    }
                    if (!string.IsNullOrEmpty(txtTotalDaysAbsent.Text))
                    {
                        dDaysAbsent = Convert.ToDecimal(txtTotalDaysAbsent.Text);
                    }

                    var sl = (from x in db.MonthlySalaries where x.EmployeeId == iEmployeeId && x.Id == iTableId select x).FirstOrDefault();

                    if (sl != null)
                    {
                        sl.TOTALDEDUCTION = sl.TOTALDEDUCTION - (sl.LOP_LATE + sl.LOP_LEAVE);
                        sl.LOP_LEAVE = dLeave;
                        sl.DaysAbsent = dDaysAbsent;
                        sl.LOP_LATE = dLate;
                        sl.TOTALDEDUCTION = sl.TOTALDEDUCTION + (sl.LOP_LATE + sl.LOP_LEAVE);
                        sl.NETSALARY = sl.TOTALEARNING - sl.TOTALDEDUCTION;
                        if (!string.IsNullOrEmpty(txtRemarks.Text))
                        {
                            sl.Remarks = txtRemarks.Text;
                        }                        

                        var da = (from x in db.ManualUnpaidLeaves where x.EmployeeId == iEmployeeId && x.Year == dtEntryDate.Year && x.Month == dtEntryDate.Month select x).FirstOrDefault();
                        if (da != null)
                        {
                            db.ManualUnpaidLeaves.Remove(da);
                            db.SaveChanges();
                        }

                        ManualUnpaidLeave leave = new ManualUnpaidLeave();
                        leave.Year = dtEntryDate.Year;
                        leave.Month = dtEntryDate.Month;
                        leave.EmployeeId = iEmployeeId;
                        leave.DaysAbsent = dDaysAbsent;
                        leave.LOP_Leave = dLeave;
                        leave.LOP_Late = dLate;
                        leave.EntryDate = DateTime.Now;
                        //leave.Remarks = txtRemarks.Text;
                        db.ManualUnpaidLeaves.Add(leave);

                        db.SaveChanges();                        
                        PersonDetailsClear();
                        QueryExec();

                    }
                    else
                    {
                        MessageBox.Show("Salary Details Not Match Contact Administrator");
                    }
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
                    sWhere = " AND EMPLOYEEID=" + Convert.ToInt32(cmbEmployee.SelectedValue);
                }

                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    string str = string.Format(" SELECT ID,EMPLOYEEID,EMPLOYEENO,EMPLOYEENAME,SALARYMONTH,TOTALWORKINGDAYS,DAYSABSENT,LOP_LEAVE,LOP_LATE,ISNULL(REMARKS,'')REMARKS \r" +
                                               " FROM MONTHLYSALARY(NOLOCK) \r" +
                                               " WHERE MONTH(SALARYMONTH)=MONTH('{0:dd/MMM/yyyy}') AND YEAR(SALARYMONTH)= YEAR('{0:dd/MMM/yyyy}') \r" + sWhere +
                                               " GROUP BY ID,EMPLOYEEID,EMPLOYEENO,EMPLOYEENAME,SALARYMONTH,TOTALWORKINGDAYS,DAYSABSENT,LOP_LEAVE,LOP_LATE,REMARKS ", dtpEntryDate.SelectedDate);

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
            else if (string.IsNullOrEmpty(txtTotalDaysAbsent.Text))
            {
                MessageBox.Show("Total OT Hours is Empty!", "Empty");
                txtTotalDaysAbsent.Focus();
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
            else if (iTableId == 0)
            {
                MessageBox.Show("Please Try Again! Monthly Salary Not Match Contact Administrator", "Contact Administrator");
                bIsValid = true;
                PersonDetailsClear();
            }            
        }

        void PersonDetailsClear()
        {
            iEmployeeId = 0;
            iTableId = 0;
            dtSalaryDate = null;
            txtEmployeeNo.Text = "";
            txtEmployeeName.Text = "";
            txtTotalAmount.Text = "";
            txtTotalDaysAbsent.Text = "";
            txtLOPLate.Text = "";
            txtRemarks.Text = "";
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

        private void txtTotalOTHours_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (txtTotalDaysAbsent.Text == "")
            //{
            //    txtTotalAmount.Text = "";
            //}
            //else
            //{
            //    using (SqlConnection con = new SqlConnection(Config.connStr))
            //    {
            //        string str = string.Format("  SELECT EMP.ID,EMP.MEMBERSHIPNO,EMP.EMPLOYEENAME,WK.ATT_YEAR,WK.ATT_MONTH,WK.WORKINGDAYS,  \r" +
            //                    " CASE WHEN ISNULL(MT.LOP,0)<>0 THEN ISNULL(MT.DaysAbsent,0) ELSE ISNULL(FL.TOTALLEAVE,0.0) END LEAVE,   \r" +
            //                    "  CASE WHEN ISNULL(MT.LOP,0)<>0 THEN ISNULL(MT.LOP,0) ELSE CONVERT(NUMERIC(18, 2),  \r" +
            //                    "  {1} * (((EMP.BASICSALARY / WK.WORKINGDAYS) ) )) END LOP,' ' REMARKS  \r" +
            //                    " FROM MASTEREMPLOYEE EMP(NOLOCK)  \r" +
            //                    " LEFT JOIN VIEWMONTHLY_WORKINGDAYS WK(NOLOCK) ON WK.EMPLOYEEID = EMP.ID \r" +
            //                    "  LEFT JOIN VIEWWORKINGDAYS FL(NOLOCK) ON FL.EMPLOYEEID = EMP.ID AND FL.ATT_YEAR = WK.ATT_YEAR AND FL.ATT_MONTH = WK.ATT_MONTH  \r" +
            //                    "  LEFT JOIN ManualUnpaidLeave MT(NOLOCK) ON MT.EMPLOYEEID=EMP.ID AND MT.YEAR=WK.ATT_YEAR AND MT.MONTH=WK.ATT_MONTH  \r" +
            //                    "  WHERE WK.ATT_MONTH=MONTH('{0:dd/MMM/yyyy}') AND WK.ATT_YEAR = YEAR('{0:dd/MMM/yyyy}') AND EMP.ISRESIGNED = 0 and emp.membershipno={2} \r" +
            //                    " GROUP BY EMP.ID,EMP.MEMBERSHIPNO,EMP.EMPLOYEENAME,WK.ATT_YEAR,WK.ATT_MONTH,WK.WORKINGDAYS,MT.DaysAbsent,FL.TOTALLEAVE, \r" +
            //                    " EMP.BASICSALARY,WK.WORKINGDAYS,MT.LOP ", dtpEntryDate.SelectedDate, txtTotalDaysAbsent.Text, txtEmployeeNo.Text);

            //        SqlCommand cmd = new SqlCommand(str, con);
            //        SqlDataAdapter adp = new SqlDataAdapter(cmd);
            //        con.Open();
            //        using (SqlDataReader read = cmd.ExecuteReader())
            //        {
            //            while (read.Read())
            //            {
            //                txtTotalAmount.Text = (read["LOP"].ToString());
            //            }
            //            read.Close();

            //        }
            //    }
            //}
            //QueryExec();
        }

        #endregion



    }
}

