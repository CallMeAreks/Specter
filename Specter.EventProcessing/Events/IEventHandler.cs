namespace Specter.EventProcessing.Events
{
    public interface IEventHandler
    {
        public EventHandlerResponse Handle(IEventData data);
    }
}
