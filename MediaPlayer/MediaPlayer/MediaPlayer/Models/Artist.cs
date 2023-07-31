using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Models
{
    public class Artist
    {
        [Key] 
        public int ArtistID { get; set; }
        public string ArtistName { get; set; } = null!;
        public string? Biography { get; set; }
        public virtual ICollection<Album> Albums { get; set; } = null!;
        public virtual ICollection<Contribution> Contributions { get; private set; } = new ObservableCollection<Contribution>();
    }
}
