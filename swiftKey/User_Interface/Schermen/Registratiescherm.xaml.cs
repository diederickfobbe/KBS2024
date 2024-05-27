using Microsoft.Maui.Controls;
using System;
using System.Net.Http.Headers;
using Data_Access;
using Business_Logic;

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

            if (!CheckComplexity.EmailValidCheck(email))
            {
                DisplayAlert("Fout", "Dit lijkt niet op een email", "OK");
                return;
            }

            if (!CheckComplexity.PasswordValidCheck(password))
            {
                DisplayAlert("Fout", "Controleer of het wachtwoord minimaal 8 tekens lang is, Controleer of het wachtwoord minstens één hoofdletter bevat, Controleer of het wachtwoord minstens één kleine letter bevat, Controleer of het wachtwoord minstens één cijfer bevat, Controleer of het wachtwoord minstens één speciaal teken bevat", "OK");
                return;
            }


            try
            {
                // Probeer een nieuwe gebruiker te maken
                string hashedPassword = Business_Logic.RegisterChecks.HashPassword(password);
               
                
                Data_Access.RegisterHandler.RegisterUser(username, email, hashedPassword);
               

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
