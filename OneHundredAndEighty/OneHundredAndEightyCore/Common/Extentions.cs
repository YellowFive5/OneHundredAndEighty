using System.Collections.Generic;

namespace OneHundredAndEightyCore.Common
{
    public static class Extentions
    {
        public static void AddIfNotContains<T>(this List<T> lst, T item)
        {
            if (!lst.Contains(item))
            {
                lst.Add(item);
            }
        }

        public static void AddIfNotNull<T>(this List<T> lst, T item)
        {
            if (item != null)
            {
                lst.Add(item);
            }
        }
    }
}