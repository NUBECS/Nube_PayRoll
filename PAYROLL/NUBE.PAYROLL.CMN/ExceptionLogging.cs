using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUBE.PAYROLL.CMN
{
    public class ExceptionLogging
    {
        private static String ErrorlineNo, Errormsg, extype, exurl, ErrorLocation;

        public static void SendErrorToText(Exception ex)
        {
            var line = Environment.NewLine + Environment.NewLine;

            ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            Errormsg = ex.GetType().Name.ToString();
            extype = ex.GetType().ToString();
            exurl = ex.StackTrace.ToString();
            ErrorLocation = ex.Message.ToString();
            try
            {
                //string filepath = Path.Combine(Environment.CurrentDirectory, @"Exception Files\" + DateTime.Now.Date.ToString("dd-MMM-yyyy") + "\\");
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }


    }
}
