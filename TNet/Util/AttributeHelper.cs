using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace TNet.Util
{
    /// <summary>
    /// 特性辅助类
    /// </summary>
    public class AttributeHelper
    {

        public static string DisplayAttributeFor<TModel, TValue>(Expression<Func<TModel, TValue>> expression) {
           return GetPropertyOnAttributeOfModelProperty<TModel,TValue,DisplayAttribute,string>(expression,"Name");
        }

        /// <summary>
        /// 获取枚举的Display的Name值
        /// </summary>
        /// <param name="val">枚举值</param>
        /// <returns></returns>
        public static string GetDisplayName<T>(T val)
        {
            Type type = val.GetType();
            FieldInfo fd = type.GetField(val.ToString());
            if (fd == null)
                return string.Empty;
            Attribute attr = fd.GetCustomAttribute(typeof(DisplayAttribute), false);
            return ((DisplayAttribute)attr).Name;
        }

        private static TAttributeProperty GetPropertyOnAttributeOfModelProperty<TModel, TValue,TAttribute,TAttributeProperty>(Expression<Func<TModel, TValue>> expression,string propertyNameOfAttribute) where TAttribute : Attribute
        {
            var memberExpression = GetMemberInfo(expression);
            TAttribute tat = memberExpression.Member.GetCustomAttribute<TAttribute>();
            Type attrType = typeof(TAttribute);
            TAttributeProperty attributePropertyValue=(TAttributeProperty) attrType.GetProperty(propertyNameOfAttribute).GetValue(tat);
            
            return attributePropertyValue;
        }

        private static MemberExpression GetMemberInfo(Expression method)
        {
            LambdaExpression lambda = method as LambdaExpression;
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }
    }
}