using System;
using TimerApp.Control;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace TimerApp
{
	public partial class App : Application
	{
        public static AppCore appCore;
		public App ()
		{
			InitializeComponent();
            appCore = AppCore.StartAppCore();
            //var navPage = new NavigationPage(new PageOne()) { Title = "Title" };
            var mdp = new View.LandingPage();
            //    new MasterDetailPage()
            //{
            //    Master = new View.StartPageMaster(),
            //    Detail = new View.StartPageDetail()
            //};

            MainPage = mdp;// new NavigationPage( mdp);
        }

		protected override void OnStart ()
		{
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
