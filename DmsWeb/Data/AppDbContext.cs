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

        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<DocumentFile> DocumentFiles { get; set; } = null!;
        public DbSet<ApprovalRequest> ApprovalRequests { get; set; } = null!;
        public DbSet<ApprovalAction> ApprovalActions { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;

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

            // --- Document config (opsiyonel: tablo adını netleştir) ---
            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("Documents");
                entity.HasKey(d => d.Id);
            });

            // --- SystemSettings config ---
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

            // =========================================================
            // ✅ YENİ TABLOLARIN CONFIG + İLİŞKİLER
            // =========================================================

            // --- Roles ---
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Name)
                      .IsRequired()
                      .HasMaxLength(20);

                // Aynı isimden 2 rol olmasın (Admin, User)
                entity.HasIndex(r => r.Name).IsUnique();
            });

            // --- DocumentFiles ---
            modelBuilder.Entity<DocumentFile>(entity =>
            {
                entity.ToTable("DocumentFiles");
                entity.HasKey(df => df.Id);

                entity.Property(df => df.StoredFileName)
                      .IsRequired()
                      .HasMaxLength(260);

                entity.Property(df => df.OriginalFileName)
                      .IsRequired()
                      .HasMaxLength(260);

                entity.Property(df => df.UploadedBy)
                      .IsRequired()
                      .HasMaxLength(100);

                // Document (1) -> (N) DocumentFiles
                entity.HasOne(df => df.Document)
                      .WithMany() // Document içine koleksiyon koymadığın için böyle
                      .HasForeignKey(df => df.DocumentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- ApprovalRequests ---
            modelBuilder.Entity<ApprovalRequest>(entity =>
            {
                entity.ToTable("ApprovalRequests");
                entity.HasKey(ar => ar.Id);

                entity.Property(ar => ar.RequestedBy)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(ar => ar.Status)
                      .IsRequired()
                      .HasMaxLength(20);

                // Document (1) -> (N) ApprovalRequests
                entity.HasOne(ar => ar.Document)
                      .WithMany()
                      .HasForeignKey(ar => ar.DocumentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- ApprovalActions ---
            modelBuilder.Entity<ApprovalAction>(entity =>
            {
                entity.ToTable("ApprovalActions");
                entity.HasKey(aa => aa.Id);

                entity.Property(aa => aa.Actor)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(aa => aa.Action)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(aa => aa.Comment)
                      .HasMaxLength(500);

                // ApprovalRequest (1) -> (N) Actions
                entity.HasOne(aa => aa.ApprovalRequest)
                      .WithMany(ar => ar.Actions)
                      .HasForeignKey(aa => aa.ApprovalRequestId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- AuditLogs ---
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("AuditLogs");
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Actor)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(a => a.Event)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(a => a.Detail)
                      .HasMaxLength(1000);
            });

            // =========================================================
            // ✅ SEED
            // =========================================================

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

            // --- Roles SEED (tablo sayısını güçlendirir) ---
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
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
