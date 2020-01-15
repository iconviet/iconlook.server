using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Iconlook.Service.Web
{
    public static class DecimalExtensions
    {
        public static MarkupString ToIcxString(this decimal instance)
        {
            var text = instance.ToString("N4");
            var part = text.Split('.');
            if (part.Length == 2)
            {
                var zero = "number-part";
                if (int.Parse(part[0], NumberStyles.Number) == 0 &&
                    int.Parse(part[1], NumberStyles.Number) == 0)
                {
                    zero = "fraction-part";
                }
                text = $"<span class=\"{zero}\">{part[0]}</span>" +
                       $"<span class=\"fraction-part\">.{part[1]}</span>";
            }
            return new MarkupString(text);
        }
    }
}