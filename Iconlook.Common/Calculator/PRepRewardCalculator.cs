namespace Iconlook.Common.Calculator
{
    public class PRepRewardCalculator
    {
        private readonly decimal _irep;
        private readonly long _ranking;
        private readonly double _delegated;

        public PRepRewardCalculator(decimal irep, long ranking, double delegated)
        {
            _irep = irep;
            _ranking = ranking;
            _delegated = delegated * 100;
        }

        public decimal GetDailyReward()
        {
            return GetYearlyReward() / 365;
        }

        public decimal GetYearlyReward()
        {
            return GetMonthlyReward() * 12;
        }

        public decimal GetMonthlyReward()
        {
            if (_ranking > 22)
            {
                return _irep / 2 * (decimal) _delegated;
            }
            if (_delegated < 1)
            {
                return _irep / 1 * (decimal) _delegated;
            }
            return _irep + _irep / 2 * (decimal) (_delegated - 1);
        }
    }
}