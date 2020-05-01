using System;
using System.Text.Json.Serialization;

namespace Specter.EventProcessing.Events
{
    public class Rtl433Message
    {
        [JsonIgnore]
        public DateTimeOffset Time { get; } = DateTimeOffset.UtcNow;

        public string Model { get; set; }

        public int Id { get; set; }

        public int Channel { get; set; }

        public int Event { get; set; }

        public string State { get; set; }

        public int Heartbeat { get; set; }
    }
}
