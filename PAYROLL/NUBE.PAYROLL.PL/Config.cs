using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
