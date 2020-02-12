using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Pi.HifiControl.Client.Services;
using Pi.HifiControl.Client.Views;

namespace Pi.HifiControl.Client
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
