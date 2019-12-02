using Lykke.Icon.Sdk.Transport.JsonRpc;

namespace Iconlook.Client.Data
{
    public class PRep
    {
        private readonly RpcObject _properties;

        public PRep(RpcObject properties)
        {
            _properties = properties;
        }

        public string GetName()
        {
            return _properties.GetItem("name")?.ToString();
        }
    }
}