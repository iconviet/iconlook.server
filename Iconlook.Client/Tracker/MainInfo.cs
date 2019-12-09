using System.Numerics;
using ServiceStack.Text;

namespace Iconlook.Client.Tracker
{
    public class MainInfo
    {
        public JsonObject TmainInfo { get; set; }

        public BigInteger GetMarketCap()
        {
            return BigDecimal.Parse(TmainInfo.Get("marketCap")).ToBigInteger();
        }

        public BigInteger GetIcxSupply()
        {
            return BigDecimal.Parse(TmainInfo.Get("icxSupply")).ToBigInteger();
        }

        public BigInteger GetIcxCirculation()
        {
            return BigDecimal.Parse(TmainInfo.Get("icxCirculationy")).ToBigInteger();
        }

        public BigInteger GetTransactionCount()
        {
            return BigDecimal.Parse(TmainInfo.Get("transactionCount")).ToBigInteger();
        }
    }
}