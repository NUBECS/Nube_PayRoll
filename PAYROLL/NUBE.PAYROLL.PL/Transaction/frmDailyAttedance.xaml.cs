using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace NUBE.PAYROLL.PL.Transaction
{
    /// <summary>
    /// Interaction logic for frmDailyAttedance.xaml
    /// </summary>
    public partial class frmDailyAttedance : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        public frmDailyAttedance()
        {
            InitializeComponent();
        }

        #region EVENTS

        private void dtpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DailyAttedance_Loaded(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(dtpDate.Text))
                {
                    LoadAttedance();
                }
                else
                {
                    MessageBox.Show("Entry Date is Empty!");
                    dtpDate.Focus();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearForm();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Function

        void LoadAttedance()
        {
            DateTime dtdate = Convert.ToDateTime(dtpDate.SelectedDate);
            var att = (from x in db.ViewDailyAttedances where x.ATTDATE == dtdate select x).ToList();
            var late = (from x in db.VIEWDAILYATTEDANCELATEs where x.ATTDATE == dtdate select x).ToList();
            DataTable dt = AppLib.LINQResultToDataTable(att);
            DataTable dtLate = AppLib.LINQResultToDataTable(late);
            if (dt.Rows.Count > 0)
            {
                DataTable dtPresent = new DataTable();
                DataTable dtLeave = new DataTable();
                DataTable dtHalf = new DataTable();
                DataTable dtOT = new DataTable();

                DataView dv = new DataView(dt);
                dv.RowFilter = "ISFULLDAYLEAVE<>1";
                dtPresent = dv.ToTable();                

                dv = new DataView(dt);
                dv.RowFilter = "ISFULLDAYLEAVE<>0";
                dtLeave = dv.ToTable();                

                dv = new DataView(dt);
                dv.RowFilter = "ISHALFDAYLEAVE<>0";
                dtHalf = dv.ToTable();              

                dv = new DataView(dt);
                dv.RowFilter = " OT_HOURS>0 OR OT_MINUTES>0";
                dtOT = dv.ToTable();                

                int i = 0;
                foreach (DataRow dr in dtPresent.Rows)
                {
                    i++;
                    dr["RNO"] = i;
                }
                dgPresent.ItemsSource = dtPresent.DefaultView;

                i = 0;
                foreach (DataRow dr in dtLeave.Rows)
                {
                    i++;
                    dr["RNO"] = i;
                }
                dgLeave.ItemsSource = dtLeave.DefaultView;

                i = 0;
                foreach (DataRow dr in dtHalf.Rows)
                {
                    i++;
                    dr["RNO"] = i;
                }
                dgHalf.ItemsSource = dtHalf.DefaultView;
                dgLatecommer.ItemsSource = dtLate.DefaultView;
                i = 0;

                foreach (DataRow dr in dtOT.Rows)
                {
                    i++;
                    dr["RNO"] = i;
                }
                dgOT.ItemsSource = dtOT.DefaultView;

                if (dt.Rows.Count > 0)
                {
                    txtTotalMember.Text = dt.Rows.Count.ToString();
                }
                else
                {
                    txtTotalMember.Text = "0";
                }

                if (dtPresent.Rows.Count > 0)
                {
                    txtPresent.Text = dtPresent.Rows.Count.ToString();
                }
                else
                {
                    txtPresent.Text = "0";
                }

                if (dtLeave.Rows.Count > 0)
                {
                    txtLeave.Text = dtLeave.Rows.Count.ToString();
                }
                else
                {
                    txtLeave.Text = "0";
                }

                if (dtHalf.Rows.Count > 0)
                {
                    txtHalfDayLeave.Text = dtHalf.Rows.Count.ToString();
                }
                else
                {
                    txtHalfDayLeave.Text = "0";
                }

                if (dtLate.Rows.Count > 0)
                {
                    txtLate.Text = dtLate.Rows.Count.ToString();
                }
                else
                {
                    txtLate.Text = "0";
                }

                if (dtOT.Rows.Count > 0)
                {
                    txtOT.Text = dtOT.Rows.Count.ToString();
                }
                else
                {
                    txtOT.Text = "0";
                }
            }
            else
            {
                MessageBox.Show("No Records Found");
            }
        }

        void ClearForm()
        {
            dtpDate.Text = "";
            dgPresent.ItemsSource = null;
            dgLeave.ItemsSource = null;
            dgHalf.ItemsSource = null;
            dgOT.ItemsSource = null;
            dgLatecommer.ItemsSource = null;
            txtTotalMember.Text = "";
            txtPresent.Text = "";
            txtLeave.Text = "";
            txtHalfDayLeave.Text = "";
            txtLeave.Text = "";
            txtOT.Text = "";
        }

        #endregion              
    }
}
