// <copyright file="ContributionModel.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// PLACEHOLDER.
    /// </summary>
    public class ContributionModel
    {
        // Primary Key Backing Fields
        private int contributionID;

        // Foreign Key Backing Fields
        private int artistID;
        private int trackID;

        // Primary Key Backed Properties

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.contributionID))]
        [Key]
        public int ContributionID
        {
            get => this.contributionID;
            set => this.contributionID = value;
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

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.trackID))]
        [ForeignKey(nameof(TrackModel))]
        public int TrackID
        {
            get => this.trackID;
            set => this.trackID = value;
        }

        // Navigation Properties

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public virtual ArtistModel Contributor { get; set; } = null!;

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public virtual TrackModel Track { get; set; } = null!;
    }
}