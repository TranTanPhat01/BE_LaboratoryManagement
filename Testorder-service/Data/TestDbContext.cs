using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TestOrderService.Models;

namespace TestOrderService.Data;

public partial class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<blood_sample> blood_samples { get; set; }

    public virtual DbSet<flagging_config_log> flagging_config_logs { get; set; }

    public virtual DbSet<flagging_configuration> flagging_configurations { get; set; }

    public virtual DbSet<result_comment> result_comments { get; set; }

    public virtual DbSet<test_parameter> test_parameters { get; set; }

    public virtual DbSet<test_result> test_results { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<blood_sample>(entity =>
        {
            entity.HasKey(e => e.id).HasName("bloodsamples_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("nextval('bloodsamples_id_seq'::regclass)");
            entity.Property(e => e.barcode).HasMaxLength(100);
            entity.Property(e => e.sample_code).HasMaxLength(50);
            entity.Property(e => e.status).HasMaxLength(20);
        });

        modelBuilder.Entity<flagging_config_log>(entity =>
        {
            entity.HasKey(e => e.id).HasName("flagging_config_log_pkey");

            entity.ToTable("flagging_config_log");

            entity.Property(e => e.action).HasMaxLength(50);
            entity.Property(e => e.source).HasMaxLength(100);

            entity.HasOne(d => d.flag_config).WithMany(p => p.flagging_config_logs)
                .HasForeignKey(d => d.flag_config_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_flagging_log_config");
        });

        modelBuilder.Entity<flagging_configuration>(entity =>
        {
            entity.HasKey(e => e.id).HasName("flagging_configurations_pkey");

            entity.Property(e => e.analyte_code).HasMaxLength(50);
            entity.Property(e => e.analyte_name).HasMaxLength(100);
            entity.Property(e => e.flag_type).HasMaxLength(20);
            entity.Property(e => e.unit).HasMaxLength(20);
            entity.Property(e => e.updated_by).HasMaxLength(100);
            entity.Property(e => e.version).HasMaxLength(20);
        });

        modelBuilder.Entity<result_comment>(entity =>
        {
            entity.HasKey(e => e.id).HasName("result_comments_pkey");

            entity.Property(e => e.commented_by).HasMaxLength(100);

            entity.HasOne(d => d.result).WithMany(p => p.result_comments)
                .HasForeignKey(d => d.result_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_result_comments_result");

            entity.HasOne(d => d.sample).WithMany(p => p.result_comments)
                .HasForeignKey(d => d.sample_id)
                .HasConstraintName("fk_result_comments_sample");
        });

        modelBuilder.Entity<test_parameter>(entity =>
        {
            entity.HasKey(e => e.id).HasName("test_parameters_pkey");

            entity.Property(e => e.abbreviation).HasMaxLength(20);
            entity.Property(e => e.female_range_max).HasPrecision(10, 2);
            entity.Property(e => e.female_range_min).HasPrecision(10, 2);
            entity.Property(e => e.flag).HasMaxLength(50);
            entity.Property(e => e.male_range_max).HasPrecision(10, 2);
            entity.Property(e => e.male_range_min).HasPrecision(10, 2);
            entity.Property(e => e.normal_range_max).HasPrecision(10, 2);
            entity.Property(e => e.normal_range_min).HasPrecision(10, 2);
            entity.Property(e => e.normal_unit).HasMaxLength(50);
            entity.Property(e => e.param_name).HasMaxLength(100);
            entity.Property(e => e.reagent_code).HasMaxLength(50);
            entity.Property(e => e.reagent_lot).HasMaxLength(50);
            entity.Property(e => e.reagent_ref_id).HasMaxLength(64);
            entity.Property(e => e.status).HasMaxLength(20);

            entity.HasOne(d => d.flag_config).WithMany(p => p.test_parameters)
                .HasForeignKey(d => d.flag_config_id)
                .HasConstraintName("fk_test_parameters_flag_config");

            entity.HasOne(d => d.test_result).WithMany(p => p.test_parameters)
                .HasForeignKey(d => d.test_result_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_test_parameters_result");
        });

        modelBuilder.Entity<test_result>(entity =>
        {
            entity.HasKey(e => e.id).HasName("test_results_pkey");

            entity.Property(e => e.ai_reviewed_by).HasMaxLength(100);
            entity.Property(e => e.flag).HasMaxLength(50);
            entity.Property(e => e.reagent_code).HasMaxLength(50);
            entity.Property(e => e.reagent_lot).HasMaxLength(50);
            entity.Property(e => e.reagent_ref_id).HasMaxLength(64);
            entity.Property(e => e.reference_range).HasMaxLength(100);
            entity.Property(e => e.result_value).HasMaxLength(100);
            entity.Property(e => e.reviewed_by).HasMaxLength(100);
            entity.Property(e => e.status).HasMaxLength(20);
            entity.Property(e => e.test_code).HasMaxLength(50);
            entity.Property(e => e.test_name).HasMaxLength(255);
            entity.Property(e => e.unit).HasMaxLength(50);

            entity.HasOne(d => d.flag_config).WithMany(p => p.test_results)
                .HasForeignKey(d => d.flag_config_id)
                .HasConstraintName("fk_test_results_flag_config");

            entity.HasOne(d => d.sample).WithMany(p => p.test_results)
                .HasForeignKey(d => d.sample_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_test_results_sample");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
