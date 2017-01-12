using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nearby.Helpers.Converters
{
    public class EventImageSelector : IValueConverter
    {
        private string[] foodeventtags = new[] { "food", "cook", "cooking", "wine", "cookout", "grill", "kitchen", "cafe", "restaurant", "meal", "dinner", "tasting","lunch" };
        private string[] concertventtags = new[] { "concert", "tour", "live", "music", "show", "festival"};

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value != "")
            {
                if (value.ToString().ToLower().Contains("music"))
                    return ImageSource.FromFile("concert.jpg");
                else if (value.ToString().ToLower().Contains("conference"))
                    return ImageSource.FromFile("conference.jpg");
                else if (value.ToString().ToLower().Contains("food"))
                    return ImageSource.FromFile("wine_food.jpg");
                else if (value.ToString().ToLower().Contains("outdoors"))
                    return ImageSource.FromFile("running.jpg");
                else if (value.ToString().ToLower().Contains("performing"))
                    return ImageSource.FromFile("performing_arts.jpg");
                else
                    return ImageSource.FromFile("stadium.jpg");
            }
            else
                return ImageSource.FromFile("stadium.jpg");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        
      //
      //comedy
      //learning_education
      //family_fun_kids
      //festivals_parades
      //movies_film
      //
      //fundraisers
      //art
      //support
      //holiday
      //books
      //attractions
      //community
      //business
      //singles_social
      //schools_alumni
      //clubs_associations
      //outdoors_recreation
      //performing_arts
      //animals
      //politics_activism
      //sales
      //science
      //religion_spirituality
      //sports
      //technology
      //other
    }
}
