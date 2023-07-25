using System;
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
        public Disc Disc { get; set; } = null!;
    }
}
