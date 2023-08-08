using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;

namespace MediaPlayer.Classes
{
    /// <summary>
    /// Class used for handling properties of music files not handled by the <see cref="MusicProperties"/> class
    /// </summary>
    public class FileProperties
    {
        /// <summary>
        /// Full path to the file
        /// </summary>
        public string Path = null!;
        /// <summary>
        /// List of contributing artists; saved in files as <see cref="System.Music.Artist"/>
        /// </summary>
        public List<string> Artists = null!;
        /// <summary>
        /// What number disc in the album the song is on; saved in files as <see cref="System.Music.PartOfSet"/>
        /// </summary>
        public int DiscNum;
        /// <summary>
        /// Name of disc in album; saved in file as <see cref="System.Music.GroupDescription"/>
        /// </summary>
        public string DiscName = null!;
        /// <summary>
        /// Denotes whether or not album has multiple discs; saved in file as <see cref="System.Music.IsCompilation"/>
        /// </summary>
        public bool IsMultiDisc;
        /// <summary>
        /// Standard music file properties (album, title etc)
        /// </summary>
        public MusicProperties MusicProperties = null!;
    }
}
