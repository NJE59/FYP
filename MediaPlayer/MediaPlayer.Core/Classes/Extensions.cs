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
        /// <param name="properties"><inheritdoc cref="StorageItemContentProperties" path='/summary'/></param>
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
        /// Method for converting an <see cref="IEnumerable{T}"/> to an <see cref="ObservableCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="input"><inheritdoc cref="IEnumerable{T}" path='/summary'/></param>
        /// <returns>new <see cref="ObservableCollection{T}"/> of the inputted <see cref="IEnumerable{T}"/>.</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> input) => new (input);
    }
}
