using System;
using System.Collections.Generic;
using System.Reflection;

namespace AmBlitz.Configuration
{
    public class AmBlitzConfiguration
    {
        //数据库实体映射
        internal List<Assembly> DomainAssembly { get; set; }
        //配置数据库连接
        internal List<DataBaseConfiguration> DataBaseConfigurations { get; set; }
        /// <summary>
        /// redids 配置信息
        /// </summary>
        internal string RedisConnect { get; set; }
        /// <summary>
        /// 随机数获取
        /// </summary>
        internal Random Rd = new Random();

        internal AmBlitzConfiguration()
        {
            DataBaseConfigurations = new List<DataBaseConfiguration>();
            DomainAssembly = new List<Assembly>();
        }
        /// <summary>
        /// 是否使用Redis
        /// </summary>
        public bool EnableRedis { get; internal set; }

        /// <summary>
        /// 根据数据库名称 获取数据库连接
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns>数据库连接字符串</returns>
        public string MasterDataBaseConnectionString(string databaseName)
        {
            var dataBaseConfiguration = DataBaseConfigurations.Find(m => m.DataBaseName == databaseName);
            if (dataBaseConfiguration == null)
            {
                throw new ArgumentException($"数据库：{databaseName} 主库连接未配置!");
            }
            return dataBaseConfiguration.MasterDataBaseConnectionString;
        }

        /// <summary>
        /// 根据数据库名称 获取从库数据库连接
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns>数据库连接字符串</returns>
        public string SlaveDataBaseConnectionString(string databaseName)
        {
            var dataBaseConfiguration = DataBaseConfigurations.Find(m => m.DataBaseName == databaseName);
            if (dataBaseConfiguration == null)
            {
                throw new ArgumentException($"数据库：{databaseName} 从库连接未配置!");
            }

            //随机一个从库连接
            var index = Rd.Next(0, dataBaseConfiguration.SlaveDataBaseConnectionStrings.Count-1);
            return dataBaseConfiguration.SlaveDataBaseConnectionStrings[index] ;
        }

        /// <summary>
        ///  是否启用从库（读从库）
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns></returns>
        public bool EnableSecondaryDb(string databaseName)
        {
            var dataBaseConfiguration = DataBaseConfigurations.Find(m => m.DataBaseName == databaseName);
            if (dataBaseConfiguration == null)
            {
                throw new ArgumentException($"数据库：{databaseName} 未配置！查找不到数据库信息");
            }
            return dataBaseConfiguration.EnableSecondaryDb;
        }

        /// <summary>
        /// 是否启用删除
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public bool EnableSoftDelete(string databaseName)
        {
            var dataBaseConfiguration = DataBaseConfigurations.Find(m => m.DataBaseName == databaseName);
            if (dataBaseConfiguration == null)
            {
                throw new ArgumentException($"数据库：{databaseName} 未配置！查找不到数据库信息");
            }
            return !dataBaseConfiguration.DisableSoftDelete;
        }


        /// <summary>
        /// redis 配置信息
        /// </summary>
        /// <returns></returns>
        public string RedisConnectionString()
        {
            return RedisConnect;
        }
    }
}
