
using System;
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
        public static bool bIsNubeServer = false;

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

        public static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9.]");
            return reg.IsMatch(str);

        }

        public static string NumericOnly(string str)
        {
            String newText = String.Empty;

            int DotCount = 0;
            foreach (Char c in str.ToCharArray())
            {
                if (Char.IsDigit(c) || Char.IsControl(c) || (c == '.' && DotCount == 0))
                {
                    newText += c;
                    if (c == '.') DotCount += 1;
                }
            }
            return newText;
        }

        public static bool IsValidEmailAddress(this string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

    }
}
