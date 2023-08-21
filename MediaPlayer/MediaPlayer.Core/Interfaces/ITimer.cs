// <copyright file="ITimer.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Interfaces
{
    /// <summary>
    /// PLACEHOLDER.
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// PLCEHOLDER.
        /// </summary>
        event EventHandler Tick;

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        TimeSpan Interval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether PLACEHOLDER.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        void Start();

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        void Stop();
    }
}
