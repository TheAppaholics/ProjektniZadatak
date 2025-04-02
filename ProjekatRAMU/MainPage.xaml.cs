using System;
using Microsoft.Maui.Controls;

namespace ProjekatRAMU
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // LOGIN FUNKCIJA
        private void OnLoginClicked(object sender, EventArgs e)
        {
            string email = emailEntry.Text;    
            string password = passwordEntry.Text;  

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                DisplayAlert("Greška", "Unesite email i lozinku.", "OK");
                return;
            }

            if (email == "test@gmail.com" && password == "123456")
            {
                DisplayAlert("Uspješno", "Uspješna prijava!", "OK");
            }
            else
            {
                DisplayAlert("Greška", "Pogrešan email ili lozinka.", "OK");
            }
        }
    }
}
