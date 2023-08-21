// <copyright file="GenreModel.cs" company="Nathan Errington">
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
    public class GenreModel
    {
        // Primary Key Backing Fields
        private int genreID;

        // Other Backing Fields
        private string genreName = null!;
        private string? description;

        // Primary Key Backed Properties

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.genreID))]
        [Key]
        public int GenreID
        {
            get => this.genreID;
            set => this.genreID = value;
        }

        // Other Backed Properties

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        [BackingField(nameof(this.genreName))]
        public string GenreName
        {
            get => this.genreName;
            set => this.genreName = value;
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
        /// Gets PLACEHOLDER.
        /// </summary>
        public virtual ICollection<SongStyleModel> TrackGenres { get; private set; } = new ObservableCollection<SongStyleModel>();
    }
}