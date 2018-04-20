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
    public partial class frmSalaryA3Format : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        public frmSalaryA3Format()
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
                       
                        if (dtPayslip.Rows.Count > 0)
                        {
                            RptPaySlip.Reset();
                            ReportDataSource masterData = new ReportDataSource("Salary", dtPayslip);
                            RptPaySlip.LocalReport.DataSources.Add(masterData);
                            
                            {
                                RptPaySlip.LocalReport.ReportEmbeddedResource = "NUBE.PAYROLL.PL.Reports.rptSalaryFormat.rdlc";
                                ReportParameter[] NB = new ReportParameter[3];
                                NB[0] = new ReportParameter("Month", string.Format("{0:MMM} ' {0:yyyy}", dtpDate.SelectedDate));
                                var mas = (from x in db.CompanyDetails select x).FirstOrDefault();
                                if (mas != null)
                                {
                                    NB[1] = new ReportParameter("CompanyName", mas.CompanyName.ToString()+" Sdn Bhd");
                                    NB[2] = new ReportParameter("SalaryDate", mas.ATTENDDATE.ToString());
                                    //NB[3] = new ReportParameter("NextSalaryDate", mas.ATTENDDATE.ToString());
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
