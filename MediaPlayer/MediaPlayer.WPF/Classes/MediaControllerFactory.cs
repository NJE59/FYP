// <copyright file="MediaControllerFactory.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.WPF.Classes
{
    using MediaPlayer.Core.Interfaces;

    /// <summary>
    /// PLACEHOLDER.
    /// </summary>
    public class MediaControllerFactory : IMediaControllerFactory
    {
        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        /// <returns><inheritdoc cref="MediaController" path='/summary'/></returns>
        public IMediaController CreateMediaController() => new MediaController();
    }
}
