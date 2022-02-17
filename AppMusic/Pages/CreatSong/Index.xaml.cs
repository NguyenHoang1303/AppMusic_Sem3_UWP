using AppMusic.Entity;
using AppMusic.Constant;
using AppMusic.Service;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Diagnostics;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppMusic.Pages.CreatSong
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Index : Windows.UI.Xaml.Controls.Page
    {
        private int checkValid;
        private static string publicIDCloudinary;
        private CloudinaryDotNet.Account accountCloudinary;
        private Cloudinary cloudinary;
        public Index()
        {
            this.InitializeComponent();
            Loaded += Index_Loaded;
        }

        private void Index_Loaded(object sender, RoutedEventArgs e)
        {
            accountCloudinary = new CloudinaryDotNet.Account(
            CloudinaryInfo.CloudName,
            CloudinaryInfo.ApiKey,
            CloudinaryInfo.ApiSecret
            );
            cloudinary = new Cloudinary(accountCloudinary);
            cloudinary.Api.Secure = true;
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
            Debug.WriteLine(song.name + song.singer + song.author + "\n" + song.link + "\n" + song.thumbnail);
            bool result = await SongService.CreateMySong(song);
            ContentDialog contentDialog = new ContentDialog();
            if (result)
            {
                contentDialog.Title = "Thành công:";
                contentDialog.Content = "Bạn đã thêm bài nhạc mới";
            }
            else
            {
                contentDialog.Title = "Thất bại:";
                contentDialog.Content = "Thêm bài nhạc mới thất bại!";
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

        private async void btnCreateThumnnail_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                BitmapImage bitmapImage = new BitmapImage();
                IRandomAccessStream fileStream = await file.OpenReadAsync();
                await bitmapImage.SetSourceAsync(fileStream);
                txtImageCreate.Source = bitmapImage;
                RawUploadParams imageUploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.Name, await file.OpenStreamForReadAsync())
                };
                RawUploadResult result = await cloudinary.UploadAsync(imageUploadParams);
                publicIDCloudinary = result.PublicId;
                txtThumbnail.Text = result.Url.ToString();
            }
            else
            {
                Debug.WriteLine("Tạo ảnh cho nhạc thất bại!");
            }
        }
    }
}
