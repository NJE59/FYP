using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Models
{
    public class Album
    {
        [Key]
        public int AlbumID { get; set; }
        //public int AlbPicID { get; set; }
        public int ReleaseYear { get; set; }
        public string AlbumName { get; set; } = null!;
        public string? Description { get; set; }
        public int ArtistID { get; set; }
        public Artist Artist { get; set; } = null!;
        public ICollection<Disc> Discs { get; set; } = null!;
    }
}
