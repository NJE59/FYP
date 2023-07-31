using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Models
{
    public class Contribution
    {
        [Key]
        public int ContributionID { get; set; }
        public int ArtistID { get; set; }
        public Artist Artist { get; set; } = null!;
        public int TrackID { get; set; }
        public Track Track { get; set; } = null!;
    }
}