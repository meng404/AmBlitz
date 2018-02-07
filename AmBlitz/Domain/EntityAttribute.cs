using System;

namespace AmBlitz.Domain
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute : Attribute
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        internal string DbName { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        internal string TableName { get; set; }

        /// <summary>
        /// 启用主从 并且属性值为true读从库
        /// </summary>
        internal bool ReadSecondary { get; set; }

        public EntityAttribute(string dataBaseName,string tableName,bool readSecondary = true)
        {
            DbName = dataBaseName;
            TableName = tableName;
            ReadSecondary = readSecondary;
        }
    }
}
