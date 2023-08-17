using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Core.Models
{
    public class GenreModel
    {
        [Key]
        public int GenreID { get; set; }
        public string GenreName { get; set; } = null!;
        public string? Description { get; set; }
        public virtual ICollection<SongStyleModel> TrackGenres { get; private set; } = new ObservableCollection<SongStyleModel>();
    }
}