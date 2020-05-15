using System.Text.Json;

namespace Specter.EventProcessing.Events
{
    public class EventParser
    {
        private JsonSerializerOptions SerializerOptions = new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        };

        public IEventData ParseFromJson(string json)
        {
            return JsonSerializer.Deserialize<EventData>(json, SerializerOptions);
        }
    }
}
