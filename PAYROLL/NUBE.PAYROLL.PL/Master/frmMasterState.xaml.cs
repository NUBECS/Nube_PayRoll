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
    /// Interaction logic for frmMasterState.xaml
    /// </summary>
    public partial class frmMasterState : UserControl
    {
        int Id = 0;
        PayrollEntity db = new PayrollEntity();
        public frmMasterState()
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
                    var mb = (from x in db.MasterStates where x.Id == Id select x).FirstOrDefault();
                    mb.StateName = txtStateName.Text;
                    mb.ShortName = txtStateShortName.Text;
                    mb.CountryId = Convert.ToInt32(cmbCountry.SelectedItem);
                    db.SaveChanges();
                    MessageBox.Show("Updated Sucessfully!");
                    LoadWindow();
                }
                else
                {
                    MasterState mb = new MasterState();
                    mb.StateName = txtStateName.Text;
                    mb.ShortName = txtStateShortName.Text;
                    mb.CountryId = Convert.ToInt32(cmbCountry.SelectedValue);
                    db.MasterStates.Add(mb);
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
                    var mb = (from x in db.MasterStates where x.Id == Id select x).FirstOrDefault();
                    db.MasterStates.Remove(mb);
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

        private void dgvState_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgvState.SelectedItem != null))
                {
                    DataRowView drv = (DataRowView)dgvState.SelectedItem;
                    Id = Convert.ToInt32(drv["Id"]);
                    txtStateName.Text = drv["StateName"].ToString();
                    txtStateShortName.Text = drv["ShortName"].ToString();
                    cmbCountry.SelectedValue = Convert.ToInt32(drv["CountryId"]);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgvState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Id = 0;
                txtStateName.Text = "";
                txtStateShortName.Text = "";
                cmbCountry.Text = "";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        #endregion

        #region FUNCTIONS

        void LoadWindow()
        {
            Id = 0;
            txtStateName.Text = "";
            txtStateShortName.Text = "";
            cmbCountry.Text = "";
            try
            {
                var cu = (from x in db.MasterCountries select x).ToList();
                cmbCountry.ItemsSource = cu;
                cmbCountry.SelectedValuePath = "Id";
                cmbCountry.DisplayMemberPath = "CountryName";

                
                var st = (from x in db.ViewMasterStates select x).ToList();
                if (st != null)
                {
                    DataTable dt = AppLib.LINQResultToDataTable(st);
                    dgvState.ItemsSource = dt.DefaultView;
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
