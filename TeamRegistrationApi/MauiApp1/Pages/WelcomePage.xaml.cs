namespace MauiApp1.Pages;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent(); 
        LoginPageLoginButton.Clicked += OnLoginButtonClicked;
        CreateAccountButton.Clicked += OnCreateAccountButtonClicked;
        ContinueAsGuestButton.Clicked += OnContinueAsGuestButtonClicked;
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());

    }

    private async void OnCreateAccountButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }

    private async void OnContinueAsGuestButtonClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Guest Mode", "Nastavljate kao gost.", "OK");
        // Navigacija na DashboardPage
        await Navigation.PushAsync(new TeamRegistrationPage());

    }
}