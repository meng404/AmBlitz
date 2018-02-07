using System;

namespace AmBlitz.Domain
{
    public class EntityDescribe
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 启用主从 并且属性值为true读从库
        /// </summary>
        public bool ReadSecondary { get; set; }
        /// <summary>
        /// 是否支持软删除对象
        /// </summary>
        public bool SoftDeleteEntity { get; set; }

        /// <summary>
        /// 业务主键描述信息
        /// </summary>
        public BusinessPrimaryKeyAttribute BusinessPrimaryKeyAttribute { get; set; }

    }
}
