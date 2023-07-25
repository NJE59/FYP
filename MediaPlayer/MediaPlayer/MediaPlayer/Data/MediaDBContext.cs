using MediaPlayer.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaPlayer.Data
{
    public class MediaDBContext
    {
        public DbSet<Track> Tracks { get; set; } = null!;
        public DbSet<Disc> Discs { get; set; } = null!;
        public DbSet<Album> Albums { get; set; } = null!;
        public DbSet<Artist> Artists { get; set; } = null!;
    }
}
