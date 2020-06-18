namespace Iconlook.Common.Tracker
{
    public class PRepResponse
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Logo { get; set; }
        public decimal Irep { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }
        public int BlockHeight { get; set; }
        public int TotalBlocks { get; set; }
        public decimal Delegated { get; set; }
        public string ApiEndpoint { get; set; }
        public decimal TotalStake { get; set; }
        public int ValidatedBlocks { get; set; }
        public decimal TotalDelegated { get; set; }
        public int IrepUpdatedBlockHeight { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}