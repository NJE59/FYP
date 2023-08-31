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
    /// The <see cref="MvxViewModel"/> for the MediaPlayer's library view.
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
        private int currentlyPlayingQueueIndex;
        private int playerPosition = 0;
        private int playerVolume = 50;
        private int selectedQueueIndex;

        // Non-Primitive Backing Fields
        private AlbumModel selectedAlbum = null!;
        private ArtistModel selectedArtist = null!;
        private DiscModel selectedDisc = null!;
        private TrackModel selectedTrack = null!;

        private ObservableCollection<MenuItemModel> navItems = new ();
        private ObservableCollection<TrackModel> trackQueue = new ();

        // Command Backing Fields
        private IMvxCommand addAlbumCommand;
        private IMvxCommand addArtistCommand;
        private IMvxCommand addDiscCommand;
        private IMvxCommand addTrackCommand;
        private IMvxCommand btnClickCommand;
        private IMvxCommand clearQueueCommand;
        private IMvxCommand navigateCommand;
        private IMvxCommand mediaEndedCommand;
        private IMvxCommand playAlbumCommand;
        private IMvxCommand playArtistCommand;
        private IMvxCommand playDiscCommand;
        private IMvxCommand playPauseCommand;
        private IMvxCommand playTrackCommand;
        private IMvxCommand removeTrackCommand;
        private IMvxCommand slideCompletedCommand;
        private IMvxCommand slideStartedCommand;
        private IMvxCommand stopCommand;
        private IMvxCommand skipBackCommand;
        private IMvxCommand skipFowardCommand;
        private IMvxCommand jumpQueueCommand;

        // private TimeSpan _trackPosition = TimeSpan.Zero;

        // Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaLibraryViewModel"/> class.
        /// </summary>
        /// <param name="newTimerFactory"><inheritdoc cref="ITimerFactory" path='/summary'/></param>
        /// <param name="newMediaControllerFactory"><inheritdoc cref="IMediaControllerFactory" path='/summary'/></param>
        public MediaLibraryViewModel(ITimerFactory newTimerFactory, IMediaControllerFactory newMediaControllerFactory)
        {
            /////////////////////////////////////////////FIX TYPES IN EXTENSION
            this.NavItems.CreateNavItems();

            // this.ResetDatabase();
            this.addAlbumCommand = new MvxCommand(this.AddAlbum);
            this.addArtistCommand = new MvxCommand(this.AddArtist);
            this.addDiscCommand = new MvxCommand(this.AddDisc);
            this.addTrackCommand = new MvxCommand(this.AddTrack);
            this.btnClickCommand = new MvxCommand(this.BtnClick);
            this.clearQueueCommand = new MvxCommand(this.ClearQueue);
            this.mediaEndedCommand = new MvxCommand(this.MediaEnded);
            this.navigateCommand = new MvxCommand(this.Navigate);
            this.playAlbumCommand = new MvxCommand(this.PlayAlbum);
            this.playArtistCommand = new MvxCommand(this.PlayArtist);
            this.playPauseCommand = new MvxCommand(this.PlayPause);
            this.playDiscCommand = new MvxCommand(this.PlayDisc);
            this.playTrackCommand = new MvxCommand(this.PlayTrack);
            this.removeTrackCommand = new MvxCommand(this.RemoveTrack);
            this.slideStartedCommand = new MvxCommand(this.SlideStarted);
            this.slideCompletedCommand = new MvxCommand(this.SlideCompleted);
            this.stopCommand = new MvxCommand(this.Stop);
            this.skipBackCommand = new MvxCommand(this.SkipBack);
            this.skipFowardCommand = new MvxCommand(this.SkipForward);
            this.jumpQueueCommand = new MvxCommand(this.JumpQueue);
            this.LoadSongs("D:\\natha\\Music\\1test", DateTime.MinValue);

#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            this.SelectedArtist = this.MediaDB.Artists.FirstOrDefault();
            this.SelectedAlbum = this.SelectedArtist.Albums.FirstOrDefault();
            this.SelectedDisc = this.SelectedAlbum.Discs.FirstOrDefault();
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            this.mediaControllerFactory = newMediaControllerFactory;
            this.mediaController = this.mediaControllerFactory.CreateMediaController(this.MediaEndedCommand);
            this.timerFactory = newTimerFactory;
            this.timer = this.timerFactory.CreateTimer();
            this.timer.Interval = TimeSpan.FromSeconds(1);
            this.timer.Tick += this.TimerTick;
            this.timer.Start();
        }

        // Readonly Backed Properties

        /// <summary>
        /// Gets the local instance of <see cref="MediaDBContext" />.
        /// </summary>
        public MediaDBContext MediaDB => this.mediaDB;

        // Primitive Backed Properties

        /// <summary>
        /// Gets or sets a value indicating whether a track is currently playing.
        /// </summary>
        public bool IsTrackPlaying
        {
            get => this.isTrackPlaying;
            set
            {
                this.SetProperty(ref this.isTrackPlaying, value);
                if (this.IsTrackPlaying == true && this.LoadedTrack == null)
                {
                    this.SetProperty(ref this.isTrackPlaying, false);
                }

                this.RaisePropertyChanged(() => this.DisplayProgression);
            }
        }

        /// <summary>
        /// Gets or sets the index in <see cref="TrackQueue"/> of the currently playing <see cref="TrackModel">track</see>.
        /// </summary>
        public int CurrentlyPlayingQueueIndex
        {
            get => this.currentlyPlayingQueueIndex;
            set
            {
                this.SetProperty(ref this.currentlyPlayingQueueIndex, value);
                if (value < this.TrackQueue.Count)
                {
                    this.SelectedQueueIndex = value;
                    this.RaisePropertyChanged(() => this.LoadedTrack);
                }
            }
        }

        /// <summary>
        /// Gets or sets the position of the currently playing track in the <see cref="IMediaController"/>.
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
        /// Gets or sets the volume of the currently playing track in the <see cref="IMediaController"/>.
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
        /// Gets or sets the track currently selected in <see cref="TrackQueue"/>.
        /// </summary>
        public int SelectedQueueIndex
        {
            get => this.selectedQueueIndex;
            set => this.SetProperty(ref this.selectedQueueIndex, value);
        }

        // Non-Primitive Backed Properties

        /// <summary>
        /// Gets or sets the artist currently selected in the Library.
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
        /// Gets or sets the album currently selected in the Library.
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
        /// Gets or sets the disc currently selected in the Library.
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
        /// Gets or sets the track currently selected in the Library.
        /// </summary>
        public TrackModel SelectedTrack
        {
            get => this.selectedTrack;
            set => this.SetProperty(ref this.selectedTrack, value);
        }

        /// <summary>
        /// Gets the track currently loaded into the <see cref="IMediaController"/>.
        /// </summary>
        public TrackModel? LoadedTrack => (this.CurrentlyPlayingQueueIndex >= 0 && this.CurrentlyPlayingQueueIndex < this.TrackQueue.Count) ? this.TrackQueue[this.CurrentlyPlayingQueueIndex] : null;

        /*public TimeSpan TrackPosition
       {
           get { return _trackPosition; }
           set { SetProperty(ref _trackPosition, value); }
       }*/

        /// <summary>
        /// Gets the <see cref="TrackModel.LoadPath">path</see> of the <see cref="TrackModel">track</see> currently loaded into the <see cref="IMediaController"/>.
        /// </summary>
        public Uri? LoadedPath => this.LoadedTrack?.LoadPath;

        /// <summary>
        /// Gets or sets the items populating the navigation panel.
        /// </summary>
        public ObservableCollection<MenuItemModel> NavItems
        {
            get => this.navItems;
            set => this.SetProperty(ref this.navItems, value);
        }

        /// <summary>
        /// Gets or sets the queue of tracks to be played by the <see cref="IMediaController"/>.
        /// </summary>
        public ObservableCollection<TrackModel> TrackQueue
        {
            get => this.trackQueue;
            set => this.SetProperty(ref this.trackQueue, value);
        }

        // Primitive Accessors

        /// <summary>
        /// Gets a value indicating whether the <see cref="IMediaController"/> currnetly has a <see cref="TrackModel">track</see> loaded into it.
        /// </summary>
        public bool IsTrackLoaded => this.mediaController.Source != null;

        /// <summary>
        /// Gets the <see cref="PlayerPosition"/> as a string formatted according to the <see cref="LoadedTrack"/>'s <see cref="TrackModel.DisplayDurationFormat"/>.
        /// </summary>
        public string DisplayPosition => TimeSpan.FromSeconds(this.PlayerPosition).ToString(this.LoadedTrack?.DisplayDurationFormat);

        /// <summary>
        /// Gets the <see cref="DisplayPosition"/> and <see cref="TrackModel.TrackDuration"/> formatted to display on the UI.
        /// </summary>
        public string DisplayProgression => this.IsTrackPlaying ? $"{this.DisplayPosition} / {this.LoadedTrack?.DisplayDuration}" : string.Empty;

        // Non-Primitive Accessors

        /// <summary>
        /// Gets the current list of artists ordered by <see cref="ArtistModel.ArtistName"/>.
        /// </summary>
        public ObservableCollection<ArtistModel> DisplayArtists => this.MediaDB.Artists.
                OrderBy(artist => artist.ArtistName).ToObservableCollection();

        /// <summary>
        /// Gets the current list of albums by the <see cref="SelectedArtist"/> ordered by <see cref="AlbumModel.AlbumName"/>.
        /// </summary>
        public ObservableCollection<AlbumModel> DisplayArtistsAlbums => this.SelectedArtist.Albums.
                OrderBy(album => album.AlbumName).ToObservableCollection();

        /// <summary>
        /// Gets the current list of discs in the <see cref="SelectedAlbum"/> ordered by <see cref="DiscModel.DiscNum"/>.
        /// </summary>
        public ObservableCollection<DiscModel> DisplayAlbumsDiscs => this.SelectedAlbum.Discs.
            OrderBy(disc => disc.DiscNum).ToObservableCollection();

        /// <summary>
        /// Gets the current list of tracks in the <see cref="SelectedDisc"/> ordered by <see cref="TrackModel.TrackNum"/>.
        /// </summary>
        public ObservableCollection<TrackModel> DisplayDiscsTracks => this.SelectedDisc.Tracks.
                OrderBy(track => track.TrackNum).ToObservableCollection();

        /// <summary>
        /// Gets the current list of tracks.
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
        /// Gets or sets the <see cref="MvxCommand"/> to add an <see cref="AlbumModel"/>'s tracks to the <see cref="TrackQueue"/>.
        /// </summary>
        public IMvxCommand AddAlbumCommand
        {
            get => this.addAlbumCommand ??= new MvxCommand(this.AddAlbum);
            set => this.SetProperty(ref this.addAlbumCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to add an <see cref="ArtistModel"/>'s tracks to the <see cref="TrackQueue"/>.
        /// </summary>
        public IMvxCommand AddArtistCommand
        {
            get => this.addArtistCommand ??= new MvxCommand(this.AddArtist);
            set => this.SetProperty(ref this.addArtistCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to add a <see cref="DiscModel"/>'s tracks to the <see cref="TrackQueue"/>.
        /// </summary>
        public IMvxCommand AddDiscCommand
        {
            get => this.addDiscCommand ??= new MvxCommand(this.AddDisc);
            set => this.SetProperty(ref this.addDiscCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to add a <see cref="TrackModel"/> to the <see cref="TrackQueue"/>.
        /// </summary>
        public IMvxCommand AddTrackCommand
        {
            get => this.addTrackCommand ??= new MvxCommand(this.AddTrack);
            set => this.SetProperty(ref this.addTrackCommand, value);
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
        /// Gets or sets the <see cref="MvxCommand"/> to empty the <see cref="TrackQueue"/>.
        /// </summary>
        public IMvxCommand ClearQueueCommand
        {
            get => this.clearQueueCommand ??= new MvxCommand(this.ClearQueue);
            set => this.SetProperty(ref this.clearQueueCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to run when the <see cref="LoadedTrack"/> ends.
        /// </summary>
        public IMvxCommand MediaEndedCommand
        {
            get => this.mediaEndedCommand ??= new MvxCommand(this.MediaEnded);
            set => this.SetProperty(ref this.mediaEndedCommand, value);
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
        /// Gets or sets the <see cref="MvxCommand"/> to play an <see cref="AlbumModel"/>'s tracks.
        /// </summary>
        public IMvxCommand PlayAlbumCommand
        {
            get => this.playAlbumCommand ??= new MvxCommand(this.PlayAlbum);
            set => this.SetProperty(ref this.playAlbumCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to play an <see cref="ArtistModel"/>'s tracks.
        /// </summary>
        public IMvxCommand PlayArtistCommand
        {
            get => this.playArtistCommand ??= new MvxCommand(this.PlayArtist);
            set => this.SetProperty(ref this.playArtistCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to play a <see cref="DiscModel"/>'s tracks.
        /// </summary>
        public IMvxCommand PlayDiscCommand
        {
            get => this.playDiscCommand ??= new MvxCommand(this.PlayDisc);
            set => this.SetProperty(ref this.playDiscCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> for the play/pause ToggleButton.
        /// </summary>
        public IMvxCommand PlayPauseCommand
        {
            get => this.playPauseCommand ??= new MvxCommand(this.PlayPause);
            set => this.SetProperty(ref this.playPauseCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to play a <see cref="TrackModel"/>.
        /// </summary>
        public IMvxCommand PlayTrackCommand
        {
            get => this.playTrackCommand ??= new MvxCommand(this.PlayTrack);
            set => this.SetProperty(ref this.playTrackCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to remove a <see cref="TrackModel"/> from the <see cref="TrackQueue"/>.
        /// </summary>
        public IMvxCommand RemoveTrackCommand
        {
            get => this.removeTrackCommand ??= new MvxCommand(this.RemoveTrack);
            set => this.SetProperty(ref this.removeTrackCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to run when the user starts using the track position sldier on the UI.
        /// </summary>
        public IMvxCommand SlideStartedCommand
        {
            get => this.slideStartedCommand;
            set => this.SetProperty(ref this.slideStartedCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to run when the user finsishes using the track position sldier on the UI.
        /// </summary>
        public IMvxCommand SlideCompletedCommand
        {
            get => this.slideCompletedCommand ??= new MvxCommand(this.SlideCompleted);
            set => this.SetProperty(ref this.slideCompletedCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to stop the <see cref="LoadedTrack"/>.
        /// </summary>
        public IMvxCommand StopCommand
        {
            get => this.stopCommand ??= new MvxCommand(this.Stop);
            set => this.SetProperty(ref this.stopCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to play the previous track in the <see cref="TrackQueue"/>.
        /// </summary>
        public IMvxCommand SkipBackCommand
        {
            get => this.skipBackCommand ??= new MvxCommand(this.SkipBack);
            set => this.SetProperty(ref this.skipBackCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to play the next track in the <see cref="TrackQueue"/>.
        /// </summary>
        public IMvxCommand SkipForwardCommand
        {
            get => this.skipFowardCommand ??= new MvxCommand(this.SkipForward);
            set => this.SetProperty(ref this.skipFowardCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to jump to playing the currently selected track in the <see cref="TrackQueue"/>.
        /// </summary>
        public IMvxCommand JumpQueueCommand
        {
            get => this.jumpQueueCommand ??= new MvxCommand(this.JumpQueue);
            set => this.SetProperty(ref this.jumpQueueCommand, value);
        }

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

        private void RemoveTrack()
        {
            var indexToRemove = this.SelectedQueueIndex;
            if (this.TrackQueue.Count > indexToRemove)
            {
                if (this.TrackQueue.Count > 1)
                {
                    if (indexToRemove == this.CurrentlyPlayingQueueIndex)
                    {
                        this.ResetQueue();
                    }

                    this.TrackQueue.RemoveAt(indexToRemove);

                    if (indexToRemove < this.CurrentlyPlayingQueueIndex)
                    {
                        this.CurrentlyPlayingQueueIndex--;
                    }
                }
                else
                {
                    this.ClearQueue();
                }
            }
        }

        private void ResetDatabase()
        {
            this.MediaDB.RemoveRange(this.MediaDB.Albums);
            this.MediaDB.RemoveRange(this.MediaDB.Artists);
            this.MediaDB.RemoveRange(this.MediaDB.Contributions);
            this.MediaDB.RemoveRange(this.MediaDB.Discs);
            this.MediaDB.RemoveRange(this.MediaDB.Genres);
            this.MediaDB.RemoveRange(this.MediaDB.Listings);
            this.MediaDB.RemoveRange(this.MediaDB.Playlists);
            this.MediaDB.RemoveRange(this.MediaDB.SongStyles);
            this.MediaDB.RemoveRange(this.MediaDB.Tracks);
            this.MediaDB.SaveChanges();
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

        private void TimerTick(object? sender, object e)
        {
            if (this.IsTrackPlaying)
            {
                this.PlayerPosition = this.isUserDraggingPosition ? this.PlayerPosition : (int)this.mediaController.Position.TotalSeconds;
            }
            else
            {
                this.PlayerPosition = 0;
            }
        }

        private void AddTrack(TrackModel track)
        {
            this.TrackQueue.Add(track);
        }

        private void AddTrack()
        {
            this.AddTrack(this.SelectedTrack);
        }

        private void AddDisc(DiscModel disc)
        {
            foreach (TrackModel track in disc.Tracks)
            {
                this.AddTrack(track);
            }
        }

        private void AddDisc()
        {
            this.AddDisc(this.SelectedDisc);
        }

        private void AddAlbum(AlbumModel album)
        {
            foreach (DiscModel disc in album.Discs)
            {
                this.AddDisc(disc);
            }
        }

        private void AddAlbum()
        {
            this.AddAlbum(this.SelectedAlbum);
        }

        private void AddArtist(ArtistModel artist)
        {
            foreach (AlbumModel album in artist.Albums)
            {
                this.AddAlbum(album);
            }
        }

        private void AddArtist()
        {
            this.AddArtist(this.SelectedArtist);
        }

        private void PlayTrack(TrackModel track)
        {
            this.ClearQueue();
            this.AddTrack(track);
            this.PlayQueue();
        }

        private void PlayTrack()
        {
            this.PlayTrack(this.SelectedTrack);
        }

        private void PlayDisc(DiscModel disc)
        {
            this.ClearQueue();
            this.AddDisc(disc);
            this.PlayQueue();
        }

        private void PlayDisc()
        {
            this.PlayDisc(this.SelectedDisc);
        }

        private void PlayAlbum(AlbumModel album)
        {
            this.ClearQueue();
            this.AddAlbum(album);
            this.PlayQueue();
        }

        private void PlayAlbum()
        {
            this.PlayAlbum(this.SelectedAlbum);
        }

        private void PlayArtist(ArtistModel artist)
        {
            this.ClearQueue();
            this.AddArtist(artist);
            this.PlayQueue();
        }

        private void PlayArtist()
        {
            this.PlayArtist(this.SelectedArtist);
        }

        private void Play()
        {
            this.mediaController.Play();
            this.IsTrackPlaying = true;
        }

        private void Pause()
        {
            this.mediaController.Pause();
            this.IsTrackPlaying = false;
        }

        private void Stop()
        {
            this.mediaController.Stop();
            this.IsTrackPlaying = false;
        }

        private void Close()
        {
            this.Stop();
            this.mediaController.Close();
        }

        private void ChangeTrack(int newIndex)
        {
            this.CurrentlyPlayingQueueIndex = newIndex;
            this.mediaController.Source = this.LoadedPath;
            this.RaisePropertyChanged(() => this.IsTrackLoaded);
        }

        private void PlayPause()
        {
            if (this.IsTrackLoaded == true)
            {
                if (this.IsTrackPlaying == true)
                {
                    this.Pause();
                }
                else
                {
                   this.Play();
                }
            }
            else
            {
                if (this.TrackQueue.Count > 0)
                {
                    this.PlayQueue();
                }
                else
                {
                    this.IsTrackPlaying = false;
                }
            }

            this.RaisePropertyChanged(() => this.IsTrackPlaying);
        }

        private void MediaEnded()
        {
            var newIndex = this.CurrentlyPlayingQueueIndex + 1;
            if (newIndex == this.TrackQueue.Count)
            {
                this.ResetQueue();
            }
            else
            {
                this.ChangeTrack(newIndex);
                this.Play();
            }
        }

        private void ClearQueue()
        {
            this.Close();
            this.TrackQueue.Clear();
            this.ChangeTrack(-1);
        }

        private void ResetQueue()
        {
            this.Close();
            this.ChangeTrack(0);
        }

        private void PlayQueue()
        {
            this.ResetQueue();
            this.Play();
        }

        private void SkipForward()
        {
            if (this.IsTrackLoaded)
            {
                var newIndex = this.CurrentlyPlayingQueueIndex + 1;
                newIndex = (newIndex >= this.TrackQueue.Count) ? 0 : newIndex;
                this.ChangeTrack(newIndex);
            }
        }

        private void SkipBack()
        {
            if (this.IsTrackLoaded)
            {
                var newIndex = this.CurrentlyPlayingQueueIndex;
                if (this.mediaController.Position <= TimeSpan.FromSeconds(5) && newIndex > 0)
                {
                    this.ChangeTrack(newIndex - 1);
                }
                else
                {
                    this.mediaController.Position = TimeSpan.Zero;
                }
            }
        }

        private void JumpQueue()
        {
            if (this.SelectedQueueIndex >= 0 && this.SelectedQueueIndex < this.TrackQueue.Count)
            {
                this.ChangeTrack(this.SelectedQueueIndex);
            }
        }
    }
}
