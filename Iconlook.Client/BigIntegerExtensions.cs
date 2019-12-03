using System.Numerics;

namespace Iconlook.Client
{
    public static class BigIntegerExtensions
    {
        public static decimal ToDecimal(this BigInteger instance)
        {
            return decimal.Parse(BigInteger.Divide(instance, BigInteger.Pow(10, 18)).ToString());
        }

        public static BigInteger DividePow(this BigInteger instance, BigInteger value, int exponent)
        {
            return BigInteger.Divide(instance, BigInteger.Pow(value, exponent));
        }
    }
}