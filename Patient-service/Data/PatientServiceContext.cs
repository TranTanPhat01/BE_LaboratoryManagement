using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Patient_service.Models;

namespace Patient_service;

public partial class PatientServiceContext : DbContext
{
    public PatientServiceContext()
    {
    }

    public PatientServiceContext(DbContextOptions<PatientServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LabCriterion> LabCriteria { get; set; }

    public virtual DbSet<LabTest> LabTests { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LabCriterion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_lab_criteria");

            entity.ToTable("lab_criteria");

            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CriteriaName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("criteria_name");
            entity.Property(e => e.ReferenceRange)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("reference_range");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<LabTest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_lab_tests");

            entity.ToTable("lab_tests");

            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("price");
            entity.Property(e => e.TestName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("test_name");

            entity.HasMany(d => d.Criteria).WithMany(p => p.LabTests)
                .UsingEntity<Dictionary<string, object>>(
                    "LabTestCriterion",
                    r => r.HasOne<LabCriterion>().WithMany()
                        .HasForeignKey("CriteriaId")
                        .HasConstraintName("fk_ltc_criteria"),
                    l => l.HasOne<LabTest>().WithMany()
                        .HasForeignKey("LabTestId")
                        .HasConstraintName("fk_ltc_lab"),
                    j =>
                    {
                        j.HasKey("LabTestId", "CriteriaId").HasName("pk_lab_test_criteria");
                        j.ToTable("lab_test_criteria");
                        j.IndexerProperty<string>("LabTestId")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("lab_test_id");
                        j.IndexerProperty<string>("CriteriaId")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("criteria_id");
                    });
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_medical_records");

            entity.ToTable("medical_records");

            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.OnsetDate).HasColumnName("onset_date");
            entity.Property(e => e.PatientId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("patient_id");
            entity.Property(e => e.RecordData).HasColumnName("record_data");
            entity.Property(e => e.RecordType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("record_type");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.VerifiedBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("verified_by");

            entity.HasOne(d => d.Patient).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("fk_patient");

            entity.HasMany(d => d.LabTests).WithMany(p => p.MedicalRecords)
                .UsingEntity<Dictionary<string, object>>(
                    "MedicalRecordLabTest",
                    r => r.HasOne<LabTest>().WithMany()
                        .HasForeignKey("LabTestId")
                        .HasConstraintName("fk_mrl_lab_test"),
                    l => l.HasOne<MedicalRecord>().WithMany()
                        .HasForeignKey("MedicalRecordId")
                        .HasConstraintName("fk_mrl_medical_record"),
                    j =>
                    {
                        j.HasKey("MedicalRecordId", "LabTestId").HasName("pk_medical_record_lab_tests");
                        j.ToTable("medical_record_lab_tests");
                        j.IndexerProperty<string>("MedicalRecordId")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("medical_record_id");
                        j.IndexerProperty<string>("LabTestId")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("lab_test_id");
                    });
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__patients__3213E83FFD5F48E0");

            entity.ToTable("patients");

            entity.HasIndex(e => e.PatientCode, "UQ__patients__58D46F1FDAF7239F").IsUnique();

            entity.HasIndex(e => e.Fullname, "idx_patients_name");

            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasColumnType("text")
                .HasColumnName("address");
            entity.Property(e => e.BloodType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("blood_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("fullname");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.IdentityNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("identity_number");
            entity.Property(e => e.PatientCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("patient_code");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("updated_at");
        });

    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
