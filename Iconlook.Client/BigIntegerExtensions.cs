using System.Numerics;
using Lykke.Icon.Sdk.Data;

namespace Iconlook.Client
{
    public static class BigIntegerExtensions
    {
        public static long ToMilliseconds(this BigInteger instance)
        {
            return (long) BigInteger.Divide(instance, BigInteger.Pow(10, 3));
        }

        public static decimal ToIcx(this BigInteger instance)
        {
            return decimal.Parse(IconAmount.Of(instance, IconAmount.Unit.Loop).ConvertUnit(IconAmount.Unit.ICX).ToString());
        }
    }
}