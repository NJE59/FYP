// <copyright file="PlaylistModel.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Models
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Model describing a custom music playlist.
    /// </summary>
    public class PlaylistModel
    {
        // Primary Key Backing Fields
        private int playlistID;

        // Other Backing Fields
        private string playlistName = null!;
        private string? description;

        // Primary Key Backed Properties

        /// <summary>
        /// Gets or sets the unique ID of this playlist.
        /// </summary>
        [BackingField(nameof(this.playlistID))]
        [Key]
        public int PlaylistID
        {
            get => this.playlistID;
            set => this.playlistID = value;
        }

        // Other Backed Properties

        /// <summary>
        /// Gets or sets the name of this playlist.
        /// </summary>
        [BackingField(nameof(this.playlistName))]
        public string PlaylistName
        {
            get => this.playlistName;
            set => this.playlistName = value;
        }

        /// <summary>
        /// Gets or sets the description of this playlist.
        /// </summary>
        [BackingField(nameof(this.description))]
        public string? Description
        {
            get => this.description;
            set => this.description = value;
        }

        // Navigation Properties

        /// <summary>
        /// Gets an <see cref="ObservableCollection{T}"/> of the listings of songs in this playlist.
        /// </summary>
        public virtual ICollection<ListingModel> Listings { get; private set; } = new ObservableCollection<ListingModel>();

        // NotMapped Properties

        /// <summary>
        /// Gets an <see cref="ObservableCollection{T}"/> of the songs in this playlist.
        /// </summary>
        [NotMapped]
        public ObservableCollection<TrackModel> Tracks => new (this.Listings.Select(listing => listing.Track));
    }
}