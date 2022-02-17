using AppMusic.Service;
using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppMusic.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppMs : Page
    {
        public AppMs()
        {
            this.InitializeComponent();
            contentApp.Navigate(typeof(Pages.ListSong.Index));
        }

        private void DemoTransaction_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SelectPage_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                //contentFrame.Navigate(typeof(SampleSettingsPage));
            }
            else
            {
                var selectedItem = args.SelectedItem as NavigationViewItem;
                switch (selectedItem.Tag.ToString())
                {
                    case "ListSong":
                        contentApp.Navigate(typeof(Pages.ListSong.Index));
                        break;
                    case "MySong":
                        contentApp.Navigate(typeof(Pages.MyListSong.Index));
                        break;
                    case "CreateSong":
                        contentApp.Navigate(typeof(Pages.CreatSong.Index));
                        break;
                    case "Account":
                        contentApp.Navigate(typeof(Pages.AccountInfo.Index));
                        break;
                    default:
                        break;
                }
            }
        }

        private void contentFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private async void NavigationViewItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
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
