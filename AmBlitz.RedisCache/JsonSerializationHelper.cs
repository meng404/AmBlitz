using Newtonsoft.Json;
using System;

namespace AmBlitz.RedisCache
{
    public static class JsonSerializationHelper
    {
        private const char TypeSeperator = '|';

        public static string SerializeWithType(object obj)
        {
            return SerializeWithType(obj, obj.GetType());
        }

        public static string SerializeWithType(object obj, Type type)
        {
            var serialized = JsonConvert.SerializeObject(obj);

            return string.Format(
                "{0}{1}{2}",
                type.AssemblyQualifiedName,
                TypeSeperator,
                serialized
                );
        }

        public static T DeserializeWithType<T>(string serializedObj)
        {
            return (T)DeserializeWithType(serializedObj);
        }

        public static object DeserializeWithType(string serializedObj)
        {
            var typeSeperatorIndex = serializedObj.IndexOf(TypeSeperator);
            var type = Type.GetType(serializedObj.Substring(0, typeSeperatorIndex));
            var serialized = serializedObj.Substring(typeSeperatorIndex + 1);
            return JsonConvert.DeserializeObject(serialized, type);
        }
    }
}
