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
using System.Windows.Shapes;
using System.Data;
using NUBE.PAYROLL.CMN;
using System.Data.SqlClient;

namespace NUBE.PAYROLL.PL.Transaction
{
    /// <summary>
    /// Interaction logic for frmAttedanceCorrectionDetails.xaml
    /// </summary>
    public partial class frmAttedanceCorrectionDetails : Window
    {
        DateTime dtEntryDate;
        int iEmployeeId = 0;
        int iMembershipNo = 0;
        string EmployeeName = "";
        string sGender = "";
        PayrollEntity db = new PayrollEntity();

        public frmAttedanceCorrectionDetails(string sDate = "", int iEmp_No = 0, int iMem_No = 0, string sEmp_Name = "", string sSex = "")
        {
            InitializeComponent();
            dtEntryDate = Convert.ToDateTime(sDate);
            iEmployeeId = iEmp_No;
            iMembershipNo = iMem_No;
            EmployeeName = sEmp_Name;
            sGender = sSex;

            txtMemberID.Text = iMembershipNo.ToString();
            txtEmployeeName.Text = EmployeeName.ToString();
            txtSex.Text = sGender.ToString();
            ComboBoxHours();
            ComboBoxMinutes();
        }

        #region EVENTS

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WindowLoad();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cmbInTimeHour.Text))
                {
                    MessageBox.Show("In Time Hours is Empty!");
                    cmbInTimeHour.Focus();
                }
                else if (string.IsNullOrEmpty(cmbInTimeMinutes.Text))
                {
                    MessageBox.Show("In Time Minutes is Empty!");
                    cmbInTimeMinutes.Focus();
                }
                else if (string.IsNullOrEmpty(cmbOutTimeHour.Text))
                {
                    MessageBox.Show("Out Time Hour is Empty!");
                    cmbOutTimeHour.Focus();
                }
                else if (string.IsNullOrEmpty(cmbOutTimeMinutes.Text))
                {
                    MessageBox.Show("Out Time Minutes is Empty!");
                    cmbOutTimeMinutes.Focus();
                }
                else if (Convert.ToInt32(cmbInTimeHour.Text) == 0)
                {
                    MessageBox.Show("In Time is Zero!");
                    cmbInTimeHour.Focus();
                }
                else if (Convert.ToInt32(cmbOutTimeHour.Text) == 0)
                {
                    MessageBox.Show("In Time is Zero!");
                    cmbOutTimeHour.Focus();
                }
                else
                {
                    var emp = (from m in db.MasterEmployees where m.Id == iEmployeeId select m).FirstOrDefault();
                    if (emp != null)
                    {
                        var shf = (from x in db.EmployeeShifts where x.Id == emp.ShiftId select x).FirstOrDefault();
                        if (shf != null)
                        {
                            var dly = (from x in db.AttedanceLogs where x.EmployeeId == iEmployeeId && x.EntryDate == dtEntryDate select x).FirstOrDefault();

                            //dly.IsModified = true;
                            //if (Convert.ToDateTime(shf.MinimumOtTime).TimeOfDay > Convert.ToDateTime(string.Format("{0} {1}:{2}:00.000", dtEntryDate, cmbOutTimeHour.Text, cmbOutTimeMinutes.Text)).TimeOfDay)
                            //{
                            //    dly.OutTime = Convert.ToDateTime(string.Format("{0} {1}:{1}:00.000", dtEntryDate, shf.OutTime));
                            //    dly. = Convert.ToDateTime(string.Format("{0} {1}:{1}:00.000", dtEntryDate, shf.OutTime));
                            //}
                        }
                        MessageBox.Show("Saved Sucessfully!");
                    }
                    else
                    {
                        MessageBox.Show("Can't Save Employee Details Not Found!", "Error");
                    }

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cmbInTimeHour.Text = "00";
            cmbInTimeMinutes.Text = "00";
            cmbOutTimeHour.Text = "00";
            cmbOutTimeMinutes.Text = "00";
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region FUNCTIONS

        void WindowLoad()
        {
            try
            {

                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    SqlCommand cmd;
                    string str = string.Format(" SELECT ROW_NUMBER() OVER(ORDER BY TA.PUNCHTIME ASC) AS RNO,TA.ID, \r" +
                        " EM.MEMBERSHIPNO EMPLOYEEID,CONVERT(VARCHAR(10),TA.ENTRYDATE,103)ENTRYDATE, \r" +
                        " right(convert(varchar(32),TA.PUNCHTIME,100),8)PUNCHTIME FROM TEMPATTENDANCETIMINGS TA(NOLOCK) \r" +
                        " LEFT JOIN MASTEREMPLOYEE EM(NOLOCK) ON EM.ID=TA.EMPLOYEEID \r" +
                        " WHERE TA.EMPLOYEEID={0} AND TA.ENTRYDATE ='{1:dd/MMM/yyyy}'", iEmployeeId, dtEntryDate);

                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    con.Open();
                    adp.Fill(dt);
                    con.Close();
                }

                if (dt.Rows.Count > 0)
                {
                    dgAttedanceCorrection.ItemsSource = dt.DefaultView;
                }
                else
                {
                    // MessageBox.Show("No Records Found");
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        void ComboBoxHours()
        {
            List<string> lstHour = new List<string>();
            lstHour.Add("00");
            lstHour.Add("01");
            lstHour.Add("02");
            lstHour.Add("03");
            lstHour.Add("04");
            lstHour.Add("05");
            lstHour.Add("06");
            lstHour.Add("07");
            lstHour.Add("08");
            lstHour.Add("09");
            lstHour.Add("10");
            lstHour.Add("11");
            lstHour.Add("12");
            lstHour.Add("13");
            lstHour.Add("14");
            lstHour.Add("15");
            lstHour.Add("16");
            lstHour.Add("17");
            lstHour.Add("18");
            lstHour.Add("19");
            lstHour.Add("20");
            lstHour.Add("21");
            lstHour.Add("22");
            lstHour.Add("23");
            cmbInTimeHour.ItemsSource = lstHour;
            cmbOutTimeHour.ItemsSource = lstHour;
        }

        void ComboBoxMinutes()
        {
            List<string> lstMinutes = new List<string>();
            lstMinutes.Add("00");
            lstMinutes.Add("01");
            lstMinutes.Add("02");
            lstMinutes.Add("03");
            lstMinutes.Add("04");
            lstMinutes.Add("05");
            lstMinutes.Add("06");
            lstMinutes.Add("07");
            lstMinutes.Add("08");
            lstMinutes.Add("09");
            lstMinutes.Add("10");
            lstMinutes.Add("11");
            lstMinutes.Add("12");
            lstMinutes.Add("13");
            lstMinutes.Add("14");
            lstMinutes.Add("15");
            lstMinutes.Add("16");
            lstMinutes.Add("17");
            lstMinutes.Add("18");
            lstMinutes.Add("19");
            lstMinutes.Add("20");
            lstMinutes.Add("21");
            lstMinutes.Add("22");
            lstMinutes.Add("23");
            lstMinutes.Add("24");
            lstMinutes.Add("25");
            lstMinutes.Add("26");
            lstMinutes.Add("27");
            lstMinutes.Add("28");
            lstMinutes.Add("29");
            lstMinutes.Add("30");
            lstMinutes.Add("31");
            lstMinutes.Add("32");
            lstMinutes.Add("33");
            lstMinutes.Add("34");
            lstMinutes.Add("35");
            lstMinutes.Add("36");
            lstMinutes.Add("37");
            lstMinutes.Add("38");
            lstMinutes.Add("39");
            lstMinutes.Add("40");
            lstMinutes.Add("41");
            lstMinutes.Add("42");
            lstMinutes.Add("43");
            lstMinutes.Add("44");
            lstMinutes.Add("45");
            lstMinutes.Add("46");
            lstMinutes.Add("47");
            lstMinutes.Add("48");
            lstMinutes.Add("49");
            lstMinutes.Add("50");
            lstMinutes.Add("51");
            lstMinutes.Add("52");
            lstMinutes.Add("53");
            lstMinutes.Add("54");
            lstMinutes.Add("55");
            lstMinutes.Add("56");
            lstMinutes.Add("57");
            lstMinutes.Add("58");
            lstMinutes.Add("59");
            cmbInTimeMinutes.ItemsSource = lstMinutes;
            cmbOutTimeMinutes.ItemsSource = lstMinutes;
        }

        #endregion

    }
}
