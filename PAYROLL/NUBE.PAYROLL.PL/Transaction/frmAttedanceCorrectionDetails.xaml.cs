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
        DateTime dtInTime;
        int iEmployeeId = 0;
        int iMembershipNo = 0;
        string EmployeeName = "";
        string sGender = "";
        int InHour = 0;
        int InMinutes;
        int iAL_Id = 0;
        Boolean bIsAlreadyModified = false;
        PayrollEntity db = new PayrollEntity();

        public frmAttedanceCorrectionDetails(string sDate = "", int iEmp_No = 0, int iMem_No = 0, string sEmp_Name = "", string sSex = "", bool IsModified = false, int ALID = 0)
        {
            InitializeComponent();
            dtEntryDate = Convert.ToDateTime(sDate);
            iEmployeeId = iEmp_No;
            iMembershipNo = iMem_No;
            EmployeeName = sEmp_Name;
            sGender = sSex;
            iAL_Id = ALID;
            bIsAlreadyModified = IsModified;

            txtMemberID.Text = iMembershipNo.ToString();
            txtEmployeeName.Text = EmployeeName.ToString();
            txtSex.Text = sGender.ToString();
            ComboBoxHours();
            ComboBoxMinutes();
            cmbInTimeHour.IsEnabled = true;
            cmbInTimeMinutes.IsEnabled = true;
        }

        #region EVENTS

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WindowLoad();
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            Config.CheckIsNumeric(e);
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
                    MessageBox.Show("Out Time is Zero!");
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
                            var dly = (from x in db.DailyAttedanceDets where x.EmployeeId == iEmployeeId && x.AttDate == dtEntryDate select x).FirstOrDefault();
                            if (dly != null)
                            {
                                dly.IsModified = true;
                                dly.InTime = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtEntryDate, cmbInTimeHour.Text, cmbInTimeMinutes.Text));
                                dly.OutTime = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtEntryDate, cmbOutTimeHour.Text, cmbOutTimeMinutes.Text));
                                if (Convert.ToDateTime(shf.MinimumOtTime).TimeOfDay < Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtEntryDate, cmbOutTimeHour.Text, cmbOutTimeMinutes.Text)).TimeOfDay)
                                {
                                    TimeSpan diff = Convert.ToDateTime(shf.OutTime).TimeOfDay - Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtEntryDate, cmbInTimeHour.Text, cmbInTimeMinutes.Text)).TimeOfDay;
                                    dly.TotalWorking_Hours = Convert.ToInt32(diff.Hours - 1);
                                    dly.TotalWorking_Minutes = Convert.ToInt32((diff.Minutes));

                                    TimeSpan diffOT = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtEntryDate, cmbOutTimeHour.Text, cmbOutTimeMinutes.Text)).TimeOfDay - Convert.ToDateTime(shf.OutTime).TimeOfDay;
                                    dly.OT_Hours = Convert.ToInt32(diffOT.Hours);
                                    dly.OT_Minutes = Convert.ToInt32((diffOT.Minutes));
                                }
                                else
                                {
                                    TimeSpan diff = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtEntryDate, cmbOutTimeHour.SelectedValue, cmbOutTimeMinutes.SelectedValue)).TimeOfDay - Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtEntryDate, cmbInTimeHour.Text, cmbInTimeMinutes.Text)).TimeOfDay;
                                    dly.TotalWorking_Hours = Convert.ToInt32(diff.Hours - 1);
                                    dly.TotalWorking_Minutes = Convert.ToInt32((diff.Minutes));
                                    dly.OT_Hours = 0;
                                    dly.OT_Minutes = 0;
                                }
                                db.SaveChanges();

                                var al = (from x in db.AttedanceLogs where x.Id == iAL_Id select x).FirstOrDefault();
                                if (al != null)
                                {
                                    al.IsModified = true;
                                    al.ModifiedOn = DateTime.Now;
                                    db.SaveChanges();
                                }
                            }
                        }
                        MessageBox.Show("Saved Sucessfully!", "Saved");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Can't Save Employee Details Not Found!", "Error");
                        this.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            //cmbInTimeHour.Text = "00";
            //cmbInTimeMinutes.Text = "00";
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

        private void cmbOutTimeHour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbInTimeHour.Text) && !string.IsNullOrEmpty(cmbInTimeMinutes.Text) && Convert.ToInt32(cmbOutTimeHour.SelectedValue) > 0 && Convert.ToInt32(cmbOutTimeMinutes.SelectedValue) > 0)
                {
                    if (Convert.ToInt32(cmbInTimeHour.Text) > 0 && Convert.ToInt32(cmbOutTimeHour.Text) > 0)
                    {
                        CalculateTime();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void cmbOutTimeMinutes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbInTimeHour.Text) && !string.IsNullOrEmpty(cmbInTimeMinutes.Text) && Convert.ToInt32(cmbOutTimeHour.SelectedValue) > 0 && Convert.ToInt32(cmbOutTimeMinutes.SelectedValue) > 0)
                {
                    if (Convert.ToInt32(cmbInTimeHour.Text) > 0 && Convert.ToInt32(cmbOutTimeHour.Text) > 0)
                    {
                        CalculateTime();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void cmbInTimeHour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbInTimeHour.Text) && !string.IsNullOrEmpty(cmbInTimeMinutes.Text) && Convert.ToInt32(cmbOutTimeHour.SelectedValue) > 0 && Convert.ToInt32(cmbOutTimeMinutes.SelectedValue) > 0)
                {
                    if (Convert.ToInt32(cmbInTimeHour.Text) > 0 && Convert.ToInt32(cmbOutTimeHour.Text) > 0)
                    {
                        CalculateTime();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void cmbInTimeMinutes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbInTimeHour.Text) && !string.IsNullOrEmpty(cmbInTimeMinutes.Text) && Convert.ToInt32(cmbOutTimeHour.SelectedValue) > 0 && Convert.ToInt32(cmbOutTimeMinutes.SelectedValue) > 0)
                {
                    if (Convert.ToInt32(cmbInTimeHour.Text) > 0 && Convert.ToInt32(cmbOutTimeHour.Text) > 0)
                    {
                        CalculateTime();
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
                        " right(convert(varchar(32),TA.PUNCHTIME,100),8)PUNCHTIME, \r" +
                        " DATEPART(HOUR, TA.PUNCHTIME)INHOUR, \r" +
                        " DATEPART(MINUTE, TA.PUNCHTIME)INMINUTES, \r" +
                        " TA.PUNCHTIME OINTIME \r" +
                        " FROM TEMPATTENDANCETIMINGS TA(NOLOCK) \r" +
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
                    InHour = Convert.ToInt32(dt.Rows[0]["INHOUR"]);
                    InMinutes = Convert.ToInt32(dt.Rows[0]["INMINUTES"]);
                    dtInTime = Convert.ToDateTime(dt.Rows[0]["OINTIME"]);                    
                    txtTotalWorkingHours.Text = "0";
                    if (bIsAlreadyModified == true)
                    {
                        var dly = (from x in db.DailyAttedanceDets where x.EmployeeId == iEmployeeId && x.AttDate == dtEntryDate select x).FirstOrDefault();
                        if (dly != null)
                        {
                            cmbInTimeHour.Text = Convert.ToDateTime(dly.InTime).TimeOfDay.Hours.ToString();
                            cmbInTimeMinutes.Text = Convert.ToDateTime(dly.InTime).TimeOfDay.Minutes.ToString();
                            cmbOutTimeHour.Text = Convert.ToDateTime(dly.OutTime).TimeOfDay.Hours.ToString();
                            cmbOutTimeMinutes.Text = Convert.ToDateTime(dly.OutTime).TimeOfDay.Minutes.ToString();
                        }
                    }

                    dgAttedanceCorrection.ItemsSource = dt.DefaultView;
                }
                else
                {
                    MessageBox.Show("No Records Found");
                    cmbInTimeHour.IsEnabled = true;
                    cmbInTimeMinutes.IsEnabled = true;
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        void CalculateTime()
        {
            TimeSpan diff = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtEntryDate, cmbOutTimeHour.SelectedValue, cmbOutTimeMinutes.SelectedValue)).TimeOfDay - Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy} {1}:{2}:00.000", dtEntryDate, cmbInTimeHour.Text, cmbInTimeMinutes.Text)).TimeOfDay;
            txtTotalWorkingHours.Text = string.Format((Convert.ToInt32(diff.Hours) - 1) + ":" + diff.Minutes);
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
