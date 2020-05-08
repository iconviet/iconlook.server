using System.Numerics;

namespace Iconlook.Client.Megaloop
{
    public class StakingInfoRpc : RpcResponse
    {
        public BigDecimal GetTotalUnstaking()
        {
            return BigDecimal.Parse(Result?.result.total_unstaking ?? "0");
        }

        public BigInteger GetStakingAddressCount()
        {
            return BigInteger.Parse(Result?.result.total_staking_wallets ?? "0");
        }

        public BigInteger GetUnstakingAddressCount()
        {
            return BigInteger.Parse(Result?.result.total_unstaking_wallets ?? "0");
        }
    }
}