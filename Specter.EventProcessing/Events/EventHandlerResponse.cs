namespace Specter.EventProcessing.Events
{
    public class EventHandlerResponse
    {
        public int EventId { get; set; }
        public bool Success => EventId > 0;
    }
}
