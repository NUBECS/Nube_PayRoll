﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUBE.PAYROLL.CMN;

namespace NUBE.PAYROLL.BLL
{
    public class UserAccount : INotifyPropertyChanged
    {
        #region Field

        public static UserAccount User = new UserAccount();

        private static UserTypeDetail _UserPermission;
        private bool _IsReadOnly;
        private bool _IsEnabled;

        private static ObservableCollection<UserAccount> _toList;

        public List<BLL.Validation> lstValidation = new List<BLL.Validation>();

        private int _Id;
        private string _LoginId;
        private string _Password;
        private int _UserTypeId;
        private string _UserName;

        private UserType _UserType;

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

        public static ObservableCollection<UserAccount> toList
        {
            get
            {
                if (_toList == null)
                {

                }
                return _toList;
            }
            set
            {
                _toList = value;
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

        public string LoginId
        {
            get
            {
                return _LoginId;
            }
            set
            {
                if (_LoginId != value)
                {
                    _LoginId = value;
                    NotifyPropertyChanged(nameof(LoginId));
                }
            }
        }

        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    NotifyPropertyChanged(nameof(Password));
                }
            }
        }

        public int UserTypeId
        {
            get
            {
                return _UserTypeId;
            }
            set
            {
                if (_UserTypeId != value)
                {
                    _UserTypeId = value;
                    NotifyPropertyChanged(nameof(UserTypeId));
                }
            }
        }

        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                if (_UserName != value)
                {
                    _UserName = value;
                    NotifyPropertyChanged(nameof(UserName));
                }
            }
        }

        public UserType UserType
        {
            get
            {
                return _UserType;
            }
            set
            {
                if (_UserType != value)
                {
                    _UserType = value;
                    NotifyPropertyChanged(nameof(UserType));
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

        #region Validations

        public static bool isValidLogin(UserAccount ua, string LId, string pwd)
        {
            bool RValue = true;
            ua.lstValidation.Clear();

            if (string.IsNullOrWhiteSpace(LId))
            {
                ua.lstValidation.Add(new Validation() { Name = nameof(LoginId), Message = "Please Enter the User Id!" });
                RValue = false;
            }

            if (string.IsNullOrWhiteSpace(pwd))
            {
                ua.lstValidation.Add(new Validation() { Name = nameof(Password), Message = "Please Enter the Password!" });
                RValue = false;
            }

            if (RValue == true)
            {
                if (ua.LoginId != LId || ua.Password != pwd)
                {
                    ua.lstValidation.Add(new Validation() { Name = nameof(LoginId), Message = "Please Enter the Valid User Id or Password!" });
                    RValue = false;
                }
            }

            return RValue;
        }
        public bool isValid()
        {
            bool RValue = true;
            lstValidation.Clear();

            if (string.IsNullOrWhiteSpace(UserName))
            {
                lstValidation.Add(new Validation() { Name = nameof(UserName), Message = string.Format(MSG.BLL.Required_Data, nameof(UserName)) });
                RValue = false;
            }

            if (string.IsNullOrWhiteSpace(LoginId))
            {
                lstValidation.Add(new Validation() { Name = nameof(LoginId), Message = string.Format(MSG.BLL.Required_Data, nameof(LoginId)) });
                RValue = false;
            }
            else if (toList.Where(x => x.LoginId.ToLower() == LoginId.ToLower() && x.Id != Id).Count() > 0)
            {
                lstValidation.Add(new Validation() { Name = nameof(LoginId), Message = string.Format(MSG.BLL.Existing_Data, LoginId) });
                RValue = false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                lstValidation.Add(new Validation() { Name = nameof(Password), Message = string.Format(MSG.BLL.Required_Data, nameof(Password)) });
                RValue = false;
            }
            if (UserType == null)
            {
                lstValidation.Add(new Validation() { Name = nameof(UserType), Message = string.Format(MSG.BLL.Required_Data, nameof(UserType)) });
                RValue = false;
            }
            return RValue;

        }

        #endregion

        #region Method  

        public static string Login(String LId, String Pwd)
        {
            var ua = PYClientHub.PYHub.Invoke<UserAccount>("UserAccount_Login", LId, Pwd).Result;
            if (isValidLogin(ua, LId, Pwd))
            {
                try
                {
                    User = ua;
                    Data_Init();
                    return "";
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendErrorToText(ex);
                }
            }
            return string.Join("\n", ua.lstValidation.Select(x => x.Message));
        }

        static void Data_Init()
        {
            BLL.UserAccount.Init();
        }

        public static bool AllowFormShow(string FormName)
        {
            bool rv = true;
            var t = User.UserType.UserTypeDetails.Where(x => x.UserTypeFormDetail.FormName == FormName).FirstOrDefault();
            if (t != null) rv = t.IsViewForm;
            return rv;
        }

        public static bool AllowInsert(string FormName)
        {
            bool rv = true;
            var t = User.UserType.UserTypeDetails.Where(x => x.UserTypeFormDetail.FormName == FormName).FirstOrDefault();
            if (t != null) rv = t.AllowInsert;
            return rv;
        }

        public static bool AllowUpdate(string FormName)
        {
            bool rv = true;
            var t = User.UserType.UserTypeDetails.Where(x => x.UserTypeFormDetail.FormName == FormName).FirstOrDefault();
            if (t != null) rv = t.AllowUpdate;
            return rv;
        }

        public static bool AllowDelete(string FormName)
        {
            bool rv = true;
            var t = User.UserType.UserTypeDetails.Where(x => x.UserTypeFormDetail.FormName == FormName).FirstOrDefault();
            if (t != null) rv = t.AllowDelete;
            return rv;
        }

        public bool Save(bool isServerCall = false)
        {
            if (!isValid()) return false;
            try
            {
                UserAccount d = toList.Where(x => x.Id == Id).FirstOrDefault();

                if (d == null)
                {
                    d = new UserAccount();
                    toList.Add(d);
                }

                this.toCopy<UserAccount>(d);
                if (isServerCall == false)
                {
                    var i = PYClientHub.PYHub.Invoke<int>("UserAccount_Save", this).Result;
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

        public void Clear()
        {
            this.UserName = "";
            this.LoginId = "";
            this.Password = "";
            this.UserTypeId = 0;
            this.UserType = null;
            IsReadOnly = !UserPermission.AllowInsert;
            NotifyAllPropertyChanged();
        }

        public bool Find(int pk)
        {
            var d = toList.Where(x => x.Id == pk).FirstOrDefault();
            if (d != null)
            {
                d.toCopy<UserAccount>(this);
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
                if (isServerCall == false) PYClientHub.PYHub.Invoke<int>("UserAccount_Delete", this.Id);
                return true;
            }
            return false;
        }

        public static void Init()
        {
            _toList = null;
            UserPermission = null;

            UserType.UserPermission = null;
        }

        #endregion
    }
}
