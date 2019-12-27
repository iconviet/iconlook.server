namespace Iconlook.Client.Chainalytic
{
    public class RpcResponse
    {
        public string Id { get; set; }
        public string Jsonrpc { get; set; }
        public dynamic Result { get; set; }
    }
}