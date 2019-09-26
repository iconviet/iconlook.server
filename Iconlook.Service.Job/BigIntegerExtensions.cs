using System.Numerics;

namespace Iconlook.Service.Job
{
    public static class BigIntegerExtensions
    {
        public static BigInteger ToZeroless(this BigInteger instance, int precision)
        {
            return BigInteger.Divide(instance, BigInteger.Pow(10, precision));
        }

        public static BigInteger ToLooplessIcx(this BigInteger instance, int precision = 0)
        {
            return BigInteger.Divide(instance, BigInteger.Pow(10, 18 - precision));
        }
    }
}