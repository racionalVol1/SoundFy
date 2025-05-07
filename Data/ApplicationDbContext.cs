using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoundFy.Models;

namespace SoundFy.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Album> Albuns { get; set; }
    public DbSet<Musica> Musicas { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
     public DbSet<PlaylistMusicas> PlaylistMusicas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PlaylistMusicas>()
            .HasKey(pm => pm.PlaylistMusicaId); 

        modelBuilder.Entity<PlaylistMusicas>()
            .HasOne(pm => pm.Playlist)
            .WithMany(p => p.PlaylistMusicas) 
            .HasForeignKey(pm => pm.PlaylistId);

        modelBuilder.Entity<PlaylistMusicas>()
            .HasOne(pm => pm.Musica)
            .WithMany(m => m.PlaylistMusicas)
            .HasForeignKey(pm => pm.MusicaId);
    }
}
