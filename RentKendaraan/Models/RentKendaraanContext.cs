using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RentKendaraan.Models
{
    public partial class RentKendaraanContext : DbContext
    {
        public RentKendaraanContext()
        {
        }

        public RentKendaraanContext(DbContextOptions<RentKendaraanContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Jaminan> Jaminans { get; set; }
        public virtual DbSet<JenisKendaraan> JenisKendaraans { get; set; }
        public virtual DbSet<Kendaraan> Kendaraans { get; set; }
        public virtual DbSet<KondisiKendaraan> KondisiKendaraans { get; set; }
        public virtual DbSet<Peminjaman> Peminjamen { get; set; }
        public virtual DbSet<Pengembalian> Pengembalians { get; set; }

 /*       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-AUR1I1DH;Database=RentKendaraan;Trusted_Connection=True;");
            }
        }*/
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.IdCostumer);

                entity.ToTable("Customer");

                entity.Property(e => e.IdCostumer)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Customer");

                entity.Property(e => e.Alamat)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.IdGender).HasColumnName("ID_Gender");

                entity.Property(e => e.NamaCustomer)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Nama_Customer");

                entity.Property(e => e.Nik)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("NIK");

                entity.Property(e => e.NoHp)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("NO_HP");

                entity.HasOne(d => d.IdGenderNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.IdGender)
                    .HasConstraintName("FK_Customer_Gender");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasKey(e => e.IdGender);

                entity.ToTable("Gender");

                entity.Property(e => e.IdGender)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Gender");

                entity.Property(e => e.NamaGender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("Nama_Gender");
            });

            modelBuilder.Entity<Jaminan>(entity =>
            {
                entity.HasKey(e => e.IdJaminan);

                entity.ToTable("Jaminan");

                entity.Property(e => e.IdJaminan)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Jaminan");

                entity.Property(e => e.NamaJaminan)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("Nama_Jaminan");
            });

            modelBuilder.Entity<JenisKendaraan>(entity =>
            {
                entity.HasKey(e => e.IdJenisKendaraan);

                entity.ToTable("Jenis_Kendaraan");

                entity.Property(e => e.IdJenisKendaraan)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Jenis_Kendaraan");

                entity.Property(e => e.NamaJenisKendaraan)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Nama_Jenis_Kendaraan");
            });

            modelBuilder.Entity<Kendaraan>(entity =>
            {
                entity.HasKey(e => e.IdKendaraan);

                entity.ToTable("Kendaraan");

                entity.Property(e => e.IdKendaraan)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Kendaraan");

                entity.Property(e => e.IdJenisKendaraan).HasColumnName("ID_Jenis_Kendaraan");

                entity.Property(e => e.Ketersediaan)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.NamaKendaraan)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("Nama_Kendaraan");

                entity.Property(e => e.NoPolisi)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("No_Polisi");

                entity.Property(e => e.NoStnk)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("No_STNK");

                entity.HasOne(d => d.IdJenisKendaraanNavigation)
                    .WithMany(p => p.Kendaraans)
                    .HasForeignKey(d => d.IdJenisKendaraan)
                    .HasConstraintName("FK_Kendaraan_Jenis_Kendaraan");
            });

            modelBuilder.Entity<KondisiKendaraan>(entity =>
            {
                entity.HasKey(e => e.IdKondisi);

                entity.ToTable("Kondisi_Kendaraan");

                entity.Property(e => e.IdKondisi)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Kondisi");

                entity.Property(e => e.NamaKondisi)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Nama_Kondisi");
            });

            modelBuilder.Entity<Peminjaman>(entity =>
            {
                entity.HasKey(e => e.IdPeminjaman);

                entity.ToTable("Peminjaman");

                entity.Property(e => e.IdPeminjaman)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Peminjaman");

                entity.Property(e => e.IdCostumer).HasColumnName("ID_Costumer");

                entity.Property(e => e.IdJaminan).HasColumnName("ID_Jaminan");

                entity.Property(e => e.IdKendaraan).HasColumnName("ID_Kendaraan");

                entity.Property(e => e.TglPeminjaman)
                    .HasColumnType("datetime")
                    .HasColumnName("Tgl_Peminjaman");

                entity.HasOne(d => d.IdCostumerNavigation)
                    .WithMany(p => p.Peminjamen)
                    .HasForeignKey(d => d.IdCostumer)
                    .HasConstraintName("FK_Peminjaman_Customer");

                entity.HasOne(d => d.IdJaminanNavigation)
                    .WithMany(p => p.Peminjamen)
                    .HasForeignKey(d => d.IdJaminan)
                    .HasConstraintName("FK_Peminjaman_Jaminan");

                entity.HasOne(d => d.IdKendaraanNavigation)
                    .WithMany(p => p.Peminjamen)
                    .HasForeignKey(d => d.IdKendaraan)
                    .HasConstraintName("FK_Peminjaman_Kendaraan");
            });

            modelBuilder.Entity<Pengembalian>(entity =>
            {
                entity.HasKey(e => e.IdPengembalian);

                entity.ToTable("Pengembalian");

                entity.Property(e => e.IdPengembalian)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Pengembalian");

                entity.Property(e => e.IdKondisi).HasColumnName("ID_Kondisi");

                entity.Property(e => e.IdPeminjaman).HasColumnName("ID_Peminjaman");

                entity.Property(e => e.TglPengembalian)
                    .HasColumnType("datetime")
                    .HasColumnName("Tgl_Pengembalian");

                entity.HasOne(d => d.IdKondisiNavigation)
                    .WithMany(p => p.Pengembalians)
                    .HasForeignKey(d => d.IdKondisi)
                    .HasConstraintName("FK_Pengembalian_Kondisi_Kendaraan");

                entity.HasOne(d => d.IdPeminjamanNavigation)
                    .WithMany(p => p.Pengembalians)
                    .HasForeignKey(d => d.IdPeminjaman)
                    .HasConstraintName("FK_Pengembalian_Peminjaman");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
