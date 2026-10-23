using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyCursS.Models
{
public partial class AviaIarnoContext : DbContext
{
    public AviaIarnoContext()
    {
     // Database.EnsureCreated();
    }

    public AviaIarnoContext(DbContextOptions<AviaIarnoContext> options)
        : base(options)
    {
       // Database.EnsureCreated();
    }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<FreePlace> FreePlaces { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }
    public virtual DbSet<Person> Persons { get; set; }
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Admin");
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(true);
                entity.Property(e => e.Password)
                    .HasMaxLength(2000)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.NumberFlight);

            entity.ToTable("Flight");

            entity.Property(e => e.Departure)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Destination)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Аirplane)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FreePlace>(entity =>
        {
            entity.HasKey(e => e.IdPlace).HasName("FreePlace_PK");

            entity.ToTable("FreePlace");

            entity.Property(e => e.FreePlace1).HasColumnName("FreePlace");

            entity.HasOne(d => d.NumberFlightNavigation).WithMany(p => p.FreePlaces)
                .HasForeignKey(d => d.NumberFlight)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FreePlace_Flight_FK");
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(e => e.IdPassenger).HasName("Passenger_PK");

            entity.ToTable("Passenger");

            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SurName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.NumberFlightNavigation).WithMany(p => p.Passengers)
                .HasForeignKey(d => d.NumberFlight)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Passenger_Flight_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
}