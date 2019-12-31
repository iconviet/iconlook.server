using System.Collections.Generic;
using System.Numerics;
using ServiceStack.Text;

namespace Iconlook.Client.Chainalytic
{
    public class UnstakingInfoRpc : RpcResponse
    {
        public BigInteger GetBlockHeight()
        {
            return BigInteger.Parse(Result?.result.wallets.height ?? "0");
        }

        public Dictionary<string, string> GetWallets()
        {
            return Result != null ? TypeSerializer.ToStringDictionary(JsonObject.Parse(Result.result.wallets.wallets.ToString())) : new Dictionary<string, string>();
        }
    }
}