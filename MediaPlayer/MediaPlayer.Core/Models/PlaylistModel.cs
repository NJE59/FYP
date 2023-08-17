using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Core.Models
{
    public class PlaylistModel
    {
        [Key]
        public int PlaylistID { get; set; }
        public string PlaylistName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public virtual ICollection<ListingModel> Listings { get; private set; } = new ObservableCollection<ListingModel>();
    }
}