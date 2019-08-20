using ServiceStack.DataAnnotations;

namespace Iconlook.Object
{
    [EnumAsInt]
    public enum PrepState
    {
        Empty,
        Enabled,
        Disabled,
        Suspended
    }
}