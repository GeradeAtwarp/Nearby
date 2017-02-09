using Nearby.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearby.viewModel
{
    public class SearchFiltersViewModel : NearbyBaseViewModel
    {
        public List<SearchFilter> Filters { get; } = new List<SearchFilter>();

        public SearchFiltersViewModel()
        {

        }

        public async Task LoadCategoriesAsync()
        {
            Filters.Clear();

            List<SearchFilter> allFilters = new List<SearchFilter>()
            {
                new SearchFilter { Color = "", Name = "Restaurant", ShortName = "restaurant" },
                new SearchFilter { Color = "", Name = "Restaurant (Delivery Only)", ShortName = "meal_delivery" },
                new SearchFilter { Color = "", Name = "Accomodation", ShortName = "lodging" },
                new SearchFilter { Color = "", Name = "Parking", ShortName = "parking" },
                new SearchFilter { Color = "", Name = "Movie Theater", ShortName = "movie_theater" },
                new SearchFilter { Color = "", Name = "Liquor Store", ShortName = "liquor_store" },
                new SearchFilter { Color = "", Name = "Coffe Shop", ShortName = "cafe" },
                new SearchFilter { Color = "", Name = "Night Club", ShortName = "night_club" },
                new SearchFilter { Color = "", Name = "Mueseum", ShortName = "museum" },
                new SearchFilter { Color = "", Name = "Library", ShortName = "library" },
                new SearchFilter { Color = "", Name = "Gym", ShortName = "gym" },
                new SearchFilter { Color = "", Name = "Police Statiom", ShortName = "police" },
                new SearchFilter { Color = "", Name = "Doctor", ShortName = "doctor" },
                new SearchFilter { Color = "", Name = "Hospital", ShortName = "hospital" },
            };

            foreach (var filter in allFilters)
            {
                filter.IsFiltered = Settings.SearchFilters.Contains(filter.ShortName);

                filter.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "IsFiltered")
                        SetShowAllCategories(sender);
                };

                Filters.Add(filter);
            }

            Save();
        }

        private void SetShowAllCategories(object selectedFilter)
        {
            var selected = selectedFilter as SearchFilter;

            if (selected.IsFiltered)
            {
                foreach (var category in Filters.Where(x => x.IsFiltered && x.Name != selected.Name))
                {
                    category.IsFiltered = false;
                }
            }
        }

        public void Save()
        {
            Settings.SearchFilters = string.Join("|", Filters?.Where(c => c.IsFiltered).Select(c => c.ShortName));
        }
    }
}
