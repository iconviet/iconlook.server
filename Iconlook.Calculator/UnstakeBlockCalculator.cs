using System;

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

        public string GetUnstakingCountdown()
        {
            return "6d 4h 2m";
        }

        public DateTime GetRequestDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}