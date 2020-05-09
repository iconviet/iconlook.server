using Iconviet.Object;

namespace Iconlook.Object
{
    public class MegaloopWinnerResponse : ResponseBase<MegaloopWinnerResponse>
    {
        public string Address { get; set; }
        public decimal Deposit { get; set; }
        public decimal Jackpot { get; set; }
        public decimal Subsidy { get; set; }
        public decimal JackpotUsd { get; set; }
    }
}