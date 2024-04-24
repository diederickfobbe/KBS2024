using Business_Logic;
using Microsoft.Maui.Controls;
using System;
using System.Net.Http.Headers;
using Data_Access;
namespace User_Interface
{
    public partial class Registratiescherm : ContentPage
    {
        
        private UserManager userManager = new UserManager();


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

            try
            {
                // Probeer een nieuwe gebruiker te maken
                string hashedPassword = Business_Logic.RegisterChecks.HashPassword(password);
               
                userManager.CreateUser(username, email, hashedPassword);
                Data_Access.RegisterUser.InsertUser(username, email, password);
               

                // Toon een melding na succesvolle registratie
                DisplayAlert("Succes", "Account succesvol geregistreerd voor " + username, "OK");

            }
            catch (Exception ex)
            {
                // Als er een fout optreedt tijdens het maken van de gebruiker, toon dan een foutmelding
                DisplayAlert("Fout", ex.Message, "OK");
            }
        }
    }
}
