﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Blitz.Extensions
{
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// 创建Get委托
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Func<object, object> MakePropertyGetFunc(this PropertyInfo property)
        {
            if (!property.CanRead)
            {
                return null;
            }

            var instanceExpr = Expression.Parameter(typeof(object), "instance");
            var delaringType = property.DeclaringType;
            if (delaringType == null)
            {
                throw new ArgumentException("property.delaringType");
            }
            var typedInstanceExpr = Expression.Convert(instanceExpr, delaringType);
            var propertyExpr = Expression.Property(typedInstanceExpr, property);
            var objectExpr = Expression.Convert(propertyExpr, typeof(object));
            var lambdaExpr = Expression.Lambda<Func<object, object>>(objectExpr, instanceExpr);

            var getFunc = lambdaExpr.Compile();
            return getFunc;
        }

        /// <summary>
        /// 创建Get委托
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Action<object, object> MakePropertySetFunc(this PropertyInfo property)
        {
            if (!property.CanWrite)
            {
                return null;
            }

            var method = property.GetSetMethod();
            var instanceExpr = Expression.Parameter(typeof(object), "instance");
            var valueExpr = Expression.Parameter(typeof(object), "value");
            var delaringType = property.DeclaringType;
            if (delaringType == null)
            {
                throw new ArgumentException("property.delaringType");
            }
            var typedInstanceExpr = Expression.Convert(instanceExpr, delaringType);
            var typedValueExpr = Expression.Convert(valueExpr, method.GetParameters()[0].ParameterType);
            var setPropertyExpr = Expression.Call(typedInstanceExpr, method, typedValueExpr);
            var lambdaExpr = Expression.Lambda<Action<object, object>>(setPropertyExpr, instanceExpr, valueExpr);

            var setFunc = lambdaExpr.Compile();
            return setFunc;
        }
    }
}
