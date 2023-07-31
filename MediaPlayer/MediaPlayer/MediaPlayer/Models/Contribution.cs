using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Models
{
    public class Contribution
    {
        [Key]
        public int ContributionID { get; set; }
        public int ArtistID { get; set; }
        public virtual Artist Artist { get; set; } = null!;
        public int TrackID { get; set; }
        public virtual Track Track { get; set; } = null!;
    }
}