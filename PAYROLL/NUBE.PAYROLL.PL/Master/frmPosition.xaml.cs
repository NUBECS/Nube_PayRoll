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
    /// Interaction logic for frmPosition.xaml
    /// </summary>
    public partial class frmPosition : UserControl
    {
        int Id = 0;
        int iDetailId = 0;
        DataTable dtMasterPosition = new DataTable();
        PayrollEntity db = new PayrollEntity();
        List<PositionDetail> lstPosition = new List<PositionDetail>();
        public frmPosition()
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
                if (string.IsNullOrEmpty(txtPositionName.Text))
                {
                    MessageBox.Show("Position Name is Empty!", "Empty");
                    txtPositionName.Focus();
                }
                else if (string.IsNullOrEmpty(txtPositionUserCode.Text))
                {
                    MessageBox.Show("Short Name is Empty!", "Empty");
                    txtPositionUserCode.Focus();
                }
                else if (lstPosition.Count == 0)
                {
                    MessageBox.Show("Yearly Leave is Empty!", "Empty");
                    txtMinYear.Focus();
                }
                else if (MessageBox.Show("Do You want to Save Position ?", "Save Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (Id != 0)
                    {
                        var mb = (from x in db.MasterPositions where x.Id == Id select x).FirstOrDefault();
                        mb.PositionName = txtPositionName.Text;
                        mb.ShortName = txtPositionUserCode.Text;
                        db.SaveChanges();

                        var ptd = (from x in db.PositionDetails where x.PositionId == Id select x).FirstOrDefault();

                        if (ptd != null)
                        {
                            db.PositionDetails.RemoveRange(db.PositionDetails.Where(x => x.PositionId == Id));
                            db.SaveChanges();
                        }

                        DataTable dt = new DataTable();
                        dt = ((DataView)dgvAnnualLeave.ItemsSource).ToTable();
                        List<PositionDetail> lstp = new List<PositionDetail>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            PositionDetail pd = new PositionDetail();
                            pd.PositionId = Id;
                            pd.MinYear = Convert.ToDecimal(dr["MinYear"]);
                            pd.MaxYear = Convert.ToDecimal(dr["MaxYear"]);
                            pd.NoOfLeave = Convert.ToDecimal(dr["NoOfLeave"]);
                            pd.NoOfMedicalLeave= Convert.ToDecimal(dr["NoOfMedicalLeave"]);
                            lstp.Add(pd);
                        }
                        db.PositionDetails.AddRange(lstp);
                        db.SaveChanges();
                        MessageBox.Show("Updated Sucessfully!");
                        LoadWindow();
                    }
                    else
                    {
                        MasterPosition mb = new MasterPosition();
                        mb.PositionName = txtPositionName.Text;
                        mb.ShortName = txtPositionUserCode.Text;
                        db.MasterPositions.Add(mb);
                        db.SaveChanges();

                        int sFid = Convert.ToInt32(db.MasterPositions.Max(x => x.Id));
                        DataTable dt = new DataTable();
                        dt = ((DataView)dgvAnnualLeave.ItemsSource).ToTable();
                        List<PositionDetail> lstp = new List<PositionDetail>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            PositionDetail pd = new PositionDetail();
                            pd.PositionId = sFid;
                            pd.MinYear = Convert.ToDecimal(dr["MinYear"]);
                            pd.MaxYear = Convert.ToDecimal(dr["MaxYear"]);
                            pd.NoOfLeave = Convert.ToDecimal(dr["NoOfLeave"]);
                            pd.NoOfMedicalLeave = Convert.ToDecimal(dr["NoOfMedicalLeave"]);
                            lstp.Add(pd);
                        }
                        db.PositionDetails.AddRange(lstp);
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
                if (MessageBox.Show("Do You want to Delete Position ?", "Delete Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (Id != 0)
                    {
                        var ptd = (from x in db.PositionDetails where x.PositionId == Id select x).FirstOrDefault();

                        if (ptd != null)
                        {
                            db.PositionDetails.RemoveRange(db.PositionDetails.Where(x => x.PositionId == Id));
                            db.SaveChanges();
                        }

                        var mb = (from x in db.MasterPositions where x.Id == Id select x).FirstOrDefault();
                        db.MasterPositions.Remove(mb);
                        db.SaveChanges();                        

                        MessageBox.Show("Deleted Sucessfully");
                        LoadWindow();
                    }
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PositionDetail mb = new PositionDetail();
                if (Id != 0)
                {
                    mb.PositionId = Id;
                }
                else
                {
                    mb.PositionId = 1;
                }
                mb.MinYear = Convert.ToDecimal(txtMinYear.Text);
                mb.MaxYear = Convert.ToDecimal(txtMaxYear.Text);
                mb.NoOfLeave = Convert.ToDecimal(txtNoOfLeave.Text);
                mb.NoOfMedicalLeave = Convert.ToDecimal(txtNoOfMedicalLeave.Text);
                lstPosition.Add(mb);
                db.SaveChanges();
                LstPosition();
                MessageBox.Show("Added Sucessfully!");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnDelet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstPosition.Count > 0)
                {
                    if (MessageBox.Show("Do You want to Delete This Annual Leave Details ?", "Delete Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        lstPosition.Clear();
                        dgvAnnualLeave.ItemsSource = null;
                        MessageBox.Show("Deleted Sucessfully");
                    }
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Empty");
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

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filteration();
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

        private void dgvPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Id = 0;
            iDetailId = 0;
            txtPositionName.Text = "";
            txtPositionUserCode.Text = "";
            txtMinYear.Text = "";
            txtMaxYear.Text = "";
            txtNoOfLeave.Text = "";
            txtNoOfMedicalLeave.Text = "";
            dgvAnnualLeave.ItemsSource = null;
            lstPosition.Clear();
        }

        private void dgvPosition_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgvPosition.SelectedItem != null))
                {
                    DataRowView drv = (DataRowView)dgvPosition.SelectedItem;
                    Id = Convert.ToInt32(drv["Id"]);
                    txtPositionName.Text = drv["PositionName"].ToString();
                    txtPositionUserCode.Text = drv["ShortName"].ToString();
                    LoadPosition();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgvAnnualLeave_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    if ((dgvAnnualLeave.SelectedItem != null))
            //    {
            //        DataRowView drv = (DataRowView)dgvAnnualLeave.SelectedItem;
            //        iDetailId = Convert.ToInt32(drv["Id"]);
            //        txtMinYear.Text = drv["MinYear"].ToString();
            //        txtMaxYear.Text = drv["MaxYear"].ToString();
            //        txtNoOfLeave.Text = drv["NoOfLeave"].ToString();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogging.SendErrorToText(ex);
            //}
        }

        private void dgvAnnualLeave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                iDetailId = 0;
                txtMinYear.Text = "";
                txtMaxYear.Text = "";
                txtNoOfLeave.Text = "";
                txtNoOfMedicalLeave.Text = "";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        #endregion

        #region FUNCITONS

        void LoadWindow()
        {
            Id = 0;
            txtPositionName.Text = "";
            txtPositionUserCode.Text = "";
            txtMinYear.Text = "";
            txtMaxYear.Text = "";
            txtNoOfLeave.Text = "";
            txtNoOfMedicalLeave.Text = "";
            lstPosition.Clear();
            dgvAnnualLeave.ItemsSource = null;

            try
            {
                var pos = (from x in db.MasterPositions select x).ToList();
                if (pos != null)
                {
                    dtMasterPosition = AppLib.LINQResultToDataTable(pos);
                    dgvPosition.ItemsSource = dtMasterPosition.DefaultView;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        void LstPosition()
        {
            txtMinYear.Text = "";
            txtMaxYear.Text = "";
            txtNoOfLeave.Text = "";
            txtNoOfMedicalLeave.Text = "";
            if (lstPosition.Count > 0)
            {
                DataTable dt = AppLib.LINQResultToDataTable(lstPosition);
                dgvAnnualLeave.ItemsSource = dt.DefaultView;
            }
        }

        void LoadPosition()
        {
            txtMinYear.Text = "";
            txtMaxYear.Text = "";
            txtNoOfLeave.Text = "";
            txtNoOfMedicalLeave.Text = "";
            if (Id != 0)
            {
                var pos = (from x in db.PositionDetails where x.PositionId == Id orderby x.MinYear select x).ToList();
                if (pos.Count > 0)
                {
                    lstPosition = pos.ToList();
                    DataTable dt = AppLib.LINQResultToDataTable(lstPosition);
                    dgvAnnualLeave.ItemsSource = dt.DefaultView;
                }
            }
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
                        sWhere = "PositionName LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else if (rptEndWith.IsChecked == true)
                    {
                        sWhere = "PositionName LIKE '%" + txtSearch.Text.ToUpper() + "'";
                    }
                    else if (rptStartWith.IsChecked == true)
                    {
                        sWhere = "PositionName LIKE '" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else
                    {
                        sWhere = "PositionName LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                }
                else
                {
                    if (rptContain.IsChecked == true)
                    {
                        sWhere = "PositionName LIKE '%" + txtSearch.Text + "%'";
                    }
                    else if (rptEndWith.IsChecked == true)
                    {
                        sWhere = "PositionName LIKE '%" + txtSearch.Text + "'";
                    }
                    else if (rptStartWith.IsChecked == true)
                    {
                        sWhere = "PositionName LIKE '" + txtSearch.Text + "%'";
                    }
                    else
                    {
                        sWhere = "PositionName LIKE '%" + txtSearch.Text + "%'";
                    }
                }
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    DataView dv = new DataView(dtMasterPosition);
                    dv.RowFilter = sWhere;
                    DataTable dtTemp = new DataTable();
                    dtTemp = dv.ToTable();
                    dgvPosition.ItemsSource = dtTemp.DefaultView;
                }
                else
                {
                    dgvPosition.ItemsSource = dtMasterPosition.DefaultView;
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
