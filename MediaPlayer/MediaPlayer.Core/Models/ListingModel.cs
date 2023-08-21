// <copyright file="ListingModel.cs" company="Nathan Errington">
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
    public class ListingModel
    {
        // Primary Key Backing Fields
        private int listingID;

        // Foreign Key Backing Fields
        private int playlistID;
        private int trackID;

        // Other Backing Fields
        private int trackPos;

        // Primary Key Backed Properties

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.listingID))]
        [Key]
        public int ListingID
        {
            get => this.listingID;
            set => this.listingID = value;
        }

        // Foreign Key Backed Properties

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.playlistID))]
        [ForeignKey(nameof(PlaylistModel))]
        public int PlaylistID
        {
            get => this.playlistID;
            set => this.playlistID = value;
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

        // Other Backed Properties

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.trackPos))]
        public int TrackPos
        {
            get => this.trackPos;
            set => this.trackPos = value;
        }

        // Navigation Properties

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public virtual TrackModel Track { get; set; } = null!;

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public virtual PlaylistModel Playlist { get; set; } = null!;
    }
}