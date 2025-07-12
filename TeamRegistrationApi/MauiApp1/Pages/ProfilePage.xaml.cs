using System.ComponentModel;

namespace MauiApp1.Pages;

public partial class ProfilePage : ContentPage, INotifyPropertyChanged
{
    private string _username;
    public string Username
    {
        get => _username;
        set
        {
            if (_username != value)
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = this;

        Username = Preferences.Get("username", "@guest");
    }

    private async void OnRegisterTeamClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TeamFormPage());
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


    private async void OnUserClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }
}
