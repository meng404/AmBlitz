using System;
using AutoMapper;

namespace AmBlitzCore.AutoMapper
{
    public class AutoMapFromAttribute: AutoMapAttributeBase
    {
        public AutoMapFromAttribute(params Type[] targetTypes)
            : base(targetTypes)
        {

        }
        public override void CreateMap(IMapperConfigurationExpression configuration, Type type)
        {
            foreach (var targetType in TargetTypes)
            {
                configuration.CreateMap(targetType, type);
            }
        }
    }
}
