using Selectel.Libs;
using Selectel.Libs.APi.Responses;
using Selectel.UI.Elements;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Selectel.UI.Frames
{
    [Bindable]
    public class ServersList : Grid
    {
        private StackPanel StackPanel = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        private string Type;

        public ServersList(string type)
        {
            this.Type = type;

            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;

            this.Children.Add(new ProgressRing
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 50,
                Height = 50,
                IsActive = true
            });
            this.Children.Add(new ScrollViewer
            {
                Content = this.StackPanel,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalScrollMode = ScrollMode.Disabled
            });

            this.Loaded += (a, b) => this.Load();
        }

        private void Load()
        {
            var search = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                PlaceholderText = Utils.LocString("Search"),
                Margin = new Thickness(10)
            };
            search.TextChanged += this.Search;
            this.StackPanel.Children.Add(search);
            Task.Factory.StartNew(() =>
            {
                var items = App.Selectel.Servers.Resources.Get();
                foreach (var item in items)
                {
                    if (item.ServiceType.StartsWith(this.Type))
                    {
                        App.UIThread(() =>
                        {
                            switch (this.Type)
                            {
                                case "serverchip":
                                case "server":
                                    this.StackPanel.Children.Add(new ServerBtn(item));
                                    break;
                            }
                        });
                    }
                }
                App.UIThread(() => this.Children.Remove(this.Children[0]));
            });
        }

        private void Search(object sender, TextChangedEventArgs e)
        {
            var query = (sender as TextBox).Text;
            foreach (var item in this.StackPanel.Children)
            {
                if (item is ServerBtn server)
                {
                    server.Visibility = Regex.IsMatch(server.ServerName, query, RegexOptions.IgnoreCase) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        public class ServerBtn : Button
        {
            public readonly string ServerName;

            public ServerBtn(ServersResponse.Resource.ResourceItem server)
            {
                this.ServerName = server.UserDescription?.Length > 0 ? server.UserDescription : server.Info;

                this.HorizontalAlignment = HorizontalAlignment.Stretch;
                this.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                this.Margin = new Thickness(10, 5, 10, 5);
                this.Content = new ServerElement(server);
                this.BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Gray);
                this.BorderThickness = new Thickness(1);
                this.CornerRadius = new CornerRadius(3);
                this.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
                this.Click += (a, b) =>
                {
                    App.Main.Navigation.SelectedItem = null;
                    App.Main.ContentFrame.Content = new ServerInfo(server);
                };
            }
        }
    }
}