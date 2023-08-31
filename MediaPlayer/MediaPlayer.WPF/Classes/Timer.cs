// <copyright file="Timer.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.WPF.Classes
{
    using System;
    using System.Windows.Threading;
    using MediaPlayer.Core.Interfaces;

    /// <inheritdoc cref="ITimer" path='/summary'/>
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

        /// <inheritdoc cref="ITimer.Tick" path='/summary'/>
        public event EventHandler Tick
        {
            add => this.internalTimer.Tick += value;
            remove => this.internalTimer.Tick -= value;
        }

        /// <inheritdoc cref="ITimer.IsEnabled" path='/summary'/>
        public bool IsEnabled
        {
            get => this.internalTimer.IsEnabled;
            set => this.internalTimer.IsEnabled = value;
        }

        /// <inheritdoc cref="ITimer.Interval" path='/summary'/>
        public TimeSpan Interval
        {
            get => this.internalTimer.Interval;
            set => this.internalTimer.Interval = value;
        }

        /// <inheritdoc cref="ITimer.Start" path='/summary'/>
        public void Start() => this.internalTimer.Start();

        /// <inheritdoc cref="ITimer.Stop" path='/summary'/>
        public void Stop() => this.internalTimer.Stop();
    }
}
