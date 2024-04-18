using Microsoft.Maui.Controls;

namespace User_Interface
{
    public partial class Loginscherm : ContentPage
    {
        public Loginscherm()
        {
            InitializeComponent();
        }

        private void Button_OnLoginClicked(object? sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                // Gebruiker heeft geen gebruikersnaam of wachtwoord ingevoerd
                DisplayAlert("Fout", "Voer een gebruikersnaam en een wachtwoord in", "OK");
            }
            else if (username.Equals("admin") && password.Equals("admin"))
            {
                // Inloggen als admin
                Navigation.PushAsync(new Oefenscherm());
            }
            else
            {
                // Ongeldige inloggegevens
                DisplayAlert("Fout", "Ongeldige inloggegevens", "OK");
            }
        }

        private void Button_OnRegisterClicked(object? sender, EventArgs e)
        {
            // Navigeer naar de registratiepagina
            Navigation.PushAsync(new Registratiescherm());
        }
    }
}
