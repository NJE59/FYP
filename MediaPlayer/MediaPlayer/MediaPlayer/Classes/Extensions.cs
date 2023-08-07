using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;
using Windows.Storage;
using Windows.Storage.Search;
using System.Collections;

namespace MediaPlayer.Classes
{
    /// <summary>
    /// Class containing all extension methods for this project
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// <see cref="StorageFolder"/> extension method creating a synchronous version of <see cref="StorageFolder.GetFilesAsync(CommonFileQuery)"/>
        /// </summary>
        /// <param name="folder"><see cref="StorageFolder"/> instance being extended, doesn't need passing as an argument</param>
        /// <returns>a <see cref="IReadOnlyList{StorageFile}"/> of all the files contained within the given <see cref="StorageFolder"/></returns>
        public static IReadOnlyList<StorageFile> GetFiles(this StorageFolder folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder)); 
            return folder.GetFilesAsync(CommonFileQuery.OrderByMusicProperties).GetAwaiter().GetResult();
        }


        /// <summary>
        /// <see cref="StorageFile"/> extension method creating a synchronous version of <see cref="StorageFile.GetBasicPropertiesAsync()"/>
        /// </summary>
        /// <param name="file"><see cref="StorageFile"/> instance being extended, doesn't need passing as an argument</param>
        /// <returns>a <see cref="BasicProperties"/> <see cref="object"/> containing the basic properties of the file</returns>
        public static BasicProperties GetBasicProperties(this StorageFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file)); 
            return file.GetBasicPropertiesAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// <see cref="StorageItemContentProperties"/> extension method creating a synchronous version of <see cref="StorageItemContentProperties.RetrievePropertiesAsync(IEnumerable{string})"/>
        /// </summary>
        /// <param name="propertyToRetrieve"><see cref="StorageItemContentProperties"/> instance being extended, doesn't need passing as an argument</param>
        /// <returns>an <see cref="object"/> containing the given properties of the file</returns>
        public static object RetrieveProperty(this StorageItemContentProperties Properties, string propertyToRetrieve)
        {
            if (Properties == null) throw new ArgumentNullException(nameof(Properties));
            return Properties.RetrievePropertiesAsync(new List<string>() { propertyToRetrieve }).GetAwaiter().GetResult()[propertyToRetrieve];
        }


        /// <summary>
        /// <see cref="StorageItemContentProperties"/> extension method creating a synchronous version of <see cref="StorageItemContentProperties.GetMusicPropertiesAsync()"/>
        /// </summary>
        /// <param name="Properties"><see cref="StorageItemContentProperties"/> instance being extended, doesn't need passing as an argument</param>
        /// <returns>a <see cref="MusicProperties"/> <see cref="object"/> containing the music properties of the file</returns>
        public static MusicProperties GetMusicProperties(this StorageItemContentProperties Properties)
        {
            if (Properties == null) throw new ArgumentNullException(nameof(Properties));
            return Properties.GetMusicPropertiesAsync().GetAwaiter().GetResult();
        }
    }
}
