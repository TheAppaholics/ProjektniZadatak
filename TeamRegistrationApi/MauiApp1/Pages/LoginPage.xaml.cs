using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using MauiApp1.Pages;
using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class LoginPage : ContentPage
    {
        private readonly HttpClient _httpClient = new();

        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            MessageLabel.IsVisible = false;

            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageLabel.Text = "Please fill email and password.";
                MessageLabel.IsVisible = true;
                return;
            }

            var loginRequest = new
            {
                Email = email,
                Password = password
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://10.0.2.2:7225/api/Auth/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[DEBUG] RAW JSON response: {json}");
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (loginResponse != null)
                    {
                        Debug.WriteLine($"[DEBUG] Email iz login response: {loginResponse.Email}");

                        Preferences.Set("auth_token", loginResponse.Token);
                        Preferences.Set("user_email", loginResponse.Email);

                        if (!string.IsNullOrEmpty(loginResponse.Email))
                        {
                            var username = "@" + loginResponse.Email.Split('@')[0];
                            Debug.WriteLine($"[DEBUG] Username koji se sprema: {username}");
                            Preferences.Set("username", username);
                        }
                        else
                        {
                            Debug.WriteLine("[DEBUG] Email iz login response je PRAZAN");
                            Preferences.Set("username", "@guest");
                        }

                        await DisplayAlert("Success", loginResponse.Message, "OK");
                        Application.Current.MainPage = new AppShell();
                    }

                }
                else
                {
                    MessageLabel.Text = "Wrong email or password.";
                    MessageLabel.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                MessageLabel.Text = "Error: " + ex.Message;
                MessageLabel.IsVisible = true;
                Debug.WriteLine("Login error: " + ex.ToString());
            }
        }
    
        private async void OnSignUpTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty; 

    }
}
