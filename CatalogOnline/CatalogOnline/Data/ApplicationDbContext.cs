using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CatalogOnline.Models.DBObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CatalogOnline.Data
{
    public partial class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Absence> Absences { get; set; } = null!;
        public virtual DbSet<Grade> Grades { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Teacher> Teachers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<Absence>(entity =>
            {
                entity.HasKey(e => e.IdAbsence)
                    .HasName("PK__Absence__D35072B29C9792E5");

                entity.ToTable("Absence");

                entity.Property(e => e.IdAbsence)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Absence");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Discipline).IsUnicode(false);

                entity.Property(e => e.IdStudent).HasColumnName("ID_Student");

                entity.Property(e => e.IdTeacher).HasColumnName("ID_Teacher");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => e.IdGrade)
                    .HasName("PK__Grade__91EA22C53C08D69F");

                entity.ToTable("Grade");

                entity.Property(e => e.IdGrade)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Grade");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Discipline).IsUnicode(false);

                entity.Property(e => e.Grade1).HasColumnName("Grade");

                entity.Property(e => e.IdStudent).HasColumnName("ID_Student");

                entity.Property(e => e.IdTeacher).HasColumnName("ID_Teacher");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.IdStudent)
                    .HasName("PK__tmp_ms_x__B9FFFA951732B36F");

                entity.ToTable("Student");

                entity.Property(e => e.IdStudent)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Student");

                entity.Property(e => e.Class).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsUnicode(false)
                    .HasColumnName("First_Name");

                entity.Property(e => e.LastName)
                    .IsUnicode(false)
                    .HasColumnName("Last_Name");

                entity.Property(e => e.PhoneNr)
                    .IsUnicode(false)
                    .HasColumnName("Phone_Nr");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.IdTeacher)
                    .HasName("PK__tmp_ms_x__99ED4E10CF9368D4");

                entity.ToTable("Teacher");

                entity.Property(e => e.IdTeacher)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Teacher");

                entity.Property(e => e.Discipline).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsUnicode(false)
                    .HasColumnName("First_Name");

                entity.Property(e => e.LastName)
                    .IsUnicode(false)
                    .HasColumnName("Last_Name");

                entity.Property(e => e.PhoneNr)
                    .IsUnicode(false)
                    .HasColumnName("Phone_Nr");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
