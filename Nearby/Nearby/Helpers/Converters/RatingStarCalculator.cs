using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nearby.Helpers.Converters
{
    public class RatingStarCalculator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)parameter == "1")
            {
                if ((int)value >= 1)
                    return ImageSource.FromFile("star-filled.png");
                else
                    return ImageSource.FromFile("star-empty.png");
            }

            if ((string)parameter == "2")
            {
                if ((int)value >= 2)
                    return ImageSource.FromFile("star-filled.png");
                else
                    return ImageSource.FromFile("star-empty.png");
            }

            if ((string)parameter == "3")
            {
                if ((int)value >= 3)
                    return ImageSource.FromFile("star-filled.png");
                else
                    return ImageSource.FromFile("star-empty.png");
            }

            if ((string)parameter == "4")
            {
                if ((int)value >= 4)
                    return ImageSource.FromFile("star-filled.png");
                else
                    return ImageSource.FromFile("star-empty.png");
            }

            if ((string)parameter == "5")
            {
                if ((int)value >= 5)
                    return ImageSource.FromFile("star-filled.png");
                else
                    return ImageSource.FromFile("star-empty.png");
            }

            return ImageSource.FromFile("star-empty.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
