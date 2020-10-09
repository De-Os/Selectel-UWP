using Selectel.UI.Elements;
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
                                    var btn = new Button
                                    {
                                        HorizontalAlignment = HorizontalAlignment.Stretch,
                                        HorizontalContentAlignment = HorizontalAlignment.Stretch,
                                        Margin = new Thickness(10, 5, 10, 5),
                                        Content = new ServerElement(item),
                                        BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Gray),
                                        BorderThickness = new Thickness(1),
                                        CornerRadius = new CornerRadius(3),
                                        Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent)
                                    };
                                    btn.Click += (a, b) =>
                                    {
                                        App.Main.Navigation.SelectedItem = null;
                                        App.Main.ContentFrame.Content = new ServerInfo(item);
                                    };
                                    this.StackPanel.Children.Add(btn);
                                    break;
                            }
                        });
                    }
                }
                App.UIThread(() => this.Children.Remove(this.Children[0]));
            });
        }
    }
}
