using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using MauiApp1.Models; 
using Microsoft.Maui.Controls;

namespace MauiApp1.Pages
{
    public partial class LiveMatchesPage : ContentPage
    {
        private readonly HttpClient _httpClient = new();

        public LiveMatchesPage()
        {
            InitializeComponent();
            LoadMatches();
        }

        private async void LoadMatches()
        {
            try
            {
                var allMatches = await _httpClient.GetFromJsonAsync<List<MatchDto>>("http://10.0.2.2:7225/api/matches");

                var now = DateTime.Now;

                // Saberi golove po rundama ako postoje
                foreach (var match in allMatches)
                {
                    match.HomeScore = match.RoundResults?.Sum(r => r.HomeScore ?? 0) ?? 0;
                    match.AwayScore = match.RoundResults?.Sum(r => r.AwayScore ?? 0) ?? 0;
                }

                // LIVE = počele i još traju (90 minuta)
                var liveMatches = allMatches
                    .Where(m =>
                        m.MatchDate <= now &&                           // počela je
                        now <= m.MatchDate.AddMinutes(90))              // i još traje
                    .OrderBy(m => m.MatchDate)
                    .ToList();

                // UPCOMING = nisu još počele
                var upcomingMatches = allMatches
                    .Where(m => m.MatchDate > now)                      // tek će početi
                    .OrderBy(m => m.MatchDate)
                    .ToList();

                // PAST = završile (prošlo više od 90 minuta od početka)
                var pastMatches = allMatches
                    .Where(m => now > m.MatchDate.AddMinutes(90))        // završila je
                    .OrderByDescending(m => m.MatchDate)
                    .ToList();

                // Bindaj na CollectionView
                LiveMatchesCollectionView.ItemsSource = liveMatches;
                UpcomingMatchesCollectionView.ItemsSource = upcomingMatches;
                PastMatchesCollectionView.ItemsSource = pastMatches;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
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



        public class MatchViewModel
        {
            public string TeamAName { get; set; }
            public string TeamBName { get; set; }
            public int ScoreA { get; set; }
            public int ScoreB { get; set; }
        }
    }
}
