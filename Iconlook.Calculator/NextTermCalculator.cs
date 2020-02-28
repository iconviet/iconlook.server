using System;

namespace Iconlook.Calculator
{
    public class NextTermCalculator
    {
        private readonly long _next;
        private readonly long _height;

        public NextTermCalculator(long height, long next)
        {
            _next = next;
            _height = height;
        }

        public string GetLocalTime()
        {
            var duration = GetDuration();
            var local_time = DateTime.UtcNow.Add(duration);
            return local_time.ToString("hh:mm:ss tt");
        }

        public TimeSpan GetDuration()
        {
            return TimeSpan.FromSeconds((_next - _height) * 2);
        }

        public string GetCountdown()
        {
            var duration = GetDuration();
            if (duration.Hours == 0)
                return duration.Minutes == 0
                    ? $"{duration:%s}s"
                    : $"{duration:%m}m {duration:%s}s";
            return $"{duration:%h}h {duration:%m}m {duration:%s}s";
        }
    }
}