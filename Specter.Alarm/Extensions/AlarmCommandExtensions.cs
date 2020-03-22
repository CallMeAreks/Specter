using Specter.Alarm.Enums;
using System.Linq;
using static Specter.Alarm.Enums.AlarmCommand;

namespace Specter.Alarm.Extensions
{
    public static class AlarmCommandExtensions
    {
        public static AlarmCommand[] ArmCommands = new [] { ArmAway, ArmHome };

        public static bool IsArmCommand(this AlarmCommand command) => ArmCommands.Contains(command);
    }
}
