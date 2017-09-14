using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NUBE.PAYROLL.CMN
{
    public static class AppLib
    {
        public static string connStr = "Data Source =.\\sqlexpress;Initial Catalog = payroll; Integrated Security = True";
        public enum Forms
        {
            frmCompanySetting,
            frmUser,
            frmUserType,
            frmBank,
            frmMasterBankBranch,
            frmMasterCity,
            frmEmployee,
            frmMasterNubeBranch,
            frmPosition
        }
        public static T toCopy<T>(this object objSource, T objDestination)
        {
            try
            {
                var l1 = objSource.GetType().GetProperties().Where(x => x.PropertyType.Namespace != "System.Collections.Generic").ToList();

                foreach (var pFrom in l1)
                {
                    try
                    {
                        var pTo = objDestination.GetType().GetProperties().Where(x => x.Name == pFrom.Name).FirstOrDefault();
                        pTo.SetValue(objDestination, pFrom.GetValue(objSource));
                    }
                    catch (Exception ex) { }

                }
            }
            catch (Exception ex)
            {

            }
            return objDestination;
        }

        public static void MutateVerbose<TField>(this INotifyPropertyChanged instance, ref TField field, TField newValue, Action<PropertyChangedEventArgs> raise, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TField>.Default.Equals(field, newValue)) return;
            field = newValue;
            raise?.Invoke(new PropertyChangedEventArgs(propertyName));
        }

        public static DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();

            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        #region Numer's & Email Validation 

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

        #endregion
    }
}
