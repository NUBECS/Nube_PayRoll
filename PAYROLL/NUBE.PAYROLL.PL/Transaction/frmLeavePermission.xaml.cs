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
using System.Data.SqlClient;
using System.Data;

namespace NUBE.PAYROLL.PL.Transaction
{
    /// <summary>
    /// Interaction logic for frmLeavePermission.xaml
    /// </summary>
    public partial class frmLeavePermission : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        DataTable dtDailyAttedance = new DataTable();
        int iEmployeeId = 0;
        Boolean bIsValid = false;
        int iLeavePermissionId = 0;
        public frmLeavePermission()
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

        private void dgHalf_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgHalf.SelectedItem != null))
                {
                    PersonDetailsClear();
                    DataRowView drv = (DataRowView)dgHalf.SelectedItem;
                    iEmployeeId = Convert.ToInt32(drv["EMPLOYEEID"]);
                    txtEmployeeNo.Text = drv["MEMBERSHIPNO"].ToString();
                    txtEmployeeName.Text = drv["EMPLOYEENAME"].ToString();
                    if (chkIsPermission.IsChecked == true)
                    {
                        iLeavePermissionId = Convert.ToInt32(drv["LEAVEPERMISSIONID"]);
                        using (SqlConnection con = new SqlConnection(Config.connStr))
                        {
                            string str = " SELECT FROMDATE,TODATE,LEAVETYPEID,NOOFDAYS,REASON,PERIOD FROM LEAVEPERMISSIONDETAILS(NOLOCK) WHERE ID=" + iLeavePermissionId;
                            SqlCommand cmd = new SqlCommand(str, con);
                            SqlDataAdapter adp = new SqlDataAdapter(cmd);
                            con.Open();
                            cmd.CommandTimeout = 0;
                            DataTable dt = new DataTable();
                            adp.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                dtpLeaveStartDate.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FROMDATE"]);
                                dtpLeaveEndDate.SelectedDate = Convert.ToDateTime(dt.Rows[0]["TODATE"]);
                                cmbLeaveType.SelectedValue = Convert.ToInt32(dt.Rows[0]["LEAVETYPEID"]);
                                txtTotalNoOfDays.Text = dt.Rows[0]["NOOFDAYS"].ToString();
                                txtRemarks.Text = dt.Rows[0]["REASON"].ToString();
                                cmbLeaveTime.Text = dt.Rows[0]["PERIOD"].ToString();
                            }
                            else
                            {
                                dtpLeaveStartDate.SelectedDate = Convert.ToDateTime(drv["ATTDATE"]);
                                dtpLeaveEndDate.SelectedDate = Convert.ToDateTime(drv["ATTDATE"]);
                            }
                        }
                    }
                    else
                    {
                        dtpLeaveStartDate.SelectedDate = Convert.ToDateTime(drv["ATTDATE"]);
                        dtpLeaveEndDate.SelectedDate = Convert.ToDateTime(drv["ATTDATE"]);
                    }
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
                else if (string.IsNullOrEmpty(cmbLeaveTime.Text))
                {
                    MessageBox.Show("Leave Time is Empty!", "Empty");
                    cmbLeaveTime.Focus();
                }
                else
                {
                    bIsValid = false;
                    Validate();
                    if (bIsValid == false)
                    {
                        if (dtpLeaveStartDate.SelectedDate == dtpLeaveEndDate.SelectedDate)
                        {
                            DateTime sDate = Convert.ToDateTime(dtpLeaveStartDate.SelectedDate);
                            if (cmbLeaveTime.Text == "Full Day")
                            {
                                LeavePermissionDetail lst = new LeavePermissionDetail();
                                lst.EmployeeId = iEmployeeId;
                                lst.EntryDate = sDate;
                                lst.FromDate = dtpLeaveStartDate.SelectedDate;
                                lst.ToDate = dtpLeaveEndDate.SelectedDate;
                                lst.LeaveTypeId = Convert.ToInt32(cmbLeaveType.SelectedValue);
                                lst.IsApproved = true;
                                lst.NoOfDays = Convert.ToDecimal(txtTotalNoOfDays.Text);
                                lst.AprovedFromDate = dtpLeaveStartDate.SelectedDate;
                                lst.AprovedToDate = dtpLeaveEndDate.SelectedDate;
                                lst.NoOfDaysApproved = Convert.ToDecimal(txtTotalNoOfDays.Text);
                                lst.Reason = txtRemarks.Text;
                                lst.Remarks = "";
                                lst.Status = "Approved";
                                lst.Period = "Full Day";
                                db.LeavePermissionDetails.Add(lst);
                                db.SaveChanges();
                            }
                            else
                            {
                                int iShiftId = 0;
                                var emp = (from x in db.MasterEmployees where x.Id == iEmployeeId select x).FirstOrDefault();
                                if (emp != null && emp.ShiftId != 0)
                                {
                                    iShiftId = emp.ShiftId;
                                    var shf = (from x in db.EmployeeShifts where x.Id == iShiftId select x).FirstOrDefault();
                                    if (shf != null)
                                    {
                                        LeavePermissionDetail lst = new LeavePermissionDetail();
                                        lst.EmployeeId = iEmployeeId;
                                        lst.EntryDate = DateTime.Today;

                                        if (cmbLeaveTime.Text == "First-half")
                                        {
                                            lst.FromDate = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtpLeaveStartDate.SelectedDate, Convert.ToDateTime(shf.InTime).Hour, Convert.ToDateTime(shf.InTime).Minute));
                                            lst.ToDate = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtpLeaveEndDate.SelectedDate, Convert.ToDateTime(shf.LunchTimeOut).Hour, Convert.ToDateTime(shf.LunchTimeOut).Minute));

                                            lst.AprovedFromDate = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtpLeaveStartDate.SelectedDate, Convert.ToDateTime(shf.InTime).Hour, Convert.ToDateTime(shf.InTime).Minute));
                                            lst.AprovedToDate = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtpLeaveEndDate.SelectedDate, Convert.ToDateTime(shf.LunchTimeOut).Hour, Convert.ToDateTime(shf.LunchTimeOut).Minute));
                                            lst.Period = "First-half";
                                        }
                                        else if (cmbLeaveTime.Text == "Second-half")
                                        {
                                            lst.FromDate = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtpLeaveStartDate.SelectedDate, Convert.ToDateTime(shf.LunchTimeIn).Hour, Convert.ToDateTime(shf.LunchTimeIn).Minute));
                                            lst.ToDate = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtpLeaveEndDate.SelectedDate, Convert.ToDateTime(shf.OutTime).Hour, Convert.ToDateTime(shf.OutTime).Minute));

                                            lst.AprovedFromDate = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtpLeaveStartDate.SelectedDate, Convert.ToDateTime(shf.LunchTimeIn).Hour, Convert.ToDateTime(shf.LunchTimeIn).Minute));
                                            lst.AprovedToDate = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtpLeaveEndDate.SelectedDate, Convert.ToDateTime(shf.OutTime).Hour, Convert.ToDateTime(shf.OutTime).Minute));
                                            lst.Period = "Second-half";
                                        }
                                        lst.LeaveTypeId = Convert.ToInt32(cmbLeaveType.SelectedValue);
                                        lst.IsApproved = true;
                                        lst.NoOfDays = Convert.ToDecimal(0.5);

                                        lst.NoOfDaysApproved = Convert.ToDecimal(0.5);
                                        lst.Reason = txtRemarks.Text;
                                        lst.Remarks = "";
                                        lst.Status = "Approved";
                                        db.LeavePermissionDetails.Add(lst);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Shift Details are Wrong!", "Error");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Shift Details are Wrong!", "Error");
                                }
                            }

                            int iLId = Convert.ToInt32(db.LeavePermissionDetails.Max(x => x.Id));

                            var da = (from x in db.DailyAttedanceDets where x.EmployeeId == iEmployeeId && (x.IsFullDayLeave == true || x.IsHalfDayLeave == true) && x.IsWeekOff == false && x.IsPublicHoliday == false && x.AttDate == sDate select x).FirstOrDefault();
                            if (da != null)
                            {
                                da.WithPermission = true;
                                da.LeavePermissionId = iLId;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            LeavePermissionDetail lst = new LeavePermissionDetail();
                            lst.EmployeeId = iEmployeeId;
                            lst.EntryDate = DateTime.Today;
                            lst.FromDate = dtpLeaveStartDate.SelectedDate;
                            lst.ToDate = dtpLeaveEndDate.SelectedDate;
                            lst.LeaveTypeId = Convert.ToInt32(cmbLeaveType.SelectedValue);
                            lst.IsApproved = true;
                            lst.NoOfDays = Convert.ToDecimal(txtTotalNoOfDays.Text);
                            lst.AprovedFromDate = dtpLeaveStartDate.SelectedDate;
                            lst.AprovedToDate = dtpLeaveEndDate.SelectedDate;
                            lst.NoOfDaysApproved = Convert.ToDecimal(txtTotalNoOfDays.Text);
                            lst.Reason = txtRemarks.Text;
                            lst.Remarks = "";
                            lst.Status = "Approved";
                            lst.Period = "Full Day";
                            db.LeavePermissionDetails.Add(lst);
                            db.SaveChanges();

                            int iLId = Convert.ToInt32(db.LeavePermissionDetails.Max(x => x.Id));

                            for (DateTime date = Convert.ToDateTime(dtpLeaveStartDate.SelectedDate); date <= Convert.ToDateTime(dtpLeaveEndDate.SelectedDate); date = date.AddDays(1))
                            {
                                var da = (from x in db.DailyAttedanceDets where x.EmployeeId == iEmployeeId && (x.IsFullDayLeave == true || x.IsHalfDayLeave == true) && x.IsWeekOff == false && x.IsPublicHoliday == false && x.AttDate == date select x).FirstOrDefault();
                                if (da != null)
                                {
                                    da.WithPermission = true;
                                    da.LeavePermissionId = iLId;
                                    db.SaveChanges();
                                }
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

        private void chkIsPermission_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkIsPermission.IsChecked == true)
                {
                    btnCancel.IsEnabled = true;
                    dgHalf.ItemsSource = null;
                }
                else
                {
                    btnCancel.IsEnabled = false;
                    dgHalf.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
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
                else if (string.IsNullOrEmpty(cmbLeaveTime.Text))
                {
                    MessageBox.Show("Leave Time is Empty!", "Empty");
                    cmbLeaveTime.Focus();
                }
                else
                {
                    bIsValid = false;
                    Validate();
                    if (bIsValid == false)
                    {
                        var lp = (from x in db.LeavePermissionDetails where x.Id == iLeavePermissionId select x).FirstOrDefault();
                        if (lp != null)
                        {
                            lp.IsApproved = false;
                            lp.NoOfDaysApproved = 0;
                            lp.Status = "Rejected";
                            db.SaveChanges();
                        }

                        for (DateTime date = Convert.ToDateTime(dtpLeaveStartDate.SelectedDate); date <= Convert.ToDateTime(dtpLeaveEndDate.SelectedDate); date = date.AddDays(1))
                        {
                            var da = (from x in db.DailyAttedanceDets where x.EmployeeId == iEmployeeId && (x.IsFullDayLeave == true || x.IsHalfDayLeave == true) && x.IsWeekOff == false && x.IsPublicHoliday == false && x.AttDate == date select x).FirstOrDefault();
                            if (da != null)
                            {
                                da.WithPermission = false;
                            }
                        }
                        db.SaveChanges();
                        QueryExec();
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

                if (chkIsPermission.IsChecked == true)
                {
                    sWhere = sWhere + " AND DA.WITHPERMISSION=1 \r";
                    sSelect = ",ISNULL(DA.LEAVEPERMISSIONID,0)LEAVEPERMISSIONID ";
                    sJoin = " LEFT JOIN LEAVEPERMISSIONDETAILS LD(NOLOCK) ON LD.EMPLOYEEID=DA.EMPLOYEEID AND LD.ID=DA.LEAVEPERMISSIONID \r";
                }
                else
                {
                    sWhere = sWhere + " AND DA.WITHPERMISSION=0 \r";
                    sSelect = "";
                    sJoin = "";
                }

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
                        dgHalf.ItemsSource = dtDailyAttedance.DefaultView;
                    }
                    else
                    {
                        dgHalf.ItemsSource = null;
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
                cmbLeaveTime.Text = "Full Day";
                cmbLeaveTime.IsEnabled = false;
                TimeSpan dfDays = (Convert.ToDateTime(dtpLeaveEndDate.SelectedDate) - Convert.ToDateTime(dtpLeaveStartDate.SelectedDate));
                txtTotalNoOfDays.Text = (dfDays.TotalDays + 1).ToString();
            }
            else
            {
                cmbLeaveTime.Text = "Full Day";
                cmbLeaveTime.IsEnabled = true;
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
            cmbLeaveTime.Text = "";
            cmbLeaveType.Text = "";
        }

        void fClear()
        {
            PersonDetailsClear();
            dtpFromDate.Text = "";
            dtpToDate.Text = "";
            dgHalf.ItemsSource = null;
            cmbEmployee.Text = "";
            chkIsPermission.IsChecked = false;
            btnCancel.IsEnabled = false;
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

            var lt = (from x in db.LeaveTypes orderby x.LeaveType1 select x).ToList();
            if (lt != null)
            {
                cmbLeaveType.ItemsSource = lt.ToList();
                cmbLeaveType.SelectedValuePath = "Id";
                cmbLeaveType.DisplayMemberPath = "LeaveType1";
            }
        }

        #endregion       
    }
}
