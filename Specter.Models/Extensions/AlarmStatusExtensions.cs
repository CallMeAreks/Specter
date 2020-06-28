using System.Linq;
using Specter.Models.Enums;
using static Specter.Models.Enums.AlarmState;

namespace Specter.Models.Extensions
{
    public static class AlarmStatusExtensions
    {
        public static AlarmState[] AlarmStates = new [] { ArmedAway, ArmedHome };

        public static bool IsArmedState(this AlarmState status) => AlarmStates.Contains(status);
    }
}
