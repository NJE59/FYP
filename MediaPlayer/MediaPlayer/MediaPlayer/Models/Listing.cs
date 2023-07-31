using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Models
{
    public class Listing
    {
        [Key]
        public int ListingID { get; set; }
        public int TrackPos { get; set; }
        public int TrackID { get; set; }
        public Track Track { get; set; } = null!;
        public int PlaylistID { get; set; }
        public Playlist Playlist { get; set; } = null!;
    }
}