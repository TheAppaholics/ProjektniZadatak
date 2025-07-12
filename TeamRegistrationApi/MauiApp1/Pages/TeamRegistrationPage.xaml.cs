namespace MauiApp1.Pages;

public partial class TeamRegistrationPage : ContentPage
{
    public TeamRegistrationPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

    }

    private async void OnRegisterTeamClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TeamFormPage());
    }


    private async void OnUserClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TeamRegistrationPage());
    }

    private async void OnScheduleClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MatchSchedulePage());
    }

    private async void OnLiveMatchesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LiveMatchesPage());
    }

}
