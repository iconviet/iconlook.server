using Agiper.Object;

namespace Iconlook.Object
{
    public class PeerResponse : ResponseBase<PeerResponse>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Ranking { get; set; }
        public string State { get; set; }
        public int PeerType { get; set; }
        public string Status { get; set; }
        public string PeerId { get; set; }
        public string LogoUrl { get; set; }
        public long BlockHeight { get; set; }
        public int MissedBlocks { get; set; }
        public int ProducedBlocks { get; set; }
        public int MadeBlockCount { get; set; }
        public int LeaderMadeBlockCount { get; set; }
        public double ProductivityPercentage { get; set; }
    }
}