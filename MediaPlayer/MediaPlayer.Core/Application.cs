// <copyright file="Application.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core
{
    using MediaPlayer.Core.ViewModels;
    using MvvmCross.ViewModels;

    /// <summary>
    /// Class describing an implementatino of the <see cref="MvxApplication"/> interface.
    /// </summary>
    public class Application : MvxApplication
    {
        /// <inheritdoc cref="MvxApplication.Initialize" path='/summary'/>
        public override void Initialize()
        {
            this.RegisterAppStart<MediaLibraryViewModel>();
        }
    }
}
