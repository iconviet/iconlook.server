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
    }
}