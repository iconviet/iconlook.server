using ServiceStack.DataAnnotations;

namespace Iconlook.Object
{
    [EnumAsInt]
    public enum AddressType
    {
        Empty,
        Ica,
        PRep,
        CRep,
        Iconist,
        Exchange
    }
}