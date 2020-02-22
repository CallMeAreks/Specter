using System;
using System.Collections.Generic;
using System.Linq;

namespace Specter.Models
{
    public sealed class Alarm
    {
        public Alarm() : this(AlarmStatus.Disarmed) { }
        public Alarm(AlarmStatus initialStatus)
        {
            StateMachine = new AlarmStateMachine(initialStatus);
            // TODO: Remove after debugging or add logging
            StateMachine.OnTransitioned((_) => Console.WriteLine($"Alarm is in state: {Status}. Available triggers: {string.Join(",", AvailableCommands)}"));
        }

        public string Id { get; set; }
        public AlarmStatus Status => StateMachine.Status;
        public int ExitDelay => 5;
        public IEnumerable<AlarmCommand> AvailableCommands => StateMachine.Triggers.Where(t => t != AlarmCommand.ArmWithTimer);
        private AlarmStateMachine StateMachine { get; set; }

        public void ArmAway() => StateMachine.TriggerArm(AlarmCommand.ArmAway, ExitDelay);
        public void ArmHome() => StateMachine.TriggerArm(AlarmCommand.ArmHome, ExitDelay);
        public void Disarm()  => StateMachine.TriggerDisarm();
    }
}
