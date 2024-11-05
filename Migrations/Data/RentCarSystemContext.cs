using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;

namespace RentCarSystem.Migrations.Data;

public partial class RentCarSystemContext : DbContext
{
    public RentCarSystemContext()
    {
    }

    public RentCarSystemContext(DbContextOptions<RentCarSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<ApprovalRequest> ApprovalRequests { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Individual> Individuals { get; set; }

    public virtual DbSet<Motor> Motors { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<RentalAgreement> RentalAgreements { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleHireService> VehicleHireServices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("pk_Admin");

            entity.ToTable("Admin");

            entity.Property(e => e.AdminId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("AdminID");
            entity.Property(e => e.LastLogin).HasColumnType("datetime");

            entity.HasOne(d => d.AdminNavigation).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Admin");
        });

        modelBuilder.Entity<ApprovalRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("pk_ApprovalRequests");

            entity.HasIndex(e => e.BsnId, "UQ__Approval__240DA66D8D0DBCE6").IsUnique();

            entity.Property(e => e.RequestId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("RequestID");
            entity.Property(e => e.AdminId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("AdminID");
            entity.Property(e => e.BsnId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("BsnID");
            entity.Property(e => e.Status).HasMaxLength(10);

            entity.HasOne(d => d.Admin).WithMany(p => p.ApprovalRequests)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkAdm_ApprovalRequests");

            entity.HasOne(d => d.Bsn).WithOne(p => p.ApprovalRequest)
                .HasForeignKey<ApprovalRequest>(d => d.BsnId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkBsn_ApprovalRequests");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("pk_Bill");

            entity.ToTable("Bill");

            entity.Property(e => e.BillId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("BillID");
            entity.Property(e => e.AgreementId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("AgreementID");
            entity.Property(e => e.CusId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CusID");
            entity.Property(e => e.PaymentImg)
                .HasMaxLength(100)
                .HasColumnName("Payment_img");
            entity.Property(e => e.Status).HasMaxLength(10);

            entity.HasOne(d => d.Agreement).WithMany(p => p.Bills)
                .HasForeignKey(d => d.AgreementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_AgreementID_Bill");

            entity.HasOne(d => d.Cus).WithMany(p => p.Bills)
                .HasForeignKey(d => d.CusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_CusID_Bill");
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.BsnId).HasName("pk_Business");

            entity.ToTable("Business");

            entity.HasIndex(e => e.UserId, "UQ__Business__1788CCADF68B758D").IsUnique();

            entity.HasIndex(e => e.Description, "UQ__Business__4EBBBAC99B91EE6F").IsUnique();

            entity.Property(e => e.BsnId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("BsnID");
            entity.Property(e => e.BusinessImg)
                .HasMaxLength(100)
                .HasColumnName("Business_img");
            entity.Property(e => e.DateOfIssue).HasColumnName("Date_of_issue");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.IssuingLocation)
                .HasMaxLength(100)
                .HasColumnName("Issuing_location");
            entity.Property(e => e.RegistrationDate).HasColumnName("Registration_date");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("UserID");
            entity.Property(e => e.Vat).HasColumnName("VAT");

            entity.HasOne(d => d.User).WithOne(p => p.Business)
                .HasForeignKey<Business>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Business");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("pk_Car");

            entity.ToTable("Car");

            entity.HasIndex(e => e.VehicleId, "UQ__Car__476B54B3EAE78138").IsUnique();

            entity.Property(e => e.CarId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("CarID");
            entity.Property(e => e.CarBrand).HasMaxLength(100);
            entity.Property(e => e.CarImage).HasMaxLength(100);
            entity.Property(e => e.FuelType)
                .HasMaxLength(10)
                .HasColumnName("Fuel_type");
            entity.Property(e => e.VehicleId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("VehicleID");

            entity.HasOne(d => d.Vehicle).WithOne(p => p.Car)
                .HasForeignKey<Car>(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Car");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.LicenseId).HasName("pk_Customer");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.UserId, "UQ__Customer__1788CCAD93A3F9A8").IsUnique();

            entity.Property(e => e.LicenseId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("LicenseID");
            entity.Property(e => e.Class)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Customer");
        });

        modelBuilder.Entity<Individual>(entity =>
        {
            entity.HasKey(e => e.IdvId).HasName("pk_Individual");

            entity.ToTable("Individual");

            entity.HasIndex(e => e.UserId, "UQ__Individu__1788CCAD8B25887C").IsUnique();

            entity.Property(e => e.IdvId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("idvID");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Individual)
                .HasForeignKey<Individual>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Individual");
        });

        modelBuilder.Entity<Motor>(entity =>
        {
            entity.HasKey(e => e.MotorId).HasName("pk_Motor");

            entity.ToTable("Motor");

            entity.HasIndex(e => e.VehicleId, "UQ__Motor__476B54B3AAB2BDE1").IsUnique();

            entity.Property(e => e.MotorId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("MotorID");
            entity.Property(e => e.MotorImage).HasMaxLength(100);
            entity.Property(e => e.VehicleId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("VehicleID");

            entity.HasOne(d => d.Vehicle).WithOne(p => p.Motor)
                .HasForeignKey<Motor>(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Motor");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("pk_Notification");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("NotificationID");
            entity.Property(e => e.Message).HasMaxLength(300);
            entity.Property(e => e.ReceiverId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ReceiverID");
            entity.Property(e => e.SenderId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("SenderID");

            entity.HasOne(d => d.Receiver).WithMany(p => p.NotificationReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkReceiver_Notification");

            entity.HasOne(d => d.Sender).WithMany(p => p.NotificationSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkSender_Notification");
        });

        modelBuilder.Entity<RentalAgreement>(entity =>
        {
            entity.HasKey(e => e.AgreementId).HasName("pk_RentalAgreement");

            entity.ToTable("RentalAgreement");

            entity.Property(e => e.AgreementId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("AgreementID");
            entity.Property(e => e.CusId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CusID");
            entity.Property(e => e.PaymentMethod).HasMaxLength(10);
            entity.Property(e => e.ServiceId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ServiceID");
            entity.Property(e => e.Status).HasMaxLength(10);
            entity.Property(e => e.VehicleId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("VehicleID");

            entity.HasOne(d => d.Cus).WithMany(p => p.RentalAgreements)
                .HasForeignKey(d => d.CusId)
                .HasConstraintName("fk_Cus_RentalAgreement");

            entity.HasOne(d => d.Service).WithMany(p => p.RentalAgreements)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("fk_ServiceID_RentalAgreement");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.RentalAgreements)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("fk_Ve_RentalAgreement");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("pk_Review");

            entity.ToTable("Review");

            entity.Property(e => e.ReviewId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ReviewID");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.CusId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CusID");
            entity.Property(e => e.VehicleId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("VehicleID");

            entity.HasOne(d => d.Cus).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.CusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_CusID_Review");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_VehicleID_Review");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("pk_Role");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Type).HasMaxLength(10);
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Roles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Role");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("pk_User");

            entity.ToTable("User");

            entity.HasIndex(e => e.PhoneNumber, "UQ__User__85FB4E3893E55C93").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534E299CF9F").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Nationality).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.PhoneNumber).HasMaxLength(10);
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("pk_Vehicle");

            entity.ToTable("Vehicle");

            entity.Property(e => e.VehicleId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("VehicleID");
            entity.Property(e => e.Category).HasMaxLength(10);
            entity.Property(e => e.LicensePlate).HasMaxLength(8);
            entity.Property(e => e.Status).HasMaxLength(15);
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Vehicle");
        });

        modelBuilder.Entity<VehicleHireService>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("pk_Vehicle_Hire_Service");

            entity.ToTable("Vehicle_Hire_Service");

            entity.HasIndex(e => e.BankAccount, "UQ__Vehicle___D70583E0AE07956B").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("UserID");
            entity.Property(e => e.BankAccount)
                .HasMaxLength(15)
                .HasColumnName("Bank_account");
            entity.Property(e => e.BankName)
                .HasMaxLength(50)
                .HasColumnName("Bank_name");
            entity.Property(e => e.ServiceType).HasMaxLength(10);

            entity.HasOne(d => d.User).WithOne(p => p.VehicleHireService)
                .HasForeignKey<VehicleHireService>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Vehicle_Hire_Service");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
