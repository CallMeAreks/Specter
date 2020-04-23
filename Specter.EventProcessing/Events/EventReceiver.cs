using CliWrap;
using CliWrap.EventStream;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Specter.EventProcessing.Events
{
    public sealed class EventReceiver
    {
        private static readonly EventReceiver instance = new EventReceiver();
        private CancellationTokenSource ReceiverCancellationTokenSource = new CancellationTokenSource();
        private int ProcessId;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static EventReceiver() { }

        private EventReceiver() { }

        public static EventReceiver Instance => instance;

        private string Rtl433Path => $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Rtl433\\rtl_433.exe";

        public async Task ListenAsync()
        {
            var cmd = Cli.Wrap(Rtl433Path);

            await foreach (var cmdEvent in cmd.ListenAsync(ReceiverCancellationTokenSource.Token))
            {
                switch (cmdEvent)
                {
                    case StartedCommandEvent started:
                        ProcessId = started.ProcessId;
                        System.Diagnostics.Debug.WriteLine($"Process started; ID: {started.ProcessId}");
                        break;
                    case StandardOutputCommandEvent stdOut:
                        System.Diagnostics.Debug.WriteLine($"Out> {stdOut.Text}");
                        break;
                    case StandardErrorCommandEvent stdErr:
                        System.Diagnostics.Debug.WriteLine($"Err> {stdErr.Text}");
                        break;
                    case ExitedCommandEvent exited:
                        System.Diagnostics.Debug.WriteLine($"Process exited; Code: {exited.ExitCode}");
                        break;
                }
            }
        }

        public void StopListening() => ReceiverCancellationTokenSource.Cancel();
    }
}
