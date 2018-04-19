using MahApps.Metro.Controls;
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
using System.Data;

namespace NUBE.PAYROLL.PL.Transaction
{
    /// <summary>
    /// Interaction logic for frmYearAllowance.xaml
    /// </summary>
    public partial class frmYearAllowance : MetroWindow
    {
        int iEmployeeId = 0, iAllowanceId = 0, iPCBid = 0, iMonthlyDeductionId = 0, iFormId = 0;
        DateTime dtEntrydate;
        PayrollEntity db = new PayrollEntity();
        public frmYearAllowance(DateTime Entrydate, int EmployeeId = 0, int AllowanceId = 0, int PCBid = 0, int MlyDeductId = 0, int FormId = 0)
        {
            InitializeComponent();
            dtEntrydate = Entrydate;
            iEmployeeId = EmployeeId;
            iAllowanceId = AllowanceId;
            iPCBid = PCBid;
            iMonthlyDeductionId = MlyDeductId;
            iFormId = FormId;
            FormLoad();
        }

        #region EVENTS
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (iFormId == 1)
                {
                    if (string.IsNullOrEmpty(txtBonus.Text))
                    {
                        MessageBox.Show("Bonus is Empty!", "Empty");
                        txtBonus.Focus();
                    }
                    else if (string.IsNullOrEmpty(txtExgratia.Text))
                    {
                        MessageBox.Show("Exgratia is Empty!", "Empty");
                        txtExgratia.Focus();
                    }
                    else
                    {
                        if (Convert.ToDecimal(txtBonus.Text) == 0 || Convert.ToDecimal(txtExgratia.Text) == 0)
                        {
                            if (MessageBox.Show("Bonus or Ex-gratia is Zero!, Are you Want Save ?", "Save Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                SaveBonus();
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("Do you Want to Save ?", "Save Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                SaveBonus();
                            }
                        }
                    }
                }
                else if (iFormId == 2)
                {
                    if (string.IsNullOrEmpty(txtBonus.Text))
                    {
                        MessageBox.Show("PCB is Empty!", "Empty");
                        txtBonus.Focus();
                    }
                    else
                    {
                        if (Convert.ToDecimal(txtBonus.Text) == 0)
                        {
                            if (MessageBox.Show("PCB is Zero!, Are you Want Save ?", "Save Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                SavePCB();
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("Do you Want to Save ?", "Save Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                SavePCB();
                            }
                        }
                    }
                }
                else if (iFormId == 3)
                {
                    if (string.IsNullOrEmpty(txtBonus.Text))
                    {
                        MessageBox.Show("Allowance Advance Amount is Empty!", "Empty");
                        txtBonus.Focus();
                    }
                    else if (string.IsNullOrEmpty(txtExgratia.Text))
                    {
                        MessageBox.Show("Other Deduction Amount is Empty!", "Empty");
                        txtExgratia.Focus();
                    }
                    else
                    {
                        if (Convert.ToDecimal(txtBonus.Text) == 0 || Convert.ToDecimal(txtExgratia.Text) == 0)
                        {
                            if (MessageBox.Show("Allowance Advance or Deduction Amount is Zero!, Are you Want Save ?", "Save Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                SaveMonthlyDeduciton();
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("Do you Want to Save ?", "Save Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                SaveMonthlyDeduciton();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            Config.CheckIsNumeric(e);
        }

        #endregion

        #region FUNCTIONS

        void SaveBonus()
        {
            if (iAllowanceId != 0)
            {
                var ya = (from x in db.YearlyAllowances where x.Id == iAllowanceId && x.EntryDate == dtEntrydate select x).FirstOrDefault();
                if (ya != null)
                {
                    ya.Bonus = Convert.ToDecimal(txtBonus.Text);
                    ya.Exgratia = Convert.ToDecimal(txtExgratia.Text);
                    db.SaveChanges();
                    MessageBox.Show("Updated Successfully!", "PAYROLL");
                    this.Close();
                }
            }
            else
            {
                YearlyAllowance ya = new YearlyAllowance();
                ya.EntryDate = dtEntrydate;
                ya.EmployeeId = iEmployeeId;
                ya.Bonus = Convert.ToDecimal(txtBonus.Text);
                ya.Exgratia = Convert.ToDecimal(txtExgratia.Text);
                db.YearlyAllowances.Add(ya);
                db.SaveChanges();
                MessageBox.Show("Saved Successfully!", "PAYROLL");
                this.Close();
            }
        }

        void SavePCB()
        {
            if (iPCBid != 0)
            {
                var pcb = (from x in db.PCBs where x.Id == iPCBid && x.EntryDate == dtEntrydate select x).FirstOrDefault();
                if (pcb != null)
                {
                    pcb.PCB1 = Convert.ToDecimal(txtBonus.Text);
                    db.SaveChanges();
                    MessageBox.Show("Updated Successfully!", "PAYROLL");
                    this.Close();
                }
            }
            else
            {
                PCB pc = new PCB();
                pc.EntryDate = dtEntrydate;
                pc.EmployeeId = iEmployeeId;
                pc.PCB1 = Convert.ToDecimal(txtBonus.Text);
                db.PCBs.Add(pc);
                db.SaveChanges();
                MessageBox.Show("Saved Successfully!", "PAYROLL");
                this.Close();
            }
        }

        void SaveMonthlyDeduciton()
        {
            if (iMonthlyDeductionId != 0)
            {
                var ya = (from x in db.MonthlyDeductions where x.Id == iMonthlyDeductionId && x.EntryDate == dtEntrydate select x).FirstOrDefault();
                if (ya != null)
                {
                    ya.AllowanceInAdvanced = Convert.ToDecimal(txtBonus.Text);
                    ya.OtherDeductions = Convert.ToDecimal(txtExgratia.Text);
                    if (!string.IsNullOrEmpty(txtDispatchAllowance.Text))
                    {
                        ya.DispatchAllowance = Convert.ToDecimal(txtDispatchAllowance.Text);
                    }
                    db.SaveChanges();
                    MessageBox.Show("Updated Successfully!", "PAYROLL");
                    this.Close();
                }
            }
            else
            {
                MonthlyDeduction ya = new MonthlyDeduction();
                ya.EntryDate = dtEntrydate;
                ya.EmployeeId = iEmployeeId;
                ya.AllowanceInAdvanced = Convert.ToDecimal(txtBonus.Text);
                ya.OtherDeductions = Convert.ToDecimal(txtExgratia.Text);
                if (!string.IsNullOrEmpty(txtDispatchAllowance.Text))
                {
                    ya.DispatchAllowance = Convert.ToDecimal(txtDispatchAllowance.Text);
                }
                ya.CreateBy = 1;
                ya.CreatedOn = DateTime.Now;
                db.MonthlyDeductions.Add(ya);
                db.SaveChanges();
                MessageBox.Show("Saved Successfully!", "PAYROLL");
                this.Close();
            }
        }

        void FormLoad()
        {
            try
            {
                txtBonus.Text = "0";
                txtBonus.Text = "0";
                txtExgratia.Text = "0";
                txtDispatchAllowance.Text = "0";

                var emp = (from x in db.MasterEmployees where x.Id == iEmployeeId select x).FirstOrDefault();
                if (emp != null)
                {
                    txtEmployeeName.Text = emp.EmployeeName;
                    txtMemberID.Text = emp.MembershipNo.ToString();
                    cmbGender.Text = emp.Gender;
                    txtNRIC.Text = emp.NRIC;

                    var mp = (from x in db.MasterPositions where x.Id == emp.PositionId select x).FirstOrDefault();
                    if (mp != null)
                    {
                        txtPosition.Text = mp.PositionName;
                    }
                    else
                    {
                        txtPosition.Text = "";
                    }
                }

                if (iFormId == 1 && iAllowanceId != 0)
                {
                    var alw = (from x in db.YearlyAllowances where x.EmployeeId == iEmployeeId && x.EntryDate == dtEntrydate select x).FirstOrDefault();
                    if (alw != null)
                    {
                        txtBonus.Text = alw.Bonus.ToString();
                        txtExgratia.Text = alw.Exgratia.ToString();
                    }
                }
                else if (iFormId == 2 && iPCBid != 0)
                {
                    var pcb = (from x in db.PCBs where x.EmployeeId == iEmployeeId && x.EntryDate == dtEntrydate select x).FirstOrDefault();
                    if (pcb != null)
                    {
                        txtBonus.Text = pcb.PCB1.ToString();
                    }
                }
                else if (iFormId == 3 && iMonthlyDeductionId != 0)
                {
                    var pcb = (from x in db.MonthlyDeductions where x.EmployeeId == iEmployeeId && x.EntryDate == dtEntrydate select x).FirstOrDefault();
                    if (pcb != null)
                    {
                        txtBonus.Text = pcb.AllowanceInAdvanced.ToString();
                        txtExgratia.Text = pcb.OtherDeductions.ToString();
                        txtDispatchAllowance.Text = pcb.DispatchAllowance.ToString();
                    }
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
