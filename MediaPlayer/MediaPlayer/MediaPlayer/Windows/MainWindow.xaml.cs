using MediaPlayer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Windows.Storage;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Media.Control;
using Windows.Storage.FileProperties;

namespace MediaPlayer.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MediaDBContext _context = new MediaDBContext();

        private CollectionViewSource artistsViewSource;
        public MainWindow()
        {
            InitializeComponent();
            artistsViewSource = (CollectionViewSource)FindResource(nameof(artistsViewSource));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Artists.Load();
            artistsViewSource.Source = _context.Artists.Local.ToObservableCollection();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _context.SaveChanges();
            dgArtists.Items.Refresh();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _context.Dispose();
            base.OnClosing(e);
        }
    }
}
