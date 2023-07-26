using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MediaPlayer.Classes;
using MediaPlayer.Pages;
using MenuItem = MediaPlayer.Classes.MenuItem;

namespace MediaPlayer.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// <see cref="ObservableCollection{T}"/> of <see cref="MenuItem">MenuItems</see> for populating <see cref="tvNav">tvNav</see> on the <see cref="MainWindow"/>
        /// </summary>
        ObservableCollection<MenuItem> NavItems;

        public MainWindow()
        {
            InitializeComponent();
            CreateNavItems();
            tvNav.ItemsSource = NavItems;
        }

        /// <summary>
        /// Method for populating <see cref="NavItems"/>
        /// </summary>
        private void CreateNavItems()
        {
            NavItems = new ObservableCollection<MenuItem>();
            MenuItem mniMusic = new MenuItem("Music", typeof(TracksPage));
            mniMusic.Children.Add(new MenuItem("Album", typeof(AlbumsPage)));
            mniMusic.Children.Add(new MenuItem("Artist", typeof(ArtistsPage)));
            mniMusic.Children.Add(new MenuItem("Genre", typeof(GenresPage)));
            mniMusic.Children.Add(new MenuItem("Year", typeof(YearsPage)));
            MenuItem mniPlaylists = new MenuItem("Playlists", typeof(PlaylistsPage));
            mniPlaylists.Children.Add(new MenuItem("Playlist1", typeof(SinglePlaylistPage)));
            mniPlaylists.Children.Add(new MenuItem("New Playlist", typeof(SinglePlaylistPage)));
            NavItems.Add(mniMusic);
            NavItems.Add(mniPlaylists);
        }

        private void tvNav_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(tvNav.SelectedItem != null)
            {
                Type pageType = ((MenuItem)tvNav.SelectedItem).PageType;
                MainView.Content = (Page)Activator.CreateInstance(pageType);
            }
        }
    }
}
