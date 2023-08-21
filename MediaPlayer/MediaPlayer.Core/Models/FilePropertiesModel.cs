// <copyright file="FilePropertiesModel.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Models
{
    using Windows.Storage.FileProperties;

    /// <summary>
    /// Class used for handling properties of music files not handled by the <see cref="MusicProperties"/> class.
    /// </summary>
    public class FilePropertiesModel
    {
        // Backing Fields
        private int discNum;

        private string discName = null!;
        private string path = null!;

        private List<string> artists = null!;
        private MusicProperties musicProps = null!;

        // Backed Properties

        /// <summary>
        /// Gets or sets what number disc in the album the song is on; saved in files as "System.Music.PartOfSet".
        /// </summary>
        public int DiscNum
        {
            get => this.discNum;
            set => this.discNum = value;
        }

        /// <summary>
        /// Gets or sets the name of the disc; saved in file as "System.Music.GroupDescription".
        /// </summary>
        public string DiscName
        {
            get => this.discName;
            set => this.discName = value;
        }

        /// <summary>
        /// Gets or sets the full path to the file.
        /// </summary>
        public string Path
        {
            get => this.path;
            set => this.path = value;
        }

        /// <summary>
        /// Gets or sets a <see cref="List{T}"/> of contributing artists; saved in files as "System.Music.Artist".
        /// </summary>
        public List<string> Artists
        {
            get => this.artists;
            set => this.artists = value;
        }

        /// <summary>
        /// Gets or sets the standard music file properties (album, title etc).
        /// </summary>
        public MusicProperties MusicProps
        {
            get => this.musicProps;
            set => this.musicProps = value;
        }
    }
}
