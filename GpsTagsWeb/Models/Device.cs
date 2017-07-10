using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace GpsTagsWeb.Models
{
    public class Device
    {
        [Key, JsonIgnore]
        public int Id { get; set; }
        [Required]
        public string DeviceName { get; set; }
        public string Description { get; set; }

        //public ApplicationUser User { get; set; }
    }
}