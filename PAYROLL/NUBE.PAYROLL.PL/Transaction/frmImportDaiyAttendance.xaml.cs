﻿
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
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using NUBE.PAYROLL.CMN;
using System.Data.SqlClient;

namespace NUBE.PAYROLL.PL.Transaction
{
    /// <summary>
    /// Interaction logic for frmImportDaiyAttendance.xaml
    /// </summary>
    public partial class frmImportDaiyAttendance : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        public frmImportDaiyAttendance()
        {
            InitializeComponent();
        }

        #region EVENTS

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            Import();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(dtpDate.SelectedDate);
                var temp = (from x in db.TempAttendanceTimings where x.EntryDate == dt select x).ToList();
                if (temp != null && temp.Count > 0)
                {
                    db.TempAttendanceTimings.RemoveRange(db.TempAttendanceTimings.Where(x => x.EntryDate == dt));
                    db.SaveChanges();
                }
                DataTable dtAttn = ((DataView)dgPunchTime.ItemsSource).ToTable();
                List<TempAttendanceTiming> lsttemp = new List<TempAttendanceTiming>();
                foreach (DataRow dr in dtAttn.Rows)
                {
                    TempAttendanceTiming tmp = new TempAttendanceTiming();
                    tmp.EntryDate = Convert.ToDateTime(dt);
                    tmp.EmployeeID = Convert.ToInt32(dr["EMPLOYEEID"]);
                    tmp.PunchTime = Convert.ToDateTime(dr["PUNCHTIME"]);
                    lsttemp.Add(tmp);
                }
                if (lsttemp != null)
                {
                    db.TempAttendanceTimings.AddRange(lsttemp);
                    db.SaveChanges();
                    DataTable dtDailyAtt = new DataTable();
                    using (SqlConnection con = new SqlConnection(Config.connStr))
                    {
                        SqlCommand cmd;
                        cmd = new SqlCommand("SPINSERTDAILYATTEDANCEDET", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ENTRYDATE", Convert.ToDateTime(dtpDate.SelectedDate).Date));
                        SqlDataAdapter adp = new SqlDataAdapter();
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    MessageBox.Show("Saved Sucessfully");
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(dtpDate.Text))
                {
                    MessageBox.Show("Date is Empty!");
                    dtpDate.Focus();

                }
                else if (MessageBox.Show("Old Data's will be Deleted, Are you sure to Sync the Tump Machine? ", "Clear Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Boolean bIsDatewise = false;
                    if (rbDaily.IsChecked == true)
                    {
                        bIsDatewise = true;
                    }

                    using (SqlConnection con = new SqlConnection(Config.connStr))
                    {
                        SqlCommand cmd;
                        cmd = new SqlCommand("SPTEMPATTENDANCE", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@DATE", string.Format("{0:dd/MMM/yyyy}", dtpDate.SelectedDate)));
                        cmd.Parameters.Add(new SqlParameter("@DAILY", bIsDatewise));
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        con.Open();
                        cmd.CommandTimeout = 0;
                        DataTable dtAttendance = new DataTable();
                        adp.Fill(dtAttendance);

                        if (dtAttendance.Rows.Count > 0)
                        {
                            string sWhere = "";
                            Boolean bIsError = false;
                            String sQuery = " SELECT ID EMPLOYEEID,SHIFTID,ISNULL(UNPAIDLEAVE,0)UNPAIDLEAVE FROM MASTEREMPLOYEE(NOLOCK) \r" +
                                            "-- WHERE ISRESIGNED=0 \r GROUP BY ID,SHIFTID,UNPAIDLEAVE \r" +
                                            " ORDER BY ID ";

                            cmd = new SqlCommand(sQuery, con);
                            cmd.CommandType = CommandType.Text;
                            adp = new SqlDataAdapter(cmd);
                            cmd.CommandTimeout = 0;
                            DataTable dtEmployee = new DataTable();
                            adp.Fill(dtEmployee);
                            List<AttedanceLog> lstAttLog = new List<AttedanceLog>();
                            if (dtEmployee.Rows.Count > 0)
                            {
                                foreach (DataRow drRow in dtEmployee.Rows)
                                {
                                    int iShiftId = Convert.ToInt32(drRow["SHIFTID"]);
                                    var sft = (from x in db.EmployeeShifts where x.Id == iShiftId select x).FirstOrDefault();
                                    if (bIsDatewise == true)
                                    {
                                        sWhere = string.Format(" AND ENTRYDATE='{0:dd/MMM/yyyy}' \r", dtpDate.SelectedDate);
                                    }
                                    else
                                    {
                                        sWhere = string.Format(" AND MONTH(ENTRYDATE)=MONTH('{0:dd/MMM/yyyy}') AND YEAR(ENTRYDATE)=YEAR('{0:dd/MMM/yyyy}') \r", dtpDate.SelectedDate);
                                    }

                                    sQuery = " SELECT ENTRYDATE FROM TEMPATTENDANCETIMINGS(NOLOCK) \r" +
                                            " WHERE EMPLOYEEID=" + drRow["EMPLOYEEID"] + sWhere +
                                            " GROUP BY ENTRYDATE \r" +
                                            " ORDER BY ENTRYDATE ";

                                    cmd = new SqlCommand(sQuery, con);
                                    cmd.CommandType = CommandType.Text;
                                    adp = new SqlDataAdapter(cmd);
                                    cmd.CommandTimeout = 0;
                                    DataTable dtEmp = new DataTable();
                                    adp.Fill(dtEmp);
                                    foreach (DataRow dr in dtEmp.Rows)
                                    {
                                        DataView drv = new DataView(dtAttendance);
                                        drv.RowFilter = string.Format(" EMPLOYEEID=" + drRow["EMPLOYEEID"] + " AND ENTRYDATE='{0:dd/MMM/yyyy}'", Convert.ToDateTime(dr["ENTRYDATE"]));
                                        DataTable dtdate = drv.ToTable();

                                        for (int i = 0; i < dtdate.Rows.Count; i = i + 2)
                                        {
                                            DateTime dtEntry = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", dtdate.Rows[i]["ENTRYDATE"]));
                                            int iEmployeeId = int.Parse(drRow["EMPLOYEEID"].ToString());
                                            var dLog = (from x in db.AttedanceLogs where x.EntryDate == dtEntry && x.EmployeeId == iEmployeeId select x).ToList();
                                            if (dLog.Count > 0)
                                            {
                                                db.AttedanceLogs.RemoveRange(db.AttedanceLogs.Where(x => x.EntryDate == dtEntry && x.EmployeeId == iEmployeeId));
                                                db.SaveChanges();
                                            }

                                            AttedanceLog atLog = new AttedanceLog();
                                            atLog.EmployeeId = iEmployeeId;
                                            atLog.EntryDate = dtEntry;
                                            if (Convert.ToDateTime(sft.OutTime).TimeOfDay > Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]).TimeOfDay)
                                            {
                                                atLog.InTime = Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]);
                                            }
                                            else
                                            {
                                                atLog.OTInTime = Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]);
                                            }
                                            try
                                            {
                                                if (Convert.ToDateTime(sft.OutTime).TimeOfDay > Convert.ToDateTime(dtdate.Rows[i + 1]["PUNCHTIME"]).TimeOfDay)
                                                {
                                                    atLog.OutTime = Convert.ToDateTime(dtdate.Rows[i + 1]["PUNCHTIME"]);
                                                    TimeSpan diff = Convert.ToDateTime(dtdate.Rows[i + 1]["PUNCHTIME"]).TimeOfDay - Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]).TimeOfDay;
                                                    atLog.WorkingHours = Convert.ToDateTime(diff.Hours + ":" + diff.Minutes).TimeOfDay;
                                                }
                                                else if (Convert.ToDateTime(sft.MinimumOtTime).TimeOfDay <= Convert.ToDateTime(dtdate.Rows[i + 1]["PUNCHTIME"]).TimeOfDay)
                                                {
                                                    if (Convert.ToDateTime(sft.OutTime).TimeOfDay > Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]).TimeOfDay)
                                                    {
                                                        atLog.OutTime = Convert.ToDateTime(sft.OutTime);
                                                        TimeSpan diff = Convert.ToDateTime(sft.OutTime).TimeOfDay - Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]).TimeOfDay;
                                                        atLog.WorkingHours = Convert.ToDateTime(diff.Hours + ":" + diff.Minutes).TimeOfDay;

                                                        atLog.OTInTime = Convert.ToDateTime(sft.OutTime);
                                                    }
                                                    atLog.OTOutTime = Convert.ToDateTime(dtdate.Rows[i + 1]["PUNCHTIME"]);
                                                    TimeSpan diffOt = Convert.ToDateTime(dtdate.Rows[i + 1]["PUNCHTIME"]).TimeOfDay - Convert.ToDateTime(sft.OutTime).TimeOfDay;
                                                    atLog.TotalOtHours = Convert.ToDateTime(diffOt.Hours + ":" + diffOt.Minutes).TimeOfDay;
                                                }
                                                else
                                                {
                                                    atLog.OutTime = Convert.ToDateTime(dtdate.Rows[i + 1]["PUNCHTIME"]);
                                                    TimeSpan diff = Convert.ToDateTime(dtdate.Rows[i + 1]["PUNCHTIME"]).TimeOfDay - Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]).TimeOfDay;
                                                    atLog.WorkingHours = Convert.ToDateTime(diff.Hours + ":" + diff.Minutes).TimeOfDay;
                                                }
                                                atLog.IsNotLogOut = false;
                                            }
                                            catch (Exception)
                                            {
                                                if (Convert.ToDateTime(sft.OutTime).TimeOfDay > Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]).TimeOfDay)
                                                {
                                                    atLog.OutTime = Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]);
                                                    TimeSpan diff = Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]).TimeOfDay - Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]).TimeOfDay;
                                                    atLog.WorkingHours = Convert.ToDateTime(diff.Hours + ":" + diff.Minutes).TimeOfDay;
                                                    atLog.IsNotLogOut = true;
                                                }
                                                else if (Convert.ToDateTime(sft.MinimumOtTime).TimeOfDay >= Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]).TimeOfDay)
                                                {
                                                    if (Convert.ToDateTime(sft.OutTime).TimeOfDay > Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]).TimeOfDay)
                                                    {
                                                        atLog.OutTime = Convert.ToDateTime(sft.OutTime);
                                                        TimeSpan diff = Convert.ToDateTime(sft.OutTime).TimeOfDay - Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]).TimeOfDay;
                                                        atLog.WorkingHours = Convert.ToDateTime(diff.Hours + ":" + diff.Minutes).TimeOfDay;

                                                        atLog.OTInTime = Convert.ToDateTime(sft.OutTime);
                                                    }
                                                    atLog.OTOutTime = Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]);
                                                    TimeSpan diffOT = Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]).TimeOfDay - Convert.ToDateTime(sft.OutTime).TimeOfDay;
                                                    atLog.TotalOtHours = Convert.ToDateTime(diffOT.Hours + ":" + diffOT.Minutes).TimeOfDay;
                                                    atLog.IsNotLogOut = true;
                                                }
                                                else
                                                {
                                                    atLog.OutTime = Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]);
                                                    TimeSpan diff = Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]).TimeOfDay - Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]).TimeOfDay;
                                                    atLog.WorkingHours = Convert.ToDateTime(diff.Hours + ":" + diff.Minutes).TimeOfDay;
                                                    atLog.IsNotLogOut = true;
                                                }
                                            }
                                            lstAttLog.Add(atLog);
                                        }
                                    }
                                }

                                if (lstAttLog != null)
                                {
                                    db.AttedanceLogs.AddRange(lstAttLog);
                                    db.SaveChanges();

                                    foreach (DataRow drRow in dtEmployee.Rows)
                                    {
                                        cmd = new SqlCommand("SPDAILYATTEDANCEDET", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@ENTRYDATE", string.Format("{0:dd/MMM/yyyy}", dtpDate.SelectedDate)));
                                        cmd.Parameters.Add(new SqlParameter("@DAILY", bIsDatewise));
                                        cmd.Parameters.Add(new SqlParameter("@EMPLOYEEID", drRow["EMPLOYEEID"]));
                                        cmd.Parameters.Add(new SqlParameter("@SHIFTID", drRow["SHIFTID"]));
                                        cmd.Parameters.Add(new SqlParameter("@ISLOP", drRow["UNPAIDLEAVE"]));
                                        cmd.CommandTimeout = 0;
                                        int i = cmd.ExecuteNonQuery();

                                        if (i == 0)
                                        {
                                            bIsError = true;
                                            MessageBox.Show("Imported Not Execute!", "Error,SPDAILYATTEDANCEDET");
                                        }
                                    }
                                }
                                else
                                {
                                    bIsError = true;
                                }

                                if (bIsError == true)
                                {
                                    MessageBox.Show("Imported Not Execute!,AttedanceLogs Error", "Error,Contact Administrator");
                                }
                                else
                                {
                                    MessageBox.Show("Imported Sucessfully !");
                                }
                            }
                            else
                            {
                                MessageBox.Show("No Record Found !", "Error");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No Record Found !", "Error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //MessageBox.Show("Contact Administrator!", "Error");
                //MessageBox.Show("Imported Sucessfully !", "Sucessfully Completed");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you Want to Clear this Data's?", "Clear Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        #endregion

        #region FUNCTIONS

        void Import()
        {
            DataTable dtTemp = new DataTable();
            OpenFileDialog OpenDialogBox = new OpenFileDialog();
            OpenDialogBox.DefaultExt = ".xlsx";
            OpenDialogBox.Filter = "XL files|*.xls;*.xlsx|All files|*.*";
            var browsefile = OpenDialogBox.ShowDialog();
            if (browsefile == true)
            {
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook excelBook = excelApp.Workbooks.Open(OpenDialogBox.FileName.ToString(), 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Excel.Worksheet excelSheet = (Excel.Worksheet)excelBook.Worksheets.get_Item(1);
                Excel.Range excelRange = excelSheet.UsedRange;

                progressBar1.Minimum = 5;
                progressBar1.Maximum = excelRange.Rows.Count;
                progressBar1.Visibility = Visibility.Visible;

                int rowCnt = 0;
                int colCnt = 0;
                string strCellData = "";
                double douCellData;
                try
                {
                    dtpDate.Text = (string)(excelRange.Cells[2, 4] as Excel.Range).Value2;
                }
                catch (Exception)
                {
                    douCellData = (excelRange.Cells[2, 4] as Excel.Range).Value2;
                    dtpDate.SelectedDate = DateTime.FromOADate(douCellData);

                }

                for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                {
                    string strColumn = "";
                    strColumn = (string)(excelRange.Cells[1, colCnt] as Excel.Range).Value2;
                    dtTemp.Columns.Add(strColumn.ToUpper(), typeof(string));
                }
                dtTemp.Columns.Add("NAME", typeof(string));
                dtTemp.Columns.Add("SNO", typeof(string));
                dtTemp.Columns.Add("GENDER", typeof(string));
                dtTemp.Columns.Add("EMPLOYEEID", typeof(string));

                for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
                {
                    string strData = "";
                    for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                    {
                        try
                        {
                            strCellData = (string)(excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                            strData += strCellData + "|";
                        }
                        catch (Exception)
                        {
                            if (colCnt == 3 || colCnt == 4)
                            {
                                douCellData = (excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                                strData += DateTime.FromOADate(douCellData).ToString() + "|";
                            }
                            else
                            {
                                douCellData = (excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                                strData += douCellData.ToString() + "|";
                            }
                        }
                    }
                    strData = strData.Remove(strData.Length - 1, 1);
                    dtTemp.Rows.Add(strData.Split('|'));
                }
                int i = 0;
                foreach (DataRow dr in dtTemp.Rows)
                {
                    i++;
                    int iMembershipNo = Convert.ToInt32(dr["MEMBERCODE"]);
                    var emp = (from x in db.MasterEmployees where x.MembershipNo == iMembershipNo select x).FirstOrDefault();
                    if (emp != null)
                    {
                        dr["NAME"] = emp.EmployeeName;
                        dr["GENDER"] = emp.Gender;
                        dr["EMPLOYEEID"] = emp.Id;
                        dr["SNO"] = i;
                    }
                }
                dgPunchTime.ItemsSource = dtTemp.DefaultView;
            }
        }

        void ClearForm()
        {
            dgPunchTime.ItemsSource = null;
            dtpDate.Text = "";
        }

        #endregion


    }
}
