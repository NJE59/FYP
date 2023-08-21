using MediaPlayer.Core.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MediaPlayer.WPF.Classes
{
    public class MediaController : IMediaController
    {
        MediaElement mediaElement { get; set; }

        public bool HasAudio { get => mediaElement.HasAudio; }
        public bool HasVideo { get => mediaElement.HasVideo; }
        public double SpeedRatio
        {
            get => mediaElement.SpeedRatio;
            set => mediaElement.SpeedRatio = value;
        }
        public double Volume
        {
            get => mediaElement.Volume;
            set => mediaElement.Volume = value;
        }
        public bool HasTimeSpan {  get => mediaElement.NaturalDuration.HasTimeSpan; }
        public TimeSpan Position
        {
            get => mediaElement.Position;
            set => mediaElement.Position = value;
        }
        public Uri Source
        {
            get => mediaElement.Source;
            set => mediaElement.Source = value;
        }
        public void Pause()
        {
            mediaElement.Pause();
        }
        public void Play()
        {
            mediaElement.Play();
        }
        public void Stop()
        {
            mediaElement.Stop();
        }
        public MediaController()
        {
            mediaElement = new MediaElement();
            mediaElement.LoadedBehavior = MediaState.Play;
            mediaElement.UnloadedBehavior = MediaState.Manual;
            mediaElement.Volume = 0.5D;
        }
    }
}
