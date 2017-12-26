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
using System.Data.Entity.Infrastructure;

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
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(dtpDate.Text))
                {
                    if (MessageBox.Show("Old Data's will be Deleted, Are you sure to Generate Salary ? ", "Clear Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
                                ReportSummary();
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
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Function

        void ReportSummary()
        {
            try
            {
                //var PY = (from x in db.MonthlySalaries where x.SalaryMonth orderby x.EmployeeNo select x).ToList();
                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    con.Open();
                    string str = string.Format(" SELECT * FROM MONTHLYSALARY(NOLOCK) WHERE MONTH(SALARYMONTH)=MONTH('{0:dd/MMM/yyyy}') AND YEAR(SALARYMONTH)=YEAR('{0:dd/MMM/yyyy}')", dtpDate.SelectedDate);
                    SqlCommand cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    cmd.CommandTimeout = 0;
                    DataTable dtPayslip = new DataTable();
                    adp.Fill(dtPayslip);
                    if (dtPayslip.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(txtMembershipNo.Text))
                        {
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
                                ReportParameter[] NB = new ReportParameter[2];
                                NB[0] = new ReportParameter("Month", string.Format("{0:MMM yyyy}", dtpDate.SelectedDate));
                                var mas = (from x in db.CompanyDetails select x).FirstOrDefault();
                                if (mas != null)
                                {
                                    NB[1] = new ReportParameter("CompanyName", mas.CompanyName.ToString());
                                }
                                RptPaySlip.LocalReport.SetParameters(NB);
                            }
                            else
                            {
                                RptPaySlip.LocalReport.ReportEmbeddedResource = "NUBE.PAYROLL.PL.Reports.rptPaySlip.rdlc";
                                ReportParameter[] NB = new ReportParameter[7];
                                NB[0] = new ReportParameter("Month", string.Format("{0:MMM yyyy}", dtpDate.SelectedDate));
                                PayrollEntity db = new PayrollEntity();
                                var mas = (from x in db.CompanyDetails where x.Id == 1 select x).FirstOrDefault();

                                if (mas != null)
                                {
                                    NB[1] = new ReportParameter("CompanyPrintName", mas.PrintName.ToString());
                                    NB[2] = new ReportParameter("RobNo", mas.RobNo == "NULL" ? "" : mas.RobNo.ToString());
                                    NB[3] = new ReportParameter("Address1", mas.AddressLine1 == "NULL" ? "" : mas.AddressLine1.ToString());
                                    NB[4] = new ReportParameter("Address2", mas.AddressLine2 == "NULL" ? "" : mas.AddressLine2.ToString());
                                    NB[5] = new ReportParameter("Address3", mas.AddressLine3 == "NULL" ? "" : mas.AddressLine3.ToString());
                                    NB[6] = new ReportParameter("TelNo", mas.TelephoneNo == "NULL" ? "" : mas.TelephoneNo.ToString());
                                }
                                RptPaySlip.LocalReport.SetParameters(NB);
                            }

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion


    }
}
