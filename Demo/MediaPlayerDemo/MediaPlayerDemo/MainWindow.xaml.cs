using MediaPlayerDemo.MediaClass;
using Microsoft.Win32;
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
        Dictionary<TextBox, string> searchHints = new Dictionary<TextBox, string>();
        Dictionary<TextBox, ListView> searchFilter = new Dictionary<TextBox, ListView>();
        Dictionary<Button, UIElement[]> visibleViews = new Dictionary<Button, UIElement[]>();

        public MainWindow()
        {
            InitializeComponent();


            searchHints.Add(txtTracks, "Search songs");
            searchHints.Add(txtAlbums, "Search albums");
            searchHints.Add(txtArtists, "Search artists");
            searchHints.Add(txtPlaylists, "Search playlists");

            

            searchFilter.Add(txtTracks, lvTracks);
            searchFilter.Add(txtAlbums, lvAlbums);
            searchFilter.Add(txtArtists, lvArtists);
            searchFilter.Add(txtPlaylists, lvPlaylists);

            UIElement[] trackView = { lvTracks, txtTracks };
            UIElement[] albumView = { lvAlbums, txtAlbums };
            UIElement[] artistView = { lvArtists, txtArtists };
            UIElement[] playlistView = { lvPlaylists, txtPlaylists };
            visibleViews.Add(btnTracks, trackView);
            visibleViews.Add(btnAlbums, albumView);
            visibleViews.Add(btnArtists, artistView);
            visibleViews.Add(btnPlaylists, playlistView);


            Tracks.Add(new Track("The Whirlwind",         5, "V. Out Of The Night",                                  "Transatlantic"));
            Tracks.Add(new Track("Bridge Across Forever", 4, "Stranger In Your Soul",                                "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",         6, "VI. Rose Colored Glasses",                             "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",         7, "VII. Evermore",                                        "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",         1, "I. Overture [Instrumental] / Whirlwind",               "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",         2, "II.The Wind Blew Them All Away",                       "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",         3, "III. On The Prowl",                                    "Transatlantic"));
            Tracks.Add(new Track("Bridge Across Forever", 2, "Suite Charlotte Pike",                                 "Transatlantic"));
            Tracks.Add(new Track("Bridge Across Forever", 3, "Bridge Across Forever",                                "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",         4, "IV. A Man Can Feel",                                   "Transatlantic"));
            Tracks.Add(new Track("Octavarium",            4, "I Walk Beside You",                                    "Dream Theater"));
            Tracks.Add(new Track("Images And Words",      3, "Take The Time",                                        "Dream Theater"));
            Tracks.Add(new Track("Images And Words",      4, "Surrounded",                                           "Dream Theater"));
            Tracks.Add(new Track("Images And Words",      5, "Metropolis",                                           "Dream Theater"));
            Tracks.Add(new Track("Images And Words",      6, "Under A Glass Moon",                                   "Dream Theater"));
            Tracks.Add(new Track("Images And Words",      7, "Wait For Sleep",                                       "Dream Theater"));
            Tracks.Add(new Track("Images And Words",      8, "Learning To Live",                                     "Dream Theater"));
            Tracks.Add(new Track("Octavarium",            5, "Panic Attack",                                         "Dream Theater"));
            Tracks.Add(new Track("Octavarium",            6, "Never Enough",                                         "Dream Theater"));
            Tracks.Add(new Track("Octavarium",            7, "Sacrificed Sons",                                      "Dream Theater"));
            Tracks.Add(new Track("The Whirlwind",         8, "VIII. Set Us Free",                                    "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",         9, "IX. Lay Down Your Life",                               "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",        10, "X. Pieces Of Heaven [Instrumental]",                   "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",        11, "XI. Is It Really Happening?",                          "Transatlantic"));
            Tracks.Add(new Track("The Whirlwind",        12, "XII. Dancing With Eternal Glory / Whirwind (Reprise)", "Transatlantic"));
            Tracks.Add(new Track("Images And Words",      1, "Pull Me Under",                                        "Dream Theater"));
            Tracks.Add(new Track("Images And Words",      2, "Another Day",                                          "Dream Theater"));
            Tracks.Add(new Track("Octavarium",            1, "The Root Of All Evil",                                 "Dream Theater"));
            Tracks.Add(new Track("Bridge Across Forever", 1, "Duel With The Devil",                                  "Transatlantic"));
            Tracks.Add(new Track("Octavarium",            2, "The Answer Lies Within",                               "Dream Theater"));
            Tracks.Add(new Track("Octavarium",            3, "These Walls",                                          "Dream Theater"));
            Tracks.Add(new Track("Octavarium",            8, "Octavarium",                                           "Dream Theater"));



            Albums.Add(new Album("The Whirlwind"));
            Albums.Add(new Album("Bridge Across Forever"));
            Albums.Add(new Album("Images And Words"));
            Albums.Add(new Album("Octavarium"));

            Artists.Add(new Artist("Transatlantic"));
            Artists.Add(new Artist("Dream Theater"));

            Playlists.Add(new Playlist("playlist 1"));
            Playlists.Add(new Playlist("new playlist"));

            lbPlaylists.ItemsSource = Playlists;
            lvTracks.ItemsSource = Tracks;
            lvAlbums.ItemsSource = Albums;
            lvArtists.ItemsSource = Artists;
            lvPlaylists.ItemsSource = Playlists;

            CollectionView songsView     = (CollectionView)CollectionViewSource.GetDefaultView(lvTracks.ItemsSource);
            CollectionView albumsView    = (CollectionView)CollectionViewSource.GetDefaultView(lvAlbums.ItemsSource);
            CollectionView artistsView   = (CollectionView)CollectionViewSource.GetDefaultView(lvArtists.ItemsSource);
            CollectionView playlistsView = (CollectionView)CollectionViewSource.GetDefaultView(lvPlaylists.ItemsSource);
            views.Add(lvTracks, songsView);
            views.Add(lvAlbums, albumsView);
            views.Add(lvArtists, artistsView);
            views.Add(lvPlaylists, playlistsView);

            views[lvTracks].SortDescriptions.Add(new SortDescription("Artist", ListSortDirection.Ascending));
            views[lvTracks].SortDescriptions.Add(new SortDescription("Album", ListSortDirection.Ascending));
            views[lvTracks].SortDescriptions.Add(new SortDescription("TrackNo", ListSortDirection.Ascending));
            views[lvAlbums].SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            views[lvArtists].SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            views[lvPlaylists].SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            views[lvTracks].Filter = TrackFilter;
            views[lvAlbums].Filter = AlbumFilter;
            views[lvArtists].Filter = ArtistFilter;
            views[lvPlaylists].Filter = PlaylistFilter;

            foreach (var item in searchHints)
            {
                item.Key.Text = item.Value;
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in visibleViews)
            {
                if (item.Key != (sender as Button))
                {
                    item.Value[0].Visibility = Visibility.Collapsed;
                    (item.Value[1] as TextBox).Foreground = Brushes.Gray;
                    (item.Value[1] as TextBox).Text = searchHints[(item.Value[1] as TextBox)];
                    (item.Value[1] as TextBox).FontStyle = FontStyles.Italic;
                    item.Value[1].Visibility = Visibility.Collapsed;
                }
            }

            visibleViews[(sender as Button)][0].Visibility = Visibility.Visible;
            visibleViews[(sender as Button)][1].Visibility = Visibility.Visible;
        }

        private void btnAlbum_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnArtist_Click(object sender, RoutedEventArgs e)
        {

        }

        private bool TrackFilter(object item)
        {
            if (String.IsNullOrEmpty(txtTracks.Text) || txtTracks.Foreground == Brushes.Gray)
                return true;
            else
                return ((item as Track)).Album.IndexOf(txtTracks.Text, StringComparison.OrdinalIgnoreCase) >= 0
                    || ((item as Track)).TrackNo.ToString().IndexOf(txtTracks.Text, StringComparison.OrdinalIgnoreCase) >= 0
                    || ((item as Track)).Title.IndexOf(txtTracks.Text, StringComparison.OrdinalIgnoreCase) >= 0
                    || ((item as Track)).Artist.IndexOf(txtTracks.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool AlbumFilter(object item)
        {
            if (String.IsNullOrEmpty(txtAlbums.Text) || txtAlbums.Foreground == Brushes.Gray)
                return true;
            else
                return ((item as Album)).Name.IndexOf(txtAlbums.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool ArtistFilter(object item)
        {
            if (String.IsNullOrEmpty(txtArtists.Text) || txtArtists.Foreground == Brushes.Gray)
                return true;
            else
                return ((item as Artist)).Name.IndexOf(txtArtists.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool PlaylistFilter(object item)
        {
            if (String.IsNullOrEmpty(txtPlaylists.Text) || txtPlaylists.Foreground == Brushes.Gray)
                return true;
            else
                return ((item as Playlist)).Name.IndexOf(txtPlaylists.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }




        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(searchFilter[sender as TextBox].ItemsSource).Refresh();
        }

        private void txtPlaylists_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SearchBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if((sender as TextBox).Foreground == Brushes.Gray) {
                (sender as TextBox).Foreground = Brushes.Black;
                (sender as TextBox).Text = "";
                (sender as TextBox).FontStyle = FontStyles.Normal;
            } 
            else
            {
                (sender as TextBox).SelectAll();
            }
        }

        private void SearchBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (String.IsNullOrEmpty((sender as TextBox).Text))
            {
                (sender as TextBox).Foreground = Brushes.Gray;
                (sender as TextBox).Text = searchHints[(sender as TextBox)];
                (sender as TextBox).FontStyle = FontStyles.Italic;
            }            
        }

       
    }
}
