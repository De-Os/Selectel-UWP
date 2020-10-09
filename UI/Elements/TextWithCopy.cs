using Selectel.Libs;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace Selectel.UI.Elements
{
    public class TextWithCopy : Grid
    {
        public TextWithCopy(string text, bool link = false)
        {
            this.ColumnDefinitions.Add(new ColumnDefinition { Width = new Windows.UI.Xaml.GridLength(1, Windows.UI.Xaml.GridUnitType.Auto) });
            this.ColumnDefinitions.Add(new ColumnDefinition { Width = new Windows.UI.Xaml.GridLength(1, Windows.UI.Xaml.GridUnitType.Auto) });

            var block = new TextBlock { Text = text, VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center };
            this.Children.Add(block);
            if (link)
            {
                block.Foreground = new SolidColorBrush(Colors.Blue);
                block.PointerPressed += async (a, b) => await Windows.System.Launcher.LaunchUriAsync(new Uri((new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b").IsMatch(text) ? "http://" : "") + text));
            }
            var copy = new Button
            {
                Content = new FontIcon
                {
                    Glyph = "\uE8C8"
                },
                CornerRadius = new Windows.UI.Xaml.CornerRadius(3),
                Background = new SolidColorBrush(Colors.Transparent),
                Margin = new Windows.UI.Xaml.Thickness(5, 0, 0, 0),
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center
            };
            Grid.SetColumn(copy, 1);
            this.Children.Add(copy);
            copy.Click += (a, b) =>
            {
                var pkg = new DataPackage();
                pkg.SetText(text);
                Clipboard.SetContent(pkg);

                var flyout = new Flyout
                {
                    Content = new TextBlock
                    {
                        Text = Utils.LocString("Copied")
                    }
                };
                flyout.ShowAt(copy);
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000);
                    App.UIThread(() => flyout.Hide());
                });
            };
        }
    }
}
