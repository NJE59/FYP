// <copyright file="Extensions.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Classes
{
    using System.Collections.ObjectModel;
    using MediaPlayer.Core.Models;
    using MediaPlayer.Core.ViewModels;
    using Windows.Storage;
    using Windows.Storage.FileProperties;
    using Windows.Storage.Search;

    /// <summary>
    /// Class containing all extension methods for this project.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// <see cref="StorageFolder"/> extension method creating a synchronous version of <see cref="StorageFolder.GetFilesAsync(CommonFileQuery)"/>.
        /// </summary>
        /// <param name="folder"><see cref="StorageFolder"/> instance being extended, doesn't need passing as an argument.</param>
        /// <returns>a <see cref="IReadOnlyList{StorageFile}"/> of all the files contained within the given <see cref="StorageFolder"/>.</returns>
        public static IReadOnlyList<StorageFile> GetFiles(this StorageFolder folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            return folder.GetFilesAsync(CommonFileQuery.OrderByMusicProperties).GetAwaiter().GetResult();
        }

        /// <summary>
        /// <see cref="StorageFile"/> extension method creating a synchronous version of <see cref="StorageFile.GetBasicPropertiesAsync()"/>.
        /// </summary>
        /// <param name="file"><see cref="StorageFile"/> instance being extended, doesn't need passing as an argument.</param>
        /// <returns>a <see cref="BasicProperties"/> <see cref="object"/> containing the basic properties of the file.</returns>
        public static BasicProperties GetBasicProperties(this StorageFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            return file.GetBasicPropertiesAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// <see cref="StorageItemContentProperties"/> extension method creating a synchronous version of <see cref="StorageItemContentProperties.RetrievePropertiesAsync(IEnumerable{string})"/>.
        /// </summary>
        /// <param name="properties">PLACEHOLDER.</param>
        /// <param name="propertyToRetrieve"><see cref="StorageItemContentProperties"/> instance being extended, doesn't need passing as an argument.</param>
        /// <returns>an <see cref="object"/> containing the given properties of the file.</returns>
        public static object RetrieveProperty(this StorageItemContentProperties properties, string propertyToRetrieve)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            return properties.RetrievePropertiesAsync(new List<string>() { propertyToRetrieve }).GetAwaiter().GetResult()[propertyToRetrieve];
        }

        /// <summary>
        /// <see cref="StorageItemContentProperties"/> extension method creating a synchronous version of <see cref="StorageItemContentProperties.GetMusicPropertiesAsync()"/>.
        /// </summary>
        /// <param name="properties"><see cref="StorageItemContentProperties"/> instance being extended, doesn't need passing as an argument.</param>
        /// <returns>a <see cref="MusicProperties"/> <see cref="object"/> containing the music properties of the file.</returns>
        public static MusicProperties GetMusicProperties(this StorageItemContentProperties properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            return properties.GetMusicPropertiesAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Method for populating <see cref="MediaLibraryViewModel.NavItems"/>.
        /// </summary>
        /// <param name="navItems"><inheritdoc cref="MediaLibraryViewModel.NavItems" path='/summary'/></param>
        /// <returns>1PLACEHOLDER.</returns>
        public static ObservableCollection<MenuItemModel> CreateNavItems(this ObservableCollection<MenuItemModel> navItems)
        {
            //////////////////////////////////////////////////////////////////////////////////////////////FIX TYPES
            MenuItemModel mniMusic = new MenuItemModel("Music", typeof(string));
            mniMusic.Children.Add(new MenuItemModel("Album", typeof(string)));
            mniMusic.Children.Add(new MenuItemModel("Artist", typeof(string)));
            mniMusic.Children.Add(new MenuItemModel("Genre", typeof(string)));
            mniMusic.Children.Add(new MenuItemModel("Year", typeof(string)));
            MenuItemModel mniPlaylists = new MenuItemModel("Playlists", typeof(string));
            mniPlaylists.Children.Add(new MenuItemModel("Playlist1", typeof(string)));
            mniPlaylists.Children.Add(new MenuItemModel("New Playlist", typeof(string)));
            navItems.Add(mniMusic);
            navItems.Add(mniPlaylists);
            return navItems;
        }
    }
}
