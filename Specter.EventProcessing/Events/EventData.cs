using System;

namespace Specter.EventProcessing.Events
{
    public class EventData : IEventData
    {
        public string DeviceId { get; set; }
        public string Payload { get; set; }
        public DateTime ReceivedOn { get; set; }
    }
}
