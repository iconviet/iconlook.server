using System.Runtime.CompilerServices;
using Agiper.Object;
using ServiceStack;

namespace Iconlook
{
    public static class ObjectExtensions
    {
        public static bool IsValid(this IObject instance, [CallerMemberName] string method = null)
        {
            return instance != null && instance.IsValid(true, method);
        }

        public static T ConvertTo<T>(this object instance, [CallerMemberName] string method = null)
        {
#if DEBUG
            instance.CanStrictlyConvertTo<T>();
#endif
            return AutoMappingUtils.ConvertTo<T>(instance);
        }
    }
}