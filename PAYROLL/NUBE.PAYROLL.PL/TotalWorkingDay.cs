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
    
    public partial class TotalWorkingDay
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public int TotalWorkingDays { get; set; }
        public int DaysAbsent { get; set; }
    
        public virtual MasterEmployee MasterEmployee { get; set; }
    }
}
