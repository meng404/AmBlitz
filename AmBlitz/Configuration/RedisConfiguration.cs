namespace AmBlitz.Configuration
{

    /// <summary>
    /// redis配置信息
    /// </summary>
    public class RedisConfiguration
    {
        public int Port { get; set; }
        public int DataBaseIndex { get; set; }
        public string Host { get; set; }
    }
}
