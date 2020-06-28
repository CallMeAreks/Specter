using System.Collections.Generic;
using Specter.Models.Enums;

namespace Specter.Models.System
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