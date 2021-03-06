﻿using Selectel.Libs;
using Selectel.Libs.APi.Responses;
using Selectel.UI.Elements;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Selectel.UI.Frames
{
    public class ServerInfo : Grid
    {
        private readonly ServersResponse.Resource.ResourceItem Server;
        private Pivot Pivot = new Pivot();

        public ServerInfo(ServersResponse.Resource.ResourceItem server)
        {
            this.Server = server;

            this.Children.Add(new ProgressRing
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 50,
                Height = 50,
                IsActive = true
            });
            this.Children.Add(this.Pivot);

            this.Loaded += (a, b) => this.Load();
        }

        private void Load()
        {
            this.LoadMainPivot();
            this.LoadNetworkPivot();
            this.LoadOSPivot();
            this.LoadConfigPivot();

            this.Children.Remove(this.Children[0]);
        }

        private void LoadMainPivot()
        {
            var panel = new StackPanel
            {
                Margin = new Thickness(10),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            Task.Factory.StartNew(() =>
            {
                var loc = App.Selectel.Servers.Location.Get(this.Server.LocationUUID);
                App.UIThread(() =>
                {
                    panel.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("UUID") }, new TextWithCopy(this.Server.UUID)));
                    panel.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("LocationUUID") }, new TextWithCopy(this.Server.LocationUUID)));
                    panel.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("Location") }, new TextWithCopy(loc.Name)));
                    if (this.Server.Tags?.Count > 0)
                    {
                        panel.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("Tags") }, new TextWithCopy(String.Join(", ", this.Server.Tags))));
                    }
                });
            });

            this.AddPivotElement(this.Server.UserDescription?.Length > 0 ? this.Server.UserDescription : this.Server.Info, panel);
        }

        private void LoadNetworkPivot()
        {
            var network = new StackPanel
            {
                Margin = new Thickness(10),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            Task.Factory.StartNew(() =>
            {
                var ips = App.Selectel.Servers.Network.GetIPs();
                ips.RemoveAll(i => i.ResourceUUID != this.Server.UUID);

                if (ips.Count > 0)
                {
                    App.UIThread(() =>
                    {
                        var stack = new StackPanel
                        {
                            Margin = new Thickness(0, 10, 0, 10)
                        };
                        network.Children.Insert(0, stack);

                        var grd = new Grid
                        {
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            Margin = new Thickness(0, 0, 0, 5)
                        };
                        grd.ColumnDefinitions.Add(new ColumnDefinition());
                        grd.ColumnDefinitions.Add(new ColumnDefinition());
                        grd.ColumnDefinitions.Add(new ColumnDefinition());
                        var subnetTitle = new TextBlock { Text = Utils.LocString("Subnet"), VerticalAlignment = VerticalAlignment.Center };
                        Grid.SetColumn(subnetTitle, 0);
                        var ipTitle = new TextBlock { Text = Utils.LocString("IP"), VerticalAlignment = VerticalAlignment.Center };
                        Grid.SetColumn(ipTitle, 1);
                        var gatewayTitle = new TextBlock { Text = Utils.LocString("Gateway"), VerticalAlignment = VerticalAlignment.Center };
                        Grid.SetColumn(gatewayTitle, 2);
                        grd.Children.Add(subnetTitle);
                        grd.Children.Add(ipTitle);
                        grd.Children.Add(gatewayTitle);

                        stack.Children.Add(grd);

                        foreach (var ip in ips)
                        {
                            var grid = new Grid
                            {
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                Margin = new Thickness(0, 5, 0, 5)
                            };
                            grid.ColumnDefinitions.Add(new ColumnDefinition());
                            grid.ColumnDefinitions.Add(new ColumnDefinition());
                            grid.ColumnDefinitions.Add(new ColumnDefinition());
                            var subnet = new TextWithCopy(ip.Subnet) { VerticalAlignment = VerticalAlignment.Center };
                            Grid.SetColumn(subnet, 0);
                            var ipText = new TextWithCopy(ip.IP, true) { VerticalAlignment = VerticalAlignment.Center };
                            Grid.SetColumn(ipText, 1);
                            var gateway = new TextWithCopy(ip.GateWay) { VerticalAlignment = VerticalAlignment.Center };
                            Grid.SetColumn(gateway, 2);

                            grid.Children.Add(subnet);
                            grid.Children.Add(ipText);
                            grid.Children.Add(gateway);

                            stack.Children.Add(grid);
                        }
                    });
                }
            });

            Task.Factory.StartNew(() =>
            {
                var speed = App.Selectel.Servers.Consumption.Get(this.Server.UUID, till: DateTime.UtcNow.ToUnixTime());
                if (speed == null) return;

                var trueSpeed = speed.Select(i => new
                {
                    Time = i[0].ToDateTime().ToLocalTime(),
                    Down = Math.Round(Convert.ToDouble(i[1]) / 8 / 1024, 2),
                    Up = Math.Round(Convert.ToDouble(i[2]) / 8 / 1024, 2)
                });
                var max = trueSpeed.OrderByDescending(i => i.Down).First().Down;
                if (trueSpeed.OrderByDescending(i => i.Up).First().Up > max) max = trueSpeed.OrderByDescending(i => i.Up).First().Up;

                App.UIThread(() =>
                {
                    var grid = new Grid
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Margin = new Thickness(0, 10, 0, 0)
                    };
                    var upBrush = new SolidColorBrush(Colors.YellowGreen);
                    var downBrush = new SolidColorBrush(getHalfTransparent(Colors.DarkGreen));

                    network.Children.Add(new TextBlock
                    {
                        FontSize = 30,
                        Text = Utils.LocString("Usage"),
                        Margin = new Thickness(0, 10, 0, 5)
                    });

                    grid.Children.Add(new TextBlock
                    {
                        Text = $"{max} {Utils.LocString("Kbs")}",
                        VerticalAlignment = VerticalAlignment.Top,
                        FontWeight = FontWeights.Bold,
                        Padding = new Thickness(0, 0, 2, 0)
                    });
                    grid.Children.Add(new TextBlock
                    {
                        Text = Utils.LocString("Speed"),
                        VerticalAlignment = VerticalAlignment.Bottom,
                        FontWeight = FontWeights.SemiBold,
                        Padding = new Thickness(0, 0, 2, 0)
                    });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                    foreach (var s in trueSpeed)
                    {
                        var up = new Rectangle()
                        {
                            Fill = upBrush,
                            MinWidth = 1,
                            MinHeight = 1,
                            MaxHeight = 300,
                            Height = getSize(s.Up),
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Bottom
                        };
                        var down = new Rectangle()
                        {
                            Fill = downBrush,
                            MinWidth = 1,
                            MinHeight = 1,
                            MaxHeight = 300,
                            Height = getSize(s.Down),
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Bottom
                        };
                        Grid.SetColumn(up, grid.ColumnDefinitions.Count);
                        Grid.SetColumn(down, grid.ColumnDefinitions.Count);
                        grid.Children.Add(up);
                        grid.Children.Add(down);
                        grid.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    network.Children.Add(grid);

                    double getSize(double s) => 300 * (s / max);
                    Color getHalfTransparent(Color color)
                    {
                        color.A = 155;
                        return color;
                    }
                });
            });

            this.AddPivotElement("Network", network);
        }

        private void LoadOSPivot()
        {
            var os = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            Task.Factory.StartNew(() =>
            {
                var info = App.Selectel.Servers.Boot.OSInfo(this.Server.UUID);

                App.UIThread(() =>
                {
                    os.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("Hostname") }, new TextWithCopy(info.UserHostname)));
                    os.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("IP") }, new TextWithCopy(info.IPv4)));
                    os.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("Login") }, new TextWithCopy(info.Login)));
                    os.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("Password") }, new TextWithCopy(info.Password)));
                    os.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("Distributive") }, new TextWithCopy(info.OSTemplate + " " + info.Version)));
                    os.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("Raid") }, new TextWithCopy(info.RaidType)));
                });
            });

            this.AddPivotElement("OS", os);
        }

        private void LoadConfigPivot()
        {
            var config = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            Task.Factory.StartNew(() =>
            {
                var hw = App.Selectel.Servers.Service.GetServer(this.Server.ServiceUUID);

                App.UIThread(() =>
                {
                    App.UIThread(() => config.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("Name") }, new TextWithCopy(hw.Name))));
                    App.UIThread(() => config.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("Type") }, new TextWithCopy(hw.TarriffLine))));
                });

                if (hw.CPU != null)
                {
                    string text = hw.CPU.Name + " " + hw.CPU.CoresPerCpu.ToString() + "×" + hw.CPU.BaseFreq.ToString() + " Ghz";
                    if (hw.CPU.Count > 1) text += " (" + hw.CPU.Count.ToString() + ")";
                    App.UIThread(() => config.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("CPU") }, new TextWithCopy(text.Replace("Ghz", Utils.LocString("Ghz"))))));
                }

                if (hw.GPU?.Name != null)
                {
                    string text = hw.GPU.Name;
                    if (hw.GPU.Count > 1) text = hw.GPU.Count.ToString() + " × " + text;
                    App.UIThread(() => config.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("GPU") }, new TextWithCopy(text))));
                }

                if (hw.RAM?.Count > 0)
                {
                    int x = 0;
                    foreach (var r in hw.RAM)
                    {
                        x++;
                        App.UIThread(() => config.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("RAM") + (x > 1 ? " " + x.ToString() : "") }, new TextWithCopy((r.Count * r.Size).ToString() + " " + Utils.LocString("Gb") + " " + r.Type))));
                    }
                }

                if (hw.ROM?.Count > 0)
                {
                    int x = 0;
                    foreach (var r in hw.ROM)
                    {
                        x++;
                        string text = r.Size.ToString() + " Gb " + r.Type;
                        if (r.Count > 1) text = r.Count.ToString() + " × " + text;
                        App.UIThread(() => config.Children.Add(this.BaseElement(new TextBlock { Text = Utils.LocString("ROM") + (x > 1 ? " " + x.ToString() : "") }, new TextWithCopy(text.Replace("Gb", Utils.LocString("Gb"))))));
                    }
                }
            });

            this.AddPivotElement("Configuration", config);
        }

        private void AddPivotElement(string locale, FrameworkElement content)
        {
            this.Pivot.Items.Add(new PivotItem
            {
                Header = Utils.LocString(locale),
                Content = new ScrollViewer
                {
                    HorizontalScrollMode = Windows.UI.Xaml.Controls.ScrollMode.Disabled,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Content = content
                }
            });
        }

        private Grid BaseElement(FrameworkElement element1, FrameworkElement element2)
        {
            var grid = new Grid { HorizontalAlignment = HorizontalAlignment.Stretch };
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            element1.VerticalAlignment = VerticalAlignment.Center;
            element2.VerticalAlignment = VerticalAlignment.Center;

            Grid.SetColumn(element1, 0);
            Grid.SetColumn(element2, 1);

            grid.Children.Add(element1);
            grid.Children.Add(element2);

            return grid;
        }
    }
}