using AppMusic.Entity;
using AppMusic.Service;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppMusic.Pages.AccountInfo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Index : Page
    {
        public Index()
        {
            this.InitializeComponent();
            this.Loaded += Index_Loaded;
        }

        private void Index_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Account account = App.accountUser;
            txtFirstName.Text = account.firstName;
            txtLastName.Text = account.lastName.ToString();
            txtEmail.Text = account.email.ToString();
            txtAddress.Text = account.address.ToString();
            txtPhone.Text = account.phone.ToString();
            //BitmapImage bitmapImageAvatar = new BitmapImage(new Uri(account.avatar.ToString()));
            // txtAvatar.Source = bitmapImageAvatar;
            txtGender.Text = account.gender.ToString();
            txtDob.Text = account.birthday.ToString();
            txtIntroduction.Text = account.introduction ?? "";
        }

        private async void Exit_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile manifestFile = await storageFolder.GetFileAsync(AccountService.tokenUserFile);
                await manifestFile.DeleteAsync();
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(Pages.Login.Index));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Có lỗi xảy ra khi logout" + ex);
            }

        }
    }
}
