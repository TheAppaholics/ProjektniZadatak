using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using MauiApp1.Models;

namespace MauiApp1.Pages
{
    public partial class TeamFormPage : ContentPage
    {
        private List<Player> players = new();
        private readonly HttpClient httpClient = new();

        public TeamFormPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterTeamClicked(object sender, EventArgs e)
        {
            string teamName = TournamentNameEntry.Text?.Trim();

            if (string.IsNullOrEmpty(teamName))
            {
                await DisplayAlert("Error", "Please enter a team name.", "OK");
                return;
            }

            if (players.Count == 0)
            {
                await DisplayAlert("Error", "You must add at least one player.", "OK");
                return;
            }

            var dto = new TeamRegistrationDto
            {
                Name = teamName,
                Players = players
            };

            try
            {
                var response = await httpClient.PostAsJsonAsync("http://10.0.2.2:7225/api/teams/register", dto);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", $"Team '{dto.Name}' registered successfully!", "OK");
                    players.Clear();
                    RefreshPlayersUI();
                    TournamentNameEntry.Text = string.Empty;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"Registration failed: {error}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"API error: {ex.Message}", "OK");
            }
        }


        private void OnAddPlayersClicked(object sender, EventArgs e)
        {
            PlayerPopup.IsVisible = true;
        }

        private void OnCancelPopupClicked(object sender, EventArgs e)
        {
            PlayerPopup.IsVisible = false;
            ClearPopupFields();
        }

        private async void OnAddPopupPlayerClicked(object sender, EventArgs e)
        {
            string name = PopupPlayerNameEntry.Text?.Trim();
            bool validAge = int.TryParse(PopupPlayerAgeEntry.Text, out int age);

            if (string.IsNullOrEmpty(name) || !validAge)
            {
                await DisplayAlert("Error", "Enter valid name and age.", "OK");
                return;
            }

            players.Add(new Player
            {
                Name = name,
                Age = age,
                Statistics = new List<PlayerStatistics>() 
            }); 
            RefreshPlayersUI();

            ClearPopupFields();
            PlayerPopup.IsVisible = false;
        }

        private void RefreshPlayersUI()
        {
            PlayersList.Children.Clear();

            foreach (var player in players)
            {
                var row = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 10 };

                row.Children.Add(new Label
                {
                    Text = $"{player.Name} ({player.Age})",
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.Center
                });

                var deleteButton = new Button
                {
                    Text = "🗑️",
                    BackgroundColor = Colors.Transparent,
                    TextColor = Colors.Red,
                    VerticalOptions = LayoutOptions.Center
                };

                deleteButton.Clicked += (s, e) =>
                {
                    players.Remove(player);
                    RefreshPlayersUI();
                };

                row.Children.Add(deleteButton);
                PlayersList.Children.Add(row);
            }
        }

        private void ClearPopupFields()
        {
            PopupPlayerNameEntry.Text = string.Empty;
            PopupPlayerAgeEntry.Text = string.Empty;
            SavePlayerCheckBox.IsChecked = false;
        }

        private async void OnViewTeamsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeamsListPage());
        }

       

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeamRegistrationPage());
        }

        private async void OnScheduleClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MatchSchedulePage());
        }
    }
}
