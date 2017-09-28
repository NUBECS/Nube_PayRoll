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
using System.Windows.Shapes;
using NUBE.PAYROLL.CMN;
using System.Data.SqlClient;
using System.Data;

namespace NUBE.PAYROLL.PL.Transaction
{
    /// <summary>
    /// Interaction logic for frmPopupBox.xaml
    /// </summary>
    public partial class frmPopBox : Window
    {
        PayrollEntity db = new PayrollEntity();
        DateTime dt = DateTime.Today;
        Boolean bIsDatewise = false;
        public frmPopBox(string dtt, Boolean bIsDate)
        {
            InitializeComponent();
            dt = Convert.ToDateTime(dtt);
            bIsDatewise = bIsDate;
        }

        private void Window_Activated(object sender, EventArgs e)
        {

        }

        private void Window_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void Window_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    SqlCommand cmd;
                    cmd = new SqlCommand("SPTEMPATTENDANCE", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@DATE", string.Format("{0:dd/MMM/yyyy}", dt)));
                    cmd.Parameters.Add(new SqlParameter("@DAILY", bIsDatewise));
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    con.Open();
                    cmd.CommandTimeout = 0;
                    DataTable dtAttendance = new DataTable();
                    adp.Fill(dtAttendance);

                    if (dtAttendance.Rows.Count > 0)
                    {
                        string sWhere = "";
                        if (bIsDatewise == true)
                        {
                            sWhere = string.Format(" WHERE ENTRYDATE='{0:dd/MMM/yyyy}' \r", dt);
                        }
                        else
                        {
                            sWhere = string.Format(" WHERE MONTH(ENTRYDATE)=MONTH('{0:dd/MMM/yyyy}') AND YEAR(ENTRYDATE)=YEAR('{0:dd/MMM/yyyy}') \r", dt);
                        }

                        String sQuery = " SELECT EMPLOYEEID,ENTRYDATE FROM " +
                                        con.Database + "..TEMPATTENDANCETIMINGS(NOLOCK) \r" +
                                        sWhere + " GROUP BY EMPLOYEEID,ENTRYDATE \r" +
                                        " ORDER BY EMPLOYEEID,ENTRYDATE ";

                        cmd = new SqlCommand(sQuery, con);
                        cmd.CommandType = CommandType.Text;
                        adp = new SqlDataAdapter(cmd);
                        cmd.CommandTimeout = 0;
                        DataTable dtEmployee = new DataTable();
                        adp.Fill(dtEmployee);

                        List<AttedanceLog> lstAttLog = new List<AttedanceLog>();

                        foreach (DataRow drRow in dtEmployee.Rows)
                        {
                            DataView dv = new DataView(dtEmployee);
                            dv.RowFilter = " EMPLOYEEID=" + drRow["EMPLOYEEID"];

                            DataTable dtEmp = dv.ToTable();
                            foreach (DataRow dr in dtEmp.Rows)
                            {
                                DataView drv = new DataView(dtAttendance);
                                drv.RowFilter = string.Format(" EMPLOYEEID=" + drRow["EMPLOYEEID"] + " AND ENTRYDATE='{0:dd/MMM/yyyy}'", Convert.ToDateTime(dr["ENTRYDATE"]));
                                DataTable dtdate = drv.ToTable();

                                for (int i = 0; i < dtdate.Rows.Count; i = i + 2)
                                {
                                    AttedanceLog atLog = new AttedanceLog();
                                    atLog.EmployeeId = int.Parse(drRow["EMPLOYEEID"].ToString());
                                    atLog.EntryDate = Convert.ToDateTime(dtdate.Rows[i]["ENTRYDATE"]);
                                    atLog.InTime = Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]);
                                    atLog.OutTime = Convert.ToDateTime(dtdate.Rows[i + 1]["PUNCHTIME"]);
                                    TimeSpan diff = Convert.ToDateTime(dtdate.Rows[i + 1]["PUNCHTIME"]) - Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]);
                                    atLog.WorkingHours = diff;
                                    lstAttLog.Add(atLog);
                                }
                            }
                        }
                        if (lstAttLog != null)
                        {
                            db.AttedanceLogs.AddRange(lstAttLog);
                            db.SaveChanges();
                        }
                        MessageBox.Show("Import Sucessfully !", "Sucessfully Completed");
                        con.Close();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No Record Found !", "Error");
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Contact Administrator!", "Error");
                this.Close();
            }
        }

        private void Window_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void Window_LayoutUpdated(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Config.connStr))
                {
                    SqlCommand cmd;
                    cmd = new SqlCommand("SPTEMPATTENDANCE", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@DATE", string.Format("{0:dd/MMM/yyyy}", dt)));
                    cmd.Parameters.Add(new SqlParameter("@DAILY", bIsDatewise));
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    con.Open();
                    cmd.CommandTimeout = 0;
                    DataTable dtAttendance = new DataTable();
                    adp.Fill(dtAttendance);

                    if (dtAttendance.Rows.Count > 0)
                    {
                        string sWhere = "";
                        if (bIsDatewise == true)
                        {
                            sWhere = string.Format(" WHERE ENTRYDATE='{0:dd/MMM/yyyy}' \r", dt);
                        }
                        else
                        {
                            sWhere = string.Format(" WHERE MONTH(ENTRYDATE)=MONTH('{0:dd/MMM/yyyy}') AND YEAR(ENTRYDATE)=YEAR('{0:dd/MMM/yyyy}') \r", dt);
                        }

                        String sQuery = " SELECT EMPLOYEEID,ENTRYDATE FROM " +
                                        con.Database + "..TEMPATTENDANCETIMINGS(NOLOCK) \r" +
                                        sWhere + " GROUP BY EMPLOYEEID,ENTRYDATE \r" +
                                        " ORDER BY EMPLOYEEID,ENTRYDATE ";

                        cmd = new SqlCommand(sQuery, con);
                        cmd.CommandType = CommandType.Text;
                        adp = new SqlDataAdapter(cmd);
                        cmd.CommandTimeout = 0;
                        DataTable dtEmployee = new DataTable();
                        adp.Fill(dtEmployee);

                        List<AttedanceLog> lstAttLog = new List<AttedanceLog>();

                        foreach (DataRow drRow in dtEmployee.Rows)
                        {
                            DataView dv = new DataView(dtEmployee);
                            dv.RowFilter = " EMPLOYEEID=" + drRow["EMPLOYEEID"];

                            DataTable dtEmp = dv.ToTable();
                            foreach (DataRow dr in dtEmp.Rows)
                            {
                                DataView drv = new DataView(dtAttendance);
                                drv.RowFilter = string.Format(" EMPLOYEEID=" + drRow["EMPLOYEEID"] + " AND ENTRYDATE='{0:dd/MMM/yyyy}'", Convert.ToDateTime(dr["ENTRYDATE"]));
                                DataTable dtdate = drv.ToTable();

                                for (int i = 0; i < dtdate.Rows.Count; i = i + 2)
                                {
                                    AttedanceLog atLog = new AttedanceLog();
                                    atLog.EmployeeId = int.Parse(drRow["EMPLOYEEID"].ToString());
                                    atLog.EntryDate = Convert.ToDateTime(dtdate.Rows[i]["ENTRYDATE"]);
                                    atLog.InTime = Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]);
                                    atLog.OutTime = Convert.ToDateTime(dtdate.Rows[i + 1]["PUNCHTIME"]);
                                    TimeSpan diff = Convert.ToDateTime(dtdate.Rows[i + 1]["PUNCHTIME"]) - Convert.ToDateTime(dtdate.Rows[i]["PUNCHTIME"]);
                                    atLog.WorkingHours = diff;
                                    lstAttLog.Add(atLog);
                                }
                            }
                        }
                        if (lstAttLog != null)
                        {
                            db.AttedanceLogs.AddRange(lstAttLog);
                            db.SaveChanges();
                        }
                        MessageBox.Show("Import Sucessfully !", "Sucessfully Completed");
                        con.Close();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No Record Found !", "Error");
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Contact Administrator!", "Error");
                this.Close();
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }
    }
}
