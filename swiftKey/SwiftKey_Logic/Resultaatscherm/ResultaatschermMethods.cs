using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic
{
    public class ResultaatschermMethods
    {
        public static int calculateMistakes(string enteredText, string targetText)
        {
            var a = targetText.ToCharArray();
            var b = enteredText.ToCharArray();
            int incorrectWords = 0;
            int smallerArray;
            if (a.Count() >= b.Count()) { smallerArray = b.Count(); } else { smallerArray = a.Count(); }
            for (int i = 0; i < smallerArray; i++)
            {

                if (a[i] != b[i]) { incorrectWords++; }
            }

            return incorrectWords;
        }
    }
}
