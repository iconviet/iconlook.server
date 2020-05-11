using Iconviet.Object;

namespace Iconlook.Object
{
    public class MegaloopUpdatedSignal : SignalBase<MegaloopUpdatedSignal>
    {
        public int PlayerCount { get; set; }
        public decimal JackpotSize { get; set; }
        public decimal JackpotSizeUsd { get; set; }
        public MegaloopPlayerResponse LastPlayer { get; set; }
        public MegaloopPlayerResponse LastWinner { get; set; }
    }
}