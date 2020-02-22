using Stateless;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Specter.Models
{
    internal sealed class AlarmStateMachine
    {
        internal IEnumerable<AlarmCommand> Triggers => StateMachine.PermittedTriggers;
        internal AlarmStatus Status => StateMachine.State;

        private StateMachine<AlarmStatus, AlarmCommand>.TriggerWithParameters<AlarmCommand, int> ArmingParameters { get; set; }
        private StateMachine<AlarmStatus, AlarmCommand> StateMachine { get; set; }
        private bool AllowAccessToArm { get; set; }

        internal AlarmStateMachine(AlarmStatus initialStatus)
        {
            InitializeStateMachine(initialStatus);
        }

        internal void TriggerArm(AlarmCommand mode, int pendingTime)
        {
            if (mode != AlarmCommand.ArmAway && mode != AlarmCommand.ArmHome)
                throw new NotSupportedException();

            StateMachine.FireAsync(ArmingParameters, mode, pendingTime);
        }

        internal void TriggerDisarm()
        {
            StateMachine.Fire(AlarmCommand.Disarm);
        }

        internal void OnTransitioned(Action<StateMachine<AlarmStatus, AlarmCommand>.Transition> callback)
        {
            StateMachine.OnTransitioned(callback);
        }

        private void InitializeStateMachine(AlarmStatus initialStatus)
        {
            StateMachine = new StateMachine<AlarmStatus, AlarmCommand>(initialStatus);

            ArmingParameters = StateMachine.SetTriggerParameters<AlarmCommand, int>(AlarmCommand.ArmWithTimer);

            ConfigureDisarmedState();
            ConfigureArmingState();
            ConfigureTriggeredState();

            ConfigureArmedStates();
        }

        private void ConfigureDisarmedState()
        {
            StateMachine.Configure(AlarmStatus.Disarmed)
                        .Permit(AlarmCommand.ArmAway, AlarmStatus.ArmedAway)
                        .Permit(AlarmCommand.ArmHome, AlarmStatus.ArmedHome)
                        .Permit(AlarmCommand.ArmWithTimer, AlarmStatus.Arming);
        }

        private void ConfigureArmingState()
        {
            StateMachine.Configure(AlarmStatus.Arming)
            .OnEntryFromAsync(
                ArmingParameters,
                async (armState, count) =>
                {
                    await Task.Delay(count * 1000);
                    AllowAccessToArm = true;
                    await StateMachine.FireAsync(armState);
                }
            )
            .OnExit(() => AllowAccessToArm = false)
            .Permit(AlarmCommand.Disarm, AlarmStatus.Disarmed)
            .PermitIf(AlarmCommand.ArmAway, AlarmStatus.ArmedAway, () => AllowAccessToArm)
            .PermitIf(AlarmCommand.ArmHome, AlarmStatus.ArmedHome, () => AllowAccessToArm)
            ;
        }

        private void ConfigureArmedStates()
        {
            // Armed away
            StateMachine.Configure(AlarmStatus.ArmedAway)
                        .Permit(AlarmCommand.Disarm, AlarmStatus.Disarmed)
                        .Permit(AlarmCommand.Trigger, AlarmStatus.Triggered)
                        ;

            // Armed home
            StateMachine.Configure(AlarmStatus.ArmedHome)
                        .SubstateOf(AlarmStatus.ArmedAway)
                        ;
        }

        private void ConfigureTriggeredState()
        {
            StateMachine.Configure(AlarmStatus.Triggered)
                        .Permit(AlarmCommand.Disarm, AlarmStatus.Disarmed)
                        ;
        }
    }
}
