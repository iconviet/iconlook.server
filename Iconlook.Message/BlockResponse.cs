using Agiper.Object;

namespace Iconlook.Message
{
    public class BlockResponse : ResponseBase<BlockResponse>
    {
        public string To { get; set; }
        public string From { get; set; }
        public decimal Fee { get; set; }
        public string Hash { get; set; }
        public decimal Amount { get; set; }
    }
}