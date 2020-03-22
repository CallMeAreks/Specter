using Specter.Alarm.Enums;
using System.Linq;
using static Specter.Alarm.Enums.AlarmState;

namespace Specter.Alarm.Extensions
{
    public static class AlarmStatusExtensions
    {
        public static AlarmState[] AlarmStates = new [] { ArmedAway, ArmedHome };

        public static bool IsArmedState(this AlarmState status) => AlarmStates.Contains(status);
    }
}
