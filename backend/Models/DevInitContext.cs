using System;
using System.Collections.Generic;
using backend.Extensions;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;

public partial class DevInitContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserOAuth> UserOAuths { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<ServiceVersion> ServicesVersion { get; set; } = null!;
    
    public DevInitContext(DbContextOptions<DevInitContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);



        modelBuilder
            .Entity<Service>()
            .HasMany(s => s.ServiceVersions)
            .WithOne(v => v.Service)
            .HasForeignKey(v => v.ServiceId)
            .IsRequired();

        modelBuilder
            .Entity<User>()
            .HasMany(u => u.OAuths)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId);

        modelBuilder
            .Entity<UserOAuth>()
            .HasIndex(o => o.ProviderId)
            .IsUnique();

        var timeStampEntities = modelBuilder.Model.GetEntityTypes()
            .Where(e => typeof(TimeStamp).IsAssignableFrom(e.ClrType));

        var createdTimeStampEntities = modelBuilder.Model.GetEntityTypes()
            .Where(e => typeof(CreatedTimeStamp).IsAssignableFrom(e.ClrType));
            
        var idenityEntities = modelBuilder.Model.GetEntityTypes()
            .Where(e => typeof(Identity).IsAssignableFrom(e.ClrType));

        foreach (var entityType in idenityEntities)
        {
            
            modelBuilder
                .Entity(entityType.ClrType)
                .Property(nameof(Identity.Id))
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder
                .Entity(entityType.ClrType)
                .HasKey(nameof(Identity.Id));
        }

        foreach (var entityType in timeStampEntities)
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property(nameof(TimeStamp.CreatedAt))
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("now()");

            modelBuilder.Entity(entityType.ClrType)
                .Property(nameof(TimeStamp.UpdatedAt))
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("now()");
        }

        foreach (var entityType in createdTimeStampEntities)
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property(nameof(CreatedTimeStamp.CreatedAt))
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("now()");
        }
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
