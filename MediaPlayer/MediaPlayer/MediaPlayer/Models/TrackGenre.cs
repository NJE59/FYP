using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Models
{
    public class TrackGenre
    {
        [Key]
        public int TrackGenreID { get; set; }
        public int TrackID { get; set; }
        public Track Track { get; set; } = null!;
        public int GenreID { get; set; }
        public Genre Genre { get; set; } = null!;
    }
}