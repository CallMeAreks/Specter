using System;

namespace Specter.Data.Models
{
    public class Log : Entity
    {
        public string Message { get; set; }
        public int Level { get; set; }
        public string Component { get; set; }
        public int Timestamp { get; set; }
    }

    public enum LogLevel
    {
        Debug,
        System
    }
}
