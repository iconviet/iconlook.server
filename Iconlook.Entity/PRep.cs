namespace Iconlook.Entity
{
    public class Prep
    {
        public int Votes { get; set; }
        public string Id { get; set; }
        public int CScore { get; set; }
        public int Voters { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public bool Direction { get; set; }
        public string Created { get; set; }
        public string LastSeen { get; set; }
        public string Location { get; set; }
        public int MissedBlocks { get; set; }
        public int ProducedBlocks { get; set; }
        public double UptimePercentage { get; set; }
        public double SupplyPercentage { get; set; }
    }
}