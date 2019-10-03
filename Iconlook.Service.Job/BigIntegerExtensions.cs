using System.Numerics;

namespace Iconlook.Service.Job
{
    public static class BigIntegerExtensions
    {
        public static BigInteger DividePow(this BigInteger instance, BigInteger value, int exponent)
        {
            return BigInteger.Divide(instance, BigInteger.Pow(value, exponent));
        }
    }
}