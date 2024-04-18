using Microsoft.Maui.Controls;
using System;

namespace User_Interface
{
    public partial class Registratiescherm : ContentPage
    {
        public Registratiescherm()
        {
            InitializeComponent();
        }

        private void Button_OnRegisterClicked(object? sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;
            string repeatPassword = RepeatPasswordEntry.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(repeatPassword))
            {
                // Als een van de velden leeg is, toon dan een melding
                DisplayAlert("Fout", "Vul alle velden in", "OK");
                return;
            }

            if (!password.Equals(repeatPassword))
            {
                // Als wachtwoorden niet overeenkomen, toon dan een melding
                DisplayAlert("Fout", "Wachtwoorden komen niet overeen", "OK");
                return;
            }

            // Registratielogica hier
            // Hier zou je de gebruikersgegevens kunnen opslaan in een database of ergens anders

            // Voorbeeld: Toon een melding na succesvolle registratie
            DisplayAlert("Succes", "Account succesvol geregistreerd voor " + username, "OK");
        }
    }
}
