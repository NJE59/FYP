// <copyright file="MediaLibraryViewModel.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using MediaPlayer.Core.Classes;
    using MediaPlayer.Core.Data;
    using MediaPlayer.Core.Interfaces;
    using MediaPlayer.Core.Models;
    using MvvmCross.Commands;
    using MvvmCross.ViewModels;
    using Windows.Storage;

    /// <summary>
    /// PLACEHOLDER.
    /// </summary>
    public class MediaLibraryViewModel : MvxViewModel
    {
        /// <summary>
        /// Value for disc number property in single disc albums and for unconfigured discs.
        /// </summary>
        private readonly int defaultDiscNum = 0;

        private readonly ITimerFactory timerFactory;
        private readonly ITimer timer;
        private readonly IMediaControllerFactory mediaControllerFactory;
        private readonly IMediaController mediaController;

        private readonly MediaDBContext mediaDB = new ();

        private int playerVolume = 50;

        private string displayPosition = null!;

        private AlbumModel selectedAlbum = null!;
        private ArtistModel selectedArtist = null!;
        private DiscModel selectedDisc = null!;
        private TrackModel selectedTrack = null!;
        private TrackModel loadedTrack = null!;

        private ObservableCollection<MenuItemModel> navItems = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaLibraryViewModel"/> class.
        /// </summary>
        /// <param name="newTimerFactory">PLACEHOLDER.</param>
        /// <param name="newMediaControllerFactory">PLACEHOLDER1.</param>
        public MediaLibraryViewModel(ITimerFactory newTimerFactory, IMediaControllerFactory newMediaControllerFactory)
        {
            /////////////////////////////////////////////FIX TYPES IN EXTENSION
            this.NavItems.CreateNavItems();
            /*this.MediaDB.RemoveRange(this.MediaDB.Albums);
            this.MediaDB.RemoveRange(this.MediaDB.Artists);
            this.MediaDB.RemoveRange(this.MediaDB.Contributions);
            this.MediaDB.RemoveRange(this.MediaDB.Discs);
            this.MediaDB.RemoveRange(this.MediaDB.Genres);
            this.MediaDB.RemoveRange(this.MediaDB.Listings);
            this.MediaDB.RemoveRange(this.MediaDB.Playlists);
            this.MediaDB.RemoveRange(this.MediaDB.SongStyles);
            this.MediaDB.RemoveRange(this.MediaDB.Tracks);
            this.MediaDB.SaveChanges();*/
            this.BtnClickCommand = new MvxCommand(this.BtnClick);
            this.NavigateCommand = new MvxCommand(this.Navigate);
            this.PlayTrackCommand = new MvxCommand(this.PlayTrack);
            this.LoadSongs("D:\\natha\\Music\\1test", DateTime.MinValue);
            this.SelectedArtist = this.MediaDB.Artists.FirstOrDefault();
            this.SelectedAlbum = this.SelectedArtist.Albums.FirstOrDefault();
            this.SelectedDisc = this.SelectedAlbum.Discs.FirstOrDefault();
            this.mediaControllerFactory = newMediaControllerFactory;
            this.mediaController = this.mediaControllerFactory.CreateMediaController();
            this.timerFactory = newTimerFactory;
            this.timer = this.timerFactory.CreateTimer();
            this.timer.Interval = TimeSpan.FromSeconds(1);
            this.timer.Tick += this.Timer_Tick;
            this.timer.Start();
        }

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public MediaDBContext MediaDB => this.mediaDB;

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<ArtistModel> DisplayArtists => new (
        this.MediaDB.Artists.
                OrderBy(artist => artist.ArtistName));

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public ArtistModel SelectedArtist
        {
            get => this.selectedArtist;
            set
            {
                this.SetProperty(ref this.selectedArtist, value);
                this.SelectedAlbum = this.DisplayArtistsAlbums.First();
                this.RaisePropertyChanged(() => this.DisplayArtistsAlbums);
            }
        }

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<AlbumModel> DisplayArtistsAlbums => new (
            this.SelectedArtist.Albums.
                OrderBy(album => album.AlbumName));

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public AlbumModel SelectedAlbum
        {
            get => this.selectedAlbum;
            set
            {
                this.SetProperty(ref this.selectedAlbum, value);
                this.SelectedDisc = this.DisplayAlbumsDiscs.First();
                this.RaisePropertyChanged(() => this.DisplayAlbumsDiscs);
            }
        }

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<DiscModel> DisplayAlbumsDiscs => new (
            this.SelectedAlbum.Discs.
                OrderBy(disc => disc.DiscNum));

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public DiscModel SelectedDisc
        {
            get => this.selectedDisc;
            set
            {
                this.SetProperty(ref this.selectedDisc, value);
                this.SelectedTrack = this.DisplayDiscsTracks.First();
                this.RaisePropertyChanged(() => this.DisplayDiscsTracks);
            }
        }

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<TrackModel> DisplayDiscsTracks => new (
            this.SelectedDisc.Tracks.
                OrderBy(track => track.TrackNum));

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public TrackModel SelectedTrack
        {
            get => this.selectedTrack;
            set => this.SetProperty(ref this.selectedTrack, value);
        }

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<TrackModel> TracksDisplay => new (
            this.MediaDB.Tracks.
            OrderBy(track => track.AlbumArtist.ArtistName).
                ThenBy(track => track.Album.AlbumName).
                ThenBy(track => track.Disc.DiscNum).
                ThenBy(track => track.TrackNum).
                ThenBy(track => track.TrackID));

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<MenuItemModel> NavItems
        {
            get => this.navItems;
            set => this.SetProperty(ref this.navItems, value);
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public TrackModel LoadedTrack
        {
            get => this.loadedTrack;
            set
            {
                this.SetProperty(ref this.loadedTrack, value);
                this.mediaController.Source = this.LoadedTrack.LoadPath;
                this.DisplayPosition = (this.LoadedTrack != null) ? TimeSpan.Zero.ToString(this.LoadedTrack.DisplayDurationFormat) : string.Empty;
                this.mediaController.Play();
            }
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public int PlayerVolume
        {
            get => this.playerVolume;
            set
            {
                this.SetProperty(ref this.playerVolume, value);
                this.mediaController.Volume = (double)this.PlayerVolume / 100D;
            }
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public string DisplayPosition
        {
            get => this.displayPosition;
            set => this.SetProperty(ref this.displayPosition, value);
        }

        /*private TimeSpan _trackPosition = TimeSpan.Zero;

        public TimeSpan TrackPosition
        {
            get { return _trackPosition; }
            set { SetProperty(ref _trackPosition, value); }
        }*/

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand PlayTrackCommand { get; set; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand NavigateCommand { get; set; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand BtnClickCommand { get; set; }

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public void PlayTrack()
        {
            this.LoadedTrack = this.SelectedTrack;
        }

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public void BtnClick()
        {
            Debug.WriteLine("Testing");
        }

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        protected void OnClosing()
        {
            this.MediaDB.Dispose();

            // base.OnClosing(e);
        }

        private void Timer_Tick(object? sender, object e)
        {
            if (this.mediaController.Source != null && this.mediaController.HasTimeSpan)
            {
                this.DisplayPosition = this.mediaController.Position.ToString(this.LoadedTrack.DisplayDurationFormat);
            }
            else
            {
                this.DisplayPosition = string.Empty;
            }
        }

        private void Navigate()
        {
            Debug.WriteLine("Navigating");
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
                Select(songFile => new FilePropertiesModel()
                {
                    Path = songFile.Path,
                    Artists = (songFile.Properties.RetrieveProperty("System.Music.Artist")
                        is string[] artistsCheck) ?
                        artistsCheck.OrderBy(artistName => artistName).ToList<string>() :
                        new List<string>(),
                    DiscNum = (songFile.Properties.RetrieveProperty("System.Music.PartOfSet")
                            is string discNumCheck &&

                            // PartOfSet can be formatted as "part/total", so remove the total
                            // and check that the part is indeed a postive integer
                            int.TryParse(discNumCheck.Split("/")[0], out discNum) == true &&
                            discNum > 0) ?

                                // If the value passes all checks return it, else return the default disc number (0)
                                discNum :
                                this.defaultDiscNum,
                    DiscName = (songFile.Properties.RetrieveProperty("System.Music.ContentGroupDescription")
                        is string discNameCheck) ?
                            discNameCheck :
                            string.Empty,
                    MusicProps = songFile.Properties.GetMusicProperties(),
                }).
                GroupBy(trackProperties => trackProperties.MusicProps.Genre);

            foreach (var genreGroup in genreGroupedTracks)
            {
                var genreList = new List<GenreModel>();

                foreach (var genreName in genreGroup.Key)
                {
                    genreList.Add((this.MediaDB.Genres.
                        Where(genreRecord => genreRecord.GenreName == genreName).SingleOrDefault()
                        is GenreModel genreCheck) ?
                            genreCheck :
                            this.MediaDB.Genres.Add(new GenreModel() { GenreName = genreName }).Entity);
                    this.MediaDB.SaveChanges();
                }

                var artistGroupedTracks = genreGroup.
                    GroupBy(trackProperties => trackProperties.MusicProps.AlbumArtist);

                foreach (var artistGroup in artistGroupedTracks)
                {
                    var albumArtist = (this.MediaDB.Artists.
                        Where(artistRecord => artistRecord.ArtistName == artistGroup.Key).SingleOrDefault()
                        is ArtistModel artistCheck) ?
                            artistCheck :
                            this.MediaDB.Artists.Add(new ArtistModel() { ArtistName = artistGroup.Key }).Entity;
                    this.MediaDB.SaveChanges();
                    var albumGroupedTracks = artistGroup.
                        GroupBy(trackProperties => new
                        {
                            trackProperties.MusicProps.Album,
                            trackProperties.MusicProps.Year,
                        });

                    foreach (var albumGroup in albumGroupedTracks)
                    {
                        var album = (albumArtist.Albums.
                            Where(albumRecord =>
                                albumRecord.AlbumName == albumGroup.Key.Album &&
                                albumRecord.ReleaseYear == albumGroup.Key.Year).SingleOrDefault()
                            is AlbumModel albumCheck) ?
                                albumCheck :
                                this.MediaDB.Albums.Add(new AlbumModel()
                                {
                                    AlbumName = albumGroup.Key.Album,
                                    ReleaseYear = (int)albumGroup.Key.Year,
                                    Artist = albumArtist,
                                }).Entity;
                        this.MediaDB.SaveChanges();
                        var isMultiDiscCheck = !albumGroup.All(fileProperties =>
                            fileProperties.DiscNum == this.defaultDiscNum) &&
                            (album.Discs.Count == 0 ||
                            (album.Discs.Count == 1 && album.Discs.First().DiscNum == this.defaultDiscNum));
                        var discGroupedTracks = albumGroup.
                            GroupBy(trackProperties => new
                            {
                                trackProperties.DiscNum,
                                trackProperties.DiscName,
                            });

                        foreach (var discGroup in discGroupedTracks)
                        {
                            var disc = (album.Discs.
                                Where(discRec =>
                                    discRec.DiscNum == discGroup.Key.DiscNum &&
                                    discRec.DiscName == discGroup.Key.DiscName).SingleOrDefault()
                                is DiscModel discCheck) ?
                                    discCheck :
                                    this.MediaDB.Discs.Add(new DiscModel()
                                    {
                                        DiscNum = discGroup.Key.DiscNum,
                                        DiscName = discGroup.Key.DiscName,
                                        Album = album,
                                    }).Entity;
                            this.MediaDB.SaveChanges();

                            var contributorGroupedTracks = discGroup.
                                GroupBy(trackProperties => trackProperties.Artists);

                            foreach (var contributorGroup in contributorGroupedTracks)
                            {
                                var contributingArtistList = new List<ArtistModel>();

                                foreach (var contributorName in contributorGroup.Key)
                                {
                                    contributingArtistList.Add((this.MediaDB.Artists.
                                        Where(artistRecord => artistRecord.ArtistName == contributorName).SingleOrDefault()
                                        is ArtistModel contributorCheck) ?
                                            contributorCheck :
                                            this.MediaDB.Artists.Add(new ArtistModel() { ArtistName = contributorName }).Entity);
                                }

                                this.MediaDB.SaveChanges();

                                foreach (var trackProperties in contributorGroup)
                                {
                                    var trackGenreList = genreList;
                                    var contributorList = contributingArtistList;

                                    var track = (disc.Tracks.SingleOrDefault(trackRecord =>
                                            string.Equals(trackProperties.Path, trackRecord.Path, StringComparison.OrdinalIgnoreCase) ||
                                            (!File.Exists(trackRecord.Path) &&
                                            (uint)trackRecord.TrackNum == trackProperties.MusicProps.TrackNumber &&
                                            string.Equals(trackRecord.TrackName, trackProperties.MusicProps.Title, StringComparison.OrdinalIgnoreCase) &&
                                            trackRecord.Contributions.
                                                Select(contributionRecord => contributionRecord.Contributor).
                                                Equals(contributorList) &&
                                            trackRecord.SongStyles.
                                                Select(songStyleRecord => songStyleRecord.TrackGenre).
                                                Equals(trackGenreList))) is TrackModel trackCheck) ?
                                                trackCheck :
                                                this.MediaDB.Tracks.Add(new TrackModel() { Disc = disc, Path = trackProperties.Path }).Entity;
                                    /////////////////////////////////////////////////////
                                    if (File.Exists(track.Path))
                                    {
                                        track.TrackNum = (int)trackProperties.MusicProps.TrackNumber;
                                        track.TrackName = trackProperties.MusicProps.Title;
                                        track.TrackDuration = trackProperties.MusicProps.Duration;
                                        foreach (var contribution in track.Contributions)
                                        {
                                            if (contributorList.Find(contributorRecord => contributorRecord.Equals(contribution.Contributor)) is ArtistModel contributorCheck)
                                            {
                                                contributorList.Remove(contributorCheck);
                                            }
                                        }

                                        foreach (var contributor in contributorList)
                                        {
                                            this.MediaDB.Contributions.Add(new ContributionModel() { Contributor = contributor, Track = track });
                                        }

                                        foreach (var songStyle in track.SongStyles)
                                        {
                                            if (trackGenreList.Find(trackGenreRecord => trackGenreRecord.Equals(songStyle.TrackGenre)) is GenreModel trackGenreCheck)
                                            {
                                                trackGenreList.Remove(trackGenreCheck);
                                            }
                                        }

                                        foreach (var trackGenre in trackGenreList)
                                        {
                                            this.MediaDB.SongStyles.Add(new SongStyleModel() { TrackGenre = trackGenre, Track = track });
                                        }
                                    }
                                    else
                                    {
                                        track.Path = trackProperties.Path;
                                    }

                                    this.MediaDB.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
