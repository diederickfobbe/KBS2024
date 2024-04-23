using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access;

namespace Business_Logic
{
    public class UserManager
    {


        public void CreateUser(string username, string email, string password)
        {
            // Controleer of de gebruiker al bestaat
            if (UserExists(username))
            {
                throw new Exception("Gebruikersnaam is al in gebruik");
            }

            // Maak een nieuwe gebruiker
            User newUser = new User(username, email, password);
            
            // Voeg de nieuwe gebruiker toe aan de lijst met gebruikers

        }


        //moet nog gemaakt worden
        private bool UserExists(string username)
        {
            // Controleer of de gebruiker al bestaat in de lijst met gebruikers
            return false;
        }
    }
}
