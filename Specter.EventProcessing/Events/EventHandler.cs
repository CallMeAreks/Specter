using Specter.Data;
using Specter.Data.Models;
using Specter.EventProcessing.Mappers;
using System;
using System.Threading.Tasks;

namespace Specter.EventProcessing.Events
{
    public class DeviceEventHandler : IEventHandler
    {
        public IRepository<Sensor> SensorRepository = RepositoryCreator.Create<Sensor>();
        public IRepository<Zone> ZoneRepository = RepositoryCreator.Create<Zone>();
        public IRepository<Event> EventRepository = RepositoryCreator.Create<Event>();

        public async Task<EventHandlerResponse> HandleAsync(IEventData data)
        {
            // Get event from data
            var alarmEvent = await GetAlarmEvent(data);

            // Save event
            int id = await EventRepository.InsertAsync(alarmEvent.ToEntityEvent());

            // Determine if alarm is triggered
            // notify
            // raise events

            return new EventHandlerResponse { EventId = id };
        }

        private async Task<Alarm.Events.Event> GetAlarmEvent(IEventData data)
        {
            // Load sensor
            var sensor = await SensorRepository.GetAsync(data.DeviceId);
            // load zone
            var zone = await ZoneRepository.GetAsync(sensor.Zone_Id);

            // Load event info
            return data.ToAlarmEvent(sensor.ToAlarmSensor(), zone.ToAlarmZone());
        }
    }
}
