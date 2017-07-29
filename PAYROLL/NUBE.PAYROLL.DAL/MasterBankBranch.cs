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
    
    public partial class MasterBankBranch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MasterBankBranch()
        {
            this.MasterEmployees = new HashSet<MasterEmployee>();
        }
    
        public int Id { get; set; }
        public string BankBranchName { get; set; }
        public string UserCode { get; set; }
        public Nullable<int> BankId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public Nullable<int> CityId { get; set; }
        public string ZipCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public Nullable<int> NubeBranchId { get; set; }
        public Nullable<bool> IsHeadQuarters { get; set; }
    
        public virtual MasterBank MasterBank { get; set; }
        public virtual MasterCity MasterCity { get; set; }
        public virtual MasterNubeBranch MasterNubeBranch { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MasterEmployee> MasterEmployees { get; set; }
    }
}
