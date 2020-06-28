using System.Collections.Generic;
using Specter.Models.Enums;
using Specter.Models.System;

namespace Specter.EventProcessing
{
    public sealed class AlarmSystem
    {
        private static readonly AlarmSystem instance = new AlarmSystem();
        private readonly Alarm _alarm;
        public static AlarmSystem Instance => instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static AlarmSystem() { }

        private AlarmSystem()
        {
            _alarm = new Alarm();
        }
        
        public AlarmState State => _alarm.State;
        public IEnumerable<AlarmCommand> AvailableCommands => _alarm.AvailableCommands;
        public IEnumerable<Zone> Zones => _alarm.Zones;
        
        public void ArmAway() => _alarm.ArmAway();
        public void ArmHome() => _alarm.ArmHome();
        public void Disarm()  => _alarm.Disarm();
    }
}
