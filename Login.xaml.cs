using Selectel.Libs.Api;
using System;
using Windows.Security.Credentials;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Utils = Selectel.Libs.Utils;

namespace Selectel
{
    public sealed partial class Login : Page
    {
        public delegate void Event();

        public event Event OnSuccess;

        public Login()
        {
            this.InitializeComponent();
            this.Logo.Source = new SvgImageSource(Utils.AssetPath("logo.svg"));
            this.LoginBtn.Content = new TextBlock { Text = Utils.LocString("Login/Login") };
            this.Token.PlaceholderText = Utils.LocString("Login/TokenPlacehodler");
        }

        private async void DoLogin(object btn, RoutedEventArgs args)
        {
            if (this.Token.Text.Length == 0) return;
            var client = new SelectelApi(this.Token.Text);
            try
            {
                if (!client.Servers.Dashboard.Maintenance().Status)
                {
                    var vault = new PasswordVault();
                    vault.Add(new PasswordCredential("selectel", "default", this.Token.Text));
                    this.OnSuccess?.Invoke();
                }
                else await new MessageDialog("Error").ShowAsync();
            }
            catch (Exception)
            {
                await new MessageDialog(Utils.LocString("Login/Error")).ShowAsync();
            }
        }

        private async void OpenHelp(object btn, RoutedEventArgs args) => await Windows.System.Launcher.LaunchUriAsync(new Uri("https://my.selectel.ru/profile/apikeys"));
    }
}