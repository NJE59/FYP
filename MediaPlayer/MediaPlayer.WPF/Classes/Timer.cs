// <copyright file="Timer.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.WPF.Classes
{
    using System;
    using System.Windows.Threading;
    using MediaPlayer.Core.Interfaces;

    /// <summary>
    /// PLACEHOLDER.
    /// </summary>
    public class Timer : ITimer
    {
        private readonly DispatcherTimer internalTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        public Timer()
        {
            this.internalTimer = new DispatcherTimer();
        }

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public event EventHandler Tick
        {
            add => this.internalTimer.Tick += value;
            remove => this.internalTimer.Tick -= value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether PLACEHOLDER.
        /// </summary>
        public bool IsEnabled
        {
            get => this.internalTimer.IsEnabled;
            set => this.internalTimer.IsEnabled = value;
        }

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public TimeSpan Interval
        {
            get => this.internalTimer.Interval;
            set => this.internalTimer.Interval = value;
        }

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public void Start() => this.internalTimer.Start();

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public void Stop() => this.internalTimer.Stop();
    }
}
