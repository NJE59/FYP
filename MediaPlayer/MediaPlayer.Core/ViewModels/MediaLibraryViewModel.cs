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
    using MvvmCross.Presenters.Hints;
    using MvvmCross.ViewModels;
    using Windows.ApplicationModel.UserDataTasks;
    using Windows.Storage;

    /// <summary>
    /// The <see cref="MvxViewModel"/> for the MediaPlayer's library view.
    /// </summary>
    public class MediaLibraryViewModel : MvxViewModel
    {
        #region Readonly Primitive Backing Fields

        private readonly int defaultDiscNum = 0;

        #endregion

        #region Readonly Non-Primitive Backing Fields

        private readonly IMediaControllerFactory mediaControllerFactory;
        private readonly IMediaController mediaController;
        private readonly ITimerFactory timerFactory;
        private readonly ITimer timer;
        private readonly MediaDBContext mediaDB = new ();

        #endregion

        #region Primitive Backing Fields

        private bool isTrackPlaying = false;
        private bool isUserDraggingPosition = false;
        private int currentlyPlayingQueueIndex;
        private int playerPosition = 0;
        private int playerVolume = 50;
        private int selectedQueueIndex;
        private bool showingAlbums = false;
        private bool showingArtists = false;
        private bool showingGenres = false;
        private bool showingPlaylists = false;
        private bool showingTracks = true;
        private bool showingArtistTracks = false;
        private bool showingAlbumTracks = false;
        private bool showingGenreTracks = false;

        #endregion

        #region Non-Primitive Backing Fields

        private AlbumModel? selectedAlbum = null!;
        private ArtistModel? selectedArtist = null!;
        private GenreModel? selectedGenre = null!;
        private TrackModel? selectedTrack = null!;

        // private TimeSpan trackPosition = TimeSpan.Zero;
        private ObservableCollection<TrackModel> trackQueue = new ();

        #endregion

        #region Command Backing Fields

        private IMvxCommand addAlbumCommand;
        private IMvxCommand addArtistCommand;
        private IMvxCommand addGenreCommand;
        private IMvxCommand addTrackCommand;
        private IMvxCommand clearQueueCommand;
        private IMvxCommand navigateCommand;
        private IMvxCommand mediaEndedCommand;
        private IMvxCommand playAlbumCommand;
        private IMvxCommand playArtistCommand;
        private IMvxCommand playGenreCommand;
        private IMvxCommand playPauseCommand;
        private IMvxCommand playTrackCommand;
        private IMvxCommand removeTrackCommand;
        private IMvxCommand slideCompletedCommand;
        private IMvxCommand slideStartedCommand;
        private IMvxCommand stopCommand;
        private IMvxCommand skipBackCommand;
        private IMvxCommand skipFowardCommand;
        private IMvxCommand jumpQueueCommand;
        private IMvxCommand showAlbumsCommand;
        private IMvxCommand showArtistsCommand;
        private IMvxCommand showGenresCommand;
        private IMvxCommand showTracksCommand;
        private IMvxCommand showArtistTracksCommand;
        private IMvxCommand showAlbumTracksCommand;
        private IMvxCommand showGenreTracksCommand;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaLibraryViewModel"/> class.
        /// </summary>
        /// <param name="newTimerFactory"><inheritdoc cref="ITimerFactory" path='/summary'/></param>
        /// <param name="newMediaControllerFactory"><inheritdoc cref="IMediaControllerFactory" path='/summary'/></param>
        public MediaLibraryViewModel(ITimerFactory newTimerFactory, IMediaControllerFactory newMediaControllerFactory)
        {
            // this.ResetDatabase();
            this.addAlbumCommand = new MvxCommand(this.AddAlbum);
            this.addArtistCommand = new MvxCommand(this.AddArtist);
            this.addGenreCommand = new MvxCommand(this.AddGenre);
            this.addTrackCommand = new MvxCommand(this.AddTrack);
            this.clearQueueCommand = new MvxCommand(this.ClearQueue);
            this.mediaEndedCommand = new MvxCommand(this.MediaEnded);
            this.navigateCommand = new MvxCommand(this.Navigate);
            this.playAlbumCommand = new MvxCommand(this.PlayAlbum);
            this.playArtistCommand = new MvxCommand(this.PlayArtist);
            this.playPauseCommand = new MvxCommand(this.PlayPause);
            this.playGenreCommand = new MvxCommand(this.PlayGenre);
            this.playTrackCommand = new MvxCommand(this.PlayTrack);
            this.removeTrackCommand = new MvxCommand(this.RemoveTrack);
            this.slideStartedCommand = new MvxCommand(this.SlideStarted);
            this.slideCompletedCommand = new MvxCommand(this.SlideCompleted);
            this.stopCommand = new MvxCommand(this.Stop);
            this.skipBackCommand = new MvxCommand(this.SkipBack);
            this.skipFowardCommand = new MvxCommand(this.SkipForward);
            this.jumpQueueCommand = new MvxCommand(this.JumpQueue);
            this.showAlbumsCommand = new MvxCommand(this.ShowAlbums);
            this.showArtistsCommand = new MvxCommand(this.ShowArtists);
            this.showGenresCommand = new MvxCommand(this.ShowGenres);
            this.showPlaylistsCommand = new MvxCommand(this.ShowPlaylists);
            this.showTracksCommand = new MvxCommand(this.ShowTracks);
            this.showArtistTracksCommand = new MvxCommand(this.ShowArtistTracks);
            this.showAlbumTracksCommand = new MvxCommand(this.ShowAlbumTracks);
            this.showGenreTracksCommand = new MvxCommand(this.ShowGenreTracks);

            this.RaisePropertyChanged(() => this.ShowingAlbums);
            this.RaisePropertyChanged(() => this.ShowingArtists);
            this.RaisePropertyChanged(() => this.ShowingGenres);
            this.RaisePropertyChanged(() => this.ShowingPlaylists);
            this.RaisePropertyChanged(() => this.ShowingTracks);
            this.RaisePropertyChanged(() => this.ShowingArtistTracks);
            this.RaisePropertyChanged(() => this.ShowingAlbumTracks);
            this.RaisePropertyChanged(() => this.ShowingGenreTracks);

            this.LoadSongs("D:\\natha\\Music\\1test", DateTime.MinValue);

            if (this.MediaDB.Playlists.Where(playlist => playlist.PlaylistName == "Playlist1").Any())
            {
                this.MediaDB.Playlists.Add(new PlaylistModel() { PlaylistName = "Playlist1" });
                this.MediaDB.SaveChanges();
                this.RaisePropertyChanged(() => this.DisplayPlaylists);
            }

            if (this.MediaDB.Playlists.Where(playlist => playlist.PlaylistName == "New Playlist").Any())
            {
                this.MediaDB.Playlists.Add(new PlaylistModel() { PlaylistName = "New Playlist" });
                this.MediaDB.SaveChanges();
                this.RaisePropertyChanged(() => this.DisplayPlaylists);
            }

            this.SelectedArtist = this.MediaDB.Artists.FirstOrDefault();
            this.SelectedAlbum = this.SelectedArtist?.Albums.FirstOrDefault();
            this.SelectedGenre = this.SelectedAlbum?.Tracks.FirstOrDefault()?.Genres.FirstOrDefault();
            this.mediaControllerFactory = newMediaControllerFactory;
            this.mediaController = this.mediaControllerFactory.CreateMediaController(this.MediaEndedCommand);
            this.timerFactory = newTimerFactory;
            this.timer = this.timerFactory.CreateTimer();
            this.timer.Interval = TimeSpan.FromSeconds(1);
            this.timer.Tick += this.TimerTick;
            this.timer.Start();
        }

        #endregion

        #region Readonly Backed Properties

        /// <summary>
        /// Gets the local instance of <see cref="MediaDBContext" />.
        /// </summary>
        public MediaDBContext MediaDB => this.mediaDB;

        #endregion

        #region Primitive Backed Properties

        /// <summary>
        /// Gets or sets a value indicating whether the UI should be showing the Tracks DataGrid.
        /// </summary>
        public bool ShowingTracks
        {
            get => this.showingTracks;
            set => this.SetProperty(ref this.showingTracks, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the UI should be showing the Albums DataGrid.
        /// </summary>
        public bool ShowingAlbums
        {
            get => this.showingAlbums;
            set => this.SetProperty(ref this.showingAlbums, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the UI should be showing the Artists DataGrid.
        /// </summary>
        public bool ShowingArtists
        {
            get => this.showingArtists;
            set => this.SetProperty(ref this.showingArtists, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the UI should be showing the Genres DataGrid.
        /// </summary>
        public bool ShowingGenres
        {
            get => this.showingGenres;
            set => this.SetProperty(ref this.showingGenres, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the UI should be showing the <see cref="SelectedArtist"/>'s tracks DataGrid.
        /// </summary>
        public bool ShowingArtistTracks
        {
            get => this.showingArtistTracks;
            set => this.SetProperty(ref this.showingArtistTracks, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the UI should be showing the <see cref="SelectedAlbum"/>'s tracks DataGrid.
        /// </summary>
        public bool ShowingAlbumTracks
        {
            get => this.showingAlbumTracks;
            set => this.SetProperty(ref this.showingAlbumTracks, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the UI should be showing the <see cref="SelectedGenre"/>'s tracks DataGrid.
        /// </summary>
        public bool ShowingGenreTracks
        {
            get => this.showingGenreTracks;
            set => this.SetProperty(ref this.showingGenreTracks, value);
        }

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

        #endregion

        #region Non-Primitive Backed Properties

        /// <summary>
        /// Gets or sets the artist currently selected in the Library.
        /// </summary>
        public ArtistModel? SelectedArtist
        {
            get => this.selectedArtist;
            set
            {
                var oldArtist = this.selectedArtist;
                this.SetProperty(ref this.selectedArtist, value);
                if (oldArtist != this.selectedArtist)
                {
                    this.SelectedAlbum = (this.SelectedAlbum?.Artist == this.SelectedArtist) ? this.SelectedArtist?.Albums.FirstOrDefault() : this.SelectedAlbum;
                }

                this.RaisePropertyChanged(() => this.DisplayArtistTracks);
            }
        }

        /// <summary>
        /// Gets or sets the album currently selected in the Library.
        /// </summary>
        public AlbumModel? SelectedAlbum
        {
            get => this.selectedAlbum;
            set
            {
                var oldAlbum = this.selectedAlbum;
                this.SetProperty(ref this.selectedAlbum, value);
                if (oldAlbum != this.selectedAlbum)
                {
                    this.SelectedTrack = (this.SelectedTrack?.Disc.Album == this.SelectedAlbum) ? this.SelectedAlbum?.Tracks.FirstOrDefault() : this.SelectedTrack;
                }

                this.SelectedArtist = this.SelectedAlbum?.Artist;
                this.RaisePropertyChanged(() => this.DisplayAlbumTracks);
            }
        }

        /// <summary>
        /// Gets or sets the genre currently selected in the Library.
        /// </summary>
        public GenreModel? SelectedGenre
        {
            get => this.selectedGenre;
            set
            {
                this.SetProperty(ref this.selectedGenre, value);
                this.SelectedTrack = this.DisplayGenreTracks?.First();
                this.RaisePropertyChanged(() => this.DisplayGenreTracks);
            }
        }

        /// <summary>
        /// Gets or sets the track currently selected in the Library.
        /// </summary>
        public TrackModel? SelectedTrack
        {
            get => this.selectedTrack;
            set
            {
                this.SetProperty(ref this.selectedTrack, value);
                this.SelectedAlbum = (this.SelectedTrack != null) ? this.SelectedTrack.Disc.Album : this.SelectedAlbum;
            }
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
        /// Gets or sets the queue of tracks to be played by the <see cref="IMediaController"/>.
        /// </summary>
        public ObservableCollection<TrackModel> TrackQueue
        {
            get => this.trackQueue;
            set => this.SetProperty(ref this.trackQueue, value);
        }

        #endregion

        #region Primitive Accessors

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
        public string DisplayProgression => this.IsTrackLoaded ? $"{this.DisplayPosition} / {this.LoadedTrack?.DisplayDuration}" : string.Empty;

        #endregion

        #region Non-Primitive Accessors

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<AlbumModel>? DisplayAlbums => this.MediaDB.Albums.
            OrderBy(album => album.AlbumName).
            ThenBy(album => album.Artist.ArtistName).
            ThenBy(album => album.AlbumID).
            ToObservableCollection();

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<ArtistModel>? DisplayArtists => this.MediaDB.Artists.
            OrderBy(artist => artist.ArtistName).
            ThenBy(artist => artist.ArtistID).
            ToObservableCollection();

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<GenreModel>? DisplayGenres => this.MediaDB.Genres.
            OrderBy(genre => genre.GenreName).
            ThenBy(genre => genre.GenreID).
            ToObservableCollection();

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<PlaylistModel>? DisplayPlaylists => this.MediaDB.Playlists.
            OrderBy(playlist => playlist.PlaylistName).
            ToObservableCollection();

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<TrackModel>? DisplayTracks => this.MediaDB.Tracks.
            OrderBy(track => track.Disc.Album.Artist.ArtistName).
            ThenBy(track => track.Disc.Album.AlbumName).
            ThenBy(track => track.Disc.DiscNum).
            ThenBy(track => track.TrackNum).
            ThenBy(track => track.TrackName).
            ThenBy(track => track.TrackID).
            ToObservableCollection();

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<TrackModel>? DisplayAlbumTracks => this.SelectedAlbum?.Tracks.
            OrderBy(track => track.Disc.DiscNum).
            ThenBy(track => track.TrackNum).
            ThenBy(track => track.TrackName).
            ThenBy(track => track.TrackID).
            ToObservableCollection();

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<TrackModel>? DisplayArtistTracks => this.SelectedArtist?.Albums.SelectMany(album => album.Tracks).
            OrderBy(track => track.Album.AlbumName).
            ThenBy(track => track.Disc.DiscNum).
            ThenBy(track => track.TrackNum).
            ThenBy(track => track.TrackName).
            ThenBy(track => track.TrackID).
            ToObservableCollection();

        /// <summary>
        /// Gets PLACEHOLDER.
        /// </summary>
        public ObservableCollection<TrackModel>? DisplayGenreTracks => this.SelectedGenre?.Tracks.
            OrderBy(track => track.AlbumArtist.ArtistName).
            ThenBy(track => track.Album.AlbumName).
            ThenBy(track => track.Disc.DiscNum).
            ThenBy(track => track.TrackNum).
            ThenBy(track => track.TrackName).
            ThenBy(track => track.TrackID).
            ToObservableCollection();

        #endregion

        #region Command Backed Properties

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
        /// Gets or sets the <see cref="MvxCommand"/> to add a <see cref="GenreModel"/>'s tracks to the <see cref="TrackQueue"/>.
        /// </summary>
        public IMvxCommand AddGenreCommand
        {
            get => this.addGenreCommand ??= new MvxCommand(this.AddGenre);
            set => this.SetProperty(ref this.addGenreCommand, value);
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
        /// Gets or sets the <see cref="MvxCommand"/> to play a <see cref="GenreModel"/>'s tracks.
        /// </summary>
        public IMvxCommand PlayGenreCommand
        {
            get => this.playGenreCommand ??= new MvxCommand(this.PlayGenre);
            set => this.SetProperty(ref this.playGenreCommand, value);
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

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to PLACEHOLDER.
        /// </summary>
        public IMvxCommand ShowAlbumsCommand
        {
            get => this.showAlbumsCommand ??= new MvxCommand(this.ShowAlbums);
            set => this.SetProperty(ref this.showAlbumsCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to PLACEHOLDER.
        /// </summary>
        public IMvxCommand ShowArtistsCommand
        {
            get => this.showArtistsCommand ??= new MvxCommand(this.ShowArtists);
            set => this.SetProperty(ref this.showArtistsCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to PLACEHOLDER.
        /// </summary>
        public IMvxCommand ShowGenresCommand
        {
            get => this.showGenresCommand ??= new MvxCommand(this.ShowGenres);
            set => this.SetProperty(ref this.showGenresCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to PLACEHOLDER.
        /// </summary>
        public IMvxCommand ShowTracksCommand
        {
            get => this.showTracksCommand ??= new MvxCommand(this.ShowTracks);
            set => this.SetProperty(ref this.showTracksCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to PLACEHOLDER.
        /// </summary>
        public IMvxCommand ShowArtistTracksCommand
        {
            get => this.showArtistTracksCommand ??= new MvxCommand(this.ShowArtistTracks);
            set => this.SetProperty(ref this.showArtistTracksCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to PLACEHOLDER.
        /// </summary>
        public IMvxCommand ShowAlbumTracksCommand
        {
            get => this.showAlbumTracksCommand ??= new MvxCommand(this.ShowAlbumTracks);
            set => this.SetProperty(ref this.showAlbumTracksCommand, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to PLACEHOLDER.
        /// </summary>
        public IMvxCommand ShowGenreTracksCommand
        {
            get => this.showGenreTracksCommand ??= new MvxCommand(this.ShowGenreTracks);
            set => this.SetProperty(ref this.showGenreTracksCommand, value);
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        protected void OnClosing()
        {
            this.MediaDB.Dispose();

            // base.OnClosing(e);
        }

        #endregion

        #region Private Command Methods

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

        private void AddTrack()
        {
            this.AddTrack(this.SelectedTrack);
        }

        private void AddGenre()
        {
            this.AddGenre(this.SelectedGenre);
        }

        private void AddAlbum()
        {
            this.AddAlbum(this.SelectedAlbum);
        }

        private void AddArtist()
        {
            this.AddArtist(this.SelectedArtist);
        }

        private void PlayTrack()
        {
            this.PlayTrack(this.SelectedTrack);
        }

        private void PlayGenre()
        {
            this.PlayGenre(this.SelectedGenre);
        }

        private void PlayAlbum()
        {
            this.PlayAlbum(this.SelectedAlbum);
        }

        private void PlayArtist()
        {
            this.PlayArtist(this.SelectedArtist);
        }

        private void ClearQueue()
        {
            this.Close();
            this.TrackQueue.Clear();
            this.ChangeTrack(-1);
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

        private void Stop()
        {
            this.mediaController.Stop();
            this.IsTrackPlaying = false;
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

        private void ShowAlbums()
        {
            this.HideAll();
            this.ShowingAlbums = true;
        }

        private void ShowArtists()
        {
            this.HideAll();
            this.ShowingArtists = true;
        }

        private void ShowGenres()
        {
            this.HideAll();
            this.ShowingGenres = true;
        }

        private void ShowTracks()
        {
            this.HideAll();
            this.ShowingTracks = true;
        }

        private void ShowArtistTracks()
        {
            this.HideAll();
            this.ShowingArtistTracks = true;
        }

        private void ShowAlbumTracks()
        {
            this.HideAll();
            this.ShowingAlbumTracks = true;
        }

        private void ShowGenreTracks()
        {
            this.HideAll();
            this.ShowingGenreTracks = true;
        }

        #endregion

        #region Private Methods
        private void HideAll()
        {
            this.ShowingAlbums = false;
            this.ShowingArtists = false;
            this.ShowingGenres = false;
            this.ShowingTracks = false;
            this.ShowingArtistTracks = false;
            this.ShowingAlbumTracks = false;
            this.ShowingGenreTracks = false;
            this.ShowingPlaylists = false;
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

        private void TimerTick(object? sender, object e)
        {
            if (this.IsTrackLoaded)
            {
                this.PlayerPosition = this.isUserDraggingPosition ? this.PlayerPosition : (int)this.mediaController.Position.TotalSeconds;
            }
            else
            {
                this.PlayerPosition = 0;
            }
        }

        private void AddTrack(TrackModel? track)
        {
            if (track != null)
            {
                this.TrackQueue.Add(track);
            }
        }

        private void AddGenre(GenreModel? genre)
        {
            if (genre != null)
            {
                foreach (TrackModel track in genre.Tracks)
                {
                    this.AddTrack(track);
                }
            }
        }

        private void AddAlbum(AlbumModel? album)
        {
            if (album != null)
            {
                foreach (TrackModel track in album.Tracks)
                {
                    this.AddTrack(track);
                }
            }
        }

        private void AddArtist(ArtistModel? artist)
        {
            if (artist != null)
            {
                foreach (AlbumModel album in artist.Albums)
                {
                    this.AddAlbum(album);
                }
            }
        }

        private void PlayTrack(TrackModel? track)
        {
            this.ClearQueue();
            this.AddTrack(track);
            this.PlayQueue();
        }

        private void PlayGenre(GenreModel? genre)
        {
            this.ClearQueue();
            this.AddGenre(genre);
            this.PlayQueue();
        }

        private void PlayAlbum(AlbumModel? album)
        {
            this.ClearQueue();
            this.AddAlbum(album);
            this.PlayQueue();
        }

        private void PlayArtist(ArtistModel? artist)
        {
            this.ClearQueue();
            this.AddArtist(artist);
            this.PlayQueue();
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

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether PLACEHOLDER.
        /// </summary>
        public bool ShowingPlaylists
        {
            get => this.showingPlaylists;
            set => this.SetProperty(ref this.showingPlaylists, value);
        }

        private IMvxCommand showPlaylistsCommand;

        /// <summary>
        /// Gets or sets the <see cref="MvxCommand"/> to PLACEHOLDER.
        /// </summary>
        public IMvxCommand ShowPlaylistsCommand
        {
            get => this.showPlaylistsCommand ??= new MvxCommand(this.ShowPlaylists);
            set => this.SetProperty(ref this.showPlaylistsCommand, value);
        }

        private void ShowPlaylists()
        {
            this.HideAll();
            this.ShowingPlaylists = true;
        }
    }
}
