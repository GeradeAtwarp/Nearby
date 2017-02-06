using Nearby.Cells;
using Nearby.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Nearby.Pages
{
    public partial class SearchFilters : ContentPage
    {
        SearchFiltersViewModel vm;

        public SearchFilters()
        {
            InitializeComponent();

            BindingContext = vm = new SearchFiltersViewModel();

            LoadCategories();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            vm.Save();
        }

        void LoadCategories()
        {
            vm.LoadCategoriesAsync().ContinueWith((result) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (var item in vm.Filters)
                    {
                        TableSectionCategories.Add(new CategoryCell
                        {
                            BindingContext = item
                        });
                    }
                });
            });
        }
    }
}
