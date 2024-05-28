using Data_Access;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using User_Interface.Schermen;
using Business_Logic;
using System.Security.Cryptography.X509Certificates;

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
            string hashedPassword = Business_Logic.RegisterChecks.HashPassword(password);

            if (Data_Access.LoginHandler.CheckLogin(email, hashedPassword))
            {

                //maak een user aan om gegevens in op te slaan

                User user = new User(Data_Access.GetUserInfo.GetUserNameFromDB(email), email);

                // Gebruiker heeft juiste gebruikersnaam en wachtwoord ingevoerd

                Navigation.PushAsync(new HomePage(user));

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