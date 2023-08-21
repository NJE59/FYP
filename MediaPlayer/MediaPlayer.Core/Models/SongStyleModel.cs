// <copyright file="SongStyleModel.cs" company="Nathan Errington">
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
    public class SongStyleModel
    {
        // Primary Key Backing Fields
        private int trackGenreID;

        // Foreign Key Backing Fields
        private int trackID;
        private int genreID;

        // Primary Key Backed Properties

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.trackGenreID))]
        [Key]
        public int TrackGenreID
        {
            get => this.trackGenreID;
            set => this.trackGenreID = value;
        }

        // Foreign Key Backed Properties

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

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.genreID))]
        [ForeignKey(nameof(GenreModel))]
        public int GenreID
        {
            get => this.genreID;
            set => this.genreID = value;
        }

        // Navigation Properties

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public virtual TrackModel Track { get; set; } = null!;

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public virtual GenreModel TrackGenre { get; set; } = null!;
    }
}