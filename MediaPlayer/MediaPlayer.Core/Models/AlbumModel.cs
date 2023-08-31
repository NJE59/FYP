// <copyright file="AlbumModel.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Models
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Model describing a music album.
    /// </summary>
    public class AlbumModel
    {
        // Primary Key Backing Fields
        private int albumID;

        // Foreign Key Backing Fields
        private int artistID;

        // Other Backing Fields
        private int releaseYear;
        private string albumName = null!;
        private string? description;

        // Primary Key Backed Properties

        /// <summary>
        /// Gets or sets the unique ID of this album.
        /// </summary>
        [BackingField(nameof(this.albumID))]
        [Key]
        public int AlbumID
        {
            get => this.albumID;
            set => this.albumID = value;
        }

        // Foreign Key Backed Properties

        /// <summary>
        /// Gets or sets the unique ID of the <see cref="ArtistModel"/> who made this album.
        /// </summary>
        [BackingField(nameof(this.artistID))]
        [ForeignKey(nameof(ArtistModel))]
        public int ArtistID
        {
            get => this.artistID;
            set => this.artistID = value;
        }

        // Other Backed Properties

        /// <summary>
        /// Gets or sets year this album released.
        /// </summary>
        [BackingField(nameof(this.releaseYear))]
        public int ReleaseYear
        {
            get => this.releaseYear;
            set => this.releaseYear = value;
        }

        /// <summary>
        /// Gets or sets name of this album.
        /// </summary>
        [BackingField(nameof(this.albumName))]
        public string AlbumName
        {
            get => this.albumName;
            set => this.albumName = value;
        }

        /// <summary>
        /// Gets or sets this album's description.
        /// </summary>
        [BackingField(nameof(this.description))]
        public string? Description
        {
            get => this.description;
            set => this.description = value;
        }

        // Navigation Properties

        /// <summary>
        /// Gets or sets the <see cref="ArtistModel"/> who made this album.
        /// </summary>
        public virtual ArtistModel Artist { get; set; } = null!;

        /// <summary>
        /// Gets the discs in this album.
        /// </summary>
        public virtual ICollection<DiscModel> Discs { get; private set; } = new ObservableCollection<DiscModel>();

        // NotMapped Properties

        /// <summary>
        /// Gets a value indicating whether this album has more than one disc.
        /// </summary>
        [NotMapped]
        public bool IsMultiDisc => this.Discs.Count > 1;
    }
}
