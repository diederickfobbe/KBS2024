using System.Net.Mail;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic
{
    public class CheckComplexity
    {
        public static bool EmailValidCheck(string email)
        {
            bool isValid = false;
            try
            {
                var mailAddress = new MailAddress(email);
                isValid = true;
            }
            catch (FormatException)
            {
                isValid = false;
            }
            return isValid;
        }

        public static bool PasswordValidCheck(string password)
        {
            // Controleer of het wachtwoord minimaal 8 tekens lang is
            if (password.Length < 8)
            {
                return false;
            }

            // Controleer of het wachtwoord minstens één hoofdletter bevat
            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                return false;
            }

            // Controleer of het wachtwoord minstens één kleine letter bevat
            if (!Regex.IsMatch(password, "[a-z]"))
            {
                return false;
            }

            // Controleer of het wachtwoord minstens één cijfer bevat
            if (!Regex.IsMatch(password, "[0-9]"))
            {
                return false;
            }

            // Controleer of het wachtwoord minstens één speciaal teken bevat
            if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                return false;
            }

            // Als aan alle eisen is voldaan, retourneer dan true
            return true;
        }

    }
}

