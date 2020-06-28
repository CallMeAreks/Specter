using System.Collections.Generic;
using System.Linq;
using Specter.Models.Enums;

namespace Specter.Models.System
{
    public sealed class Alarm
    {
        public Alarm() : this(AlarmState.Disarmed, AlarmDelayConfig.Default) { }
        
        public Alarm(AlarmState initialStatus, AlarmDelayConfig config)
        {
            StateMachine = new AlarmStateMachine(initialStatus);
        }

        public string Id { get; set; }
        public AlarmState State => StateMachine.State;
        public IEnumerable<AlarmCommand> AvailableCommands => StateMachine.Triggers.Where(t => t != AlarmCommand.ArmWithTimer);

        public ICollection<Zone> Zones { get; set; }

        private AlarmStateMachine StateMachine { get; set; }
        private AlarmDelayConfig DelayConfig { get; set; }

        public void ArmAway() => StateMachine.TriggerArm(AlarmCommand.ArmAway, DelayConfig.ArmedAwayDelay);
        public void ArmHome() => StateMachine.TriggerArm(AlarmCommand.ArmHome, DelayConfig.ArmedHomeDelay);
        public void Disarm()  => StateMachine.TriggerDisarm();
    }
}
