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
            QueryExec();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fClear();
            }
            catch (Exception)
            {

                throw;
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
                    txtEmployeeNo.Text = drv["MEMBERSHIPNO"].ToString();
                    txtEmployeeName.Text = drv["EMPLOYEENAME"].ToString();
                    //if (chkIsPermission.IsChecked == true)
                    //{
                    //    iLeavePermissionId = Convert.ToInt32(drv["LEAVEPERMISSIONID"]);
                    //    using (SqlConnection con = new SqlConnection(Config.connStr))
                    //    {
                    //        string str = " SELECT FROMDATE,TODATE,LEAVETYPEID,NOOFDAYS,REASON,PERIOD FROM LEAVEPERMISSIONDETAILS(NOLOCK) WHERE ID=" + iLeavePermissionId;
                    //        SqlCommand cmd = new SqlCommand(str, con);
                    //        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    //        con.Open();
                    //        cmd.CommandTimeout = 0;
                    //        DataTable dt = new DataTable();
                    //        adp.Fill(dt);
                    //        if (dt.Rows.Count > 0)
                    //        {
                    //            dtpLeaveStartDate.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FROMDATE"]);
                    //            dtpLeaveEndDate.SelectedDate = Convert.ToDateTime(dt.Rows[0]["TODATE"]);
                    //            cmbLeaveType.SelectedValue = Convert.ToInt32(dt.Rows[0]["LEAVETYPEID"]);
                    //            txtTotalNoOfDays.Text = dt.Rows[0]["NOOFDAYS"].ToString();
                    //            txtRemarks.Text = dt.Rows[0]["REASON"].ToString();                                
                    //        }
                    //        else
                    //        {
                    //            dtpLeaveStartDate.SelectedDate = Convert.ToDateTime(drv["ATTDATE"]);
                    //            dtpLeaveEndDate.SelectedDate = Convert.ToDateTime(drv["ATTDATE"]);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    dtpLeaveStartDate.SelectedDate = Convert.ToDateTime(drv["ATTDATE"]);
                    dtpLeaveEndDate.SelectedDate = Convert.ToDateTime(drv["ATTDATE"]);
                    //}
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
                if (string.IsNullOrEmpty(txtEmployeeNo.Text))
                {
                    MessageBox.Show("Employee No is Empty!", "Empty");
                    txtEmployeeNo.Focus();
                }
                else if (string.IsNullOrEmpty(txtEmployeeName.Text))
                {
                    MessageBox.Show("Employee Name is Empty!", "Empty");
                    txtEmployeeName.Focus();
                }
                else if (string.IsNullOrEmpty(dtpLeaveStartDate.Text))
                {
                    MessageBox.Show("Leave Start Date is Empty!", "Empty");
                    dtpLeaveStartDate.Focus();
                }
                else if (string.IsNullOrEmpty(dtpLeaveEndDate.Text))
                {
                    MessageBox.Show("Leave End Date is Empty!", "Empty");
                    dtpLeaveEndDate.Focus();
                }
                else if (string.IsNullOrEmpty(cmbLeaveType.Text))
                {
                    MessageBox.Show("Leave Type is Empty!", "Empty");
                    cmbLeaveType.Focus();
                }
                else
                {
                    bIsValid = false;
                    Validate();
                    if (bIsValid == false && (dtpLeaveStartDate.SelectedDate == dtpLeaveEndDate.SelectedDate))
                    {
                        DateTime sDate = Convert.ToDateTime(dtpLeaveStartDate.SelectedDate);
                        var da = (from x in db.DailyAttedanceDets where x.EmployeeId == iEmployeeId && (x.IsFullDayLeave == true || x.IsHalfDayLeave == true) && x.IsWeekOff == false && x.IsPublicHoliday == false && x.AttDate == sDate select x).FirstOrDefault();
                        if (da != null)
                        {
                            if (cmbLeaveType.Text == "Half Present")
                            {
                                da.IsHalfDayLeave = true;
                                da.IsFullDayLeave = false;
                            }
                            else
                            {
                                da.IsHalfDayLeave = false;
                                da.IsFullDayLeave = false;
                            }
                            da.IsModified = true;
                            db.SaveChanges();
                        }
                        MessageBox.Show("Saved Sucessfully!", "Sucess");
                        QueryExec();
                    }
                    else if (bIsValid == false)
                    {
                        for (DateTime date = Convert.ToDateTime(dtpLeaveStartDate.SelectedDate); date <= Convert.ToDateTime(dtpLeaveEndDate.SelectedDate); date = date.AddDays(1))
                        {
                            var da = (from x in db.DailyAttedanceDets where x.EmployeeId == iEmployeeId && (x.IsFullDayLeave == true || x.IsHalfDayLeave == true) && x.IsWeekOff == false && x.IsPublicHoliday == false && x.AttDate == date select x).FirstOrDefault();
                            if (da != null)
                            {
                                if (cmbLeaveType.Text == "Half Present")
                                {
                                    da.IsHalfDayLeave = true;
                                    da.IsFullDayLeave = false;
                                }
                                else
                                {
                                    da.IsHalfDayLeave = false;
                                    da.IsFullDayLeave = false;
                                }
                                da.IsModified = true;
                                db.SaveChanges();
                            }
                        }
                        MessageBox.Show("Saved Sucessfully!", "Sucess");
                        QueryExec();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dtpLeaveStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtpLeaveStartDate.SelectedDate != null && dtpLeaveEndDate.SelectedDate != null)
                {
                    DateChangingFunct();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dtpLeaveEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtpLeaveStartDate.SelectedDate != null && dtpLeaveEndDate.SelectedDate != null)
                {
                    DateChangingFunct();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        #endregion

        #region Function

        void QueryExec()
        {
            try
            {
                string sWhere = "", sJoin = "", sSelect = "";
                PersonDetailsClear();

                if (!string.IsNullOrEmpty(cmbEmployee.Text))
                {
                    sWhere = " AND DA.EMPLOYEEID=" + Convert.ToInt32(cmbEmployee.SelectedValue);
                }

                //if (chkIsPermission.IsChecked == true)
                //{
                //    sWhere = sWhere + " AND DA.WITHPERMISSION=1 \r";
                //    sSelect = ",ISNULL(DA.LEAVEPERMISSIONID,0)LEAVEPERMISSIONID ";
                //    sJoin = " LEFT JOIN LEAVEPERMISSIONDETAILS LD(NOLOCK) ON LD.EMPLOYEEID=DA.EMPLOYEEID AND LD.ID=DA.LEAVEPERMISSIONID \r";
                //}
                //else
                //{
                //sWhere = sWhere + " AND DA.WITHPERMISSION=0 \r";
                sSelect = "";
                sJoin = "";
                //}

                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    string str = string.Format(" SELECT DA.EMPLOYEEID,EM.MEMBERSHIPNO,EM.EMPLOYEENAME,DA.ATTDATE,WITHPERMISSION,DA.REMARKS, \r" +
                                              " CASE WHEN ISNULL(DA.ISFULLDAYLEAVE, 0) <> 0 THEN 'FULL DAY' ELSE 'HALF DAY' END LEAVE " + sSelect + " \r" +
                                              " FROM DAILYATTEDANCEDET DA(NOLOCK) \r" +
                                              " LEFT JOIN MASTEREMPLOYEE EM(NOLOCK)ON EM.ID=DA.EMPLOYEEID \r" + sJoin +
                                              " WHERE(DA.ISFULLDAYLEAVE = 1 OR DA.ISHALFDAYLEAVE = 1) AND DA.ISWEEKOFF = 0 AND EM.UNPAIDLEAVE=1 \r" +
                                              " AND DA.ISPUBLICHOLIDAY = 0 AND DA.ATTDATE BETWEEN '{0:dd/MMM/yyyy}' AND '{1:dd/MMM/yyyy}' \r" + sWhere +
                                              " ORDER BY EM.EMPLOYEENAME,DA.ATTDATE", dtpFromDate.SelectedDate, dtpToDate.SelectedDate);
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
            for (DateTime date = Convert.ToDateTime(dtpLeaveStartDate.SelectedDate); date <= Convert.ToDateTime(dtpLeaveEndDate.SelectedDate); date = date.AddDays(1))
            {
                var da = (from x in db.DailyAttedanceDets where x.EmployeeId == iEmployeeId && (x.IsFullDayLeave == true || x.IsHalfDayLeave == true) && x.IsWeekOff == false && x.IsPublicHoliday == false && x.AttDate == date select x).FirstOrDefault();
                if (da == null)
                {
                    MessageBox.Show(txtEmployeeName.Text + " Present on that Date - " + string.Format("{0:dd/MMM/yyyy}", date) + " Please select other date!");
                    dtpLeaveEndDate.Focus();
                    bIsValid = true;
                }
            }
        }

        void DateChangingFunct()
        {
            if (dtpLeaveEndDate.SelectedDate < dtpLeaveStartDate.SelectedDate)
            {
                MessageBox.Show("Leave Start Date is must Less than Leave End Date !", "Incorrect Date Format");
                dtpLeaveStartDate.SelectedDate = dtpLeaveEndDate.SelectedDate;
                dtpLeaveEndDate.Focus();
            }

            if (dtpLeaveStartDate.SelectedDate != dtpLeaveEndDate.SelectedDate)
            {
                TimeSpan dfDays = (Convert.ToDateTime(dtpLeaveEndDate.SelectedDate) - Convert.ToDateTime(dtpLeaveStartDate.SelectedDate));
                txtTotalNoOfDays.Text = (dfDays.TotalDays + 1).ToString();
            }
            else
            {
                txtTotalNoOfDays.Text = "1";
            }
        }

        void PersonDetailsClear()
        {
            iEmployeeId = 0;
            txtEmployeeName.Text = "";
            txtEmployeeNo.Text = "";
            txtRemarks.Text = "";
            txtTotalNoOfDays.Text = "";
            dtpLeaveStartDate.Text = "";
            dtpLeaveEndDate.Text = "";
            cmbLeaveType.Text = "";
        }

        void fClear()
        {
            PersonDetailsClear();
            dtpFromDate.Text = "";
            dtpToDate.Text = "";
            dgManualAttendance.ItemsSource = null;
            cmbEmployee.Text = "";
            bIsValid = false;
            iEmployeeId = 0;
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
    }
}
