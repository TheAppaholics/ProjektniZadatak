using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace MauiApp1.Pages
{
    public partial class SignUpPage : ContentPage
    {
        private readonly HttpClient _httpClient = new();

        public SignUpPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            MessageLabel.IsVisible = false;

            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text;
            var confirmPassword = ConfirmPasswordEntry.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageLabel.Text = "Please fill all fields.";
                MessageLabel.IsVisible = true;
                return;
            }

            if (password != confirmPassword)
            {
                MessageLabel.Text = "Passwords do not match.";
                MessageLabel.IsVisible = true;
                return;
            }

            var registerRequest = new
            {
                Email = email,
                Password = password
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://10.0.2.2:7225/api/Auth/register", registerRequest);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var registerResponse = JsonSerializer.Deserialize<RegisterResponse>(json);

                    if (registerResponse != null)
                    {
                        await DisplayAlert("Success", registerResponse.Message, "OK");

                        await Navigation.PushAsync(new LoginPage());

                    }
                }
                else
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    MessageLabel.Text = $"Registration failed: {errorMsg}";
                    MessageLabel.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                MessageLabel.Text = "Error: " + ex.Message;
                MessageLabel.IsVisible = true;
            }
        }
        private async void OnLoginTapped(object sender, EventArgs e)
        {
             await Navigation.PushAsync(new LoginPage());
        }

    }

    public class RegisterResponse
    {
        public string Message { get; set; } = string.Empty;
    }
}
