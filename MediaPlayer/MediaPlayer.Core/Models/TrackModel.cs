// <copyright file="TrackModel.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Models
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// PLACEHOLDER.
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
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.trackID))]
        [Key]
        public int TrackID
        {
            get => this.trackID;
            set => this.trackID = value;
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.path))]
        public string Path
        {
            get => this.path;
            set => this.path = value;
        }

        // Foreign Key Backed Properties

        /// <summary>
        /// Gets or sets PLACEHOLDER.
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
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.trackNum))]
        public int TrackNum
        {
            get => this.trackNum;
            set => this.trackNum = value;
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.trackName))]
        public string TrackName
        {
            get => this.trackName;
            set => this.trackName = value;
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.description))]
        public string? Description
        {
            get => this.description;
            set => this.description = value;
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.lyrics))]
        public string? Lyrics
        {
            get => this.lyrics;
            set => this.lyrics = value;
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.trackDuration))]
        public TimeSpan TrackDuration
        {
            get => this.trackDuration;
            set => this.trackDuration = value;
        }

        // Navigation Properties

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public virtual DiscModel Disc { get; set; } = null!;

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public virtual ICollection<ContributionModel> Contributions { get; private set; } = new ObservableCollection<ContributionModel>();

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public virtual ICollection<SongStyleModel> SongStyles { get; private set; } = new ObservableCollection<SongStyleModel>();

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public virtual ICollection<ListingModel> Listings { get; private set; } = new ObservableCollection<ListingModel>();

        // NotMapped Properties

        /// <summary>
        /// Gets a value indicating whether PLACEHOLDER.
        /// </summary>
        [NotMapped]
        public bool IsLongerThanHour => this.TrackDuration > TimeSpan.FromHours(1);

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        [NotMapped]
        public int ControlDuration => (int)this.TrackDuration.TotalSeconds;

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        [NotMapped]
        public string DisplayDurationFormat => $"{((this.TrackDuration > TimeSpan.FromHours(1)) ? "hh\\:" : string.Empty)}mm\\:ss";

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        [NotMapped]
        public string DisplayDuration => this.TrackDuration.ToString(this.DisplayDurationFormat);

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        [NotMapped]
        public AlbumModel Album => this.Disc.Album;

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        [NotMapped]
        public ArtistModel AlbumArtist => this.Disc.Album.Artist;

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        [NotMapped]
        public Uri LoadPath => new (this.Path);
    }
}
