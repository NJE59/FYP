// <copyright file="TimerFactory.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.WPF.Classes
{
    using MediaPlayer.Core.Interfaces;

    /// <summary>
    /// PLACEHOLDER.
    /// </summary>
    public class TimerFactory : ITimerFactory
    {
        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        /// <returns><inheritdoc cref="Timer" path='/summary'/></returns>
        public ITimer CreateTimer() => new Timer();
    }
}
