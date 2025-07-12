using MauiApp1.Models;

namespace MauiApp1.Pages
{
    public partial class AddPlayerPage : ContentPage
    {
        public event EventHandler<Player>? PlayerAdded;

        public AddPlayerPage()
        {
            InitializeComponent();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(); 
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PlayerNameEntry.Text) || !int.TryParse(PlayerAgeEntry.Text, out int age))
            {
                await DisplayAlert("Error", "Please enter valid name and age.", "OK");
                return;
            }

            var newPlayer = new Player
            {
                Name = PlayerNameEntry.Text,
                Age = age
            };

            PlayerAdded?.Invoke(this, newPlayer);
            await Navigation.PopModalAsync();
        }
    }
}
