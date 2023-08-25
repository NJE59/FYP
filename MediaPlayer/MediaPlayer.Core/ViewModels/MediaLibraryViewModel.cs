// <copyright file="MediaLibraryViewModel.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Input;
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
        // Readonly Primitive Backing Fields
        private readonly int defaultDiscNum = 0;

        // Readonly Non-Primitive Backing Fields
        private readonly IMediaControllerFactory mediaControllerFactory;
        private readonly IMediaController mediaController;
        private readonly ITimerFactory timerFactory;
        private readonly ITimer timer;
        private readonly MediaDBContext mediaDB = new ();

        // Primitive Backing Fields
        private bool isTrackPlaying = false;
        private bool isUserDraggingPosition = false;
        private int currentQueueIndex;
        private int playerPosition = 0;
        private int playerVolume = 50;

        // Non-Primitive Backing Fields
        private AlbumModel selectedAlbum = null!;
        private ArtistModel selectedArtist = null!;
        private DiscModel selectedDisc = null!;
        private TrackModel selectedTrack = null!;
        private TrackModel loadedTrack = null!;

        private ObservableCollection<MenuItemModel> navItems = new ();
        private ObservableCollection<TrackModel> trackQueue = new ();

        // Command Backing Fields
        private IMvxCommand addSelectedArtistCommand;
        private IMvxCommand addSelectedAlbumCommand;
        private IMvxCommand addSelectedDiscCommand;
        private IMvxCommand addSelectedTrackCommand;
        private IMvxCommand btnClickCommand;
        private IMvxCommand playPauseCommand;
        private IMvxCommand stopCommand;
        private IMvxCommand slideStartedCommand;
        private IMvxCommand slideCompletedCommand;
        private IMvxCommand startTrackCommand;
        private IMvxCommand navigateCommand;

        // private TimeSpan _trackPosition = TimeSpan.Zero;

        // Constructors

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

            this.addSelectedArtistCommand = new MvxCommand(this.AddSelectedArtist);
            this.addSelectedAlbumCommand = new MvxCommand(this.AddSelectedAlbum);
            this.addSelectedDiscCommand = new MvxCommand(this.AddSelectedDisc);
            this.addSelectedTrackCommand = new MvxCommand(this.AddSelectedTrack);
            this.btnClickCommand = new MvxCommand(this.BtnClick);
            this.navigateCommand = new MvxCommand(this.Navigate);
            this.playPauseCommand = new MvxCommand(this.PlayPause);
            this.stopCommand = new MvxCommand(this.Stop);
            this.slideStartedCommand = new MvxCommand(this.SlideStarted);
            this.slideCompletedCommand = new MvxCommand(this.SlideCompleted);
            this.startTrackCommand = new MvxCommand(this.StartTrack);

            this.LoadSongs("D:\\natha\\Music\\1test", DateTime.MinValue);

