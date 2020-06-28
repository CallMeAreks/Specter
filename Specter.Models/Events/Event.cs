using System;
using Specter.Models.System;

namespace Specter.Models.Events
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
