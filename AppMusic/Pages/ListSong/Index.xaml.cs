using AppMusic.Entity;
using AppMusic.Service;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppMusic.Pages.ListSong
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Index : Page
    {
        private SongService songService;
        private MediaPlaybackList mediaPlaybackList;
        public Index()
        {
            this.InitializeComponent();
            songService = new SongService();
            this.Loaded += Index_Loaded;
        }

        private async void Index_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var list = await this.songService.GetAllSong();
            ObservableCollection<Song> contacts3 = new ObservableCollection<Song>(list);
            listSong.ItemsSource = contacts3;
            loadMusic.Visibility = Visibility.Collapsed;

            mediaPlaybackList = new MediaPlaybackList();
            foreach (var song in list)
            {
                try
                {
                    var mediaPlaybackItem = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(song.link)));
                    mediaPlaybackList.Items.Add(mediaPlaybackItem);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }
            var mediaPlayer = new MediaPlayer();
            mediaPlayer.Source = mediaPlaybackList; // MediaPlayerElement < MediaPlayer < MediaPlaybackList < MediaPlaybackItem           
            MyMediaPlayer.SetMediaPlayer(mediaPlayer);
        }

        private void Play_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Song currentSong = listSong.SelectedItem as Song;
            for (var i = 0; i < mediaPlaybackList.Items.Count; i++)
            {
                MediaPlaybackItem item = mediaPlaybackList.Items[i];
                if (string.Equals(currentSong.link, item.Source.Uri.ToString()))
                {
                    mediaPlaybackList.MoveTo(Convert.ToUInt32(i));
                }
                else
                {

                }
            }
        }
    }
}
