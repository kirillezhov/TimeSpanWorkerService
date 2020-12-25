using System;

namespace TimeStampWorkerService
{
    public class TimeStampProvider : ITimeStampProvider
    {
        private readonly object _locker = new object();
        
        private long _lastTimeStamp = GetRoundedCurrentTimeStamp();

        public DateTime TimeStamp()
        {
            lock (_locker)
            {
                var currentTimeStamp = GetRoundedCurrentTimeStamp();
        
                _lastTimeStamp = Math.Max(currentTimeStamp, _lastTimeStamp + TimeSpan.TicksPerMillisecond);
        
                return new DateTime(_lastTimeStamp);
            }
        }

        private static long GetRoundedCurrentTimeStamp()
        {
            var currentTicks = DateTime.Now.Ticks;

            return currentTicks - (currentTicks % TimeSpan.TicksPerMillisecond);
        }
    }
}