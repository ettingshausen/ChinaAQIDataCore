using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ChinaAQIDataCore.Models
{
    [Table("AQI")]
    public class AQIDTO
    {
        public string AQI { get; set; }
        public string Area { get; set; }
        public string CO { get; set; }
        public string CO_24h { get; set; }
        public string CityCode { get; set; }
        public string CityPinyin { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Measure { get; set; }
        public string NO2 { get; set; }
        public string NO2_24h { get; set; }
        public string O3 { get; set; }
        public string O3_24h { get; set; }
        public string O3_8h { get; set; }
        public string O3_8h_24h { get; set; }
        public string OrderId { get; set; }
        [JsonProperty(PropertyName = "pm10")]
        public string PM10 { get; set; }
        [JsonProperty(PropertyName = "pm10_24h")]
        public string PM10_24h { get; set; }
        [JsonProperty(PropertyName = "pm2_5")]
        public string PM2_5 { get; set; }
        [JsonProperty(PropertyName = "pm2_5_24h")]
        public string PM2_5_24h { get; set; }
        public string PositionName { get; set; }
        public string PrimaryPollutant { get; set; }
        public string ProvinceId { get; set; }
        public string Quality { get; set; }
        [JsonProperty(PropertyName = "so2")]
        public string SO2 { get; set; }
        [JsonProperty(PropertyName = "so2_24h")]
        public string SO2_24h { get; set; }
        public string StationCode { get; set; }
        public string TimePoint { get; set; }
        public string Unheathful { get; set; }

    }

    class AQIEqualityComparer : IEqualityComparer<AQIDTO>
    {
        public bool Equals(AQIDTO x, AQIDTO y) => x.Area == y.Area && x.StationCode == y.StationCode && x.TimePoint == y.TimePoint;
        public int GetHashCode(AQIDTO obj) => (obj.Area + obj.StationCode + obj.TimePoint).GetHashCode();
    }
}
