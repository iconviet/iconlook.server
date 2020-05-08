using System.Numerics;
using Lykke.Icon.Sdk.Transport.JsonRpc;

namespace Iconlook.Client.Megaloop
{
    public class JackpotSizeRpc
    {
        private readonly RpcObject _properties;

        public JackpotSizeRpc(RpcObject properties)
        {
            _properties = properties;
        }

        public BigInteger GetTotalStaked()
        {
            return _properties.GetItem("totalStake")?.ToInteger() ?? 0;
        }

        public BigInteger GetTotalDelegated()
        {
            return _properties.GetItem("totalDelegated")?.ToInteger() ?? 0;
        }
    }
}