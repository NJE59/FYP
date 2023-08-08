using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Models
{
    public class SongStyle
    {
        [Key]
        public int TrackGenreID { get; set; }
        public int TrackID { get; set; }
        public virtual Track Track { get; set; } = null!;
        public int GenreID { get; set; }
        public virtual Genre TrackGenre { get; set; } = null!;
    }
}