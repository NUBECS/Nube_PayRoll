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
    /// Interaction logic for frmMasterState.xaml
    /// </summary>
    public partial class frmMasterState : UserControl
    {
        int Id = 0;
        PayrollEntity db = new PayrollEntity();
        DataTable dtState = new DataTable();
        public frmMasterState()
        {
            InitializeComponent();
        }

        #region EVENTS

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    SqlCommand cmd;
                    string str = " SELECT Id,CountryName \r" +
                                 " FROM MASTERCOUNTRY (NOLOCK) \r" +
                                 " WHERE ISCANCEL=0 \r" +
                                 " ORDER BY CountryName";

                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    con.Open();
                    adp.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        cmbCountry.ItemsSource = null;
                        cmbCountry.ItemsSource = dt.DefaultView;
                        cmbCountry.SelectedValuePath = "Id";
                        cmbCountry.DisplayMemberPath = "CountryName";
                    }
                }
                LoadWindow();
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
                if (string.IsNullOrEmpty(txtStateName.Text))
                {
                    MessageBox.Show("State Name is Empty!", "Empty");
                    txtStateName.Focus();
                }
                else if (string.IsNullOrEmpty(txtStateShortName.Text))
                {
                    MessageBox.Show("Short Name is Empty!", "Empty");
                    txtStateShortName.Focus();
                }
                else if (string.IsNullOrEmpty(cmbCountry.Text))
                {
                    MessageBox.Show("Country is Empty!", "Empty");
                    cmbCountry.Focus();
                }
                else
                {
                    if (Id != 0)
                    {
                        var mb = (from x in db.MasterStates where x.Id == Id select x).FirstOrDefault();
                        mb.StateName = txtStateName.Text;
                        mb.ShortName = txtStateShortName.Text;
                        mb.CountryId = Convert.ToInt32(cmbCountry.SelectedValue);
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
                        var mb = (from x in db.MasterStates where x.Id == Id select x).FirstOrDefault();
                        mb.IsCancel = true;
                        mb.CancelOn = DateTime.Now;
                        //db.MasterStates.Remove(mb);
                        db.SaveChanges();
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
            dtState.Rows.Clear();
            try
            {
                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    SqlCommand cmd;
                    string str = " SELECT ST.Id,ST.StateName,ST.ShortName,ST.CountryId,CY.CountryName \r" +
                                 " FROM MASTERSTATE ST(NOLOCK) \r" +
                                 " LEFT JOIN MASTERCOUNTRY CY(NOLOCK)ON CY.ID = ST.COUNTRYID \r" +
                                 " WHERE ST.ISCANCEL=0 \r " +
                                 " ORDER BY ST.StateName";

                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    con.Open();
                    adp.Fill(dtState);
                    con.Close();
                    if (dtState.Rows.Count > 0)
                    {
                        dgvState.ItemsSource = dtState.DefaultView;
                        Filteration();
                    }
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
                string sWhere = "";
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    if (rptContain.IsChecked == true)
                    {
                        sWhere = "StateName LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else if (rptEndWith.IsChecked == true)
                    {
                        sWhere = "StateName LIKE '%" + txtSearch.Text.ToUpper() + "'";
                    }
                    else if (rptStartWith.IsChecked == true)
                    {
                        sWhere = "StateName LIKE '" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else
                    {
                        sWhere = "StateName LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                }

                if (!string.IsNullOrEmpty(sWhere))
                {
                    DataView dv = new DataView(dtState);
                    dv.RowFilter = sWhere;
                    DataTable dtTemp = new DataTable();
                    dtTemp = dv.ToTable();
                    dgvState.ItemsSource = dtTemp.DefaultView;
                }
                else
                {
                    dgvState.ItemsSource = dtState.DefaultView;
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
