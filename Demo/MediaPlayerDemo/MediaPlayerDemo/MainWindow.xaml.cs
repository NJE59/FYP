using MediaPlayerDemo.MediaClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Track = MediaPlayerDemo.MediaClass.Track;

namespace MediaPlayerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Track> Tracks = new ObservableCollection<Track>();
        ObservableCollection<Album> Albums = new ObservableCollection<Album>();
        ObservableCollection<Artist> Artists = new ObservableCollection<Artist>();
        ObservableCollection<Playlist> Playlists = new ObservableCollection<Playlist>();
        Dictionary<ListView, CollectionView> views = new Dictionary<ListView, CollectionView>();


        public MainWindow()
        {
            InitializeComponent();

            


            Tracks.Add(new Track("The Whirlwind",  5, "V. Out Of The Night",                                  "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",  6, "VI. Rose Colored Glasses",                             "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",  7, "VII. Evermore",                                        "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",  1, "I. Overture [Instrumental] / Whirlwind",               "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",  2, "II.The Wind Blew Them All Away",                       "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",  3, "III. On The Prowl",                                    "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",  4, "IV. A Man Can Feel",                                   "Transatlantic"));
            Tracks.Add(new Track("Octavarium",     4, "I Walk Beside You",                                    "Dream Theater"));
            Tracks.Add(new Track("Octavarium",     5, "Panic Attack",                                         "Dream Theater"));
            Tracks.Add(new Track("Octavarium",     6, "Never Enough",                                         "Dream Theater"));
            Tracks.Add(new Track("Octavarium",     7, "Sacrificed Sons",                                      "Dream Theater"));
            Tracks.Add(new Track("The Whirlwind",  8, "VIII. Set Us Free",                                    "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",  9, "IX. Lay Down Your Life",                               "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind", 10, "X. Pieces Of Heaven [Instrumental]",                   "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind", 11, "XI. Is It Really Happening?",                          "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind", 12, "XII. Dancing With Eternal Glory / Whirwind (Reprise)", "Transatlantic"));
            Tracks.Add(new Track("Octavarium",     1, "The Root Of All Evil",                                 "Dream Theater"));
            Tracks.Add(new Track("Octavarium",     2, "The Answer Lies Within",                               "Dream Theater"));
            Tracks.Add(new Track("Octavarium",     3, "These Walls",                                          "Dream Theater"));
            Tracks.Add(new Track("Octavarium",     8, "Octavarium",                                           "Dream Theater"));



            Albums.Add(new Album("The Whirlwind"));
            Albums.Add(new Album("Octavarium"));

            Artists.Add(new Artist("Transatlantic"));
            Artists.Add(new Artist("Dream Theater"));

            Playlists.Add(new Playlist("playlist 1"));
            Playlists.Add(new Playlist("new playlist"));

            lbPlaylists.ItemsSource = Playlists;
            lvSongs.ItemsSource = Tracks;
            lvAlbums.ItemsSource = Albums;
            lvArtists.ItemsSource = Artists;
            lvPlaylists.ItemsSource = Playlists;

            CollectionView songsView     = (CollectionView)CollectionViewSource.GetDefaultView(lvSongs.ItemsSource);
            CollectionView albumsView    = (CollectionView)CollectionViewSource.GetDefaultView(lvAlbums.ItemsSource);
            CollectionView artistsView   = (CollectionView)CollectionViewSource.GetDefaultView(lvArtists.ItemsSource);
            CollectionView playlistsView = (CollectionView)CollectionViewSource.GetDefaultView(lvPlaylists.ItemsSource);
            views.Add(lvSongs, songsView);
            views.Add(lvAlbums, albumsView);
            views.Add(lvArtists, artistsView);
            views.Add(lvPlaylists, playlistsView);

            views[lvSongs].SortDescriptions.Add(new SortDescription("Artist", ListSortDirection.Ascending));
            views[lvSongs].SortDescriptions.Add(new SortDescription("Album", ListSortDirection.Ascending));
            views[lvSongs].SortDescriptions.Add(new SortDescription("SongNo", ListSortDirection.Ascending));
            views[lvAlbums].SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            views[lvArtists].SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            views[lvPlaylists].SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));



        }

        private void btnSongs_Click(object sender, RoutedEventArgs e)
        {
            lvAlbums.Visibility = Visibility.Collapsed;
            lvArtists.Visibility = Visibility.Collapsed;
            lvPlaylists.Visibility = Visibility.Collapsed;
            lvSongs.Visibility = Visibility.Visible;
        }

        private void btnAlbums_Click(object sender, RoutedEventArgs e)
        {
            lvSongs.Visibility = Visibility.Collapsed;
            lvArtists.Visibility = Visibility.Collapsed;
            lvPlaylists.Visibility = Visibility.Collapsed;
            lvAlbums.Visibility = Visibility.Visible;
        }

        private void btnArtists_Click(object sender, RoutedEventArgs e)
        {
            lvSongs.Visibility = Visibility.Collapsed;
            lvAlbums.Visibility = Visibility.Collapsed;
            lvPlaylists.Visibility = Visibility.Collapsed;
            lvArtists.Visibility = Visibility.Visible;
        }

        private void btnPlaylists_Click(object sender, RoutedEventArgs e)
        {
            lvSongs.Visibility = Visibility.Collapsed;
            lvAlbums.Visibility = Visibility.Collapsed;
            lvArtists.Visibility = Visibility.Collapsed;
            lvPlaylists.Visibility = Visibility.Visible;
        }

        private void btnAlbum_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnArtist_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
