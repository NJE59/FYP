using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Models
{
    public class Disc
    {
        [Key]
        public int DiscID { get; set; }
        
        public int DiscNum { get; set; }
        public string? DiscName { get; set; }
        public int AlbumID { get; set; }
        public Album Album { get; set; } = null!;
        public virtual ICollection<Track> Tracks { get; private set; } = new ObservableCollection<Track>();
    }
}