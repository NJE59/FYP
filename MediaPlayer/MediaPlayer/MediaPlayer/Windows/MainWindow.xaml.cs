using MediaPlayer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Windows.Storage;
using Windows.Storage.Search;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Media.Control;
using Windows.Storage.FileProperties;
using System.Runtime.InteropServices;
using MediaPlayer.Classes;
using MediaPlayer.Models;
using Windows.ApplicationModel.Store.Preview.InstallControl;
using System.IO;
using System.DirectoryServices.ActiveDirectory;
using MediaPlayer.Pages;
using MenuItem = MediaPlayer.Classes.MenuItem;

namespace MediaPlayer.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Value for disc number property in single disc albums and for unconfigured discs
        /// </summary>
        private int defaultDiscNum = 0;
        
        private readonly MediaDBContext MediaDB = new MediaDBContext();
        private CollectionViewSource artistsViewSource, albumsViewSource, discsViewSource, tracksViewSource;
        /// <summary>
        /// <see cref="ObservableCollection{T}"/> of <see cref="MenuItem">MenuItems</see> for populating <see cref="tvNav">tvNav</see> on the <see cref="MainWindow"/>
        /// </summary>
        ObservableCollection<MenuItem> NavItems;

        public MainWindow()
        {
            InitializeComponent();
            NavItems = CreateNavItems();
            tvNav.ItemsSource = NavItems;
            MediaDB.RemoveRange(MediaDB.Albums);
            MediaDB.RemoveRange(MediaDB.Artists);
            MediaDB.RemoveRange(MediaDB.Contributions);
            MediaDB.RemoveRange(MediaDB.Discs);
            MediaDB.RemoveRange(MediaDB.Genres);
            MediaDB.RemoveRange(MediaDB.Listings);
            MediaDB.RemoveRange(MediaDB.Playlists);
            MediaDB.RemoveRange(MediaDB.SongStyles);
            MediaDB.RemoveRange(MediaDB.Tracks);
            MediaDB.SaveChanges();
            artistsViewSource = (CollectionViewSource)FindResource(nameof(artistsViewSource));
            albumsViewSource = (CollectionViewSource)FindResource(nameof(albumsViewSource));
            discsViewSource = (CollectionViewSource)FindResource(nameof(discsViewSource));
            tracksViewSource = (CollectionViewSource)FindResource(nameof(tracksViewSource));
        }

        /// <summary>
        /// Method for populating <see cref="NavItems"/>
        /// </summary>
        private ObservableCollection<MenuItem> CreateNavItems()
        {
            var navItemsCreate = new ObservableCollection<MenuItem>();
            MenuItem mniMusic = new MenuItem("Music", typeof(TracksPage));
            mniMusic.Children.Add(new MenuItem("Album", typeof(AlbumsPage)));
            mniMusic.Children.Add(new MenuItem("Artist", typeof(ArtistsPage)));
            mniMusic.Children.Add(new MenuItem("Genre", typeof(GenresPage)));
            mniMusic.Children.Add(new MenuItem("Year", typeof(YearsPage)));
            MenuItem mniPlaylists = new MenuItem("Playlists", typeof(PlaylistsPage));
            mniPlaylists.Children.Add(new MenuItem("Playlist1", typeof(SinglePlaylistPage)));
            mniPlaylists.Children.Add(new MenuItem("New Playlist", typeof(SinglePlaylistPage)));
            navItemsCreate.Add(mniMusic);
            navItemsCreate.Add(mniPlaylists);
            return navItemsCreate;
        }

        private void tvNav_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            /*if (tvNav.SelectedItem != null)
            {
                Type pageType = ((MenuItem)tvNav.SelectedItem).PageType;
                MainView.Content = (Page)Activator.CreateInstance(pageType);
            }*/
        }
            

        private void LoadSongs(string rootFolder, DateTime lastUpdated)
        {
            int discNum = 0;
            var acceptedFileTypes = new List<string>() { ".mp3", ".flac" };

            // Finds all the new/recently edited music files in the given root folder and its subfolders
            // and generates a list of all their properties grouped by their genre(s)
            var genreGroupedTracks = 
                StorageFolder.GetFolderFromPathAsync(rootFolder).GetAwaiter().GetResult().GetFiles().
                Where(f => f.GetBasicProperties().DateModified.DateTime > lastUpdated && 
                                      acceptedFileTypes.Contains(f.FileType)).
                Select(songFile => new FileProperties() 
                {
                    Path = songFile.Path,
                    Artists = (songFile.Properties.RetrieveProperty("System.Music.Artist")
                        is String[] artistsCheck) ?
                        artistsCheck.OrderBy(artistName => artistName).ToList<string>() :
                        new List<string>(),
                    DiscNum = (songFile.Properties.RetrieveProperty("System.Music.PartOfSet")
                            is string discNumCheck &&
                            // PartOfSet can be formatted as "part/total", so remove the total
                            // and check that the part is indeed a postive integer
                            Int32.TryParse(discNumCheck.Split("/")[0], out discNum) == true &&
                            discNum > 0) ?
                            // If the value passes all checks return it, else return the default disc number (0)
                                discNum : 
                                defaultDiscNum,
                    DiscName = (songFile.Properties.RetrieveProperty("System.Music.ContentGroupDescription")
                        is string discNameCheck) ? 
                            discNameCheck : 
                            "",
                    IsMultiDisc = (songFile.Properties.RetrieveProperty("System.Music.IsCompilation")
                        is bool isMultiDiscCheck) ? 
                            isMultiDiscCheck : 
                            false,
                    MusicProperties = songFile.Properties.GetMusicProperties()
                }).     
                GroupBy(trackProperties => trackProperties.MusicProperties.Genre);

            foreach (var genreGroup in genreGroupedTracks)
            {
                var genreList = new List<Genre>();

                foreach (var genreName in genreGroup.Key)
                {
                    genreList.Add((MediaDB.Genres.
                        Where(genreRecord => genreRecord.GenreName == genreName).SingleOrDefault() 
                        is Genre genreCheck) ? 
                            genreCheck : 
                            MediaDB.Genres.Add(new Genre() { GenreName = genreName }).Entity
                    );
                    MediaDB.SaveChanges();
                }
                var artistGroupedTracks = genreGroup.
                    GroupBy(trackProperties => trackProperties.MusicProperties.AlbumArtist);
                
                foreach (var artistGroup in artistGroupedTracks)
                {
                    var albumArtist = (MediaDB.Artists.
                        Where(artistRecord => artistRecord.ArtistName == artistGroup.Key).SingleOrDefault()
                        is Artist artistCheck) ? 
                            artistCheck : 
                            MediaDB.Artists.Add(new Artist() { ArtistName = artistGroup.Key }).Entity;
                    MediaDB.SaveChanges();
                    var albumGroupedTracks = artistGroup.
                        GroupBy(trackProperties => new 
                        { 
                            trackProperties.MusicProperties.Album, 
                            trackProperties.MusicProperties.Year
                        });

                    foreach(var albumGroup in albumGroupedTracks)
                    {

                        var album = (albumArtist.Albums.
                            Where(albumRecord =>
                                albumRecord.AlbumName == albumGroup.Key.Album &&
                                albumRecord.ReleaseYear == albumGroup.Key.Year).SingleOrDefault()
                            is Album albumCheck) ?
                                albumCheck :
                                MediaDB.Albums.Add(new Album()
                                {
                                    AlbumName = albumGroup.Key.Album,
                                    ReleaseYear = (int)albumGroup.Key.Year,
                                    Artist = albumArtist
                                }).Entity;
                        MediaDB.SaveChanges();
                        var isMultiDiscCheck = !albumGroup.All(fileProperties => 
                            fileProperties.DiscNum == defaultDiscNum) && 
                            (album.Discs.Count == 0 ||
                             album.Discs.Count == 1 && album.Discs.First().DiscNum == defaultDiscNum);
                        var discGroupedTracks = albumGroup.
                            GroupBy(trackProperties => new
                            {
                                trackProperties.DiscNum,
                                trackProperties.DiscName
                            });

                        foreach(var discGroup in discGroupedTracks)
                        {
                            var disc = (album.Discs.
                                Where(discRec => 
                                    discRec.DiscNum == discGroup.Key.DiscNum &&
                                    discRec.DiscName == discGroup.Key.DiscName).SingleOrDefault()
                                is Disc discCheck) ?
                                    discCheck :
                                    MediaDB.Discs.Add(new Disc()
                                    {
                                        DiscNum = discGroup.Key.DiscNum,
                                        DiscName = discGroup.Key.DiscName,
                                        Album = album
                                    }).Entity;
                            MediaDB.SaveChanges();

                            var contributorGroupedTracks = discGroup.
                                GroupBy(trackProperties => trackProperties.Artists);

                            foreach(var contributorGroup in contributorGroupedTracks)
                            {
                                List<Artist>? contributingArtistList = new List<Artist>();
                                
                                foreach (var contributorName in contributorGroup.Key)
                                {
                                    contributingArtistList.Add((MediaDB.Artists.
                                        Where(artistRecord => artistRecord.ArtistName == contributorName).SingleOrDefault()
                                        is Artist contributorCheck) ?
                                            contributorCheck :
                                            MediaDB.Artists.Add(new Artist() { ArtistName = contributorName }).Entity
                                    );                                    
                                }
                                MediaDB.SaveChanges();

                                foreach (var trackProperties in contributorGroup)
                                {
                                    var trackGenreList = genreList;
                                    var contributorList = contributingArtistList;

                                    var track = (disc.Tracks.
                                        Where(trackRecord =>
                                            (string.Equals(trackProperties.Path, trackRecord.Path, StringComparison.OrdinalIgnoreCase)) ||
                                            (!File.Exists(trackRecord.Path) &&
                                            (uint)trackRecord.TrackNum == trackProperties.MusicProperties.TrackNumber &&
                                            string.Equals(trackRecord.TrackName, trackProperties.MusicProperties.Title, StringComparison.OrdinalIgnoreCase) &&
                                            trackRecord.Contributions.
                                                Select(contributionRecord => contributionRecord.Contributor).
                                                Equals(contributorList)) &&
                                            trackRecord.SongStyles.
                                                Select(songStyleRecord => songStyleRecord.TrackGenre).
                                                Equals(trackGenreList)).
                                            SingleOrDefault() is Track trackCheck) ?
                                                trackCheck :
                                                MediaDB.Tracks.Add(new Track() { Disc = disc, Path = trackProperties.Path }).Entity;
                                    /////////////////////////////////////////////////////
                                    if (File.Exists(track.Path))
                                    {
                                        track.TrackNum = (int)trackProperties.MusicProperties.TrackNumber;
                                        track.TrackName = trackProperties.MusicProperties.Title;
                                        track.TrackLength = trackProperties.MusicProperties.Duration;
                                        foreach (var contribution in track.Contributions)
                                            if (contributorList.Find(contributorRecord => contributorRecord.Equals(contribution.Contributor)) is Artist contributorCheck)
                                                contributorList.Remove(contributorCheck);
                                        foreach (var contributor in contributorList)
                                            MediaDB.Contributions.Add(new Contribution() { Contributor = contributor, Track = track });
                                        foreach (var songStyle in track.SongStyles)
                                            if (trackGenreList.Find(trackGenreRecord => trackGenreRecord.Equals(songStyle.TrackGenre)) is Genre trackGenreCheck)
                                                trackGenreList.Remove(trackGenreCheck);
                                        foreach (var trackGenre in trackGenreList)
                                            MediaDB.SongStyles.Add(new SongStyle() { TrackGenre = trackGenre, Track = track });
                                    }
                                    else
                                        track.Path = trackProperties.Path;
                                    MediaDB.SaveChanges();

                                }

                            }

                            
                        }
                        
                        
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSongs(rootFolder: "D:\\natha\\Music\\1test", DateTime.MinValue);
            artistsViewSource.SortDescriptions.Add(new SortDescription("ArtistName", ListSortDirection.Ascending));
            albumsViewSource.SortDescriptions.Add(new SortDescription("AlbumName", ListSortDirection.Ascending));
            discsViewSource.SortDescriptions.Add(new SortDescription("DiscNum", ListSortDirection.Ascending));
            tracksViewSource.SortDescriptions.Add(new SortDescription("TrackNum", ListSortDirection.Ascending));
            artistsViewSource.Source = MediaDB.Artists.Local.ToObservableCollection();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Testing");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            MediaDB.Dispose();
            base.OnClosing(e);
        }
    }
}
