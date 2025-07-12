using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Threading.Tasks;
using MauiApp1.Models;

namespace MauiApp1.Pages
{
    public partial class TeamsListPage : ContentPage
    {
        private readonly HttpClient _httpClient = new();
        public ObservableCollection<TeamRegistrationDto> TeamList { get; set; } = new();

        public TeamsListPage()
        {
            InitializeComponent();
            BindingContext = this;
            _ = LoadTeams();
        }

        private async Task LoadTeams()
        {
            try
            {
                var teamsFromApi = await _httpClient.GetFromJsonAsync<List<TeamRegistrationDto>>("http://10.0.2.2:7225/api/teams");
                TeamList.Clear();

                if (teamsFromApi != null)
                {
                    foreach (var team in teamsFromApi)
                    {
                        TeamList.Add(team);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unable to load teams: {ex.Message}", "OK");
            }
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
        {
            await LoadTeams();
        }

        private async void OnDeleteTeamClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int teamId)
            {
                bool confirmed = await DisplayAlert("Confirmation", "Are you sure you want to delete the team?", "Yes", "No");
                if (!confirmed)
                    return;

                try
                {
                    var response = await _httpClient.DeleteAsync($"http://10.0.2.2:7225/api/teams/{teamId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var teamToRemove = TeamList.FirstOrDefault(t => t.Id == teamId);
                        if (teamToRemove != null)
                            TeamList.Remove(teamToRemove);

                        await DisplayAlert("Success", "The team has been deleted.", "OK");
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        await DisplayAlert("Error", $"Deletion failed: {error}", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
                }
            }
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
}
