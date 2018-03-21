using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AmBlitzCore.AutoMapper
{
    public class MapperManager
    {
        public static void BulidMapper(List<Assembly> assemblies)
        {
            //Mapper.Initialize(options => {
            //    //属性自动挡。。。
            //    foreach (var assembly in assemblies)
            //    {
            //        var types = assembly.GetExportedTypes().Where(m =>
            //        m.IsDefined(typeof(AutoMapFromAttribute)) ||
            //        m.IsDefined(typeof(AutoMapToAttribute)));

            //        foreach (var type in types)
            //        {
            //            var mappers = type.GetCustomAttributes<AutoMapAttributeBase>();

            //            mappers.ToList().ForEach(m => m.CreateMap(options, type));
            //        }
            //    }
            //    //手动挡 。。。。
            //    options.AddProfiles(assemblies);
            //});
        }
    }
}
