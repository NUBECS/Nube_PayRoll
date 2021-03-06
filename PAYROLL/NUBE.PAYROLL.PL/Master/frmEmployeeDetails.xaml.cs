﻿using MahApps.Metro.Controls;
using NUBE.PAYROLL.CMN;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NUBE.PAYROLL.PL.Master
{
    /// <summary>
    /// Interaction logic for frmEmployeeDetails.xaml
    /// </summary>
    public partial class frmEmployeeDetails : MetroWindow
    {
        int Id = 0;
        Boolean bValidate = true;
        PayrollEntity db = new PayrollEntity();
        public frmEmployeeDetails(int id = 0)
        {
            InitializeComponent();
            Id = id;

            WindowsLoaded();
            if (Id != 0)
            {
                FormFill();
            }
        }

        #region EVENTS

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bValidate = true;
                FormValidation();
                if (bValidate == true)
                {
                    if (Id != 0)
                    {
                        var emp = (from x in db.MasterEmployees where x.Id == Id select x).FirstOrDefault();
                        emp.EmployeeName = txtEmployeeName.Text;
                        emp.MembershipNo = string.IsNullOrEmpty(txtMemberID.Text) ? 0 : Convert.ToInt32(txtMemberID.Text);
                        emp.NRIC = txtNRIC.Text;
                        emp.Gender = cmbGender.Text;
                        emp.PositionId = string.IsNullOrEmpty(cmbPosition.Text) ? 0 : Convert.ToInt32(cmbPosition.SelectedValue);
                        emp.Address1 = txtAddress1.Text;
                        emp.Address2 = txtAddress2.Text;
                        emp.Address3 = txtAddress3.Text;
                        emp.ZipCode = txtPostalCode.Text;
                        emp.CityId = string.IsNullOrEmpty(cmbCity.Text) ? 0 : Convert.ToInt32(cmbCity.SelectedValue);
                        emp.StateId = string.IsNullOrEmpty(cmbState.Text) ? 0 : Convert.ToInt32(cmbState.SelectedValue);
                        emp.CountryId = string.IsNullOrEmpty(cmbCountry.Text) ? 0 : Convert.ToInt32(cmbCountry.SelectedValue);
                        emp.TelephoneNo = txtTelephone.Text;
                        emp.EmergencyNo = txtEmergencyContactNo.Text;
                        emp.MobileNo = txtMobile.Text;
                        emp.BankId = string.IsNullOrEmpty(cmbBank.Text) ? 0 : Convert.ToInt32(cmbBank.SelectedValue);
                        emp.DateOfBirth = Convert.ToDateTime(dtDOB.SelectedDate);
                        emp.RaceId = string.IsNullOrEmpty(cmbRace.Text) ? 0 : Convert.ToInt32(cmbRace.SelectedValue);
                        emp.DateOfJoining = Convert.ToDateTime(dtDOJ.SelectedDate);
                        emp.Email = txtMail.Text;
                        emp.EPFNumber = txtEPFNo.Text;
                        emp.EPFNumber = txtEPFNo1.Text;
                        emp.SOCSONumber = txtSOCSONo.Text;
                        emp.SOCSONumber = txtSOCSONo1.Text;
                        emp.SIPNumber = txtSIPNumber.Text;
                        emp.ShiftId = Convert.ToInt32(cmbShift.SelectedValue);
                        emp.MobileAllowance = string.IsNullOrEmpty(txtMobileAllowance.Text) ? 0 : Convert.ToDecimal(txtMobileAllowance.Text);

                        emp.BankAccountNo = txtAccountNo.Text;
                        emp.BasicSalary = string.IsNullOrEmpty(txtBasicSalary.Text) ? 0 : Convert.ToDecimal(txtBasicSalary.Text);
                        emp.POB = string.IsNullOrEmpty(txtPOB.Text) ? 0 : Convert.ToDecimal(txtPOB.Text);
                        emp.NEC = string.IsNullOrEmpty(txtNEC.Text) ? 0 : Convert.ToDecimal(txtNEC.Text);
                        emp.SECONDMENT = string.IsNullOrEmpty(txtSECONDMENT.Text) ? 0 : Convert.ToDecimal(txtSECONDMENT.Text);
                        emp.SPECIAL = string.IsNullOrEmpty(txtSPECIAL.Text) ? 0 : Convert.ToDecimal(txtSPECIAL.Text);
                        emp.COLA = string.IsNullOrEmpty(txtCOLA.Text) ? 0 : Convert.ToDecimal(txtCOLA.Text);

                        emp.IsOverTimeEligible = Convert.ToBoolean(ChkOTEligible.IsChecked);
                        emp.EPFContributionRate = Convert.ToBoolean(ChkEPFCONTRIBUTIONRATE.IsChecked);
                        emp.EPFContribution = Convert.ToBoolean(ChkEPFCONTRIBUTION.IsChecked);
                        emp.SOCSOContribution = Convert.ToBoolean(ChkSOCSOCONTRIBUTION.IsChecked);
                        emp.EPFContribution = Convert.ToBoolean(ChkEPF1.IsChecked);
                        emp.SOCSOContribution = Convert.ToBoolean(ChkSOCSO1.IsChecked);
                        emp.SIP = Convert.ToBoolean(ChkSIP.IsChecked);

                        emp.KOPERASI = string.IsNullOrEmpty(txtKOPERASI.Text) ? 0 : Convert.ToDecimal(txtKOPERASI.Text);
                        emp.GMIS = string.IsNullOrEmpty(txtGMIS.Text) ? 0 : Convert.ToDecimal(txtGMIS.Text);
                        emp.BIMBLoan = string.IsNullOrEmpty(txtBIMBLOAN.Text) ? 0 : Convert.ToDecimal(txtBIMBLOAN.Text);
                        emp.GELA = string.IsNullOrEmpty(txtGELA.Text) ? 0 : Convert.ToDecimal(txtGELA.Text);
                        emp.HomeCarLoans = string.IsNullOrEmpty(txtHOMECARLOANS.Text) ? 0 : Convert.ToDecimal(txtHOMECARLOANS.Text);
                        emp.OtherLoans = string.IsNullOrEmpty(txtOtherLoans.Text) ? 0 : Convert.ToDecimal(txtOtherLoans.Text);
                        emp.OtherLoanTotal = string.IsNullOrEmpty(txtTotalOtherLoan.Text) ? 0 : Convert.ToDecimal(txtTotalOtherLoan.Text);

                        emp.PCB = Convert.ToBoolean(ChkPCB.IsChecked);
                        emp.UnpaidLeave = Convert.ToBoolean(ChkUNPAIDLEAVE.IsChecked);
                        emp.IsOtherEligible = Convert.ToBoolean(ChkOTHERS.IsChecked);
                        emp.Others = string.IsNullOrEmpty(txtOTHERS.Text) ? 0 : Convert.ToDecimal(txtOTHERS.Text);
                        emp.RatioOfContribution = Convert.ToBoolean(ChkRATIOOFCONTRIBUTION.IsChecked);
                        emp.EPF = Convert.ToBoolean(ChkEPF.IsChecked);
                        emp.SOCSO = Convert.ToBoolean(ChkSOCSO.IsChecked);
                        if (!string.IsNullOrEmpty(dtResigned.Text))
                        {
                            emp.ResignedDate = Convert.ToDateTime(dtResigned.SelectedDate);
                        }
                        emp.ResignedReason = txtResignReason.Text;
                        emp.IsResigned = Convert.ToBoolean(ChkResigned.IsChecked);
                        emp.IncomeTax = string.IsNullOrEmpty(txtIncomeTax.Text) ? 0 : Convert.ToDecimal(txtIncomeTax.Text);
                        emp.NoOfCL = string.IsNullOrEmpty(txtNoOfCL.Text) ? 0 : Convert.ToDecimal(txtNoOfCL.Text);
                        emp.NoOfML = string.IsNullOrEmpty(txtNoOfML.Text) ? 0 : Convert.ToDecimal(txtNoOfML.Text);
                        emp.IsUserLogin = Convert.ToBoolean(ChkUserLogin.IsChecked);
                        emp.IsAdmin = Convert.ToBoolean(ChkIsAdmin.IsChecked);
                        emp.UserName = txtUserName.Text;
                        emp.Password = txtPassword.Password;

                        if (Config.bIsNubeServer == true)
                        {
                            emp.NubeSubscription = Convert.ToBoolean(ChkNUBESUBSCRIPTION.IsChecked);
                            emp.NubeBranchId = string.IsNullOrEmpty(cmbNubeBranch.Text) ? 0 : Convert.ToInt32(cmbNubeBranch.SelectedValue);
                        }
                        db.SaveChanges();
                        MessageBox.Show("Updated Sucessfully");
                        this.Close();
                    }
                    else
                    {
                        MasterEmployee emp = new MasterEmployee();
                        emp.EmployeeName = txtEmployeeName.Text;
                        emp.MembershipNo = string.IsNullOrEmpty(txtMemberID.Text) ? 0 : Convert.ToInt32(txtMemberID.Text);
                        emp.NRIC = txtNRIC.Text;
                        emp.Gender = cmbGender.Text;
                        emp.PositionId = string.IsNullOrEmpty(cmbPosition.Text) ? 0 : Convert.ToInt32(cmbPosition.SelectedValue);
                        emp.Address1 = txtAddress1.Text;
                        emp.Address2 = txtAddress2.Text;
                        emp.Address3 = txtAddress3.Text;
                        emp.ZipCode = txtPostalCode.Text;
                        emp.CityId = string.IsNullOrEmpty(cmbCity.Text) ? 0 : Convert.ToInt32(cmbCity.SelectedValue);
                        emp.StateId = string.IsNullOrEmpty(cmbState.Text) ? 0 : Convert.ToInt32(cmbState.SelectedValue);
                        emp.CountryId = string.IsNullOrEmpty(cmbCountry.Text) ? 0 : Convert.ToInt32(cmbCountry.SelectedValue);
                        emp.TelephoneNo = txtTelephone.Text;
                        emp.MobileNo = txtMobile.Text;
                        emp.EmergencyNo = txtEmergencyContactNo.Text;
                        emp.BankId = string.IsNullOrEmpty(cmbBank.Text) ? 0 : Convert.ToInt32(cmbBank.SelectedValue);
                        emp.DateOfBirth = dtDOB.SelectedDate;
                        emp.RaceId = string.IsNullOrEmpty(cmbRace.Text) ? 0 : Convert.ToInt32(cmbRace.SelectedValue);
                        emp.DateOfJoining = dtDOJ.SelectedDate;
                        emp.Email = txtMail.Text;
                        emp.EPFNumber = txtEPFNo.Text;
                        emp.SOCSONumber = txtSOCSONo1.Text;
                        emp.EPFNumber = txtEPFNo1.Text;
                        emp.SOCSONumber = txtSOCSONo.Text;
                        emp.SIPNumber = txtSIPNumber.Text;
                        emp.ShiftId = Convert.ToInt32(cmbShift.SelectedValue);
                        emp.MobileAllowance = string.IsNullOrEmpty(txtMobileAllowance.Text) ? 0 : Convert.ToDecimal(txtMobileAllowance.Text);

                        emp.BankAccountNo = txtAccountNo.Text;
                        emp.BasicSalary = string.IsNullOrEmpty(txtBasicSalary.Text) ? 0 : Convert.ToDecimal(txtBasicSalary.Text);
                        emp.POB = string.IsNullOrEmpty(txtPOB.Text) ? 0 : Convert.ToDecimal(txtPOB.Text);
                        emp.NEC = string.IsNullOrEmpty(txtNEC.Text) ? 0 : Convert.ToDecimal(txtNEC.Text);
                        emp.SECONDMENT = string.IsNullOrEmpty(txtSECONDMENT.Text) ? 0 : Convert.ToDecimal(txtSECONDMENT.Text);
                        emp.SPECIAL = string.IsNullOrEmpty(txtSPECIAL.Text) ? 0 : Convert.ToDecimal(txtSPECIAL.Text);
                        emp.COLA = string.IsNullOrEmpty(txtCOLA.Text) ? 0 : Convert.ToDecimal(txtCOLA.Text);

                        emp.IsOverTimeEligible = Convert.ToBoolean(ChkOTEligible.IsChecked);
                        emp.EPFContributionRate = Convert.ToBoolean(ChkEPFCONTRIBUTIONRATE.IsChecked);
                        emp.EPFContribution = Convert.ToBoolean(ChkEPFCONTRIBUTION.IsChecked);
                        emp.SOCSOContribution = Convert.ToBoolean(ChkSOCSOCONTRIBUTION.IsChecked);
                        emp.EPFContribution = Convert.ToBoolean(ChkEPF1.IsChecked);
                        emp.SOCSOContribution = Convert.ToBoolean(ChkSOCSO1.IsChecked);
                        emp.SIP = Convert.ToBoolean(ChkSIP.IsChecked);
                        emp.KOPERASI = string.IsNullOrEmpty(txtKOPERASI.Text) ? 0 : Convert.ToDecimal(txtKOPERASI.Text);
                        emp.GMIS = string.IsNullOrEmpty(txtGMIS.Text) ? 0 : Convert.ToDecimal(txtGMIS.Text);
                        emp.BIMBLoan = string.IsNullOrEmpty(txtBIMBLOAN.Text) ? 0 : Convert.ToDecimal(txtBIMBLOAN.Text);
                        emp.GELA = string.IsNullOrEmpty(txtGELA.Text) ? 0 : Convert.ToDecimal(txtGELA.Text);
                        emp.HomeCarLoans = string.IsNullOrEmpty(txtHOMECARLOANS.Text) ? 0 : Convert.ToDecimal(txtHOMECARLOANS.Text);
                        emp.OtherLoans = string.IsNullOrEmpty(txtOtherLoans.Text) ? 0 : Convert.ToDecimal(txtOtherLoans.Text);
                        emp.OtherLoanTotal = string.IsNullOrEmpty(txtTotalOtherLoan.Text) ? 0 : Convert.ToDecimal(txtTotalOtherLoan.Text);

                        emp.PCB = Convert.ToBoolean(ChkPCB.IsChecked);
                        emp.UnpaidLeave = Convert.ToBoolean(ChkUNPAIDLEAVE.IsChecked);
                        emp.IsOtherEligible = Convert.ToBoolean(ChkOTHERS.IsChecked);
                        emp.Others = string.IsNullOrEmpty(txtOTHERS.Text) ? 0 : Convert.ToDecimal(txtOTHERS.Text);
                        emp.RatioOfContribution = Convert.ToBoolean(ChkRATIOOFCONTRIBUTION.IsChecked);
                        emp.EPF = Convert.ToBoolean(ChkEPF.IsChecked);
                        emp.SOCSO = Convert.ToBoolean(ChkSOCSO.IsChecked);
                        if (!string.IsNullOrEmpty(dtResigned.Text))
                        {
                            emp.ResignedDate = dtResigned.SelectedDate;
                        }
                        emp.ResignedReason = txtResignReason.Text;
                        emp.IsResigned = Convert.ToBoolean(ChkResigned.IsChecked);
                        emp.IncomeTax = string.IsNullOrEmpty(txtIncomeTax.Text) ? 0 : Convert.ToDecimal(txtIncomeTax.Text);
                        emp.NoOfCL = string.IsNullOrEmpty(txtNoOfCL.Text) ? 0 : Convert.ToDecimal(txtNoOfCL.Text);
                        emp.NoOfML = string.IsNullOrEmpty(txtNoOfML.Text) ? 0 : Convert.ToDecimal(txtNoOfML.Text);
                        emp.IsUserLogin = Convert.ToBoolean(ChkUserLogin.IsChecked);
                        emp.IsAdmin = Convert.ToBoolean(ChkIsAdmin.IsChecked);
                        emp.UserName = txtUserName.Text;
                        emp.Password = txtPassword.Password;

                        if (Config.bIsNubeServer == true)
                        {
                            emp.NubeBranchId = string.IsNullOrEmpty(cmbNubeBranch.Text) ? 0 : Convert.ToInt32(cmbNubeBranch.SelectedValue);
                            emp.NubeSubscription = Convert.ToBoolean(ChkNUBESUBSCRIPTION.IsChecked);
                        }
                        db.MasterEmployees.Add(emp);
                        db.SaveChanges();
                        MessageBox.Show("Saved Sucessfully");
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void ChkUserLogin_Click(object sender, RoutedEventArgs e)
        {
            if (ChkUserLogin.IsChecked == true)
            {
                grbUserName.Visibility = Visibility.Visible;
                ChkIsAdmin.Visibility = Visibility.Visible;
            }
            else
            {
                grbUserName.Visibility = Visibility.Collapsed;
                ChkIsAdmin.Visibility = Visibility.Collapsed;
            }
        }

        private void ChkResigned_Click(object sender, RoutedEventArgs e)
        {
            if (ChkResigned.IsChecked == true)
            {
                txtResignReason.IsEnabled = true;
                dtResigned.IsEnabled = true;
            }
            else
            {
                txtResignReason.IsEnabled = false;
                dtResigned.IsEnabled = false;
            }
        }

        private void ChkOTHERS_Click(object sender, RoutedEventArgs e)
        {
            if (ChkOTHERS.IsChecked == true)
            {
                txtOTHERS.IsEnabled = true;
            }
            else
            {
                txtOTHERS.IsEnabled = false;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChkUNPAIDLEAVE_Click(object sender, RoutedEventArgs e)
        {
            if (ChkUNPAIDLEAVE.IsChecked == false)
            {
                if (MessageBox.Show("Make Sure that,LOP Unchecked  ?", "Confirmation Message", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    ChkUNPAIDLEAVE.IsChecked = true;
                }
            }
        }

        private void ChkOTEligible_Click(object sender, RoutedEventArgs e)
        {
            if (ChkOTEligible.IsChecked == false)
            {
                if (MessageBox.Show("Make Sure that,OT Unchecked  ?", "Confirmation Message", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    ChkOTEligible.IsChecked = true;
                }
            }
        }

        private void txtMail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtMail_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            Config.CheckIsNumeric(e);
        }

        private void cmbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(cmbCity.SelectedValue) != 0)
                {
                    int iCityId = Convert.ToInt32(cmbCity.SelectedValue);

                    var ct = (from x in db.MasterCities where x.Id == iCityId select x).FirstOrDefault();
                    cmbState.SelectedValue = ct.StateId;
                    int iStateId = Convert.ToInt32(ct.StateId);
                    var st = (from x in db.MasterStates where x.Id == iStateId select x).FirstOrDefault();
                    cmbCountry.SelectedValue = st.CountryId;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dtDOB_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                DateTime now = DateTime.Today;
                TimeSpan ts = DateTime.Now - Convert.ToDateTime(dtDOB.SelectedDate);
                int age = Convert.ToInt32(ts.Days) / 365;
                txtAge.Text = age.ToString();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        #endregion

        #region Functions

        void FormFill()
        {
            try
            {
                var em = (from x in db.MasterEmployees where x.Id == Id select x).FirstOrDefault();

                if (em != null)
                {
                    txtEmployeeName.Text = em.EmployeeName.ToString();
                    txtMemberID.Text = em.MembershipNo.ToString();
                    txtNRIC.Text = em.NRIC;
                    cmbGender.Text = em.Gender;
                    cmbPosition.SelectedValue = em.PositionId;
                    txtAddress1.Text = em.Address1;
                    txtAddress2.Text = em.Address2;
                    txtAddress3.Text = em.Address3;
                    txtPostalCode.Text = em.ZipCode;
                    cmbCity.SelectedValue = em.CityId;
                    cmbState.SelectedValue = em.StateId;
                    cmbCountry.SelectedValue = em.CountryId;
                    txtTelephone.Text = em.TelephoneNo;
                    txtMobile.Text = em.MobileNo;
                    txtEmergencyContactNo.Text = em.EmergencyNo;
                    txtMail.Text = em.Email;
                    txtMobileAllowance.Text = em.MobileAllowance.ToString();
                    txtPaidAmount.Text = em.PaidAmount.ToString();
                    txtBalance.Text = em.BalanceAmount.ToString();
                    txtTotalOtherLoan.Text = em.OtherLoanTotal.ToString();

                    cmbBank.SelectedValue = em.BankId;
                    cmbShift.SelectedValue = em.ShiftId;
                    dtDOB.SelectedDate = em.DateOfBirth;
                    cmbRace.SelectedValue = em.RaceId;
                    dtDOJ.SelectedDate = em.DateOfJoining;
                    cmbNubeBranch.SelectedValue = em.NubeBranchId;
                    txtAccountNo.Text = em.BankAccountNo;
                    txtBasicSalary.Text = em.BasicSalary.ToString();
                    txtPOB.Text = em.POB.ToString();
                    txtNEC.Text = em.NEC.ToString();
                    txtSECONDMENT.Text = em.SECONDMENT.ToString();
                    txtSPECIAL.Text = em.SPECIAL.ToString();
                    txtCOLA.Text = em.COLA.ToString();
                    ChkOTEligible.IsChecked = em.IsOverTimeEligible;
                    ChkEPFCONTRIBUTIONRATE.IsChecked = em.EPFContributionRate;
                    ChkEPFCONTRIBUTION.IsChecked = em.EPFContribution;
                    ChkSOCSOCONTRIBUTION.IsChecked = em.SOCSOContribution;
                    ChkEPF1.IsChecked = em.EPFContribution;
                    ChkSOCSO1.IsChecked = em.SOCSOContribution;
                    ChkSIP.IsChecked = em.SIP;
                    txtKOPERASI.Text = em.KOPERASI.ToString();
                    ChkNUBESUBSCRIPTION.IsChecked = em.NubeSubscription;
                    txtGMIS.Text = em.GMIS.ToString();
                    txtBIMBLOAN.Text = em.BIMBLoan.ToString();
                    txtGELA.Text = em.GELA.ToString();
                    txtHOMECARLOANS.Text = em.HomeCarLoans.ToString();
                    txtOtherLoans.Text = em.OtherLoans.ToString();
                    ChkPCB.IsChecked = em.PCB;
                    ChkUNPAIDLEAVE.IsChecked = em.UnpaidLeave;
                    ChkOTHERS.IsChecked = em.IsOtherEligible;
                    txtOTHERS.Text = em.Others.ToString();
                    ChkRATIOOFCONTRIBUTION.IsChecked = em.RatioOfContribution;
                    ChkEPF.IsChecked = em.EPF;
                    ChkSOCSO.IsChecked = em.SOCSO;
                    dtResigned.Text = em.ResignedDate.ToString();
                    txtResignReason.Text = em.ResignedReason;
                    ChkResigned.IsChecked = em.IsResigned;
                    txtEPFNo.Text = em.EPFNumber;
                    txtSOCSONo.Text = em.SOCSONumber;
                    txtSIPNumber.Text = em.SIPNumber;
                    txtEPFNo1.Text = em.EPFNumber;
                    txtSOCSONo1.Text = em.SOCSONumber;
                    txtIncomeTax.Text = em.IncomeTax.ToString();

                    txtNoOfCL.Text = em.NoOfCL.ToString();
                    txtNoOfML.Text = em.NoOfML.ToString();
                    ChkUserLogin.IsChecked = em.IsUserLogin;
                    ChkIsAdmin.IsChecked = em.IsAdmin;
                    txtUserName.Text = em.UserName.ToString();
                    txtPassword.Password = em.Password.ToString();

                    if (em.IsUserLogin == true)
                    {
                        grbUserName.Visibility = Visibility.Visible;
                        ChkIsAdmin.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        grbUserName.Visibility = Visibility.Collapsed;
                        ChkIsAdmin.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    MessageBox.Show("No Records Found");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        void WindowsLoaded()
        {
            try
            {
                if (Config.bIsNubeServer == true)
                {
                    ChkNUBESUBSCRIPTION.Visibility = Visibility.Visible;
                    cmbNubeBranch.Visibility = Visibility.Visible;
                    grbEPFSOCSODEDUCTIONS.Visibility = Visibility.Visible;
                    grbEMPLOYERSCONTRIBUTION.Visibility = Visibility.Visible;
                    grbTotalLoans.Visibility = Visibility.Collapsed;
                    grbEPFSOCSO.Visibility = Visibility.Collapsed;
                    grbDeduction1.Header = "STANDING INSTRUCTION DEDUCTIONS";
                    var nb = (from x in db.MasterNubeBranches where x.IsCancel == false select x).ToList();
                    cmbNubeBranch.ItemsSource = nb;
                    cmbNubeBranch.SelectedValuePath = "Id";
                    cmbNubeBranch.DisplayMemberPath = "NubeBranchName";
                    txtEPFNo.Visibility = Visibility.Visible;
                    txtSOCSONo.Visibility = Visibility.Visible;
                    txtOtherAllowance.Text = "COLA";

                }
                else
                {
                    ChkNUBESUBSCRIPTION.Visibility = Visibility.Collapsed;
                    cmbNubeBranch.Visibility = Visibility.Collapsed;
                    txtOtherAllowance.Text = "Other Allowance";
                }

                ChkResigned.IsChecked = false;
                ChkOTHERS.IsChecked = false;
                ChkUserLogin.IsChecked = false;
                ChkOTEligible.IsChecked = true;
                ChkUNPAIDLEAVE.IsChecked = true;

                if (ChkUserLogin.IsChecked == true)
                {
                    grbUserName.Visibility = Visibility.Visible;
                    ChkIsAdmin.Visibility = Visibility.Visible;
                }
                else
                {
                    grbUserName.Visibility = Visibility.Collapsed;
                    ChkIsAdmin.Visibility = Visibility.Collapsed;
                }

                if (ChkOTHERS.IsChecked == true)
                {
                    txtOTHERS.IsEnabled = true;
                }
                else
                {
                    txtOTHERS.IsEnabled = false;
                }

                if (ChkResigned.IsChecked == true)
                {
                    txtResignReason.IsEnabled = true;
                    dtResigned.IsEnabled = true;
                }
                else
                {
                    txtResignReason.IsEnabled = false;
                    dtResigned.IsEnabled = false;
                }

                var cy = (from x in db.MasterCountries where x.IsCancel == false select x).ToList();
                cmbCountry.ItemsSource = cy;
                cmbCountry.SelectedValuePath = "Id";
                cmbCountry.DisplayMemberPath = "CountryName";

                var cu = (from x in db.MasterStates where x.IsCancel == false select x).ToList();
                cmbState.ItemsSource = cu;
                cmbState.SelectedValuePath = "Id";
                cmbState.DisplayMemberPath = "StateName";

                var st = (from x in db.MasterCities where x.IsCancel == false select x).ToList();
                cmbCity.ItemsSource = st;
                cmbCity.SelectedValuePath = "Id";
                cmbCity.DisplayMemberPath = "CityName";

                var mb = (from x in db.MasterBanks where x.IsCancel == false select x).ToList();
                cmbBank.ItemsSource = mb;
                cmbBank.SelectedValuePath = "Id";
                cmbBank.DisplayMemberPath = "UserCode";

                var sf = (from x in db.EmployeeShifts where x.IsCancel == false select x).ToList();
                cmbShift.ItemsSource = sf;
                cmbShift.SelectedValuePath = "Id";
                cmbShift.DisplayMemberPath = "Name";

                var mp = (from x in db.MasterPositions where x.IsCancel == false select x).ToList();
                cmbPosition.ItemsSource = mp;
                cmbPosition.SelectedValuePath = "Id";
                cmbPosition.DisplayMemberPath = "PositionName";

                var mr = (from x in db.MasterRaces where x.IsCancel == false select x).ToList();
                cmbRace.ItemsSource = mr;
                cmbRace.SelectedValuePath = "Id";
                cmbRace.DisplayMemberPath = "RaceName";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        void ClearAll()
        {
            txtEmployeeName.Text = "";
            txtMemberID.Text = "";
            txtNRIC.Text = "";
            cmbGender.Text = "";
            cmbPosition.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtPostalCode.Text = "";
            cmbCity.Text = "";
            cmbState.Text = "";
            cmbCountry.Text = "";
            txtTelephone.Text = "";
            txtMobile.Text = "";
            txtEmergencyContactNo.Text = "";
            txtMobileAllowance.Text = "";

            cmbBank.Text = "";
            dtDOB.Text = "";
            txtAge.Text = "";
            cmbRace.Text = "";
            dtDOJ.Text = "";
            cmbShift.Text = "";
            cmbNubeBranch.Text = "";
            txtAccountNo.Text = "";
            txtBasicSalary.Text = "";
            txtPOB.Text = "";
            txtNEC.Text = "";
            txtSECONDMENT.Text = "";
            txtSPECIAL.Text = "";
            txtCOLA.Text = "";
            ChkOTEligible.IsChecked = true;
            ChkEPFCONTRIBUTIONRATE.IsChecked = false;
            ChkEPFCONTRIBUTION.IsChecked = false;
            ChkSOCSOCONTRIBUTION.IsChecked = false;
            txtKOPERASI.Text = "";
            ChkNUBESUBSCRIPTION.IsChecked = false;
            txtGMIS.Text = "";
            txtBIMBLOAN.Text = "";
            txtGELA.Text = "";
            txtHOMECARLOANS.Text = "";
            txtOtherLoans.Text = "";
            ChkPCB.IsChecked = false;
            ChkUNPAIDLEAVE.IsChecked = true;
            ChkOTHERS.IsChecked = false;
            txtOTHERS.Text = "";
            ChkRATIOOFCONTRIBUTION.IsChecked = false;
            ChkEPF.IsChecked = false;
            ChkSOCSO.IsChecked = false;
            txtTotalOtherLoan.Text = "";
            txtPaidAmount.Text = "";
            txtBalance.Text = "";

            ChkResigned.IsChecked = false;
            dtResigned.Text = "";
            txtResignReason.Text = "";
            txtEPFNo.Text = "";
            txtSOCSONo.Text = "";
            txtEPFNo1.Text = "";
            txtSOCSONo1.Text = "";
            txtMail.Text = "";
            txtIncomeTax.Text = "";

            txtNoOfCL.Text = "";
            txtNoOfML.Text = "";
            ChkUserLogin.IsChecked = false;
            ChkIsAdmin.IsChecked = false;
            txtUserName.Text = "";
            txtPassword.Password = "";
        }

        void FormValidation()
        {
            if (string.IsNullOrEmpty(txtEmployeeName.Text))
            {
                MessageBox.Show("EmployeeName is Empty!");
                txtEmployeeName.Focus();
                bValidate = false;
            }
            else if (string.IsNullOrEmpty(txtMemberID.Text))
            {
                MessageBox.Show("Employee No is Empty!");
                txtMemberID.Focus();
                bValidate = false;
            }
            else if (string.IsNullOrEmpty(cmbGender.Text))
            {
                MessageBox.Show("Gender is Empty!");
                cmbGender.Focus();
                bValidate = false;
            }
            else if (string.IsNullOrEmpty(dtDOB.Text))
            {
                MessageBox.Show("DOB is Empty!");
                dtDOB.Focus();
                bValidate = false;
            }
            else if (string.IsNullOrEmpty(dtDOJ.Text))
            {
                MessageBox.Show("Date Of Joining is Empty!");
                dtDOJ.Focus();
                bValidate = false;
            }
            else if (string.IsNullOrEmpty(cmbShift.Text))
            {
                MessageBox.Show("Shift is Empty!");
                cmbShift.Focus();
                bValidate = false;
            }
            else if (string.IsNullOrEmpty(cmbPosition.Text))
            {
                MessageBox.Show("Position is Empty!");
                cmbPosition.Focus();
                bValidate = false;
            }
            else if (string.IsNullOrEmpty(cmbBank.Text))
            {
                MessageBox.Show("Bank is Empty!");
                cmbBank.Focus();
                bValidate = false;
            }
            else if (string.IsNullOrEmpty(cmbRace.Text))
            {
                MessageBox.Show("Race is Empty!");
                cmbRace.Focus();
                bValidate = false;
            }
            else if (string.IsNullOrEmpty(cmbNubeBranch.Text) && (Config.bIsNubeServer == true))
            {
                MessageBox.Show("NUBE Branch is Empty!");
                cmbNubeBranch.Focus();
                bValidate = false;
            }
            else if (string.IsNullOrEmpty(cmbCity.Text))
            {
                MessageBox.Show("City is Empty!");
                cmbCity.Focus();
                bValidate = false;
            }
            else if (string.IsNullOrEmpty(cmbState.Text))
            {
                MessageBox.Show("State is Empty!");
                cmbState.Focus();
                bValidate = false;
            }
            else if (string.IsNullOrEmpty(cmbCountry.Text))
            {
                MessageBox.Show("Country is Empty!");
                cmbCountry.Focus();
                bValidate = false;
            }
            else if (!string.IsNullOrEmpty(txtTotalOtherLoan.Text) && string.IsNullOrEmpty(txtOtherLoans.Text))
            {
                MessageBox.Show("Monthly Deduct Loan Amount is Empty!","Other Loan Deduction Error");
                txtOtherLoans.Focus();
                bValidate = false;
            }
        }

        #endregion

        private void txtnOtherloans_TextChanged(object sender, TextChangedEventArgs e)
        {

        }      

        private void txtAddress1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtEPFNo1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


    }
}
