using System;

namespace AmBlitzCore.Module
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependAttribute : Attribute
    {
        public Type[] DependedModuleTypes { get; }

        public DependAttribute(params Type[] dependedModuleTypes)
        {
            DependedModuleTypes = dependedModuleTypes;
        }
    }
}
