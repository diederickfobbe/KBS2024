using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using User_Interface.Schermen;

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
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;


            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                // Gebruiker heeft geen gebruikersnaam of wachtwoord ingevoerd
                DisplayAlert("Fout", "Voer een gebruikersnaam en een wachtwoord in", "OK");
            }
            else if (email.Equals("admin") && password.Equals("admin"))
            {
                // Inloggen als admin
                Navigation.PushAsync(new SelecterenOefening());
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