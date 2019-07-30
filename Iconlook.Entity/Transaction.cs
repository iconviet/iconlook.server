namespace Iconlook.Entity
{
    public class Transaction
    {
        public string To { get; set; }
        public string From { get; set; }
        public decimal Fee { get; set; }
        public string Hash { get; set; }
        public decimal Amount { get; set; }
    }
}