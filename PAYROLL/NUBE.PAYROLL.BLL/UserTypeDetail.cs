using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUBE.PAYROLL.BLL
{
    public class UserTypeDetail
    {
        #region Field

        private int _Id;
        private int _UserTypeId;
        private int _UserTypeFormDetailId;
        private bool _IsViewForm;
        private bool _AllowInsert;
        private bool _AllowUpdate;
        private bool _AllowDelete;
        
        private BLL.UserTypeFormDetail _UserTypeFormDetail;

        #endregion

        #region Property

        public BLL.UserTypeFormDetail UserTypeFormDetail
        {
            get
            {
                return _UserTypeFormDetail;
            }
            set
            {
                if (_UserTypeFormDetail != value)
                {
                    _UserTypeFormDetail = value;
                    NotifyPropertyChanged(nameof(UserTypeFormDetail));
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
        public int UserTypeFormDetailId
        {
            get
            {
                return _UserTypeFormDetailId;
            }
            set
            {
                if (_UserTypeFormDetailId != value)
                {
                    _UserTypeFormDetailId = value;
                    NotifyPropertyChanged(nameof(UserTypeFormDetailId));
                }
            }
        }
        public bool IsViewForm
        {
            get
            {
                return _IsViewForm;
            }
            set
            {
                if (_IsViewForm != value)
                {
                    _IsViewForm = value;
                    NotifyPropertyChanged(nameof(IsViewForm));
                    if (value == false)
                    {
                        AllowInsert = false;
                        AllowUpdate = false;
                        AllowDelete = false;
                    }
                }
            }
        }
        public bool AllowInsert
        {
            get
            {
                return _AllowInsert;
            }
            set
            {
                if (_AllowInsert != value)
                {
                    _AllowInsert = value;
                    NotifyPropertyChanged(nameof(AllowInsert));
                }
            }
        }
        public bool AllowUpdate
        {
            get
            {
                return _AllowUpdate;
            }
            set
            {
                if (_AllowUpdate != value)
                {
                    _AllowUpdate = value;
                    NotifyPropertyChanged(nameof(AllowUpdate));
                }
            }
        }
        public bool AllowDelete
        {
            get
            {
                return _AllowDelete;
            }
            set
            {
                if (_AllowDelete != value)
                {
                    _AllowDelete = value;
                    NotifyPropertyChanged(nameof(AllowDelete));
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

        #endregion

    }
}
