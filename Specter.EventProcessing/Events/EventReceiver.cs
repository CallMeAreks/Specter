using CliWrap;
using CliWrap.EventStream;
using Specter.EventProcessing.Utils;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        private string Rtl433Path => $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\rtl\\rtl_433.exe";
        private EventRateLimiter RateLimiter = new EventRateLimiter();
        private EventParser EventParser = new EventParser();
        private IEventHandler Handler = new DeviceEventHandler();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static EventReceiver() { }

        private EventReceiver() 
        {
            // Close rtl_433 if it's already running
            CloseRtl433Process();
        }

        public static EventReceiver Instance => instance;

        public async Task ListenAsync()
        {
            var cmd = Cli.Wrap(Rtl433Path);

            await foreach (var cmdEvent in cmd.ListenAsync(ReceiverCancellationTokenSource.Token))
            {
                switch (cmdEvent)
                {
                    case StartedCommandEvent started:
                        ProcessId = started.ProcessId;
                        Debug.WriteLine($"Process started; ID: {started.ProcessId}");
                        break;
                    case StandardOutputCommandEvent stdOutEvent:
                        await HandleEvent(stdOutEvent);
                        break;
                    case StandardErrorCommandEvent stdErr:
                        Debug.WriteLine($"Err> {stdErr.Text}");
                        break;
                    case ExitedCommandEvent exited:
                        Debug.WriteLine($"Process exited; Code: {exited.ExitCode}");
                        break;
                }
            }
        }

        private async Task HandleEvent(StandardOutputCommandEvent ev)
        {
            var eventData = EventParser.ParseFromJson(ev.Text);
            var uid = RateLimiter.ValidateEvent(eventData);
            
            if (uid.HasValue)
            {
                var response = await Handler.HandleAsync(eventData);

                if (response.Success)
                {
                    RateLimiter.MarkEventAsProcessed(uid.Value, eventData);
                }
            }
        }

        private void CloseRtl433Process()
        {
            Process.GetProcessesByName("rtl_433").ToList().ForEach(p => p.Kill());
        }

        public void StopListening()
        {
            ReceiverCancellationTokenSource.Cancel();
            CloseRtl433Process();
        }
    }
}
