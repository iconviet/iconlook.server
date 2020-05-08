using Iconlook.Object;

namespace Iconlook.Service.Web
{
    public static class LocalCache
    {
        public static PeerResponse LastPeerResponse { get; set; }
        public static BlockResponse LastBlockResponse { get; set; }
        public static ChainResponse LastChainResponse { get; set; }
        public static MegaloopResponse LastMegaloopResponse { get; set; }
    }
}