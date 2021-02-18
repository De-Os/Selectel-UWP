using Selectel.Libs;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Selectel.UI.Frames
{
    public class PaymentHistory : Grid
    {
        private StackPanel StackPanel = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Padding = new Thickness(10, 5, 10, 5)
        };

        public PaymentHistory()
        {
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;

            this.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            this.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var title = new TextBlock
            {
                Text = Utils.LocString("PaymentHistory"),
                FontSize = 30,
                Margin = new Thickness(10, 5, 10, 0)
            };
            Grid.SetRow(title, 0);
            this.Children.Add(title);

            var ring = new ProgressRing
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 50,
                Height = 50,
                IsActive = true
            };
            Grid.SetRow(ring, 1);
            this.Children.Add(ring);

            var scroll = new ScrollViewer
            {
                Content = this.StackPanel,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalScrollMode = ScrollMode.Disabled
            };
            Grid.SetRow(scroll, 1);
            this.Children.Add(scroll);

            this.Loaded += (a, b) => this.Load();
        }

        private void Load()
        {
            App.Main.UpdateBalance();
            Task.Factory.StartNew(() =>
            {
                var response = App.Selectel.Billing.Transactions.Get();
                if (response?.Count > 0)
                {
                    var lang = Windows.System.UserProfile.GlobalizationPreferences.Languages[0].ToString();
                    if (lang.Contains("-")) lang = lang.Split("-")[0];

                    App.UIThread(() =>
                    {
                        for (int i = 0; i < response.Count; i++)
                        {
                            if (i == 0 || response[i - 1].Created.Year != response[i].Created.Year || response[i - 1].Created.Month != response[i].Created.Month)
                            {
                                this.StackPanel.Children.Add(new TextBlock
                                {
                                    Text = response[i].Created.ToString("MMMM yyyy"),
                                    FontWeight = FontWeights.SemiBold,
                                    FontSize = 25,
                                    Margin = new Thickness(0, 5, 0, 0)
                                });
                            }
                            this.StackPanel.Children.Add(new PaymentElement(response[i], lang));
                            this.StackPanel.Children.Add(new Rectangle
                            {
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                Height = 1,
                                Fill = new SolidColorBrush(Colors.LightGray),
                                Margin = new Thickness(0, 2, 0, 1)
                            });
                        }
                        this.Children.RemoveAt(1);
                    });
                }
            });
        }

        private class PaymentElement : Grid
        {
            public PaymentElement(Libs.APi.Responses.BillingResponse.BillingInfo transaction, string lang)
            {
                this.HorizontalAlignment = HorizontalAlignment.Stretch;

                this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                var date = new TextBlock
                {
                    Text = transaction.Created.ToString("dd.MM HH:mm"),
                    FontSize = 20,
                    Margin = new Thickness(0, 0, 5, 0),
                    FontWeight = FontWeights.Light,
                    Foreground = new SolidColorBrush(Colors.Gray)
                };
                this.Children.Add(date);

                var name = new TextBlock
                {
                    Text = transaction.Description.ContainsKey(lang) ? transaction.Description[lang] : transaction.Description["en"],
                    FontSize = 20
                };
                Grid.SetColumn(name, 1);
                this.Children.Add(name);

                var sum = new TextBlock
                {
                    Text = $"{(transaction.Price > 0 ? "+" : "")}{transaction.Price / 100} ₽",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    FontSize = 20,
                    Foreground = new SolidColorBrush(transaction.State == "DELETED" ? Colors.Gray : transaction.Price < 0 ? Colors.Red : Colors.Green)
                };
                Grid.SetColumn(sum, 2);
                this.Children.Add(sum);
            }
        }
    }
}