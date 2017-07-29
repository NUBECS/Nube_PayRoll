using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUBE.PAYROLL.CMN;
using System.ComponentModel;

namespace NUBE.PAYROLL.BLL
{
    public class CompanyDetails
    {
        #region Fields

        bool isServerCall = false;

        private static ObservableCollection<CompanyDetails> _toList;
        private static UserTypeDetail _UserPermission;
        private bool _IsReadOnly;
        private bool _IsEnabled;

        public List<BLL.Validation> lstValidation = new List<BLL.Validation>();

        private int _Id;
        private string _CompanyName;
        private string _AddressLine1;
        private string _AddressLine2;
        private int _CityCode;
        private int _StateCode;
        private int _CountryCode;
        private string _PostalCode;
        private string _TelephoneNo;
        private string _MobileNo;
        private string _EMailId;
        private int _IsActive;
        private string _GSTNo;

        #endregion

        #region Property

        public static UserTypeDetail UserPermission
        {
            get
            {
                if (_UserPermission == null)
                {
                    _UserPermission = UserAccount.User.UserType == null ? new UserTypeDetail() : UserAccount.User.UserType.UserTypeDetails.Where(x => x.UserTypeFormDetail.FormName == AppLib.Forms.frmCompanySetting.ToString()).FirstOrDefault();
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

        public static ObservableCollection<CompanyDetails> toList
        {
            get
            {
                if (_toList == null)
                {
                    var l1 = PYClientHub.PYHub.Invoke<List<CompanyDetails>>("CompanyDetails_List").Result;
                    _toList = new ObservableCollection<CompanyDetails>(l1);
                }

                return _toList;
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

        public string CompanyName
        {
            get
            {
                return _CompanyName;
            }

            set
            {
                if (_CompanyName != value)
                {
                    _CompanyName = value;
                    NotifyPropertyChanged(nameof(CompanyName));
                }
            }
        }
        public string AddressLine1
        {
            get
            {
                return _AddressLine1;
            }

            set
            {
                if (_AddressLine1 != value)
                {
                    _AddressLine1 = value;
                    NotifyPropertyChanged(nameof(AddressLine1));
                }
            }
        }
        public string AddressLine2
        {
            get
            {
                return _AddressLine2;
            }

            set
            {
                if (_AddressLine2 != value)
                {
                    _AddressLine2 = value;
                    NotifyPropertyChanged(nameof(AddressLine2));
                }
            }
        }

        public int CityCode
        {
            get
            {
                return _CityCode;
            }

            set
            {
                if (_CityCode != value)
                {
                    _CityCode = value;
                    NotifyPropertyChanged(nameof(CityCode));
                }
            }
        }
        public int StateCode
        {
            get
            {
                return _StateCode;
            }

            set
            {
                if (_StateCode != value)
                {
                    _StateCode = value;
                    NotifyPropertyChanged(nameof(StateCode));
                }
            }
        }
        public int CountryCode
        {
            get
            {
                return _CountryCode;
            }

            set
            {
                if (_CountryCode != value)
                {
                    _CountryCode = value;
                    NotifyPropertyChanged(nameof(CountryCode));
                }
            }
        }

        public string PostalCode
        {
            get
            {
                return _PostalCode;
            }

            set
            {
                if (_PostalCode != value)
                {
                    _PostalCode = value;
                    NotifyPropertyChanged(nameof(PostalCode));
                }
            }
        }
        public string TelephoneNo
        {
            get
            {
                return _TelephoneNo;
            }

            set
            {
                if (_TelephoneNo != value)
                {
                    _TelephoneNo = value;
                    NotifyPropertyChanged(nameof(TelephoneNo));
                }
            }
        }
        public string MobileNo
        {
            get
            {
                return _MobileNo;
            }

            set
            {
                if (_MobileNo != value)
                {
                    _MobileNo = value;
                    NotifyPropertyChanged(nameof(MobileNo));
                }
            }
        }
        public string EMailId
        {
            get
            {
                return _EMailId;
            }

            set
            {
                if (_EMailId != value)
                {
                    _EMailId = value;
                    NotifyPropertyChanged(nameof(EMailId));
                }
            }
        }
        public int IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                if (_IsActive != value)
                {
                    _IsActive = value;
                    NotifyPropertyChanged(nameof(IsActive));
                }
            }
        }
        public string GSTNo
        {
            get
            {
                return _GSTNo;
            }

            set
            {
                if (_GSTNo != value)
                {
                    _GSTNo = value;
                    NotifyPropertyChanged(nameof(GSTNo));
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


            if (string.IsNullOrWhiteSpace(CompanyName))
            {
                lstValidation.Add(new Validation() { Name = nameof(CompanyName), Message = string.Format(MSG.BLL.Required_Data, nameof(CompanyName)) });
                RValue = false;
            }
            else if (toList.Where(x => x.CompanyName.ToLower() == CompanyName.ToLower() && x.Id != Id).Count() > 0)
            {
                lstValidation.Add(new Validation() { Name = nameof(CompanyName), Message = string.Format(MSG.BLL.Existing_Data, CompanyName) });
                RValue = false;
            }
            //if (Id == 0)
            //{
            //    if (string.IsNullOrWhiteSpace(UserId))
            //    {
            //        lstValidation.Add(new Validation() { Name = nameof(UserId), Message = string.Format(MSG.BLL.Required_Data, nameof(UserId)) });
            //        RValue = false;
            //    }

            //    if (string.IsNullOrWhiteSpace(Password))
            //    {
            //        lstValidation.Add(new Validation() { Name = nameof(Password), Message = string.Format(MSG.BLL.Required_Data, nameof(Password)) });
            //        RValue = false;
            //    }

            //}

            return RValue;

        }

        #endregion

        #region Methods

        public bool Save(bool isServerCall = false)
        {
            if (!isValid()) return false;
            try
            {
                CompanyDetails d = toList.Where(x => x.Id == Id).FirstOrDefault();
                int i = 0;
                if (d == null)
                {
                    d = new CompanyDetails();
                    toList.Add(d);
                }

                this.toCopy<CompanyDetails>(d);
                if (isServerCall == false)
                {
                    i = PYClientHub.PYHub.Invoke<int>("CompanyDetails_Save", this).Result;
                    d.Id = i;
                }

                return i != 0;
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return false;
            }

        }

        public void Clear()
        {
            new CompanyDetails().toCopy<CompanyDetails>(this);
            IsReadOnly = !UserPermission.AllowInsert;

            NotifyAllPropertyChanged();
        }

        public bool Find(int pk)
        {
            var d = toList.Where(x => x.Id == pk).FirstOrDefault();
            if (d != null)
            {
                d.toCopy<CompanyDetails>(this);
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
                if (isServerCall == false) PYClientHub.PYHub.Invoke<int>("CompanyDetails_Delete", this.Id);
                return true;
            }

            return false;
        }

        public bool DeleteWareHouse(int Id)
        {
            var c = toList.Where(x => x.Id == Id).FirstOrDefault();

            if (c != null)
            {
                toList.Remove(c);
                if (isServerCall == false) PYClientHub.PYHub.Invoke<int>("Company_Delete", c.Id);
                return true;
            }
            return false;
        }


        #endregion

    }
}
