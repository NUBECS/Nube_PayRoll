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
    /// Interaction logic for frmHolidayList.xaml
    /// </summary>
    public partial class frmHolidayList : UserControl
    {
        int Id = 0;
        PayrollEntity db = new PayrollEntity();
        DataTable dtHoliday = new DataTable();
        public frmHolidayList()
        {
            InitializeComponent();
        }

        #region EVENTS

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtHolidayName.Text))
                {
                    MessageBox.Show("HolidayName is Empty!", "Empty");
                    txtHolidayName.Focus();
                }
                else if (string.IsNullOrEmpty(txtDiscription.Text))
                {
                    MessageBox.Show("Discription is Empty!", "Empty");
                    txtDiscription.Focus();
                }
                else if (string.IsNullOrEmpty(dtDate.Text))
                {
                    MessageBox.Show("Date is Empty!");
                    dtDate.Focus();
                }
                else
                {
                    if (Id != 0)
                    {
                        var mb = (from x in db.HolidayLists where x.Id == Id select x).FirstOrDefault();
                        mb.HolidayName = txtHolidayName.Text;
                        mb.Discription = txtDiscription.Text;
                        mb.HolidayDate = Convert.ToDateTime(dtDate.SelectedDate);
                        db.SaveChanges();
                        MessageBox.Show("Updated Sucessfully!");
                        LoadWindow();
                    }
                    else
                    {
                        HolidayList mb = new HolidayList();
                        mb.HolidayName = txtHolidayName.Text;
                        mb.Discription = txtDiscription.Text;
                        mb.HolidayDate = Convert.ToDateTime(dtDate.SelectedDate);
                        db.HolidayLists.Add(mb);
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
                        var mb = (from x in db.HolidayLists where x.Id == Id select x).FirstOrDefault();
                        db.HolidayLists.Remove(mb);
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

        private void dgHoliday_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Id = 0;
                txtHolidayName.Text = "";
                txtDiscription.Text = "";
                dtDate.Text = "";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgHoliday_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgHoliday.SelectedItem != null))
                {
                    DataRowView drv = (DataRowView)dgHoliday.SelectedItem;
                    Id = Convert.ToInt32(drv["Id"]);
                    txtHolidayName.Text = drv["HolidayName"].ToString();
                    dtDate.SelectedDate = Convert.ToDateTime(drv["HolidayDate"]);
                    txtDiscription.Text = drv["Discription"].ToString();
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWindow();
        }

        #endregion

        #region FUNCITONS

        void LoadWindow()
        {
            Id = 0;
            txtHolidayName.Text = "";
            txtSearch.Text = "";
            txtDiscription.Text = "";
            dtDate.Text = "";
            try
            {
                var holi = (from x in db.HolidayLists select x).ToList();
                if (holi != null)
                {
                    dtHoliday.Rows.Clear();
                    dtHoliday = AppLib.LINQResultToDataTable(holi);
                    dgHoliday.ItemsSource = dtHoliday.DefaultView;
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
                string sWhere = "";
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    if (rptContain.IsChecked == true)
                    {
                        sWhere = " HolidayName LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else if (rptEndWith.IsChecked == true)
                    {
                        sWhere = " HolidayName LIKE '%" + txtSearch.Text.ToUpper() + "'";
                    }
                    else if (rptStartWith.IsChecked == true)
                    {
                        sWhere = " HolidayName LIKE '" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else
                    {
                        sWhere = " HolidayName LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                }

                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    DataView dv = new DataView(dtHoliday);
                    dv.RowFilter = sWhere;
                    DataTable dtTemp = dv.ToTable();
                    dgHoliday.ItemsSource = dtTemp.DefaultView;
                }
                else
                {
                    dgHoliday.ItemsSource = dtHoliday.DefaultView;
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
