using MediaPlayer.Core.Interfaces;

namespace MediaPlayer.WPF.Classes
{
    public class MediaControllerFactory : IMediaControllerFactory
    {
        public IMediaController CreateMediaController()
        {
            return new MediaController();
        }
    }
}
