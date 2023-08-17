using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Core.Models
{
    public class DiscModel
    {
        [Key]
        public int DiscID { get; set; }
        
        public int DiscNum { get; set; }
        public string? DiscName { get; set; }
        public int AlbumID { get; set; }
        public virtual AlbumModel Album { get; set; } = null!;
        public virtual ICollection<TrackModel> Tracks { get; private set; } = new ObservableCollection<TrackModel>();
    }
}