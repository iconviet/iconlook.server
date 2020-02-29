using System;
using Humanizer;
using Humanizer.Localisation;

namespace Iconlook.Calculator
{
    public class UnstakeBlockCalculator
    {
        private readonly long _height;
        private readonly long _unstaked;
        private readonly long _requested;

        public UnstakeBlockCalculator(long height, long requested, long unstaked)
        {
            _height = height;
            _unstaked = unstaked;
            _requested = requested;
        }

        public DateTime GetRequestDateTime()
        {
            return DateTime.UtcNow.AddSeconds((_requested - _height) * 2);
        }

        public string GetUnstakingCountdownShort()
        {
            var duration = TimeSpan.FromSeconds((_unstaked - _height) * 2);
            if (duration.Days == 0)
                return duration.Hours == 0
                    ? $"{duration:%m}m"
                    : $"{duration:%h}h {duration:%m}m";
            return $"{duration:%d}d {duration:%h}h {duration:%m}m";
        }

        public string GetUnstakingCountdown()
        {
            return TimeSpan
                .FromSeconds((_unstaked - _height) * 2)
                .Humanize(3, null, TimeUnit.Day).Replace("minute", "min").Replace("second", "sec");
        }
    }
}