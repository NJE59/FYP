
namespace MediaPlayer.Core.Interfaces
{
    public interface IMediaController
    {
        public bool HasAudio { get; }
        public bool HasVideo { get; }
        public double SpeedRatio { get; set; }
        public double Volume { get; set; }
        public bool HasTimeSpan { get; }
        public TimeSpan Position { get; set; }
        public Uri Source { get; set; }
        public void Pause();
        public void Play();
        public void Stop();
    }
}
