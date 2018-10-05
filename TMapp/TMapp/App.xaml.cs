using System;
using TMapp.Models;
using TMapp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace TMapp
{
    public partial class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }
        public static User CurrentUser { get; set; }

		public App ()
		{
			InitializeComponent();

		}

		protected override void OnStart ()
		{
            MainPage = new NavigationPage(new Login());
            // Handle when your app starts
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
