using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Core.Models
{
    public class ListingModel
    {
        [Key]
        public int ListingID { get; set; }
        public int TrackPos { get; set; }
        public int TrackID { get; set; }
        public virtual TrackModel Track { get; set; } = null!;
        public int PlaylistID { get; set; }
        public virtual PlaylistModel Playlist { get; set; } = null!;
    }
}