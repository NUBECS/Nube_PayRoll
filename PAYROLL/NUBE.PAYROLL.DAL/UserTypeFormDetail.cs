//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NUBE.PAYROLL.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserTypeFormDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserTypeFormDetail()
        {
            this.UserTypeDetails = new HashSet<UserTypeDetail>();
        }
    
        public int Id { get; set; }
        public string FormName { get; set; }
        public string FormType { get; set; }
        public string OrderNo { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsInsert { get; set; }
        public Nullable<bool> IsUpdate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> IsMenu { get; set; }
        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserTypeDetail> UserTypeDetails { get; set; }
    }
}
