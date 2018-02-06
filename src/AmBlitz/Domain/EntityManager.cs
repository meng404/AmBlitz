using Autofac;
using AmBlitz.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AmBlitz.Domain
{
    public class EntityManager
    {
        private readonly Dictionary<Type, EntityDescribe> _moduelDescribes = new Dictionary<Type, EntityDescribe>();

        private EntityManager()
        {

        }

        public static EntityManager Bulid(ContainerBuilder containerBuilder, params Assembly[] domainAssemblies)
        {
            var entityManager = new EntityManager();
            entityManager.Scan(domainAssemblies);
            //自我注册
            containerBuilder.RegisterInstance(entityManager).SingleInstance();

            return entityManager;
        }
        /// <summary>
        /// 扫描程序集 实体对象
        /// </summary>
        /// <param name="assemblies"></param>
        private void Scan(Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetExportedTypes().Where(m => m.GetCustomAttribute<EntityAttribute>() != null);
                foreach (var type in types)
                {
                    if (_moduelDescribes.ContainsKey(type))
                    {
                        continue;
                    }
                    
                    var entityAttribute = type.GetCustomAttribute<EntityAttribute>();

                    var entityDescribe = new EntityDescribe
                    {
                        DbName = entityAttribute.DbName,
                        TableName = entityAttribute.TableName,
                        ReadSecondary = entityAttribute.ReadSecondary
                    };


                    //处理包含业务主键的类型实体
                    var prop = type.GetProperties().Where(m => m.HasAttribute<BusinessPrimaryKeyAttribute>()).FirstOrDefault();
                    if (prop !=null)
                    {
                        entityDescribe.BusinessPrimaryKeyAttribute = prop.GetCustomAttribute<BusinessPrimaryKeyAttribute>();
                        entityDescribe.BusinessPrimaryKeyAttribute.BusinessPrimaryKeyType = prop.PropertyType;
                        entityDescribe.BusinessPrimaryKeyAttribute.KeyDescriptor = new PropertyDescriptor
                        {
                            _setValue = prop.MakePropertySetFunc(),
                            _getValue = prop.MakePropertyGetFunc()
                        };
                    }
                  
                    //业务主键属性
                    _moduelDescribes.Add(type, entityDescribe);
                }

            }
        }

        public  EntityDescribe GetEntityDescribe(Type type)
        {
            if (_moduelDescribes.ContainsKey(type))
            {
                return _moduelDescribes[type];
            }
            throw new ArgumentException($"非法的实体对象，请检查类型:{type.FullName}是否已经注册");
        }
    }
}
