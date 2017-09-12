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
    /// Interaction logic for MasterBankBranch.xaml
    /// </summary>
    public partial class MasterBankBranch : UserControl
    {
        int Id = 0;
        PayrollEntity db = new PayrollEntity();
        public MasterBankBranch()
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
                if (string.IsNullOrEmpty(txtBankBranchName.Text))
                {
                    MessageBox.Show("Bank BranchName is Empty!", "Empty");
                    txtBankBranchName.Focus();
                }
                else if (string.IsNullOrEmpty(txtUserCode.Text))
                {
                    MessageBox.Show("UserCode is Empty!", "Empty");
                    txtUserCode.Focus();
                }
                else
                {
                    if (Id != 0)
                    {
                        var mb = (from x in db.MasterBankBranches where x.Id == Id select x).FirstOrDefault();
                        mb.BankBranchName = txtBankBranchName.Text;
                        mb.UserCode = txtUserCode.Text;
                        mb.BankId = Convert.ToInt32(cmbBank.SelectedValue);
                        mb.NubeBranchId = Convert.ToInt32(cmbNubeBranch.SelectedValue);
                        mb.Address1 = txtAddress1.Text;
                        mb.Address2 = txtAddress2.Text;
                        mb.CityId = Convert.ToInt32(cmbState.SelectedValue);
                        mb.ZipCode = txtPostalCode.Text;
                        mb.Phone1 = txtTelephone.Text;
                        mb.Phone2 = txtMobile.Text;
                        mb.IsHeadQuarters = Convert.ToBoolean(ChkHeadOffice.IsChecked);
                        db.SaveChanges();
                        MessageBox.Show("Updated Sucessfully!");
                        LoadWindow();
                    }
                    else
                    {
                        MasterBankBranch bb = new MasterBankBranch();

                        //MasterBankBranch mb = new MasterBankBranch();                                                            
                        //mb.BankBranchName = txtBankBranchName.Text;
                        //mb.UserCode = txtUserCode.Text;
                        //mb.BankId = Convert.ToInt32(cmbBank.SelectedValue);
                        //mb.NubeBranchId = Convert.ToInt32(cmbNubeBranch.SelectedValue);
                        //mb.Address1 = txtAddress1.Text;
                        //mb.Address2 = txtAddress2.Text;
                        //mb.CityId = Convert.ToInt32(cmbState.SelectedValue);
                        //mb.ZipCode = txtPostalCode.Text;
                        //mb.Phone1 = txtTelephone.Text;
                        //mb.Phone2 = txtMobile.Text;
                        //mb.IsHeadQuarters = Convert.ToBoolean(ChkHeadOffice.IsChecked);
                        //db.MasterBankBranches.Add(mb);
                        //db.SaveChanges();
                        //MessageBox.Show("Saved Sucessfully!");
                        //LoadWindow();
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
            if (MessageBox.Show("Do you want to Delete ?", "Delete Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
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

        private void dgvBankBranch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgvBankBranch.SelectedItem != null))
                {
                    DataRowView drv = (DataRowView)dgvBankBranch.SelectedItem;
                    Id = Convert.ToInt32(drv["Id"]);
                    txtBankBranchName.Text = drv["BankBranchName"].ToString();
                    txtUserCode.Text = drv["UserCode"].ToString();
                    cmbBank.SelectedValue = Convert.ToInt32(drv["BankId"]);
                    cmbNubeBranch.SelectedValue = Convert.ToInt32(drv["NubeBranchId"]);
                    txtAddress1.Text = drv["Address1"].ToString();
                    txtAddress2.Text = drv["Address2"].ToString();
                    cmbCity.SelectedValue = Convert.ToInt32(drv["CityId"]);
                    txtPostalCode.Text = drv["ZipCode"].ToString();
                    cmbState.SelectedValue = Convert.ToInt32(drv["StateId"]);
                    cmbCountry.SelectedValue = Convert.ToInt32(drv["CountryId"]);
                    txtTelephone.Text = drv["Phone1"].ToString();
                    txtMobile.Text = drv["Phone2"].ToString();
                    ChkHeadOffice.IsChecked = Convert.ToBoolean(drv["IsHeadQuarters"]);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgvBankBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadWindow();
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {

        }
        #endregion

        #region FUNCTIONS

        void LoadWindow()
        {
            Id = 0;
            txtBankBranchName.Text = "";
            txtUserCode.Text = "";
            cmbBank.Text = "";
            cmbNubeBranch.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            cmbCity.Text = "";
            txtPostalCode.Text = "";
            cmbState.Text = "";
            cmbCountry.Text = "";
            txtTelephone.Text = "";
            txtMobile.Text = "";
            ChkHeadOffice.IsChecked = false;


            try
            {
                var cy = (from x in db.MasterCountries where x.IsCancel == false select x).ToList();
                cmbCountry.ItemsSource = cy;
                cmbCountry.SelectedValuePath = "Id";
                cmbCountry.DisplayMemberPath = "CountryName";

                var cu = (from x in db.MasterStates where x.IsCancel == false select x).ToList();
                cmbState.ItemsSource = cu;
                cmbState.SelectedValuePath = "Id";
                cmbState.DisplayMemberPath = "StateName";

                var st = (from x in db.MasterCities where x.IsCancel == false select x).ToList();
                cmbCity.ItemsSource = st;
                cmbCity.SelectedValuePath = "Id";
                cmbCity.DisplayMemberPath = "CityName";

                var mb = (from x in db.MasterBanks where x.IsCancel == false select x).ToList();
                cmbBank.ItemsSource = mb;
                cmbBank.SelectedValuePath = "Id";
                cmbBank.DisplayMemberPath = "BankName";

                var nb = (from x in db.MasterNubeBranches where x.IsCancel == false select x).ToList();
                cmbNubeBranch.ItemsSource = nb;
                cmbNubeBranch.SelectedValuePath = "Id";
                cmbNubeBranch.DisplayMemberPath = "NubeBranchName";

                var vb = (from x in db.ViewMasterbankbranches select x).ToList();
                if (st != null)
                {
                    DataTable dt = AppLib.LINQResultToDataTable(st);
                    dgvBankBranch.ItemsSource = dt.DefaultView;
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
