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
        
        public int AverageWpm { get; set; }
        public double AverageAccuracy { get; set; }
        public int ExercisesCount { get; set; }


        public User(string Username, string Email)
        {
            this.Username = Username;
            this.Email = Email;
        }

        public void CalculateAverageWpm()
        {
            int AverageWpm = Data_Access.GetUserInfo.GetWPM(Id);
        }

        public void CalculateAverageAccuracy()
        {
            int AverageAccuracy = Data_Access.GetUserInfo.GetAccuracy(Id);
        }

        public void CalculateExercisesCount()
        {
            int ExercisesCount = Data_Access.GetUserInfo.GetExercisesCount(Id);

        }
    }
}
