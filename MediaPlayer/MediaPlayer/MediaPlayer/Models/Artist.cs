using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Models
{
    public class Artist
    {
        [Key] 
        public int ArtistID { get; set; }
        public string ArtistName { get; set; } = null!;
        public string? Biography { get; set; }
        public ICollection<Album> Albums { get; set; } = null!;
        public ICollection<Contribution> Contributions { get; set; } = null!;
    }
}
