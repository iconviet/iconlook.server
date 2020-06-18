using System.Collections.Generic;
using System.Numerics;
using ServiceStack.Text;

namespace Iconlook.Shared.Chainalytic
{
    public class UnstakingInfoRpc : RpcResponse
    {
        public BigInteger GetBlockHeight()
        {
            return BigInteger.Parse(Result?.result.height ?? "0");
        }

        public Dictionary<string, string> GetWallets()
        {
            return Result != null ? TypeSerializer.ToStringDictionary(JsonObject.Parse(Result.result.wallets.ToString())) : new Dictionary<string, string>();
        }
    }
}