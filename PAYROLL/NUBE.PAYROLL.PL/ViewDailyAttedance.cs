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
    
    public partial class ViewDailyAttedance
    {
        public Nullable<long> RNO { get; set; }
        public int EMPLOYEEID { get; set; }
        public Nullable<int> MEMBERSHIPNO { get; set; }
        public string EMPLOYEENAME { get; set; }
        public string POSITIONNAME { get; set; }
        public Nullable<System.DateTime> ATTDATE { get; set; }
        public Nullable<System.TimeSpan> INTIME { get; set; }
        public Nullable<System.TimeSpan> OUTTIME { get; set; }
        public decimal TOTALWORKINGHOURS { get; set; }
        public bool WITHPERMISSION { get; set; }
        public Nullable<decimal> OTHOURS { get; set; }
        public string REMARKS { get; set; }
        public bool ISFULLDAYLEAVE { get; set; }
        public bool ISHALFDAYLEAVE { get; set; }
    }
}
