using Specter.Data;
using Specter.Data.Models;
using Specter.EventProcessing.Mappers;
using System.Threading.Tasks;

namespace Specter.EventProcessing.Events
{
    public sealed class DeviceEventHandler : IEventHandler
    {
        private readonly IRepository<Sensor> _sensorRepository = RepositoryCreator.Create<Sensor>();
        private readonly IRepository<Zone>   _zoneRepository   = RepositoryCreator.Create<Zone>();
        private readonly IRepository<Event>  _eventRepository  = RepositoryCreator.Create<Event>();

        public async Task<EventHandlerResponse> HandleAsync(IEventData data)
        {
            // Get event from data
            var alarmEvent = await GetAlarmEvent(data);

            // Save event
            int id = await _eventRepository.InsertAsync(alarmEvent.ToEntityEvent());

            // Determine if alarm is triggered
            // notify
            // raise events

            return new EventHandlerResponse { EventId = id };
        }

        private async Task<Models.Events.Event> GetAlarmEvent(IEventData data)
        {
            // Load sensor
            var sensor = await _sensorRepository.GetAsync(data.DeviceId);
            // load zone
            var zone   = await _zoneRepository.GetAsync(sensor.Zone_Id);

            // Load event info
            return data.ToAlarmEvent(sensor.ToAlarmSensor(), zone.ToAlarmZone());
        }
    }
}
