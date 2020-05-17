using Iconviet.Object;

namespace Iconlook.Object
{
    public class MegaloopUpdatedSignal : SignalBase<MegaloopUpdatedSignal>
    {
        public int PlayerCount { get; set; }
        public decimal JackpotSize { get; set; }
        public decimal JackpotSubsidy { get; set; }
        public decimal TotalJackpotSize { get; set; }
        public decimal TotalJackpotSizeUsd { get; set; }
        public MegaloopPlayerResponse DiffPlayer { get; set; }
        public MegaloopPlayerResponse LastPlayer { get; set; }
        public MegaloopPlayerResponse LastWinner { get; set; }
    }
}