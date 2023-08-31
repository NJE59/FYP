// <copyright file="TimerFactory.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.WPF.Classes
{
    using MediaPlayer.Core.Interfaces;

    /// <inheritdoc cref="ITimerFactory" path='/summary'/>
    public class TimerFactory : ITimerFactory
    {
        /// <inheritdoc cref="ITimerFactory.CreateTimer" path='/summary'/>
        public ITimer CreateTimer() => new Timer();
    }
}
