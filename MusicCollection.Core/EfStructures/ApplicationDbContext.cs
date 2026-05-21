using Microsoft.EntityFrameworkCore;
using MusicCollection.Models.Entities;

namespace MusicCollection.Core.EfStructures;

public partial class ApplicationDbContext : DbContext
{
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<PhysicalDisc> Discs => Set<PhysicalDisc>();
    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<AlbumImage> Images => Set<AlbumImage>();


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Конфигурация Artist
        modelBuilder.Entity<Artist>(entity =>
        {
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.Name);
        });

        // Конфигурация Album
        modelBuilder.Entity<Album>(entity =>
        {
            entity.Property(e => e.Title).IsRequired().HasMaxLength(300);
            entity.Property(e => e.CatalogNumber).HasMaxLength(100);
            entity.Property(e => e.Label).HasMaxLength(200);
            entity.Property(e => e.Packaging).HasConversion<string>().HasMaxLength(50);
        });

        // Конфигурация 1-к-1: Альбом -> Обложка
        modelBuilder.Entity<AlbumImage>(entity =>
        {
            entity.Property(e => e.Data).IsRequired().HasColumnType("varbinary(max)");
            entity.HasOne(i => i.Album)
                  .WithOne(a => a.Image)
                  .HasForeignKey<AlbumImage>(i => i.AlbumId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Конфигурация Track
        modelBuilder.Entity<Track>(entity =>
        {
            entity.Property(e => e.Duration).HasColumnType("time");
        });
    }




}
