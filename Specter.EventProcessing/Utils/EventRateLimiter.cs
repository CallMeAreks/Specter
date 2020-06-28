using Specter.EventProcessing.Events;
using System.Collections.Generic;

namespace Specter.EventProcessing.Utils
{
    public sealed class EventRateLimiter
    {
        private readonly IDictionary<int, long> _cache = new Dictionary<int, long>();
        private const int MinimumIntervalTicks = 35000000;

        /// <summary>
        /// Determines if the event is valid and hasn't been processed before.
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public int? ValidateEvent(IEventData eventData)
        {
            // Calculate a unique hash for the device id and event type
            var key = GetUniqueKey(eventData.DeviceId, eventData.EventType);
            // Get the date and time as ticks to make the comparisons
            var eventTicks = eventData.ReceivedOn.Ticks;

            return IsValid(key, eventTicks) ? key : (int?)null;                                   
        }

        public void MarkEventAsProcessed(int key, IEventData eventData)
        {
            _cache[key] = eventData.ReceivedOn.Ticks;
        }

        /// <summary>
        /// Returns whether the event is considered valid
        /// </summary>
        /// <param name="key"></param>
        /// <param name="eventTicks"></param>
        /// <returns></returns>
        private bool IsValid(int key, long eventTicks)
        {
            var existing = _cache.TryGetValue(key, out var lastEventTicks);
            
            // It is considered valid if it's new
            // or the difference is greater than MinimumIntervalTicks
            // meaning it's not a duplicate signal
            return !existing || eventTicks - lastEventTicks > MinimumIntervalTicks;
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
