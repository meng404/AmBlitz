using System.Collections.Generic;

namespace AmBlitz.Configuration
{
    /// <summary>
    /// 数据库配置信息
    /// </summary>
    internal class DataBaseConfiguration
    {
        public string DataBaseName { get; set; }

        public string MasterDataBaseConnectionString { get; set; }

        public List<string> SlaveDataBaseConnectionStrings { get; set; }

        public bool EnableSecondaryDb { get; set; }

        public bool DisableSoftDelete { get; set; }
    }
}
