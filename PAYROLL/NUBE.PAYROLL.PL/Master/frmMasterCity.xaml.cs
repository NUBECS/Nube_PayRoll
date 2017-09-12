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
    /// Interaction logic for frmMasterCity.xaml
    /// </summary>
    public partial class frmMasterCity : UserControl
    {
        int Id = 0;
        PayrollEntity db = new PayrollEntity();
        DataTable dtCity = new DataTable();
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
                if (string.IsNullOrEmpty(txtCityName.Text))
                {
                    MessageBox.Show("City Name is Empty!", "Empty");
                    txtCityName.Focus();
                }
                else if (string.IsNullOrEmpty(txtCityShortName.Text))
                {
                    MessageBox.Show("Short Name is Empty!", "Empty");
                    txtCityShortName.Focus();
                }
                else if (string.IsNullOrEmpty(cmbState.Text))
                {
                    MessageBox.Show("State is Empty!", "Empty");
                    cmbState.Focus();
                }
                else
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
                        var mb = (from x in db.MasterCities where x.Id == Id select x).FirstOrDefault();
                        mb.IsCancel = true;
                        mb.CancelOn = DateTime.Now;
                        //db.MasterCities.Remove(mb);
                        db.SaveChanges();
                        MessageBox.Show("Deleted Sucessfully");
                        LoadWindow();
                    }
                }
            }
            catch (Exception ex)
            {
                // ExceptionLogging.SendErrorToText(ex);
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

        private void txtSearch_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            Filteration();
        }
        #endregion

        #region FUNCTIONS

        void LoadWindow()
        {
            Id = 0;
            txtCityName.Text = "";
            txtCityShortName.Text = "";
            cmbState.Text = "";
            dtCity.Rows.Clear();
            try
            {
                DataTable dt = new DataTable();
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
                    adp.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        cmbState.ItemsSource = null;
                        cmbState.ItemsSource = dt.DefaultView;
                        cmbState.SelectedValuePath = "Id";
                        cmbState.DisplayMemberPath = "StateName";
                    }
                }

                var st = (from x in db.ViewMasterCities select x).ToList();
                if (st != null)
                {
                    dtCity = AppLib.LINQResultToDataTable(st);
                    dgvCity.ItemsSource = dtCity.DefaultView;
                }
                Filteration();
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
                        sWhere = "CityName LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else if (rptEndWith.IsChecked == true)
                    {
                        sWhere = "CityName LIKE '%" + txtSearch.Text.ToUpper() + "'";
                    }
                    else if (rptStartWith.IsChecked == true)
                    {
                        sWhere = "CityName LIKE '" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else
                    {
                        sWhere = "CityName LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                }

                if (!string.IsNullOrEmpty(sWhere))
                {
                    DataView dv = new DataView(dtCity);
                    dv.RowFilter = sWhere;
                    DataTable dtTemp = new DataTable();
                    dtTemp = dv.ToTable();
                    dgvCity.ItemsSource = dtTemp.DefaultView;
                }
                else
                {
                    dgvCity.ItemsSource = dtCity.DefaultView;
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
