using Specter.Alarm.Enums;
using Specter.EventProcessing.Events;
using System;

namespace Specter.EventProcessing.Mappers
{
    public static class MappingExtensions
    {
        public static Data.Models.Event ToEntityEvent(this Alarm.Events.Event ev)
        {
            return new Data.Models.Event
            {
                Id = ev.Id,
                Sensor_Id = ev.Sensor.Id,
                Type = (int)ev.Sensor.Type,
                Zone_Id = ev.Zone.Id,
                Timestamp = (int)(ev.CreatedOn.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
            };
        }

        public static Alarm.System.Zone ToAlarmZone(this Data.Models.Zone zone)
        {
            return new Alarm.System.Zone
            {
                Id = zone.Id,
                Number = zone.Number,
                TriggerType = (TriggerType)zone.TriggerType
            };
        }

        public static Alarm.System.Sensor ToAlarmSensor(this Data.Models.Sensor sensor)
        {
            return new Alarm.System.Sensor
            {
                Id = sensor.Id,
                Enabled = sensor.Enabled == 1,
                Name = sensor.Name,
                //State = (SensorState)sensor.State,
                Type = (SensorType)sensor.Type
            };
        }

        public static Alarm.Events.Event ToAlarmEvent(this IEventData eventData, Alarm.System.Sensor sensor, Alarm.System.Zone zone)
        {
            return new Alarm.Events.Event 
            {
                CreatedOn = eventData.ReceivedOn,
                Sensor = sensor,
                Zone = zone
            };

        }
    }
}
