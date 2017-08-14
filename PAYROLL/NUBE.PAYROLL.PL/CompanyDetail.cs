//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NUBE.PAYROLL.PL
{
    using System;
    using System.Collections.Generic;
    
    public partial class CompanyDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompanyDetail()
        {
            this.UserTypes = new HashSet<UserType>();
            this.ErrorLogs = new HashSet<ErrorLog>();
        }
    
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public Nullable<int> CityCode { get; set; }
        public Nullable<int> StateCode { get; set; }
        public Nullable<int> CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string TelephoneNo { get; set; }
        public string MobileNo { get; set; }
        public string EMailId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string GSTNo { get; set; }
        public Nullable<System.DateTime> InTime { get; set; }
        public Nullable<System.DateTime> OutTime { get; set; }
        public Nullable<System.DateTime> MinimumOtTime { get; set; }
        public bool WeekofTwoDays { get; set; }
        public bool IsGraceTime { get; set; }
        public Nullable<decimal> GraceTime { get; set; }
        public string ServerName { get; set; }
        public string DbName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserType> UserTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ErrorLog> ErrorLogs { get; set; }
    }
}
