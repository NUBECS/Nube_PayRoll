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
using System.Data;
using System.Data.SqlClient;

namespace NUBE.PAYROLL.PL.Master
{
    /// <summary>
    /// Interaction logic for frmEmployeeShift.xaml
    /// </summary>
    public partial class frmEmployeeShift : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        int Id = 0;
        DataTable dtEmployeeShift = new DataTable();
        public frmEmployeeShift()
        {
            InitializeComponent();
            ComboBoxHours();
            ComboBoxMinutes();
        }

        #region EVENTS

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWindow();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtShiftName.Text))
                {
                    MessageBox.Show("Shift Name is Empty!");
                    txtShiftName.Focus();
                }
                else if (string.IsNullOrEmpty(cmbInTimeHour.Text))
                {
                    MessageBox.Show("InTime is Empty!");
                    cmbInTimeHour.Focus();
                }
                else if (string.IsNullOrEmpty(cmbInTimeMinutes.Text))
                {
                    MessageBox.Show("InTime is Empty!");
                    cmbInTimeMinutes.Focus();
                }
                else if (string.IsNullOrEmpty(cmbOutTimeHour.Text))
                {
                    MessageBox.Show("Out Time is Empty!");
                    cmbOutTimeHour.Focus();
                }
                else if (string.IsNullOrEmpty(cmbOutTimeMinutes.Text))
                {
                    MessageBox.Show("Out Time is Empty!");
                    cmbOutTimeMinutes.Focus();
                }
                else if (MessageBox.Show("Do You want to Save Shift ?", "Save Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (Id != 0)
                    {
                        var mb = (from x in db.EmployeeShifts where x.Id == Id select x).FirstOrDefault();
                        mb.Name = txtShiftName.Text;
                        mb.InTime = Convert.ToDateTime(string.Format("2017-08-01 {0}:{1}:00.000", cmbInTimeHour.Text, cmbInTimeMinutes.Text));
                        mb.LunchTimeOut = Convert.ToDateTime(string.Format("2017-08-01 {0}:{1}:00.000", cmbLunchStartHour.Text, cmbLunchStartMinutes.Text));
                        mb.LunchTimeIn = Convert.ToDateTime(string.Format("2017-08-01 {0}:{1}:00.000", cmbLunchEndHour.Text, cmbLunchEndMinutes.Text));
                        mb.OutTime = Convert.ToDateTime(string.Format("2017-08-01 {0}:{1}:00.000", cmbOutTimeHour.Text, cmbOutTimeMinutes.Text));
                        mb.MinimumOtTime = Convert.ToDateTime(string.Format("2017-08-01 {0}:{1}:00.000", cmbOTTimeHour.Text, cmbOTTimeMinutes.Text));
                        if (rpt5Days.IsChecked == true)
                        {
                            mb.WeekofTwoDays = true;
                        }
                        else
                        {
                            mb.WeekofTwoDays = false;
                        }
                        mb.IsGraceTime = Convert.ToBoolean(ChkGraceTime.IsChecked);
                        if (!string.IsNullOrEmpty(cmbGracePeriod.Text))
                        {
                            mb.GraceTime = Convert.ToDecimal(cmbGracePeriod.Text);
                        }
                        db.SaveChanges();
                        MessageBox.Show("Updated Sucessfully!");
                        LoadWindow();
                    }
                    else
                    {
                        EmployeeShift mb = new EmployeeShift();
                        mb.Name = txtShiftName.Text;
                        mb.InTime = Convert.ToDateTime(string.Format("2017-08-01 {0}:{1}:00.000", cmbInTimeHour.Text, cmbInTimeMinutes.Text));
                        mb.LunchTimeOut = Convert.ToDateTime(string.Format("2017-08-01 {0}:{1}:00.000", cmbLunchStartHour.Text, cmbLunchStartMinutes.Text));
                        mb.LunchTimeIn = Convert.ToDateTime(string.Format("2017-08-01 {0}:{1}:00.000", cmbLunchEndHour.Text, cmbLunchEndMinutes.Text));
                        mb.OutTime = Convert.ToDateTime(string.Format("2017-08-01 {0}:{1}:00.000", cmbOutTimeHour.Text, cmbOutTimeMinutes.Text));
                        mb.MinimumOtTime = Convert.ToDateTime(string.Format("2017-08-01 {0}:{1}:00.000", cmbOTTimeHour.Text, cmbOTTimeMinutes.Text));
                        if (rpt5Days.IsChecked == true)
                        {
                            mb.WeekofTwoDays = true;
                        }
                        else
                        {
                            mb.WeekofTwoDays = false;
                        }
                        mb.IsGraceTime = Convert.ToBoolean(ChkGraceTime.IsChecked);
                        if (!string.IsNullOrEmpty(cmbGracePeriod.Text))
                        {
                            mb.GraceTime = Convert.ToDecimal(cmbGracePeriod.Text);
                        }
                        db.EmployeeShifts.Add(mb);
                        db.SaveChanges();
                        MessageBox.Show("Saved Sucessfully!");
                        LoadWindow();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do You want to Delete Shift ?", "Delete Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (Id != 0)
                    {
                        var mb = (from x in db.EmployeeShifts where x.Id == Id select x).FirstOrDefault();
                        db.EmployeeShifts.Remove(mb);
                        db.SaveChanges();
                        MessageBox.Show("Deleted Sucessfully");
                        LoadWindow();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't Delete This!", "Reference Shift Can't Delete");
                //ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadWindow();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgvEmployeeShift_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgvEmployeeShift.SelectedItem != null))
                {
                    DataRowView drv = (DataRowView)dgvEmployeeShift.SelectedItem;
                    Id = Convert.ToInt32(drv["ID"]);
                    txtShiftName.Text = drv["NAME"].ToString();
                    ChkGraceTime.IsChecked = Convert.ToBoolean(drv["ISGRACETIME"]);
                    if (Convert.ToBoolean(drv["WEEKOFTWODAYS"]) == true)
                    {
                        rpt5Days.IsChecked = true;
                    }
                    else
                    {
                        rpt6Days.IsChecked = true;
                    }
                    cmbGracePeriod.Text = drv["GRACETIME"].ToString();
                    cmbInTimeHour.Text = Convert.ToDateTime(drv["INTIME"]).Hour.ToString();
                    cmbInTimeMinutes.Text = Convert.ToDateTime(drv["INTIME"]).Minute.ToString();
                    cmbOutTimeHour.Text = Convert.ToDateTime(drv["OUTTIME"]).Hour.ToString();
                    cmbOutTimeMinutes.Text = Convert.ToDateTime(drv["OUTTIME"]).Minute.ToString();
                    cmbLunchStartHour.Text = Convert.ToDateTime(drv["LUNCHTIMEOUT"]).Hour.ToString();
                    cmbLunchStartMinutes.Text = Convert.ToDateTime(drv["LUNCHTIMEOUT"]).Minute.ToString();
                    cmbLunchEndHour.Text = Convert.ToDateTime(drv["LUNCHTIMEIN"]).Hour.ToString();
                    cmbLunchEndMinutes.Text = Convert.ToDateTime(drv["LUNCHTIMEIN"]).Minute.ToString();
                    cmbOTTimeHour.Text = Convert.ToDateTime(drv["MINIMUMOTTIME"]).Hour.ToString();
                    cmbOTTimeMinutes.Text = Convert.ToDateTime(drv["MINIMUMOTTIME"]).Minute.ToString();                   
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgvEmployeeShift_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FormClear();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void ChkGraceTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (ChkGraceTime.IsChecked == true)
                //{
                //    cmbGracePeriod.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    cmbGracePeriod.Visibility = Visibility.Hidden;
                //}
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void cbxCase_Checked(object sender, RoutedEventArgs e)
        {
            Filteration();
        }

        private void cbxCase_Unchecked(object sender, RoutedEventArgs e)
        {
            Filteration();
        }

        private void rptStartWith_Checked(object sender, RoutedEventArgs e)
        {
            Filteration();
        }

        private void rptContain_Checked(object sender, RoutedEventArgs e)
        {
            Filteration();
        }

        private void rptEndWith_Checked(object sender, RoutedEventArgs e)
        {
            Filteration();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filteration();
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {

        }

        #endregion

        #region Functions      

        void FormClear()
        {
            Id = 0;
            txtShiftName.Text = "";
            ChkGraceTime.IsChecked = false;
            rpt5Days.IsChecked = true;
            cmbGracePeriod.Text = "";
            cmbInTimeHour.Text = "00";
            cmbInTimeMinutes.Text = "00";
            cmbOutTimeHour.Text = "00";
            cmbOutTimeMinutes.Text = "00";
            cmbLunchStartHour.Text = "00";
            cmbLunchStartMinutes.Text = "00";
            cmbLunchEndHour.Text = "00";
            cmbLunchEndMinutes.Text = "00";
            cmbOTTimeHour.Text = "00";
            cmbOTTimeMinutes.Text = "00";           
        }

        void LoadWindow()
        {
            FormClear();
            try
            {                
                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    SqlCommand cmd;
                    string str = " SELECT ID,NAME,RIGHT(CONVERT(VARCHAR(32),INTIME,100),8)INTIME,RIGHT(CONVERT(VARCHAR(32),LUNCHTIMEOUT,100),8)LUNCHTIMEOUT,\r" +
                                 " RIGHT(CONVERT(VARCHAR(32), LUNCHTIMEIN, 100), 8)LUNCHTIMEIN,\r" +
                                 " RIGHT(CONVERT(VARCHAR(32), OUTTIME, 100), 8)OUTTIME,\r" +
                                 " RIGHT(CONVERT(VARCHAR(32), MINIMUMOTTIME, 100), 8)MINIMUMOTTIME,\r" +
                                 " WEEKOFTWODAYS, ISGRACETIME, GRACETIME\r" +
                                 " FROM EMPLOYEESHIFT(NOLOCK)";

                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    con.Open();
                    dtEmployeeShift.Rows.Clear();
                    adp.Fill(dtEmployeeShift);
                    con.Close();
                    if (dtEmployeeShift.Rows.Count > 0)
                    {
                        dgvEmployeeShift.ItemsSource = dtEmployeeShift.DefaultView;
                    }
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
            cmbLunchStartHour.ItemsSource = lstHour;
            cmbLunchEndHour.ItemsSource = lstHour;
            cmbOTTimeHour.ItemsSource = lstHour;
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
            cmbLunchStartMinutes.ItemsSource = lstMinutes;
            cmbLunchEndMinutes.ItemsSource = lstMinutes;
            cmbOTTimeMinutes.ItemsSource = lstMinutes;
            cmbGracePeriod.ItemsSource = lstMinutes;
        }

        void Filteration()
        {
            try
            {
                string sWhere = "";

                if (cbxCase.IsChecked == true)
                {
                    if (rptContain.IsChecked == true)
                    {
                        sWhere = " NAME LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else if (rptEndWith.IsChecked == true)
                    {
                        sWhere = " NAME LIKE '%" + txtSearch.Text.ToUpper() + "'";
                    }
                    else if (rptStartWith.IsChecked == true)
                    {
                        sWhere = " NAME LIKE '" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else
                    {
                        sWhere = " NAME LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                }
                else
                {
                    if (rptContain.IsChecked == true)
                    {
                        sWhere = " NAME LIKE '%" + txtSearch.Text + "%'";
                    }
                    else if (rptEndWith.IsChecked == true)
                    {
                        sWhere = " NAME LIKE '%" + txtSearch.Text + "'";
                    }
                    else if (rptStartWith.IsChecked == true)
                    {
                        sWhere = " NAME LIKE '" + txtSearch.Text + "%'";
                    }
                    else
                    {
                        sWhere = "NAME LIKE '%" + txtSearch.Text + "%'";
                    }
                }
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    DataView dv = new DataView(dtEmployeeShift);
                    dv.RowFilter = sWhere;
                    DataTable dtTemp = new DataTable();
                    dtTemp = dv.ToTable();
                    dgvEmployeeShift.ItemsSource = dtTemp.DefaultView;
                }
                else
                {
                    dgvEmployeeShift.ItemsSource = dtEmployeeShift.DefaultView;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        #endregion

        
    }
}
