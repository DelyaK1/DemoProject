using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DemoProject
{
    public partial class agcc_rd_testContext : DbContext
    {
        public agcc_rd_testContext()
        {
        }

        public agcc_rd_testContext(DbContextOptions<agcc_rd_testContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TestAttribute> TestAttributes { get; set; }
        public virtual DbSet<TestByte> TestBytes { get; set; }
        public virtual DbSet<TestCheck> TestChecks { get; set; }
        public virtual DbSet<TestCheckResult> TestCheckResults { get; set; }
        public virtual DbSet<TestDocument> TestDocuments { get; set; }
        public virtual DbSet<TestFile> TestFiles { get; set; }
        public virtual DbSet<TestTransmittal> TestTransmittals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-41HG2P4P;Database=agcc_rd_test;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<TestAttribute>(entity =>
            {
                entity.ToTable("Test_Attributes");

                entity.Property(e => e.ClientRev).HasMaxLength(100);

                entity.Property(e => e.ContractorName).HasMaxLength(100);

                entity.Property(e => e.EngDescription).HasMaxLength(1000);

                entity.Property(e => e.FileName).HasMaxLength(1000);

                entity.Property(e => e.FooterName).HasMaxLength(100);

                entity.Property(e => e.Issue).HasMaxLength(100);

                entity.Property(e => e.PapeSize).HasMaxLength(100);

                entity.Property(e => e.PurposeIssue).HasMaxLength(100);

                entity.Property(e => e.Rev).HasMaxLength(100);

                entity.Property(e => e.RusDescription).HasMaxLength(1000);

                entity.Property(e => e.Scale).HasMaxLength(100);

                entity.Property(e => e.StageEn).HasMaxLength(100);

                entity.Property(e => e.StageRu).HasMaxLength(100);

                entity.Property(e => e.Status).HasMaxLength(100);
            });

            modelBuilder.Entity<TestByte>(entity =>
            {
                entity.ToTable("Test_Bytes");
            });

            modelBuilder.Entity<TestCheck>(entity =>
            {
                entity.ToTable("Test_Checks");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TestCheckResult>(entity =>
            {
                entity.ToTable("Test_CheckResults");

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<TestDocument>(entity =>
            {
                entity.ToTable("Test_Documents");

                entity.Property(e => e.DocumentName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Transmittal)
                    .WithMany(p => p.TestDocuments)
                    .HasForeignKey(d => d.TransmittalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Test_Documents_Test_Transmittals");
            });

            modelBuilder.Entity<TestFile>(entity =>
            {
                entity.ToTable("Test_Files");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Attributes)
                    .WithMany(p => p.TestFiles)
                    .HasForeignKey(d => d.AttributesId)
                    .HasConstraintName("FK_Test_Files_Test_Attributes");

                entity.HasOne(d => d.Byte)
                    .WithMany(p => p.TestFiles)
                    .HasForeignKey(d => d.ByteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Test_Files_Test_Bytes");

                entity.HasOne(d => d.CheckResults)
                    .WithMany(p => p.TestFiles)
                    .HasForeignKey(d => d.CheckResultsId)
                    .HasConstraintName("FK_Test_Files_Test_CheckResults");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.TestFiles)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Test_Files_Test_Documents");
            });

            modelBuilder.Entity<TestTransmittal>(entity =>
            {
                entity.ToTable("Test_Transmittals");

                entity.Property(e => e.Contractor).HasMaxLength(50);

                entity.Property(e => e.TransmittalName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UserUpload).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
