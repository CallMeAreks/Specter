using Specter.Alarm.Enums;

namespace Specter.Alarm.System
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public SensorType Type { get; set; }
        public SensorState State { get; set; }
    }
}