// <copyright file="MediaDBContext.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Data
{
    using System.Reflection;
    using MediaPlayer.Core.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Class describing the media database.
    /// </summary>
    public class MediaDBContext : DbContext
    {
        /// <summary>
        /// Gets or sets the tracks table.
        /// </summary>
        public DbSet<TrackModel> Tracks { get; set; }

        /// <summary>
        /// Gets or sets the discs table.
        /// </summary>
        public DbSet<DiscModel> Discs { get; set; }

        /// <summary>
        /// Gets or sets the albums table.
        /// </summary>
        public DbSet<AlbumModel> Albums { get; set; }

        /// <summary>
        /// Gets or sets the artists table.
        /// </summary>
        public DbSet<ArtistModel> Artists { get; set; }

        /// <summary>
        /// Gets or sets the contributions table.
        /// </summary>
        public DbSet<ContributionModel> Contributions { get; set; }

        /// <summary>
        /// Gets or sets the genres table.
        /// </summary>
        public DbSet<GenreModel> Genres { get; set; }

        /// <summary>
        /// Gets or sets the song styles table.
        /// </summary>
        public DbSet<SongStyleModel> SongStyles { get; set; }

        /// <summary>
        /// Gets or sets the playlists table.
        /// </summary>
        public DbSet<PlaylistModel> Playlists { get; set; }

        /// <summary>
        /// Gets or sets the lsitings table.
        /// </summary>
        public DbSet<ListingModel> Listings { get; set; }

        /// <inheritdoc cref="DbContext.OnConfiguring(DbContextOptionsBuilder)" path='/summary'/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "FileName=MediaDB.db", option =>
                {
                    option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                });
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
