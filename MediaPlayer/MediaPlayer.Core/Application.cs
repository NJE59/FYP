using MediaPlayer.Core.Interfaces;
using MediaPlayer.Core.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;
namespace MediaPlayer.Core
{
    public class Application : MvxApplication
    {

        public override void Initialize()
        {
            RegisterAppStart<MediaLibraryViewModel>();
        }
    }
}
