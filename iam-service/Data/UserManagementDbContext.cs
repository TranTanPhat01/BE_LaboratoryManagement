
using iam_service.Modals;
using Microsoft.EntityFrameworkCore;

namespace iam_service.Data
{
    public partial class UserManagementDbContext : DbContext
    {
        public UserManagementDbContext()
        {
        }

        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<OtpCodes> OtpCodes { get; set; } = null!;

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Username)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Password)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.CreatedAt)
                      .HasColumnType("datetime");

                entity.Property(e => e.LastLoginAt)
                      .HasColumnType("datetime");

                entity.HasOne(d => d.Role)
                      .WithMany(p => p.Accounts)
                      .HasForeignKey(d => d.RoleId)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Description)
                      .HasMaxLength(500);
            });

            modelBuilder.Entity<OtpCodes>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.OtpCode)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(e => e.CreatedAt)
                      .HasColumnType("datetime");

                entity.Property(e => e.ExpiresAt)
                      .HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
