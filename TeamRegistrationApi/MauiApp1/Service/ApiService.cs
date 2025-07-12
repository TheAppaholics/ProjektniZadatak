using MauiApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiApp1.Service
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://10.0.2.2:7225/")
            };
        }

        public async Task<List<MatchResult>> GetMatchResultsAsync()
        {
            var response = await _httpClient.GetAsync("api/matches/results"); // prilagodi ako je drugačiji endpoint
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<MatchResult>>(json);
            }

            return new List<MatchResult>();
        }


        public async Task<List<Match>> GetMatchesAsync()
        {
            var response = await _httpClient.GetAsync("api/matches");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Match>>(json);
            }

            return new List<Match>();
        }


    }
}