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
    
    public partial class MonthlyDeduction
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<decimal> AllowanceInAdvanced { get; set; }
        public Nullable<decimal> OtherDeductions { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public decimal DispatchAllowance { get; set; }
    
        public virtual MasterEmployee MasterEmployee { get; set; }
    }
}
