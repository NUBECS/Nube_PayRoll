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
        public frmHolidayList()
        {
            InitializeComponent();
        }

        #region EVENTS

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

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(dtDate.Text))
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
                else
                {
                    MessageBox.Show("Date is Empty!");
                    dtDate.Focus();
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
                    var mb = (from x in db.HolidayLists where x.Id == Id select x).FirstOrDefault();
                    db.HolidayLists.Remove(mb);
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
                    DataTable dt = AppLib.LINQResultToDataTable(holi);
                    dgHoliday.ItemsSource = dt.DefaultView;
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
