using System;
using System.Collections.Generic;
using System.Data;
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
        
        public int AverageWpm { get; set; }
        public double AverageAccuracy { get; set; }
        public int ExercisesCount { get; set; }


        public User(string Username, string Email)
        {
            this.Username = Username;
            this.Email = Email;
            this.Id = Data_Access.GetUserInfo.GetUserIDByEmail(Email);
        }


        public void RefreshStats()
        {
            this.AverageWpm = Data_Access.GetUserInfo.GetAverageWPM(Id);
            this.AverageAccuracy = Data_Access.GetUserInfo.GetAverageAccuracy(Id);
            this.ExercisesCount = Data_Access.GetUserInfo.GetExercisesCount(Id);
        }
        

    }
}
