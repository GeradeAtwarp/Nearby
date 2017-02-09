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
            if ((double)value <= 1.5)
            {
                return ImageSource.FromFile("star_filled.png");
            }

            if ((double)value > 1.5 && (double)value <= 2.5)
            {
                return ImageSource.FromFile("star_filled.png");
            }

            if ((double)value > 2.5 && (double)value <= 3.5)
            {
                return ImageSource.FromFile("star_filled.png");
            }

            if ((double)value > 3.5 && (double)value <= 4.5)
            {
                return ImageSource.FromFile("star_filled.png");
            }

            if ((double)value > 4.5 && (double)value <= 5.0)
            {
                return ImageSource.FromFile("star_filled.png");
            }

            return ImageSource.FromFile("star_empty.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
