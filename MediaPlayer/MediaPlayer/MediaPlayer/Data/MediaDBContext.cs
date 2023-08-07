using MediaPlayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MediaPlayer.Data
{
    public class MediaDBContext : DbContext
    {
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Disc> Discs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<SongStyle> SongStyles { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Listing> Listings { get; set; }

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
