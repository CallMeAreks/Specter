namespace Specter.Alarm
{
    public class Sensor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public SensorType Type { get; set; }
        public ZoneType ZoneType { get; set; }
    }

    public enum ZoneType
    {
        EntryExit,
        Perimeter,
        Interior,
        Follower,
        Panic,
        Fire
    }

    public enum SensorType
    {
        Door,
        Window,
        Motion,
        Button
    }
}
