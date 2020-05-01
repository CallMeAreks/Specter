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
            var msg = JsonSerializer.Deserialize<Rtl433Message>(json, SerializerOptions);

            return new EventData
            {
                DeviceId = msg.Id.ToString(),
                Payload = msg.Event.ToString(),
                ReceivedOn = msg.Time.UtcDateTime
            };
        }
    }
}
