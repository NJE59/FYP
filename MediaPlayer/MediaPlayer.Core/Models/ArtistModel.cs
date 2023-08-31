// <copyright file="ArtistModel.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Models
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Model describing a music artist.
    /// </summary>
    public class ArtistModel
    {
        // Primary Key Backing Fields
        private int artistID;

        // Other Backing Fields
        private string artistName = null!;
        private string? biography;

        // Primary Key Backed Properties

        /// <summary>
        /// Gets or sets the unique ID of this artist.
        /// </summary>
        [BackingField(nameof(this.artistID))]
        [Key]
        public int ArtistID
        {
            get => this.artistID;
            set => this.artistID = value;
        }

        // Other Backed Properties

        /// <summary>
        /// Gets or sets the name of this artist.
        /// </summary>
        [BackingField(nameof(this.artistName))]
        public string ArtistName
        {
            get => this.artistName;
            set => this.artistName = value;
        }

        /// <summary>
        /// Gets or sets the biography of this artist.
        /// </summary>
        [BackingField(nameof(this.biography))]
        public string? Biography
        {
            get => this.biography;
            set => this.biography = value;
        }

        // Navigation Properties

        /// <summary>
        /// Gets the albums made by this artist.
        /// </summary>
        public virtual ICollection<AlbumModel> Albums { get; private set; } = new ObservableCollection<AlbumModel>();

        /// <summary>
        /// Gets the individual song contributions made by this artist.
        /// </summary>
        public virtual ICollection<ContributionModel> Contributions { get; private set; } = new ObservableCollection<ContributionModel>();
    }
}
