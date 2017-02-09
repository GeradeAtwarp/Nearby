using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nearby.Helpers.Converters
{
    public class ToTitleCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((string)value).Length > 0)
            {
                string auxStr = ((string)value).ToLower();
                string[] auxArr = auxStr.Split(' ');
                string result = "";
                bool firstWord = true;
                foreach (string word in auxArr)
                {
                    if (!firstWord)
                        result += " ";
                    else
                        firstWord = false;

                    result += word.Substring(0, 1).ToUpper() + word.Substring(1, word.Length - 1);
                }

                return result;
            }
            else
                return (string)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
