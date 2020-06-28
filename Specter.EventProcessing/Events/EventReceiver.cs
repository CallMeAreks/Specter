using CliWrap;
using CliWrap.EventStream;
using Specter.EventProcessing.Utils;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Specter.Data;
using Specter.Data.Models;

namespace Specter.EventProcessing.Events
{
    public sealed class EventReceiver
    {
        private static readonly EventReceiver instance = new EventReceiver();
        private readonly CancellationTokenSource _receiverCancellationTokenSource = new CancellationTokenSource();
        private int _processId;

        private readonly EventRateLimiter _rateLimiter = new EventRateLimiter();
        private readonly EventParser _eventParser = new EventParser();
        private readonly IEventHandler _handler = new DeviceEventHandler();

        static EventReceiver() { }

        private EventReceiver() 
        {
            // Close rtl_433 if it's already running
            CloseRtl433Process();
        }

        public static EventReceiver Instance => instance;

        public async Task ListenAsync()
        {
            var cmd = Cli.Wrap("rtl_433");

            await foreach (var cmdEvent in cmd.ListenAsync(_receiverCancellationTokenSource.Token))
            {
                switch (cmdEvent)
                {
                    case StartedCommandEvent started:
                        _processId = started.ProcessId;
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
            var eventData = _eventParser.ParseFromJson(ev.Text);
            var uid = _rateLimiter.ValidateEvent(eventData);

            if (uid.HasValue)
            {
                var response =  await _handler.HandleAsync(eventData);
            
                if (response.Success)
                {
                    _rateLimiter.MarkEventAsProcessed(uid.Value, eventData);
                }
            }
        }

        private void CloseRtl433Process() => Process.GetProcessesByName("rtl_433").ToList().ForEach(p => p.Kill());

        public void StopListening()
        {
            _receiverCancellationTokenSource.Cancel();
            CloseRtl433Process();
        }
    }
}
