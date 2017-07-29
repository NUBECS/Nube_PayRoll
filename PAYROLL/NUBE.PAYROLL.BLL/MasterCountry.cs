using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUBE.PAYROLL.BLL
{
    public class MasterCountry : INotifyPropertyChanged
    {
        #region Fields

        private int _Id;
        private string _CountryName;
        private string _ShortName;

        #endregion

        #region Property

        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    NotifyPropertyChanged(nameof(Id));
                }
            }
        }

        public string CountryName
        {
            get
            {
                return _CountryName;
            }
            set
            {
                if (_CountryName != value)
                {
                    _CountryName = value;
                    NotifyPropertyChanged(nameof(CountryName));
                }
            }
        }

        public string ShortName
        {
            get
            {
                return _ShortName;
            }
            set
            {
                if (_ShortName != value)
                {
                    _ShortName = value;
                    NotifyPropertyChanged(nameof(CountryName));
                }
            }
        }

        #endregion

        #region Property  Changed Event

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }       

        #endregion
    }
}
