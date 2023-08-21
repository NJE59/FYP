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
    /// PLACEHOLDER.
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
        /// Gets or sets PLACEHOLDER.
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
        /// Gets or sets PLACEHOLDER.
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
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.releaseYear))]
        public int ReleaseYear
        {
            get => this.releaseYear;
            set => this.releaseYear = value;
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.albumName))]
        public string AlbumName
        {
            get => this.albumName;
            set => this.albumName = value;
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

        // Navigation Properties

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public virtual ArtistModel Artist { get; set; } = null!;

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public virtual ICollection<DiscModel> Discs { get; private set; } = new ObservableCollection<DiscModel>();

        // NotMapped Properties

        /// <summary>
        /// Gets a value indicating whether PLACEHOLDER.
        /// </summary>
        [NotMapped]
        public bool IsMultiDisc => this.Discs.Count > 1;
    }
}
