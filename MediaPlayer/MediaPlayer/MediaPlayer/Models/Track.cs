using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Models
{
    public class Track
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
        public virtual Disc Disc { get; set; } = null!;
        public virtual ICollection<Contribution> Contributions { get; private set; } = new ObservableCollection<Contribution>();
        public virtual ICollection<SongStyle> SongStyles { get; private set; } = new ObservableCollection<SongStyle>();
        public virtual ICollection<Listing> Listings { get; private set; } = new ObservableCollection<Listing>();
    }
}
