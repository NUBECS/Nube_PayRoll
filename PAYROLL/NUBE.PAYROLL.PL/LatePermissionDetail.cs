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
    
    public partial class LatePermissionDetail
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<System.DateTime> FromTime { get; set; }
        public Nullable<System.DateTime> ToTime { get; set; }
        public bool IsApproved { get; set; }
        public Nullable<System.TimeSpan> NoOfHours { get; set; }
        public Nullable<System.DateTime> AprovedFromTime { get; set; }
        public Nullable<System.DateTime> AprovedToTime { get; set; }
        public Nullable<System.TimeSpan> NoOfHoursApproved { get; set; }
        public string Reason { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
    
        public virtual MasterEmployee MasterEmployee { get; set; }
    }
}
