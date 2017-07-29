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
    public class UserType
    {
        #region Field

        private static UserTypeDetail _UserPermission;
        private bool _IsReadOnly;
        private bool _IsEnabled;

        private static ObservableCollection<UserType> _toList;
        public List<BLL.Validation> lstValidation = new List<BLL.Validation>();

        private int _Id;
        private string _Name;
        private string _Description;
        private int _CompanyId;
        private CompanyDetails _Company;
        private ObservableCollection<UserTypeDetail> _UserTypeDetails;

        #endregion

        #region Property

        public static UserTypeDetail UserPermission
        {
            get
            {
                if (_UserPermission == null)
                {
                    // _UserPermission = UserAccount.User.UserTypeId  == null ? new UserTypeDetail() : UserAccount.User.UserTypeId.UserTypeDetai.Where(x => x.UserTypeFormDetail.FormName == AppLib.Forms.frmUser.ToString()).FirstOrDefault();
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
        public static ObservableCollection<UserType> toList
        {
            get
            {
                if (_toList == null)
                {
                    _toList = new ObservableCollection<UserType>(PYClientHub.PYHub.Invoke<List<UserType>>("UserType_List").Result);
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
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }
        public int CompanyId
        {
            get
            {
                return _CompanyId;
            }
            set
            {
                if (_CompanyId != value)
                {
                    _CompanyId = value;
                    NotifyPropertyChanged(nameof(CompanyId));
                }

            }
        }
        public CompanyDetails Company
        {
            get
            {
                return _Company;
            }
            set
            {
                if (_Company != value)
                {
                    _Company = value;
                    NotifyPropertyChanged(nameof(Company));
                }
            }
        }

        public ObservableCollection<UserTypeDetail> UserTypeDetails
        {
            get
            {
                if (_UserTypeDetails == null) _UserTypeDetails = new ObservableCollection<UserTypeDetail>();
                return _UserTypeDetails;
            }
            set
            {
                if (_UserTypeDetails != value)
                {
                    _UserTypeDetails = value;
                    NotifyPropertyChanged(nameof(UserTypeDetails));
                }
            }
        }

        #endregion

        #region Property Notify Changed

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

        #region Methods

        public bool isValid()
        {
            bool RValue = true;
            lstValidation.Clear();
            if (string.IsNullOrWhiteSpace(Name))
            {
                lstValidation.Add(new Validation() { Name = nameof(Name), Message = string.Format(MSG.BLL.Required_Data, nameof(Name)) });
            }
            else if (toList.Where(x => x.Name.ToLower() == Name.ToLower() && x.Id != Id).Count() > 0)
            {
                lstValidation.Add(new Validation() { Name = nameof(Name), Message = string.Format(MSG.BLL.Existing_Data, Name) });
                RValue = false;
            }
            return RValue;
        }

        public bool Save(bool isServerCall = false)
        {
            if (!isValid()) return false;
            try
            {
                UserType d = toList.Where(x => x.Id == Id).FirstOrDefault();
                if (d == null)
                {
                    d = new UserType();
                    toList.Add(d);
                }
                this.toCopy<UserType>(d);
                if (isServerCall == false)
                {
                    var i = PYClientHub.PYHub.Invoke<int>("userType_Save", this).Result;
                    d.Id = i;
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return false;
            }
            return true;
        }

        public void Clear()
        {
            new UserType().toCopy<UserType>(this);
            this.UserTypeDetails = new ObservableCollection<UserTypeDetail>();
            foreach (var d in UserTypeFormDetail.toList)
            {
                UserTypeDetail utd = new UserTypeDetail();
                utd.UserTypeFormDetail = d.toCopy<BLL.UserTypeFormDetail>(new UserTypeFormDetail());
                utd.UserTypeFormDetailId = utd.UserTypeFormDetail.Id;
                UserTypeDetails.Add(utd);
            }
            IsReadOnly = !UserPermission.AllowInsert;
            NotifyAllPropertyChanged();
        }

        public bool Find(int pk)
        {
            var d = toList.Where(x => x.Id == pk).FirstOrDefault();
            if (d != null)
            {
                d.toCopy<UserType>(this);
                IsReadOnly = !UserPermission.AllowUpdate;
                return true;
            }
            return false;
        }

        public bool Delete(bool isServerCall = false)
        {
            var d = toList.Where(x => x.Id == Id).FirstOrDefault();
            if (d != null)
            {
                toList.Remove(d);
                if (isServerCall == false) PYClientHub.PYHub.Invoke<int>("userType_Delete", this.Id);
                return true;
            }

            return false;
        }

        #endregion

    }
}
