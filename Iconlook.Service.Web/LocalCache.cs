using Iconlook.Object;

namespace Iconlook.Service.Web
{
    // TODO: replace this with Redis Cache
    public static class LocalCache
    {
        public static PeerResponse LastPeerResponse { get; set; }
        public static BlockResponse LastBlockResponse { get; set; }
        public static ChainResponse LastChainResponse { get; set; }
    }
}