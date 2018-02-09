using System.Collections.Generic;
using System.Linq;

namespace AmBlitz.Toolkit
{
    public static class EnumerableExtensions
    {
        public static bool HasElement<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Any();
        }
    }
}
