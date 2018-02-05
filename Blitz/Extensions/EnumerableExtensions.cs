using System.Collections.Generic;
using System.Linq;

namespace Blitz.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool HasElement<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Any();
        }
    }
}
