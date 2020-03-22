using Specter.Alarm.Enums;

namespace Specter.Alarm
{
    public class Sensor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public SensorType Type { get; set; }
        public SensorState State { get; set; }
    }
}