using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.Models
{
    public class Album
    {
        [Key]
        public int AlbumID { get; set; }
        public int ReleaseYear { get; set; }
        public string AlbumName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsMultiDisc { get; set; }
        public int ArtistID { get; set; }
        public virtual Artist Artist { get; set; } = null!;
        public virtual ICollection<Disc> Discs { get; private set; } = new ObservableCollection<Disc>();
    }
}
