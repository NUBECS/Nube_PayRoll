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
using Microsoft.AspNet.SignalR.Client;

namespace NUBE.PAYROLL.PL
{
    /// <summary>
    /// Interaction logic for frmLogin.xaml
    /// </summary>
    public partial class frmLogin : MetroWindow
    {
        PayrollEntity db = new PayrollEntity();

        public frmLogin()
        {
            InitializeComponent();
        }

        #region EVENTS

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtUserId.Text))
                {
                    MessageBox.Show("User Name is Empty");
                    txtUserId.Focus();
                }
                else if (String.IsNullOrEmpty(txtPassword.Password))
                {
                    MessageBox.Show("Password is Empty");
                    txtPassword.Focus();
                }
                else
                {
                    var usr = (from x in db.UserAccounts where x.LoginId == txtUserId.Text && x.Password == txtPassword.Password.ToString() select x).FirstOrDefault();
                    if (usr != null)
                    {
                        var cmp = (from x in db.CompanyDetails select x).FirstOrDefault();
                        if (cmp != null)
                        {
                            Config.EsslDatasource = cmp.ServerName;
                            Config.EsslDB = cmp.DbName;
                            Config.EsslUserId = cmp.UserId;
                            Config.EsslPassword = cmp.Password;
                            Config.bIsNubeServer = cmp.IsNUBE;
                        }
                        else
                        {
                            Config.bIsNubeServer = false;
                        }
                        txtUserId.Text = "";
                        txtPassword.Password = "";
                        frmHome frm = new frmHome();
                        frm.ShowDialog();
                    }
                    else
                    {
                        txtUserId.Text = "";
                        txtPassword.Password = "";
                        txtUserId.Focus();
                        MessageBox.Show("Invalid User Name or Password ", "Error");
                    }
                }
            }            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    btnLogin.Focus();
                    if (String.IsNullOrEmpty(txtUserId.Text))
                    {
                        MessageBox.Show("User Name is Empty");
                        txtUserId.Focus();
                    }
                    else if (String.IsNullOrEmpty(txtPassword.Password))
                    {
                        MessageBox.Show("Password is Empty");
                        txtPassword.Focus();
                    }
                    else
                    {
                        var usr = (from x in db.UserAccounts where x.LoginId == txtUserId.Text && x.Password == txtPassword.Password.ToString() select x).FirstOrDefault();
                        if (usr != null)
                        {
                            var cmp = (from x in db.CompanyDetails select x).FirstOrDefault();
                            if (cmp != null)
                            {
                                Config.EsslDatasource = cmp.ServerName;
                                Config.EsslDB = cmp.DbName;
                                Config.EsslUserId = cmp.UserId;
                                Config.EsslPassword = cmp.Password;
                                Config.bIsNubeServer = cmp.IsNUBE;
                            }
                            else
                            {
                                Config.bIsNubeServer = false;
                            }
                            txtUserId.Text = "";
                            txtPassword.Password = "";
                            frmHome frm = new frmHome();
                            frm.ShowDialog();
                        }
                        else
                        {
                            txtUserId.Text = "";
                            txtPassword.Password = "";
                            txtUserId.Focus();
                            MessageBox.Show("Invalid User Name or Password ", "Error");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure to Exit?", "Exit", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtUserId.Text = "";
            txtPassword.Password = "";
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #region PREVIEWEXECUTED

        private void txtPassword_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void txtUserId_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        public static implicit operator frmLogin(frmHome v)
        {
            throw new NotImplementedException();
        }

        #endregion      
    }
}
