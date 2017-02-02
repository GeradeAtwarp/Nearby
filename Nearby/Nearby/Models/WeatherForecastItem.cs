using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearby.Models
{
    public class WeatherForecastItem
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string timezone { get; set; }
        public int offset { get; set; }
        public Daily daily { get; set; }

        public class Forecast
        {
            public string summary { get; set; }
            public string icon { get; set; }
            public int sunriseTime { get; set; }
            public int sunsetTime { get; set; }
            public double moonPhase { get; set; }
            public double precipIntensity { get; set; }
            public double precipIntensityMax { get; set; }
            public double precipProbability { get; set; }
            public double temperatureMin { get; set; }
            public int temperatureMinTime { get; set; }
            public double temperatureMax { get; set; }
            public int temperatureMaxTime { get; set; }
            public double apparentTemperatureMin { get; set; }
            public int apparentTemperatureMinTime { get; set; }
            public double apparentTemperatureMax { get; set; }
            public int apparentTemperatureMaxTime { get; set; }
            public double dewPoint { get; set; }
            public double humidity { get; set; }
            public double windSpeed { get; set; }
            public int windBearing { get; set; }
            public double visibility { get; set; }
            public double cloudCover { get; set; }
            public double pressure { get; set; }
            public double ozone { get; set; }
            public int? precipIntensityMaxTime { get; set; }
            public string precipType { get; set; }
            
            [JsonConverter(typeof(MyDateTimeConverter))]
            public DateTime time { get; set; }
        }

        public class Daily
        {
            public string summary { get; set; }
            public string icon { get; set; }
            public List<Forecast> data { get; set; }
        }
    }

    public class MyDateTimeConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var t = (long)reader.Value;
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds((double)t).ToLocalTime();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
