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
    public class PowerButtons : StackPanel
    {
        public ToggleButton Power = new ToggleButton
        {
            Content = new ProgressRing { IsActive = true },
            IsChecked = false,
            CornerRadius = new CornerRadius(3),
            Margin = new Thickness(5, 0, 10, 0)
        };

        public Button Reboot = new Button
        {
            Content = new FontIcon
            {
                Glyph = "\uE777"
            },
            CornerRadius = new CornerRadius(3),
            Margin = new Thickness(10, 0, 5, 0)
        };

        private readonly string UUID;
        private bool _rebooting = false;

        public PowerButtons(string UUID)
        {
            this.UUID = UUID;

            this.Orientation = Orientation.Horizontal;
            this.Children.Add(this.Reboot);
            this.Children.Add(this.Power);

            Task.Factory.StartNew(() =>
            {
                var power = App.Selectel.Servers.Power.Get(this.UUID).DriverStatus.PowerState.EndsWith("on");
                App.UIThread(() =>
                {
                    this.Power.IsChecked = power;
                    this.Power.Content = new FontIcon
                    {
                        Glyph = "\uE7E8"
                    };
                });
            });
            this.Reboot.Click += (a, b) => this.OnReboot();
            this.Power.Click += (a, b) => this.OnPower();
        }

        private void OnReboot()
        {
            if (this._rebooting) return;
            this._rebooting = true;
            this.Reboot.Content = new ProgressRing { IsActive = true };
            this.Power.IsChecked = false;
            Task.Factory.StartNew(() =>
            {
                App.Selectel.Servers.Power.Reboot(this.UUID);

                var power = false;
                int wait = 2;
                while (!power)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(wait));
                    power = App.Selectel.Servers.Power.Get(this.UUID).DriverStatus.PowerState.EndsWith("on");
                    if (power)
                    {
                        App.UIThread(() =>
                        {
                            this.Power.IsChecked = true;
                            this.Reboot.Content = new FontIcon
                            {
                                Glyph = "\uE777"
                            };
                        });
                    }
                    wait *= 2;
                }
                this._rebooting = false;
            });
        }

        private void OnPower()
        {
            if (this._rebooting) return;
            this._rebooting = true;
            this.Power.Content = new ProgressRing { IsActive = true };
            var state = !(bool)this.Power.IsChecked;
            Task.Factory.StartNew(() =>
            {
                App.Selectel.Servers.Power.Set(this.UUID, state);

                App.UIThread(() =>
                {
                    this.Power.Content = new FontIcon
                    {
                        Glyph = "\uE7E8"
                    };
                });

                this._rebooting = false;
            });
        }
    }
}