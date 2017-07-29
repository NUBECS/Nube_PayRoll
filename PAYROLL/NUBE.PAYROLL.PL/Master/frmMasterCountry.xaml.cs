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

namespace NUBE.PAYROLL.PL.Master
{
    /// <summary>
    /// Interaction logic for frmMasterCountry.xaml
    /// </summary>
    public partial class frmMasterCountry : UserControl
    {
        int Id = 0;
        PayrollEntity db = new PayrollEntity();
        public frmMasterCountry()
        {
            InitializeComponent();
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
                if (Id != 0)
                {
                    var mb = (from x in db.MasterCountries where x.Id == Id select x).FirstOrDefault();
                    mb.CountryName = txtCountryName.Text;
                    mb.ShortName = txtCountryShortName.Text;
                    db.SaveChanges();
                    MessageBox.Show("Updated Sucessfully!");
                    LoadWindow();
                }
                else
                {
                    MasterCountry mb = new MasterCountry();
                    mb.CountryName = txtCountryName.Text;
                    mb.ShortName = txtCountryShortName.Text;
                    db.MasterCountries.Add(mb);
                    db.SaveChanges();
                    MessageBox.Show("Saved Sucessfully!");
                    LoadWindow();
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
                if (Id != 0)
                {
                    var mb = (from x in db.MasterCountries where x.Id == Id select x).FirstOrDefault();
                    db.MasterCountries.Remove(mb);
                    db.SaveChanges();
                    MessageBox.Show("Deleted Sucessfully");
                    LoadWindow();
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

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cbxCase_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void rptStartWith_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rptContain_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rptEndWith_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void dgvCountry_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgvCountry.SelectedItem != null))
                {
                    DataRowView drv = (DataRowView)dgvCountry.SelectedItem;
                    Id = Convert.ToInt32(drv["Id"]);
                    txtCountryName.Text = drv["CountryName"].ToString();
                    txtCountryShortName.Text = drv["ShortName"].ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgvCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Id = 0;
                txtCountryName.Text = "";
                txtCountryShortName.Text = "";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void cbxCase_Checked(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region FUNCITONS

        void LoadWindow()
        {
            Id = 0;
            txtCountryName.Text = "";
            txtCountryShortName.Text = "";
            try
            {
                var Country = (from x in db.MasterCountries select x).ToList();
                if (Country != null)
                {
                    DataTable dt = AppLib.LINQResultToDataTable(Country);
                    dgvCountry.ItemsSource = dt.DefaultView;
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
