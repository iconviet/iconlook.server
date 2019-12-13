using System;
using Agiper.Object;

namespace Iconlook.Object
{
    public class PeerResponse : ResponseBase<PeerResponse>
    {
        public int Votes { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }   
        public string State { get; set; }
        public string Address { get; set; }
        public int MissedBlocks { get; set; }
        public int ProducedBlocks { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public double DelegatedPercentage { get; set; }
        public double ProductivityPercentage { get; set; }
    }
}