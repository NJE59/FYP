// <copyright file="MediaControllerFactory.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.WPF.Classes
{
    using MediaPlayer.Core.Interfaces;
    using MvvmCross.Commands;

    /// <inheritdoc cref="IMediaControllerFactory" path='/summary'/>
    public class MediaControllerFactory : IMediaControllerFactory
    {
        /// <inheritdoc cref="IMediaControllerFactory.CreateMediaController(IMvxCommand)" path='/summary'/>
        public IMediaController CreateMediaController(IMvxCommand mediaEndedCommand) => new MediaController(mediaEndedCommand);
    }
}
