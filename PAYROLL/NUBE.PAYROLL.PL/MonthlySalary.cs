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
    
    public partial class MonthlySalary
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int EmployeeNo { get; set; }
        public Nullable<System.DateTime> SalaryMonth { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<System.DateTime> DateOfJoining { get; set; }
        public string EPFNumber { get; set; }
        public string SOCSONumber { get; set; }
        public string NRIC { get; set; }
        public string BankAccountNo { get; set; }
        public string BankName { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal TotalWorkingDays { get; set; }
        public decimal DaysAbsent { get; set; }
        public decimal OTHours { get; set; }
        public decimal OT_Amount { get; set; }
        public decimal LOP_Leave { get; set; }
        public decimal LOP_Late { get; set; }
        public decimal POB { get; set; }
        public decimal NEC { get; set; }
        public decimal SECONDMENT { get; set; }
        public decimal SPECIAL { get; set; }
        public decimal COLA { get; set; }
        public decimal MOBILEALLOWANCE { get; set; }
        public decimal ALLOWANCE { get; set; }
        public decimal BONUS { get; set; }
        public decimal EXGRATIA { get; set; }
        public decimal EPF { get; set; }
        public decimal EPFUNION { get; set; }
        public decimal SOCSO { get; set; }
        public decimal SOCSOUNION { get; set; }
        public decimal INCOMETAX { get; set; }
        public decimal GMIS { get; set; }
        public decimal GELA { get; set; }
        public decimal BIMBLOAN { get; set; }
        public decimal HOMECARLOANS { get; set; }
        public decimal OTHERLOAN { get; set; }
        public decimal TOTALLOAN { get; set; }
        public decimal OTHERS { get; set; }
        public decimal KOPERASI { get; set; }
        public decimal NUBESUBSCRIPTION { get; set; }
        public decimal PCB { get; set; }
        public decimal AllowanceInAdvanced { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal TOTALEARNING { get; set; }
        public decimal TOTALDEDUCTION { get; set; }
        public decimal NETSALARY { get; set; }
        public string PositionName { get; set; }
        public string SIPNumber { get; set; }
        public decimal SIP { get; set; }
        public decimal SIPUNION { get; set; }
    }
}
