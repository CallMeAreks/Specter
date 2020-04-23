using Specter.Alarm.System;
using System;

namespace Specter.Alarm.Events
{
    public class Event
    {
        public int Id { get; set; }
        public EventType Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public Sensor Sensor { get; set; }
        public Zone Zone { get; set; }
    }
}
