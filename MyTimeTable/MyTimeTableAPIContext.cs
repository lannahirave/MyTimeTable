using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MyTimeTable.Models;

namespace MyTimeTable;

public partial class MyTimeTableContext : DbContext
{
    public MyTimeTableContext()
    {
    }

    public MyTimeTableContext(DbContextOptions<MyTimeTableContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Control> Controls { get; set; }
    public virtual DbSet<Faculty> Faculties { get; set; }
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<Lector> Lectors { get; set; }
    public virtual DbSet<Organization> Organizations { get; set; }
    public virtual DbSet<OrganizationsLectors> OrganizationsLectors { get; set; }
    public virtual DbSet<Subject> Subjects { get; set; }
    public virtual DbSet<TimeTable> TimeTables { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Control>(entity =>
        {
            entity.Property(e => e.Id);
            entity.Property(e => e.Type)
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.Property(e => e.Id);
            entity.Property(e => e.Name)
                .HasMaxLength(255);
            entity.Property(e => e.Type)
                .HasMaxLength(255);
            entity.Property(e => e.Hours);

            entity.HasOne(e => e.Control)
                .WithMany(p => p.Subjects)
                .HasForeignKey(d => d.ControlId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.TimeTables)
                .WithOne(e => e.Subject)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Lector>(entity =>
        {
            entity.Property(e => e.Id);
            entity.Property(e => e.FullName)
                .HasMaxLength(255);
            entity.Property(e => e.Phone);
            entity.Property(e => e.Degree)
                .HasMaxLength(255);

            entity.HasMany(d => d.Organizations)
                .WithMany(p => p.Lectors)
                .UsingEntity<OrganizationsLectors>(
                    configureRight => configureRight
                        .HasOne(d => d.Organization)
                        .WithMany()
                        .HasForeignKey(d => d.OrganizationId)
                        .OnDelete(DeleteBehavior.Cascade),
                    configureRight => configureRight
                        .HasOne(d => d.Lector)
                        .WithMany()
                        .HasForeignKey(d => d.LectorId)
                        .OnDelete(DeleteBehavior.Cascade),
                    builder => builder
                        .ToTable("OrganizationsLectors")
                        .Property(x => x.Id)
                );

            entity.HasMany(d => d.TimeTables)
                .WithOne(e => e.Lector)
                .HasForeignKey(d => d.LectorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Id);
                entity.Property(e => e.Name)
                    .HasMaxLength(255);
                entity.Property(e => e.Course);
                entity.Property(e => e.Quantity);

                entity.HasOne(e => e.Faculty)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.FacultyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.TimeTables)
                    .WithOne(e => e.Group)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        );

        modelBuilder.Entity<Organization>(entity =>
            {
                entity.Property(e => e.Id);
                entity.Property(e => e.Name)
                    .HasMaxLength(255);
            }
        );
        modelBuilder.Entity<TimeTable>(entity =>
            {
                entity.Property(e => e.Id);
                entity.Property(e => e.Day)
                    .HasMaxLength(255);
                entity.Property(e => e.Auditory);
                entity.Property(e => e.Lection);
            }
        );
    }
}