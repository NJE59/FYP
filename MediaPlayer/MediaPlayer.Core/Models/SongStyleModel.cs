using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Core.Models
{
    public class SongStyleModel
    {
        [Key]
        public int TrackGenreID { get; set; }
        public int TrackID { get; set; }
        public virtual TrackModel Track { get; set; } = null!;
        public int GenreID { get; set; }
        public virtual GenreModel TrackGenre { get; set; } = null!;
    }
}