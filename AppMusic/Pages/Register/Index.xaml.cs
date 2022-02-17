using AppMusic.Entity;
using AppMusic.Service;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Account = AppMusic.Entity.Account;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppMusic.Pages.Register
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Index : Windows.UI.Xaml.Controls.Page
    {
        private static string publicIDCloudinary;
        private CloudinaryDotNet.Account accountCloudinary;
        private Cloudinary cloudinary;
        private AccountService accountService;
        private int selectedGender;
        private string dob;
        private int countError;
        public Index()
        {
            this.InitializeComponent();
            accountService = new AccountService();
            Loaded += Index_Loaded;
        }

        private void Index_Loaded(object sender, RoutedEventArgs e)
        {
            accountCloudinary = new CloudinaryDotNet.Account(
           "nguyenhs",
           "145514162246859",
           "-TrF50dJvtpBQMR28i4rpCIg5K4"
           );
            cloudinary = new Cloudinary(accountCloudinary);
            cloudinary.Api.Secure = true;
        }

        private void LoginRedirect_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pages.Login.Index));
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            ValidateRegister();
            Debug.WriteLine("account");
            if (countError > 0)
            {
                return;
            }

            Account account = new Account()
            {
                firstName = txtFirstName.Text,
                lastName = txtLastName.Text,
                email = txtEmail.Text,
                phone = txtPhone.Text,
                password = txtPassword.Password.ToString(),
                address = txtAddress.Text,
                gender = selectedGender,
                avatar = txtAvatar.Text,
                birthday = dob,
                introduction = txtIntroduction.Text,
            };
            bool result = await accountService.Register(account);
            ContentDialog contentDialog = new ContentDialog();
            if (result)
            {
                contentDialog.Title = "Acction success!";
                contentDialog.Content = "Tạo tài khoản thành công!";
            }
            else
            {
                contentDialog.Title = "Acction false!";
                contentDialog.Content = "Tạo tài khoản thất bại!";
            }
            contentDialog.CloseButtonText = "Ok";
            await contentDialog.ShowAsync();
        }

        private void ValidateRegister()
        {
            countError = 0;

            if (string.IsNullOrEmpty(txtFirstName.Text))
            {
                errorFirstName.Visibility = Visibility.Visible;
                countError++;
            }
            else
            {
                errorFirstName.Visibility = Visibility.Collapsed;

            }

            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                errorLastName.Visibility = Visibility.Visible;
                countError++;
            }
            else
            {
                errorLastName.Visibility = Visibility.Collapsed;

            }
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                errorEmail.Visibility = Visibility.Visible;
                countError++;
            }
            else
            {
                if (!CheckEmail(txtEmail.Text))
                {
                    errorEmail.Text = "Đây không phải là email";
                    errorEmail.Visibility = Visibility.Visible;
                    countError++;
                }
                else
                {
                    errorEmail.Visibility = Visibility.Collapsed;

                }
            }
            var password = txtPassword.Password.ToString();
            var confirmPass = txtConfirmPassword.Password.ToString();
            if (string.IsNullOrEmpty(password))
            {
                errorPassword.Visibility = Visibility.Visible;
                countError++;
                Debug.WriteLine("2.password: " + password);
            }
            else
            {
                errorPassword.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(confirmPass))
            {
                errorConfirmPassword.Visibility = Visibility.Visible;
                countError++;
            }
            else
            {
                errorConfirmPassword.Visibility = Visibility.Collapsed;
            }

            if (!string.Equals(password, confirmPass))
            {
                errorPassword.Text = "* Mật khẩu không giống nhau";
                errorPassword.Visibility = Visibility.Visible;
            }
            else
            {
                errorPassword.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                errorPhone.Visibility = Visibility.Visible;
                countError++;
            }
            else
            {
                if (!CheckPhone(txtPhone.Text))
                {
                    errorPhone.Text = "Đây không phải là số điện thoại";
                    errorPhone.Visibility = Visibility.Visible;
                    countError++;
                }
                else
                {
                    errorPhone.Visibility = Visibility.Collapsed;

                }
            }
            if (string.IsNullOrEmpty(txtAvatar.Text))
            {
                errorAvatar.Visibility = Visibility.Visible;
                countError++;
            }
            else
            {
                errorAvatar.Visibility = Visibility.Collapsed;

            }
            if (selectedGender == 0)
            {
                errorGender.Visibility = Visibility.Visible;
                countError++;
            }
            else
            {
                errorGender.Visibility = Visibility.Collapsed;

            }
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                errorAddress.Visibility = Visibility.Visible;
                countError++;
            }
            else
            {
                errorAddress.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty(dob))
            {
                errorDob.Visibility = Visibility.Visible;
                countError++;
            }
            else
            {
                errorDob.Visibility = Visibility.Collapsed;
            }
        }

        public static bool CheckEmail(string email)
        {
            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            return Regex.IsMatch(email, pattern);
        }

        public static bool CheckPhone(string number)
        {
            return Regex.Match(number, @"^(84|0[3|5|7|8|9])+([0-9]{8})$").Success;
        }

        private void Gender_Checked(object sender, RoutedEventArgs e)
        {
            var check = sender as RadioButton;
            switch (check.Content)
            {
                case "Nam":
                    selectedGender = 1;
                    break;
                case "Nữ":
                    selectedGender = 2;
                    break;
                case "Khác":
                    selectedGender = 3;
                    break;
                default:
                    selectedGender = 0;
                    break;
            }
        }

        private void dtmDob_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            dob = sender.Date.Value.ToString("yyyy-MM-dd");
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
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
                ImageAcc.Source = bitmapImage;
                RawUploadParams imageUploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.Name, await file.OpenStreamForReadAsync())
                };
                RawUploadResult result = await cloudinary.UploadAsync(imageUploadParams);
                publicIDCloudinary = result.PublicId;
                txtAvatar.Text = result.Url.ToString();
            }
            else
            {

                Debug.WriteLine("Tải ảnh lên thất bại!");
            }
        }
    }
}
