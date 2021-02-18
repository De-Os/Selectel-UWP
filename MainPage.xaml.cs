using Selectel.Libs;
using Selectel.UI.Frames;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Selectel
{
    public sealed partial class MainPage : Page
    {
        public delegate void LogoutEvent();

        public LogoutEvent OnLogout;

        public MainPage()
        {
            this.InitializeComponent();
            this.LoadLocalizations();

            this.Navigation.SelectionChanged += this.NavigationChanged;
            this.Navigation.SelectedItem = this.MenuItemServers;

            this.Logout.Tapped += (a, b) => this.OnLogout?.Invoke();
            this.Balance.Tapped += (a, b) =>
            {
                this.Navigation.SelectedItem = null;
                this.ContentFrame.Content = new PaymentHistory();
            };
            this.UpdateBalance();
        }

        public void UpdateBalance()
        {
            this.Balance.Content = new ProgressRing { IsActive = true };
            Task.Factory.StartNew(() =>
            {
                var balance = App.Selectel.Billing.Balance.Get();
                App.UIThread(() =>
                {
                    this.Balance.Content = new TextBlock
                    {
                        Text = Utils.LocString("Balance").Replace("%money%", $"{balance.Primary.Main / 100} ₽")
                    };
                });
            });
        }

        private void NavigationChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            FrameworkElement newContent = null;

            if (args.SelectedItem == this.MenuItemServers)
            {
                newContent = new ServersList("server");
            }

            if (this.ContentFrame.Content != newContent) this.ContentFrame.Content = newContent;
        }

        private void LoadLocalizations()
        {
            this.MenuItemServers.Content = new TextBlock { Text = Utils.LocString("Servers") };
            this.Logout.Content = new TextBlock { Text = Utils.LocString("Login/Logout") };
        }
    }
}