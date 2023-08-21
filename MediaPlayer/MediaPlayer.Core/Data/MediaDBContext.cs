// <copyright file="MediaDBContext.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Data
{
    using System.Reflection;
    using MediaPlayer.Core.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// PLACEHOLDER.
    /// </summary>
    public class MediaDBContext : DbContext
    {
        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public DbSet<TrackModel> Tracks { get; set; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public DbSet<DiscModel> Discs { get; set; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public DbSet<AlbumModel> Albums { get; set; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public DbSet<ArtistModel> Artists { get; set; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public DbSet<ContributionModel> Contributions { get; set; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public DbSet<GenreModel> Genres { get; set; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public DbSet<SongStyleModel> SongStyles { get; set; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public DbSet<PlaylistModel> Playlists { get; set; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public DbSet<ListingModel> Listings { get; set; }

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        /// <param name="optionsBuilder"><inheritdoc cref="DbContextOptionsBuilder" path='/summary'/></param>
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
