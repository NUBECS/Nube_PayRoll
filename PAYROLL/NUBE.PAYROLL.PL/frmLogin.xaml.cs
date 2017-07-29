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
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //string RValue = BLL.UserAccount.Login(txtUserId.Text, txtPassword.Password);
            string RValue = "";
            if (RValue == "")
            {
                if (txtUserId.Text != "Admin")
                {
                    MessageBox.Show("Invalid User");
                    txtUserId.Focus();
                }
                else if (txtPassword.Password != "Admin")
                {
                    MessageBox.Show("Invalid Password");
                    txtPassword.Focus();
                }
                else
                {
                    frmHome frm = new frmHome();
                    frm.ShowDialog();                   
                }              
                //App.frmHome= new frmHome();
                //this.Hide();
                //App.frmHome.ShowDialog();
                //this.Show();
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

        #region PreviewExecuted

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
