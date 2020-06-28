using System.Linq;
using Specter.Models.Enums;
using static Specter.Models.Enums.AlarmCommand;

namespace Specter.Models.Extensions
{
    public static class AlarmCommandExtensions
    {
        public static AlarmCommand[] ArmCommands = new [] { ArmAway, ArmHome };

        public static bool IsArmCommand(this AlarmCommand command) => ArmCommands.Contains(command);
    }
}
