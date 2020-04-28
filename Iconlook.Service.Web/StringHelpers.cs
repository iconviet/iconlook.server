using Iconviet;

namespace Iconlook.Service.Web
{
    public static class StringHelpers
    {
        public static string ParseLogoUrl(string logoUrl)
        {
            return logoUrl.HasValue() && logoUrl.StartsWith("http") ? logoUrl : "https://icon.foundation/prep/logo256.png";
        }
    }
}