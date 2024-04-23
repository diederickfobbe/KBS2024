using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; }


        public User(string Username, string Email, string Password)
        {
            this.Username = Username;
            this.Email = Email;
            this.Password = Password;
        }
    }
}
