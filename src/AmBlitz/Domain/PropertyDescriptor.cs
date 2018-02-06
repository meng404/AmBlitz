using System;

namespace AmBlitz.Domain
{
    public class PropertyDescriptor
    {
        /// <summary>
        /// 获取方法
        /// </summary>
        internal Func<object, object> _getValue;

        /// <summary>
        /// 设置方法
        /// </summary>
        internal Action<object, object> _setValue;

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public object GetValue(object instance)
        {
            return _getValue(instance);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public void SetValue(object instance, object value)
        {
            _setValue(instance, value);
        }
    }
}
