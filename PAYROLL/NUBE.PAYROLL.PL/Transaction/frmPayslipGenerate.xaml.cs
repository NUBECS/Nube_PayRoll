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
using System.Data.SqlClient;

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
                    ReportSummary();
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

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(dtpDate.Text))
                {
                    using (SqlConnection con = new SqlConnection(Config.connStr))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("SPMONTHLYSALARY", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ENTRYDATE", string.Format("{0:dd/MMM/yyyy}", dtpDate.SelectedDate)));
                        cmd.CommandTimeout = 0;
                        int i = cmd.ExecuteNonQuery();
                        if (i == 0)
                        {
                            MessageBox.Show("Can't Generate Payslip, Contact Administrator", "Error");
                        }
                        else
                        {
                            var PY = (from x in db.MonthlySalaries orderby x.EmployeeNo select x).ToList();
                            if (PY.Count > 0)
                            {
                                DataTable dtPayslip = AppLib.LINQResultToDataTable(PY);
                                if (!string.IsNullOrEmpty(txtMembershipNo.Text))
                                {
                                    dtPayslip.Rows.Clear();
                                    DataView dv = new DataView(dtPayslip);
                                    dv.RowFilter = "EMPLOYEENO=" + txtMembershipNo.Text;
                                    dtPayslip = dv.ToTable();
                                }
                                if (dtPayslip.Rows.Count > 0)
                                {
                                    RptPaySlip.Reset();
                                    ReportDataSource masterData = new ReportDataSource("payroll", dtPayslip);
                                    RptPaySlip.LocalReport.DataSources.Add(masterData);
                                    if (Config.bIsNubeServer == true)
                                    {
                                        RptPaySlip.LocalReport.ReportEmbeddedResource = "NUBE.PAYROLL.PL.Reports.rptNubePayslip.rdlc";
                                    }
                                    else
                                    {
                                        RptPaySlip.LocalReport.ReportEmbeddedResource = "NUBE.PAYROLL.PL.Reports.rptPaySlip.rdlc";
                                    }

                                    ReportParameter[] NB = new ReportParameter[2];
                                    NB[0] = new ReportParameter("Month", string.Format("{0:MMM yyyy}", dtpDate.SelectedDate));
                                    var mas = (from x in db.CompanyDetails select x).FirstOrDefault();
                                    if (mas != null)
                                    {
                                        NB[1] = new ReportParameter("CompanyName", mas.CompanyName.ToString());
                                    }
                                    RptPaySlip.LocalReport.SetParameters(NB);
                                    RptPaySlip.RefreshReport();
                                }
                                else
                                {
                                    MessageBox.Show("No Record Found!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("No Record Found!");
                            }
                        }
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
            var PY = (from x in db.MonthlySalaries orderby x.EmployeeNo select x).ToList();
            if (PY.Count > 0)
            {
                DataTable dtPayslip = AppLib.LINQResultToDataTable(PY);
                if (!string.IsNullOrEmpty(txtMembershipNo.Text))
                {
                    dtPayslip.Rows.Clear();
                    DataView dv = new DataView(dtPayslip);
                    dv.RowFilter = "EMPLOYEENO=" + txtMembershipNo.Text;
                    dtPayslip = dv.ToTable();
                }

                if (dtPayslip.Rows.Count > 0)
                {
                    RptPaySlip.Reset();
                    ReportDataSource masterData = new ReportDataSource("payroll", dtPayslip);
                    RptPaySlip.LocalReport.DataSources.Add(masterData);
                    if (Config.bIsNubeServer == true)
                    {
                        RptPaySlip.LocalReport.ReportEmbeddedResource = "NUBE.PAYROLL.PL.Reports.rptNubePayslip.rdlc";
                    }
                    else
                    {
                        RptPaySlip.LocalReport.ReportEmbeddedResource = "NUBE.PAYROLL.PL.Reports.rptPaySlip.rdlc";
                    }
                    ReportParameter[] NB = new ReportParameter[2];
                    NB[0] = new ReportParameter("Month", string.Format("{0:MMM yyyy}", dtpDate.SelectedDate));
                    var mas = (from x in db.CompanyDetails select x).FirstOrDefault();
                    if (mas != null)
                    {
                        NB[1] = new ReportParameter("CompanyName", mas.CompanyName.ToString());
                    }
                    RptPaySlip.LocalReport.SetParameters(NB);
                    RptPaySlip.RefreshReport();
                }
                else
                {
                    MessageBox.Show("No Record Found!");
                }
            }
            else
            {
                MessageBox.Show("No Record Found!");
            }

        }

        #endregion


    }
}
