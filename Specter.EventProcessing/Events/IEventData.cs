using System;

namespace Specter.EventProcessing.Events
{
    public interface IEventData
    {
        string DeviceId { get; set; }
        string Payload { get; set; }
        DateTime ReceivedOn { get; set; }
    }
}
