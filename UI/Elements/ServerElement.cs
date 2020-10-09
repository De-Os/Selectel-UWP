using Selectel.Libs;
using Selectel.Libs.APi.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;

namespace Selectel.UI.Elements
{
    [Bindable]
    public class ServerElement : Grid
    {
        private ServersResponse.Resource.ResourceItem Server;

        public ServerElement(ServersResponse.Resource.ResourceItem item)
        {
            this.Server = item;
            this.Load();
        }

        private void Load()
        {
            this.RowDefinitions.Add(new RowDefinition());
            this.RowDefinitions.Add(new RowDefinition());

            var nameGrid = new Grid();
            nameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            nameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            nameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            nameGrid.Children.Add(new TextBlock
            {
                Text = this.Server.UserDescription?.Length > 0 ? this.Server.UserDescription : this.Server.Info,
                Margin = new Thickness(10),
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 20,
                FontWeight = Windows.UI.Text.FontWeights.Bold,
                TextTrimming = TextTrimming.CharacterEllipsis
            });
            var power = new PowerButtons(this.Server.UUID);
            Grid.SetColumn(power, 1);
            nameGrid.Children.Add(power);
            Grid.SetRow(nameGrid, 0);
            this.Children.Add(nameGrid);

            var textGrid = new Grid();
            textGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            textGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            Grid.SetRow(textGrid, 1);
            this.Children.Add(textGrid);

            var hwtext = new TextBlock
            {
                Text = this.Server.ConfigName,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10),
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            textGrid.Children.Add(hwtext);

            Task.Factory.StartNew(() =>
            {
                var loc = App.Selectel.Servers.Location.Get(this.Server.LocationUUID);
                App.UIThread(() =>
                {
                    var stack = new StackPanel
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0, 0, 10, 0)
                    };
                    Grid.SetColumn(stack, 1);

                    stack.Children.Add(new TextBlock
                    {
                        Text = loc.Name,
                        HorizontalAlignment = HorizontalAlignment.Right
                    });
                    stack.Children.Add(new TextBlock
                    {
                        Text = Utils.LocString("PaidTill").Replace("%date%", this.Server.PaidTill.ToDateTime().ToString("dd.MM.yy")),
                        HorizontalAlignment = HorizontalAlignment.Right
                    });

                    textGrid.Children.Add(stack);
                });
            });
        }
    }
}
