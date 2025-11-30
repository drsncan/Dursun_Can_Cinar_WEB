using DmsWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DmsWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<AppUser> Users { get; set; } = null!;
        public DbSet<SystemSettings> SystemSettings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- AppUser config ---
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.Password)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.FullName)
                      .HasMaxLength(100);

                entity.Property(u => u.Role)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(u => u.IsActive)
                      .HasDefaultValue(true);
            });

            // --- SystemSettings config (opsiyonel ama dursun) ---
            modelBuilder.Entity<SystemSettings>(entity =>
            {
                entity.ToTable("SystemSettings");

                entity.HasKey(s => s.Id);

                entity.Property(s => s.SystemName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(s => s.InstitutionName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(s => s.Theme)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(s => s.LogoPath)
                      .HasMaxLength(260);

                entity.Property(s => s.AllowedExtensions)
                      .IsRequired()
                      .HasMaxLength(200);
            });

            // --- AppUser SEED ---
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1,
                    Username = "admin",
                    Password = "1234",
                    FullName = "Sistem Yöneticisi",
                    Role = "Admin",
                    IsActive = true
                },
                new AppUser
                {
                    Id = 2,
                    Username = "user",
                    Password = "1234",
                    FullName = "Standart Kullanıcı",
                    Role = "User",
                    IsActive = true
                }
            );

            // --- SystemSettings SEED ---
            modelBuilder.Entity<SystemSettings>().HasData(
                new SystemSettings
                {
                    Id = 1,
                    SystemName = "Döküman Yönetim Sistemi",
                    InstitutionName = "Ankara Üniversitesi",
                    Theme = "dark",
                    LogoPath = null,
                    MaxUploadSizeMb = 20,
                    AllowedExtensions = ".pdf,.docx,.xlsx,.pptx"
                }
            );
        }
    }
}
