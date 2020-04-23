﻿namespace Specter.Data.Models
{
    public class Event : Entity
    {
        public int Type { get; set; }
        public int Timestamp { get; set; }
        public string Sensor_Id { get; set; }
        public int Zone_Id { get; set; }
    }
}
