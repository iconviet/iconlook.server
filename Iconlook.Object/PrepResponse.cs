using Agiper.Object;

namespace Iconlook.Object
{
    public class PrepResponse : ResponseBase<PrepResponse>
    {
        public int Votes { get; set; }
        public int Score { get; set; }
        public string Id { get; set; }
        public int Voters { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public bool Direction { get; set; }
        public string Created { get; set; }
        public string LastSeen { get; set; }
        public string Location { get; set; }
        public int ProducedBlocks { get; set; }
        public int RejectedBlocks { get; set; }
        public double UptimePercentage { get; set; }
        public double SupplyPercentage { get; set; }
    }
}