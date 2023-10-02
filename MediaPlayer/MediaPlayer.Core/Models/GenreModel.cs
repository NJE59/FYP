// <copyright file="GenreModel.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Models
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using MediaPlayer.Core.Classes;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Model describnig a music genre.
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
        /// Gets or sets the unique ID of this genre.
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
        /// Gets or sets the name of this genre.
        /// </summary>
        [BackingField(nameof(this.genreName))]
        public string GenreName
        {
            get => this.genreName;
            set => this.genreName = value;
        }

        /// <summary>
        /// Gets or sets the description of this genre.
        /// </summary>
        [BackingField(nameof(this.description))]
        public string? Description
        {
            get => this.description;
            set => this.description = value;
        }

        // Navigation Properties

        /// <summary>
        /// Gets an <see cref="ObservableCollection{T}"/> of the usages of this genre.
        /// </summary>
        public virtual ICollection<SongStyleModel> TrackGenres { get; private set; } = new ObservableCollection<SongStyleModel>();

        // NotMapped Accessors

        /// <summary>
        /// Gets the full list of tracks in this genre.
        /// </summary>
        [NotMapped]
        public ObservableCollection<TrackModel> Tracks => this.TrackGenres.Select(songStyle => songStyle.Track).ToObservableCollection();
    }
}