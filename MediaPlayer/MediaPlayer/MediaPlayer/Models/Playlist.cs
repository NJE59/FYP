namespace MediaPlayer.Models
{
    public class Playlist
    {
        public int PlaylistID { get; set; }
        public string PlaylistName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}