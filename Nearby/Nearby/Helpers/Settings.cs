// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nearby.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public class Settings
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

        #region Custom location

        const string CustomLocationkey = "custom_location";
        static readonly string CustomLocationDefault = "";
        public string CustomLocation
        {
            get { return AppSettings.GetValueOrDefault<string>(CustomLocationkey, CustomLocationDefault); }
            set
            {
                if (AppSettings.AddOrUpdateValue<string>(CustomLocationkey, value))
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