namespace Iconlook.Object
{
    public class UndelegatedAddressResponse : AddressResponse
    {
        public decimal Undelegated { get; set; }
        public long UndelegatedBlockHeight { get; set; }
    }
}