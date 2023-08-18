using MediaPlayer.Core;
using Microsoft.Extensions.Logging;
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
    }
}
