using System;

namespace Iconlook.Calculator
{
    public class BlockCalculator
    {
        private readonly long _currentBlockHeight;
        
        private readonly long _lastTermBlockHeight = 13251043;

        public BlockCalculator(long height)
        {
            _currentBlockHeight = height;
        }

        public long GetNextTermBlockHeight()
        {
            return _lastTermBlockHeight + 43200;
        }

        public string GetNextTermCountdown()
        {
            var duration = GetNextTermDuration();
            if (Math.Abs(duration.TotalHours) < 0)
                return Math.Abs(duration.TotalMinutes) < 0
                    ? $"{duration:%s}s"
                    : $"{duration:%m}m {duration:%s}s";
            return $"{duration:%h}h {duration:%m}m {duration:%s}s";
        }

        public TimeSpan GetNextTermDuration()
        {
            return TimeSpan.FromSeconds((GetNextTermBlockHeight() - _currentBlockHeight) * 2);
        }
    }
}