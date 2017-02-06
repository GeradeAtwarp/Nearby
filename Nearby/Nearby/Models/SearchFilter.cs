using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearby.Models
{
    public class SearchFilter : ObservableObject
    {
        /// <summary>
        /// Gets or sets the name that is displayed during filtering
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the short name/code that is displayed on the sessions page.
        /// For instance the short name for Xamarin.Forms is X.Forms
        /// </summary>
        /// <value>The short name.</value>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the color in Hex form, for instance #FFFFFF
        /// </summary>
        /// <value>The color.</value>
        public string Color { get; set; }
        
        bool filtered;
        public bool IsFiltered
        {
            get { return filtered; }
            set { SetProperty(ref filtered, value); }
        }
        
        public string BadgeName => string.IsNullOrWhiteSpace(ShortName) ? Name : ShortName; 
       
    }
}
