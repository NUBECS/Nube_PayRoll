using Microsoft.Win32;
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
using Microsoft.Reporting.WinForms;

namespace NUBE.PAYROLL.PL.Transaction
{
    /// <summary>
    /// Interaction logic for frmDailyAttedance.xaml
    /// </summary>
    public partial class frmDailyAttedance : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        public frmDailyAttedance()
        {
            InitializeComponent();
            try
            {
                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    string str = " SELECT EM.ID,EM.EMPLOYEENAME \r" +
                                 " FROM MASTEREMPLOYEE EM(NOLOCK) \r " +
                                 " WHERE EM.ISCANCEL=0 \r " +
                                 " ORDER BY EM.EMPLOYEENAME ";
                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    con.Open();
                    cmd.CommandTimeout = 0;
                    DataTable dtEmployee = new DataTable();
                    adp.Fill(dtEmployee);
                    if (dtEmployee.Rows.Count > 0)
                    {
                        cmbEmployee.ItemsSource = dtEmployee.DefaultView;
                        cmbEmployee.SelectedValuePath = "ID";
                        cmbEmployee.DisplayMemberPath = "EMPLOYEENAME";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        #region EVENTS

        private void dtpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DailyAttedance_Loaded(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            tbGPresent.Visibility = Visibility.Visible;
            tbGHalfDay.Visibility = Visibility.Visible;
            tbGLateCommers.Visibility = Visibility.Visible;
            tbGOT.Visibility = Visibility.Visible;
            tbGLeave.Visibility = Visibility.Visible;
            tbPresent.Visibility = Visibility.Collapsed;
            tbHalfDate.Visibility = Visibility.Collapsed; ;
            tbLate.Visibility = Visibility.Collapsed;
            tbOT.Visibility = Visibility.Collapsed;
            tbLeave.Visibility = Visibility.Collapsed;
            tbGPresent.IsSelected = true;
            try
            {
                if (string.IsNullOrEmpty(dtpDate.Text))
                {
                    MessageBox.Show("From Date is Empty!");
                    dtpDate.Focus();
                }
                else if (string.IsNullOrEmpty(dtpToDate.Text))
                {
                    MessageBox.Show("To Date is Empty!");
                    dtpToDate.Focus();
                }
                else if ((dtpDate.SelectedDate) > (dtpToDate.SelectedDate))
                {
                    MessageBox.Show("From Date is Must be Less Than To Date !");
                    dtpToDate.Focus();
                }
                else
                {
                    LoadAttedance();
                }
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
                ClearForm();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Function

        void LoadAttedance()
        {
            DateTime dtdate = Convert.ToDateTime(dtpDate.SelectedDate);
            DateTime dtTodate = Convert.ToDateTime(dtpToDate.SelectedDate);

            DataTable dt = new DataTable();
            DataTable dtLate = new DataTable();

            string sWhere = "";
            if (!string.IsNullOrEmpty(cmbEmployee.Text))
            {
                sWhere = sWhere + " AND DA.EMPLOYEEID=" + Convert.ToInt32(cmbEmployee.SelectedValue);
            }
            else
            {
                sWhere = "";
            }

            using (SqlConnection con = new SqlConnection(Config.connStr))
            {
                string str = string.Format(" SELECT ROW_NUMBER() OVER(ORDER BY EMPLOYEENAME,DA.ATTDATE ASC) AS RNO,DA.EMPLOYEEID,ME.MEMBERSHIPNO,ME.EMPLOYEENAME,\r" +
                                           " ISNULL(MP.POSITIONNAME, '')POSITIONNAME, CONVERT(VARCHAR, DA.ATTDATE, 105) ATTDATE, CAST(DA.INTIME AS TIME)INTIME, CAST(DA.OUTTIME AS TIME)OUTTIME,\r" +
                                           " ((ISNULL(DA.TOTALWORKING_HOURS, 0)) + DBO.MINUTES_TO_HOUR((ISNULL(DA.TOTALWORKING_MINUTES, 0))))WORKING_HOURS,\r" +
                                           " CONVERT(NUMERIC(18, 2), '0.' + DBO.MINUTES_TO_MINUTES((ISNULL(DA.TOTALWORKING_MINUTES, 0))))WORKING_MINUTES,\r" +
                                           " DA.WITHPERMISSION, ((ISNULL(DA.OT_HOURS, 0)) + DBO.MINUTES_TO_HOUR((ISNULL(DA.OT_MINUTES, 0))))OT_HOURS,\r" +
                                           " DBO.MINUTES_TO_MINUTES((ISNULL(DA.OT_MINUTES, 0))) OT_MINUTES,\r" +
                                           " ISNULL(DA.REMARKS, '')REMARKS, DA.ISFULLDAYLEAVE, DA.ISHALFDAYLEAVE\r" +
                                           " FROM DAILYATTEDANCEDET DA(NOLOCK)\r" +
                                           " LEFT JOIN MASTEREMPLOYEE ME(NOLOCK) ON ME.ID = DA.EMPLOYEEID\r" +
                                           " LEFT JOIN MASTERPOSITION MP(NOLOCK) ON MP.ID = ME.POSITIONID\r" +
                                           " WHERE ATTDATE BETWEEN '{0:dd/MMM/yyyy}' AND '{1:dd/MMM/yyyy}' AND DA.ISPUBLICHOLIDAY=0 AND DA.ISWEEKOFF=0 \r " + sWhere, dtdate, dtTodate);
                SqlCommand cmd = new SqlCommand(str, con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandTimeout = 0;
                adp.Fill(dt);

                string strLate = string.Format(" SELECT ROW_NUMBER() OVER(ORDER BY EMPLOYEENAME,DA.ATTDATE ASC) AS RNO,DA.EMPLOYEEID,ME.MEMBERSHIPNO,ME.EMPLOYEENAME,\r" +
                                               " ISNULL(MP.POSITIONNAME, '')POSITIONNAME, CONVERT(VARCHAR, DA.ATTDATE, 105) ATTDATE, CAST(DA.INTIME AS TIME)INTIME, CAST(DA.OUTTIME AS TIME)OUTTIME,\r" +
                                               " (SUM(ISNULL(DA.TOTALWORKING_HOURS, 0)) + DBO.MINUTES_TO_HOUR(SUM(ISNULL(DA.TOTALWORKING_MINUTES, 0))))WORKING_HOURS,\r" +
                                               " CONVERT(NUMERIC(18, 2), '0.' + DBO.MINUTES_TO_MINUTES(SUM(ISNULL(DA.TOTALWORKING_MINUTES, 0))))WORKING_MINUTES,\r" +
                                               " DA.WITHPERMISSION,(SUM(ISNULL(DA.OT_HOURS, 0)) + DBO.MINUTES_TO_HOUR(SUM(ISNULL(DA.OT_MINUTES, 0))))OT_HOURS,\r" +
                                               " DBO.MINUTES_TO_MINUTES(SUM(ISNULL(DA.OT_MINUTES, 0))) OT_MINUTES,\r" +
                                               " ISNULL(DA.REMARKS, '')REMARKS, DA.ISFULLDAYLEAVE, DA.ISHALFDAYLEAVE,\r" +
                                               " DBO.MINUTES_TO_HOUR(ISNULL(LT.LATEMINUTES, 0)) + '.' + DBO.MINUTES_TO_MINUTES(ISNULL(LT.LATEMINUTES, 0))LATEHOURS\r" +
                                               " FROM DAILYATTEDANCEDET DA(NOLOCK)\r" +
                                               " LEFT JOIN MASTEREMPLOYEE ME(NOLOCK) ON ME.ID = DA.EMPLOYEEID\r" +
                                               " LEFT JOIN EMPLOYEESHIFT ES(NOLOCK) ON ES.ID = ME.SHIFTID\r" +
                                               " LEFT JOIN MASTERPOSITION MP(NOLOCK) ON MP.ID = ME.POSITIONID\r" +
                                               " LEFT JOIN DAILYLATELOGS LT(NOLOCK) ON LT.EMPLOYEEID = da.EmployeeId and LT.EntryDate = da.AttDate\r" +
                                               " WHERE ISNULL(LT.LATEMINUTES, 0) > 0 AND DA.ATTDATE BETWEEN '{0:dd/MMM/yyyy}' AND '{1:dd/MMM/yyyy}' AND DA.ISPUBLICHOLIDAY=0 AND DA.ISWEEKOFF=0 \r " + sWhere +
                                               " GROUP BY DA.EMPLOYEEID, ME.MEMBERSHIPNO, ME.EMPLOYEENAME, MP.POSITIONNAME, DA.ATTDATE, DA.INTIME, DA.OUTTIME, LT.LATEMINUTES,\r" +
                                               " DA.WITHPERMISSION, DA.REMARKS, DA.ISFULLDAYLEAVE, DA.ISHALFDAYLEAVE\r", dtdate, dtTodate);
                SqlCommand cmsd = new SqlCommand(strLate, con);
                SqlDataAdapter adps = new SqlDataAdapter(cmsd);
                cmsd.CommandTimeout = 0;
                adps.Fill(dtLate);
            }

            //var att = (from x in db.ViewDailyAttedances where x.ATTDATE >= dtdate && x.ATTDATE <= dtTodate && x. == iEId select x).ToList();
            //var late = (from x in db.VIEWDAILYATTEDANCELATEs where x.ATTDATE >= dtdate && x.ATTDATE <= dtTodate && x.EMPLOYEEID == iEId select x).ToList();
            //dt = AppLib.LINQResultToDataTable(att);
            //dtLate = AppLib.LINQResultToDataTable(late);

            if (dt.Rows.Count > 0)
            {
                DataTable dtPresent = new DataTable();
                DataTable dtLeave = new DataTable();
                DataTable dtHalf = new DataTable();
                DataTable dtOT = new DataTable();

                DataView dv = new DataView(dt);
                dv.RowFilter = "ISFULLDAYLEAVE<>1";
                dtPresent = dv.ToTable();

                dv = new DataView(dt);
                dv.RowFilter = "ISFULLDAYLEAVE<>0";
                dtLeave = dv.ToTable();

                dv = new DataView(dt);
                dv.RowFilter = "ISHALFDAYLEAVE<>0";
                dtHalf = dv.ToTable();

                dv = new DataView(dt);
                dv.RowFilter = " OT_HOURS>0 OR OT_MINUTES>0";
                dtOT = dv.ToTable();

                int i = 0;
                foreach (DataRow dr in dtPresent.Rows)
                {
                    i++;
                    dr["RNO"] = i;
                }
                dgPresent.ItemsSource = dtPresent.DefaultView;

                i = 0;
                foreach (DataRow dr in dtLeave.Rows)
                {
                    i++;
                    dr["RNO"] = i;
                }
                dgLeave.ItemsSource = dtLeave.DefaultView;

                i = 0;
                foreach (DataRow dr in dtHalf.Rows)
                {
                    i++;
                    dr["RNO"] = i;
                }
                dgHalf.ItemsSource = dtHalf.DefaultView;
                dgLatecommer.ItemsSource = dtLate.DefaultView;
                i = 0;

                foreach (DataRow dr in dtOT.Rows)
                {
                    i++;
                    dr["RNO"] = i;
                }
                dgOT.ItemsSource = dtOT.DefaultView;

                if (dt.Rows.Count > 0)
                {
                    txtTotalMember.Text = dt.Rows.Count.ToString();
                }
                else
                {
                    txtTotalMember.Text = "0";
                }

                if (dtPresent.Rows.Count > 0)
                {
                    txtPresent.Text = dtPresent.Rows.Count.ToString();
                }
                else
                {
                    txtPresent.Text = "0";
                }

                if (dtLeave.Rows.Count > 0)
                {
                    txtLeave.Text = dtLeave.Rows.Count.ToString();
                }
                else
                {
                    txtLeave.Text = "0";
                }

                if (dtHalf.Rows.Count > 0)
                {
                    txtHalfDayLeave.Text = dtHalf.Rows.Count.ToString();
                }
                else
                {
                    txtHalfDayLeave.Text = "0";
                }

                if (dtLate.Rows.Count > 0)
                {
                    txtLate.Text = dtLate.Rows.Count.ToString();
                }
                else
                {
                    txtLate.Text = "0";
                }

                if (dtOT.Rows.Count > 0)
                {
                    txtOT.Text = dtOT.Rows.Count.ToString();
                }
                else
                {
                    txtOT.Text = "0";
                }
            }
            else
            {
                MessageBox.Show("No Records Found");
            }
        }
        void LoadAttedance1()
        {
            DateTime dtdate = Convert.ToDateTime(dtpDate.SelectedDate);
            DateTime dtTodate = Convert.ToDateTime(dtpToDate.SelectedDate);

            DataTable dt = new DataTable();
            DataTable dtLate = new DataTable();

            string sWhere = "";
            if (!string.IsNullOrEmpty(cmbEmployee.Text))
            {
                sWhere = sWhere + " AND DA.EMPLOYEEID=" + Convert.ToInt32(cmbEmployee.SelectedValue);
            }
            else
            {
                sWhere = "";
            }

            using (SqlConnection con = new SqlConnection(Config.connStr))
            {
                string str = string.Format(" SELECT ROW_NUMBER() OVER(ORDER BY EMPLOYEENAME,DA.ATTDATE ASC) AS RNO,DA.EMPLOYEEID,ME.MEMBERSHIPNO,ME.EMPLOYEENAME,\r" +
                                           " ISNULL(MP.POSITIONNAME, '')POSITIONNAME, CONVERT(VARCHAR, DA.ATTDATE, 105) ATTDATE, CAST(DA.INTIME AS TIME)INTIME, CAST(DA.OUTTIME AS TIME)OUTTIME,\r" +
                                           " ((ISNULL(DA.TOTALWORKING_HOURS, 0)) + DBO.MINUTES_TO_HOUR((ISNULL(DA.TOTALWORKING_MINUTES, 0))))WORKING_HOURS,\r" +
                                           " CONVERT(NUMERIC(18, 2), '0.' + DBO.MINUTES_TO_MINUTES((ISNULL(DA.TOTALWORKING_MINUTES, 0))))WORKING_MINUTES,\r" +
                                           " DA.WITHPERMISSION, ((ISNULL(DA.OT_HOURS, 0)) + DBO.MINUTES_TO_HOUR((ISNULL(DA.OT_MINUTES, 0))))OT_HOURS,\r" +
                                           " DBO.MINUTES_TO_MINUTES((ISNULL(DA.OT_MINUTES, 0))) OT_MINUTES,\r" +
                                           " ISNULL(DA.REMARKS, '')REMARKS, DA.ISFULLDAYLEAVE, DA.ISHALFDAYLEAVE\r" +
                                           " FROM DAILYATTEDANCEDET DA(NOLOCK)\r" +
                                           " LEFT JOIN MASTEREMPLOYEE ME(NOLOCK) ON ME.ID = DA.EMPLOYEEID\r" +
                                           " LEFT JOIN MASTERPOSITION MP(NOLOCK) ON MP.ID = ME.POSITIONID\r" +
                                           " WHERE ATTDATE BETWEEN '{0:dd/MMM/yyyy}' AND '{1:dd/MMM/yyyy}' AND DA.ISPUBLICHOLIDAY=0 AND DA.ISWEEKOFF=0 \r " + sWhere, dtdate, dtTodate);
                SqlCommand cmd = new SqlCommand(str, con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                con.Open();
                cmd.CommandTimeout = 0;
                adp.Fill(dt);

                string strLate = string.Format(" SELECT ROW_NUMBER() OVER(ORDER BY EMPLOYEENAME,DA.ATTDATE ASC) AS RNO,DA.EMPLOYEEID,ME.MEMBERSHIPNO,ME.EMPLOYEENAME,\r" +
                                               " ISNULL(MP.POSITIONNAME, '')POSITIONNAME, CONVERT(VARCHAR, DA.ATTDATE, 105) ATTDATE, CAST(DA.INTIME AS TIME)INTIME, CAST(DA.OUTTIME AS TIME)OUTTIME,\r" +
                                               " (SUM(ISNULL(DA.TOTALWORKING_HOURS, 0)) + DBO.MINUTES_TO_HOUR(SUM(ISNULL(DA.TOTALWORKING_MINUTES, 0))))WORKING_HOURS,\r" +
                                               " CONVERT(NUMERIC(18, 2), '0.' + DBO.MINUTES_TO_MINUTES(SUM(ISNULL(DA.TOTALWORKING_MINUTES, 0))))WORKING_MINUTES,\r" +
                                               " DA.WITHPERMISSION,(SUM(ISNULL(DA.OT_HOURS, 0)) + DBO.MINUTES_TO_HOUR(SUM(ISNULL(DA.OT_MINUTES, 0))))OT_HOURS,\r" +
                                               " DBO.MINUTES_TO_MINUTES(SUM(ISNULL(DA.OT_MINUTES, 0))) OT_MINUTES,\r" +
                                               " ISNULL(DA.REMARKS, '')REMARKS, DA.ISFULLDAYLEAVE, DA.ISHALFDAYLEAVE,\r" +
                                               " DBO.MINUTES_TO_HOUR(ISNULL(LT.LATEMINUTES, 0)) + '.' + DBO.MINUTES_TO_MINUTES(ISNULL(LT.LATEMINUTES, 0))LATEHOURS\r" +
                                               " FROM DAILYATTEDANCEDET DA(NOLOCK)\r" +
                                               " LEFT JOIN MASTEREMPLOYEE ME(NOLOCK) ON ME.ID = DA.EMPLOYEEID\r" +
                                               " LEFT JOIN EMPLOYEESHIFT ES(NOLOCK) ON ES.ID = ME.SHIFTID\r" +
                                               " LEFT JOIN MASTERPOSITION MP(NOLOCK) ON MP.ID = ME.POSITIONID\r" +
                                               " LEFT JOIN DAILYLATELOGS LT(NOLOCK) ON LT.EMPLOYEEID = da.EmployeeId and LT.EntryDate = da.AttDate\r" +
                                               " WHERE ISNULL(LT.LATEMINUTES, 0) > 0 AND DA.ATTDATE BETWEEN '{0:dd/MMM/yyyy}' AND '{1:dd/MMM/yyyy}' AND DA.ISPUBLICHOLIDAY=0 AND DA.ISWEEKOFF=0 \r " + sWhere +
                                               " GROUP BY DA.EMPLOYEEID, ME.MEMBERSHIPNO, ME.EMPLOYEENAME, MP.POSITIONNAME, DA.ATTDATE, DA.INTIME, DA.OUTTIME, LT.LATEMINUTES,\r" +
                                               " DA.WITHPERMISSION, DA.REMARKS, DA.ISFULLDAYLEAVE, DA.ISHALFDAYLEAVE\r", dtdate, dtTodate);
                SqlCommand cmsd = new SqlCommand(strLate, con);
                SqlDataAdapter adps = new SqlDataAdapter(cmsd);
                cmsd.CommandTimeout = 0;
                adps.Fill(dtLate);
            }

            //var att = (from x in db.ViewDailyAttedances where x.ATTDATE >= dtdate && x.ATTDATE <= dtTodate && x. == iEId select x).ToList();
            //var late = (from x in db.VIEWDAILYATTEDANCELATEs where x.ATTDATE >= dtdate && x.ATTDATE <= dtTodate && x.EMPLOYEEID == iEId select x).ToList();
            //dt = AppLib.LINQResultToDataTable(att);
            //dtLate = AppLib.LINQResultToDataTable(late);

            if (dt.Rows.Count > 0)
            {

                DataTable dtPresent = new DataTable();
                DataTable dtLeave = new DataTable();
                DataTable dtHalf = new DataTable();
                DataTable dtOT = new DataTable();
                DataView dv = new DataView(dt);
                dv.RowFilter = "ISFULLDAYLEAVE<>1";
                dtPresent = dv.ToTable();
                rptPresentReport.Reset();
                ReportDataSource PRESENT = new ReportDataSource("Present", dtPresent);
                rptPresentReport.LocalReport.DataSources.Add(PRESENT);
                rptPresentReport.LocalReport.ReportEmbeddedResource = "NUBE.PAYROLL.PL.Reports.AttendanceReport.rptPresentReport.rdlc";
                rptPresentReport.RefreshReport();

                dv = new DataView(dt);
                dv.RowFilter = "ISFULLDAYLEAVE<>0";
                dtLeave = dv.ToTable();
                rptLeaveReport.Reset();
                ReportDataSource LEAVE = new ReportDataSource("Leave", dtLeave);
                rptLeaveReport.LocalReport.DataSources.Add(LEAVE);
                rptLeaveReport.LocalReport.ReportEmbeddedResource = "NUBE.PAYROLL.PL.Reports.AttendanceReport.rptLeaveReport.rdlc";
                rptLeaveReport.RefreshReport();

                dv = new DataView(dt);
                dv.RowFilter = "ISHALFDAYLEAVE<>0";
                dtHalf = dv.ToTable();
                rptHalfDayReport.Reset();
                ReportDataSource halfday = new ReportDataSource("HalfDay", dtHalf);
                rptHalfDayReport.LocalReport.DataSources.Add(halfday);
                rptHalfDayReport.LocalReport.ReportEmbeddedResource = "NUBE.PAYROLL.PL.Reports.AttendanceReport.rptHalfDayReport.rdlc";
                rptHalfDayReport.RefreshReport();

                dv = new DataView(dt);
                dv.RowFilter = " OT_HOURS>0 OR OT_MINUTES>0";
                dtOT = dv.ToTable();
                rptOTReport.Reset();
                ReportDataSource OT = new ReportDataSource("OT", dtOT);
                rptOTReport.LocalReport.DataSources.Add(OT);
                rptOTReport.LocalReport.ReportEmbeddedResource = "NUBE.PAYROLL.PL.Reports.AttendanceReport.rptOTReport.rdlc";
                rptOTReport.RefreshReport();


                rptLateCommers.Reset();
                ReportDataSource late = new ReportDataSource("LateCommers", dtLate);
                rptLateCommers.LocalReport.DataSources.Add(late);
                rptLateCommers.LocalReport.ReportEmbeddedResource = "NUBE.PAYROLL.PL.Reports.AttendanceReport.rptLateCommer's.rdlc";
                rptLateCommers.RefreshReport();


                int i = 0;
                foreach (DataRow dr in dtPresent.Rows)
                {
                    i++;
                    dr["RNO"] = i;
                }
                dgPresent.ItemsSource = dtPresent.DefaultView;

                i = 0;
                foreach (DataRow dr in dtLeave.Rows)
                {
                    i++;
                    dr["RNO"] = i;
                }
                dgLeave.ItemsSource = dtLeave.DefaultView;

                i = 0;
                foreach (DataRow dr in dtHalf.Rows)
                {
                    i++;
                    dr["RNO"] = i;
                }
                dgHalf.ItemsSource = dtHalf.DefaultView;
                dgLatecommer.ItemsSource = dtLate.DefaultView;
                i = 0;

                foreach (DataRow dr in dtOT.Rows)
                {
                    i++;
                    dr["RNO"] = i;
                }
                dgOT.ItemsSource = dtOT.DefaultView;

                if (dt.Rows.Count > 0)
                {
                    txtTotalMember.Text = dt.Rows.Count.ToString();
                }
                else
                {
                    txtTotalMember.Text = "0";
                }

                if (dtPresent.Rows.Count > 0)
                {
                    txtPresent.Text = dtPresent.Rows.Count.ToString();
                }
                else
                {
                    txtPresent.Text = "0";
                }

                if (dtLeave.Rows.Count > 0)
                {
                    txtLeave.Text = dtLeave.Rows.Count.ToString();
                }
                else
                {
                    txtLeave.Text = "0";
                }

                if (dtHalf.Rows.Count > 0)
                {
                    txtHalfDayLeave.Text = dtHalf.Rows.Count.ToString();
                }
                else
                {
                    txtHalfDayLeave.Text = "0";
                }

                if (dtLate.Rows.Count > 0)
                {
                    txtLate.Text = dtLate.Rows.Count.ToString();
                }
                else
                {
                    txtLate.Text = "0";
                }

                if (dtOT.Rows.Count > 0)
                {
                    txtOT.Text = dtOT.Rows.Count.ToString();
                }
                else
                {
                    txtOT.Text = "0";
                }
            }
            else
            {
                MessageBox.Show("No Records Found");
            }
        }

        void ClearForm()
        {
            dtpDate.Text = "";
            dtpToDate.Text = "";
            cmbEmployee.Text = "";
            dgPresent.ItemsSource = null;
            dgLeave.ItemsSource = null;
            dgHalf.ItemsSource = null;
            dgOT.ItemsSource = null;
            dgLatecommer.ItemsSource = null;
            txtTotalMember.Text = "";
            txtPresent.Text = "";
            txtLeave.Text = "";
            txtHalfDayLeave.Text = "";
            txtLeave.Text = "";
            txtOT.Text = "";
            rptHalfDayReport.Clear();
            rptLateCommers.Clear();
            rptLeaveReport.Clear();
            rptOTReport.Clear();

        }

        #endregion

        private void btnPrintView_Click(object sender, RoutedEventArgs e)
        {
            tbGPresent.Visibility = Visibility.Collapsed;
            tbGHalfDay.Visibility = Visibility.Collapsed;
            tbGLateCommers.Visibility = Visibility.Collapsed;
            tbGOT.Visibility = Visibility.Collapsed;
            tbGLeave.Visibility = Visibility.Collapsed;
            tbPresent.Visibility = Visibility.Visible;
            tbHalfDate.Visibility = Visibility.Visible;
            tbLate.Visibility = Visibility.Visible;
            tbOT.Visibility = Visibility.Visible;
            tbLeave.Visibility = Visibility.Visible;
            tbPresent.IsSelected = true;
            try
            {
                if (string.IsNullOrEmpty(dtpDate.Text))
                {
                    MessageBox.Show("From Date is Empty!");
                    dtpDate.Focus();
                }
                else if (string.IsNullOrEmpty(dtpToDate.Text))
                {
                    MessageBox.Show("To Date is Empty!");
                    dtpToDate.Focus();
                }
                else if ((dtpDate.SelectedDate) > (dtpToDate.SelectedDate))
                {
                    MessageBox.Show("From Date is Must be Less Than To Date !");
                    dtpToDate.Focus();
                }
                else
                {
                    LoadAttedance1();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgOT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
