﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PayrollEntity : DbContext
    {
        public PayrollEntity()
            : base("name=PayrollEntity")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CompanyDetail> CompanyDetails { get; set; }
        public virtual DbSet<EntityType> EntityTypes { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<LogDetail> LogDetails { get; set; }
        public virtual DbSet<LogDetailType> LogDetailTypes { get; set; }
        public virtual DbSet<LogMaster> LogMasters { get; set; }
        public virtual DbSet<MasterBank> MasterBanks { get; set; }
        public virtual DbSet<MasterBankBranch> MasterBankBranches { get; set; }
        public virtual DbSet<MasterCity> MasterCities { get; set; }
        public virtual DbSet<MasterCountry> MasterCountries { get; set; }
        public virtual DbSet<MasterNubeBranch> MasterNubeBranches { get; set; }
        public virtual DbSet<MasterRace> MasterRaces { get; set; }
        public virtual DbSet<MasterState> MasterStates { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<UserTypeDetail> UserTypeDetails { get; set; }
        public virtual DbSet<UserTypeFormDetail> UserTypeFormDetails { get; set; }
        public virtual DbSet<ViewMasterCity> ViewMasterCities { get; set; }
        public virtual DbSet<ViewMasterState> ViewMasterStates { get; set; }
        public virtual DbSet<ViewMasterbankbranch> ViewMasterbankbranches { get; set; }
        public virtual DbSet<ViewEmployeeDetail> ViewEmployeeDetails { get; set; }
        public virtual DbSet<ViewMasterEmployee> ViewMasterEmployees { get; set; }
        public virtual DbSet<ViewMasterPosition> ViewMasterPositions { get; set; }
        public virtual DbSet<YearlyAllowance> YearlyAllowances { get; set; }
        public virtual DbSet<TempAttendanceTiming> TempAttendanceTimings { get; set; }
        public virtual DbSet<EPFCont> EPFConts { get; set; }
        public virtual DbSet<SocsoCont> SocsoConts { get; set; }
        public virtual DbSet<HolidayList> HolidayLists { get; set; }
        public virtual DbSet<LatePermissionDetail> LatePermissionDetails { get; set; }
        public virtual DbSet<LeavePermissionDetail> LeavePermissionDetails { get; set; }
        public virtual DbSet<DailyAttedanceDet> DailyAttedanceDets { get; set; }
        public virtual DbSet<OverTimeDetail> OverTimeDetails { get; set; }
        public virtual DbSet<TotalWorkingDay> TotalWorkingDays { get; set; }
        public virtual DbSet<ViewDailyAttedance> ViewDailyAttedances { get; set; }
        public virtual DbSet<VIEWDAILYATTEDANCELATE> VIEWDAILYATTEDANCELATEs { get; set; }
        public virtual DbSet<VIEWTOTALOT> VIEWTOTALOTs { get; set; }
        public virtual DbSet<VIEWPAYSLIP> VIEWPAYSLIPs { get; set; }
        public virtual DbSet<MasterEmployee> MasterEmployees { get; set; }
        public virtual DbSet<PCB> PCBs { get; set; }
        public virtual DbSet<ViewManualPayment> ViewManualPayments { get; set; }
        public virtual DbSet<ViewOTCalculation> ViewOTCalculations { get; set; }
        public virtual DbSet<LeaveType> LeaveTypes { get; set; }
        public virtual DbSet<MonthlySalary> MonthlySalaries { get; set; }
        public virtual DbSet<EmployeeIncrement> EmployeeIncrements { get; set; }
        public virtual DbSet<OTClaim> OTClaims { get; set; }
        public virtual DbSet<MasterPosition> MasterPositions { get; set; }
    }
}
