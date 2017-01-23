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
                else if (value.ToString().ToLower().Contains("performing"))
                    return ImageSource.FromFile("performance_arts.jpg");
                if (value.ToString().ToLower().Contains("comedy"))
                    return ImageSource.FromFile("stand_up.jpg");
                else if (value.ToString().ToLower().Contains("family_fun_kids"))
                    return ImageSource.FromFile("Family_fun_day.jpg");
                else if (value.ToString().ToLower().Contains("movies_film"))
                    return ImageSource.FromFile("movie_theater.jpg");
                else if (value.ToString().ToLower().Contains("social"))
                    return ImageSource.FromFile("social_single.jpg");
                else if (value.ToString().ToLower().Contains("outdoors"))
                    return ImageSource.FromFile("running.jpg");
                else if (value.ToString().ToLower().Contains("sport"))
                    return ImageSource.FromFile("stadium.jpg");
                else if (value.ToString().ToLower().Contains("outdoors_recreation"))
                    return ImageSource.FromFile("recreational_activities.jpg");
                else
                    return ImageSource.FromFile("generic_placeholder.jpg");
            }
            else
                return ImageSource.FromFile("generic_placeholder.jpg");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        
      //
      //
      //
      //
      //
      //_film
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
      //
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
