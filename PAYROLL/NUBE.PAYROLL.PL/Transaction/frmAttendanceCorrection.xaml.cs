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
using System.Data;
using NUBE.PAYROLL.CMN;
using System.Data.SqlClient;

namespace NUBE.PAYROLL.PL.Transaction
{
    /// <summary>
    /// Interaction logic for frmAttendanceCorrection.xaml
    /// </summary>
    public partial class frmAttendanceCorrection : UserControl
    {
        DataTable dtAttedanceCorrection = new DataTable();
        public frmAttendanceCorrection()
        {
            InitializeComponent();
        }

        #region Events

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //dgAttedanceCorrection.Columns[7].Visibility = Visibility.Collapsed;
            FormLoading();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(dtMonth.Text))
                {
                    FormFill();
                }
                else
                {
                    MessageBox.Show("Date is Empty!");
                    dtMonth.Focus();
                    return;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FormLoading();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgAttedanceCorrection_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgAttedanceCorrection.SelectedItem != null))
                {

                    DataRowView drv = (DataRowView)dgAttedanceCorrection.SelectedItem;
                    DateTime dt;
                    dt = string.IsNullOrEmpty(drv["ENTRYDATE"].ToString()) ? Convert.ToDateTime(dtMonth.SelectedDate) : Convert.ToDateTime(drv["ENTRYDATE"]);

                    frmAttedanceCorrectionDetails frm = new frmAttedanceCorrectionDetails((drv["ENTRYDATE"]).ToString(), Convert.ToInt32(drv["EMPLOYEEID"]), +
                                                        Convert.ToInt32(drv["MEMBERSHIPNO"]), drv["EMPLOYEENAME"].ToString(), drv["GENDER"].ToString(), Convert.ToBoolean(drv["ISMODIFIED"]), Convert.ToInt32(drv["ID"]));
                    frm.ShowDialog();
                    FormFill();
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

        private void chkIsModified_Click(object sender, RoutedEventArgs e)
        {

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


        #endregion

        #region Functions

        void FormLoading()
        {
            dgAttedanceCorrection.ItemsSource = null;
            chkIsModified.IsChecked = false;
            dtMonth.Text = "";
            rbDaily.IsChecked = false;
            rbMonthly.IsChecked = true;
        }

        void FormFill()
        {
            try
            {
                if (!string.IsNullOrEmpty(dtMonth.Text))
                {
                    string sWhere = "";
                    if (rbMonthly.IsChecked == true)
                    {
                        sWhere = string.Format(" AND MONTH(AL.ENTRYDATE)=MONTH('{0:dd/MMM/yyyy}') AND YEAR(AL.ENTRYDATE)=YEAR('{0:dd/MMM/yyyy}')", dtMonth.SelectedDate);
                    }
                    else
                    {
                        sWhere = string.Format(" AND AL.ENTRYDATE='{0:dd/MMM/yyyy}' ", dtMonth.SelectedDate);
                    }
                    using (SqlConnection con = new SqlConnection(Config.connStr))
                    {
                        SqlCommand cmd;
                        string str = "";
                        dtAttedanceCorrection.Rows.Clear();
                        if (chkIsModified.IsChecked == true)
                        {
                            str = " SELECT ROW_NUMBER() OVER(ORDER BY ENTRYDATE,ISNULL(AL.INTIME,ISNULL(AL.OUTTIME,ISNULL(AL.OTOUTTIME,ISNULL(AL.OUTTIME,'')))) ASC) AS RNO, \r" +
                           " AL.ID,AL.EMPLOYEEID,EM.MEMBERSHIPNO,EM.EMPLOYEENAME,EM.GENDER,CONVERT(VARCHAR(10),AL.ENTRYDATE,103)ENTRYDATE, \r" +
                           " RIGHT(CONVERT(VARCHAR(32),DA.INTIME ,100),8)INTIME,AL.ISMODIFIED, \r" +
                           " RIGHT(CONVERT(VARCHAR(32),DA.OUTTIME ,100),8)OUTTIME \r" +
                           " FROM ATTEDANCELOGS AL(NOLOCK) \r" +
                           " LEFT JOIN MASTEREMPLOYEE EM(NOLOCK) ON EM.ID=AL.EMPLOYEEID \r" +
                           " LEFT JOIN DAILYATTEDANCEDET DA(NOLOCK) ON DA.EMPLOYEEID=EM.ID AND DA.ATTDATE=AL.ENTRYDATE \r" +
                           " WHERE AL.ISNOTLOGOUT=1 AND AL.ISMODIFIED=1 " + sWhere;
                        }
                        else
                        {
                            str = " SELECT ROW_NUMBER() OVER(ORDER BY ENTRYDATE,ISNULL(AL.INTIME,ISNULL(AL.OUTTIME,ISNULL(AL.OTOUTTIME,ISNULL(AL.OUTTIME,'')))) ASC) AS RNO, \r" +
                           " AL.ID,AL.EMPLOYEEID,EM.MEMBERSHIPNO,EM.EMPLOYEENAME,EM.GENDER,CONVERT(VARCHAR(10),AL.ENTRYDATE,103)ENTRYDATE,RIGHT(CONVERT(VARCHAR(32),ISNULL(AL.OTOUTTIME,ISNULL(AL.OUTTIME,'')),100),8)OUTTIME, \r" +
                           " RIGHT(CONVERT(VARCHAR(32),ISNULL(AL.INTIME,ISNULL(AL.OUTTIME,ISNULL(AL.OTOUTTIME,ISNULL(AL.OUTTIME,'')))),100),8)INTIME,AL.ISMODIFIED \r" +
                           " FROM ATTEDANCELOGS AL(NOLOCK) \r" +
                           " LEFT JOIN MASTEREMPLOYEE EM(NOLOCK) ON EM.ID=AL.EMPLOYEEID \r" +
                           " WHERE ISNOTLOGOUT=1 AND AL.ISMODIFIED=0 " + sWhere;
                        }

                        cmd = new SqlCommand(str, con);
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        con.Open();
                        adp.Fill(dtAttedanceCorrection);
                        con.Close();
                        if (dtAttedanceCorrection.Rows.Count > 0)
                        {
                            dgAttedanceCorrection.ItemsSource = dtAttedanceCorrection.DefaultView;
                            Filteration();
                        }
                        else
                        {
                            MessageBox.Show("No Records Found", "Empty");
                        }
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
                        sWhere = " EMPLOYEENAME LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else if (rptEndWith.IsChecked == true)
                    {
                        sWhere = " EMPLOYEENAME LIKE '%" + txtSearch.Text.ToUpper() + "'";
                    }
                    else if (rptStartWith.IsChecked == true)
                    {
                        sWhere = " EMPLOYEENAME LIKE '" + txtSearch.Text.ToUpper() + "%'";
                    }
                    else
                    {
                        sWhere = " EMPLOYEENAME LIKE '%" + txtSearch.Text.ToUpper() + "%'";
                    }

                    if (!string.IsNullOrEmpty(txtSearch.Text))
                    {
                        DataView dv = new DataView(dtAttedanceCorrection);
                        dv.RowFilter = sWhere;
                        DataTable dtTemp = new DataTable();
                        dtTemp = dv.ToTable();
                        dgAttedanceCorrection.ItemsSource = dtTemp.DefaultView;
                    }
                    else
                    {
                        dgAttedanceCorrection.ItemsSource = dtAttedanceCorrection.DefaultView;
                    }
                }
                else
                {
                    dgAttedanceCorrection.ItemsSource = dtAttedanceCorrection.DefaultView;
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
