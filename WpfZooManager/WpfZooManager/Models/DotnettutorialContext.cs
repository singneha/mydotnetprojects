using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WpfZooManager.Models;

public partial class DotnettutorialContext : DbContext
{
    public virtual DbSet<Zoo> Zoos { get; set; }
    public virtual DbSet<Animal> Animals { get; set; }

    public virtual DbSet<ZooAnimalMap> ZooAnimalMap { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;Database=dotnettutorial;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Zoo>(entity =>
        {
            entity.ToTable("Zoo");
        });
        
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.ToTable("Animal");
        });
        modelBuilder.Entity<ZooAnimalMap>(entity =>
        {
            entity.ToTable("ZooAnimal");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
