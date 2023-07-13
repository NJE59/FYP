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
        public int TrackNo { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }

        public Track(string album, int trackNo, string title, string artist) {
            Album = album;
            TrackNo = trackNo;
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
