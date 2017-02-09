using Nearby.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Nearby.Cells
{
    public class CategoryCell : ViewCell
    {
        public CategoryCell()
        {
            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
                Height = 50;
            else
                Height = 44;
            View = new CategoryCellView();
        }
    }

    public partial class CategoryCellView : ContentView
    {
        public CategoryCellView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var category = BindingContext as SearchFilter;
            if (category == null)
                return;
            if (string.IsNullOrWhiteSpace(category.Color))
            {
                Grid.SetColumn(LabelName, 0);
                Grid.SetColumnSpan(LabelName, 2);
            }
        }
    }
}
