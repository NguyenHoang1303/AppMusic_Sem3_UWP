using AppMusic.Entity;
using AppMusic.Service;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppMusic.Pages.Login
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Index : Page
    {
        private AccountService serviveAcc = new AccountService();
        public Index()
        {
            this.InitializeComponent();
        }

        private void RedirectRegister_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pages.Register.Index));
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            var isValid = ValidateLogin();
            if (!isValid)
            {
                return;
            }
            Loading.Visibility = Visibility.Visible;
            Credential result = await serviveAcc.Login(txtEmail.Text, txtPassword.Password.ToString());
            ContentDialog contentDialog = new ContentDialog();
            Loading.Visibility = Visibility.Collapsed;
            if (result != null)
            {
                contentDialog.Title = "Đăng nhập thành công!";
                contentDialog.Content = "Xin chào!! Chúc bạn nghe nhạc vui vẻ.";
                Frame.Navigate(typeof(Pages.AppMs));
            }
            else
            {
                contentDialog.Title = "Đăng nhập thất bại!";
                contentDialog.Content = "Đăng nhập thất bại!";
            }
            contentDialog.CloseButtonText = "Ok";
            await contentDialog.ShowAsync();
        }

        private bool ValidateLogin()
        {
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                errorEmail.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                errorEmail.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty(txtPassword.Password.ToString()))
            {
                errorPassword.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                errorPassword.Visibility = Visibility.Collapsed;
            }
            return true;
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
