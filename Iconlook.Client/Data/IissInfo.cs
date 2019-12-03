using System.Numerics;
using Lykke.Icon.Sdk.Transport.JsonRpc;

namespace Iconlook.Client.Data
{
    public class IissInfo
    {
        private readonly RpcObject _properties;

        public IissInfo(RpcObject properties)
        {
            _properties = properties;
        }

        public BigInteger GetBlockHeight()
        {
            return _properties.GetItem("blockHeight")?.ToInteger() ?? 0;
        }

        public BigInteger GetNextPRepTerm()
        {
            return _properties.GetItem("nextPRepTerm")?.ToInteger() ?? 0;
        }

        public BigInteger GetNextCalculation()
        {
            return _properties.GetItem("nextCalculation")?.ToInteger() ?? 0;
        }
    }
}