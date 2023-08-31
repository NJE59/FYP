// <copyright file="SongStyleModel.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Model describing a genre's contribution to a song.
    /// </summary>
    public class SongStyleModel
    {
        // Primary Key Backing Fields
        private int songStyleID;

        // Foreign Key Backing Fields
        private int trackID;
        private int genreID;

        // Primary Key Backed Properties

        /// <summary>
        /// Gets or sets the unique ID of this SongStyle.
        /// </summary>
        [BackingField(nameof(this.songStyleID))]
        [Key]
        public int SongStyleID
        {
            get => this.songStyleID;
            set => this.songStyleID = value;
        }

        // Foreign Key Backed Properties

        /// <summary>
        /// Gets or sets the unique ID of the song this SongStyle is contributing to.
        /// </summary>
        [BackingField(nameof(this.trackID))]
        [ForeignKey(nameof(TrackModel))]
        public int TrackID
        {
            get => this.trackID;
            set => this.trackID = value;
        }

        /// <summary>
        /// Gets or sets the unique ID of the contributing genre to this SongStyle.
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
        /// Gets or sets the song this SongStyle is contributing to.
        /// </summary>
        public virtual TrackModel Track { get; set; } = null!;

        /// <summary>
        /// Gets or sets the contributing genre to this SongStyle.
        /// </summary>
        public virtual GenreModel TrackGenre { get; set; } = null!;
    }
}