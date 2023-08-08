using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MediaPlayer.Models
{
    public class Playlist
    {
        public int PlaylistID { get; set; }
        public string PlaylistName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public virtual ICollection<Listing> Listings { get; private set; } = new ObservableCollection<Listing>();
    }
}