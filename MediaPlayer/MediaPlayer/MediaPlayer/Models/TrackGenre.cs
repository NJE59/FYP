using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Models
{
    public class TrackGenre
    {
        [Key]
        public int TrackGenreID { get; set; }
        public int TrackID { get; set; }
        public virtual Track Track { get; set; } = null!;
        public int GenreID { get; set; }
        public virtual Genre Genre { get; set; } = null!;
    }
}