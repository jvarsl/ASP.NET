using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalonWithRazor.Models
{
    public class sp_Hours24Table
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("hour00")]

        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour00 { get; set; }
        [JsonProperty("hour01")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour01 { get; set; }
        [JsonProperty("hour02")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour02 { get; set; }
        [JsonProperty("hour03")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour03 { get; set; }
        [JsonProperty("hour04")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour04 { get; set; }
        [JsonProperty("hour05")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour05 { get; set; }
        [JsonProperty("hour06")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour06 { get; set; }
        [JsonProperty("hour07")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour07 { get; set; }
        [JsonProperty("hour08")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour08 { get; set; }
        [JsonProperty("hour09")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour09 { get; set; }
        [JsonProperty("hour10")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour10 { get; set; }
        [JsonProperty("hour11")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour11 { get; set; }
        [JsonProperty("hour12")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour12 { get; set; }
        [JsonProperty("hour13")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour13 { get; set; }
        [JsonProperty("hour14")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour14 { get; set; }
        [JsonProperty("hour15")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour15 { get; set; }
        [JsonProperty("hour16")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour16 { get; set; }
        [JsonProperty("hour17")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour17 { get; set; }
        [JsonProperty("hour18")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour18 { get; set; }
        [JsonProperty("hour19")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour19 { get; set; }
        [JsonProperty("hour20")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour20 { get; set; }
        [JsonProperty("hour21")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour21 { get; set; }
        [JsonProperty("hour22")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour22 { get; set; }
        [JsonProperty("hour23")]
        [JsonConverter(typeof(Tools.TimespanConverter))]
        public TimeSpan? Hour23 { get; set; }
    }
}
