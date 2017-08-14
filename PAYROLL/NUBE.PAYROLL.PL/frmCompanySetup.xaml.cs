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

namespace NUBE.PAYROLL.PL
{
    /// <summary>
    /// Interaction logic for frmCompanySetup.xaml
    /// </summary>
    public partial class frmCompanySetup : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        public frmCompanySetup()
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

        private void cmbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {

        }

        private void ChkGraceTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ChkGraceTime.IsChecked == true)
                {
                    txtGracePeriod.Visibility = Visibility.Visible;
                }
                else
                {
                    txtGracePeriod.Visibility = Visibility.Hidden;
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
                var cmp = (from x in db.CompanyDetails where x.Id == 1 select x).FirstOrDefault();
                if (cmp != null)
                {
                    cmp.CompanyName = txtCompanyName.Text;
                    cmp.AddressLine1 = txtAddress1.Text;
                    cmp.AddressLine2 = txtAddress2.Text;
                    cmp.AddressLine3 = txtAddress3.Text;

                    cmp.CityCode = Convert.ToInt32(cmbCity.SelectedValue);
                    cmp.StateCode = Convert.ToInt32(cmbState.SelectedValue);
                    cmp.CountryCode = Convert.ToInt32(cmbCountry.SelectedValue);
                    cmp.PostalCode = txtPostalCode.Text;
                    cmp.TelephoneNo = txtTelephone.Text;
                    cmp.MobileNo = txtMobile.Text;
                    if (rpt5Days.IsChecked == true)
                    {
                        cmp.WeekofTwoDays = true;
                    }
                    else
                    {
                        cmp.WeekofTwoDays = false;
                    }
                    cmp.IsGraceTime = Convert.ToBoolean(ChkGraceTime.IsChecked);
                    cmp.GraceTime = Convert.ToDecimal(txtGracePeriod.Text);
                    cmp.InTime = Convert.ToDateTime(string.Format("2017-08-01 {0}:{1}:00.000", cmbInTimeHour.Text, cmbInTimeMinutes.Text));
                    cmp.OutTime = Convert.ToDateTime(string.Format("2017-08-01 {0}:{1}:00.000", cmbOutTimeHour.Text, cmbOutTimeMinutes.Text));
                    cmp.MinimumOtTime = Convert.ToDateTime(string.Format("2017-08-01 {0}:{1}:00.000", cmbOTTimeHour.Text, cmbOTTimeMinutes.Text));
                    db.SaveChanges();

                    MessageBox.Show("Saved Sucessfully!", "PAYROLL");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            LoadWindow();
        }

        #endregion

        #region Functions       

        void FormClear()
        {
            txtCompanyName.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            cmbCity.Text = "";
            cmbState.Text = "";
            cmbCountry.Text = "";
            txtPostalCode.Text = "";
            txtTelephone.Text = "";
            txtMobile.Text = "";
            rpt5Days.IsChecked = true;
            txtGracePeriod.Text = "";
            ChkGraceTime.IsChecked = true;
            txtGracePeriod.Visibility = Visibility.Visible;
        }

        void LoadWindow()
        {
            try
            {
                var cy = (from x in db.MasterCountries select x).ToList();
                cmbCountry.ItemsSource = cy;
                cmbCountry.SelectedValuePath = "Id";
                cmbCountry.DisplayMemberPath = "CountryName";

                var cu = (from x in db.MasterStates select x).ToList();
                cmbState.ItemsSource = cu;
                cmbState.SelectedValuePath = "Id";
                cmbState.DisplayMemberPath = "StateName";

                var st = (from x in db.MasterCities select x).ToList();
                cmbCity.ItemsSource = st;
                cmbCity.SelectedValuePath = "Id";
                cmbCity.DisplayMemberPath = "CityName";

                FormClear();

                var cmp = (from x in db.CompanyDetails where x.Id == 1 select x).FirstOrDefault();
                if (cmp != null)
                {
                    txtCompanyName.Text = cmp.CompanyName;
                    txtAddress1.Text = cmp.AddressLine1;
                    txtAddress2.Text = cmp.AddressLine2;
                    txtAddress3.Text = cmp.AddressLine3;
                    cmbCity.SelectedValue = cmp.CityCode;
                    cmbState.SelectedValue = cmp.StateCode;
                    cmbCountry.SelectedValue = cmp.CountryCode;
                    txtPostalCode.Text = cmp.PostalCode;
                    txtTelephone.Text = cmp.TelephoneNo;
                    txtMobile.Text = cmp.MobileNo;
                    if (cmp.WeekofTwoDays == true)
                    {
                        rpt5Days.IsChecked = true;
                    }
                    else
                    {
                        rpt6Days.IsChecked = true;
                    }
                    txtGracePeriod.Text = cmp.GraceTime.ToString();
                    ChkGraceTime.IsChecked = cmp.IsGraceTime;
                    cmbInTimeHour.Text = Convert.ToDateTime(cmp.InTime).Hour.ToString();
                    cmbInTimeMinutes.Text = Convert.ToDateTime(cmp.InTime).Minute.ToString();
                    cmbOutTimeHour.Text = Convert.ToDateTime(cmp.OutTime).Hour.ToString();
                    cmbOutTimeMinutes.Text = Convert.ToDateTime(cmp.OutTime).Minute.ToString();
                    cmbOTTimeHour.Text = Convert.ToDateTime(cmp.MinimumOtTime).Hour.ToString();
                    cmbOTTimeMinutes.Text = Convert.ToDateTime(cmp.MinimumOtTime).Minute.ToString();
                    if (cmp.IsGraceTime == true)
                    {
                        txtGracePeriod.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        txtGracePeriod.Visibility = Visibility.Hidden;
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
            cmbOTTimeMinutes.ItemsSource = lstMinutes;
        }


        #endregion
    }
}
