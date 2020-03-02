using System;

namespace Iconlook.Object
{
    public class UnstakingAddressResponse : AddressResponse
    {
        public long UnstakedBlockHeight { get; set; }
        public long RequestedBlockHeight { get; set; }
        public string UnstakingCountdown { get; set; }
        public DateTime RequestedDateTime { get; set; }
        public string RequestedDateTimeAge { get; set; }
        public string UnstakingCountdownShort { get; set; }
        public string RequestedDateTimeAgeShort { get; set; }
    }
}