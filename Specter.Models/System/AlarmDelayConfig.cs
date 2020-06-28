using Specter.Models.Extensions;
using System;
using Specter.Models.Enums;

namespace Specter.Models.System
{
    public class AlarmDelayConfig
    {
        public int ArmedAwayDelay { get; private set; }
        public int ArmedHomeDelay { get; private set; }

        public static AlarmDelayConfig Default => new AlarmDelayConfig
        {
            ArmedAwayDelay = 30,
            ArmedHomeDelay = 30
        };

        public int GetDelayForCommand(AlarmCommand command)
        {
            if (!command.IsArmCommand())
                throw new ArgumentException();

            return command == AlarmCommand.ArmAway
                ? ArmedAwayDelay
                : ArmedHomeDelay;
        }
    }
}
