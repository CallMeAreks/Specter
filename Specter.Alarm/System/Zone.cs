using Specter.Alarm.Enums;
using System.Collections.Generic;

namespace Specter.Alarm.System
{
    public class Zone
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public ZoneType Type { get; set; }
        public TriggerType TriggerType { get; set; }
        public ICollection<Sensor> Sensors { get; set; }
    }
}