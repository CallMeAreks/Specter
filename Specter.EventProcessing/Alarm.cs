namespace Specter.EventProcessing
{
    public sealed class AlarmSystem
    {
        private static readonly AlarmSystem instance = new AlarmSystem();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static AlarmSystem() { }

        private AlarmSystem()
        {
            // TODO: Initialize alarm
        }

        public static AlarmSystem Instance => instance;
    }
}
