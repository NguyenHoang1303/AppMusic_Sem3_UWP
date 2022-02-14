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
        public Index()
        {
            this.InitializeComponent();
        }

        private void RegisterRedirect_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pages.Login.Index));
        }

        private void RedirectRegister_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pages.Register.Index));
        }
    }
}
