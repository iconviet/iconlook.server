using System.Numerics;
using Lykke.Icon.Sdk.Transport.JsonRpc;

namespace Iconlook.Client.Service
{
    public class IissInfoRpc
    {
        private readonly RpcObject _properties;

        public IissInfoRpc(RpcObject properties)
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

        public BigInteger GetTotalIRep()
        {
            var variable = _properties.GetItem("variable");
            return variable?.ToObject().GetItem("irep")?.ToInteger() ?? 0;
        }

        public BigInteger GetTotalRRep()
        {
            var variable = _properties.GetItem("variable");
            return variable?.ToObject().GetItem("rrep")?.ToInteger() ?? 0;
        }

        public BigInteger GetNextCalculation()
        {
            return _properties.GetItem("nextCalculation")?.ToInteger() ?? 0;
        }
    }
}