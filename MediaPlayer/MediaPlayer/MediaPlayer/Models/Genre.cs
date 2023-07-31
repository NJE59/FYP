using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Models
{
    public class Genre
    {
        [Key]
        public int GenreID { get; set; }
        public string GenreName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ICollection<TrackGenre> TrackGenres { get; set; } = null!;
    }
}