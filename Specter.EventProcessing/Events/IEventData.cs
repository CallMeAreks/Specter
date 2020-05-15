using System;

namespace Specter.EventProcessing.Events
{
    public interface IEventData
    {
        int DeviceId { get; set; }
        int EventType { get; set; }
        DateTime ReceivedOn { get; set; }
    }
}
