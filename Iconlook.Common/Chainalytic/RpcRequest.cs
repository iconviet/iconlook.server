namespace Iconlook.Common.Chainalytic
{
    public class RpcRequest
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public object Params { get; set; }
        public string Jsonrpc { get; set; }

        public RpcRequest()
        {
            Jsonrpc = "2.0";
        }
    }
}