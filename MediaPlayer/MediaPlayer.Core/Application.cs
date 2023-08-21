// <copyright file="Application.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core
{
    using MediaPlayer.Core.ViewModels;
    using MvvmCross.ViewModels;

    /// <summary>
    /// PLACEHOLDER.
    /// </summary>
    public class Application : MvxApplication
    {
        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public override void Initialize()
        {
            this.RegisterAppStart<MediaLibraryViewModel>();
        }
    }
}
