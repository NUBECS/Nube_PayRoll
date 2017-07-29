using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NUBE.PAYROLL.PL
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static frmLogin frmHome;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Window frm = new frmLogin();
            frm.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }
    }
}
