using System.Numerics;

namespace Iconlook.Client.Chainalytic
{
    public class StakingInfoRpc : RpcResponse
    {
        public BigDecimal GetTotalUnstaking()
        {
            return BigDecimal.Parse(Result.result.total_unstaking);
        }

        public BigInteger GetStakingAddressCount()
        {
            return BigInteger.Parse(Result.result.total_staking_wallets);
        }

        public BigInteger GetUnstakingAddressCount()
        {
            return BigInteger.Parse(Result.result.total_unstaking_wallets);
        }
    }
}