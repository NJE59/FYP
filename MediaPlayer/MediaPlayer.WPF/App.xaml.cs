// <copyright file="App.xaml.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.WPF
{
    using MvvmCross.Core;
    using MvvmCross.Platforms.Wpf.Views;

    /// <inheritdoc cref="MvxApplication" path='/summary'/>
    public partial class App : MvxApplication
    {
        /// <inheritdoc cref="MvxApplication.RegisterSetup" path='/summary'/>
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<Setup>();
        }
    }
}
