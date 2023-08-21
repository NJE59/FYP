using MediaPlayer.Core.Interfaces;

namespace MediaPlayer.WPF.Classes
{
    public class TimerFactory : ITimerFactory
    {
        public ITimer CreateTimer() => new Timer();
    }
}
