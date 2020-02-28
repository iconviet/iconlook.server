using ServiceStack.DataAnnotations;

namespace Iconlook.Object
{
    [EnumAsInt]
    public enum AddressType
    {
        Empty,
        Wallet,
        Contract
    }
}