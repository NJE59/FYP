// <copyright file="IMediaControllerFactory.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Interfaces
{
    using MvvmCross.Commands;

    /// <summary>
    /// PLACEHOLDER.
    /// </summary>
    public interface IMediaControllerFactory
    {
        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        /// <param name="mediaEndedCommand">PLACEHOLDER1.</param>
        /// <returns>PLACEHOLDER2.</returns>
        IMediaController CreateMediaController(IMvxCommand mediaEndedCommand);
    }
}
