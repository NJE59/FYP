using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Core.Models
{
    public class TrackModel
    {
        [Key]
        public int TrackID { get; set; }
        public string Path { get; set; } = null!;
        public string TrackName { get; set; } = null!;
        public int TrackNum { get; set; }
        public TimeSpan TrackLength { get; set; }
        public string? Description { get; set; }
        public string? Lyrics { get; set; }
        public int DiscID { get; set; }
        public virtual DiscModel Disc { get; set; } = null!;
        public virtual ICollection<ContributionModel> Contributions { get; private set; } = new ObservableCollection<ContributionModel>();
        public virtual ICollection<SongStyleModel> SongStyles { get; private set; } = new ObservableCollection<SongStyleModel>();
        public virtual ICollection<ListingModel> Listings { get; private set; } = new ObservableCollection<ListingModel>();
    }
}
