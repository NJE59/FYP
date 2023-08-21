using MediaPlayer.Core.Interfaces;
using System;
using System.Windows.Threading;

namespace MediaPlayer.WPF.Classes
{
    public class Timer : ITimer
    {
        private readonly DispatcherTimer internalTimer;
        
        public Timer()
        {
            internalTimer = new DispatcherTimer();
        }

        public bool IsEnabled
        {
            get => internalTimer.IsEnabled;
            set => internalTimer.IsEnabled = value;
        }

        public TimeSpan Interval
        {
            get => internalTimer.Interval;
            set => internalTimer.Interval = value;
        }

        public void Start() => internalTimer.Start();
        public void Stop() => internalTimer.Stop();

        public event EventHandler Tick
        {
            add => internalTimer.Tick += value;
            remove => internalTimer.Tick -= value;
        }
    }
}
