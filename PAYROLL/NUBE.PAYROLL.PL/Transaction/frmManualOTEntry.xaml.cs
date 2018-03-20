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
    /// Interaction logic for frmManualOTEntry.xaml
    /// </summary>
    public partial class frmManualOTEntry : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        DataTable dtDailyAttedance = new DataTable();
        Boolean bIsValid = false;
        public frmManualOTEntry()
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
            try
            {
                QueryExec();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bIsValid = false;
                Validate();
                if (bIsValid == false)
                {
                    int iEmpId = Convert.ToInt32(cmbEmployeeOTEntry.SelectedValue);
                    using (SqlConnection con = new SqlConnection(Config.connStr))
                    {
                        string str = string.Format(" SELECT ID,OTMONTH,EMPLOYEEID FROM MANUALOTENTRY OT(NOLOCK) \r " +
                                                   " WHERE MONTH(OT.OTMONTH)=MONTH('{0:dd/MMM/yyyy}') AND YEAR(OT.OTMONTH)=YEAR('{0:dd/MMM/yyyy}')  AND OT.EMPLOYEEID={1} ", dtpOTMonth.SelectedDate, iEmpId);
                        SqlCommand cmd = new SqlCommand(str, con);
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        con.Open();
                        cmd.CommandTimeout = 0;
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            int iId = Convert.ToInt32(dt.Rows[0]["ID"]);
                            var OT = (from x in db.ManualOTEntries where x.Id == iId select x).FirstOrDefault();
                            if (OT != null)
                            {
                                OT.TotalNoOfHours = Convert.ToDecimal(txtTotalHours.Text);
                                OT.Remarks = txtRemarks.Text;
                                OT.OTMonth = Convert.ToDateTime(dtpOTMonth.SelectedDate);
                                OT.UpdatedBy = 1;
                                OT.UpdatedOn = DateTime.Today;
                                db.SaveChanges();
                                PersonDetailsClear();
                                QueryExec();
                            }
                        }
                        else
                        {
                            ManualOTEntry OT = new ManualOTEntry();
                            OT.OTMonth = Convert.ToDateTime(dtpOTMonth.SelectedDate);
                            OT.EmployeeId = iEmpId;
                            OT.EmployeeName = cmbEmployeeOTEntry.Text;
                            OT.TotalNoOfHours = Convert.ToDecimal(txtTotalHours.Text);
                            OT.Remarks = txtRemarks.Text;
                            OT.CreatedBy = 1;
                            OT.CreatedOn = DateTime.Today;
                            db.ManualOTEntries.Add(OT);
                            db.SaveChanges();
                            PersonDetailsClear();
                            QueryExec();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgOTEntry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                PersonDetailsClear();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgOTEntry_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgOTEntry.SelectedItem != null))
                {
                    PersonDetailsClear();
                    DataRowView drv = (DataRowView)dgOTEntry.SelectedItem;
                    txtEmployeeNo.Text = drv["MEMBERSHIPNO"].ToString();
                    dtpOTMonth.SelectedDate = Convert.ToDateTime(drv["OTMONTH"]).Date;
                    cmbEmployee.Text = drv["EMPLOYEENAME"].ToString();
                    txtRemarks.Text = drv["REMARKS"].ToString();
                    txtTotalHours.Text = drv["TOTALNOOFHOURS"].ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void cmbEmployeeOTEntry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbEmployeeOTEntry.Text))
                {
                    int iEmpId = Convert.ToInt32(cmbEmployeeOTEntry.SelectedValue);

                    var emp = (from x in db.MasterEmployees where x.Id == iEmpId select x).FirstOrDefault();
                    if (emp != null)
                    {
                        txtEmployeeNo.Text = emp.MembershipNo.ToString();
                        txtRemarks.Text = "";
                        txtTotalHours.Text = "0";
                        if (!string.IsNullOrEmpty(dtpEntryDate.Text))
                        {
                            dtpOTMonth.SelectedDate = dtpEntryDate.SelectedDate;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Records Found!");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void txtEmployeeNo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter && !string.IsNullOrEmpty(txtEmployeeNo.Text))
                {
                    int iEmpId = Convert.ToInt32(txtEmployeeNo.Text);

                    var emp = (from x in db.MasterEmployees where x.MembershipNo == iEmpId select x).FirstOrDefault();
                    if (emp != null)
                    {
                        cmbEmployee.SelectedValue = emp.Id;
                        txtRemarks.Text = "";
                        txtTotalHours.Text = "0";
                        if (!string.IsNullOrEmpty(dtpEntryDate.Text))
                        {
                            dtpOTMonth.SelectedDate = dtpEntryDate.SelectedDate;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Records Found!");
                        txtEmployeeNo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        #endregion

        #region FUNCTIONS

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

        void PersonDetailsClear()
        {
            cmbEmployeeOTEntry.Text = "";
            txtEmployeeNo.Text = "";
            txtRemarks.Text = "";
            txtTotalHours.Text = "";
            dtpOTMonth.Text = "";
        }

        void fClear()
        {
            PersonDetailsClear();
            dtpEntryDate.Text = "";
            dgOTEntry.ItemsSource = null;
            cmbEmployee.Text = "";
            bIsValid = false;
        }

        void QueryExec()
        {
            try
            {
                string sWhere = "";
                PersonDetailsClear();

                if (!string.IsNullOrEmpty(cmbEmployee.Text))
                {
                    sWhere = " AND OT.EMPLOYEEID=" + Convert.ToInt32(cmbEmployee.SelectedValue);
                }


                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    string str = string.Format(" SELECT OT.EMPLOYEEID,OTMONTH,EM.MEMBERSHIPNO,EM.EMPLOYEENAME,OT.TOTALNOOFHOURS,ISNULL(OT.REMARKS,'')REMARKS \r" +
                                               " FROM MANUALOTENTRY OT(NOLOCK) \r " +
                                               " LEFT JOIN MASTEREMPLOYEE EM(NOLOCK) ON EM.ID = OT.EMPLOYEEID \r" +
                                               " WHERE MONTH(OT.OTMONTH)=MONTH('{0:dd/MMM/yyyy}') AND YEAR(OT.OTMONTH)=YEAR('{1:dd/MMM/yyyy}') \r" + sWhere +
                                               " ORDER BY EM.EMPLOYEENAME ", dtpEntryDate.SelectedDate);
                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    con.Open();
                    cmd.CommandTimeout = 0;
                    dtDailyAttedance.Rows.Clear();
                    adp.Fill(dtDailyAttedance);
                    if (dtDailyAttedance.Rows.Count > 0)
                    {
                        dgOTEntry.ItemsSource = dtDailyAttedance.DefaultView;
                    }
                    else
                    {
                        dgOTEntry.ItemsSource = null;
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
            if (!string.IsNullOrEmpty(txtEmployeeNo.Text))
            {
                MessageBox.Show("Employee No is Empty");
                txtEmployeeNo.Focus();
                bIsValid = true;
            }
            else if (!string.IsNullOrEmpty(cmbEmployeeOTEntry.Text))
            {
                MessageBox.Show("Employee Name is Empty");
                cmbEmployeeOTEntry.Focus();
                bIsValid = true;
            }
            else if (!string.IsNullOrEmpty(txtTotalHours.Text))
            {
                MessageBox.Show("Total No.of Hours is Empty");
                txtTotalHours.Focus();
                bIsValid = true;
            }
            else if (!string.IsNullOrEmpty(dtpOTMonth.Text))
            {
                MessageBox.Show("OT Month is Empty");
                dtpOTMonth.Focus();
                bIsValid = true;
            }
        }

        #endregion       
    }
}
