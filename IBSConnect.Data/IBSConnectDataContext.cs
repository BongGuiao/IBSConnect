using Microsoft.EntityFrameworkCore;
using IBSConnect.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace IBSConnect.Data;

public class IBSConnectDataContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Year> Years { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<College> Colleges { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<MemberSession> MemberSessions { get; set; }
    public DbSet<MemberCredit> MemberCredits { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<UnitArea> UnitAreas { get; set; }
    public DbSet<SessionApplication> SessionApplications { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<BillingPeriod> BillingPeriods { get; set; }
    public DbSet<MemberBill> MemberBills { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentArrear> PaymentArrears { get; set; }
    public DbSet<IBSTranHistory> IBSTranHistories { get; set; }
    public DbSet<IBSResetHistory> IBSResetHistories { get; set; }



    public IBSConnectDataContext(DbContextOptions<IBSConnectDataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .ToTable("categories")
            .HasKey(e => e.Id);

        modelBuilder.Entity<Year>()
            .ToTable("years")
            .HasKey(e => e.Id);

        modelBuilder.Entity<Course>()
            .ToTable("courses")
            .HasKey(e => e.Id);

        modelBuilder.Entity<Section>()
            .ToTable("sections")
            .HasKey(e => e.Id);

        modelBuilder.Entity<College>()
            .ToTable("colleges")
            .HasKey(e => e.Id);

        modelBuilder.Entity<User>()
            .ToTable("users")
            .HasKey(e => e.Id);

        var members = modelBuilder.Entity<Member>()
            .ToTable("members");
        members.HasKey(e => e.Id);

        members.HasMany(f => f.Credits)
            .WithOne()
            .HasForeignKey(f => f.MemberId)
            ;

        members.HasMany(f => f.Sessions)
            .WithOne()
            .HasForeignKey(f => f.MemberId)
            ;

        members.HasMany(f => f.Payments)
            .WithOne()
            .HasForeignKey(f => f.MemberId)
            ;

        var memberCredit = modelBuilder.Entity<MemberCredit>()
            .ToTable("member_credits");
        
        memberCredit.HasKey(e => e.Id);
        
        var memberSession = modelBuilder.Entity<MemberSession>()
            .ToTable("member_sessions");

        memberSession.HasKey(e => e.Id);

        memberSession
            .HasMany(f => f.Applications)
            .WithOne(f => f.Session)
            .HasForeignKey(f => f.SessionId);

        memberSession
            .HasOne(f => f.UnitArea);

        modelBuilder.Entity<Application>()
            .ToTable("applications")
            .HasKey(e => e.Id);

        modelBuilder.Entity<UnitArea>()
            .ToTable("unit_areas")
            .HasKey(e => e.Id);

        var sessionApplication = modelBuilder.Entity<SessionApplication>()
            .ToTable("session_applications");

        sessionApplication.HasKey(e => e.Id);

        var setting = modelBuilder.Entity<Setting>()
            .ToTable("settings");

        setting.HasKey(e => e.Id);


        var billingPeriod = modelBuilder.Entity<BillingPeriod>()
            .ToTable("billing_periods");

        billingPeriod.HasKey(e => e.Id);

        var memberbill = modelBuilder.Entity<MemberBill>()
            .ToTable("vw_billing");

        memberbill.HasKey(e => e.MemberId);

        modelBuilder.Entity<Payment>()
            .ToTable("member_payments")
            .HasKey(e => e.Id);
        modelBuilder.Entity<PaymentArrear>()
            .ToTable("member_arrears_payments")
            .HasKey(e => e.Id);
        modelBuilder.Entity<IBSTranHistory>()
            .ToTable("ibs_tran_history")
            .HasKey(e => e.Id);
        modelBuilder.Entity<IBSResetHistory>()
            .ToTable("IBS_RESET_HISTORY")
            .HasKey(e => e.Id);
    }

}