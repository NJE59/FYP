// <copyright file="MediaController.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.WPF.Classes
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using MediaPlayer.Core.Interfaces;
    using MvvmCross.Commands;

    /// <summary>
    /// PLACEHOLDER.
    /// </summary>
    public class MediaController : IMediaController
    {
        private readonly MediaElement mediaElement;

        private IMvxCommand mediaEndedCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaController"/> class.
        /// </summary>
        /// <param name="mediaEndedCommand">PLACEHOLDER.</param>
        public MediaController(IMvxCommand mediaEndedCommand)
        {
            this.mediaElement = new ()
            {
                LoadedBehavior = MediaState.Play,
                UnloadedBehavior = MediaState.Manual,
                Volume = 0.5D,
            };
            this.mediaEndedCommand = mediaEndedCommand;
            this.mediaElement.MediaEnded += new RoutedEventHandler(this.MediaElement_MediaEnded);
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
        public Uri? Source
        {
            get => this.mediaElement.Source;
            set => this.mediaElement.Source = value;
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand MediaEndedCommand
        {
            get => this.mediaEndedCommand;
            set => this.mediaEndedCommand = value;
        }

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public void Close() => this.mediaElement.Close();

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

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.MediaEndedCommand.Execute();
        }
    }
}
