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

    /// <inheritdoc cref="IMediaController" path='/summary'/>
    public class MediaController : IMediaController
    {
        private readonly MediaElement mediaElement;

        private IMvxCommand mediaEndedCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaController"/> class.
        /// </summary>
        /// <param name="mediaEndedCommand"><inheritdoc cref="IMediaController.MediaEndedCommand" path='/summary'/></param>
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

        /// <inheritdoc cref="IMediaController.HasAudio" path='/summary'/>
        public bool HasAudio => this.mediaElement.HasAudio;

        /// <inheritdoc cref="IMediaController.HasVideo" path='/summary'/>
        public bool HasVideo => this.mediaElement.HasVideo;

        /// <inheritdoc cref="IMediaController.SpeedRatio" path='/summary'/>
        public double SpeedRatio
        {
            get => this.mediaElement.SpeedRatio;
            set => this.mediaElement.SpeedRatio = value;
        }

        /// <inheritdoc cref="IMediaController.Volume" path='/summary'/>
        public double Volume
        {
            get => this.mediaElement.Volume;
            set => this.mediaElement.Volume = value;
        }

        /// <inheritdoc cref="IMediaController.HasTimeSpan" path='/summary'/>
        public bool HasTimeSpan => this.mediaElement.NaturalDuration.HasTimeSpan;

        /// <inheritdoc cref="IMediaController.Position" path='/summary'/>
        public TimeSpan Position
        {
            get => this.mediaElement.Position;
            set => this.mediaElement.Position = value;
        }

        /// <inheritdoc cref="IMediaController.Source" path='/summary'/>
        public Uri? Source
        {
            get => this.mediaElement.Source;
            set => this.mediaElement.Source = value;
        }

        /// <inheritdoc cref="IMediaController.MediaEndedCommand" path='/summary'/>
        public IMvxCommand MediaEndedCommand
        {
            get => this.mediaEndedCommand;
            set => this.mediaEndedCommand = value;
        }

        /// <inheritdoc cref="IMediaController.Close" path='/summary'/>
        public void Close() => this.mediaElement.Close();

        /// <inheritdoc cref="IMediaController.Pause" path='/summary'/>
        public void Pause() => this.mediaElement.Pause();

        /// <inheritdoc cref="IMediaController.Play" path='/summary'/>
        public void Play() => this.mediaElement.Play();

        /// <inheritdoc cref="IMediaController.Stop" path='/summary'/>
        public void Stop() => this.mediaElement.Stop();

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.MediaEndedCommand.Execute();
        }
    }
}
