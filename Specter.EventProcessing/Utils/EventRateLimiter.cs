using Specter.EventProcessing.Events;
using System;
using System.Collections.Generic;

namespace Specter.EventProcessing.Utils
{
    public class EventRateLimiter
    {
        private readonly IDictionary<int, long> Cache = new Dictionary<int, long>();
        private const int MinimumIntervalTicks = 35000000;

        public bool IsAllowedEvent(IEventData eventData)
        {
            var key = GetUniqueKey(eventData.DeviceId, eventData.Payload);
            long eventTicks = eventData.ReceivedOn.Ticks;

            if (Cache.TryGetValue(key, out var lastEventTicks))
            {
                var isValid = eventTicks - lastEventTicks > MinimumIntervalTicks;

                if (isValid)
                {
                    Cache[key] = eventTicks;
                }

                return isValid;
            }
            else
            {
                Cache.Add(key, eventTicks);
                return true;
            }
        }

        private int GetUniqueKey(string s1, string s2)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + s1.GetHashCode();
                hash = hash * 23 + s2.GetHashCode();
                return hash;
            }
        }
    }
}
