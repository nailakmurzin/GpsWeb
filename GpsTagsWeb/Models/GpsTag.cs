using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GpsTagsWeb.Models
{
    public class GpsTag
    {
        [JsonIgnore]
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public int RemainingChargePercent { get; set; }

        public byte BatteryStatus { get; set; }

        public string DeviceName { get; set; }

        [JsonIgnore]
        public ApplicationUser User { get; set; }
    }

    public class GpsTagViewModel
    {
        public DateTime DateTime { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public int RemainingChargePercent { get; set; }

        public byte BatteryStatus { get; set; }

        public string DeviceName { get; set; }
    }

    public static class GpsTagExtensions
    {
        public static GpsTag ConvertToUser(this GpsTagViewModel viewModel, ApplicationUser user) => new GpsTag
        {
            DateTime = viewModel.DateTime,
            BatteryStatus = viewModel.BatteryStatus,
            DeviceName = viewModel.DeviceName,
            Latitude = viewModel.Latitude,
            Longitude = viewModel.Longitude,
            RemainingChargePercent = viewModel.RemainingChargePercent,
            User = user
        };
    }
}