using System;
using System.Globalization;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;

namespace User_Interface.Converters
{
    public class DifficultyToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string difficulty)
            {
                switch (difficulty.ToLower())
                {
                    case "hard":
                        return Colors.Red;
                    case "medium":
                        return Colors.Orange;
                    case "easy":
                        return Colors.Green;
                    default:
                        return Colors.Gray; // Default color if no match found
                }
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
