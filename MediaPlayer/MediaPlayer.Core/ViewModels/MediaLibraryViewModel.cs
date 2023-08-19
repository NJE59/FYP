using MediaPlayer.Core.Classes;
using MediaPlayer.Core.Data;
using MediaPlayer.Core.Models;
using Microsoft.UI.Xaml;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
//using System.Windows.Data;
using Windows.Storage;

namespace MediaPlayer.Core.ViewModels
{
    public class MediaLibraryViewModel : MvxViewModel
    {
        //private CollectionViewSource artistsViewSource, albumsViewSource, discsViewSource, tracksViewSource;
        private readonly MediaDBContext _mediaDB = new MediaDBContext();
		public MediaDBContext MediaDB
		{
			get { return _mediaDB; }
		}

        public ObservableCollection<ArtistModel> DisplayArtists => new ObservableCollection<ArtistModel>(
            MediaDB.Artists.
                OrderBy(artist => artist.ArtistName));

        private ArtistModel _selectedArtist;
        public ArtistModel SelectedArtist
        {
            get { return _selectedArtist; }
            set 
            { 
                SetProperty(ref _selectedArtist, value);
                SelectedAlbum = DisplayArtistsAlbums.First();
                RaisePropertyChanged(() => DisplayArtistsAlbums);
            }
        }

        public ObservableCollection<AlbumModel> DisplayArtistsAlbums => new ObservableCollection<AlbumModel>(
            SelectedArtist.Albums.
                OrderBy(album => album.AlbumName));

