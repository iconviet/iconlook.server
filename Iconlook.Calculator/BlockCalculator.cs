using System;

namespace Iconlook.Calculator
{
    public class BlockCalculator
    {
        private readonly long _next;
        private readonly long _height;

        public BlockCalculator(long height, long next)
        {
            _next = next;
            _height = height;
        }

        public string GetNextTermLocalTime()
        {
            var duration = GetNextTermDuration();
            var local_time = DateTime.UtcNow.Add(duration);
            return local_time.ToString("hh:mm:ss tt");
        }

        public TimeSpan GetNextTermDuration()
        {
            return TimeSpan.FromSeconds((_next - _height) * 2);
        }

        public string GetNextTermCountdown()
        {
            var duration = GetNextTermDuration();
            if (duration.Hours == 0)
                return duration.Minutes == 0
                    ? $"{duration:%s}s"
                    : $"{duration:%m}m {duration:%s}s";
            return $"{duration:%h}h {duration:%m}m {duration:%s}s";
        }
    }
}