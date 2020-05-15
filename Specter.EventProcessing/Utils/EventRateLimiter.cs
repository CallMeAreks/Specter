using Specter.EventProcessing.Events;
using System.Collections.Generic;

namespace Specter.EventProcessing.Utils
{
    public class EventRateLimiter
    {
        private readonly IDictionary<int, long> Cache = new Dictionary<int, long>();
        private const int MinimumIntervalTicks = 35000000;

        public int? ValidateEvent(IEventData eventData)
        {
            // Calculate a unique hash for the device id and event type
            var key = GetUniqueKey(eventData.DeviceId, eventData.EventType);
            // Get the date and time as ticks to make the comparisons
            long eventTicks = eventData.ReceivedOn.Ticks;

            return IsValid(key, eventTicks) ? key : (int?)null;                                   
        }

        public void MarkEventAsProcessed(int key, IEventData eventData)
        {
            Cache[key] = eventData.ReceivedOn.Ticks;
        }

        /// <summary>
        /// Returns whether the event is considered a duplicate
        /// </summary>
        /// <param name="key"></param>
        /// <param name="eventTicks"></param>
        /// <returns></returns>
        private bool IsValid(int key, long eventTicks)
        {
            // The time is considered valid if the difference is greater than MinimumIntervalTicks
            return Cache.TryGetValue(key, out var lastEventTicks) // If the key exists 
                        ? eventTicks - lastEventTicks > MinimumIntervalTicks // return whether is it's a duplicate or not
                        : true; // If not, return true since it's a new event.
        }

        private int GetUniqueKey(int s1, int s2)
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
