using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public AlbumModel Album => Disc.Album;
        [NotMapped]
        public ArtistModel AlbumArtist => Disc.Album.Artist;
        [NotMapped]
        public Uri LoadPath => new Uri(Path);
        [NotMapped]
        public bool IsLongerThanHour => TrackLength > TimeSpan.FromHours(1);
        [NotMapped]
        public string DisplayFormat => $"{((TrackLength > TimeSpan.FromHours(1)) ? "hh\\:" : "")}mm\\:ss";
        [NotMapped]
        public string DisplayLength => TrackLength.ToString(DisplayFormat);
    }
}
