// <copyright file="ITimer.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Interfaces
{
    /// <summary>
    /// Interface describing the necessary functionality of a background timer, to enable use of the platform specific version in the ViewModel.
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// event that fires whenever the timer ticks.
        /// </summary>
        event EventHandler Tick;

        /// <summary>
        /// Gets or sets the interval between ticks.
        /// </summary>
        TimeSpan Interval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the timer is currnetly enabled.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Function to start the timer.
        /// </summary>
        void Start();

        /// <summary>
        /// Function to start the timer.
        /// </summary>
        void Stop();
    }
}
