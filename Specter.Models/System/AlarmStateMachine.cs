using Specter.Models.Extensions;
using Stateless;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Specter.Models.Enums;

namespace Specter.Models.System
{
    internal sealed class AlarmStateMachine
    {
        internal IEnumerable<AlarmCommand> Triggers => StateMachine.PermittedTriggers;
        internal AlarmState State => StateMachine.State;

        private StateMachine<AlarmState, AlarmCommand>.TriggerWithParameters<AlarmCommand, int> ArmingParameters { get; set; }
        private StateMachine<AlarmState, AlarmCommand> StateMachine { get; set; }
        private bool AllowAccessToArm { get; set; }

        internal AlarmStateMachine(AlarmState initialStatus) => InitializeStateMachine(initialStatus);

        internal void TriggerArm(AlarmCommand mode, int pendingTime)
        {
            if (!mode.IsArmCommand())
                throw new NotSupportedException();

            StateMachine.FireAsync(ArmingParameters, mode, pendingTime);
        }

        internal void TriggerDisarm() => StateMachine.Fire(AlarmCommand.Disarm);

        internal void OnTransitioned(Action<StateMachine<AlarmState, AlarmCommand>.Transition> callback) 
            => StateMachine.OnTransitioned(callback);

        private void InitializeStateMachine(AlarmState initialStatus)
        {
            StateMachine = new StateMachine<AlarmState, AlarmCommand>(initialStatus);

            ArmingParameters = StateMachine.SetTriggerParameters<AlarmCommand, int>(AlarmCommand.ArmWithTimer);

            ConfigureDisarmedState();
            ConfigureArmingState();
            ConfigureTriggeredState();

            ConfigureArmedStates();
        }

        private void ConfigureDisarmedState()
        {
            StateMachine.Configure(AlarmState.Disarmed)
                        .Permit(AlarmCommand.ArmAway, AlarmState.ArmedAway)
                        .Permit(AlarmCommand.ArmHome, AlarmState.ArmedHome)
                        .Permit(AlarmCommand.ArmWithTimer, AlarmState.Arming);
        }

        private void ConfigureArmingState()
        {
            StateMachine.Configure(AlarmState.Arming)
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
            .Permit(AlarmCommand.Disarm, AlarmState.Disarmed)
            .PermitIf(AlarmCommand.ArmAway, AlarmState.ArmedAway, () => AllowAccessToArm)
            .PermitIf(AlarmCommand.ArmHome, AlarmState.ArmedHome, () => AllowAccessToArm)
            ;
        }

        private void ConfigureArmedStates()
        {
            // When the alarm is armed the only allowed states are Disarm and Trigger
            // Armed away
            StateMachine.Configure(AlarmState.ArmedAway)
                        .Permit(AlarmCommand.Disarm, AlarmState.Disarmed)
                        .Permit(AlarmCommand.Trigger, AlarmState.Triggered)
                        ;

            // Armed home
            StateMachine.Configure(AlarmState.ArmedHome)
                        .SubstateOf(AlarmState.ArmedAway)
                        ;
        }

        private void ConfigureTriggeredState()
        {
            StateMachine.Configure(AlarmState.Triggered)
                        .Permit(AlarmCommand.Disarm, AlarmState.Disarmed)
                        ;
        }
    }
}
