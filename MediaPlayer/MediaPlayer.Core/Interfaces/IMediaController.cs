// <copyright file="IMediaController.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Interfaces
{
    using MvvmCross.Commands;

    /// <summary>
    /// PLACEHOLDER.
    /// </summary>
    public interface IMediaController
    {
        /// <summary>
        /// Gets a value indicating whether PLACEHOLDER.
        /// </summary>
        public bool HasAudio { get; }

        /// <summary>
        /// Gets a value indicating whether PLACEHOLDER.
        /// </summary>
        public bool HasVideo { get; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public double SpeedRatio { get; set; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// Gets a value indicating whether PLACEHOLDER.
        /// </summary>
        public bool HasTimeSpan { get; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public TimeSpan Position { get; set; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public Uri Source { get; set; }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public IMvxCommand MediaEndedCommand { get; set; }

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public void Pause();

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public void Play();

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public void Stop();
    }
}
