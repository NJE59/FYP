// <copyright file="ArtistModel.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Models
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// PLACEHOLDER.
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
        /// Gets or sets PLACEHOLDER.
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
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.artistName))]
        public string ArtistName
        {
            get => this.artistName;
            set => this.artistName = value;
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.biography))]
        public string? Biography
        {
            get => this.biography;
            set => this.biography = value;
        }

        // Navigation Properties

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public virtual ICollection<AlbumModel> Albums { get; private set; } = new ObservableCollection<AlbumModel>();

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public virtual ICollection<ContributionModel> Contributions { get; private set; } = new ObservableCollection<ContributionModel>();
    }
}
