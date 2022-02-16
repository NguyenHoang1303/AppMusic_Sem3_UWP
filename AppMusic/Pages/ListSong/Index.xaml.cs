using AppMusic.Entity;
using AppMusic.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Media.Core;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppMusic.Pages.ListSong
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Index : Page
    {
        private SongService songService;
        public Index()
        {
            this.InitializeComponent();
            songService = new SongService();
            this.Loaded += Index_Loaded;
        }

        private async void Index_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            List<Song> listSong = await songService.GetAllSong();
            AllSong.ItemsSource = new ObservableCollection<Song>(listSong);
        }

        private void Play_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Song currentSong = AllSong.SelectedItem as Song;
            MyMediaPlayer.MediaPlayer.Source = MediaSource.CreateFromUri(new Uri(currentSong.link));
            MyMediaPlayer.MediaPlayer.Play();
        }
    }
}
