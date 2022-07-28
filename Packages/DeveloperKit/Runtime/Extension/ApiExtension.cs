using UnityDeveloperKit.Runtime.Api;
using UnityEngine;

namespace UnityDeveloperKit.Runtime.Extension
{
    public static class ApiExtension
    {
        public static bool IsNull(this IObject self)
        {
            if (self is UnityEngine.Object obj)
            {
                return !obj;
            }

            return object.ReferenceEquals(self, null);
        }

    }

    public static class UnityExtension
    {
        public static bool IsNull(this UnityEngine.Object self)
        {
            return !self;
        }
    }
}