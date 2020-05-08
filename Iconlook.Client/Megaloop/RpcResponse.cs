namespace Iconlook.Client.Megaloop
{
    public class RpcResponse
    {
        public int Id { get; set; }
        public string Jsonrpc { get; set; }
        public dynamic Result { get; set; }

        public RpcResponse()
        {
            Jsonrpc = "2.0";
        }
    }
}