#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            this.SelectedArtist = this.MediaDB.Artists.FirstOrDefault();
            this.SelectedAlbum = this.SelectedArtist.Albums.FirstOrDefault();
            this.SelectedDisc = this.SelectedAlbum.Discs.FirstOrDefault();
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            this.mediaControllerFactory = newMediaControllerFactory;
            this.mediaController = this.mediaControllerFactory.CreateMediaController();
            this.timerFactory = newTimerFactory;
            this.timer = this.timerFactory.CreateTimer();
            this.timer.Interval = TimeSpan.FromSeconds(1);
            this.timer.Tick += this.TimerTick;
            this.timer.Start();
        }

        // Readonly Backed Properties

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public MediaDBContext MediaDB => this.mediaDB;

        // Primitive Backed Properties

        /// <summary>
        /// Gets or sets a value indicating whether PLACEHOLDER.
        /// </summary>
        public bool IsTrackPlaying
        {
            get => this.isTrackPlaying;
            set => this.SetProperty(ref this.isTrackPlaying, value);
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public int CurrentQueueIndex
        {
            get => this.currentQueueIndex;
            set => this.SetProperty(ref this.currentQueueIndex, value);
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public int PlayerPosition
        {
            get => this.playerPosition;
            set
            {
                this.SetProperty(ref this.playerPosition, value);
                this.RaisePropertyChanged(() => this.DisplayPosition);
                this.RaisePropertyChanged(() => this.DisplayProgression);
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

        // Non-Primitive Backed Properties

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
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public TrackModel SelectedTrack
        {
            get => this.selectedTrack;
            set => this.SetProperty(ref this.selectedTrack, value);
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

                if (this.IsTrackLoaded)
                {
                    this.mediaController.Source = this.LoadedTrack.LoadPath;
                    this.PlayPause();
                }

                this.RaisePropertyChanged(() => this.IsTrackLoaded);
                this.RaisePropertyChanged(() => this.DisplayPosition);
            }
        }

        /*public TimeSpan TrackPosition
       {
           get { return _trackPosition; }
           set { SetProperty(ref _trackPosition, value); }
       }*/

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
        public ObservableCollection<TrackModel> TrackQueue
        {
            get => this.trackQueue;
            set => this.SetProperty(ref this.trackQueue, value);
        }

        // Primitive Accessors

        /// <summary>
        /// Gets a value indicating whether PLACEHOLDER.
        /// </summary>
        public bool IsTrackLoaded => this.MediaDB.Tracks.Contains(this.LoadedTrack);

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public string DisplayPosition => TimeSpan.FromSeconds(this.PlayerPosition).ToString(this.LoadedTrack.DisplayDurationFormat);

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public string DisplayProgression => this.IsTrackPlaying ? $"{this.DisplayPosition} / {this.LoadedTrack.DisplayDuration}" : string.Empty;

        // Non-Primitive Accessors

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<ArtistModel> DisplayArtists => this.MediaDB.Artists.
                OrderBy(artist => artist.ArtistName).ToObservableCollection();

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<AlbumModel> DisplayArtistsAlbums => this.SelectedArtist.Albums.
                OrderBy(album => album.AlbumName).ToObservableCollection();

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<DiscModel> DisplayAlbumsDiscs => this.SelectedAlbum.Discs.
            OrderBy(disc => disc.DiscNum).ToObservableCollection();

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<TrackModel> DisplayDiscsTracks => this.SelectedDisc.Tracks.
                OrderBy(track => track.TrackNum).ToObservableCollection();

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<TrackModel> TracksDisplay => this.MediaDB.Tracks.
            OrderBy(track => track.AlbumArtist.ArtistName).
                ThenBy(track => track.Album.AlbumName).
                ThenBy(track => track.Disc.DiscNum).
                ThenBy(track => track.TrackNum).
                ThenBy(track => track.TrackID).
            ToObservableCollection();

        // Command Backed Properties

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand AddSelectedArtistCommand
        {
            get => this.addSelectedArtistCommand ??= new MvxCommand(this.AddSelectedArtist);
            set => this.SetProperty(ref this.addSelectedArtistCommand, value);
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand AddSelectedAlbumCommand
        {
            get => this.addSelectedAlbumCommand ??= new MvxCommand(this.AddSelectedAlbum);
            set => this.SetProperty(ref this.addSelectedAlbumCommand, value);
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand AddSelectedDiscCommand
        {
            get => this.addSelectedDiscCommand ??= new MvxCommand(this.AddSelectedDisc);
            set => this.SetProperty(ref this.addSelectedDiscCommand, value);
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand AddSelectedTrackCommand
        {
            get => this.addSelectedTrackCommand ??= new MvxCommand(this.AddSelectedTrack);
            set => this.SetProperty(ref this.addSelectedTrackCommand, value);
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand BtnClickCommand
        {
            get => this.btnClickCommand ??= new MvxCommand(this.BtnClick);
            set => this.SetProperty(ref this.btnClickCommand, value);
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand NavigateCommand
        {
            get => this.navigateCommand ??= new MvxCommand(this.Navigate);
            set => this.SetProperty(ref this.navigateCommand, value);
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand StartTrackCommand
        {
            get => this.startTrackCommand ??= new MvxCommand(this.StartTrack);
            set => this.SetProperty(ref this.startTrackCommand, value);
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand PlayPauseCommand
        {
            get => this.playPauseCommand ??= new MvxCommand(this.PlayPause);
            set => this.SetProperty(ref this.playPauseCommand, value);
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand StopCommand
        {
            get => this.stopCommand ??= new MvxCommand(this.Stop);
            set => this.SetProperty(ref this.stopCommand, value);
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand SlideStartedCommand
        {
            get => this.slideStartedCommand;
            set => this.SetProperty(ref this.slideStartedCommand, value);
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand SlideCompletedCommand
        {
            get => this.slideCompletedCommand ??= new MvxCommand(this.SlideCompleted);
            set => this.SetProperty(ref this.slideCompletedCommand, value);
        }

        private bool IsIndexEven => this.CurrentQueueIndex % 2 == 0;

        // Protected Methods

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        protected void OnClosing()
        {
            this.MediaDB.Dispose();

            // base.OnClosing(e);
        }

        // Private Methods
        private void BtnClick()
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

        private void Navigate()
        {
            Debug.WriteLine("Navigating");
            /*if (tvNav.SelectedItem != null)
            {
                Type pageType = ((MenuItem)tvNav.SelectedItem).PageType;
                MainView.Content = (Page)Activator.CreateInstance(pageType);
            }*/
        }

        private void PlayPause()
        {
            if (this.IsTrackLoaded)
            {
                if (this.IsTrackPlaying)
                {
                    Debug.WriteLine("Pause");
                    this.mediaController.Pause();
                    this.IsTrackPlaying = false;
                }
                else
                {
                    Debug.WriteLine("Play");
                    this.mediaController.Play();
                    this.IsTrackPlaying = true;
                }
            }
        }

        private void Stop()
        {
            Debug.WriteLine("Stop");
            if (this.IsTrackLoaded)
            {
                this.mediaController.Stop();
                this.IsTrackPlaying = false;
            }
        }

        private void SlideStarted()
        {
            Debug.WriteLine("Slide Started");
            this.isUserDraggingPosition = true;
        }

        private void SlideCompleted()
        {
            Debug.WriteLine("Slide Completed");
            this.isUserDraggingPosition = false;
            this.mediaController.Position = TimeSpan.FromSeconds(this.PlayerPosition);
        }

        private void StartTrack()
        {
            this.LoadedTrack = this.SelectedTrack;
        }

        private void TimerTick(object? sender, object e)
        {
            if (this.mediaController.Source != null && this.mediaController.HasTimeSpan)
            {
                this.PlayerPosition = this.isUserDraggingPosition ? this.PlayerPosition : (int)this.mediaController.Position.TotalSeconds;
            }
            else
            {
                this.PlayerPosition = 0;
            }
        }

        private void AddSelectedArtist()
        {
            this.AddArtist(this.SelectedArtist);
        }

        private void AddSelectedAlbum()
        {
            this.AddAlbum(this.SelectedAlbum);
        }

        private void AddSelectedDisc()
        {
            this.AddDisc(this.SelectedDisc);
        }

        private void AddSelectedTrack()
        {
            this.AddTrack(this.SelectedTrack);
        }

        private void AddArtist(ArtistModel artist)
        {
            foreach (var album in artist.Albums.OrderBy(albumRecord => albumRecord.AlbumName))
            {
                this.AddAlbum(album);
            }
        }

        private void AddAlbum(AlbumModel album)
        {
            foreach (var disc in album.Discs.OrderBy(discRecord => discRecord.DiscNum))
            {
                this.AddDisc(disc);
            }
        }

        private void AddDisc(DiscModel disc)
        {
            foreach (var track in disc.Tracks.OrderBy(trackRecord => trackRecord.TrackNum))
            {
                this.AddTrack(track);
            }
        }

        private void AddTracks(IEnumerable<TrackModel> tracks)
        {
            foreach (var track in tracks)
            {
                this.AddTrack(track);
            }
        }

        private void AddTrack(TrackModel track)
        {
            this.TrackQueue.Add(track);
        }
    }
}
