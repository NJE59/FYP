using MediaPlayer.Core;
using MediaPlayer.Core.Interfaces;
using MediaPlayer.WPF.Classes;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Platforms.Wpf.Core;
using Serilog;
using Serilog.Extensions.Logging;

namespace MediaPlayer.WPF
{
    public class Setup : MvxWpfSetup<Core.Application>
    {
        protected override ILoggerProvider? CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory? CreateLogFactory()
        {
            // serilog configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }

        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            Mvx.IoCProvider.RegisterType<ITimerFactory, TimerFactory>();
            Mvx.IoCProvider.RegisterType<IMediaControllerFactory, MediaControllerFactory>();
            base.InitializeFirstChance(iocProvider);
        }
    }
}
