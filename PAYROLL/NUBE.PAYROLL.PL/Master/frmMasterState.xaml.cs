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
        public frmMasterState()
        {
            InitializeComponent();

        }

        #region EVENTS

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var cu = (from x in db.MasterCountries select x).ToList();
                cmbCountry.ItemsSource = cu;
                cmbCountry.SelectedValuePath = "Id";
                cmbCountry.DisplayMemberPath = "CountryName";
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
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    SqlCommand cmd;
                    string str = " SELECT ST.Id,ST.StateName,ST.ShortName,ST.CountryId,CY.CountryName \r" +
                                 " FROM MASTERSTATE ST(NOLOCK) \r" +
                                 " LEFT JOIN MASTERCOUNTRY CY(NOLOCK)ON CY.ID = ST.COUNTRYID \r" +
                                 " ORDER BY ST.StateName";

                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    con.Open();
                    adp.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        dgvState.ItemsSource = dt.DefaultView;
                    }
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
