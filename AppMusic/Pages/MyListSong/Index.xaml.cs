using AppMusic.Entity;
using AppMusic.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppMusic.Pages.MyListSong
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
            List<Song> mySongs = await songService.GetMySongs();
            ObservableCollection<Song> observableSongs = new ObservableCollection<Song>(mySongs);
            //Debug.WriteLine(listSong.Count);
            listMySong.ItemsSource = observableSongs;
            loadMusic.Visibility = Visibility.Collapsed;


            mediaPlaybackList = new MediaPlaybackList();
            foreach (var song in mySongs)
            {
                try
                {
                    var mediaPlaybackItem = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(song.link)));
                    mediaPlaybackList.Items.Add(mediaPlaybackItem);
                }
                catch (UriFormatException ex)
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

            Song currentSong = listMySong.SelectedItem as Song;
            ContentDialog contentDialog = new ContentDialog();
            for (var i = 0; i < mediaPlaybackList.Items.Count; i++)
            {
                MediaPlaybackItem item = mediaPlaybackList.Items[i];
                if (string.Equals(currentSong.link, item.Source.Uri.ToString()))
                {
                    try
                    {
                        mediaPlaybackList.MoveTo(Convert.ToUInt32(i));
                    }
                    catch
                    {
                        contentDialog.Title = "Fail!";
                        contentDialog.Content = "Bài hát lỗi!";
                    }

                }

            }
        }
    }
}
