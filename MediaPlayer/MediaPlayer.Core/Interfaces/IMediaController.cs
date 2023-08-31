// <copyright file="IMediaController.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Interfaces
{
    using MvvmCross.Commands;

    /// <summary>
    /// Interface describing the necessary functionality of a media player, to enable use of the platform specific version in the ViewModel.
    /// </summary>
    public interface IMediaController
    {
        /// <summary>
        /// Gets a value indicating whether this media controller contains audio.
        /// </summary>
        public bool HasAudio { get; }

        /// <summary>
        /// Gets a value indicating whether this media controller contains video.
        /// </summary>
        public bool HasVideo { get; }

        /// <summary>
        /// Gets or sets the speed of playback of this media controller.
        /// </summary>
        public double SpeedRatio { get; set; }

        /// <summary>
        /// Gets or sets the volume of this media controller.
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// Gets a value indicating whether this media controller has loaded content with a <see cref="TimeSpan"/> - ie whether there is anything currently loaded.
        /// </summary>
        public bool HasTimeSpan { get; }

        /// <summary>
        /// Gets or sets the current position in the song currently beign played by this media controller.
        /// </summary>
        public TimeSpan Position { get; set; }

        /// <summary>
        /// Gets or sets the source of the media to load and play in this media controller.
        /// </summary>
        public Uri? Source { get; set; }

        /// <summary>
        /// Gets or sets the command to fire when the song ends.
        /// </summary>
        public IMvxCommand MediaEndedCommand { get; set; }

        /// <summary>
        /// Function to empty the source of this media controller.
        /// </summary>
        public void Close();

        /// <summary>
        /// Function to pause the song currently playing in this media controller.
        /// </summary>
        public void Pause();

        /// <summary>
        /// Function to play the song currently loaded into this media controller.
        /// </summary>
        public void Play();

        /// <summary>
        /// Function to stop the song currently playing in this media controller.
        /// </summary>
        public void Stop();
    }
}
