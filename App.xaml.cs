using Selectel.Libs.Api;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Security.Credentials;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Selectel
{
    public sealed partial class App : Application
    {
        public static SelectelApi Selectel;
        public static Login Login;
        public static MainPage Main;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    if (this.DoesTokenExists())
                    {
                        this.LoadMain();
                        rootFrame.Content = App.Main;
                    }
                    else
                    {
                        App.Login = new Login();
                        App.Login.OnSuccess += () =>
                        {
                            this.LoadMain();
                            rootFrame.Content = App.Main;
                            App.Login = null;
                        };
                        rootFrame.Content = App.Login;
                    }
                }
                Window.Current.Activate();
            }
        }

        private void LoadMain()
        {
            App.Selectel = new SelectelApi(new PasswordVault().Retrieve("selectel", "default").Password);
            App.Main = new MainPage();
        }

        private bool DoesTokenExists()
        {
            try
            {
                var vault = new PasswordVault();
                return vault.Retrieve("selectel", "default") != null;
            }
            catch
            {
                return false;
            }
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        public static async void UIThread(DispatchedHandler action, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal) => await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(priority, action);
    }
}