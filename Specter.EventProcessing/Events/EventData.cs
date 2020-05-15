using System;
using System.Text.Json.Serialization;

namespace Specter.EventProcessing.Events
{
    public class EventData : IEventData
    {
        [JsonPropertyName("Id")]
        public int DeviceId { get; set; }

        [JsonPropertyName("Event")]
        public int EventType { get; set; }

        [JsonIgnore]
        public DateTime ReceivedOn { get; set; } = DateTime.UtcNow;
    }
}
