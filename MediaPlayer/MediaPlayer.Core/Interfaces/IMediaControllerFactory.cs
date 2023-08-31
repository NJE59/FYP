// <copyright file="IMediaControllerFactory.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Interfaces
{
    using MvvmCross.Commands;

    /// <summary>
    /// Interface describing a class to enable the creation of multiple <see cref="IMediaController">IMediaControllers</see>.
    /// </summary>
    public interface IMediaControllerFactory
    {
        /// <summary>
        /// function to create a new instance of a class implementing the <see cref="IMediaController"/> inteface.
        /// </summary>
        /// <param name="mediaEndedCommand"><inheritdoc cref="IMediaController.MediaEndedCommand" path='/summary'/></param>
        /// <returns>A new instance of a class implementing the <see cref="IMediaController"/> interface.</returns>
        IMediaController CreateMediaController(IMvxCommand mediaEndedCommand);
    }
}
