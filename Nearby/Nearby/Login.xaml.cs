using Nearby.Pages.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Nearby
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();

        }

        void OnLoginClicked(object sender, EventArgs e)
        {
            // Use a custom renderer to display the authentication UI
            Navigation.PushModalAsync(new Authenticate());
        }
    }
}
