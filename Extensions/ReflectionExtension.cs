using System;
using System.Reflection;

namespace AmBlitz.Extensions
{
    public static class ReflectionExtension
    {
        /// <summary>
        /// 是否声明指定的Attribute
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool HasAttribute<TAttribute>(this MemberInfo property) where TAttribute : Attribute
        {
            var attribute = property.GetCustomAttribute<TAttribute>();
            return attribute != null;
        }
    }
}
