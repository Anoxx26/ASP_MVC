using System;
using System.Collections.Generic;
using ASP_MVC.Data.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ASP_MVC.Data;

public partial class MydbContext : DbContext
{
    public MydbContext()
    {
    }

    public MydbContext(DbContextOptions<MydbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Problem> Problems { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<WorkStatus> WorkStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=1234;database=mydb", ServerVersion.Parse("8.0.36-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.IdApplication).HasName("PRIMARY");

            entity.ToTable("applications");

            entity.HasIndex(e => e.IdProblem, "problemFK_idx");

            entity.HasIndex(e => e.IdStaff, "staffFK_idx");

            entity.HasIndex(e => e.Status, "statusesFK_idx");

            entity.Property(e => e.IdApplication).HasColumnName("id_application");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasColumnName("comment");
            entity.Property(e => e.Cost)
                .HasPrecision(8, 2)
                .HasColumnName("cost");
            entity.Property(e => e.DateAddition).HasColumnName("date_addition");
            entity.Property(e => e.DateEnd).HasColumnName("date_end");
            entity.Property(e => e.FullnameClient)
                .HasMaxLength(45)
                .HasColumnName("fullname_client");
            entity.Property(e => e.IdProblem).HasColumnName("id_problem");
            entity.Property(e => e.IdStaff).HasColumnName("id_staff");
            entity.Property(e => e.NameEquipment)
                .HasMaxLength(45)
                .HasColumnName("name_equipment");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TimeWork).HasColumnName("time_work");
            entity.Property(e => e.WorkStatus)
                .HasMaxLength(45)
                .HasColumnName("work_status");

            entity.HasOne(d => d.IdProblemNavigation).WithMany(p => p.Applications)
                .HasForeignKey(d => d.IdProblem)
                .HasConstraintName("problemFK");

            entity.HasOne(d => d.IdStaffNavigation).WithMany(p => p.Applications)
                .HasForeignKey(d => d.IdStaff)
                .HasConstraintName("staffFK");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Applications)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("statusesFK");
        });

        modelBuilder.Entity<Problem>(entity =>
        {
            entity.HasKey(e => e.IdProblem).HasName("PRIMARY");

            entity.ToTable("problems");

            entity.Property(e => e.IdProblem).HasColumnName("id_problem");
            entity.Property(e => e.NameProblem)
                .HasMaxLength(60)
                .HasColumnName("name_problem");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Idrole).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.Idrole).HasColumnName("idrole");
            entity.Property(e => e.RoleName)
                .HasMaxLength(60)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.IdStaff).HasName("PRIMARY");

            entity.ToTable("staff");

            entity.HasIndex(e => e.Role, "rolesFK_idx");

            entity.Property(e => e.IdStaff).HasColumnName("id_staff");
            entity.Property(e => e.Fullname)
                .HasMaxLength(90)
                .HasColumnName("fullname");
            entity.Property(e => e.Login)
                .HasMaxLength(45)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");
            entity.Property(e => e.Role).HasColumnName("role");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("RolesFK");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Idstatus).HasName("PRIMARY");

            entity.ToTable("statuses");

            entity.Property(e => e.Idstatus).HasColumnName("idstatus");
            entity.Property(e => e.StatusName)
                .HasMaxLength(60)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<WorkStatus>(entity =>
        {
            entity.HasKey(e => e.IdworkStatus).HasName("PRIMARY");

            entity.ToTable("work_statuses");

            entity.Property(e => e.IdworkStatus).HasColumnName("idwork_status");
            entity.Property(e => e.WorkStatusName)
                .HasMaxLength(60)
                .HasColumnName("work_status_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
