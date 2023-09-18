// <copyright file="DiscModel.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Models
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Model describing a single disc in a music album.
    /// </summary>
    public class DiscModel
    {
        // Primary Key Backing Fields
        private int discID;

        // Foreign Key Backing Fields
        private int albumID;

        // Other Backing Fields
        private int discNum;
        private string? discName;

        // Primary Key Backed Properties

        /// <summary>
        /// Gets or sets the unqiue ID of this disc.
        /// </summary>
        [BackingField(nameof(this.discID))]
        [Key]
        public int DiscID
        {
            get => this.discID;
            set => this.discID = value;
        }

        // Foreign Key Backed Properties

        /// <summary>
        /// Gets or sets the unqiue ID of the album this disc belongs to.
        /// </summary>
        [BackingField(nameof(this.albumID))]
        [ForeignKey(nameof(AlbumModel))]
        public int AlbumID
        {
            get => this.albumID;
            set => this.albumID = value;
        }

        // Other Backed Properties

        /// <summary>
        /// Gets or sets the number of this disc in its album.
        /// </summary>
        [BackingField(nameof(this.discNum))]
        public int DiscNum
        {
            get => this.discNum;
            set => this.discNum = value;
        }

        /// <summary>
        /// Gets or sets the name of this disc.
        /// </summary>
        [BackingField(nameof(this.discName))]
        public string? DiscName
        {
            get => this.discName;
            set => this.discName = value;
        }

        // Navigation Properties

        /// <summary>
        /// Gets or sets the album this disc belongs to.
        /// </summary>
        public virtual AlbumModel Album { get; set; } = null!;

        /// <summary>
        /// Gets an <see cref="ObservableCollection{T}"/> of the tracks belonging to the album.
        /// </summary>
        public virtual ICollection<TrackModel> Tracks { get; private set; } = new ObservableCollection<TrackModel>();

        // NotMapped Properties

        /// <summary>
        /// Gets the artist who made the album this disc belongs to.
        /// </summary>
        [NotMapped]
        public ArtistModel AlbumArtist => this.Album.Artist;

    }
}