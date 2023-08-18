using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Core.Models
{
    public class ContributionModel
    {
        [Key]
        public int ContributionID { get; set; }
        public int ArtistID { get; set; }
        public virtual ArtistModel Contributor { get; set; } = null!;
        public int TrackID { get; set; }
        public virtual TrackModel Track { get; set; } = null!;
    }
}