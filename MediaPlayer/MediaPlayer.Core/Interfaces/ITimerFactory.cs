// <copyright file="ITimerFactory.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Interfaces
{
    /// <summary>
    /// Interface describing a class to enable the creation of multiple <see cref="ITimer">ITimers</see>.
    /// </summary>
    public interface ITimerFactory
    {
        /// <summary>
        /// function to create a new instance of a class implementing the <see cref="ITimer"/> inteface.
        /// </summary>
        /// <returns>A new instance of a class implementing the <see cref="ITimer"/> interface.</returns>
        ITimer CreateTimer();
    }
}
