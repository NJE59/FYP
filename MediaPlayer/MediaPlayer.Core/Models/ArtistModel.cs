using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Core.Models
{
    public class ArtistModel
    {
        [Key] 
        public int ArtistID { get; set; }
        public string ArtistName { get; set; } = null!;
        public string? Biography { get; set; }
        public virtual ICollection<AlbumModel> Albums { get; private set; } = new ObservableCollection<AlbumModel>();
        public virtual ICollection<ContributionModel> Contributions { get; private set; } = new ObservableCollection<ContributionModel>();
    }
}
