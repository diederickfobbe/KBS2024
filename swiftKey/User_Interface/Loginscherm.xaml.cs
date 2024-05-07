using Data_Access;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using Business_Logic;

namespace User_Interface
{
    public partial class Loginscherm : ContentPage
    {
        public Loginscherm()
        {
            new RegisterHandler();
            InitializeComponent();
        }

        private void Button_OnLoginClicked(object? sender, EventArgs e)
        {
           

            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;

            if (Business_Logic.LoginChecks.CheckLogin(email, password))
            {
                // Gebruiker heeft juiste gebruikersnaam en wachtwoord ingevoerd
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