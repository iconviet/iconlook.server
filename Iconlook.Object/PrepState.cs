using ServiceStack.DataAnnotations;

namespace Iconlook.Object
{
    [EnumAsInt]
    public enum PRepState
    {
        Empty,
        Enabled,
        Disabled,
        Suspended
    }
}