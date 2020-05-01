using System.Threading.Tasks;

namespace Specter.EventProcessing.Events
{
    public interface IEventHandler
    {
        public Task<EventHandlerResponse> HandleAsync(IEventData data);
    }
}
