using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Models
{
    public class Genre
    {
        [Key]
        public int GenreID { get; set; }
        public string GenreName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public virtual ICollection<TrackGenre> TrackGenres { get; private set; } = new ObservableCollection<TrackGenre>();
    }
}