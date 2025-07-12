using MauiApp1.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace MauiApp1.Pages;

public partial class MatchDetailPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    public ObservableCollection<MatchRoundResultDto> RoundResults { get; set; } = new();


    public MatchDto Match { get; set; }
    public int? NewRoundNumber { get; set; }
    public int? NewHomeScore { get; set; }
    public int? NewAwayScore { get; set; }

    public Command SaveRoundResultCommand { get; }

    public Command<MatchRoundResultDto> EditRoundResultCommand { get; }

    public string TotalScore => "Total score";

    public MatchDetailPage(int matchId)
    {
        InitializeComponent();
        SaveRoundResultCommand = new Command(async () => await SaveRoundResult());
        BindingContext = this;

        _ = LoadMatch(matchId);

        EditRoundResultCommand = new Command<MatchRoundResultDto>(async (round) =>
        {
            await EditRoundResult(round);
        });

    }

    private async Task LoadMatch(int matchId)
    {
        try
        {
            var match = await _httpClient.GetFromJsonAsync<MatchDto>($"http://10.0.2.2:7225/api/matches/{matchId}");
            if (match != null)
            {
                Match = match;

                RoundResults.Clear();
                foreach (var round in match.RoundResults)
                {
                    RoundResults.Add(round);
                }

                OnPropertyChanged(nameof(Match));
                //OnPropertyChanged(nameof(TotalScore));
                OnPropertyChanged(nameof(RoundResults));
            }
            else
            {
                await DisplayAlert("Error", "Match not found.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load match: {ex.Message}\n{ex.InnerException?.Message}", "OK");
        }
    }

    private async Task SaveRoundResult()
    {
        if (NewRoundNumber == null || NewRoundNumber <= 0)
        {
            await DisplayAlert("Error", "Enter a valid round number (greater than 0).", "OK");
            return;
        }

        var roundResultDto = new MatchRoundResultDto
        {
            RoundNumber = NewRoundNumber.Value,
            HomeScore = NewHomeScore,
            AwayScore = NewAwayScore
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync($"http://10.0.2.2:7225/api/matches/{Match.MatchId}/rounds", roundResultDto);
            if (response.IsSuccessStatusCode)
            {
                await LoadMatch(Match.MatchId);
                await DisplayAlert("Success", "The round result has been saved.", "OK");

                // Resetuj polja
                NewRoundNumber = null;
                NewHomeScore = null;
                NewAwayScore = null;

                OnPropertyChanged(nameof(NewRoundNumber));
                OnPropertyChanged(nameof(NewHomeScore));
                OnPropertyChanged(nameof(NewAwayScore));
            }
            else
            {
                await DisplayAlert("Error", "Error while saving the result.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private void OnDeleteRoundClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is MatchRoundResultDto round)
        {
            if (RoundResults.Contains(round))
            {
                RoundResults.Remove(round);
            }
        }
    
}
    private async Task EditRoundResult(MatchRoundResultDto round)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"http://10.0.2.2:7225/api/matches/{Match.MatchId}/rounds", round);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "The round result has been updated.", "OK");
                await LoadMatch(Match.MatchId);
            }
            else
            {
                await DisplayAlert("Error", "Failed to update the round.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
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


    private async void OnUserClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }

}