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
    
    public partial class AttedanceLog
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<System.DateTime> InTime { get; set; }
        public Nullable<System.DateTime> OutTime { get; set; }
        public Nullable<decimal> WorkingHours { get; set; }
        public bool IsNotLogOut { get; set; }
    
        public virtual MasterEmployee MasterEmployee { get; set; }
    }
}
