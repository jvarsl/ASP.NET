using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SalonWithRazor.Tools
{
    /// <summary>
    /// TimeSpans are not serialized consistently depending on what properties are present. So this 
    /// serializer will ensure the format is maintained no matter what.
    /// </summary>
    public class TimespanConverter : JsonConverter<TimeSpan?>
    {
        /// <summary>
        /// Format: Hours:Minutes
        /// </summary>
        public const string TimeSpanFormatString = @"hh\:mm";

        public override void WriteJson(JsonWriter writer, TimeSpan? value, JsonSerializer serializer)
        {
            var timespanFormatted = $"{value.Value.ToString(TimeSpanFormatString)}";
            writer.WriteValue(timespanFormatted);
        }

        public override TimeSpan? ReadJson(JsonReader reader, Type objectType, TimeSpan? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            TimeSpan parsedTimeSpan;
            TimeSpan.TryParseExact((string)reader.Value, TimeSpanFormatString, null, out parsedTimeSpan);
            TimeSpan? parsedTimeSpanNullable = parsedTimeSpan;
            return parsedTimeSpanNullable;

        }
    }
}