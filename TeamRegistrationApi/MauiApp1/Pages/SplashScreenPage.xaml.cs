using MauiApp1.Pages;

namespace MauiApp1.Pages;

public partial class SplashScreenPage : ContentPage
{
	public SplashScreenPage()
	{
		InitializeComponent();
	}
        private async void OnStartButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WelcomePage());
    }
}