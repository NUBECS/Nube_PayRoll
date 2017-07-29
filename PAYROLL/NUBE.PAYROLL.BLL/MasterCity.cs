using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUBE.PAYROLL.CMN;

namespace NUBE.PAYROLL.BLL
{
    public class MasterCity : INotifyPropertyChanged
    {
        #region Fields

        public static MasterCity City = new MasterCity();

        private static UserTypeDetail _UserPermission;
        private bool _IsReadOnly;
        private bool _IsEnabled;

        private static ObservableCollection<MasterCity> _toList;
        public List<BLL.Validation> lstValidation = new List<BLL.Validation>();

        private int _Id;
        private string _CityName;
        private string _ShortName;
        private int _StateId;

        #endregion


        #region Property

        public static UserTypeDetail UserPermission
        {
            get
            {
                if (_UserPermission == null)
                {
                    _UserPermission = UserAccount.User.UserType == null ? new UserTypeDetail() : UserAccount.User.UserType.UserTypeDetails.Where(x => x.UserTypeFormDetail.FormName == AppLib.Forms.frmUser.ToString()).FirstOrDefault();
                }
                return _UserPermission;
            }

            set
            {
                if (_UserPermission != value)
                {
                    _UserPermission = value;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _IsReadOnly;
            }

            set
            {
                if (_IsReadOnly != value)
                {
                    _IsReadOnly = value;
                    IsEnabled = !value;
                    NotifyPropertyChanged(nameof(IsReadOnly));
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }

            set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    NotifyPropertyChanged(nameof(IsEnabled));
                }
            }
        }

        public static ObservableCollection<MasterCity> toList
        {
            get
            {
                if (_toList == null)
                {
                    _toList = new ObservableCollection<MasterCity>(PYClientHub.PYHub.Invoke<List<MasterCity>>("MasterCity_List").Result);
                }
                return _toList;
            }
        }

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

        public string CityName
        {
            get
            {
                return _CityName;
            }
            set
            {
                if (_CityName != value)
                {
                    _CityName = value;
                    NotifyPropertyChanged(nameof(CityName));
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
                    NotifyPropertyChanged(nameof(ShortName));
                }
            }
        }

        public int StateId
        {
            get
            {
                return _StateId;
            }
            set
            {
                if (_StateId != value)
                {
                    _StateId = value;
                    NotifyPropertyChanged(nameof(StateId));
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

        private void NotifyAllPropertyChanged()
        {
            foreach (var p in this.GetType().GetProperties()) NotifyPropertyChanged(p.Name);
        }

        #endregion

        #region Validation

        public bool isValid()
        {
            bool RValue = true;
            lstValidation.Clear();
            if (string.IsNullOrWhiteSpace(CityName))
            {
                lstValidation.Add(new Validation() { Name = nameof(CityName), Message = string.Format(MSG.BLL.Required_Data, nameof(CityName)) });
            }
            else if (toList.Where(x => x.CityName.ToLower() == CityName.ToLower() && x.Id != Id).Count() > 0)
            {
                lstValidation.Add(new Validation() { Name = nameof(CityName), Message = string.Format(MSG.BLL.Existing_Data, CityName) });
                RValue = false;
            }
            return RValue;
        }

        #endregion

        #region Method

        public bool Save(bool isServerCall = false)
        {
            if (!isValid()) return false;
            try
            {
                MasterCity d = toList.Where(x => x.Id == Id).FirstOrDefault();

                if (d == null)
                {
                    d = new MasterCity();
                    toList.Add(d);
                }

                this.toCopy<MasterCity>(d);
                if (isServerCall == false)
                {
                    var i = PYClientHub.PYHub.Invoke<int>("MasterCity_Save", this).Result;
                    d.Id = i;
                }

                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return false;
            }
        }

        public bool Delete(bool isServerCall = false)
        {
            var d = toList.Where(x => x.Id == Id).FirstOrDefault();
            if (d != null)
            {
                toList.Remove(d);
                if (isServerCall == false) PYClientHub.PYHub.Invoke<int>("MasterCity_Delete", this.Id);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            this.Id = 0;
            this.CityName = "";
            this.ShortName = "";
            this.StateId = 0;
            IsReadOnly = !UserPermission.AllowInsert;
            NotifyAllPropertyChanged();
        }

        public bool Find(int pk)
        {
            var d = toList.Where(x => x.Id == pk).FirstOrDefault();
            if (d != null)
            {
                d.toCopy<MasterCity>(this);
                IsReadOnly = !UserPermission.AllowUpdate;
                return true;
            }

            return false;
        }

        #endregion
    }
}
