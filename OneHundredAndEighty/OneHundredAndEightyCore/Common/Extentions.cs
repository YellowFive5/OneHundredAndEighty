#region Usings

using System.Collections.Generic;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public static class Extentions
    {
        public static void AddIfNotContains<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

        public static void AddIfNotNull<T>(this List<T> list, T item)
        {
            if (item != null)
            {
                list.Add(item);
            }
        }
    }
}