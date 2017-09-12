using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace NUBE.PAYROLL.PL
{
    public static class Config
    {
        public static string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["PAYROLL"].ConnectionString;
        public static string EsslDatasource = "";
        public static string EsslDB = "";
        public static string EsslUserId = "";
        public static string EsslPassword = "";
        public static string EsslServer = @"Data Source=" + EsslDatasource + ";Initial Catalog=" + EsslDB + ";user id=" + EsslUserId + ";password=" + EsslPassword + ";";
        public static Boolean bIsNubeServer = false;

        public static void CheckIsNumeric(TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9.9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                NUBE.PAYROLL.CMN.ExceptionLogging.SendErrorToText(ex);
            }
        }

    }
}
