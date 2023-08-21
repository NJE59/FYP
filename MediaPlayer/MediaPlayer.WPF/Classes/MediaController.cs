// <copyright file="MediaController.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.WPF.Classes
{
    using System;
    using System.Windows.Controls;
    using MediaPlayer.Core.Interfaces;

    /// <summary>
    /// PLACEHOLDER.
    /// </summary>
    public class MediaController : IMediaController
    {
        private readonly MediaElement mediaElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaController"/> class.
        /// </summary>
        public MediaController()
        {
            this.mediaElement = new ();
            this.mediaElement.LoadedBehavior = MediaState.Play;
            this.mediaElement.UnloadedBehavior = MediaState.Manual;
            this.mediaElement.Volume = 0.5D;
        }

        /// <summary>
        /// Gets a value indicating whether PLACEHOLDER.
        /// </summary>
        public bool HasAudio => this.mediaElement.HasAudio;

        /// <summary>
        /// Gets a value indicating whether PLACEHOLDER.
        /// </summary>
        public bool HasVideo => this.mediaElement.HasVideo;

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public double SpeedRatio
        {
            get => this.mediaElement.SpeedRatio;
            set => this.mediaElement.SpeedRatio = value;
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public double Volume
        {
            get => this.mediaElement.Volume;
            set => this.mediaElement.Volume = value;
        }

        /// <summary>
        /// Gets a value indicating whether PLACEHOLDER.
        /// </summary>
        public bool HasTimeSpan => this.mediaElement.NaturalDuration.HasTimeSpan;

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public TimeSpan Position
        {
            get => this.mediaElement.Position;
            set => this.mediaElement.Position = value;
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public Uri Source
        {
            get => this.mediaElement.Source;
            set => this.mediaElement.Source = value;
        }

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public void Pause() => this.mediaElement.Pause();

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public void Play() => this.mediaElement.Play();

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public void Stop() => this.mediaElement.Stop();
    }
}
