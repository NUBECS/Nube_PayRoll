﻿using System;
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

namespace NUBE.PAYROLL.PL
{
    /// <summary>
    /// Interaction logic for frmCompanySetup.xaml
    /// </summary>
    public partial class frmCompanySetup : UserControl
    {
        PayrollEntity db = new PayrollEntity();
        public frmCompanySetup()
        {
            InitializeComponent();
        }

        #region EVENTS

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWindow();
        }

        private void cmbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {

        }

        private void ChkGraceTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCompanyName.Text))
                {
                    MessageBox.Show("Company Name is Empty!");
                    txtCompanyName.Focus();
                }
                else if (string.IsNullOrEmpty(txtTUMPDB.Text))
                {
                    MessageBox.Show("THUMP Database Name is Empty!");
                    txtTUMPDB.Focus();
                }
                else
                {
                    var cmp = (from x in db.CompanyDetails where x.Id == 1 select x).FirstOrDefault();
                    if (cmp != null)
                    {
                        cmp.CompanyName = txtCompanyName.Text;
                        cmp.PrintName = txtPrintName.Text;
                        cmp.AddressLine1 = txtAddress1.Text;
                        cmp.AddressLine2 = txtAddress2.Text;
                        cmp.AddressLine3 = txtAddress3.Text;

                        cmp.CityCode = Convert.ToInt32(cmbCity.SelectedValue);
                        cmp.StateCode = Convert.ToInt32(cmbState.SelectedValue);
                        cmp.CountryCode = Convert.ToInt32(cmbCountry.SelectedValue);
                        cmp.PostalCode = txtPostalCode.Text;
                        cmp.TelephoneNo = txtTelephone.Text;
                        cmp.MobileNo = txtMobile.Text;
                        cmp.DbName = txtTUMPDB.Text;
                        cmp.RobNo = txtRobNo.Text;
                        db.SaveChanges();
                        MessageBox.Show("Updated Sucessfully!", "PAYROLL");
                        App.frmHome.ShowWelcome();
                    }
                    else
                    {
                        CompanyDetail cd = new CompanyDetail();
                        cd.CompanyName = txtCompanyName.Text;
                        cd.PrintName = txtPrintName.Text;
                        cd.AddressLine1 = txtAddress1.Text;
                        cd.AddressLine2 = txtAddress2.Text;
                        cd.AddressLine3 = txtAddress3.Text;

                        cd.CityCode = Convert.ToInt32(cmbCity.SelectedValue);
                        cd.StateCode = Convert.ToInt32(cmbState.SelectedValue);
                        cd.CountryCode = Convert.ToInt32(cmbCountry.SelectedValue);
                        cd.PostalCode = txtPostalCode.Text;
                        cd.TelephoneNo = txtTelephone.Text;
                        cd.MobileNo = txtMobile.Text;
                        cd.DbName = txtTUMPDB.Text;
                        cd.RobNo = txtRobNo.Text;
                        db.CompanyDetails.Add(cd);
                        db.SaveChanges();
                        MessageBox.Show("Saved Sucessfully!", "PAYROLL");
                        App.frmHome.ShowWelcome();
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
            if (MessageBox.Show("Are you Sure Clear This Data !?", "Clear Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                FormClear();
            }
        }

        #endregion

        #region Functions       

        void FormClear()
        {
            txtCompanyName.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            cmbCity.Text = "";
            cmbState.Text = "";
            cmbCountry.Text = "";
            txtPostalCode.Text = "";
            txtTelephone.Text = "";
            txtMobile.Text = "";
            txtTUMPDB.Text = "";
            txtPrintName.Text = "";
            txtRobNo.Text = "";
        }

        void LoadWindow()
        {
            try
            {
                var cy = (from x in db.MasterCountries select x).ToList();
                cmbCountry.ItemsSource = cy;
                cmbCountry.SelectedValuePath = "Id";
                cmbCountry.DisplayMemberPath = "CountryName";

                var cu = (from x in db.MasterStates select x).ToList();
                cmbState.ItemsSource = cu;
                cmbState.SelectedValuePath = "Id";
                cmbState.DisplayMemberPath = "StateName";

                var st = (from x in db.MasterCities select x).ToList();
                cmbCity.ItemsSource = st;
                cmbCity.SelectedValuePath = "Id";
                cmbCity.DisplayMemberPath = "CityName";

                FormClear();

                var cmp = (from x in db.CompanyDetails where x.Id == 1 select x).FirstOrDefault();
                if (cmp != null)
                {
                    txtCompanyName.Text = cmp.CompanyName;
                    txtAddress1.Text = cmp.AddressLine1;
                    txtAddress2.Text = cmp.AddressLine2;
                    txtAddress3.Text = cmp.AddressLine3;
                    cmbCity.SelectedValue = cmp.CityCode;
                    cmbState.SelectedValue = cmp.StateCode;
                    cmbCountry.SelectedValue = cmp.CountryCode;
                    txtPostalCode.Text = cmp.PostalCode;
                    txtTelephone.Text = cmp.TelephoneNo;
                    txtMobile.Text = cmp.MobileNo;
                    txtTUMPDB.Text = cmp.DbName;
                    txtPrintName.Text = cmp.PrintName;
                    txtRobNo.Text = cmp.RobNo;
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
