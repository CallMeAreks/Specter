using CliWrap;
using CliWrap.EventStream;
using Specter.EventProcessing.Utils;
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

        private EventReceiver() 
        {
            // TODO: Close rtl process
        }

        public static EventReceiver Instance => instance;

        private string Rtl433Path => $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\rtl\\rtl_433.exe";

        public async Task ListenAsync()
        {
            var cmd = Cli.Wrap(Rtl433Path);
            IEventHandler handler = new DeviceEventHandler();
            var eventRateLimiter = new EventRateLimiter();
            var eventParser = new EventParser();
            
            await foreach (var cmdEvent in cmd.ListenAsync(ReceiverCancellationTokenSource.Token))
            {
                switch (cmdEvent)
                {
                    case StartedCommandEvent started:
                        ProcessId = started.ProcessId;
                        System.Diagnostics.Debug.WriteLine($"Process started; ID: {started.ProcessId}");
                        break;
                    case StandardOutputCommandEvent stdOut:
                        var eventData = eventParser.ParseFromJson(stdOut.Text);
                        if (eventRateLimiter.IsAllowedEvent(eventData))
                        {
                            
                            System.Diagnostics.Debug.WriteLine($"Event accepted: {eventData.DeviceId}_{eventData.Payload.PadLeft(2, '0')} = {eventData.ReceivedOn.Ticks}");
                            //await handler.HandleAsync(eventData);
                        }
                        else
                        {

                            System.Diagnostics.Debug.WriteLine($"Event rejected: {eventData.DeviceId}_{eventData.Payload.PadLeft(2, '0')} = {eventData.ReceivedOn.Ticks}");
                        }
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
