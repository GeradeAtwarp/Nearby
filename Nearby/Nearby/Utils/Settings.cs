using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nearby.Utils
{
    public class Settings : INotifyPropertyChanged
    {
        static ISettings AppSettings
        {
            get { return CrossSettings.Current; }
        }

        static Settings settings;

        public static Settings Current
        {
            get { return settings ?? (settings = new Settings()); }
        }

        //Settings
        #region Is custom location enabled

        const string IsCustomLocationEnabledkey = "custom_location_enabled";
        static readonly bool IsCustomLocationEnabledDefault = false;
        public bool CustomLocationEnabled
        {
            get { return AppSettings.GetValueOrDefault<bool>(IsCustomLocationEnabledkey, IsCustomLocationEnabledDefault); }
            set
            {
                if (AppSettings.AddOrUpdateValue<bool>(IsCustomLocationEnabledkey, value))
                    OnPropertyChanged();
            }
        }

        #endregion

        #region Custom latitude

        const string CustomLatitudekey = "custom_latitude";
        static readonly string CustomLatitudeDefault = "";
        public string CustomLatitude
        {
            get { return AppSettings.GetValueOrDefault<string>(CustomLatitudekey, CustomLatitudeDefault); }
            set
            {
                if (AppSettings.AddOrUpdateValue<string>(CustomLatitudekey, value))
                    OnPropertyChanged();
            }
        }

        #endregion

        #region Custom longitude

        const string CustomLongitudekey = "custom_longitude";
        static readonly string CustomLongitudeDefault = "";
        public string CustomLongitude
        {
            get { return AppSettings.GetValueOrDefault<string>(CustomLongitudekey, CustomLongitudeDefault); }
            set
            {
                if (AppSettings.AddOrUpdateValue<string>(CustomLongitudekey, value))
                    OnPropertyChanged();
            }
        }

        #endregion













        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
