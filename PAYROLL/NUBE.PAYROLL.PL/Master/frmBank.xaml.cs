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
    /// Interaction logic for frmBank.xaml
    /// </summary>
    public partial class frmBank : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        int Id = 0;
        DataTable dtBank = new DataTable();
        public frmBank()
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
                if (string.IsNullOrEmpty(txtBankName.Text))
                {
                    MessageBox.Show("City Name is Empty!", "Empty");
                    txtBankName.Focus();
                }
                else if (string.IsNullOrEmpty(txtBankUserCode.Text))
                {
                    MessageBox.Show("UserCode is Empty!", "Empty");
                    txtBankUserCode.Focus();
                }
                else
                {
                    if (Id != 0)
                    {
                        var mb = (from x in db.MasterBanks where x.Id == Id select x).FirstOrDefault();
                        mb.BankName = txtBankName.Text;
                        mb.UserCode = txtBankUserCode.Text;
                        db.SaveChanges();
                        MessageBox.Show("Updated Sucessfully!");
                        LoadWindow();
                    }
                    else
                    {
                        MasterBank mb = new MasterBank();
                        mb.BankName = txtBankName.Text;
                        mb.UserCode = txtBankUserCode.Text;
                        db.MasterBanks.Add(mb);
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
                if (Id != 0)
                {
                    if (MessageBox.Show("Do you want to Delete ?", "Delete Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var mb = (from x in db.MasterBanks where x.Id == Id select x).FirstOrDefault();
                        mb.IsCancel = true;
                        mb.CancelOn = DateTime.Now;
                        db.SaveChanges();
                        //db.MasterBanks.Remove(mb);                        
                        MessageBox.Show("Deleted Sucessfully");
                        LoadWindow();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "You Can't Delete");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            LoadWindow();
        }

        private void dgvBank_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Id = 0;
                txtBankName.Text = "";
                txtBankUserCode.Text = "";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgvBank_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgvBank.SelectedItem != null))
                {
                    DataRowView drv = (DataRowView)dgvBank.SelectedItem;
                    Id = Convert.ToInt32(drv["Id"]);
                    txtBankName.Text = drv["BankName"].ToString();
                    txtBankUserCode.Text = drv["UserCode"].ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSearch_PreviewKeyUp(object sender, KeyEventArgs e)
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

        #endregion

        #region FUNCTIONS

        void LoadWindow()
        {
            Id = 0;
            txtBankName.Text = "";
            txtBankUserCode.Text = "";
            dtBank.Rows.Clear();
            try
            {
                var Bank = (from x in db.MasterBanks where x.IsCancel == false select x).ToList();
                if (Bank != null)
                {
                    dtBank = AppLib.LINQResultToDataTable(Bank);
                    dgvBank.ItemsSource = dtBank.DefaultView;
                    Filteration();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        void Filteration()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    string sWhere = "";

                    if (rptContain.IsChecked == true)
                    {
                        sWhere = "BankName LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else if (rptEndWith.IsChecked == true)
                    {
                        sWhere = "BankName LIKE '%" + txtSearch.Text.ToUpper() + "'";
                    }
                    else if (rptStartWith.IsChecked == true)
                    {
                        sWhere = "BankName LIKE '" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else
                    {
                        sWhere = "BankName LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }

                    if (!string.IsNullOrEmpty(sWhere))
                    {
                        DataView dv = new DataView(dtBank);
                        dv.RowFilter = sWhere;
                        DataTable dtTemp = dv.ToTable();
                        dgvBank.ItemsSource = dtTemp.DefaultView;
                    }
                    else
                    {
                        dgvBank.ItemsSource = dtBank.DefaultView;
                    }
                }
                else
                {
                    dgvBank.ItemsSource = dtBank.DefaultView;
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
