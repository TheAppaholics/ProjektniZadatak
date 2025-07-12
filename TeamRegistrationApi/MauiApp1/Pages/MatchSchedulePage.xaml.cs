using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using MauiApp1.Models;
using Microsoft.Maui.Controls;

namespace MauiApp1.Pages
{
    public partial class MatchSchedulePage : ContentPage
    {
        private readonly HttpClient _httpClient = new();

        private List<Team> _teams = new();

        public ObservableCollection<MatchDto> Matches { get; } = new();

        public MatchSchedulePage()
        {
            InitializeComponent();

            MatchesCollectionView.ItemsSource = Matches;

            LoadTeams();
            LoadMatches();
        }

        private async void LoadTeams()
        {
            try
            {
                _teams = await _httpClient.GetFromJsonAsync<List<Team>>("http://10.0.2.2:7225/api/teams");
                HomeTeamPicker.ItemsSource = _teams;
                AwayTeamPicker.ItemsSource = _teams;

                HomeTeamPicker.ItemDisplayBinding = new Binding("Name");
                AwayTeamPicker.ItemDisplayBinding = new Binding("Name");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Could not retrieve teams: " + ex.Message, "OK");
            }
        }

        private async void LoadMatches()
        {
            try
            {
                var matches = await _httpClient.GetFromJsonAsync<List<MatchDto>>("http://10.0.2.2:7225/api/matches");
                Matches.Clear();
                foreach (var match in matches)
                {
                    Matches.Add(match);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Could not retrieve matches: " + ex.Message, "OK");
            }
        }

        private async void OnScheduleMatchClicked(object sender, EventArgs e)
        {
            if (HomeTeamPicker.SelectedItem == null || AwayTeamPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please select both teams.", "OK");
                return;
            }

            var homeTeam = (Team)HomeTeamPicker.SelectedItem;
            var awayTeam = (Team)AwayTeamPicker.SelectedItem;

            if (homeTeam.Id == awayTeam.Id)
            {
                await DisplayAlert("Error", "Home and Away teams cannot be the same.", "OK");
                return;
            }

            DateTime matchDateTime = MatchDatePicker.Date.Date + MatchTimePicker.Time;

            var dto = new MatchDto
            {
                HomeTeamName = homeTeam.Name,
                AwayTeamName = awayTeam.Name,
                MatchDate = matchDateTime,
                Round = "Scheduled"
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://10.0.2.2:7225/api/matches/schedule-single", dto);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", "The match has been scheduled.", "OK");
                    LoadMatches();
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"Scheduling failed: {error}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error while scheduling: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteMatchClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter == null)
                return;

            int matchId = (int)button.CommandParameter;

            bool confirm = await DisplayAlert("Confirmation", "Are you sure you want to delete this match?", "Yes", "No");
            if (!confirm)
                return;

            try
            {
                var response = await _httpClient.DeleteAsync($"http://10.0.2.2:7225/api/matches/{matchId}");
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", "The match has been deleted.", "OK");
                    LoadMatches();
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"Deletion failed: {error}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error while deleting: {ex.Message}", "OK");
            }
        }

        private async void OnMatchSelected(object sender, SelectionChangedEventArgs e)
        {
            var selectedMatch = e.CurrentSelection.FirstOrDefault() as MatchDto;
            if (selectedMatch == null)
                return;

            await Navigation.PushAsync(new MatchDetailPage(selectedMatch.MatchId));

            ((CollectionView)sender).SelectedItem = null;
        }

        private async void OnMatchTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is MatchDto match)
            {
                await Navigation.PushAsync(new MatchDetailPage(match.MatchId));
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
}
