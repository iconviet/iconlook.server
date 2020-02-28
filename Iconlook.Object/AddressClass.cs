using ServiceStack.DataAnnotations;

namespace Iconlook.Object
{
    [EnumAsInt]
    public enum AddressClass
    {
        Empty,
        Ica,
        PRep,
        CRep,
        Iconist,
        Exchange
    }
}