// <copyright file="TrackModel.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Models
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using MediaPlayer.Core.Classes;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Model descring a sing music track / song.
    /// </summary>
    public class TrackModel
    {
        // Primary Key Backing Fields
        private int trackID;
        private string path = null!;

        // Foreign Key Backing Fields
        private int discID;

        // Other Backing Fields
        private int trackNum;
        private string trackName = null!;
        private string? description;
        private string? lyrics;
        private TimeSpan trackDuration;

        // Primary Key Backed Properties

        /// <summary>
        /// Gets or sets the unique ID of this song.
        /// </summary>
        [BackingField(nameof(this.trackID))]
        [Key]
        public int TrackID
        {
            get => this.trackID;
            set => this.trackID = value;
        }

        /// <summary>
        /// Gets or sets the file path of this song.
        /// </summary>
        [BackingField(nameof(this.path))]
        public string Path
        {
            get => this.path;
            set => this.path = value;
        }

        // Foreign Key Backed Properties

        /// <summary>
        /// Gets or sets the unique ID of the disc this song belongs to.
        /// </summary>
        [BackingField(nameof(this.discID))]
        [ForeignKey(nameof(DiscModel))]
        public int DiscID
        {
            get => this.discID;
            set => this.discID = value;
        }

        // Other Backed Properties

        /// <summary>
        /// Gets or sets the number of this song in its disc.
        /// </summary>
        [BackingField(nameof(this.trackNum))]
        public int TrackNum
        {
            get => this.trackNum;
            set => this.trackNum = value;
        }

        /// <summary>
        /// Gets or sets the name of this song.
        /// </summary>
        [BackingField(nameof(this.trackName))]
        public string TrackName
        {
            get => this.trackName;
            set => this.trackName = value;
        }

        /// <summary>
        /// Gets or sets the description of this song.
        /// </summary>
        [BackingField(nameof(this.description))]
        public string? Description
        {
            get => this.description;
            set => this.description = value;
        }

        /// <summary>
        /// Gets or sets the lyrics of this song.
        /// </summary>
        [BackingField(nameof(this.lyrics))]
        public string? Lyrics
        {
            get => this.lyrics;
            set => this.lyrics = value;
        }

        /// <summary>
        /// Gets or sets the duration of this song.
        /// </summary>
        [BackingField(nameof(this.trackDuration))]
        public TimeSpan TrackDuration
        {
            get => this.trackDuration;
            set => this.trackDuration = value;
        }

        // Navigation Properties

        /// <summary>
        /// Gets or sets the disc this song belongs to.
        /// </summary>
        public virtual DiscModel Disc { get; set; } = null!;

        /// <summary>
        /// Gets an <see cref="ObservableCollection{T}"/> of the artist contributions to this song.
        /// </summary>
        public virtual ICollection<ContributionModel> Contributions { get; private set; } = new ObservableCollection<ContributionModel>();

        /// <summary>
        /// Gets an <see cref="ObservableCollection{T}"/> of the genre contributions to this song.
        /// </summary>
        public virtual ICollection<SongStyleModel> SongStyles { get; private set; } = new ObservableCollection<SongStyleModel>();

        /// <summary>
        /// Gets an <see cref="ObservableCollection{T}"/> of this song's listings in playlists.
        /// </summary>
        public virtual ICollection<ListingModel> Listings { get; private set; } = new ObservableCollection<ListingModel>();

        // NotMapped Properties

        /// <summary>
        /// Gets a value indicating whether the song is longer than an hour, for the purpose of formatting the <see cref="DisplayDuration"/>.
        /// </summary>
        [NotMapped]
        public bool IsLongerThanHour => this.TrackDuration > TimeSpan.FromHours(1);

        /// <summary>
        /// Gets the <see cref="TrackDuration"/> in integer form representing how many seconds long this song is.
        /// </summary>
        [NotMapped]
        public int ControlDuration => (int)this.TrackDuration.TotalSeconds;

        /// <summary>
        /// Gets the format for this song's <see cref="DisplayDuration"/> - ie whether or not to include the hour component.
        /// </summary>
        [NotMapped]
        public string DisplayDurationFormat => $"{((this.TrackDuration > TimeSpan.FromHours(1)) ? "hh\\:" : string.Empty)}mm\\:ss";

        /// <summary>
        /// Gets this song's <see cref="TrackDuration"/> as a string, formatted according to its <see cref="DisplayDurationFormat"/>.
        /// </summary>
        [NotMapped]
        public string DisplayDuration => this.TrackDuration.ToString(this.DisplayDurationFormat);

        /// <summary>
        /// Gets the album this song belongs sto.
        /// </summary>
        [NotMapped]
        public AlbumModel Album => this.Disc.Album;

        /// <summary>
        /// Gets the name of the album this song belongs sto.
        /// </summary>
        [NotMapped]
        public string AlbumName => this.Album.AlbumName;

        /// <summary>
        /// Gets the artist who made the album this song belongs sto.
        /// </summary>
        [NotMapped]
        public ArtistModel AlbumArtist => this.Disc.Album.Artist;

        /// <summary>
        /// Gets the file path of this song in Uri format for loadign into the media controller.
        /// </summary>
        [NotMapped]
        public Uri LoadPath => new (this.Path);

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        [NotMapped]
        public int DiscNum => this.Disc.DiscNum;

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        [NotMapped]
        public ObservableCollection<GenreModel> Genres => this.SongStyles.Select(songStyle => songStyle.TrackGenre).ToObservableCollection();
    }
}