        private AlbumModel _selectedAlbum;
        public AlbumModel SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                SetProperty(ref _selectedAlbum, value);
                SelectedDisc = DisplayAlbumsDiscs.First();
                RaisePropertyChanged(() => DisplayAlbumsDiscs);
            }
        }

        public ObservableCollection<DiscModel> DisplayAlbumsDiscs => new ObservableCollection<DiscModel>(
            SelectedAlbum.Discs.
                OrderBy(disc => disc.DiscNum));

        private DiscModel _selectedDisc;
        public DiscModel SelectedDisc
        {
            get { return _selectedDisc; }
            set
            {
                SetProperty(ref _selectedDisc, value);
                SelectedTrack = DisplayDiscsTracks.First();
                RaisePropertyChanged(() => DisplayDiscsTracks);
            }
        }

        public ObservableCollection<TrackModel> DisplayDiscsTracks => new ObservableCollection<TrackModel>(
            SelectedDisc.Tracks.
                OrderBy(track => track.TrackNum));

        private TrackModel _selectedTrack;

        public TrackModel SelectedTrack
        {
            get { return _selectedTrack; }
            set { SetProperty(ref _selectedTrack, value); }
        }


        public ObservableCollection<TrackModel> TracksDisplay => new ObservableCollection<TrackModel>(
            MediaDB.Tracks.
            OrderBy(track => track.AlbumArtist.ArtistName).
                ThenBy(track => track.Album.AlbumName).
                ThenBy(track => track.Disc.DiscNum).
                ThenBy(track => track.TrackNum).
                ThenBy(track => track.TrackID));

        /// <summary>
        /// <see cref="ObservableCollection{MenuItemModel}"/> of <see cref="MenuItem">MenuItems</see> for populating <see cref="tvNav">tvNav</see> on the <see cref="MainWindow"/>
        /// </summary>
        private ObservableCollection<MenuItemModel> _navItems = new ObservableCollection<MenuItemModel>();
        public ObservableCollection<MenuItemModel> NavItems
		{
			get { return _navItems; }
			set { SetProperty(ref _navItems, value); }
		}

        private TrackModel _loadedTrack;

        public TrackModel LoadedTrack
        {
            get { return _loadedTrack; }
            set { SetProperty(ref _loadedTrack, value); }
        }

        /// <summary>
        /// Value for disc number property in single disc albums and for unconfigured discs
        /// </summary>
        private int defaultDiscNum = 0;


        public MediaLibraryViewModel()
		{
/////////////////////////////////////////////FIX TYPES IN EXTENSION
            NavItems.CreateNavItems();
            /*MediaDB.RemoveRange(MediaDB.Albums);
            MediaDB.RemoveRange(MediaDB.Artists);
            MediaDB.RemoveRange(MediaDB.Contributions);
            MediaDB.RemoveRange(MediaDB.Discs);
            MediaDB.RemoveRange(MediaDB.Genres);
            MediaDB.RemoveRange(MediaDB.Listings);
            MediaDB.RemoveRange(MediaDB.Playlists);
            MediaDB.RemoveRange(MediaDB.SongStyles);
            MediaDB.RemoveRange(MediaDB.Tracks);
            MediaDB.SaveChanges();*/
            /*artistsViewSource = (CollectionViewSource)FindResource(nameof(artistsViewSource));
            albumsViewSource = (CollectionViewSource)FindResource(nameof(albumsViewSource));
            discsViewSource = (CollectionViewSource)FindResource(nameof(discsViewSource));
            tracksViewSource = (CollectionViewSource)FindResource(nameof(tracksViewSource));*/
            btnClickCommand = new MvxCommand(btnClick);
            NavigateCommand = new MvxCommand(Navigate);
            PlayTrackCommand = new MvxCommand(PlayTrack);
            LoadSongs("D:\\natha\\Music\\1test", DateTime.MinValue);
            SelectedArtist = MediaDB.Artists.FirstOrDefault();
            SelectedAlbum = SelectedArtist.Albums.FirstOrDefault();
            SelectedDisc = SelectedAlbum.Discs.FirstOrDefault();
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += Timer_Tick;
            //timer.Start();
        }

        private void Timer_Tick(object? sender, object e)
        {
            throw new NotImplementedException();
        }

        public IMvxCommand PlayTrackCommand { get; set; }
        public void PlayTrack()
        {
            LoadedTrack = SelectedTrack;
        }

        private void Window_Loaded()
        {
            /*artistsViewSource.SortDescriptions.Add(new SortDescription("ArtistName", ListSortDirection.Ascending));
            albumsViewSource.SortDescriptions.Add(new SortDescription("AlbumName", ListSortDirection.Ascending));
            discsViewSource.SortDescriptions.Add(new SortDescription("DiscNum", ListSortDirection.Ascending));
            tracksViewSource.SortDescriptions.Add(new SortDescription("TrackNum", ListSortDirection.Ascending));
            artistsViewSource.Source = MediaDB.Artists.Local.ToObservableCollection();*/
        }
        protected void OnClosing()
        {
            MediaDB.Dispose();
            //base.OnClosing(e);
        }
        
        public IMvxCommand NavigateCommand { get; set; }
        private void Navigate()
        {
            Debug.WriteLine("Navigating");
            /*if (tvNav.SelectedItem != null)
            {
                Type pageType = ((MenuItem)tvNav.SelectedItem).PageType;
                MainView.Content = (Page)Activator.CreateInstance(pageType);
            }*/
        }

        public IMvxCommand btnClickCommand { get; set; }
        public void btnClick()
        {
            Debug.WriteLine("Testing");
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
                Select(songFile => new FilePropertiesModel()
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
                var genreList = new List<GenreModel>();

                foreach (var genreName in genreGroup.Key)
                {
                    genreList.Add((MediaDB.Genres.
                        Where(genreRecord => genreRecord.GenreName == genreName).SingleOrDefault()
                        is GenreModel genreCheck) ?
                            genreCheck :
                            MediaDB.Genres.Add(new GenreModel() { GenreName = genreName }).Entity
                    );
                    MediaDB.SaveChanges();
                }
                var artistGroupedTracks = genreGroup.
                    GroupBy(trackProperties => trackProperties.MusicProperties.AlbumArtist);

                foreach (var artistGroup in artistGroupedTracks)
                {
                    var albumArtist = (MediaDB.Artists.
                        Where(artistRecord => artistRecord.ArtistName == artistGroup.Key).SingleOrDefault()
                        is ArtistModel artistCheck) ?
                            artistCheck :
                            MediaDB.Artists.Add(new ArtistModel() { ArtistName = artistGroup.Key }).Entity;
                    MediaDB.SaveChanges();
                    var albumGroupedTracks = artistGroup.
                        GroupBy(trackProperties => new
                        {
                            trackProperties.MusicProperties.Album,
                            trackProperties.MusicProperties.Year
                        });

                    foreach (var albumGroup in albumGroupedTracks)
                    {

                        var album = (albumArtist.Albums.
                            Where(albumRecord =>
                                albumRecord.AlbumName == albumGroup.Key.Album &&
                                albumRecord.ReleaseYear == albumGroup.Key.Year).SingleOrDefault()
                            is AlbumModel albumCheck) ?
                                albumCheck :
                                MediaDB.Albums.Add(new AlbumModel()
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

                        foreach (var discGroup in discGroupedTracks)
                        {
                            var disc = (album.Discs.
                                Where(discRec =>
                                    discRec.DiscNum == discGroup.Key.DiscNum &&
                                    discRec.DiscName == discGroup.Key.DiscName).SingleOrDefault()
                                is DiscModel discCheck) ?
                                    discCheck :
                                    MediaDB.Discs.Add(new DiscModel()
                                    {
                                        DiscNum = discGroup.Key.DiscNum,
                                        DiscName = discGroup.Key.DiscName,
                                        Album = album
                                    }).Entity;
                            MediaDB.SaveChanges();

                            var contributorGroupedTracks = discGroup.
                                GroupBy(trackProperties => trackProperties.Artists);

                            foreach (var contributorGroup in contributorGroupedTracks)
                            {
                                var contributingArtistList = new List<ArtistModel>();

                                foreach (var contributorName in contributorGroup.Key)
                                {
                                    contributingArtistList.Add((MediaDB.Artists.
                                        Where(artistRecord => artistRecord.ArtistName == contributorName).SingleOrDefault()
                                        is ArtistModel contributorCheck) ?
                                            contributorCheck :
                                            MediaDB.Artists.Add(new ArtistModel() { ArtistName = contributorName }).Entity
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
                                            SingleOrDefault() is TrackModel trackCheck) ?
                                                trackCheck :
                                                MediaDB.Tracks.Add(new TrackModel() { Disc = disc, Path = trackProperties.Path }).Entity;
                                    /////////////////////////////////////////////////////
                                    if (File.Exists(track.Path))
                                    {
                                        track.TrackNum = (int)trackProperties.MusicProperties.TrackNumber;
                                        track.TrackName = trackProperties.MusicProperties.Title;
                                        track.TrackLength = trackProperties.MusicProperties.Duration;
                                        foreach (var contribution in track.Contributions)
                                            if (contributorList.Find(contributorRecord => contributorRecord.Equals(contribution.Contributor)) is ArtistModel contributorCheck)
                                                contributorList.Remove(contributorCheck);
                                        foreach (var contributor in contributorList)
                                            MediaDB.Contributions.Add(new ContributionModel() { Contributor = contributor, Track = track });
                                        foreach (var songStyle in track.SongStyles)
                                            if (trackGenreList.Find(trackGenreRecord => trackGenreRecord.Equals(songStyle.TrackGenre)) is GenreModel trackGenreCheck)
                                                trackGenreList.Remove(trackGenreCheck);
                                        foreach (var trackGenre in trackGenreList)
                                            MediaDB.SongStyles.Add(new SongStyleModel() { TrackGenre = trackGenre, Track = track });
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
    }
}
