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
    
    public partial class LeavePermissionDetail
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<int> LeaveTypeId { get; set; }
        public bool IsApproved { get; set; }
        public decimal NoOfDays { get; set; }
        public Nullable<System.DateTime> AprovedFromDate { get; set; }
        public Nullable<System.DateTime> AprovedToDate { get; set; }
        public Nullable<decimal> NoOfDaysApproved { get; set; }
        public string Reason { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
    
        public virtual MasterEmployee MasterEmployee { get; set; }
        public virtual LeaveType LeaveType { get; set; }
    }
}
