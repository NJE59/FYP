using MediaPlayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MediaPlayer.Data
{
    public class MediaDBContext : DbContext
    {
        public DbSet<Track> Tracks { get; set; } = null!;
        public DbSet<Disc> Discs { get; set; } = null!;
        public DbSet<Album> Albums { get; set; } = null!;
        public DbSet<Artist> Artists { get; set; } = null!;
        public DbSet<Contribution> Contributions { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; }
        public DbSet<TrackGenre> TrackGenres { get; set; } = null!;
        public DbSet<Playlist> Playlists { get; set; } = null!;
        public DbSet<Listing> Listings { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "FileName=MediaDB.db", option =>
                {
                    option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                });
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
