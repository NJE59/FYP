using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayerDemo.MediaClass
{
    public class Track
    {
        public string Title { get; set; }
        public int SongNo { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }

        public Track(string album, int songNo, string title, string artist) {
            Album = album;
            SongNo = songNo;
            Title = title;
            Artist = artist;
        }
    }

    public class Album
    {
        public string Name { get; set; }

        public Album(string name)
        {
            Name = name;
        }
       
    }

    public class Artist
    {
        public string Name { get; set; }

        public Artist(string name)
        {
            Name = name;
        }

    }

    public class Playlist
    {
        public string Name { get; set; }
        public Playlist(string name)
        {
            Name = name;
        }

    }
}
