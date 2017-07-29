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
    /// Interaction logic for frmMasterCity.xaml
    /// </summary>
    public partial class frmMasterCity : UserControl
    {
        int Id = 0;
        PayrollEntity db = new PayrollEntity();
        public frmMasterCity()
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
                    var mb = (from x in db.MasterCities where x.Id == Id select x).FirstOrDefault();
                    mb.CityName = txtCityName.Text;
                    mb.ShortName = txtCityShortName.Text;
                    mb.StateId = Convert.ToInt32(cmbState.SelectedValue);
                    db.SaveChanges();
                    MessageBox.Show("Updated Sucessfully!");
                    LoadWindow();
                }
                else
                {
                    MasterCity mb = new MasterCity();
                    mb.CityName = txtCityName.Text;
                    mb.ShortName = txtCityShortName.Text;
                    mb.StateId = Convert.ToInt32(cmbState.SelectedValue);
                    db.MasterCities.Add(mb);
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
                    var mb = (from x in db.MasterCities where x.Id == Id select x).FirstOrDefault();
                    db.MasterCities.Remove(mb);
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

        private void cbxCase_Checked(object sender, RoutedEventArgs e)
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

        private void dgvCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Id = 0;
            txtCityName.Text = "";
            txtCityShortName.Text = "";
            cmbState.Text = "";
        }

        private void dgvCity_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgvCity.SelectedItem != null))
                {
                    DataRowView drv = (DataRowView)dgvCity.SelectedItem;
                    Id = Convert.ToInt32(drv["Id"]);
                    txtCityName.Text = drv["CityName"].ToString();
                    txtCityShortName.Text = drv["ShortName"].ToString();
                    cmbState.SelectedValue = Convert.ToInt32(drv["StateId"]);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        #endregion

        #region FUNCTIONS

        void LoadWindow()
        {
            Id = 0;
            txtCityName.Text = "";
            txtCityShortName.Text = "";
            cmbState.Text = "";
            try
            {
                var cu = (from x in db.MasterStates select x).ToList();
                cmbState.ItemsSource = cu;
                cmbState.SelectedValuePath = "Id";
                cmbState.DisplayMemberPath = "StateName";

                var st = (from x in db.ViewMasterCities select x).ToList();
                if (st != null)
                {
                    DataTable dt = AppLib.LINQResultToDataTable(st);
                    dgvCity.ItemsSource = dt.DefaultView;
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
