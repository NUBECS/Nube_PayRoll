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
    
    public partial class MasterState
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MasterState()
        {
            this.MasterCities = new HashSet<MasterCity>();
            this.MasterEmployees = new HashSet<MasterEmployee>();
        }
    
        public int Id { get; set; }
        public string StateName { get; set; }
        public string ShortName { get; set; }
        public Nullable<int> CountryId { get; set; }
        public bool IsCancel { get; set; }
        public Nullable<System.DateTime> CancelOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MasterCity> MasterCities { get; set; }
        public virtual MasterCountry MasterCountry { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MasterEmployee> MasterEmployees { get; set; }
    }
}
