using MediaPlayer.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MediaPlayer.Core.Data
{
    public class MediaDBContext : DbContext
    {
        public DbSet<TrackModel> Tracks { get; set; }
        public DbSet<DiscModel> Discs { get; set; }
        public DbSet<AlbumModel> Albums { get; set; }
        public DbSet<ArtistModel> Artists { get; set; }
        public DbSet<ContributionModel> Contributions { get; set; }
        public DbSet<GenreModel> Genres { get; set; }
        public DbSet<SongStyleModel> SongStyles { get; set; }
        public DbSet<PlaylistModel> Playlists { get; set; }
        public DbSet<ListingModel> Listings { get; set; }

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
