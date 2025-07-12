using MauiApp1.Pages;

namespace MauiApp1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new LoginPage(); // Otvara LoginPage pri pokretanju
            MainPage = new NavigationPage(new SplashScreenPage());

        }
    }
}
