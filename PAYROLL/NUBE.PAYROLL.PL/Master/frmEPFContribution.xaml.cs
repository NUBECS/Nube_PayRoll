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
using System.Data;

namespace NUBE.PAYROLL.PL.Master
{
    /// <summary>
    /// Interaction logic for frmEPFContribution.xaml
    /// </summary>
    public partial class frmEPFContribution : UserControl
    {
        int Id = 0;
        PayrollEntity db = new PayrollEntity();
        DataTable dtEPF = new DataTable();
        public frmEPFContribution()
        {
            InitializeComponent();
        }

        #region EVENTS

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWindow();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMinRM.Text))
                {
                    MessageBox.Show("Min RM is Empty!", "Empty");
                    txtMinRM.Focus();
                }
                else if (string.IsNullOrEmpty(txtSalryUpto.Text))
                {
                    MessageBox.Show("SalryUpto is Empty!", "Empty");
                    txtSalryUpto.Focus();
                }
                else if (string.IsNullOrEmpty(txtMajikan.Text))
                {
                    MessageBox.Show("Majikan is Empty!", "Empty");
                    txtMajikan.Focus();
                }
                else if (string.IsNullOrEmpty(txtPakerja.Text))
                {
                    MessageBox.Show("Pakerja is Empty!", "Empty");
                    txtPakerja.Focus();
                }
                else if (string.IsNullOrEmpty(txtJumlahCaruman.Text))
                {
                    MessageBox.Show("Jumlah Caruman is Empty!", "Empty");
                    txtJumlahCaruman.Focus();
                }
                else
                {
                    if (Id != 0)
                    {
                        var mb = (from x in db.EPFConts where x.Id == Id select x).FirstOrDefault();
                        if (mb != null)
                        {
                            mb.MinRM = Convert.ToDecimal(txtMinRM.Text);
                            mb.MaxRM = Convert.ToDecimal(txtSalryUpto.Text);
                            mb.Majikan = Convert.ToDecimal(txtMajikan.Text);
                            mb.Pakerja = Convert.ToDecimal(txtPakerja.Text);
                            mb.JumlahCaruman = Convert.ToDecimal(txtJumlahCaruman.Text);
                            db.SaveChanges();
                            MessageBox.Show("Updated Sucessfully!");
                            LoadWindow();
                        }
                    }
                    else
                    {
                        EPFCont mb = new EPFCont();
                        mb.MinRM = Convert.ToDecimal(txtMinRM.Text);
                        mb.MaxRM = Convert.ToDecimal(txtSalryUpto.Text);
                        mb.Majikan = Convert.ToDecimal(txtMajikan.Text);
                        mb.Pakerja = Convert.ToDecimal(txtPakerja.Text);
                        mb.JumlahCaruman = Convert.ToDecimal(txtJumlahCaruman.Text);
                        db.EPFConts.Add(mb);
                        db.SaveChanges();
                        MessageBox.Show("Saved Sucessfully!");
                        LoadWindow();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Id != 0)
                {
                    if (MessageBox.Show("Do you want to Delete Shift ?", "Delete Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var mb = (from x in db.EPFConts where x.Id == Id select x).FirstOrDefault();
                        mb.IsCancel = true;
                        mb.CancelOn = DateTime.Now;
                        //db.EPFConts.Remove(mb);
                        db.SaveChanges();
                        MessageBox.Show("Deleted Sucessfully");
                        LoadWindow();
                    }
                }
            }
            catch (Exception ex)
            {
                // ExceptionLogging.SendErrorToText(ex);
                MessageBox.Show(ex.Message, "You Can't Delete");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            LoadWindow();
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSearch_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            Filteration();
        }

        private void txtMajikan_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMajikan.Text) && !string.IsNullOrEmpty(txtPakerja.Text))
            {
                txtJumlahCaruman.Text = (Convert.ToDecimal(txtMajikan.Text) + Convert.ToDecimal(txtPakerja.Text)).ToString();
            }
        }

        private void txtPakerja_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMajikan.Text) && !string.IsNullOrEmpty(txtPakerja.Text))
            {
                txtJumlahCaruman.Text = (Convert.ToDecimal(txtMajikan.Text) + Convert.ToDecimal(txtPakerja.Text)).ToString();
            }
        }

        private void dgEPF_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Id = 0;
                txtMinRM.Text = "";
                txtSalryUpto.Text = "";
                txtMajikan.Text = "";
                txtPakerja.Text = "";
                txtJumlahCaruman.Text = "";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void dgEPF_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((dgEPF.SelectedItem != null))
                {
                    DataRowView drv = (DataRowView)dgEPF.SelectedItem;
                    Id = Convert.ToInt32(drv["Id"]);
                    txtMinRM.Text = drv["MinRM"].ToString();
                    txtSalryUpto.Text = drv["MaxRM"].ToString();
                    txtMajikan.Text = drv["Majikan"].ToString();
                    txtPakerja.Text = drv["Pakerja"].ToString();
                    txtJumlahCaruman.Text = drv["JumlahCaruman"].ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            Config.CheckIsNumeric(e);
        }

        #endregion

        #region FUNCITONS

        void LoadWindow()
        {
            Id = 0;
            txtMinRM.Text = "";
            txtSalryUpto.Text = "";
            txtMajikan.Text = "";
            txtPakerja.Text = "";
            txtJumlahCaruman.Text = "";

            try
            {
                var EPF = (from x in db.EPFConts where x.IsCancel == false orderby x.MinRM ascending select x).ToList();
                if (EPF != null)
                {
                    dtEPF = AppLib.LINQResultToDataTable(EPF);
                    dgEPF.ItemsSource = dtEPF.DefaultView;
                    Filteration();
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
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    string sWhere = "";
                    sWhere = "(MinRM=" + Convert.ToDecimal(txtSearch.Text) + ") OR (MaxRM=" + Convert.ToDecimal(txtSearch.Text) + ")";

                    if (!string.IsNullOrEmpty(txtSearch.Text))
                    {
                        DataView dv = new DataView(dtEPF);
                        dv.RowFilter = sWhere;
                        DataTable dtTemp = new DataTable();
                        dtTemp = dv.ToTable();
                        dgEPF.ItemsSource = dtTemp.DefaultView;
                    }
                    else
                    {
                        dgEPF.ItemsSource = dtEPF.DefaultView;
                    }
                }
                else
                {
                    dgEPF.ItemsSource = dtEPF.DefaultView;
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
