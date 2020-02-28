namespace Iconlook.Object
{
    public class UnstakingAddressResponse : AddressResponse
    {
        public string UnstakedDuration { get; set; }
        public long UnstakedBlockHeight { get; set; }
        public string RequestedDateTime { get; set; }
        public long RequestedBlockHeight { get; set; }
    }
}