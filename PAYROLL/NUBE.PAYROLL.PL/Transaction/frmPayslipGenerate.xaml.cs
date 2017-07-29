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
using Microsoft.Reporting.WinForms;

namespace NUBE.PAYROLL.PL.Transaction
{
    /// <summary>
    /// Interaction logic for frmPayslipGenerate.xaml
    /// </summary>
    public partial class frmPayslipGenerate : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        public frmPayslipGenerate()
        {
            InitializeComponent();
        }

        #region Field
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(dtpDate.Text))
                {
                    DateTime dt = Convert.ToDateTime(dtpDate.SelectedDate);
                    if (dt.Month == 5)
                    {
                        ReportSummary();
                    }
                    else
                    {
                        MessageBox.Show("No Records Found!");
                        dtpDate.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Please Select the Month");
                    dtpDate.Focus();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        #endregion

        #region Function

        void ReportSummary()
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(txtMembershipNo.Text))
            {
                int iCode = Convert.ToInt32(txtMembershipNo.Text);

                var vpay = (from x in db.VIEWPAYSLIPs where x.CODE == iCode select x).ToList();
                dt = AppLib.LINQResultToDataTable(vpay);
            }
            else
            {
                var vpay = (from x in db.VIEWPAYSLIPs select x).ToList();
                dt = AppLib.LINQResultToDataTable(vpay);
            }

            RptPaySlip.Reset();
            ReportDataSource masterData = new ReportDataSource("payroll", dt);
            RptPaySlip.LocalReport.DataSources.Add(masterData);
            RptPaySlip.LocalReport.ReportEmbeddedResource = "NUBE.PAYROLL.PL.Reports.rptPaySlip.rdlc";
            ReportParameter prm = new ReportParameter("Month", string.Format("{0:MMM yyyy}", dtpDate.SelectedDate));
            RptPaySlip.LocalReport.SetParameters(prm);
            //RptPaySlip.LocalReport.ReportPath = @"NUBE.PAYROLL.PL\Reports\rptPaySlip.rdlc";
            RptPaySlip.RefreshReport();
        }

        #endregion
    }
}
