// <copyright file="Setup.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.WPF
{
    using MediaPlayer.Core.Interfaces;
    using MediaPlayer.WPF.Classes;
    using Microsoft.Extensions.Logging;
    using MvvmCross;
    using MvvmCross.IoC;
    using MvvmCross.Platforms.Wpf.Core;
    using Serilog;
    using Serilog.Extensions.Logging;

    /// <inheritdoc cref="MvxWpfSetup" path='/summary'/>
    public class Setup : MvxWpfSetup<Core.Application>
    {
        /// <summary>
        /// Creates the log provider.
        /// </summary>
        /// <returns><inheritdoc cref="SerilogLoggerProvider" path='/summary'/></returns>
        protected override ILoggerProvider? CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        /// <summary>
        /// Creates the log factory.
        /// </summary>
        /// <returns><inheritdoc cref="SerilogLoggerFactory" path='/summary'/></returns>
        protected override ILoggerFactory? CreateLogFactory()
        {
            // serilog configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }

        /// <summary>
        /// Sets the associations between <see cref="ITimerFactory"/> and <see cref="TimerFactory"/> and <see cref="IMediaControllerFactory"/> and <see cref="MediaControllerFactory"/> for Inversion of Control.
        /// </summary>
        /// <param name="iocProvider"><inheritdoc cref="Mvx.IoCProvider" path='/summary'/></param>
        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Mvx.IoCProvider.RegisterType<ITimerFactory, TimerFactory>();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Mvx.IoCProvider.RegisterType<IMediaControllerFactory, MediaControllerFactory>();
            base.InitializeFirstChance(iocProvider);
        }
    }
}
