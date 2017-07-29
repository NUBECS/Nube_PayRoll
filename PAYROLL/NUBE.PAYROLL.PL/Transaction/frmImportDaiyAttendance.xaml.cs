
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
                    using (SqlConnection con = new SqlConnection("Data Source =.\\sqlexpress;Initial Catalog = payroll; Integrated Security = True"))
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
                catch (Exception ex)
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
                        catch (Exception ex)
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
