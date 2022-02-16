using AppMusic.Entity;
using AppMusic.Service;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppMusic.Pages.CreatSong
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Index : Page
    {
        private int checkValid;

        public Index()
        {
            this.InitializeComponent();
        }

        private async void CreateSingle_Click(object sender, RoutedEventArgs e)
        {
            CheckValidate();
            if (checkValid > 0)
            {
                return;
            }
            
            var song = new Song()
            {
                name = txtName.Text,
                description = txtDescription.Text,
                singer = txtSinger.Text,
                author = txtAuthor.Text,
                thumbnail = txtThumbnail.Text,
                link = txtLink.Text,
            };
            Debug.WriteLine(song.name + song.singer + song.author+ "\n" + song.link + "\n" + song.thumbnail);
            bool result = await SongService.CreateMySong(song);
            ContentDialog contentDialog = new ContentDialog();
            if (result)
            {
                contentDialog.Title = "Creat Music:";
                contentDialog.Content = "Tạo nhạc mới thành công";
            }
            else
            {
                contentDialog.Title = "Creat Music:";
                contentDialog.Content = "Tạo nhạc mới thất bại!";
            }
            contentDialog.CloseButtonText = "Ok";
            await contentDialog.ShowAsync();
        }

        private void CheckValidate()
        {
            checkValid = 0;
            if (string.IsNullOrEmpty(txtName.Text))
            {
                errName.Visibility = Visibility.Visible;
                checkValid++;
            }
            else
            {
                errName.Visibility = Visibility.Collapsed;
                
            }
            
            if (string.IsNullOrEmpty(txtSinger.Text))
            {
                errSinger.Visibility = Visibility.Visible;
                checkValid++;
            }
            else
            {
                errSinger.Visibility = Visibility.Collapsed;
                
            }
            if (string.IsNullOrEmpty(txtAuthor.Text))
            {
                errAuthor.Visibility = Visibility.Visible;
                checkValid++;
            }
            else
            {
                errAuthor.Visibility = Visibility.Collapsed;
                
            }
            if (string.IsNullOrEmpty(txtThumbnail.Text))
            {
                errThumbnail.Visibility = Visibility.Visible;
                checkValid++;
            }
            else
            {
                errThumbnail.Visibility = Visibility.Collapsed;
                
            }
            if (string.IsNullOrEmpty(txtLink.Text))
            {
                errLink.Visibility = Visibility.Visible; 
                checkValid++;
            }
            else
            {
                errLink.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                errDescription.Visibility = Visibility.Visible;
                checkValid++;
            }
            else
            {
                errDescription.Visibility = Visibility.Collapsed;
                
            }
        }
    }
}
